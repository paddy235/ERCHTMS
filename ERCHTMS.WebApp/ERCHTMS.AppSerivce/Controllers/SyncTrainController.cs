using ERCHTMS.Busines.BaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.SystemManage;
using System.Web;
using ERCHTMS.AppSerivce.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using ERCHTMS.Busines.SyncHiKDoor;
using ERCHTMS.Code;

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 区域
    /// </summary>
    public class SyncTrainController : ApiController
    {
        DepartmentBLL deptBll = new DepartmentBLL();



        /// <summary>
        ///调用海康人员接口所有人员的证件号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetAllIdCardsFormHik()
        {
            string logPath = HttpContext.Current.Server.MapPath("~/logs/menjin/");
            try
            {
                //设置推送的参数（由海康提供）
                HttpUtillib.SetPlatformInfo(Config.GetValue("appKey"), Config.GetValue("appSecret"), Config.GetValue("appIP"), int.Parse(Config.GetValue("appPort")), bool.Parse(Config.GetValue("isHttps")));
                //根据身份证号获取海康人员信息
                byte[] bytes = HttpUtillib.HttpPost("/artemis/api/resource/v2/person/personList", Newtonsoft.Json.JsonConvert.SerializeObject(new { pageSize = 1000, pageNo = 1 }), 300);
                string data = System.Text.Encoding.UTF8.GetString(bytes);//获取调用结果
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(data);
                //如果调用成功
                List<string> lstIdCards = new List<string>();
                if (dy.code == "0")
                {
                    StringBuilder sbSql = new StringBuilder("begin\r\n");
                    deptBll.ExecuteSql("delete from tmp_idcards");
                    List<object> lstUsers = dy.data.list;
                    long total = dy.data.total;
                    if (total <= 1000)
                    {
                        foreach (object obj in lstUsers)
                        {
                            dy = obj;
                            long certType = dy.certificateType;
                            if (certType == 111)
                            {
                                sbSql.AppendFormat("insert into tmp_idcards(idcard) values('{0}');\r\n", dy.certificateNo);
                                lstIdCards.Add(dy.certificateNo);
                            }
                        }
                    }
                    else
                    {
                        int page = 2;
                        //分页设置，因接口最多支持1000个查询条件
                        int totalCount = int.Parse(total.ToString());
                        if (totalCount % 1000 == 0)
                        {
                            page = totalCount / 1000;
                        }
                        else
                        {
                            page = totalCount / 1000 + 1;
                        }
                        for (int i = 2; i <= page; i++)
                        {
                            bytes = HttpUtillib.HttpPost("/artemis/api/resource/v2/person/personList", Newtonsoft.Json.JsonConvert.SerializeObject(new { pageSize = 1000, pageNo = i }), 300);
                            data = System.Text.Encoding.UTF8.GetString(bytes);//获取调用结果
                            dy = JsonConvert.DeserializeObject<ExpandoObject>(data);
                            if (dy.code == "0")
                            {
                                lstUsers = dy.data.list;
                                foreach (object obj in lstUsers)
                                {
                                    dy = obj;
                                    long certType = dy.certificateType;
                                    if (certType == 111)
                                    {
                                        sbSql.AppendFormat("insert into tmp_idcards(idcard) values('{0}');\r\n", dy.certificateNo);
                                        lstIdCards.Add(dy.certificateNo);
                                    }
                                }
                            }
                        }
                    }
                    sbSql.Append("commit;\r\n end;");
                    if (lstIdCards.Count > 0)
                    {
                        deptBll.ExecuteSql(sbSql.ToString());
                    }
                    System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：查询到的用户身份证件号：{lstIdCards.ToJson()}。SQL:{sbSql.ToString()}\n\n");
                }
                return new { code = 0, message = "操作成功", data = string.Join(",",lstIdCards) };
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：查询身份证号出现异常：{ex.Message}。\n\n");
                return new { code = 1, message = ex.Message };
            }
        }

        /// <summary>
        ///调用海康人员接口配置两边人员映射关系(通过身份证号关联海康人员personId)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object ConfigUsers()
        {
            string logPath = HttpContext.Current.Server.MapPath("~/logs/menjin/");
            try
            {
                //获取所有人员的身份证号
                DataTable dtUsers = deptBll.GetDataTable(string.Format("select identifyid from base_user"));
                string[] idcards = new string[] { };
                int page = 0;
                //分页设置，因接口最多支持1000个查询条件
                int totalCount = dtUsers.Rows.Count;
                if (totalCount % 1000 == 0)
                {
                    page = totalCount / 1000;
                }
                else
                {
                    page = totalCount / 1000 + 1;
                }
                System.IO.File.AppendAllText(logPath+"\\configusers.log",page.ToString() + "\r\n");
                int count = 0;
                for (int j = 0; j < page; j++)
                {
                    //根据分页获取身份证号
                    idcards = dtUsers.AsEnumerable().Select(t => t.Field<string>("identifyid")).Skip(j * 1000).Take(1000).ToArray();
                    //设置推送的参数（由海康提供）
                    HttpUtillib.SetPlatformInfo(Config.GetValue("appKey"), Config.GetValue("appSecret"), Config.GetValue("appIP"), int.Parse(Config.GetValue("appPort")), bool.Parse(Config.GetValue("isHttps")));
                    System.IO.File.AppendAllText(@"d:\logs\configusers.log", idcards+ "\r\n\n");
                    //根据身份证号获取海康人员信息
                    byte[] bytes = HttpUtillib.HttpPost("/artemis/api/resource/v1/person/condition/personInfo", Newtonsoft.Json.JsonConvert.SerializeObject(new { paramName = "certificateNo", paramValue = idcards }), 300);
                    string data = System.Text.Encoding.UTF8.GetString(bytes);//获取调用结果
                    dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(data);
                    StringBuilder sbSql = new StringBuilder();
                    //如果调用成功
                    if (dy.code == "0")
                    {
                        sbSql.Append("begin \r\n");
                        List<object> lstUsers = dy.data.list;
                        System.IO.File.AppendAllText(logPath + "\\configusers.log", lstUsers.Count+"\r\n\n");
                        foreach (object obj in lstUsers)
                        {
                            dy = obj;
                            long certType = dy.certificateType;
                            if (certType == 111)
                            {
                                string idCard = dy.certificateNo;
                                string personId = dy.personId;
                                string deptId = dy.orgIndexCode;
                                string personName = dy.personName;
                                DataTable dtUser = deptBll.GetDataTable(string.Format("select userid,account from base_user where identifyid='{0}'", idCard));

                                if (dtUser.Rows.Count > 0)
                                {
                                    DataRow drUser = dtUser.Rows[0];
                                    sbSql.AppendFormat("delete from HIK_USERRELATION where personId='{0}';\r\n", personId);
                                    sbSql.AppendFormat("insert into HIK_USERRELATION(userid,account,idcard,personid,orgindexcode) values('{0}','{1}','{2}','{3}','{4}');\r\n", drUser[0].ToString(), drUser[1].ToString(), idCard, personId, deptId);
                                    count++;
                                }
                            }
                        }
                        System.IO.File.AppendAllText(logPath + "\\configusers.log", "3\r\n\n");
                        sbSql.Append("commit;\r\n end;");
                        deptBll.ExecuteSql(sbSql.ToString());
                        System.IO.File.AppendAllText(logPath + "\\configusers.log", sbSql.ToString()+"\r\n\n");

                    }

                }
               string message = string.Format("成功匹配{0}条数据",count);
               return new { code =0, message = "操作成功",data= message };
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logPath + "\\configusers.log", ex.Message+ "\r\n\n");
                return new {code=1,message=ex.Message };
            }
        }

        /// <summary>
        /// 推送用户到海康门禁
        /// </summary>
        /// <param name="pfrom">数据来源,0:培训平台,1:工具箱</param>
        /// <returns></returns>
        [HttpPost]
        public object DeleteUsers()
        {
            string logPath = HttpContext.Current.Server.MapPath("~/logs/menjin/");
            try
            {
                string userIds = HttpContext.Current.Request["userIds"];//需要推送的用户账号
                string sql = string.Format("select personid from HIK_USERRELATION where userid in('{0}')", userIds.Replace(",","','"));
                DataTable dtUsers = deptBll.GetDataTable(sql);
                System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"删除的用户信息：{dtUsers.ToJson()}。\n\n");
                if (dtUsers.Rows.Count > 0)
                {
                    string[] personIds = dtUsers.AsEnumerable().Select(t => t.Field<string>("personid")).ToArray();
                    //设置推送的参数（由海康提供）
                    HttpUtillib.SetPlatformInfo(Config.GetValue("appKey"), Config.GetValue("appSecret"), Config.GetValue("appIP"), int.Parse(Config.GetValue("appPort")), bool.Parse(Config.GetValue("isHttps")));


                    System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"推送的数据：{personIds.ToJson()}。\n\n");

                    //开始推送数据
                    byte[] bytes = HttpUtillib.HttpPost("/artemis/api/resource/v1/person/batch/delete", Newtonsoft.Json.JsonConvert.SerializeObject(new { personIds=personIds }), 300);
                    string data = System.Text.Encoding.UTF8.GetString(bytes);//获取调用结果

                    System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"门禁返回的结果：{data}。\n\n");

                    dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(data);
                    StringBuilder sbMessge = new StringBuilder();
                    //如果调用成功
                    if (dy.code == "0")
                    {
                        //同步删除人员进入通道权限配置
                        List<object> lstData = new List<object>();
                        lstData.Add(new {
                            indexCodes= personIds,
                            personDataType= "person"
                        });
                        bytes = HttpUtillib.HttpPost("/artemis/api/acps/v1/auth_config/delete", Newtonsoft.Json.JsonConvert.SerializeObject(new { personDatas= lstData }), 300);
                        data = System.Text.Encoding.UTF8.GetString(bytes);//获取调用结果
                        dy = JsonConvert.DeserializeObject<ExpandoObject>(data);
                        if(dy.code == "0")
                        {
                            sbMessge.AppendFormat("批量删除人员门禁通道权限成功,返回信息：{0}。", dy.msg);
                            System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"调用结果：{ sbMessge.ToString()}。\n\n");
                            return new { code = 0, message = sbMessge.ToString() };
                        }
                        else
                        {
                            sbMessge.AppendFormat("批量删除人员门禁通道权限失败,返回信息：{0}。", data);
                            System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"调用结果：{ sbMessge.ToString()}。\n\n");
                            return new { code = 1, message = sbMessge.ToString() };
                        }
                    }
                    else
                    {
                        sbMessge.AppendFormat("批量删除人员失败,返回信息：{0}。", data);
                        return new { code = 1, message = sbMessge.ToString() };
                    }


                }
                else
                {
                    return new { code =1, message ="没有找到符合条件的记录"};
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"异常信息：{ ex.Message}。\n\n");
                return new { code = 1, message = ex.Message };
            }
        }
        /// <summary>
        /// 推送用户到海康门禁
        /// </summary>
        /// <param name="pfrom">数据来源,0:培训平台,1:工具箱</param>
        /// <returns></returns>
        [HttpPost]
        public object SyncUsers(int pfrom,string orgId="")
        {
            string logPath = HttpContext.Current.Server.MapPath("~/logs/menjin/");
            try
            {
                string accounts = HttpContext.Current.Request["accounts"];//需要推送的用户账号
                List<object> lstUsers = new List<object>();//用户存储要推送的用户集合
                StringBuilder sbSql = new StringBuilder(); //Sql语句
                byte[] bytes;
                Dictionary<long, string> dict = new Dictionary<long, string>();//临时存储培训平台推送的用户信息
                if (pfrom == 0) //培训平台
                {
                    string sql = string.Format("select userid, u.realname,u.gender,u.identifyid,u.email,u.mobile,u.birthday,u.encode,account from base_user u where u.account in ('{0}') or newaccount in ('{0}')", accounts.Replace(",", "','"));
                    DataTable dtUsers = deptBll.GetDataTable(sql);
                    int j = 0;
                    System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：查询到的用户信息：{dtUsers.ToJson()}。\n\n");
                    foreach (DataRow dr in dtUsers.Rows)
                    {
                        string account = dr["account"].ToString();//用户id
                        string userId = dr["userid"].ToString();//用户id
                        string userName = dr["realname"].ToString();//姓名
                        string sex = dr["gender"].ToString();//性别
                        string idCard = dr["identifyid"].ToString(); //身份证号
                        string email = dr["email"].ToString();//邮箱
                        string mobile = dr["mobile"].ToString();//手机号
                        string birthday = dr["birthday"].ToString();//生日
                        string encode = dr["encode"].ToString();//工号
                      
                        //转换生日格式
                        if(!string.IsNullOrWhiteSpace(birthday))
                        {
                            DateTime time;
                            bool flag= DateTime.TryParse(birthday,out time);
                            if(flag)
                            {
                                birthday = time.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                birthday = "";
                            }
                        }
                        //转换性别
                        int gender = 0;
                        int certType = 111;//表示身份证类型
                        if(sex.Length>0)
                        {
                            gender = sex == "男" ? 1 : 2;
                        }
                        //构造需要推送的用户参数
                        lstUsers.Add(new
                        {
                            clientId = j,
                            personId = userId,
                            personName = userName,
                            gender = gender,
                            certificateType = certType,
                            certificateNo = idCard,
                            orgIndexCode = "a10d1bf0-c9d5-4b0b-82c3-abd624a79f76",
                            jobNo=encode,
                            email= email,
                            phoneNo=mobile,
                            birthday= birthday
                        });
                        j++;
                        dict.Add(j, account);
                    }
                }
                if (pfrom==1)//工具箱
                {
                    //查询需要推送的用户
                    DataTable dtUsers = deptBll.GetDataTable(string.Format("select id,username,idcard from PX_TRAINRECORD t where account in('{0}')", accounts.Replace(",","','")));
                    int j = 0;
                    System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：查询到的用户信息：{dtUsers.ToJson()}。\n\n");
                    foreach (DataRow dr in dtUsers.Rows)
                    {
                        int gender = 0;//性别，0:未知,1:男,2:女
                        string id = dr["id"].ToString(); //主键ID
                        string userName = dr["username"].ToString().Trim();
                        string idCard = dr["idcard"].ToString().Trim(); //证件号
                        int len = idCard.Length;
                        int certType = 414; //护照类型
                        if (len == 15 || len == 18)
                        {
                            certType = 111; //身份证类型
                            string ch = idCard.Substring(len - 2,1);
                            int i ;
                            bool result = int.TryParse(ch, out i);
                            if(result)
                            {
                                if (i % 2 == 0)
                                {
                                    gender=2;
                                }
                                else
                                {
                                    gender=1;
                                }
                            }
                            
                        }
                        //构造需要推送的用户参数
                        lstUsers.Add(new {
                            clientId = j,
                            personId = id,
                            personName = userName,
                            gender = gender,
                            certificateType = certType,
                            certificateNo = idCard,
                            orgIndexCode = "a10d1bf0-c9d5-4b0b-82c3-abd624a79f76"
                        });
                        j++;
                    }
                }
                string usersData = Newtonsoft.Json.JsonConvert.SerializeObject(lstUsers);
                System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：Api地址：/artemis/api/resource/v1/person/batch/add，推送的数据：{usersData}。\n\n");
                //设置推送的参数（由海康提供）
                HttpUtillib.SetPlatformInfo(Config.GetValue("appKey"), Config.GetValue("appSecret"), Config.GetValue("appIP"), int.Parse(Config.GetValue("appPort")), bool.Parse(Config.GetValue("isHttps")));
                //开始推送数据
                bytes=HttpUtillib.HttpPost("/artemis/api/resource/v1/person/batch/add", usersData, 300);
                string data = System.Text.Encoding.UTF8.GetString(bytes);//获取调用结果
                System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：海康门禁服务返回的结果：{data}。\n\n");
                dynamic dy= JsonConvert.DeserializeObject<ExpandoObject>(data);
                //如果调用成功
                if(dy.code == "0")
                {
                    //获取海康门禁推送成功的记录
                    List<object> lstSuccess = dy.data.successes;
                    int count = lstSuccess.Count;
                    if (count > 0)
                    {
                        System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：调用海康门禁推送人员接口成功,成功记录数{count}\n\n");

                        List<string> lstUserIds = new List<string>();
                        foreach (object obj in lstSuccess)
                        {
                            dynamic dy1 = obj;
                            if(pfrom==1)
                            {
                                string personId = dy1.personId;
                                if (!lstUserIds.Contains(personId))
                                {
                                    lstUserIds.Add(personId);
                                }
                            }
                            if(pfrom==0)
                            {
                                long clientId = dy1.clientId;
                                if (!dict.ContainsKey(clientId))
                                {
                                    lstUserIds.Add(dict[clientId]);
                                }
                            }
                        }
                        //把推送成功的记录插入推送历史记录表
                        if (lstUserIds.Count > 0)
                        {
                            //根据推送结果查询成功的用户信息
                            sbSql.AppendFormat("insert into px_pushrecord(id,username,account,idcard,unitname,deptname,postname,worktype,score,datatype,deviceno,projectid,time) select id,username,account,idcard,unitname,deptname,postname,worktype,score,{1},deviceno,projectid,'{2}' from PX_TRAINRECORD where id in('{0}')", string.Join(",", lstUserIds).Replace(",", "','"), pfrom, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        if (pfrom == 0)
                        {
                            if(lstUserIds.Count>0)
                            {
                                System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：开始准备获取培训平台人员培训记录\n\n");
                                DepartmentEntity dept = deptBll.GetEntity(orgId);
                                WebClient wc = new WebClient();
                                wc.Credentials = CredentialCache.DefaultCredentials;
                                Dictionary<string, string> dictData = new Dictionary<string, string>();
                                dictData.Add("companyId", dept.InnerPhone);
                                dictData.Add("pageIndex", "1");
                                dictData.Add("pageSize", "1000");
                                dictData.Add("userAccounts", string.Join(",", lstUserIds).Replace(",", "','"));
                                dictData.Add("type", "1");
                                wc.Headers.Add("Content-Type", "application/json;charset=UTF-8");
                                wc.Encoding = Encoding.UTF8;
                                string apiUrl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("TrainApiAddress", "Train");
                                string dicData = Newtonsoft.Json.JsonConvert.SerializeObject(dictData);
                                System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"Api地址：{apiUrl}/api/api/trainRecord/queryTrainList，给培训平台推送的数据：{dicData}。\n\n");
                                string result = wc.UploadString(apiUrl+"/api/api/trainRecord/queryTrainList", "POST", dicData);

                                System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：培训平台返回的结果：{result}。\n\n");
                                dynamic dyData = JsonConvert.DeserializeObject<ExpandoObject>(result);
                                if (dyData.meta.success)
                                {
                                    sbSql.Append("begin  \r\n");
                                    List<object> lstTrainUsers = dyData.data.trainList;
                                    foreach (object obj in lstTrainUsers)
                                    {
                                        dyData = obj;
                                        string id = Guid.NewGuid().ToString();
                                        string userName = dyData.userName;
                                        string sex = dyData.sex == "1" ? "男" : "女";
                                        string account = dyData.userAccount;
                                        string idCard = dyData.idNumber;
                                        string unitName = dyData.companyName;
                                        string deptName = dyData.deptName;
                                        string postName = dyData.category;
                                        string workType = dyData.station;
                                        string score = dyData.examScore;
                                        string projectId = dyData.projectId;
                                        sbSql.AppendFormat("insert into PX_PUSHRECORD(id,username,account,sex,idcard,unitname,deptname,postname,worktype,score,projectid,time) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'); \r\n", Guid.NewGuid().ToString(), userName, account, sex, idCard, unitName, deptName, postName, workType, score, projectId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    }
                                    sbSql.Append("commit;\r\n end;");

                                    System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：执行的SQL语句：{sbSql.ToString()}。\n\n");

                                }
                            }
                            
                        }
                        //插入记录到推送记录表
                        if(sbSql.Length>0)
                        {
                            deptBll.ExecuteSql(sbSql.ToString());
                        }
                       
                    }
                    if(dy.data.failures.Count>0)
                    {
                        System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：海康门禁推送人员接口有失败的记录。\n\n");

                        return new { code = 1, message = "推送失败的数据："+Newtonsoft.Json.JsonConvert.SerializeObject(dy.data.failures), data = dy.data };
                    }
                    else
                    {
                        return new { code = 0, message = dy.msg, data = dy.data };
                    }
                }
                else
                {
                    return new { code = 1, message = dy.msg, data = data };
                }
            }
            catch(Exception ex)
            {
                System.IO.File.AppendAllText(logPath + $"{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}：异常信息：{ex.Message}。\n\n");
                return new { code = 1, message = ex.Message };
            } 
        }
        /// <summary>
        /// 接收工具箱消息队列中的培训记录和人员培训档案并同步到双控表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SyncTrainRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dyData = JsonConvert.DeserializeObject<ExpandoObject>(res);
                List<object> list = dyData.data;
                DepartmentBLL departmentBLL = new DepartmentBLL();
                StringBuilder sb = new StringBuilder("begin  \r\n");
                if (dyData.type == 3)//培训人员
                {
                    foreach (object obj in list)
                    {
                        dynamic dy = obj;
                        string idCard = dy.idCard;//证件号
                        bool isDelete = dy.isDelete;
                        string number = departmentBLL.GetDataTable(string.Format("select count(1) from PX_TRAINRECORD where idcard='{0}'", idCard)).Rows[0][0].ToString();
                        if (number != "0" || isDelete)
                        {
                            sb.AppendFormat("delete from PX_TRAINRECORD where idcard='{0}'; \r\n", idCard);
                        }
                        string userName = dy.userName;//姓名
                        string postName = dy.postName;//岗位
                        string workType = dy.workType;//工种
                        string unitName = dy.unitName;//单位名称
                        double hours = dy.hours;//培训学时
                        long passScore = dy.passScore;//合格分
                        double score = dy.score;//成绩
                        string deviceNo = dy.deviceNo;//设备编号
                        string unitId = dy.unitId;
                        string deptId = dy.deptId;
                        string projectId = dy.projectId;


                        string sql = string.Format("insert into PX_TRAINRECORD(id,USERNAME,IDCARD,UNITNAME,UNITID,DEPTID,POSTNAME,WORKTYPE,PROJECTID,SCORE,PASSSCORE,DEVICENO,account) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{2}'); \r\n", Guid.NewGuid().ToString(), userName, idCard, unitName, unitId, deptId, postName, workType, projectId, score, passScore, deviceNo);
                        sb.AppendFormat(sql);

                    }
                }
                if (dyData.type == 2) //培训项目
                {
                    foreach (object obj in list)
                    {
                        dynamic dy = obj;
                        string projectId = dy.projectId;//ID
                        bool isDelete = dy.isDelete;
                        string number = departmentBLL.GetDataTable(string.Format("select count(1) from PX_TRAINPROJECT where id='{0}'", projectId)).Rows[0][0].ToString();
                        if (number != "0" || isDelete)
                        {
                            sb.AppendFormat("delete from PX_TRAINPROJECT where id='{0}'; \r\n", projectId);
                        }
                        string projectName = dy.projectName;//培训项目名称
                        string startTime = dy.startTime;//培训开始时间
                        string endTime = dy.endTime;//培训结束时间
                        double hours = dy.hours;//培训学时
                        string trainType = dy.trainType;//培训类型

                        string sql = string.Format("insert into PX_TRAINPROJECT(id,PROJECTNAME,STARTTIME,ENDTIME,TRAINTYPE,HOURS) values('{0}','{1}','{2}','{3}','{4}','{5}'); \r\n", projectId, projectName, startTime, endTime, trainType, hours);
                        sb.AppendFormat(sql);
                    }
                }

                sb.Append("commit;\r\n end;");
                int count = departmentBLL.ExecuteSql(sb.ToString());
                return new { code = 0, message = "操作成功", data = res };
            }
            catch(Exception ex)
            {
                return new { code = 1, message =ex.Message};
            }

        }
    }
}