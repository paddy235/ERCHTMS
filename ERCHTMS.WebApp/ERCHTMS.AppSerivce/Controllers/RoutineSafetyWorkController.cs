using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.RoutineSafetyWork;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.RoutineSafetyWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class RoutineSafetyWorkController : BaseApiController
    {
        public HttpContext ctx { get { return HttpContext.Current; } }
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private AnnouncementBLL announcementbll = new AnnouncementBLL();//通知公告
        private SecurityDynamicsBLL securitydynamicsbll = new SecurityDynamicsBLL();//安全动态
        private SecurityRedListBLL securityredlistbll = new SecurityRedListBLL();//安全红黑榜
        private ConferenceBLL conferencebll = new ConferenceBLL();//安全会议
        private ConferenceUserBLL conferenceuserbll = new ConferenceUserBLL();//安全会议参会人员
        private SpecialEquipmentBLL seBll = new SpecialEquipmentBLL();
        private AnnounDetailBLL announdetailbll = new AnnounDetailBLL();

        /// <summary>
        /// 获取通知公告列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAnnouncementList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        type = string.Empty,
                        title = string.Empty,
                        pageNum = 1,
                        pageSize = 20,
                        NoticType = string.Empty,//通知公告类型
                        showrange = string.Empty,//0全部  1本人发布 2本人接收
                        Status = string.Empty,//空全部 0未读  1已读
                        keyword=string.Empty
                    }
                });
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }

                Pagination pagination = new Pagination();
                pagination.conditionJson = " 1=1 ";
                pagination.page = dy.data.pageNum;
                pagination.rows = dy.data.pageSize;
                pagination.sidx = "ReleaseTime";
                pagination.sord = "desc";
                var list = announcementbll.GetPageList(pagination, JsonConvert.SerializeObject(dy.data), "app");
                list.Columns.Add("Url");
                var webUrl = new DataItemDetailBLL().GetItemValue("imgUrl", "AppSettings");
                foreach (DataRow item in list.Rows)
                {
                    item["Url"] = webUrl + string.Format("/Content/SecurityDynamics/AppShowNotice.html?keyValue={0}", item["id"]);
                }
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm" //格式化日期
                };
                settings.Converters.Add(new DecimalToStringConverter());
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(list, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }

        }

        [HttpPost]
        public object GetNoticType()
        {
            try
            {
                string EnCode = "NoticeCategory";
                var data = new DataItemDetailBLL().GetDataItemListByItemCode("'" + EnCode + "'");
                //if (!string.IsNullOrWhiteSpace(Remark))
                //{
                //    data = data.Where(x => x.ItemCode == Remark);
                //}

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                return new { code = 0, info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
       
        }
        /// <summary>
        /// 获取通知公告详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAnnouncement([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.AnnouncementId;//通知公告记录ID
                AnnouncementEntity entity = announcementbll.GetEntity(id);
                if (entity == null)
                {
                    throw new ArgumentException("未找到信息");
                }
                //未开放
                else
                {
                    if (entity.IsSend == "0")
                    {
                        var detail = announdetailbll.GetEntity(curUser.UserId, entity.Id);
                        if (detail != null)
                        {
                            detail.Status = 1;
                            detail.LookTime = DateTime.Now;
                            announdetailbll.SaveForm(detail.Id, detail);
                        }
                        ////更改状态
                        //announcementbll.UpdateStatus(id);
                    }
                }
                string jsondata = JsonConvert.SerializeObject(entity);
                NoticeData model = JsonConvert.DeserializeObject<NoticeData>(jsondata);
                List<Photo> pList = new List<Photo>(); //附件
                DataTable file = fileInfoBLL.GetFiles(entity.Id);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList.Add(p);
                }
                model.file = pList;
                model.NotReadUser = string.Join(",", announdetailbll.GetList().Where(x => x.Status == 0 && x.AuuounId == entity.Id).Select(x => x.UserName).ToList());
                model.ReadUser = string.Join(",", announdetailbll.GetList().Where(x => x.Status == 1 && x.AuuounId == entity.Id).Select(x => x.UserName).ToList());
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
        
                return new { code = 0, info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(model, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 新增安全动态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AddSecurityDynamics()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                SecurityDynamicsEntity entity = new SecurityDynamicsEntity();
                entity.Id = Guid.NewGuid().ToString();
                entity.Content = dy.data.Content;//内容
                entity.IsSend = "0";//是否发送
                entity.Publisher = curUser.UserName;
                entity.PublisherId = userId;
                entity.ReleaseTime = Convert.ToDateTime(dy.data.ReleaseTime);//发布时间
                entity.Title = dy.data.Title;//标题
                securitydynamicsbll.SaveForm("", entity);
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                //上传设备图片
                UploadifyFile(entity.Id, "SecurityDynamics", files);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 获取安全动态列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSecurityDynamicsList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                //标题名称
                string title = dy.data.title;
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                //                pagination.p_fields = @"t.Title,t.Publisher,to_char(t.ReleaseTime,'yyyy-Mm-dd') as ReleaseTime,(select count(e.id) from BIS_ReadUserManage e where e.moduleid=t.id and e.isdz='0') as dzNum,
                //(select count(e.id) from BIS_ReadUserManage e where e.moduleid=t.id and e.isyd='0') as readNum";
                pagination.p_fields = string.Format(@" t.Title,t.Publisher,to_char(t.ReleaseTime,'yyyy-Mm-dd') as ReleaseTime,(select count(e.id) from BIS_ReadUserManage e where e.moduleid=t.id and e.isdz='3') as dzNum,
(select sum(e.isyd) from BIS_ReadUserManage e where e.moduleid=t.id) as readNum,
(select max(e.isdz) from BIS_ReadUserManage e where e.moduleid=t.id and e.userid='{0}' group by e.moduleid,e.userid ) as state ", userId);
                pagination.p_tablename = "BIS_SecurityDynamics t";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "t.releasetime";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = string.Format(" t.IsSend='0' and t.ISOVER='1' ");
                pagination.conditionJson += string.Format(" and t.CreateUserOrgCode='{0}'", curUser.OrganizeCode);
                if (!string.IsNullOrEmpty(title))
                {
                    pagination.conditionJson += string.Format(" and  t.title like '%{0}%' ", title);
                }
                DataTable dt = securitydynamicsbll.GetPageList(pagination, null);
                //安全动态地址
                string SecurityDynamics = ConfigurationManager.AppSettings.Get("SecurityDynamics");
                List<object> data = new List<object>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dynamic d = new ExpandoObject();
                        d.id = dr["id"].ToString();
                        d.title = dr["title"].ToString();
                        d.publisher = dr["publisher"].ToString();
                        d.releasetime = dr["releasetime"].ToString();
                        d.releasetime = dr["releasetime"].ToString();
                        d.dznum = dr["dznum"].ToString();
                        if (!string.IsNullOrEmpty(dr["readnum"].ToString()))
                        {
                            d.readnum = dr["readnum"].ToString();
                        }
                        else
                        {
                            d.readnum = "0";
                        }
                        if (!string.IsNullOrEmpty(dr["state"].ToString()))
                        {
                            d.state = dr["state"].ToString();
                        }
                        else
                        {
                            d.state = "4";
                        }
                        d.url = SecurityDynamics + "?keyValue=" + dr["id"].ToString() + "&state=0";
                        IList<Photo> pList = new List<Photo>(); //附件
                        DataTable file = fileInfoBLL.GetFiles(dr["id"].ToString());
                        foreach (DataRow drs in file.Rows)
                        {
                            Photo p = new Photo();
                            p.id = drs["fileid"].ToString();
                            p.filename = drs["filename"].ToString();
                            p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + drs["filepath"].ToString().Substring(1);
                            pList.Add(p);
                        }
                        d.file = pList;
                        data.Add(d);
                    }
                }
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }


        }

        /// <summary>
        /// 获取安全动态详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSecurityDynamicsEntity([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.SecurityDynamicsId;//安全动态记录ID
                SecurityDynamicsEntity entity = securitydynamicsbll.GetEntity(id);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.Id;
                obj.Content = entity.Content;//内容
                obj.Publisher = entity.Publisher;//发布人
                obj.ReleaseTime = entity.ReleaseTime.Value.ToString("yyyy-MM-dd");//发布时间
                obj.Title = entity.Title;//标题
                IList<Photo> pList = new List<Photo>(); //附件
                DataTable file = fileInfoBLL.GetFiles(entity.Id);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList.Add(p);
                }
                obj.file = pList;
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = obj };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 安全动态、红黑榜阅读/点赞
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object UpdateLikeState([FromBody]JObject json)
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //安全动态/红榜ID
                string id = dy.data.id;
                //类型(阅读0，点赞1)
                string type = dy.data.type;
                //是否点赞(3已赞,4赞)默认为4
                string state = dy.data.state;
                string sql = string.Format("select t.userid,t.isyd from bis_readusermanage t where t.moduleid='{0}' and t.userid='{1}' ", id, userId);
                DataTable dt = seBll.SelectData(sql);
                bool b = false;
                int num = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    b = true;
                }
                string sqlWhere = "";
                string st = "4";
                if (type == "0")
                {
                    //只是阅读动态
                    if (b)
                    {
                        num = int.Parse(dt.Rows[0][1].ToString()) + 1;
                        sqlWhere = string.Format("isyd='{0}'", num);

                    }
                    else
                    {
                        st = "4";
                    }
                }
                else
                {
                    //点击赞按钮时
                    if (b)
                    {
                        num = int.Parse(dt.Rows[0][1].ToString()) + 1;
                        sqlWhere = string.Format("IsDz='{0}',isyd='{1}'", state, num);
                    }
                    else
                    {
                        st = "3";
                    }
                }
                if (b)
                {
                    sql = string.Format("update bis_readusermanage set {0} where moduleid='{1}' and userid='{2}'", sqlWhere, id, userId);
                }
                else
                {
                    sql = string.Format(@"insert into bis_readusermanage (ID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, UserName, UserId, ModuleId, IsDz, IsYd)
values ('{0}', '{1}', '{2}', '{3}', to_date('{4}','yyyy-mm-dd hh24:mi:ss'), '{5}','{6}', '{7}', '{8}', '{9}', '{10}')", Guid.NewGuid().ToString(), curUser.UserId, curUser.DeptCode, curUser.OrganizeCode, DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm"), curUser.UserName, curUser.UserName, curUser.UserId, id, st, "1");
                }
                seBll.UpdateData(sql);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败:" + ex.Message };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 新增安全红黑榜
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AddSecurityRed()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                SecurityRedListEntity entity = new SecurityRedListEntity();
                entity.Id = Guid.NewGuid().ToString();
                entity.IsSend = "0";//是否发送
                entity.Publisher = curUser.UserName;
                entity.PublisherId = userId;
                entity.ReleaseTime = Convert.ToDateTime(dy.data.ReleaseTime);//发布时间
                entity.Title = dy.data.Title;//标题
                entity.Content = dy.data.Content;
                entity.State = dy.data.State;//红黑榜类型（0红，1黑）
                entity.PublisherDept = dy.data.PublisherDept;//所属单位
                entity.PublisherDeptId = dy.data.PublisherDeptId;//所属单位ID
                securityredlistbll.SaveForm("", entity);
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                //上传设备图片
                UploadifyFile(entity.Id, "SecurityRedList", files);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 获取安全红黑榜列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSecurityRedList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                //标题名称
                string title = dy.data.title;
                //红黑榜类型（0红，1黑）
                string state = dy.data.state;
                //红黑榜记录ID
                string recordId = dy.data.recordId;
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                if (state == "0")
                {
                    pagination.p_fields = string.Format(@"t.Title,t.Publisher,t.publisherdept,t.content,to_char(t.ReleaseTime,'yyyy-Mm-dd') as ReleaseTime,(select count(e.id) from BIS_ReadUserManage e where e.moduleid=t.id and e.isdz='3') as dzNum,
(select sum(e.isyd) from BIS_ReadUserManage e where e.moduleid=t.id ) as readNum,(select e.isdz from BIS_ReadUserManage e where e.moduleid=t.id and e.userid='{0}') as state", userId);
                }
                else
                {
                    pagination.p_fields = @"t.Title,t.Publisher,t.publisherdept,t.content,to_char(t.ReleaseTime,'yyyy-Mm-dd') as ReleaseTime,
(select sum(e.isyd) from BIS_ReadUserManage e where e.moduleid=t.id ) as readNum";
                }
                pagination.p_tablename = "BIS_SecurityRedList t";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "t.releasetime";//排序字段
                pagination.sord = "desc";//排序方式
                if (!string.IsNullOrEmpty(recordId))
                {
                    pagination.conditionJson = string.Format(" t.id='{0}'", recordId);
                }
                else
                {
                    if (state == "0")
                    {
                        pagination.conditionJson = string.Format(" t.state='0'");
                    }
                    else
                    {
                        pagination.conditionJson = string.Format(" t.state='1'");
                    }
                    pagination.conditionJson += string.Format(" and t.IsSend='0'");
                    pagination.conditionJson += string.Format(" and t.CreateUserOrgCode='{0}'", curUser.OrganizeCode);
                    if (!string.IsNullOrEmpty(title))
                    {
                        pagination.conditionJson += string.Format(" and t.title like '%{0}%' ", title);
                    }
                }

                DataTable dt = securityredlistbll.GetPageList(pagination, null);
                //红黑榜地址
                string SecurityRedList = ConfigurationManager.AppSettings.Get("SecurityDynamics");
                List<object> data = new List<object>();
                string sql = string.Empty;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dynamic d = new ExpandoObject();
                        d.id = dr["id"].ToString();
                        d.title = dr["title"].ToString();
                        d.publisher = dr["publisher"].ToString();
                        d.releasetime = dr["releasetime"].ToString();
                        d.releasetime = dr["releasetime"].ToString();
                        if (!string.IsNullOrEmpty(dr["readnum"].ToString()))
                        {
                            d.readnum = dr["readnum"].ToString();
                        }
                        else
                        {
                            d.readnum = "0";
                        }

                        d.publisherdept = dr["publisherdept"].ToString();
                        d.content = dr["content"].ToString();
                        if (state == "0")
                        {
                            if (!string.IsNullOrEmpty(dr["state"].ToString()))
                            {
                                d.state = dr["state"].ToString();
                            }
                            else
                            {
                                d.state = "4";
                            }
                            d.dznum = dr["dznum"].ToString();
                            //点赞人员（取四名）
                            string sql1 = string.Format(@" select wm_concat(username) from (select t.username from BIS_ReadUserManage t where t.isdz='3' and t.moduleid='{0}' order by t.modifydate desc) where rownum<5", dr["id"].ToString());
                            string dzName = string.Empty;
                            DataTable dt1 = seBll.SelectData(sql1);
                            if (dt1 != null && dt1.Rows.Count > 0)
                            {
                                dzName = dt1.Rows[0][0].ToString();
                            }
                            d.dzname = dzName;
                        }
                        d.url = SecurityRedList + "?keyValue=" + dr["id"].ToString() + "&state=1";

                        IList<Photo> pList = new List<Photo>(); //附件
                        DataTable file = fileInfoBLL.GetFiles(dr["id"].ToString());
                        foreach (DataRow drs in file.Rows)
                        {
                            Photo p = new Photo();
                            p.id = drs["fileid"].ToString();
                            p.filename = drs["filename"].ToString();
                            p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + drs["filepath"].ToString().Substring(1);
                            pList.Add(p);
                        }
                        d.file = pList;
                        data.Add(d);
                    }
                }
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }


        }

        /// <summary>
        /// 获取安全动态和红黑榜详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetNewsRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //安全动态或红黑榜记录ID
                string id = dy.data.SecurityRedId;
                string type = dy.data.type;//数据类型,0安全动态，1红黑榜
                dynamic obj = new ExpandoObject();
                //安全动态地址
                string SecurityDynamics = ConfigurationManager.AppSettings.Get("SecurityDynamics");
                obj.id = id;
                //web端页面地址
                obj.url = SecurityDynamics + "?keyValue=" + id + "&state=1";
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = obj };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取安全会议清单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetConferenceList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                //会议名称
                string ConferenceName = dy.data.ConferenceName;
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = "ConferenceName,to_char(ConferenceTime,'yyyy-Mm-dd HH:ss:mm') as ConferenceTime";
                pagination.p_tablename = "BIS_Conference t";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "ConferenceTime";//排序字段
                pagination.sord = "desc";//排序方式
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), "10d10b8d-c5f6-418d-924d-07859f942164");
                pagination.conditionJson = where + string.Format(" and ((instr(userid,'{0}')>0 and IsSend='0') or (createuserid='{0}' and IsSend='0')) ", userId);
                if (!string.IsNullOrEmpty(ConferenceName))
                {
                    pagination.conditionJson += string.Format(" and ConferenceName like '%{0}%' ", ConferenceName);
                }
                DataTable dt = conferencebll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }


        }

        /// <summary>
        /// 获取安全会议详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetConference([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.ConferenceId;//会议ID
                ConferenceEntity entity = conferencebll.GetEntity(id);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.Id;
                obj.Content = entity.Content;//内容
                obj.Compere = entity.Compere;//主持人
                obj.CompereDept = entity.CompereDept;//召开部门
                obj.ConferenceName = entity.ConferenceName;//会议名称
                obj.ConferencePerson = entity.ConferencePerson;//会议应到人数
                obj.ConferenceTime = entity.ConferenceTime.ToString();//会议时间
                obj.Locale = entity.Locale;//地点
                obj.UserName = entity.UserName;//参会人员
                IList<Photo> pList = new List<Photo>();
                IList<Photo> rList = new List<Photo>();
                //会议资料附件
                DataTable file = fileInfoBLL.GetFiles(entity.Id);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList.Add(p);
                }
                obj.pfile = pList;
                //会议记录附件
                file = fileInfoBLL.GetFiles(entity.ConferenceRedId);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    rList.Add(p);
                }
                obj.rfile = rList;
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = obj };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 会议签到
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public Object registerMeeting()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.ConferenceId;//会议ID
                //获取会议记录
                ConferenceEntity ce = conferencebll.GetEntity(id);
                //获取参会人员记录
                ConferenceUserEntity entity = conferenceuserbll.GetEntity(id, userId);
                //DateTime time = Convert.ToDateTime(ce.ConferenceTime);
                //if (DateTime.Now.AddHours(1) < time)
                //{
                //    return new { info = "未到签到时间", code = 3, count = -1, data = new List<Object>() };
                //}
                //else if (DateTime.Now.Date > time.Date)
                //{
                //    return new { info = "已过签到时间", code = 4, count = -1, data = new List<Object>() };
                //}
                //else
                //{

                //}
                HttpFileCollection hf = ctx.Request.Files;//上传的文件 
                HttpPostedFile file = hf[0];
                string filename = DateTime.Now.Ticks + file.FileName;
                string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                string virtualPath = string.Format("/Resource/DocumentFile/{0}/{1}/{2}", userId, uploadDate, filename);
                string virtualPath1 = string.Format("/Resource/DocumentFile/{0}/{1}/{2}", userId, uploadDate, filename);
                string fullFileName = dataitemdetailbll.GetItemValue("imgPath") + virtualPath1;
                //创建文件夹
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                file.SaveAs(fullFileName);//参数路径，保存
                entity.Issign = "0";
                entity.PhotoUrl = virtualPath;
                conferenceuserbll.SaveForm(ce.Id, entity);
                return new { info = "会议签到成功", code = 0, count = -1, data = new List<Object>() };

            }
            catch (Exception ex)
            {
                return new { info = "会议签到失败：" + ex.Message, code = 0, count = -1, data = new List<Object>() };
            }

        }

        /// <summary>
        /// 获取安全会议是否签到/请假列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetConferenceIsSign([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                //列表数据状态(0未签到，1已签到，2已请假，3审批列表)
                string state = dy.data.State;
                //会议名称
                string ConferenceName = dy.data.ConferenceName;
                Pagination pagination = new Pagination();
                pagination.p_kid = "e.id";
                pagination.p_fields = "e.conferencename,to_char(ConferenceTime,'yyyy-MM-dd HH:ss:mm') as ConferenceTime,t.reviewstate";
                pagination.p_tablename = "Bis_Conferenceuser t left join bis_conference e on t.conferenceid=e.id";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "conferencetime";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = " e.issend='0' ";//已发送的会议
                //reviewstate（0表示未申请请假，1表示请假审批中,2表示请假已批准，3表示请假未批准）issign（1表示未签到）
                switch (state)
                {
                    case "0":
                        pagination.conditionJson += string.Format(" and t.userid='{0}' and t.issign='1' and (t.reviewstate='0' or t.reviewstate='3' or t.reviewstate='1')", userId);
                        break;
                    case "1":
                        pagination.conditionJson += string.Format(" and t.userid='{0}' and (t.issign='0' or t.reviewstate='2')", userId);
                        break;
                    case "2":
                        pagination.conditionJson += string.Format(" and t.userid='{0}' and t.reviewstate='2' ", userId);
                        break;
                    case "3":
                        pagination.p_fields = "e.conferencename,to_char(ConferenceTime,'yyyy-MM-dd HH:ss:mm') as ConferenceTime,t.username,t.userid,t.reviewstate";
                        pagination.conditionJson += string.Format(" and t.ReviewUserID='{0}' and t.ReviewState='1' ", userId);
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(ConferenceName))
                {
                    pagination.conditionJson += string.Format(" and e.conferencename like '%{0}%' ", ConferenceName);
                }
                DataTable dt = conferencebll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }


        }

        /// <summary>
        /// 审批会议请假
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object UpdateReviewState([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //会议ID
                string ConferenceId = dy.data.ConferenceId;
                //请假人ID
                string LeaverUserID = dy.data.LeaverUserID;
                //审核意见(2同意，3不同意)
                string state = dy.data.State;
                ConferenceUserEntity entity = conferenceuserbll.GetEntity(ConferenceId, LeaverUserID);
                entity.ReviewState = state;
                conferenceuserbll.SaveForm(ConferenceId, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "审批失败" };
            }

            return new { code = 0, count = 0, info = "审批成功" };
        }

        /// <summary>
        /// 获取审批页面详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetReviewInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.ConferenceId;//会议ID
                string LeaverUserID = dy.data.LeaverUserID;//请假人ID
                ConferenceEntity entity = conferencebll.GetEntity(id);
                //获取请假人员记录
                ConferenceUserEntity cue = conferenceuserbll.GetEntity(id, LeaverUserID);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.Id;
                obj.Content = entity.Content;//内容
                obj.Compere = entity.Compere;//主持人
                obj.CompereId = entity.CompereId;//主持人Id
                obj.CompereDept = entity.CompereDept;//召开部门
                obj.ConferenceName = entity.ConferenceName;//会议名称
                obj.ConferencePerson = entity.ConferencePerson;//会议应到人数
                obj.ConferenceTime = entity.ConferenceTime.ToString();//会议时间
                obj.Locale = entity.Locale;//地点
                obj.UserName = entity.UserName;//参会人员
                obj.LeaveUser = cue.UserName;//请假人员
                obj.Reason = cue.Reason;//请假原因
                obj.ReviewUser = cue.ReviewUser;//审批人
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = obj };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 会议请假信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SendLeave([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //会议ID
                string ConferenceId = dy.data.ConferenceId;
                //请假原因
                string Reason = dy.data.Reason;
                //审批人
                string ReviewUser = dy.data.ReviewUser;
                //审批人ID
                string ReviewUserID = dy.data.ReviewUserID;
                ConferenceUserEntity entity = conferenceuserbll.GetEntity(ConferenceId, userId);
                entity.Reason = Reason;
                entity.ReviewUser = ReviewUser;
                entity.ReviewUserID = ReviewUserID;
                entity.ReviewState = "1";
                conferenceuserbll.SaveForm(ConferenceId, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="Filedata"></param>
        public void UploadifyFile(string folderId, string foldername, HttpFileCollection fileList)
        {

            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = fileList[i];
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string userId = OperatorProvider.Provider.Current().UserId;
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string virtualPath = string.Format("~/Resource/DocumentFile/{0}/{1}/{2}{3}", userId, uploadDate, fileGuid, FileEextension);
                        string virtualPath1 = string.Format("/Resource/DocumentFile/{0}/{1}/{2}{3}", userId, uploadDate, fileGuid, FileEextension);
                        string fullFileName = dataitemdetailbll.GetItemValue("imgPath") + virtualPath1;
                        //创建文件夹
                        string path = Path.GetDirectoryName(fullFileName);
                        Directory.CreateDirectory(path);
                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(fullFileName))
                        {
                            //保存文件
                            file.SaveAs(fullFileName);
                            //文件信息写入数据库

                            fileInfoEntity.Create();
                            fileInfoEntity.FileId = fileGuid;
                            if (!string.IsNullOrEmpty(folderId))
                            {
                                fileInfoEntity.FolderId = folderId;
                            }
                            else
                            {
                                fileInfoEntity.FolderId = "0";
                            }
                            fileInfoEntity.FileName = file.FileName;
                            fileInfoEntity.FilePath = virtualPath;
                            fileInfoEntity.FileSize = filesize.ToString();
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.Replace(".", "");
                            fileInfoBLL.SaveForm("", fileInfoEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
