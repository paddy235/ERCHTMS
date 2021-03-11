using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class WorkMeetingController : BaseApiController
    {
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private WorkMeetingBLL workMeetingbll = new WorkMeetingBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private DepartmentBLL deptbll = new DepartmentBLL();
        private DangerdataBLL dangerdatabll = new DangerdataBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }
        #region 查询条件
        /// <summary>
        /// 获取外包工程
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]       
        public object GetEngineerList([FromBody]JObject json)
        {
            string res = json.Value<string>("json").ToLower();
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string senddeptid = "";//发包单位
            string outprojectid = "";//外包单位            
            if (res.Contains("data"))
            {
                if (res.Contains("senddeptid"))
                {
                    senddeptid = dy.data.senddeptid;
                }
                if (res.Contains("outprojectid"))
                {
                    outprojectid = dy.data.outprojectid;
                }
            }
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }
            string roleNames = curUser.RoleName;
            //分页获取数据
            Pagination pagination = new Pagination();
            pagination.page = 1;// int.Parse(dy.page ?? "1");
            pagination.rows = 9000;// int.Parse(dy.rows ?? "1");
            pagination.p_kid = "e.ID";
            pagination.p_fields = @"e.usedeptpeople,
	                                        e.engineerusedept,
	                                        e.usedeptpeopphone,
	                                        e.engineerdirector,
	                                        e.engineerdirectorphone,
                                           e.ENGINEERCODE,b.senddeptid,b.senddeptname,
                                           e.ENGINEERNAME,
                                           d.itemname ENGINEERTYPE,
                                           l.itemname engineerlevel,
                                           e.OUTPROJECTID,
                                           e.PLANENDDATE,
                                           e.ACTUALENDDATE,
                                           s.itemname engineerstate,
                                           e.CREATEDATE,
                                           b.fullname outprojectName,
                                            decode(ss.EXAMSTATUS, '1', '', '单位资质审查') EXAMSTATUS,
                                           decode(ss.pactstatus, '1', '', '协议管理') pactstatus,
                                           decode(ss.threetwostatus, '1', '', '三措两案') threetwostatus,
                                           decode(ss.technicalstatus, '1', '', '安全技术交底') technicalstatus,
                                           decode(ss.equipmenttoolstatus, '1', '', '工器具验收') equipmenttoolstatus,
                                            decode(ss.peoplestatus, '1', '', '人员资质审查') peoplestatus,
                                             decode(ss.compactstatus, '1', '', '合同管理') compactstatus,
                                            i.busvalidstarttime,
                                            i.busvalidendtime,
                                            i.splvalidstarttime,
                                            i.splvalidendtime,   
                                            i.cqvalidstarttime,
                                            i.cqvalidendtime 
                                    ";
            pagination.p_tablename = @"EPG_OutSouringEngineer e
                                          left join base_department b on b.departmentid=e.outprojectid
                                          left join epg_aptitudeinvestigateinfo i on i.outengineerid = e.id
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate
                                          left join epg_startappprocessstatus ss on ss.outengineerid=e.id";
            pagination.sidx = "e.createdate";//排序字段
            pagination.sord = "desc";//排序方式
            if (curUser.IsSystem)
            {
                pagination.conditionJson = "  1=1 ";
            }
            else if (curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("公司管理员"))
            {
                pagination.conditionJson = string.Format(" e.createuserorgcode = '{0}' ", curUser.OrganizeCode);
            }
            else if (curUser.RoleName.Contains("承包商级用户"))
            {
                pagination.conditionJson = string.Format("  e.OUTPROJECTID ='{0}'", curUser.DeptId);
            }
            else
            {
                pagination.conditionJson = string.Format("  e.engineerletdeptid='{0}'", curUser.DeptId);
            }
            if (!string.IsNullOrWhiteSpace(senddeptid))
            {//发包单位下的外包工程
                pagination.conditionJson = string.Format("  e.engineerletdeptid='{0}'", senddeptid);
            }
            if (!string.IsNullOrWhiteSpace(outprojectid))
            {//外包单位下的外包工程
                pagination.conditionJson = string.Format("  e.OUTPROJECTID ='{0}'", outprojectid);
            }
         
            var queryJson = JsonConvert.SerializeObject(new { engineerstate="002",IsDeptAdd=1 });
            var data = outsouringengineerbll.GetPageList(pagination, queryJson);

            //var JsonData = new
            //{
            //    rows = data,
            //    total = pagination.total,
            //    page = pagination.page,
            //    records = pagination.records,
            //};
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                //ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                DateFormatString = "yyyy-MM-dd HH:mm:ss", //格式化日期
                //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
            };
            return new { code = 0, info = "获取数据成功", count = data == null ? 0 : data.Rows.Count, data = JArray.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
            //return Json(new { code = 0, info = "获取数据成功", count = data == null ? 0 : data.Rows.Count, data = JsonData }, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
        }
        /// <summary>
        /// 获取工程详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEngineerDetails([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }
            try
            {
                string id = dy.data ?? "";
                var disBll = new DistrictBLL();
                var didBll = new DataItemDetailBLL();
                var deptBll = new DepartmentBLL();
                var data = outsouringengineerbll.GetEntity(id);
                if (!string.IsNullOrWhiteSpace(data.ENGINEERAREA))
                {
                    var area = disBll.GetNameAndID(data.ENGINEERAREA);
                    if (area.Rows.Count>0)
                    {
                        string[] areanames = area.AsEnumerable().Select(t => t.Field<string>("districtname")).ToArray();
                        data.ENGINEERAREANAME = string.Join(",", areanames);
                    }
                }
                if (!string.IsNullOrWhiteSpace(data.ENGINEERTYPE))
                {
                    var listType = didBll.GetDataItem("ProjectType", data.ENGINEERTYPE).ToList();
                    if (listType != null && listType.Count > 0)
                        data.ENGINEERTYPENAME = listType[0].ItemName;
                }
                if (!string.IsNullOrWhiteSpace(data.ENGINEERLEVEL))
                {
                    var listLevel = didBll.GetDataItem("ProjectLevel", data.ENGINEERLEVEL).ToList();
                    if (listLevel != null && listLevel.Count > 0)
                        data.ENGINEERLEVELNAME = listLevel[0].ItemName;
                }
                if (!string.IsNullOrWhiteSpace(data.OUTPROJECTID))
                {
                    var dept = deptbll.GetEntity(data.OUTPROJECTID);
                    data.OUTPROJECTCODE = dept.EnCode;
                    data.OUTPROJECTNAME = dept.FullName;
                }
                //return Json(new
                //{
                //    code = 0,
                //    info = "获取数据成功",
                //    count = 0,
                //    data = data
                //}, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    //ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm:ss", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", count = 1, data = JObject.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
                //return new
                //{
                //    code = 0,
                //    info = "获取数据成功",
                //    count = 0,
                //    data = data
                //};
            }
            catch(Exception ex)
            {
                return new { code = -1, count = 0, info = "获取失败，错误：" + ex.Message, data = new object() };
            }
        }
        /// <summary>
        /// 获取外包单位
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEngineerDeptList([FromBody]JObject json)
        {
            string res = json.Value<string>("json").ToLower();
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string senddeptid = "";//发包单位
            string engineerid = "";//外包工程
            if (res.Contains("data"))
            {
                if (res.Contains("senddeptid"))
                {
                    senddeptid = dy.data.senddeptid;
                }
                if (res.Contains("engineerid"))
                {
                    engineerid = dy.data.engineerid;
                }
            }

            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }
            string roleNames = curUser.RoleName;
            //分页获取数据
            Pagination pagination = new Pagination();
            pagination.page = 1;// int.Parse(dy.page ?? "1");
            pagination.rows = 9000; //int.Parse(dy.rows ?? "1");
            pagination.p_kid = "departmentid";
            pagination.p_fields = @" organizeid,parentid,encode,fullname,shortname,nature,depttype ";
            pagination.p_tablename = @"base_department";
            pagination.sidx = "encode asc,sortcode";//排序字段
            pagination.sord = "asc";//排序方式
            string conditionJson = " nature='承包商' ";
            string where = "";
            if (curUser.IsSystem || curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("公司管理员"))
            {
                where = string.Format(" and encode like '{0}%' ", curUser.OrganizeCode);
            }
            else if (curUser.RoleName.Contains("承包商级用户"))
            {
                where = string.Format(" and departmentid ='{0}'", curUser.DeptId);
            }
            else
            {
                where = string.Format(" and departmentid in(select distinct(e.outprojectid) from epg_outsouringengineer e where e.engineerletdeptid='{0}')", curUser.DeptId);
            }
            if (!string.IsNullOrWhiteSpace(senddeptid))
            {//发包单位下的外包单位
                where = string.Format(" and departmentid in(select distinct(e.outprojectid) from epg_outsouringengineer e where e.engineerletdeptid='{0}')", senddeptid);
            }
            if (!string.IsNullOrWhiteSpace(engineerid))
            {//外包工程对应的外包单位
                where = string.Format(" and departmentid in(select outprojectid from EPG_OutSouringEngineer where  id ='{0}')", engineerid);
            }
            pagination.conditionJson = conditionJson + where;
            var data = deptbll.GetPageList(pagination, "");

            /*var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
            };*/

            return new { code = 0, info = "获取数据成功", count = data == null ? 0 : data.Rows.Count, data = data };
        }
        /// <summary>
        /// 获取发包单位
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSendDeptList([FromBody]JObject json)
        {
            string res = json.Value<string>("json").ToLower();
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string outprojectid = "";//外包单位
            string engineerid = "";//外包工程
            if (res.Contains("data"))
            {
                if (res.Contains("outprojectid"))
                {
                    outprojectid = dy.data.outprojectid;
                }
                if (res.Contains("engineerid"))
                {
                    engineerid = dy.data.engineerid;
                }
            }
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }
            Pagination pagination = new Pagination();
            pagination.page = 1;// int.Parse(dy.page ?? "1");
            pagination.rows = 9000; //int.Parse(dy.rows ?? "1");
            pagination.p_kid = "departmentid";
            pagination.p_fields = @" organizeid,parentid,encode,fullname,shortname,nature ";
            pagination.p_tablename = @"base_department";
            pagination.sidx = "encode asc,sortcode";//排序字段
            pagination.sord = "asc";//排序方式
            string conditionJson = " nature='部门' and (description is null or description<>'外包工程承包商') ";
            string where = "";
            if (curUser.IsSystem || curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("公司管理员"))
            {
                where = string.Format(" and encode like '{0}%' ", curUser.OrganizeCode);
            }
            else if (curUser.RoleName.Contains("承包商级用户"))
            {
                where = string.Format(" and departmentid in(select engineerletdeptid from EPG_OutSouringEngineer where outprojectid='{0}')", curUser.DeptId);
            }
            else
            {
                where = string.Format(" and departmentid ='{0}'", curUser.DeptId);
            }
            if (!string.IsNullOrWhiteSpace(outprojectid))
            {//外包单位下的发包单位
                where = string.Format(" and departmentid in(select engineerletdeptid from EPG_OutSouringEngineer where outprojectid='{0}')", outprojectid);
            }
            if (!string.IsNullOrWhiteSpace(engineerid))
            {//外包工程对应的发包单位
                where = string.Format(" and departmentid in(select engineerletdeptid from EPG_OutSouringEngineer where  id ='{0}')", engineerid);
            }
            pagination.conditionJson = conditionJson + where;
            var data = deptbll.GetPageList(pagination, "");

            return new { code = 0, info = "获取数据成功", count = data == null ? 0 : data.Rows.Count, data = data };
        }
        #endregion 

        #region 获取开收工会新增权限

        /// <summary>
        /// 获取开收工会新增权限
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetOperAuthority([FromBody]JObject json)
        {
            string res = json.Value<string>("json").ToLower();
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string moduleid = "b861d009-f570-4ffc-a666-d088a1aebd46";
            string encode = "add";
            OperatorProvider.AppUserId = userid;
            AuthorizeBLL authBLL = new AuthorizeBLL();
            Boolean OperAuthority = string.IsNullOrEmpty(authBLL.GetOperAuthority(OperatorProvider.Provider.Current(), moduleid, encode)) ? false : true;
            var JsonData = new
            {
                operauthority = OperAuthority
            };
            return new { code = 0, info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(JsonData)) };
        }

        #endregion

        #region 获取开收工会清单
        /// <summary>
        /// 获取开收工会清单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dyy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dyy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }
            string roleNames = curUser.RoleName;
            //分页获取数据
            var dy = dyy.data;
            Pagination pagination = new Pagination();
            pagination.page = int.Parse(dy.page ?? "1");
            pagination.rows = int.Parse(dy.rows ?? "1");
            pagination.p_kid = "a.id";
            pagination.p_fields = @"a.autoid,a.createdate,a.createuserid,a.createuserdeptcode,a.createuserorgcode,
a.modifydate,a.modifyuserid,a.meetingname,a.meetingtype,a.meetingdate,
a.shoudpernum,a.realpernum,a.address,b.fullname outprojectname,a.risklevel,a.isover";
            pagination.p_tablename = "BIS_WORKMEETING a left join ( select d.FullName,c.ID from EPG_OUTSOURINGENGINEER c, BASE_DEPARTMENT d where c.OUTPROJECTID=d.DEPARTMENTID) b on a.engineerid=b.id";
            pagination.sidx = "a.meetingdate";
            pagination.sord = "desc";
            pagination.conditionJson = " a.iscommit='1' ";            
            if (curUser.IsSystem || curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("公司管理员"))
            {
                pagination.conditionJson += string.Format(" and createuserorgcode = '{0}' ", curUser.OrganizeCode);
            }
            else
            {
                //发包部门查看所属承包商的开收工会、承包商查看发包部门创建的开收工会。
                pagination.conditionJson += string.Format(@" and (engineerid in (select e.ID from EPG_OutSouringEngineer e where e.engineerletdeptid ='{0}') or engineerid in (select e.ID from EPG_OutSouringEngineer e where e.outprojectid='{0}') )", curUser.DeptId);
                ////发包部门查看所属承包商的开收工会、承包商查看发包部门创建的开收工会。
                //pagination.conditionJson += string.Format(@" and (engineerid in (select e.ID from EPG_OutSouringEngineer e where e.outprojectid in(select departmentid from base_department where senddeptid='{0}')) or engineerid in (select e.ID from EPG_OutSouringEngineer e where e.outprojectid='{0}') )", curUser.DeptId);
            }
            //外包工程
            string engineerid = dy.engineerid ?? "";
            if (!string.IsNullOrWhiteSpace(engineerid))
            {
                pagination.conditionJson += string.Format(@" and  engineerid = '{0}' ", engineerid);
            }
            //外包单位
            string engineerdeptid = dy.engineerdeptid ?? "";
            if (!string.IsNullOrWhiteSpace(engineerdeptid))
            {
                pagination.conditionJson += string.Format(@" and  engineerid in (select e.ID from EPG_OutSouringEngineer e where e.outprojectid='{0}') ", engineerdeptid);
            }
            //发包部门
            string senddeptid = dy.senddeptid ?? "";
            if (!string.IsNullOrWhiteSpace(senddeptid) && !curUser.RoleName.Contains("承包商级用户"))
            {
                //发包部门下属外包单位的外包工程数据
                pagination.conditionJson += string.Format(@" and  engineerid in (select e.ID from EPG_OutSouringEngineer e where e.engineerletdeptid ='{0}') ", senddeptid);                
            }
            //开始时间
            string startTime = dy.startTime ?? "";
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                pagination.conditionJson += string.Format(" and meetingdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            //结束时间
            string endTime = dy.endTime ?? "";
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                pagination.conditionJson += string.Format(" and meetingdate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //类型 
            string meetingtype = dy.meetingtype ?? "";
            if (!string.IsNullOrWhiteSpace(meetingtype))
            {
                pagination.conditionJson += string.Format(@" and meetingtype = '{0}'", meetingtype);
            }
            //会议名称
            string meetingname = dy.meetingname ?? "";
            if (!string.IsNullOrWhiteSpace(meetingname))
            {
                pagination.conditionJson += string.Format(@" and meetingname like '%{0}%'", meetingname);
            }
            var data = workMeetingbll.GetPageList(pagination, null);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
            };
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                //ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                DateFormatString = "yyyy-MM-dd HH:mm:ss", //格式化日期
                //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
            };
            //return Json(new { code = 0, info = "获取数据成功", count = pagination.records, data = JsonData }, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });

            return new { code = 0, info = "获取数据成功", count = pagination.records, data = JObject.Parse(JsonConvert.SerializeObject(JsonData, Formatting.None, settings)) };
        }


        #endregion

        #region 获取开收工会详情
        /// <summary>
        /// 获取开收工会详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDetial([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string id = dy.data ?? "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
            
                var entity = workMeetingbll.GetEntity(id);
                if (entity != null)
                {
                    //外包单位
                    OutsouringengineerBLL engneerBll = new OutsouringengineerBLL();
                    var engneer = engneerBll.GetEntity(entity.ENGINEERID);
                    if (engneer != null)
                    {
                        entity.OUTPROJECTNAME = new DepartmentBLL().GetEntity(engneer.OUTPROJECTID).FullName;
                    }
                    if (!string.IsNullOrWhiteSpace(engneer.OUTPROJECTID))
                    {
                        var dept = deptbll.GetEntity(engneer.OUTPROJECTID);
                        entity.OUTPROJECTCODE = dept.EnCode;
                        entity.OUTPROJECTNAME = dept.FullName;
                    }
                    Dictionary<string, List<Photo>> files = new Dictionary<string, List<Photo>>();
                    //内容1附件
                    DataTable file = fileInfoBLL.GetFiles(entity.ID + "c1");
                    var pC1 = new List<Photo>();
                    foreach (DataRow dr in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = dr["fileid"].ToString();
                        p.filename = dr["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                        pC1.Add(p);
                    }
                    files.Add("c1", pC1);
                    //内容2附件
                    file = fileInfoBLL.GetFiles(entity.ID + "c2");
                    var pC2 = new List<Photo>();
                    foreach (DataRow dr in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = dr["fileid"].ToString();
                        p.filename = dr["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                        pC2.Add(p);
                    }
                    files.Add("c2", pC2);
                    //内容3附件
                    file = fileInfoBLL.GetFiles(entity.ID + "c3");
                    var pC3 = new List<Photo>();
                    foreach (DataRow dr in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = dr["fileid"].ToString();
                        p.filename = dr["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                        pC3.Add(p);
                    }
                    files.Add("c3", pC3);
                    //签名附件
                    file = fileInfoBLL.GetFiles(entity.ID);
                    var pList = new List<Photo>();
                    foreach (DataRow dr in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = dr["fileid"].ToString();
                        p.filename = dr["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                        pList.Add(p);
                    }
                    files.Add("sign", pList);
                    entity.FILES = files;
                    entity.MeasuresList = new WorkmeetingmeasuresBLL().GetList("").Where(x => x.Workmeetingid == entity.ID).OrderBy(t => t.CreateDate).ToList();
                    //return json(new
                    //{
                    //    code = 0,
                    //    info = "获取数据成功",
                    //    count = 0,
                    //    data = entity
                    //}, new jsonserializersettings() { dateformatstring = "yyyy-mm-dd hh:mm:ss" });
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        //ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                        DateFormatString = "yyyy-MM-dd HH:mm:ss", //格式化日期
                        //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                    };
                    return new
                    {
                        code = 0,
                        info = "获取数据成功",
                        count = 0,
                        data = JObject.Parse(JsonConvert.SerializeObject(entity, Formatting.None, settings))
                    };
                }
                else
                {
                    return new { code = -1, count = 0, info = "获取失败，记录不存在。", data = new object() };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "获取失败，错误：" + ex.Message, data = new object() };
            }
        }
        #endregion
        /// <summary>
        /// 根据用户获取未提交的数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetNotCommitData([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                WorkMeetingEntity entity = new WorkMeetingEntity();
                var workMeeting = workMeetingbll.GetNotCommitData(userid);
                if (workMeeting.Rows.Count > 0) {
                    entity = workMeetingbll.GetEntity(workMeeting.Rows[0]["id"].ToString());
                    entity.MeasuresList = new WorkmeetingmeasuresBLL().GetList("").Where(x => x.Workmeetingid == entity.ID).ToList();
                }
                return new { code = 0, count = workMeeting.Rows.Count, info = "获取成功", data = entity };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }
            

        }


        #region 新增开收工会
        /// <summary>
        /// 新增开收工会
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object Add()
        {
            try
            {
                string res = HttpContext.Current.Request["json"]; //json.Value<string>("json");
                Submit<WorkMeetingEntity> dy = JsonConvert.DeserializeObject<Submit<WorkMeetingEntity>>(res, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy'-'MM'-'dd HH':'mm':'ss" });
                string userid = dy.userId;
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
               

                var data = dy.data;
                WorkMeetingEntity entity = new WorkMeetingEntity()
                {
                    ID = Guid.NewGuid().ToString(),
                    ENGINEERAREA = data.ENGINEERAREA,
                    ENGINEERCODE = data.ENGINEERCODE,
                    ENGINEERCONTENT = data.ENGINEERCONTENT,
                    ENGINEERLETDEPT = data.ENGINEERLETDEPT,
                    ENGINEERID = data.ENGINEERID,
                    ISCOMMIT=data.ISCOMMIT,
                    ENGINEERLEVEL = data.ENGINEERLEVEL,
                    ENGINEERNAME = data.ENGINEERNAME,
                    ENGINEERTYPE = data.ENGINEERTYPE,
                    MEETINGDATE = data.MEETINGDATE,
                    MEETINGNAME = data.MEETINGNAME,
                    MEETINGTYPE = data.MEETINGTYPE,
                    ADDRESS = data.ADDRESS,    
                    CERTSTA = data.CERTSTA,
                    HEALTHSTA = data.HEALTHSTA,
                    CLOTHESTA = data.CLOTHESTA,
                    SAFEGOODSSTA = data.SAFEGOODSSTA,
                    ENUM = (int?)data.ENUM,
                    GNUM = (int?)data.GNUM,                    
                    LNUM = (int?)data.LNUM,
                    JNUM = (int?)data.JNUM,
                    ONUM = (int?)data.ONUM,
                    REMARK = data.REMARK,                                     
                    REALPERNUM = (int?)data.REALPERNUM,      
                    SHOUDPERNUM = (int?)data.SHOUDPERNUM,
                    SIGNPERSONS = data.SIGNPERSONS,
                    CONTENT1 = data.CONTENT1,
                    CONTENT2 = data.CONTENT2,
                    CONTENT3 = data.CONTENT3,
                    CONTENTOTHER = data.CONTENTOTHER,
                    RiskLevel=data.RiskLevel,
                    StartMeetingid=data.StartMeetingid
                };
         
                workMeetingbll.SaveWorkMeetingForm("", entity, dy.data.MeasuresList, data.ids);
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                UploadifyFile(entity.ID, "sign", files);//上传签名附件
                UploadifyFile(entity.ID + "c1", "c1", files);//上传录音1
                UploadifyFile(entity.ID + "c2", "c2", files);//上传录音2
                UploadifyFile(entity.ID + "c3", "c3", files);//上传录音3
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败，错误："+ex.Message, data = new object() };
            }
            return new { code = 0, count = 0, info = "保存成功", data = new object() };
        }
        #endregion

        #region 编辑开收工会
        /// <summary>
        /// 编辑开收工会
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object Edit()
        {
            try
            {
                string res = HttpContext.Current.Request["json"]; //json.Value<string>("json");
                Submit<WorkMeetingEntity> dy = JsonConvert.DeserializeObject<Submit<WorkMeetingEntity>>(res, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy'-'MM'-'dd HH':'mm':'ss" });
                string userid = dy.userId;                
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                string roleNames = curUser.RoleName;
                var data = dy.data;
                string recordId = data.ID;
                WorkMeetingEntity entity = workMeetingbll.GetEntity(recordId);
                if (entity!=null)
                {
                    entity.ENGINEERAREA = data.ENGINEERAREA;
                    entity.ENGINEERCODE = data.ENGINEERCODE;
                    entity.ENGINEERCONTENT = data.ENGINEERCONTENT;
                    entity.ENGINEERLETDEPT = data.ENGINEERLETDEPT;
                    entity.ENGINEERID = data.ENGINEERID;
                    entity.ENGINEERLEVEL = data.ENGINEERLEVEL;
                    entity.ENGINEERNAME = data.ENGINEERNAME;
                    entity.ENGINEERTYPE = data.ENGINEERTYPE;
                    entity.MEETINGDATE = data.MEETINGDATE;
                    entity.MEETINGNAME = data.MEETINGNAME;
                    entity.MEETINGTYPE = data.MEETINGTYPE;
                    entity.ADDRESS = data.ADDRESS;
                    entity.CERTSTA = data.CERTSTA;
                    entity.HEALTHSTA = data.HEALTHSTA;
                    entity.CLOTHESTA = data.CLOTHESTA;
                    entity.SAFEGOODSSTA = data.SAFEGOODSSTA;
                    entity.ENUM = (int?)data.ENUM;
                    entity.GNUM = (int?)data.GNUM;
                    entity.LNUM = (int?)data.LNUM;
                    entity.ONUM = (int?)data.ONUM;
                    entity.JNUM = (int?)data.JNUM;
                    entity.ISCOMMIT = data.ISCOMMIT;
                    entity.REMARK = data.REMARK;
                    entity.REALPERNUM = (int?)data.REALPERNUM;
                    entity.SHOUDPERNUM = (int?)data.SHOUDPERNUM;
                    entity.SIGNPERSONS = data.SIGNPERSONS;
                    entity.CONTENT1 = data.CONTENT1;
                    entity.CONTENT2 = data.CONTENT2;
                    entity.CONTENT3 = data.CONTENT3;
                    entity.CONTENTOTHER = data.CONTENTOTHER;
                    entity.RiskLevel = data.RiskLevel;
                    entity.StartMeetingid = data.StartMeetingid;
                    //获取删除附件ID
                    string deleteFileId = data.DELETEFILEID;
                    if (!string.IsNullOrEmpty(deleteFileId))
                    {
                        DeleteFile(deleteFileId);
                    }
                    HttpFileCollection files = ctx.Request.Files;//上传的文件 
                    UploadifyFile(entity.ID, "sign", files);//上传签名附件
                    UploadifyFile(entity.ID + "c1", "c1", files);//上传录音1
                    UploadifyFile(entity.ID + "c2", "c2", files);//上传录音2
                    UploadifyFile(entity.ID + "c3", "c3", files);//上传录音3
                    workMeetingbll.SaveWorkMeetingForm(recordId, entity,dy.data.MeasuresList,data.ids);

                }
                else
                {
                    return new { code = -1, count = 0, info = "保存失败，记录不存在。", data = new object() };
                }            
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败，错误：" + ex.Message, data = new object() };
            }
            return new { code = 0, count = 0, info = "保存成功", data = new object() };
        }
        #endregion

        #region 删除开收工会
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object Remove([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string id = dy.data ?? "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                workMeetingbll.RemoveForm(id);
                DeleteFileByRec(id);//删除签到附件
                DeleteFileByRec(id + "c1");//删除录音1
                DeleteFileByRec(id + "c2");//删除录音2
                DeleteFileByRec(id + "c3");//删除录音3
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "删除失败，错误：" + ex.Message, data = new object() };
            }
            return new { code = 0, count = 0, info = "删除成功", data = new object() };
        }
        #endregion

        #region 上传附件、删除附件
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="foldername"></param>
        /// <param name="fileList"></param>
        public void UploadifyFile(string folderId, string foldername, HttpFileCollection fileList)
        {
            try
            {
                if (fileList.Count > 0)
                {
                    foreach (string key in fileList.AllKeys)
                    {
                        if (key.Contains(foldername))
                        {
                            HttpPostedFile file = fileList[key];
                            //获取文件完整文件名(包含绝对路径)
                            //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                            string userId = OperatorProvider.Provider.Current().UserId;
                            string fileGuid = Guid.NewGuid().ToString();
                            long filesize = file.ContentLength;
                            string FileEextension = Path.GetExtension(file.FileName);
                            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                            string uploadDate1 = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                            string virtualPath = string.Format("~/Resource/WorkMeeting/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                            string virtualPath1 = string.Format("/Resource/WorkMeeting/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
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
                                fileInfoEntity.RecId = folderId; //关联ID
                                fileInfoEntity.FolderId = "WorkMeeting";
                                fileInfoEntity.FileName = uploadDate1+ FileEextension;
                                fileInfoEntity.FilePath = virtualPath;
                                fileInfoEntity.FileSize = filesize.ToString();
                                fileInfoEntity.FileExtensions = FileEextension;
                                fileInfoEntity.FileType = FileEextension.Replace(".", "");
                                fileInfoBLL.SaveForm("", fileInfoEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="fileInfoIds"></param>
        public bool DeleteFile(string fileInfoIds)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(fileInfoIds))
            {
                string ids = "";

                string[] strArray = fileInfoIds.Split(',');

                foreach (string s in strArray)
                {
                    ids += "'" + s + "',";
                    var entity = fileInfoBLL.GetEntity(s);
                    if (entity != null)
                    {
                        var filePath = dataitemdetailbll.GetItemValue("imgPath") + entity.FilePath.Replace("~", "");
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);
                }                
                int count = fileInfoBLL.DeleteFileForm(ids);

                result = count > 0 ? true : false;
            }

            return result;
        }
        public void DeleteFileByRec(string recId)
        {
            if (!string.IsNullOrWhiteSpace(recId))
            {
                var list = fileInfoBLL.GetFileList(recId);
                foreach (var file in list)
                {
                    fileInfoBLL.RemoveForm(file.FileId);
                    var filePath = dataitemdetailbll.GetItemValue("imgPath") + file.FilePath.Replace("~", "");
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// 获取危险点数据库
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDangerData([FromBody]JObject json) {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            int pageSize = res.Contains("pagesize") ? int.Parse(dy.pagesize.ToString()) : 10; //每页的记录数

            int pageIndex = res.Contains("pageindex") ? int.Parse(dy.pageindex.ToString()) : 1;  //当前页索引
            Pagination pagination = new Pagination();
            pagination.page = pageIndex;
            pagination.rows = pageSize;
            pagination.p_kid = "t.id";
            pagination.p_fields = " createuserid,createuserdeptcode,createuserorgcode,createdate,createusername,dangerpoint,usernum,updatenum,measures";
            pagination.p_tablename = "epg_dangerdata t";
            pagination.sidx = "createdate";
            pagination.sord = "desc";
            pagination.conditionJson = " 1=1 ";

            var data = workMeetingbll.GetPageList(pagination, null);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return new { code = 0, count = jsonData.records, info = "获取成功", data = jsonData };
        }
        /// <summary>
        /// 获取今日临时外包工程信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetTempProjectList([FromBody]JObject json) {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }

                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                string deptid = dy.data.deptId;
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "b.id bid";
                pagination.p_fields = @"b.engineername,b.engineerletdept,b.address,(select count(1) from epg_workmeetingmeasures m where m.workmeetingid=b.id)  worknum,realpernum,risklevel,d.fullname untiname";
                pagination.p_tablename = @"bis_workmeeting b left join epg_outsouringengineer e on e.id=b.engineerid left join base_department d on d.departmentid=e.outprojectid";
                pagination.sidx = "b.meetingdate ";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = "b.iscommit=1 and b.meetingtype='开工会' and to_char(b.meetingdate,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd') and b.engineerid in(select e.id from epg_outsouringengineer e where e.engineertype='002')";
                string role = curUser.RoleName;
                if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
                {
                    pagination.conditionJson += string.Format(" and b.createuserorgcode  = '{0}'", curUser.OrganizeCode);
                }
                else
                    if (role.Contains("承包商级用户"))
                    {
                        pagination.conditionJson += string.Format(" and b.engineerid in (select e.id from epg_outsouringengineer e where e.engineertype = '002' and e.outprojectid='{0}')", curUser.DeptId);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and b.engineerid in (select e.id from epg_outsouringengineer e where e.engineertype = '002' and e.engineerletdeptid='{0}') ", curUser.DeptId);
                    }
                if (!string.IsNullOrWhiteSpace(deptid))
                {
                    pagination.conditionJson += string.Format(" and b.engineerid in (select e.id from epg_outsouringengineer e where e.engineertype = '002' and e.engineerletdeptid='{0}')", deptid);
                }
                var data = workMeetingbll.GetPageList(pagination, null);
                //今日临时外包工程数量
                var projectnum = new WorkMeetingBLL().GetTodayTempProject(curUser);
                //今日开工会数量
                var meetingnum = data.Rows.Count;
                var worktasknum = 0;
                var workpeoplenum = 0;
                //今日工作任务数量
                if (data.Rows.Count > 0)
                {
                    worktasknum = Convert.ToInt32(data.Compute("Sum(worknum)", ""));
                    workpeoplenum = Convert.ToInt32(data.Compute("Sum(realpernum)", ""));
                }

                var JsonData = new
                {
                    data = data,
                    projectnum = projectnum,
                    meetingnum = meetingnum,
                    worktasknum = worktasknum,
                    workpeoplenum = workpeoplenum
                };
                return new { code = 0, count = pagination.records, info = "获取数据成功", data = JsonData };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }
          
        }
        /// <summary>
        /// 获取工作任务
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkTaskList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                string Workmeetingid = dy.data.workmeetingid;
                var data = new WorkmeetingmeasuresBLL().GetList("").Where(x => x.Workmeetingid == Workmeetingid).Select(x => new
                {
                    WorkTask = x.WorkTask,
                    DangerPoint = x.DangerPoint,
                    Measures = x.Measures,
                    Remark1 = x.Remark1
                }).ToList();
                return new { code = 0, count = data.Count, info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }
           
        }
        /// <summary>
        /// 西塞山大屏使用-根据责任部门分组获取今日临时外包工程
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetTempProjectData([FromBody]JObject json) {

            ////string res = json.Value<string>("json");
            ////dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            List<TempProjectData> tempData = new List<TempProjectData>();
            try
            {
                string sql = string.Format(@"select b.engineerletdept,e.engineerletdeptid,sum(b.realpernum) realpernum
                                              from bis_workmeeting b
                                              left join epg_outsouringengineer e on e.id=b.engineerid
                                         where b.iscommit=1 and b.meetingtype = '开工会' and to_char(b.meetingdate,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd') and b.engineerid in 
                                         (select e.id from epg_outsouringengineer e where e.engineertype = '002') and  b.id not in(select startmeetingid from bis_workmeeting where  meetingtype = '收工会' and startmeetingid is not null)
                                         group by b.engineerletdept,e.engineerletdeptid");
                var data = workMeetingbll.GetTable(sql);
                var totalProNum = 0;
                var totalPersonNum = 0;
                if (data.Rows.Count > 0) {
                     totalPersonNum = Convert.ToInt32(data.Compute("Sum(realpernum)", ""));
                }
             
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    TempProjectData itemData = new TempProjectData();
                    List<ProEntity> ProList = new List<ProEntity>();
                    itemData.DeptName = data.Rows[i]["engineerletdept"].ToString();
                    itemData.DeptId = data.Rows[i]["engineerletdeptid"].ToString();
                    itemData.RealperNum = Convert.ToInt32(data.Rows[i]["realpernum"].ToString());
                    string sqlWhere = string.Format(@"select distinct b.engineerid
                                          from bis_workmeeting b
                                         where b.iscommit=1 and b.meetingtype = '开工会'
                                           and to_char(b.meetingdate, 'yyyy-MM-dd') =
                                               to_char(sysdate, 'yyyy-MM-dd') 
                                           and b.engineerid in (select e.id
                                                                  from epg_outsouringengineer e
                                                                 where e.engineertype = '002' and e.engineerletdeptid='{0}')", data.Rows[i]["engineerletdeptid"].ToString());
                    itemData.ProNum = workMeetingbll.GetTable(sqlWhere).Rows.Count;
                    totalProNum += itemData.ProNum;
                    string Sql1 = string.Format(@" select b.id,b.engineername,b.engineerletdept,b.address,realpernum,risklevel,e.engineerletpeople,e.outprojectid
                                                             from bis_workmeeting b
                                                             left join epg_outsouringengineer e on e.id=b.engineerid        
                                                             where b.iscommit=1 and b.meetingtype='开工会' and to_char(b.meetingdate,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd') and b.engineerid in( select e.id from epg_outsouringengineer e where e.engineertype='002' and  b.id not in(select startmeetingid from bis_workmeeting where  meetingtype = '收工会' and startmeetingid is not null)
                                                            and e.engineerletdeptid='{0}')", data.Rows[i]["engineerletdeptid"].ToString());
                    var dt = workMeetingbll.GetTable(Sql1);
                    foreach (DataRow item in dt.Rows)
                    {
                        ProEntity pro = new ProEntity();
                        List<WorkEntity> workList = new List<WorkEntity>();
                        pro.Address = item["address"].ToString();
                        pro.DeptName = data.Rows[i]["engineerletdept"].ToString();
                        pro.DeptPersonName = item["engineerletpeople"].ToString();
                        pro.ProName = item["engineername"].ToString();
                        pro.RealperNum = Convert.ToInt32(item["realpernum"].ToString());
                        pro.RiskLevel = item["risklevel"].ToString();
                        pro.UnitName = new DepartmentBLL().GetEntity(item["outprojectid"].ToString()) == null ? "" : new DepartmentBLL().GetEntity(item["outprojectid"].ToString()).FullName;
                        pro.Meetingid = item["id"].ToString();
                        var list = new WorkmeetingmeasuresBLL().GetList("").Where(x => x.Workmeetingid == item["id"].ToString()).Select(x => new
                        {
                            x.WorkTask,
                            x.DangerPoint,
                            x.Measures,
                            x.Remark1
                        }).ToList();

                        foreach (var workitem in list)
                        {
                            WorkEntity e = new WorkEntity();
                            e.DangerPoint = workitem.DangerPoint;
                            e.WorkTask = workitem.WorkTask;
                            e.Measures = workitem.Measures;
                            e.WorkAddress = workitem.Remark1;
                            workList.Add(e);
                        }
                        pro.workList = workList;
                        ProList.Add(pro);
                    }
                    itemData.ProList = ProList;
                    tempData.Add(itemData);
                }
                var jsonData = new
                {
                    tempData = tempData,
                    totalProNum = totalProNum,
                    totalPersonNum = totalPersonNum
                };
                return new { code = 0, count = tempData.Count, info = "获取数据成功", data = jsonData };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }
          
        }

        /// <summary>
        /// 西塞山大屏使用-获取今日临时外包工程数量
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetTempProjectCount()
        {

            ////string res = json.Value<string>("json");
            ////dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            List<TempProjectData> tempData = new List<TempProjectData>();
            try
            {
                string sql = string.Format(@"select count(b.id) as num
                                              from bis_workmeeting b
                                              left join epg_outsouringengineer e on e.id=b.engineerid
                                         where b.iscommit=1 and b.meetingtype = '开工会' and to_char(b.meetingdate,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd') and b.engineerid in 
                                         (select e.id from epg_outsouringengineer e where e.engineertype = '002') and  b.id not in(select startmeetingid from bis_workmeeting where  meetingtype = '收工会' and startmeetingid is not null)");
                var data = workMeetingbll.GetTable(sql);
                var totalProNum = 0;
                if (data.Rows.Count > 0)
                {
                    totalProNum = Convert.ToInt32(data.Rows[0][0].ToString());
                }
                return new { code = 0, count = tempData.Count, info = "获取数据成功", data = totalProNum };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }

    }
    public class TempProjectData {
        /// <summary>
        /// 责任部门
        /// </summary>
        public string DeptName { get; set; }
        public string DeptId { get; set; }
        /// <summary>
        /// 施工人数
        /// </summary>
        public int RealperNum { get; set; }
        /// <summary>
        /// 工程数量
        /// </summary>
        public int ProNum { get; set; }

        public List<ProEntity> ProList = new List<ProEntity>();
    }
    public class ProEntity {
        public string Meetingid { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        public string  ProName { get; set; }
        /// <summary>
        /// 外包单位
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 施工地点
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 风险等级
        /// </summary>
        public string RiskLevel { get; set; }
        /// <summary>
        /// 施工人数
        /// </summary>
        public int RealperNum { get; set; }
        /// <summary>
        /// 责任部门负责人
        /// </summary>
        public string DeptPersonName { get; set; }

        public List<WorkEntity> workList = new List<WorkEntity>();
    }
    public class WorkEntity {
        /// <summary>
        /// 作业内容
        /// </summary>
        public string WorkTask { get; set; }
        /// <summary>
        /// 存在风险
        /// </summary>
        public string DangerPoint { get; set; }
        /// <summary>
        /// 预控措施
        /// </summary>
        public string Measures { get; set; }
        /// <summary>
        /// 作业地点
        /// </summary>
        public string WorkAddress { get; set; }
    }
    public class Submit<T>
    {
        public string userId { get; set; }
        public string tokenId { get; set; }
        public T data { get; set; }
    }
}
