using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.OccupationalHealthManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PersonManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class PersonController : BaseApiController
    {
        ERCHTMS.Busines.BaseManage.UserBLL userBll = new ERCHTMS.Busines.BaseManage.UserBLL();
        ERCHTMS.Busines.BaseManage.DepartmentBLL deptBll = new ERCHTMS.Busines.BaseManage.DepartmentBLL();
        private TransferBLL transferbll = new TransferBLL();
        private DataItemCache dic = new DataItemCache();
        public HttpContext ctx { get { return HttpContext.Current; } }
        /// <summary>
        /// 14.人员列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetList([FromBody]JObject json)
        {
            try
            {
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;

                string username = dy.data.username;//姓名
                string deptid = dy.data.deptid;//部门
                string card = dy.data.idcard;//身份证
                long pageIndex = dy.data.pageIndex;
                long pageSize = dy.data.pageSize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.sidx = "createdate";
                pagination.sord = "desc";
                pagination.p_kid = "u.USERID";
                pagination.p_fields = "REALNAME,MOBILE,DEPTNAME,usertype,GENDER,identifyid,case when headicon is null then '' else ('" + path + "' || headicon) end faceurl,IsEpiboly,'' status,organizeid,DEPARTMENTCODE,nature,dutyname postname";
                pagination.p_tablename = "v_userinfo u";
                pagination.conditionJson = "Account!='System'";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), "ea93dc6b-83fc-4ac2-a1b7-56ef6909445c", "departmentcode", "organizecode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }

                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    userName = username,
                    idCard = card,
                    departmentId = deptid
                });

                var data = new UserBLL().GetPageList(pagination, queryJson);
                string softName = BSFramework.Util.Config.GetValue("SoftName").ToLower();
                //是否特定账户
                bool IsAppointAccount = false;
                string accounts = new DataItemDetailBLL().GetItemValue("SpecialAccount", "HjbBasice")?.ToLower();
                if (!string.IsNullOrEmpty(accounts))
                {
                    List<string> accountArray = accounts.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (accountArray.Contains(user.Account.ToLower()))
                        IsAppointAccount = true;
                }


                foreach (DataRow dr in data.Rows)
                {
                    if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                    {
                        DataTable dt = deptBll.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode", dr["DEPARTMENTCODE"], "部门", dr["organizeid"]));
                        if (dt.Rows.Count > 0)
                        {
                            string name = "";
                            foreach (DataRow dr1 in dt.Rows)
                            {
                                name += dr1["fullname"].ToString() + "/";
                            }
                            dr["deptname"] = name.TrimEnd('/');
                        }
                    }
                    if (dr["IsEpiboly"].ToString() == "是" && softName.StartsWith("xss"))
                    {
                        DataTable dtItems = deptBll.GetDataTable(string.Format("select remark from XSS_ENTERRECORD where idcard='{0}' order by time desc", dr["identifyid"].ToString()));
                        if (dtItems.Rows.Count > 0)
                        {
                            dr["status"] = dtItems.Rows[0][0].ToString() == "0" ? "在厂" : "";
                        }
                    }
                    if (softName.StartsWith("gdhjb"))
                    {
                        string sqlTemp = string.Format("select inout,case (select count(userid) from hjb_personset where moduletype=0 and userid=s.userid) when 0 then 0 else 1 end as SpecialUser from bis_hikinoutlog s where s.userid='{0}'  order by createdate desc", dr["userid"].ToString());
                        DataTable dtItems = deptBll.GetDataTable(sqlTemp);
                        string inoutFlag = dtItems.Rows.Count > 0 ? dtItems.Rows[0][0].ToString() : "1";
                        if (!IsAppointAccount)
                        {
                            if (dtItems.Rows.Count > 0 && dtItems.Rows[0][1].ToString() == "1" && dr["userid"].ToString() == user.UserId)
                                dr["status"] = inoutFlag == "0" ? "在厂" : "";
                            else if (dtItems.Rows.Count > 0 && dtItems.Rows[0][1].ToString() == "0")
                                dr["status"] = inoutFlag == "0" ? "在厂" : "";
                        }
                        else
                            dr["status"] = inoutFlag == "0" ? "在厂" : "";
                    }

                }
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 获取证件状态
        /// </summary>
        /// <param name="endDate"></param>
        /// <param name="applyDate"></param>
        /// <returns></returns>
        private int GetStatus(string id, DateTime? endDate, DateTime? applyDate)
        {
            DateTime warnDate = DateTime.Now.AddMonths(3);
            DateTime nowDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            if (endDate != null)
            {
                if (endDate < nowDate)
                {
                    return 2;
                }
                else
                {
                    if (endDate < warnDate && endDate >= nowDate)
                    {
                        if (applyDate != null)
                        {
                            string count = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTAUDIT where certid='{0}' and audittype='复审' and  auditdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss')", id, applyDate.Value.ToString("yyyy-MM-dd HH:mm:ss"))).Rows[0][0].ToString();
                            if (count == "0")
                            {
                                if (applyDate < nowDate)
                                {
                                    return 4;
                                }
                                else
                                {
                                    if (applyDate < warnDate && applyDate > nowDate)
                                    {
                                        return 3;
                                    }
                                }
                            }
                            else
                            {
                                return 0;
                            }

                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
            if (applyDate != null)
            {
                string count = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTAUDIT where certid='{0}' and audittype='复审' and  auditdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss')", id, applyDate.Value.ToString("yyyy-MM-dd HH:mm:ss"))).Rows[0][0].ToString();
                if (count == "0")
                {
                    if (applyDate < nowDate)
                    {
                        return 4;
                    }
                    else
                    {
                        if (applyDate < warnDate && applyDate > nowDate)
                        {
                            return 3;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }
        /// <summary>
        /// 扫码获取人员相关信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.uid;
                bool IsPermission = false;//是否有权限
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var user = userBll.GetUserInfoEntity(id);   //获取用户基本信息

                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                string path = itemBll.GetItemValue("imgUrl");
                var fileBll = new FileInfoBLL();
                var cert = new ERCHTMS.Busines.PersonManage.CertificateBLL().GetList(id).Select(t => new { t.CertType, t.CertName, t.CertNum, SendDate = t.SendDate.Value.ToString("yyyy-MM-dd"), EndDate = t.EndDate == null ? "" : t.EndDate.Value.ToString("yyyy-MM-dd"), ApplyDate = t.ApplyDate == null ? "" : t.ApplyDate.Value.ToString("yyyy-MM-dd"), t.Years, t.SendOrgan, Status = GetStatus(t.Id, t.EndDate, t.ApplyDate), FilePath = "", Images = fileBll.GetFileList(t.Id).Select(f => new { url = path + f.FilePath.Replace("~", "") }) }).ToList();//获取人员证书信息
                var wzInfo = new DesktopBLL().GetWZInfoByUserId(id);//违章信息
                var wzdjInfo = new DesktopBLL().GetWZInfoByUserId(id, 1);//违章登记信息 
                var health = new ERCHTMS.Busines.OccupationalHealthManage.OccupationalstaffdetailBLL().NewGetUserTable(id);//职业病信息
                //20190326 fwz 新增查询个人接触职业危害因素
                string Hazardfactor = "";
                //判断是否是本人\厂领导\EHS部与人力资源部的人
                if (userId == id)
                {
                    IsPermission = true;
                }
                //获取当前操作用户
                var Appuser = userBll.GetUserInfoEntity(userId);   //获取用户基本信息
                //EHS部与人力资源部配置在字典中 通过字典查找
                var Perdeptname = Appuser.DepartmentCode;
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                var data = dataItemDetailBLL.GetDataItemListByItemCode("'SelectDept'").ToList();
                if (data != null)
                {
                    foreach (var Peritem in data)
                    {
                        string value = Peritem.ItemValue;
                        string[] values = value.Split('|');
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i] == Perdeptname) //如果部门编码对应则是有权限的人
                            {
                                IsPermission = true;
                                break;
                            }
                        }
                    }
                }

                //如果是厂领导也有权限
                if (Appuser.RoleName.Contains("厂级部门用户") && Appuser.RoleName.Contains("公司领导"))
                {
                    IsPermission = true;
                }

                if (IsPermission)
                {
                    Hazardfactor = new HazardfactoruserBLL().GetUserHazardfactor(user.Account);
                }

                var work = new WorkRecordBLL().GetList(id).Select(t => new { EnterDate = t.EnterDate.ToString().Contains("0001") ? "" : t.EnterDate.ToString("yyyy-MM-dd"), LeaveTime = t.LeaveTime.ToString().Contains("0001") ? "" : t.LeaveTime.ToString("yyyy-MM-dd"), t.DeptName, t.PostName, t.OrganizeName, t.JobName }).ToList();//工作记录
                string deptname = new TransferBLL().GetDeptName(user.DepartmentId);
                string projectName = "";
                if (!string.IsNullOrEmpty(user.ProjectId))
                {
                    OutsouringengineerEntity entity = new OutsouringengineerBLL().GetEntity(user.ProjectId);
                    if (entity != null)
                    {
                        projectName = entity.ENGINEERNAME;
                    }

                }

                string photo = "";
                if (!string.IsNullOrEmpty(user.HeadIcon))
                {

                    if (!string.IsNullOrEmpty(path))
                    {
                        photo = path + user.HeadIcon;
                    }

                }
                int score = 100;
                var item = itemBll.GetEntity("csjf");
                if (item != null)
                {
                    score = int.Parse(item.ItemValue);
                }

                int point = 0;
                string argValue = new DataItemDetailBLL().GetItemValue("Point");
                if (!string.IsNullOrEmpty(argValue) && argValue == "point")
                {
                    dynamic pointdata = UpdatePoint(user.Account);
                    if (pointdata != null && pointdata.Qualitydata.Count > 0)
                    {
                        point = pointdata.Qualitydata[0].point;
                    }
                }


                //2019-03-08 Fwz修改 加入职务字段 部门显示改为层级显示
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = new { realname = user.RealName, point = point, sex = user.Gender, identifyid = user.IdentifyID, deptname = deptname, deptid = user.DepartmentId, deptcode = user.DepartmentCode, postname = user.DutyName, dutyname = user.PostName, organizename = user.OrganizeName, organizecode = user.OrganizeCode, organizeid = user.OrganizeId, native = user.Native, nation = user.Nation, isSpecial = user.IsSpecial, isSpecialEqu = user.IsSpecialEqu, isBlack = user.IsBlack, mobile = user.Mobile, usertype = user.UserType, isEpiboly = user.isEpiboly, enterDate = user.EnterTime == null ? "" : user.EnterTime.Value.ToString("yyyy-MM-dd"), leaveTime = user.DepartureTime == null ? "" : user.DepartureTime.Value.ToString("yyyy-MM-dd"), isPresence = user.isPresence, score = new UserScoreBLL().GetUserScore(user.UserId, DateTime.Now.Year.ToString()) + score, projectName = projectName, faceUrl = photo, CertInfo = cert, IllInfo = health, Hazardfactor = Hazardfactor, IsPermission = IsPermission, WorkInfo = work, lllegalInfo = wzInfo, djlllegalInfo = wzdjInfo, isfourperson = user.IsFourPerson, fourpersontype = user.FourPersonType } };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 同步培训平台人员素质意识值
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="fileName"></param>
        public dynamic UpdatePoint(string userAccount)
        {
            try
            {
                var trainserviceurl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("TrainServiceUrl");
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                //发送请求到web api并获取返回值，默认为post方式
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Business = "GetQualityObj",
                    userAccount = userAccount
                });
                nc.Add("json", queryJson);
                byte[] arr = wc.UploadValues(new Uri(trainserviceurl), nc);
                return JsonConvert.DeserializeObject<ExpandoObject>(System.Text.Encoding.Default.GetString(arr));
            }
            catch (Exception)
            {
                return null;
            }

        }
        [HttpPost]
        public object GetStayingUserStat([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                DepartmentBLL departmentBLL = new DepartmentBLL();
                List<string> sb = new List<string>();
                string sql = string.Format(@"select userid,depttype,u.departmentid from (select idcard from (select idcard,remark,row_number() over(partition by idcard order by time desc) as num from XSS_ENTERRECORD where idcard in(select idcard from XSS_ENTERRECORD where remark='0' and idcard in(select identifyid from base_user u left join base_department d on u.departmentid=d.departmentid where isepiboly='1' ))) c where c.num=1 and remark=0) t
left join base_user u on t.idcard=u.identifyid left join base_department d on u.departmentid=d.departmentid");
                DataTable dtUsers = departmentBLL.GetDataTable(sql);
                int totalPersonCount = dtUsers.Rows.Count;
                int cxPersonCount = dtUsers.Select("depttype='长协'").Length;
                int lsPersonCount = totalPersonCount - cxPersonCount;
                int totalUnitCount = dtUsers.AsEnumerable().GroupBy(t => t.Field<string>("departmentid")).Count();
                return new { Code = 0, data = new { totalpersoncount = totalPersonCount, totalunitcount = totalUnitCount, cxpersoncount = cxPersonCount, temppersoncount = lsPersonCount }, Info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 获取在厂外包人员清单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetStayingUserList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userName = res.Contains("username") ? dy.data.username : "";//姓名
                string deptType = res.Contains("depttype") ? dy.data.depttype : "";//外包单位类型：长协,临时
                string deptName = "";
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                DepartmentBLL departmentBLL = new DepartmentBLL();
                string where = " 1=1 ";
                List<string> sb = new List<string>();
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    where += string.Format(" and depttype='{0}'", deptType.Trim());
                }
                if (!string.IsNullOrWhiteSpace(deptName))
                {
                    where += string.Format(" and fullname like '%{0}%'", deptName.Trim());
                }
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    where += string.Format(" and username like '%{0}%'", userName.Trim());
                }
                string sql = string.Format(@"select * from (select idcard,remark,row_number() over(partition by idcard order by time desc) as num from XSS_ENTERRECORD where idcard in(select idcard from XSS_ENTERRECORD where remark='0' and idcard in(select identifyid from base_user u left join base_department d on u.departmentid=d.departmentid where isepiboly='1' and {0}))) c where c.num=1 and remark=0", where);

                DataTable dtIdCards = departmentBLL.GetDataTable(sql);
                sb = dtIdCards.AsEnumerable().Select(t => t.Field<string>("idcard")).ToList();
                List<object> list = new List<object>();
                if (sb.Count > 0)
                {
                    string idCards = string.Join(",", sb);
                    idCards = idCards.Replace(",", "','");
                    where = " 1=1 ";
                    if (!string.IsNullOrWhiteSpace(deptType))
                    {
                        where = "depttype='" + deptType + "'";
                    }
                    if (!string.IsNullOrWhiteSpace(deptName))
                    {
                        where += string.Format(" and fullname like '%{0}%'", deptName.Trim());
                    }
                    sql = string.Format("select d.departmentid,fullname,depttype from base_department d where {1} and  d.departmentid in(select u.departmentid from base_user u where u.isepiboly='1' and  u.identifyid in('{0}'))", idCards, where);
                    DataTable dtDepts = departmentBLL.GetDataTable(sql);
                    foreach (DataRow dr in dtDepts.Rows)
                    {
                        DataTable dtUsers = departmentBLL.GetDataTable(string.Format("select userid,a.realname,a.gender,a.identifyid,a.dutyname postname,entertime,case when headicon is null then '' else ('" + path + "' || headicon) end faceurl from base_user a  where departmentid='{0}' and  a.identifyid in('{1}')", dr[0].ToString(), idCards));
                        dtUsers.Columns.Add("way"); dtUsers.Columns.Add("times"); dtUsers.Columns.Add("hours");
                        foreach (DataRow dr1 in dtUsers.Rows)
                        {
                            string idCard = dr1["identifyid"].ToString();
                            DataTable dtItem = departmentBLL.GetDataTable(string.Format("select *from (select enrtyway,time,remark from XSS_ENTERRECORD where idcard='{0}' order by time desc) where rownum<3", idCard));
                            if (dtItem.Rows.Count > 0)
                            {
                                if (dtItem.Rows[0]["remark"].ToString() == "0")
                                {
                                    long ticks = DateTime.Now.Ticks - DateTime.Parse(dtItem.Rows[0]["time"].ToString()).Ticks;
                                    TimeSpan ts = new TimeSpan(ticks);
                                    dr1["way"] = dtItem.Rows[0]["enrtyway"].ToString() + "→";
                                    dr1["times"] = dtItem.Rows[0]["time"].ToString();
                                    dr1["hours"] = Math.Round(ts.TotalHours, 2);
                                    continue;
                                }
                                if (dtItem.Rows.Count == 1)
                                {
                                    if (dtItem.Rows[0]["remark"].ToString() == "0")
                                    {
                                        long ticks = DateTime.Now.Ticks - DateTime.Parse(dtItem.Rows[0]["time"].ToString()).Ticks;
                                        TimeSpan ts = new TimeSpan(ticks);
                                        dr1["way"] = dtItem.Rows[0]["enrtyway"].ToString() + "→";
                                        dr1["times"] = dtItem.Rows[0]["time"].ToString();
                                        dr1["hours"] = Math.Round(ts.TotalHours, 2);
                                        continue;
                                    }
                                    else
                                    {
                                        dr1["way"] = "→" + dtItem.Rows[0]["enrtyway"].ToString();
                                        dr1["times"] = "--" + dtItem.Rows[0]["time"].ToString();
                                        dr1["hours"] = "-";

                                    }
                                }

                                if (dtItem.Rows.Count > 1)
                                {
                                    if (dtItem.Rows[0]["remark"].ToString() == "1" && dtItem.Rows[1]["remark"].ToString() == "0")
                                    {
                                        long ticks = DateTime.Parse(dtItem.Rows[0]["time"].ToString()).Ticks - DateTime.Parse(dtItem.Rows[1]["time"].ToString()).Ticks;
                                        TimeSpan ts = new TimeSpan(ticks);
                                        dr1["way"] = dtItem.Rows[1]["enrtyway"].ToString() + "→" + dtItem.Rows[0]["enrtyway"].ToString();
                                        dr1["times"] = dtItem.Rows[1]["time"].ToString() + "--" + dtItem.Rows[0]["time"].ToString();
                                        dr1["hours"] = Math.Round(ts.TotalHours, 2);

                                    }
                                    if (dtItem.Rows[0]["remark"].ToString() == "1" && dtItem.Rows[1]["remark"].ToString() == "1")
                                    {

                                        dr1["way"] = "→" + dtItem.Rows[0]["enrtyway"].ToString();
                                        dr1["times"] = "--" + dtItem.Rows[0]["time"].ToString();
                                        dr1["hours"] = "-";

                                    }
                                }
                            }

                        }
                        list.Add(new
                        {
                            deptId = dr[0].ToString(),
                            deptName = dr[1].ToString(),
                            count = dtUsers.Rows.Count,
                            users = dtUsers
                        });
                    }

                }
                //按单位内在厂人数排序显示
                var newList = (from s in list orderby s.GetType().GetProperty("count").GetValue(s, null) descending select s).ToList();
                return new { Code = 0, Count = newList.Count, data = newList, Info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 人员门禁进出记录
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEnterRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string idCard = dy.data.idcard;//身份证号
                DepartmentBLL departmentBLL = new DepartmentBLL();
                List<string> ids = new List<string>();
                List<object> list = new List<object>();
                DataTable dt = departmentBLL.GetDataTable(string.Format("select * from XSS_ENTERRECORD where idcard='{0}' and time>to_date('{1}','yyyy-mm-dd hh24:mi:ss') order by time desc", idCard.Trim(), DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd 00:00:00")));
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j]["remark"].ToString() == "0")
                    {
                        if (j == 0)
                        {
                            ids.Add(dt.Rows[j]["id"].ToString());
                            long ticks = DateTime.Now.Ticks - DateTime.Parse(dt.Rows[j]["time"].ToString()).Ticks;
                            TimeSpan ts = new TimeSpan(ticks);
                            list.Add(new
                            {
                                entryWay = dt.Rows[j]["enrtyway"].ToString() + "→",
                                times = dt.Rows[j]["time"].ToString() + "--",
                                hours = Math.Round(ts.TotalHours, 2),
                                status = list.Count > 0 ? 0 : 1    //在厂
                            });

                            continue;
                        }
                        else
                        {
                            if (dt.Rows[j - 1]["remark"].ToString() == "0" && !ids.Contains(dt.Rows[j]["id"].ToString()))
                            {
                                ids.Add(dt.Rows[j]["id"].ToString());
                                list.Add(new
                                {
                                    entryWay = dt.Rows[j]["enrtyway"].ToString() + "→",
                                    times = dt.Rows[j]["time"].ToString() + "--",
                                    hours = "-",
                                    status = list.Count > 0 ? 0 : 1  //在厂
                                });

                            }
                            else
                            {
                                if (!ids.Contains(dt.Rows[j]["id"].ToString()))
                                {
                                    long ticks = DateTime.Parse(dt.Rows[j - 1]["time"].ToString()).Ticks - DateTime.Parse(dt.Rows[j]["time"].ToString()).Ticks;
                                    TimeSpan ts = new TimeSpan(ticks);
                                    ids.Add(dt.Rows[j]["id"].ToString());
                                    ids.Add(dt.Rows[j - 1]["id"].ToString());
                                    list.Add(new
                                    {
                                        entryWay = dt.Rows[j]["enrtyway"].ToString() + "→" + dt.Rows[j - 1]["enrtyway"].ToString(),
                                        times = dt.Rows[j]["time"].ToString() + "--" + dt.Rows[j - 1]["time"].ToString(),
                                        hours = Math.Round(ts.TotalHours, 2),
                                        status = list.Count > 0 ? 0 : 2   //离场
                                    });

                                }

                            }
                        }
                    }
                    else
                    {
                        if (j + 1 < dt.Rows.Count)
                        {
                            if (dt.Rows[j + 1]["remark"].ToString() == "0" && !ids.Contains(dt.Rows[j]["id"].ToString()))
                            {
                                ids.Add(dt.Rows[j]["id"].ToString());
                                ids.Add(dt.Rows[j + 1]["id"].ToString());
                                long ticks = DateTime.Parse(dt.Rows[j]["time"].ToString()).Ticks - DateTime.Parse(dt.Rows[j + 1]["time"].ToString()).Ticks;
                                TimeSpan ts = new TimeSpan(ticks);
                                list.Add(new
                                {
                                    entryWay = dt.Rows[j + 1]["enrtyway"].ToString() + "→" + dt.Rows[j]["enrtyway"].ToString(),
                                    times = dt.Rows[j + 1]["time"].ToString() + "--" + dt.Rows[j]["time"].ToString(),
                                    hours = Math.Round(ts.TotalHours, 2),
                                    status = list.Count > 0 ? 0 : 2   //离场
                                });

                            }
                            else
                            {
                                ids.Add(dt.Rows[j]["id"].ToString());
                                list.Add(new
                                {
                                    entryWay = "→" + dt.Rows[j]["enrtyway"].ToString(),
                                    times = "--" + dt.Rows[j]["time"].ToString(),
                                    hours = "-",
                                    status = list.Count > 0 ? 0 : 2   //离场
                                });

                            }
                        }
                        else
                        {
                            if (!ids.Contains(dt.Rows[j]["id"].ToString()))
                            {
                                ids.Add(dt.Rows[j]["id"].ToString());
                                list.Add(new
                                {
                                    entryWay = "→" + dt.Rows[j]["enrtyway"].ToString(),
                                    times = "--" + dt.Rows[j]["time"].ToString(),
                                    hours = "-",
                                    status = list.Count > 0 ? 0 : 2   //离场
                                });

                            }

                        }

                    }
                }

                return new { Code = 0, Count = list.Count, Info = "获取诗句成功", data = list };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }


        /// <summary>
        /// 人员门禁进出记录按单位汇总
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetSumList()
        {
            DepartmentBLL departmentBLL = new DepartmentBLL();
            //DataTable dtCards = departmentBLL.GetDataTable(string.Format("select distinct idcard from XSS_ENTERRECORD where remark='0' and idcard in(select identifyid from base_user where isepiboly='1')"));
            //List<string> sb = new List<string>();
            //foreach (DataRow dr in dtCards.Rows)
            //{
            //    DataTable dtRows = departmentBLL.GetDataTable(string.Format("select *from (select remark from XSS_ENTERRECORD where idcard='{0}' order by time desc) where rownum=1", dr[0].ToString()));
            //    if (dtRows.Rows.Count > 0)
            //    {
            //        if (dtRows.Rows[0][0].ToString() == "0")
            //        {
            //            if (!sb.Contains(dr[0].ToString()))
            //            {
            //                sb.Add(dr[0].ToString());
            //            }

            //        }
            //    }
            //}
            //List<object> list = new List<object>();
            int count = 0;
            DataTable dtCount = departmentBLL.GetDataTable(@"select distinct idcard from (select idcard,remark,time,row_number() over(partition by idcard order by idcard,time desc) as num from XSS_ENTERRECORD t  left join base_user p on t.idcard=p.identifyid where isepiboly='1')
where remark='0' and num=1");
            count = dtCount.Rows.Count;
            DataTable dtData = new DataTable();
            if (count > 0)
            {
                //string idCards = string.Join(",", sb);
                //idCards = idCards.Replace(",", "','");
                //DataTable dtDepts = departmentBLL.GetDataTable(string.Format("select d.departmentid,fullname from base_department d where d.departmentid in(select u.departmentid from base_user u where u.isepiboly='1' and  u.identifyid in('{0}'))", idCards));
                //foreach (DataRow dr in dtDepts.Rows)
                //{
                //    DataTable dtUsers = departmentBLL.GetDataTable(string.Format("select userid from base_user a  where departmentid='{0}' and  identifyid in('{1}')", dr[0].ToString(), idCards));
                //    count += dtUsers.Rows.Count;
                //    DataTable dtCount = departmentBLL.GetDataTable(string.Format("select count(1) from EPG_OUTSOURINGENGINEER t where t.ENGINEERTYPE='001' and OUTPROJECTID='{0}' and ENGINEERSTATE='002'", dr[0].ToString()));
                //    string deptType = dtCount.Rows[0][0].ToString();
                //    list.Add(new
                //    {
                //        deptId = dr[0].ToString(),
                //        deptName = dr[1].ToString(),
                //        deptType = deptType,
                //        count = dtUsers.Rows.Count,
                //    });
                //}
                string sql = string.Format(@"select d.departmentid deptid,fullname deptname,count(u.userid) count,'0' depttype from base_department d 
left join base_user u on d.departmentid=u.departmentid 
where  d.nature='承包商' and u.isepiboly='1' and u.identifyid in(select idcard from (select idcard,remark,time,row_number() over(partition by idcard order by idcard,time desc) as num from XSS_ENTERRECORD t left join base_user p on t.idcard=p.identifyid )
where remark='0' and num=1)
group by d.departmentid,fullname");
                dtData = departmentBLL.GetDataTable(sql);
                foreach (DataRow dr in dtData.Rows)
                {
                    string num = departmentBLL.GetDataTable(string.Format("select count(1) from EPG_OUTSOURINGENGINEER where ENGINEERSTATE='002' and ENGINEERTYPE='001' and OUTPROJECTID='{0}'", dr[0].ToString())).Rows[0][0].ToString();
                    dr["depttype"] = num;
                }
            }

            DataView dv = dtData.DefaultView;
            dv.Sort = "count desc";
            dtData = dv.ToTable();
            //按单位内在厂人数排序显示
            //var newList = (from s in list orderby s.GetType().GetProperty("count").GetValue(s, null) descending select s).ToList();
            return new { data = dtData, count = count };
        }

        [HttpGet]
        public object GetWBStat(string deptCode = "", int hours = 12)
        {

            List<int> list = new List<int>();
            List<string> x = new List<string>();
            int startHour = DateTime.Now.AddHours(0 - hours).Hour;

            DepartmentBLL departmentBLL = new DepartmentBLL();
            try
            {
                string sql = string.Format("select idcard from XSS_ENTERRECORD where idcard in(select identifyid from base_user where isepiboly='1')");
                if (!string.IsNullOrWhiteSpace(deptCode))
                {
                    sql = string.Format("select idcard from XSS_ENTERRECORD where  idcard in(select identifyid from base_user where isepiboly='1' and departmentcode='{0}')", deptCode);
                }
                //    DataTable dtCards = departmentBLL.GetDataTable(sql);
                //    List<string> sb = new List<string>();
                //    foreach (DataRow dr in dtCards.Rows)
                //    {
                //        DataTable dtRows = departmentBLL.GetDataTable(string.Format("select *from (select remark from XSS_ENTERRECORD where idcard='{0}' order by time desc) where rownum=1", dr[0].ToString()));
                //        if (dtRows.Rows.Count > 0)
                //        {
                //            if (dtRows.Rows[0][0].ToString() == "0")
                //            {
                //                if (!sb.Contains(dr[0].ToString()))
                //                {
                //                    sb.Add(dr[0].ToString());
                //                }

                //            }
                //        }
                //    }
                //    string idCards = string.Join(",", sb);
                //    idCards = idCards.Replace(",", "','");

                DataTable dtUsers = new DataTable();
                for (int j = 1; j <= hours; j++)
                {
                    x.Add(DateTime.Now.AddHours(j - hours).ToString("HH:00"));
                    dtUsers = departmentBLL.GetDataTable(string.Format(@"select distinct idcard from (select idcard,remark,time,row_number() over(partition by idcard order by idcard,time desc) as num from XSS_ENTERRECORD t where time<=to_date('{0}','yyyy-mm-dd hh24:mi:ss'))
where remark='0' and num=1 and time<=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and idcard in({1})", DateTime.Now.AddHours(j - hours).ToString("yyyy-MM-dd HH:00:00"), sql));
                    list.Add(dtUsers.Rows.Count);
                }
                x.Add(DateTime.Now.ToString("HH:mm"));
                dtUsers = departmentBLL.GetDataTable(string.Format(@"select distinct idcard from (select idcard,remark,time,row_number() over(partition by idcard order by idcard,time desc) as num from XSS_ENTERRECORD t  where time<=to_date('{0}','yyyy-mm-dd hh24:mi:ss'))
where remark='0' and num=1 and time<=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and idcard in({1})", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), sql));
                list.Add(dtUsers.Rows.Count);
                return new { x = x, y = list };
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        /// <summary>
        /// 加入黑名单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SetBlacklist([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string uId = dy.data.uid; //人员Id
                string reason = dy.data.reason;//加入黑名单原因
                OperatorProvider.AppUserId = userId;  //设置当前用户
                new BlacklistBLL().SaveForm("", new BlacklistEntity
                {
                    UserId = uId,
                    Reason = reason,
                    JoinTime = DateTime.Now
                });
                return new { Code = 0, Count = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 积分明细
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object IntegralInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string uId = dy.data.uid; //人员Id
                OperatorProvider.AppUserId = userId;  //设置当前用户
                UserEntity user = new UserBLL().GetEntity(uId);
                DataTable dt = new UserScoreBLL().GetList(uId);

                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity("csjf");
                decimal score = new UserScoreBLL().GetUserScore(uId, DateTime.Now.Year.ToString());
                score = entity == null ? score : int.Parse(entity.ItemValue) + score;
                return new { Code = 0, Count = 0, Info = "操作成功", data = new { score = score, Details = dt } };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///获取考核项目
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getItems([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100;
                pagination.p_kid = "id as itemid";
                pagination.p_fields = "ItemName,itemtype,score";
                pagination.p_tablename = "BIS_SCORESET";
                pagination.conditionJson = "isauto=0";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (user.IsSystem)
                {
                    pagination.conditionJson += " and deptcode='00'";
                }
                else
                {
                    pagination.conditionJson += string.Format(" and (deptcode='00' or deptcode='{0}')", user.OrganizeCode);
                }

                var data = new UserScoreBLL().GetPageJsonList(pagination, "");

                return new { Code = 0, Count = 0, Info = "操作成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///获取考核项目详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getItemInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.itemid;
                OperatorProvider.AppUserId = userId;  //设置当前用户

                var entity = new ScoreSetBLL().GetEntity(id);

                return new { Code = 0, Count = 0, Info = "操作成功", data = new { itemid = id, itemname = entity.ItemName, score = entity.Score, itemtype = entity.ItemType } };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///登记积分
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object addIntegral([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string uId = dy.data.uid; //人员Id
                string itemid = dy.data.itemid;
                long score = dy.data.score;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                ScoreSetEntity ss = new ScoreSetBLL().GetEntity(itemid);
                UserScoreEntity us = new UserScoreEntity
                {
                    Id = System.Guid.NewGuid().ToString(),
                    UserId = uId,
                    ItemId = itemid,
                    Score = ss.ItemType == "加分" ? decimal.Parse(score.ToString()) : decimal.Parse("-" + score.ToString()),
                    Year = System.DateTime.Now.Year.ToString(),
                    CreateDate = System.DateTime.Now,
                    CreateUserId = userId,
                    CreateUserDeptCode = user.DeptCode,
                    CreateUserOrgCode = user.OrganizeCode
                };
                new UserScoreBLL().SaveForm("", us);

                return new { Code = 0, Count = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 上传用户签名图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object uploadSign()
        {
            try
            {
                var dd = new DataItemDetailBLL();
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                long mode = 0;
                if (res.Contains("mode"))
                {
                    mode = dy.data.mode;
                }
                HttpFileCollection files = ctx.Request.Files;//签名图片 
                string userId = dy.userid; //新增类型

                string path = dd.GetItemValue("imgPath") + "\\Resource\\sign";
                string imgurl = dd.GetItemValue("imgUrl");
                if (mode == 10)
                {
                    path = dd.GetItemValue("imgPath") + "\\Resource\\PhotoFile";
                }
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                string ext = System.IO.Path.GetExtension(files[0].FileName);
                string fileName = Guid.NewGuid().ToString() + ".png";
                files[0].SaveAs(path + "\\" + fileName);
                string url = imgurl + "/Resource/sign/" + fileName;
                if (mode == 0)
                {
                    userBll.UploadSignImg(userId, "/Resource/sign/" + fileName);
                }
                if (mode == 10)
                {
                    url = imgurl + "/Resource/PhotoFile/" + fileName;
                    userBll.UploadSignImg(userId, "/Resource/PhotoFile/" + fileName, 1);
                }
                string bzAppUrl = new DataItemDetailBLL().GetItemValue("bzAppUrl");
                if (!string.IsNullOrEmpty(bzAppUrl))
                {
                    UpdateSign(userId, fileName, path, imgurl, bzAppUrl, int.Parse(mode.ToString()));
                }

                return new { Code = 0, Count = 0, Info = "操作成功", data = new { signUrl = url } };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };

            }
        }
        /// <summary>
        /// 获取用户工作状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkStatus([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                DataTable dt = new DepartmentBLL().GetDataTable(string.Format("select workstatus from base_user where userid='{0}'", userId));
                if (dt.Rows.Count > 0)
                {
                    string status = dt.Rows[0][0].ToString();
                    status = string.IsNullOrWhiteSpace(status) ? "01" : status;
                    return new { Code = 0, Count = 0, Info = "操作成功", data = new { status = status } };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "当前用户不存在" };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 设置用户工作状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SetWorkStatus([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string status = dy.data.status;
                int count = new DepartmentBLL().ExecuteSql(string.Format("updata base_user set workstatus='{0}' where userid='{1}'", status, userId));
                if (count > 0)
                {

                    return new { Code = 0, Count = 0, Info = "操作成功" };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "操作失败" };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 班组同步签名信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="fileName"></param>
        public void UpdateSign(string userid, string fileName, string path, string imgurl, string bzAppUrl, int mode = 0)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                System.IO.File.AppendAllText(path + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步成功" + "\r\n");
                string url = mode == 0 ? imgurl + "/Resource/sign/" + fileName : imgurl + "/Resource/PhotoFile/" + fileName;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    userid = userid,
                    filepath = url,
                    mode = mode
                });
                nc.Add("json", json);
                wc.UploadValuesAsync(new Uri(bzAppUrl + "UpdateUrl"), nc);

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(new DataItemDetailBLL().GetItemValue("imgPath") + "~/logs/" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：数据失败" + ",异常信息：" + ex.Message + "\r\n");
            }

        }

        /// <summary>
        /// 人员离职
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object leaveJob()
        {
            try
            {
                var dd = new DataItemDetailBLL();
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //用户Id
                string time = dy.data.time;//离职时间
                string Reson = dy.data.reson;//离职原因
                int count = userBll.SetLeave(userId, time, Reson);
                //离职后生成工作记录
                new WorkRecordBLL().EditRecord(userId, time);
                string msg = count > 0 ? "操作成功" : "操作失败,请确认该人员是否在系统中存在！";
                return new { Code = 0, Info = msg };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message };

            }
        }
        /// <summary>
        /// 人员调岗
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object changeJob()
        {
            try
            {
                var dd = new DataItemDetailBLL();
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //用户Id
                string postId = dy.data.postId;//新的岗位Id
                string postName = dy.data.postName;//新的岗位名称
                string dutyId = dy.data.dutyId;//新的职务Id
                string dutyName = dy.data.dutyName;//新的职务名称
                string time = dy.data.time;//转岗时间
                string deptId = dy.data.deptId;//新的部门
                int count = userBll.LeavePost(userId, deptId, postId, postName, dutyId, dutyName, time);
                string msg = count > 0 ? "操作成功" : "操作失败,请确认该人员是否在系统中存在！";
                return new { Code = 0, Info = msg };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message };

            }
        }

        /// <summary>
        /// 人员调岗
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Transfer()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string data = JsonConvert.SerializeObject(dy.data);
                TransferEntity entity = JsonConvert.DeserializeObject<TransferEntity>(data);
                string uid = entity.UserId;//获取到需要转岗的用户id
                //是否是新增 0是新增 1是修改
                string isNew = dy.isNew;
                UserEntity ue = userBll.GetEntity(uid);
                entity.InDeptId = ue.DepartmentId;
                entity.InDeptName = deptBll.GetEntity(ue.DepartmentId).FullName;
                entity.InDeptCode = ue.DepartmentCode;
                entity.InJobId = ue.PostId;
                entity.InJobName = ue.PostName;
                entity.InPostId = ue.DutyId;
                entity.InPostName = ue.DutyName;
                transferbll.AppSaveForm(isNew, entity, dy.userId);
                return new { Code = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message };

            }
        }

        [HttpGet]
        public object GetTrainResult(string IdCard)
        {
            try
            {
                SstmsService.CheckHeader header = new SstmsService.CheckHeader();
                header.Key = "C8E52648921869CE5BF5C80569046C11";
                // 实例化服务对象
                SstmsService.DataServiceSoapClient service = new SstmsService.DataServiceSoapClient();
                SstmsService.Depart[] data = service.GetDeparts(header);

                //string json = "{\"Examdata\":[{\"PARTYID\":\"1785106885\",\"USERNAME\":\"冯亮\",\"DEPARTID\":\"-1014595119\",\"USERACCOUNT\":\"20018576\",\"DEPARTNAME\":\"人事\",\"EXAMNAME\":\"2016年非生产人员安全培训_考试\",\"EXAMSTARTTIME\":\"2016-05-31T00:00:00\",\"EXAMENDTIME\":\"2016-07-30T23:59:00\",\"TRAINPLANCODE\":\"1766144250\",\"POINT\":67.0,\"STATE\":\"合格\",\"CREATEDATE\":\"2016-06-16T10:30:09\",\"PARENTID\":\"1781654785\",\"TIMES\":null,\"PASSLINE\":\"60\",\"EXAMID\":\"-1065522118\",\"CREATENAME\":\"沙角B管理员\"},{\"PARTYID\":\"1785106885\",\"USERNAME\":\"冯亮\",\"DEPARTID\":\"-1014595119\",\"USERACCOUNT\":\"20018576\",\"DEPARTNAME\":\"人事\",\"EXAMNAME\":\"2017年非生产人员安全培训考试补考2\",\"EXAMSTARTTIME\":\"2017-08-10T15:45:00\",\"EXAMENDTIME\":\"2017-09-01T15:43:00\",\"TRAINPLANCODE\":\"1766144250\",\"POINT\":null,\"STATE\":null,\"CREATEDATE\":null,\"PARENTID\":\"1781654785\",\"TIMES\":null,\"PASSLINE\":\"60\",\"EXAMID\":\"24380e16-bd6e-4a0e-8c1c-7cc97d8fe721\",\"CREATENAME\":\"沙角B管理员\"}]}";
                //var dtExams = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json.Replace("{\"Examdata\":", "").TrimEnd('}'));
                //if (string.IsNullOrEmpty(IdCard))
                //{
                //    return new { code = 0, result = 3, message = "身份证号不允许为空" };
                //}
                //if(!BSFramework.Util.ValidateUtil.IsIdCard(IdCard))
                //{
                //    return new { code = 0, result = 3, message = "身份证格式不正确" };
                //}
                //var user = userBll.GetUserByIdCard(IdCard);
                //if(user==null)
                //{
                //    return new { code = 0,result=4, message = "该人员信息不存在" };
                //}
                //bool flag=new AptitudeinvestigatepeopleBLL().IsAuditByUserId(user.UserId);
                //if (1!=1)
                //{
                //    return new { code = 0, result = 2, message = "资质未审核通过" };
                //}
                //else
                //{
                //    //json = userBll.GetExamRecord(user.Account, user.DepartmentId);
                //    if (json.Length > 30)
                //    {
                //        //dtExams = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json.Replace("{\"Examdata\":", "").TrimEnd('}'));
                //        var rows = dtExams.Select("point is not null");     
                //        if (rows.Count() > 0)
                //        {
                //            if (int.Parse(rows[0]["point"].ToString()) < int.Parse(rows[0]["passline"].ToString()))
                //            {
                //                return new { code = 0, result =2, message = "培训考试成绩不合格" };
                //            }
                //            else
                //            {
                //                return new { code = 0, result =1, message = "培训考试及资质通过" };
                //            }
                //        }
                //        else
                //        {
                //            return new { code = 0, result = 0, message = "未参加培训考试，没有查询到记录" };
                //        }
                //     }
                //    else
                //    {
                //        return new { code = 0, result = 0, message = "未参加培训考试，没有查询到记录" };
                //    }
                //}
                return new { Code = 0, Info = data };
            }
            catch (Exception ex)
            {
                return new { code = 0, result = 5, message = ex.Message };

            }
        }
        /// <summary>
        /// 同步工具箱部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object SyncToolsDepts()
        {
            try
            {
                DataTable dt = deptBll.GetDataTable(string.Format("select deptid,unitid,keys from BIS_TOOLSDEPT"));
                IList<UserEntity> listUsers = new List<UserEntity>();
                foreach (DataRow dr in dt.Rows)
                {
                    DepartmentEntity dept = deptBll.GetEntity(dr[0].ToString());
                    if (dept != null)
                    {
                        SstmsService.CheckHeader header = new SstmsService.CheckHeader();
                        header.Key = dr[2].ToString();
                        List<SstmsService.DictionaryEntry> dic = new List<SstmsService.DictionaryEntry>();
                        SstmsService.DictionaryEntry de = new SstmsService.DictionaryEntry();
                        de.Key = "LaterID";
                        de.Value = dr[1].ToString();
                        dic.Add(de);
                        de = new SstmsService.DictionaryEntry();
                        de.Key = "Identify";
                        de.Value = "";
                        dic.Add(de);

                        // 实例化服务对象
                        SstmsService.DataServiceSoapClient service = new SstmsService.DataServiceSoapClient();
                        SstmsService.PersonInfo[] data = service.GetPersons(header);
                        DepartmentEntity org = deptBll.GetEntity(dept.OrganizeId);
                        foreach (SstmsService.PersonInfo per in data)
                        {
                            string unitId = per.UnitID;
                            string userId = "";
                            string roleName = "承包商级用户";
                            string roleId = "c5530ccf-e84e-4df8-8b27-fd8954a9bbe9";
                            if (unitId == dr["unitid"].ToString())
                            {
                                string nature = dept.Nature;
                                switch (nature)
                                {
                                    case "部门":
                                        roleName = "部门级用户";
                                        roleId = "6c094cef-cca3-4b41-a71b-6ee5e6b89008";
                                        break;
                                    case "专业":
                                        roleName = "专业级用户";
                                        roleId = "e3062d59-2484-4046-a420-478886d58656";
                                        break;
                                    case "班组":
                                        roleName = "班组级用户";
                                        roleId = "d9432a6e-5659-4f04-9c10-251654199714";
                                        break;
                                    case "厂级":
                                        roleName = "公司级用户";
                                        roleId = "aece6d68-ef8a-4eac-a746-e97f0067fab5";
                                        break;
                                    case "省级":
                                        roleName = "省级用户";
                                        roleId = "9a834c93-ff60-440e-845d-79b311eeacae";
                                        break;
                                }
                                roleName += ",普通用户";
                                roleId += ",2a878044-06e9-4fe4-89f0-ba7bd5a1bde6";
                                UserEntity user = userBll.GetUserByIdCard(per.IdentifyID);
                                if (user == null)
                                {
                                    //    userId = user.UserId;
                                    //}
                                    //else
                                    //{
                                    user = new UserEntity();
                                    user.UserId = Guid.NewGuid().ToString();
                                    user.Account = per.IdentifyID;
                                    userId = user.UserId;
                                    // }

                                    user.MSN = "1";
                                    user.UserType = "一般工作人员";
                                    user.EnCode = per.TraID;
                                    user.Degrees = user.DegreesID = per.Degrees;
                                    user.Birthday = per.BirthDay;
                                    user.RoleId = roleId;
                                    user.RoleName = roleName;
                                    user.IdentifyID = per.IdentifyID;
                                    user.Gender = per.Sex;
                                    user.Nation = per.Nation;
                                    user.Email = per.Email;
                                    user.EnterTime = per.EntranceDate;
                                    user.RealName = per.PersonName;
                                    user.Degrees = per.Degrees;
                                    user.Native = per.Native;
                                    user.DepartureTime = per.LeaveDate;
                                    user.Telephone = per.TelPhone;
                                    user.IsPresence = per.IsOut == "是" ? "0" : "1";
                                    user.Password = "123456";
                                    user.DepartmentId = dept.DepartmentId;
                                    user.DepartmentCode = dept.EnCode;
                                    user.OrganizeId = dept.OrganizeId;
                                    user.OrganizeCode = org.EnCode;
                                    user.Craft = per.Category;
                                    user.IsEpiboly = roleName.Contains("承包商") || roleName.Contains("分包商") ? "1" : "0";
                                    if (user.ModifyDate != null)
                                    {
                                        if (per.OperDate > user.ModifyDate)
                                        {
                                            userBll.SaveForm(userId, user);
                                        }
                                    }
                                    else
                                    {
                                        userBll.SaveForm(userId, user);
                                    }
                                    user.Password = "123456";
                                    listUsers.Add(user);
                                }
                            }
                        }
                    }

                }
                if (listUsers.Count > 0)
                {
                    if (!string.IsNullOrEmpty(new DataItemDetailBLL().GetItemValue("bzAppUrl")))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            ImportUser(listUsers);
                        });
                    }
                }
                Task.Factory.StartNew(() =>
                {
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/") + fileName, string.Format("{0}:同步工具箱人员信息成功\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                });
                return new { Code = 1, message = "同步工具箱人员信息成功" };
            }
            catch (Exception ex)
            {
                Task.Run(() =>
                {
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/") + fileName, string.Format("{0}:{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message));

                });
                return new { code = 0, message = ex.Message };
            }
        }
        private void ImportUser(IList<UserEntity> userList)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //当前操作用户账号
                nc.Add("account", "System");
                foreach (UserEntity item in userList)
                {
                    //用户信息
                    item.Gender = item.Gender == "男" ? "1" : "0";
                    if (item.RoleName.Contains("班组级用户"))
                    {
                        if (item.RoleName.Contains("负责人"))
                        {
                            item.RoleId = "a1b68f78-ec97-47e0-b433-2ec4a5368f72";
                            item.RoleName = "班组长";
                        }
                        else
                        {
                            item.RoleId = "e503d929-daa6-472d-bb03-42533a11f9c6";
                            item.RoleName = "班组成员";
                        }
                    }
                    if (item.RoleName.Contains("部门级用户"))
                    {
                        if (item.RoleName.Contains("负责人"))
                        {
                            item.RoleId = "1266af38-9c0a-4eca-a04a-9829bc2ee92d";
                            item.RoleName = "部门管理员";
                        }
                        else
                        {
                            item.RoleId = "3a4b56ac-6207-429d-ac07-28ab49dca4a6";
                            item.RoleName = "部门级用户";
                        }
                    }
                    if (item.RoleName.Contains("公司级用户"))
                    {
                        //if (user.RoleName.Contains("负责人"))
                        //{
                        item.RoleId = "97869267-e5eb-4f20-89bd-61e7202c4ecd";
                        item.RoleName = "厂级管理员";
                        // }

                    }
                    if (item.EnterTime == null)
                    {
                        item.EnterTime = DateTime.Now;
                    }
                }

                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(userList));
                wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "SaveUsers"), nc);

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步数据失败，同步信息" + Newtonsoft.Json.JsonConvert.SerializeObject(userList) + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            //将同步结果写入日志文件
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
        }
        /// <summary>
        /// 博安云后台注册用户调用接口
        /// </summary>
        /// <param name="json">用户信息（如：json:{userName:"",mobile:"",deptName:""}）</param>
        ///参数说明：userName：姓名，mobile：手机号(作为系统账号)，deptName：单位名称
        /// <returns></returns>
        [HttpPost]
        public object RegisterUser()
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            string json = HttpContext.Current.Request.Params["json"];
            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                    if (string.IsNullOrEmpty(dy.userName))
                    {
                        return new { code = 1, data = "", info = "参数：userName为空" };
                    }
                    if (string.IsNullOrEmpty(dy.mobile))
                    {
                        return new { code = 1, data = "", info = "参数：mobile为空" };
                    }
                    if (string.IsNullOrEmpty(dy.deptName))
                    {
                        return new { code = 1, data = "", info = "参数：deptName为空" };
                    }
                    //判断账号是否在系统中存在
                    bool result = userBll.ExistAccount(dy.mobile, "");
                    if (result)
                    {
                        string deptid = new DataItemDetailBLL().GetItemValue("dept", "TryDept");//所属单位ID，在后台配置（目前处理方案是在后台初始化好一个演示的单位，注册的用户直接挂在此单位即可）
                        string pathurl = new DataItemDetailBLL().GetItemValue("imgUrl"); //web平台对应的地址，放在编码配置也可后台直接后去，请根据实际情况处理
                        if (!string.IsNullOrEmpty(deptid))
                        {
                            var dept = deptBll.GetEntity(deptid);
                            UserEntity user = new UserEntity();
                            user.UserId = Guid.NewGuid().ToString();
                            user.Account = dy.mobile;
                            Random rnd = new Random();
                            string str = rnd.Next(10000, 99999).ToString();
                            user.RealName = dy.userName;
                            user.Password = str;
                            user.DepartmentId = dept.DepartmentId;
                            user.DepartmentCode = dept.EnCode;
                            user.OrganizeId = dept.OrganizeId;
                            user.OrganizeCode = dept.EnCode;
                            user.Mobile = dy.mobile;
                            user.IsEpiboly = "0";
                            user.RoleName = "公司级用户,公司管理员";
                            user.RoleId = "aece6d68-ef8a-4eac-a746-e97f0067fab5,5af22786-e2f2-4a3d-8da3-ecfb16b96f36";
                            user.AllowStartTime = DateTime.Now;
                            user.AllowEndTime = Convert.ToDateTime(user.AllowStartTime).AddDays(15);
                            user.Description = dy.deptName;
                            var tempdata = new
                            {
                                account = dy.mobile,//账号
                                password = str,
                                allowEndTime = Convert.ToDateTime(user.AllowEndTime).ToString("yyyy-MM-dd HH:mm:ss"),
                                path = pathurl
                            };
                            userBll.SaveOnlyForm(user.UserId, user);
                            //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,注册成功,参数为:" + json + "\r\n");
                            return new { code = 0, data = tempdata, info = "操作成功" };
                        }
                        else
                        {
                            return new { code = 1, data = "", info = "试用单位未配置" };
                        }
                    }
                    else
                    {
                        return new { code = 1, data = "", info = "手机号已存在!" };
                    }
                }
                else
                {
                    return new { code = 1, data = "", info = "json参数为空" };
                }
            }
            catch (Exception ex)
            {
                //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,注册失败,异常信息:" + ex.Message + "参数为:" + json + "\r\n");
                return new { code = 1, data = "", info = ex.Message };
            }
        }
        /// <summary>
        /// 华电西塞山与人脸系统对接向双控平台推送用户
        /// </summary>
        /// <param name="json">推送信息</param>
        /// <returns></returns>
        [HttpPost]
        public object PushUserInfo([FromBody]JObject json)
        {
            string fileName = "user_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string dir = HttpContext.Current.Server.MapPath("~/logs/xssmj");
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            string message = "操作成功";
            int code = 1;
            string orgId = "28803381-c588-4875-ad66-b2ba75e3d9cd";
            DataItemDetailBLL dd = new DataItemDetailBLL();
            orgId = dd.GetItemValue("dept", "TryDept");
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string enCode = res.Contains("enCode") ? dy.enCode : "";
                string userName = res.Contains("userName") ? dy.userName : "";
                string sex = res.Contains("sex") ? dy.sex : "";
                string unitName = res.Contains("unitName") ? dy.unitName : "";
                string tel = res.Contains("tel") ? dy.tel : "";
                string nation = res.Contains("nation") ? dy.nation : "";
                string birth = res.Contains("birth") ? dy.birth : "";
                string native = res.Contains("native") ? dy.native : "";
                string politic = res.Contains("politic") ? dy.politic : "";
                string idCard = res.Contains("idCard") ? dy.idCard : "";
                string education = res.Contains("education") ? dy.education : "";
                string photo = res.Contains("photo") ? dy.photo : "";
                string userId = "";
                if (string.IsNullOrWhiteSpace(enCode) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(unitName) || string.IsNullOrWhiteSpace(idCard) || string.IsNullOrWhiteSpace(birth) || string.IsNullOrWhiteSpace(sex))
                {
                    message = "推送失败：编号(enCode)、姓名(userName)、单位名称(unitName)、身份证号(idCard)、出生日期(birth)、性别(sex)都不能为空！";
                    System.IO.File.AppendAllText(dir + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ," + message + ",推送信息：" + json + "\r\n");
                    return new { code = 0, message = message };
                }
                if (!BSFramework.Util.ValidateUtil.IsIdCard(idCard))
                {
                    message = "推送失败：身份证号格式错误!";
                    System.IO.File.AppendAllText(dir + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ," + message + ",推送信息：" + json + "\r\n");
                    return new { code = 0, message = message };
                }
                if (!userBll.ExistAccount(enCode, ""))
                {
                    UserInfoEntity userInfo = userBll.GetUserInfoByAccount(enCode);
                    if (userInfo != null)
                    {
                        userId = userInfo.UserId;
                    }
                    //return new { code = 0, message = "账号已存在！" };
                }
                if (!userBll.ExistIdentifyID(idCard, ""))
                {
                    UserEntity userInfo = userBll.GetUserByIdCard(idCard);
                    if (userInfo != null)
                    {
                        userId = userInfo.UserId;
                    }
                    //return new { code = 0, message = "身份证号已存在,无法插入信息！" };
                }
                RoleBLL roleBll = new RoleBLL();
                UserEntity user = new UserEntity
                {
                    UserId = string.IsNullOrWhiteSpace(userId) ? "" : userId,
                    IsPresence = "1",
                    Account = idCard,
                    RealName = userName,
                    Gender = sex,
                    IdentifyID = idCard,
                    EnCode = enCode,
                    Birthday = DateTime.Parse(birth),
                    Nation = nation,
                    Native = native,
                    Telephone = tel,
                    Political = politic,
                    DegreesID = education
                };
                DepartmentEntity org = deptBll.GetEntity(orgId);
                if (org == null)
                {
                    message = "推送失败：单位信息不正确！";
                    System.IO.File.AppendAllText(dir + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ," + message + ",推送信息：" + json + "\r\n");
                    return new { code = 0, message = message };
                }
                user.OrganizeCode = org.EnCode;
                if (!string.IsNullOrEmpty(photo))
                {
                    byte[] byteData = byteData = Convert.FromBase64String(photo);
                    dd = new DataItemDetailBLL();
                    string path = dd.GetItemValue("imgPath") + "\\Resource\\PhotoFile";
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    string imageName = Guid.NewGuid().ToString() + ".png";
                    //FileStream fs = new FileStream(path + "\\" + imageName,FileMode.Create);
                    //fs.Write(byteData, 0, byteData.Length);
                    //fs.Close();
                    //fs.Dispose();
                    System.IO.File.WriteAllBytes(path + "\\" + imageName, byteData);
                    user.HeadIcon = "/Resource/PhotoFile/" + imageName;
                }

                if (!string.IsNullOrWhiteSpace(unitName))
                {
                    DepartmentEntity dept = new DepartmentBLL().GetList(orgId).Where(t => t.FullName == unitName).FirstOrDefault();
                    if (dept != null)
                    {
                        user.DepartmentId = dept.DepartmentId;
                        user.DepartmentCode = dept.EnCode;
                        user.OrganizeId = orgId;

                        string roleName = "";
                        string roleId = "";
                        //如果选择的是厂级部门的话，角色会默认追加“厂级部门用户”
                        if (dept.IsOrg == 1)
                        {
                            roleName += "厂级部门用户,";
                            RoleEntity cj = roleBll.GetList().Where(a => a.FullName == "厂级部门用户").FirstOrDefault();
                            if (cj != null)
                                roleId += cj.RoleId + ",";
                        }
                        string nature = dept.Nature;
                        switch (nature)
                        {
                            case "部门":
                                roleName += "部门级用户";
                                user.IsEpiboly = "0";
                                roleId += "6c094cef-cca3-4b41-a71b-6ee5e6b89008";
                                break;
                            case "专业":
                                roleName += "专业级用户";
                                user.IsEpiboly = "0";
                                roleId += "e3062d59-2484-4046-a420-478886d58656";
                                break;
                            case "班组":
                                roleName += "班组级用户";
                                user.IsEpiboly = "0";
                                roleId += "d9432a6e-5659-4f04-9c10-251654199714";
                                break;
                            case "承包商":
                                roleName += "承包商级用户";
                                user.IsEpiboly = "1";
                                roleId += "c5530ccf-e84e-4df8-8b27-fd8954a9bbe9";
                                break;
                            case "厂级":
                                roleName += "公司级用户";
                                user.IsEpiboly = "0";
                                roleId += "aece6d68-ef8a-4eac-a746-e97f0067fab5";
                                break;
                        }
                        roleName += ",普通用户";
                        roleId += ",2a878044-06e9-4fe4-89f0-ba7bd5a1bde6";
                        user.RoleId = roleId;
                        user.RoleName = roleName;
                        res = new UserBLL().SaveForm(userId, user);
                        if (!string.IsNullOrEmpty(res))
                        {
                            code = 1;
                            message = string.IsNullOrWhiteSpace(userId) ? "推送成功" : "人员信息修改成功";
                        }
                        else
                        {
                            code = 0;
                            message = "推送失败";
                        }
                    }
                    else
                    {
                        code = 0;
                        message = "单位名称与系统中名称不匹配，无法插入！";
                    }
                    System.IO.File.AppendAllText(dir + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ," + message + ",推送信息：" + json + "\r\n");
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                System.IO.File.AppendAllText(dir + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":推送人员信息>" + ex.Message + ",推送信息：" + json + "\r\n");
            }
            return new { code = code, message = message };
        }
        /// <summary>
        /// 华电西塞山与人脸系统对接向双控平台推送人员进出门禁记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object PushEnterInfo([FromBody]JObject json)
        {
            string fileName = "menjin_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string dir = HttpContext.Current.Server.MapPath("~/logs/xssmj");
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string time = res.Contains("time") ? dy.time : "";
                string userName = res.Contains("userName") ? dy.userName : "";
                string idCard = res.Contains("idCard") ? dy.idCard : "";
                string unitName = res.Contains("unitName") ? dy.unitName : "";
                string enrtyWay = res.Contains("enrtyWay") ? dy.enrtyWay : "";

                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(unitName) || string.IsNullOrWhiteSpace(idCard) || string.IsNullOrWhiteSpace(time) || string.IsNullOrWhiteSpace(enrtyWay))
                {
                    string msg = "推送失败：姓名(userName)、单位名称(unitName)、身份证号(idCard)、时间(time)、进出通道(enrtyWay)都不能为空！";
                    System.IO.File.AppendAllText(dir + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ," + msg + ",推送信息：" + json + "\r\n");
                    return new { code = 0, message = msg };
                }
                if (!BSFramework.Util.ValidateUtil.IsIdCard(idCard))
                {
                    string msg = "推送失败：身份证号格式错误！";
                    System.IO.File.AppendAllText(dir + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ," + msg + ",推送信息：" + json + "\r\n");
                    return new { code = 0, message = msg };
                }

                UserEntity user = new UserBLL().GetUserByIdCard(idCard);
                if (user == null)
                {
                    string msg = "推送失败：用户信息在系统中不存在(身份证号不匹配)";
                    System.IO.File.AppendAllText(dir + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ," + msg + ",推送信息：" + json + "\r\n");
                    return new { code = 0, message = msg };
                }
                else
                {
                    int isOut = enrtyWay.EndsWith("入口") ? 0 : 1;
                    string sql = string.Format("insert into XSS_ENTERRECORD(id,createtime,username,unitname,time,idcard,enrtyway,remark) values('{0}',to_date('{1}','yyyy-mm-dd hh24:mi:ss'),'{2}','{3}',to_date('{4}','yyyy-mm-dd hh24:mi:ss'),'{5}','{6}','{7}')", Guid.NewGuid().ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userName, unitName, time, idCard, enrtyWay, isOut);
                    int count = new DepartmentBLL().ExecuteSql(sql);
                    string msg = count > 0 ? "推送人员门禁进出记录成功" : "推送人员门禁进出记录失败";
                    System.IO.File.AppendAllText(dir + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ," + msg + ",推送信息：" + json + "\r\n");
                    return new { code = 1, message = msg };
                }
            }
            catch (Exception ex)
            {
                string msg = "推送人员门禁进出记录发生异常，错误信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(ex);
                System.IO.File.AppendAllText(dir + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " :推送门禁进出记录>" + msg + ",推送信息：" + json + "\r\n");
                return new { code = 0, message = ex.Message };
            }
        }

        /// <summary>
        /// 设置个人待办事项提醒状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object setWaitWorkStatus([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string sql = string.Format("begin\r\ndelete from BIS_USERWAIWORK where userid='{1}';\r\ninsert into BIS_USERWAIWORK(id,userid,status) values('{0}','{1}',1);\r\n end\r\ncommit;", Guid.NewGuid().ToString(), userId);
                int count = deptBll.ExecuteSql(sql);
                return new { Code = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

    }
}