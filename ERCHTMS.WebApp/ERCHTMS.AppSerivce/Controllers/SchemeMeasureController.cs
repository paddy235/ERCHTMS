using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.Busines.OutsourcingProject;
using System.Data;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.PublicInfoManage;
using System.IO;
using System.Web;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Data;
using System.Data.Common;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class SchemeMeasureController : BaseApiController
    {
        private SchemeMeasureBLL schemeMeasurebll = new SchemeMeasureBLL();
        private OutsouringengineerBLL outSouringengineerbll = new OutsouringengineerBLL();
        private HistorySchemeMeasureBLL historySchemeMeasurebll = new HistorySchemeMeasureBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private HistorySchemeMeasureBLL historyschememeasurebll = new HistorySchemeMeasureBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DataItemDetailBLL didBll = new DataItemDetailBLL();
        private DistrictBLL disBll = new DistrictBLL();
        private DepartmentBLL deptbll = new DepartmentBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private IntromissionBLL intromissionbll = new IntromissionBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private UserBLL userbll = new UserBLL();

        public HttpContext ctx { get { return HttpContext.Current; } }

        #region 获取三措两岸列表
        /// <summary>
        /// 获取三措两岸列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string startDate = dy.data.startDate; //开始时间
                string endDate = dy.data.endDate;//结束时间
                string projectId = dy.data.projectId; //外包工程Id
                string unitId = dy.data.unitId;//外包单位Id
                string senddeptId = dy.data.deptId;//发包部门Id
                string actiontype = dy.data.actiontype;//0全部 1 我的
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                string BELONGMAJOR = dy.data.BELONGMAJOR;
                string BELONGDEPTID = dy.data.BELONGDEPTID;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "s.id as appid";
                pagination.p_fields = @"s.issaved,s.submitdate,s.submitperson,s.projectid,s.organizer,s.organiztime,s.isover,s.engineerletdeptid as kbengineerletdeptid,
                                       s.flowdeptname,s.flowdept,s.flowrolename,s.flowname,nvl(s.ENGINEERNAME,e.engineername) engineername,e.outprojectid,b.fullname,
                                        case when isover='1' then '1'
                                          when isover ='0' and issaved='1' then '2'
                                            when isover='0' and issaved ='0' then '0' end status,
                                              case when isover='1' then '审核完成'
                                          when isover ='0' and issaved='1' then '待审核'
                                            when isover='0' and issaved ='0' then '待提交' end statusinfo,'' as approveuserid,'' as approveusername,s.flowid,s.projectid as outengineerid,'' as approveuserids ";
                pagination.p_tablename = @"epg_schememeasure s 
                                            left join epg_outsouringengineer e on e.id = s.projectid 
                                            left join base_department b on b.departmentid = e.outprojectid";
                pagination.sidx = "s.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                pagination.conditionJson += "1=1 ";



                string role = currUser.RoleName;
                string deptId = string.Empty;
                string deptName = string.Empty;

                //公司级用户取机构对象
                if (role.Contains("公司级用户"))
                {
                    deptId = currUser.OrganizeId;  //机构ID
                    deptName = currUser.OrganizeName;//机构名称
                }
                else
                {
                    deptId = currUser.DeptId; //部门ID
                    deptName = currUser.DeptName; //部门ID
                }
                if (actiontype == "0")
                {
                    //pagination.conditionJson += "";
                    string allrangedept = "";
                    try
                    {
                        allrangedept = dataitemdetailbll.GetDataItemByDetailCode("SBDept", "SBDeptId").FirstOrDefault().ItemValue;
                    }
                    catch (Exception)
                    {

                    }
                    if (currUser.IsSystem)
                    {

                    }
                    else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户") || allrangedept.Contains(currUser.DeptId))
                    {
                        pagination.conditionJson += string.Format(" and (s.createuserorgcode='{0}'", currUser.OrganizeCode);
                    }
                    else if (currUser.RoleName.Contains("承包商级用户"))
                    {
                        pagination.conditionJson += string.Format(" and ((b.departmentid ='{0}' or e.supervisorid='{0}')", currUser.DeptId);
                    }
                    else
                    {
                        var deptentity = departmentbll.GetEntity(currUser.DeptId);
                        while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                        {
                            deptentity = departmentbll.GetEntity(deptentity.ParentId);
                        }
                        pagination.conditionJson += string.Format(" and (  e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%')  ", deptentity.EnCode, currUser.UserId);

                        //pagination.conditionJson += string.Format(" and e.engineerletdeptid ='{0}' ", currUser.DeptId);
                    }
                    //(s.issaved = '0' and s.createuserid ='{0}')

                    pagination.conditionJson += " and s.issaved = '1'";
                    // 康巴什里所有人可以看到手动输入的工程申请后的数据(工程id为null 提交后的数据) 2020/12/02
                    pagination.conditionJson += string.Format(" or (s.PROJECTID is null and s.ISSAVED =1 ))");
                }
                else if (actiontype == "1")
                {
                    string strCondition = "";
                    strCondition = string.Format(" and s.createuserorgcode='{0}' and s.isover='0' and s.issaved='1'", currUser.OrganizeCode);
                    DataTable dt = intromissionbll.GetDataTableBySql("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var engineerEntity = outsouringengineerbll.GetEntity(dt.Rows[i]["outengineerid"].ToString());
                        //var excutdept =departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                        //var outengineerdept = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                        //var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                        ////获取下一步审核人
                        //string str = manypowercheckbll.GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["appid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                        //dt.Rows[i]["approveuserids"] = str;
                        if (engineerEntity != null)
                        {
                            var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                            var outengineerdept1 = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                            var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                            //获取下一步审核人
                            string str = manypowercheckbll.GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept1, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                            dt.Rows[i]["approveuserids"] = str;
                        }
                        else
                        {
                            string str = manypowercheckbll.GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["appid"].ToString(), "", "", dt.Rows[i]["kbengineerletdeptid"].ToString(), "", "", "", "", "", "");
                            dt.Rows[i]["approveuserids"] = str;
                        }
                    }

                    string[] applyids = dt.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("appid")).ToArray();

                    pagination.conditionJson += string.Format(" and (s.id in ('{0}')", string.Join("','", applyids), strCondition);
                    pagination.conditionJson += string.Format(" or (s.issaved = '0' and s.createuserid ='{0}'))", currUser.UserId);
                }
                else if (actiontype == "2")//待我审核审批
                {
                    string strCondition = "";
                    strCondition = string.Format(" and s.createuserorgcode='{0}' and s.isover='0' and s.issaved='1'", currUser.OrganizeCode);
                    DataTable dt = intromissionbll.GetDataTableBySql("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var engineerEntity = outsouringengineerbll.GetEntity(dt.Rows[i]["outengineerid"].ToString());
                        //var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                        //var outengineerdept = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                        //var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                        ////获取下一步审核人
                        //string str = manypowercheckbll.GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["appid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                        //dt.Rows[i]["approveuserids"] = str;
                        if (engineerEntity != null)
                        {
                            var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                            var outengineerdept1 = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                            var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                            //获取下一步审核人
                            string str = manypowercheckbll.GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept1, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                            dt.Rows[i]["approveuserids"] = str;
                        }
                        else
                        {
                            string str = manypowercheckbll.GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["appid"].ToString(), "", "", dt.Rows[i]["kbengineerletdeptid"].ToString(), "", "", "", "", "", "");
                            dt.Rows[i]["approveuserids"] = str;
                        }
                    }

                    string[] applyids = dt.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("appid")).ToArray();

                    pagination.conditionJson += string.Format(" and s.id in ('{0}')", string.Join("','", applyids), strCondition);
                }
                else//已审核
                {
                    if (currUser.IsSystem)
                    {

                    }
                    else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                    {
                        pagination.conditionJson += string.Format(" and s.createuserorgcode='{0}'", currUser.OrganizeCode);
                    }
                    else if (currUser.RoleName.Contains("承包商级用户"))
                    {
                        pagination.conditionJson += string.Format(" and (b.departmentid ='{0}' or e.supervisorid='{0}')", currUser.DeptId);
                    }
                    else
                    {
                        var deptentity = departmentbll.GetEntity(currUser.DeptId);
                        while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                        {
                            deptentity = departmentbll.GetEntity(deptentity.ParentId);
                        }
                        pagination.conditionJson = string.Format(" e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%')  ", deptentity.EnCode, currUser.UserId);

                        //pagination.conditionJson += string.Format(" and e.engineerletdeptid ='{0}' ", currUser.DeptId);
                    }
                    //(s.issaved = '0' and s.createuserid ='{0}')

                    pagination.conditionJson += " and s.issaved = '1' and s.isover='1' ";
                }

                if (!string.IsNullOrEmpty(startDate))
                {
                    pagination.conditionJson += string.Format(" and s.createdate>= to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", startDate);
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    pagination.conditionJson += string.Format(" and s.createdate<= to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", endDate);
                }
                if (!string.IsNullOrEmpty(projectId))
                {
                    pagination.conditionJson += string.Format(" and s.projectid = '{0}' ", projectId);
                }
                if (!string.IsNullOrEmpty(unitId))
                {
                    pagination.conditionJson += string.Format(" and e.outprojectid = '{0}' ", unitId);
                }
                if (!string.IsNullOrEmpty(senddeptId))
                {
                    pagination.conditionJson += string.Format(" and e.engineerletdeptid = '{0}' ", senddeptId);
                }

                if (!string.IsNullOrEmpty(BELONGMAJOR))
                {
                    pagination.conditionJson += string.Format(" and s.BELONGMAJOR ='{0}' ", BELONGMAJOR);
                }

                if (!string.IsNullOrEmpty(BELONGDEPTID))
                {
                    pagination.conditionJson += string.Format(" and s.BELONGDEPTID ='{0}' ", BELONGDEPTID);
                }

                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                });
                //// 康巴什里所有人可以看到手动输入的工程申请后的数据(工程id为null 提交后的数据) 2020/12/02
                //pagination.conditionJson += string.Format(" or (s.PROJECTID is null and s.ISSAVED =1 ) ");
                var data = schemeMeasurebll.GetList(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = outsouringengineerbll.GetEntity(data.Rows[i]["outengineerid"].ToString());

                    if (engineerEntity != null)
                    {
                        var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                        var outengineerdept1 = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                        var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                        //获取下一步审核人
                        string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept1, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
                        data.Rows[i]["approveuserids"] = str;
                    }
                    else
                    {
                        string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["appid"].ToString(), "", "", data.Rows[i]["kbengineerletdeptid"].ToString(), "", "", "", "", "", "");
                        data.Rows[i]["approveuserids"] = str;
                    }
                }
                foreach (DataRow row in data.Rows)
                {
                    row["approveuserid"] = row["approveuserids"].ToString();
                    row["approveusername"] = string.Join(",", userbll.GetUserTable(row["approveuserids"].ToString().Split(',')).AsEnumerable().Select(d => d.Field<string>("realname")).ToArray());
                }
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        ///// <summary>
        ///// 外包工程与外包单位,发包部门关联
        ///// </summary>
        ///// <param name="json"></param>
        ///// <returns></returns>
        //public object GetProjectDetail([FromBody]JObject json) {
        //    try
        //    {
        //        string res = json.Value<string>("json");
        //        dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
        //        string userId = dy.userid;
        //        string projectId = dy.projectId; //外包工程Id
        //        //获取用户基本信息
        //        OperatorProvider.AppUserId = userId;  //设置当前用户
        //        Operator user = OperatorProvider.Provider.Current();
        //        Pagination pagination = new Pagination();
        //        pagination.p_kid = "e.id as appid";
        //        pagination.p_fields = @"e.outprojectid,b.senddeptid,b.fullname,b.senddeptname";
        //        pagination.p_tablename = @"epg_outsouringengineer e
        //                                    left join base_department b on b.departmentid = e.outprojectid";
        //        pagination.sidx = "e.outprojectid";//排序字段
        //        pagination.sord = "desc";//排序方式
        //        pagination.conditionJson += "1=1 ";

        //        pagination.conditionJson += string.Format(" and e.outprojectid='{0}'", projectId);

        //        string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
        //        {
        //            ProjectId = projectId
        //        });
        //        var data = schemeMeasurebll.GetList(pagination, queryJson);
        //        return new { Code = 0, Count = data.Rows.Count, Info = "获取数据成功", data = data };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { Code = -1, Count = 0, Info = ex.Message };
        //    }
        //}

        #region 获取历史记录
        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHistory([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string contractid = dy.data.contractid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100;
                pagination.p_kid = "t.id";
                pagination.p_fields = @"  r.id as engineerid,e.fullname,nvl(t.ENGINEERNAME,r.engineername) engineername,to_char(t.submitdate,'yyyy-mm-dd') as submitdate,t.submitperson,
                                     (select count(1) from base_fileinfo o where o.recid=t.id) as filenum,t.createuserid ,t.issaved,t.isover,t.flowdeptname,t.flowdept,t.flowrolename,t.flowrole,t.flowname,t.organizer,t.organiztime";
                pagination.p_tablename = @" epg_historyschememeasure t 
                                        left join epg_outsouringengineer r  on t.projectid=r.id 
                                        left join base_department e on r.outprojectid=e.departmentid";
                string role = user.RoleName;
                if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
                {
                    pagination.conditionJson = string.Format(" t.createuserorgcode  = '{0}'", user.OrganizeCode);
                }
                else if (role.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format("(e.departmentid = '{0}' or t.PROJECTID is null)", user.DeptId);
                }
                else
                {
                    pagination.conditionJson = string.Format(" (r.engineerletdeptid = '{0}' or t.PROJECTID is null)", user.DeptId);
                }

                //关联ID
                if (!string.IsNullOrEmpty(contractid))
                {
                    pagination.conditionJson += string.Format(" and t.contractid = '{0}'", contractid);
                }

                pagination.conditionJson += string.Format(" or  (t.createuserid = '{0}'", user.UserId);

                //关联ID
                if (!string.IsNullOrEmpty(contractid))
                {
                    pagination.conditionJson += string.Format(" and t.contractid = '{0}'", contractid);
                }

                pagination.conditionJson += ")";
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    userid = userId
                });
                var data = schemeMeasurebll.GetList(pagination, queryJson);
                return new { Code = 0, Count = data.Rows.Count, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取历史记录详情
        /// <summary>
        /// 获取历史纪录详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHistoryDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.id;
                string levename = "";
                string belongmajname = "";
                string ENGINEERTYPENAME = "";
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var schemeMeasureEntity = historySchemeMeasurebll.GetEntity(id);//获取三措两岸历史记录表
                var outsouringengineerEntity = outSouringengineerbll.GetEntity(schemeMeasureEntity.PROJECTID);//获取外包工程信息
                //if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.ENGINEERAREA))
                //{
                //    var area = disBll.GetEntity(outsouringengineerEntity.ENGINEERAREA);
                //    if (area != null)
                //        outsouringengineerEntity.ENGINEERAREANAME = area.DistrictName;
                //}
                //if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.ENGINEERTYPE))
                //{
                //    var listType = didBll.GetDataItem("ProjectType", outsouringengineerEntity.ENGINEERTYPE).ToList();
                //    if (listType != null && listType.Count > 0)
                //        outsouringengineerEntity.ENGINEERTYPENAME = listType[0].ItemName;
                //}
                //if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.ENGINEERLEVEL))
                //{
                //    var listLevel = didBll.GetDataItem("ProjectLevel", outsouringengineerEntity.ENGINEERLEVEL).ToList();
                //    if (listLevel != null && listLevel.Count > 0)
                //        outsouringengineerEntity.ENGINEERLEVELNAME = listLevel[0].ItemName;
                //}
                //if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.OUTPROJECTID))
                //{
                //    var dept = deptbll.GetEntity(outsouringengineerEntity.OUTPROJECTID);
                //    outsouringengineerEntity.OUTPROJECTCODE = dept.EnCode;
                //    outsouringengineerEntity.OUTPROJECTNAME = dept.FullName;
                //}

                if (outsouringengineerEntity != null)
                {
                    if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.ENGINEERTYPE))
                    {
                        var listType = didBll.GetDataItem("ProjectType", outsouringengineerEntity.ENGINEERTYPE).ToList();
                        if (listType != null && listType.Count > 0)
                            outsouringengineerEntity.ENGINEERTYPENAME = listType[0].ItemName;
                    }
                    if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.ENGINEERLEVEL))
                    {
                        var listLevel = didBll.GetDataItem("ProjectLevel", outsouringengineerEntity.ENGINEERLEVEL).ToList();
                        if (listLevel != null && listLevel.Count > 0)
                            outsouringengineerEntity.ENGINEERLEVELNAME = listLevel[0].ItemName;
                    }
                    if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.OUTPROJECTID))
                    {
                        var dept = deptbll.GetEntity(outsouringengineerEntity.OUTPROJECTID);
                        outsouringengineerEntity.OUTPROJECTCODE = dept.EnCode;
                        outsouringengineerEntity.OUTPROJECTNAME = dept.FullName;
                    }
                    levename = outsouringengineerEntity.ENGINEERLEVELNAME;
                    ENGINEERTYPENAME = outsouringengineerEntity.ENGINEERTYPENAME;
                }
                else
                {
                    var listLevel = didBll.GetDataItem("ProjectLevel", schemeMeasureEntity.ENGINEERLEVEL).ToList();
                    if (listLevel != null && listLevel.Count > 0)
                        levename = listLevel[0].ItemName;

                    var listType = didBll.GetDataItem("ProjectType", schemeMeasureEntity.ENGINEERTYPE).ToList();
                    if (listType != null && listType.Count > 0)
                        ENGINEERTYPENAME = listType[0].ItemName;
                }

                var listmaj = didBll.GetDataItem("BelongMajor", schemeMeasureEntity.BELONGMAJOR).ToList();
                if (listmaj != null && listmaj.Count > 0)
                    belongmajname = listmaj[0].ItemName;

                var files = new FileInfoBLL().GetFiles(id);//获取相关附件
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in files.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }

                List<AptitudeinvestigateauditEntity> AptitudeList = aptitudeinvestigateauditbll.GetAuditList(schemeMeasureEntity.ID);
                for (int i = 0; i < AptitudeList.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(AptitudeList[i].AUDITSIGNIMG)) AptitudeList[i].AUDITSIGNIMG = string.Empty;
                    else
                        AptitudeList[i].AUDITSIGNIMG = webUrl + AptitudeList[i].AUDITSIGNIMG.ToString().Replace("../../", "/").ToString();
                }
                return new
                {
                    Code = 0,
                    Count = 1,
                    Info = "获取数据成功",
                    data = new
                    {
                        keyvalue = schemeMeasureEntity.ID,
                        projectid = schemeMeasureEntity.PROJECTID,
                        organizer = schemeMeasureEntity.ORGANIZER,
                        organiztime = schemeMeasureEntity.ORGANIZTIME,
                        submitdate = schemeMeasureEntity.SUBMITDATE,
                        submitperson = schemeMeasureEntity.SUBMITPERSON,
                        //engineername = outsouringengineerEntity.ENGINEERNAME,
                        //outprojectid = outsouringengineerEntity.OUTPROJECTID,
                        //outprojectname = outsouringengineerEntity.OUTPROJECTNAME,
                        //engineerletdeptid = outsouringengineerEntity.ENGINEERLETDEPTID,
                        //engineerletdept = outsouringengineerEntity.ENGINEERLETDEPT,
                        //engineercode = outsouringengineerEntity.ENGINEERCODE,
                        //engineerarea = outsouringengineerEntity.ENGINEERAREA,
                        //engineerareaname = outsouringengineerEntity.EngAreaName,
                        //engineertype = outsouringengineerEntity.ENGINEERTYPE,
                        //engineertypename = outsouringengineerEntity.ENGINEERTYPENAME,
                        //engineerlevel = outsouringengineerEntity.ENGINEERLEVEL,
                        //engineerlevelname = outsouringengineerEntity.ENGINEERLEVELNAME,
                        //engineercontent = outsouringengineerEntity.ENGINEERCONTENT,

                        engineername = schemeMeasureEntity.ENGINEERNAME,
                        outprojectid = outsouringengineerEntity != null ? outsouringengineerEntity.OUTPROJECTID : "",
                        outprojectname = outsouringengineerEntity != null ? outsouringengineerEntity.OUTPROJECTNAME : "",
                        engineerletdeptid = schemeMeasureEntity.ENGINEERLETDEPTID,
                        engineerletdept = schemeMeasureEntity.ENGINEERLETDEPT,
                        engineercode = schemeMeasureEntity.ENGINEERCODE,
                        engineerarea = schemeMeasureEntity.ENGINEERAREA,
                        engineerareaname = schemeMeasureEntity.ENGINEERAREANAME,
                        engineertype = schemeMeasureEntity.ENGINEERTYPE,
                        engineertypename = ENGINEERTYPENAME,
                        engineerlevel = schemeMeasureEntity.ENGINEERLEVEL,
                        engineerlevelname = levename,
                        engineercontent = schemeMeasureEntity.ENGINEERCONTENT,
                        SummitContent = schemeMeasureEntity.SummitContent,
                        belongmajorname = belongmajname,

                        engineerletdeptcode = schemeMeasureEntity.ENGINEERLETDEPTCODE,
                        belongmajor = schemeMeasureEntity.BELONGMAJOR,
                        belongdeptname = schemeMeasureEntity.BELONGDEPTNAME,
                        belongdeptid = schemeMeasureEntity.BELONGDEPTID,
                        belongcode = schemeMeasureEntity.BELONGCODE,
                        Files = files,
                        AuditInfo = AptitudeList
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取外包工程列表
        /// <summary>
        /// 获取外包工程列表
        /// </summary>
        /// <param name="json">mode=006</param>
        /// <returns></returns>
        [HttpPost]
        public object GetProjectList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string mode = dy.data.mode;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                DataTable dt = outSouringengineerbll.GetEngineerDataByCurrdeptId(currUser, mode);
                return new
                {
                    Code = 0,
                    Count = dt.Rows.Count,
                    Info = "获取数据成功",
                    data = dt
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取外包工程详细
        /// <summary>
        /// 获取外包工程详细
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetProjectDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string keyValue = dy.keyvalue;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                var outsouringengineerEntity = outSouringengineerbll.GetEntity(keyValue);//获取外包工程信息
                return new
                {
                    Code = 0,
                    Count = 1,
                    Info = "获取数据成功",
                    data = new
                    {
                        item = outsouringengineerEntity
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 获取表单
        /// <summary>
        /// 获取表单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetForm([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.id;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var schemeMeasureEntity = schemeMeasurebll.GetEntity(id);
                string levename = "";
                string ENGINEERTYPENAME = "";
                string belongmajname = "";


                var outsouringengineerEntity = outSouringengineerbll.GetEntity(schemeMeasureEntity.PROJECTID);//获取外包工程信息
                //if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.ENGINEERAREA))
                //{
                //    var area = disBll.GetEntity(outsouringengineerEntity.ENGINEERAREA);
                //    if (area != null)
                //        outsouringengineerEntity.ENGINEERAREANAME = area.DistrictName;
                //}
                if (outsouringengineerEntity != null)
                {
                    if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.ENGINEERTYPE))
                    {
                        var listType = didBll.GetDataItem("ProjectType", outsouringengineerEntity.ENGINEERTYPE).ToList();
                        if (listType != null && listType.Count > 0)
                            outsouringengineerEntity.ENGINEERTYPENAME = listType[0].ItemName;
                    }
                    if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.ENGINEERLEVEL))
                    {
                        var listLevel = didBll.GetDataItem("ProjectLevel", outsouringengineerEntity.ENGINEERLEVEL).ToList();
                        if (listLevel != null && listLevel.Count > 0)
                            outsouringengineerEntity.ENGINEERLEVELNAME = listLevel[0].ItemName;
                    }
                    if (!string.IsNullOrWhiteSpace(outsouringengineerEntity.OUTPROJECTID))
                    {
                        var dept = deptbll.GetEntity(outsouringengineerEntity.OUTPROJECTID);
                        outsouringengineerEntity.OUTPROJECTCODE = dept.EnCode;
                        outsouringengineerEntity.OUTPROJECTNAME = dept.FullName;
                    }
                    levename = outsouringengineerEntity.ENGINEERLEVELNAME;
                    ENGINEERTYPENAME = outsouringengineerEntity.ENGINEERTYPENAME;
                }
                else
                {
                    var listLevel = didBll.GetDataItem("ProjectLevel", schemeMeasureEntity.ENGINEERLEVEL).ToList();
                    if (listLevel != null && listLevel.Count > 0)
                        levename = listLevel[0].ItemName;

                    var listType = didBll.GetDataItem("ProjectType", schemeMeasureEntity.ENGINEERTYPE).ToList();
                    if (listType != null && listType.Count > 0)
                        ENGINEERTYPENAME = listType[0].ItemName;


                }
                var listmaj = didBll.GetDataItem("BelongMajor", schemeMeasureEntity.BELONGMAJOR).ToList();
                if (listmaj != null && listmaj.Count > 0)
                    belongmajname = listmaj[0].ItemName;

                var files = new FileInfoBLL().GetFiles(id);//获取相关附件
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in files.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }

                List<AptitudeinvestigateauditEntity> AptitudeList = aptitudeinvestigateauditbll.GetAuditList(schemeMeasureEntity.ID);
                for (int i = 0; i < AptitudeList.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(AptitudeList[i].AUDITSIGNIMG)) AptitudeList[i].AUDITSIGNIMG = string.Empty;
                    else
                        AptitudeList[i].AUDITSIGNIMG = webUrl + AptitudeList[i].AUDITSIGNIMG.ToString().Replace("../../", "/").ToString();
                }
                //查询审核流程图
                List<CheckFlowData> nodeList = new AptitudeinvestigateinfoBLL().GetAppFlowList(id, "5", curUser);
                return new
                {
                    Code = 0,
                    Count = 1,
                    Info = "获取数据成功",
                    data = new
                    {
                        summitcontent = schemeMeasureEntity.SummitContent,
                        keyvalue = schemeMeasureEntity.ID,
                        projectid = schemeMeasureEntity.PROJECTID,
                        organizer = schemeMeasureEntity.ORGANIZER,
                        organiztime = schemeMeasureEntity.ORGANIZTIME,
                        submitdate = schemeMeasureEntity.SUBMITDATE,
                        submitperson = schemeMeasureEntity.SUBMITPERSON,
                        engineername = schemeMeasureEntity.ENGINEERNAME,
                        outprojectid = outsouringengineerEntity != null ? outsouringengineerEntity.OUTPROJECTID : "",
                        outprojectname = outsouringengineerEntity != null ? outsouringengineerEntity.OUTPROJECTNAME : "",
                        engineerletdeptid = schemeMeasureEntity.ENGINEERLETDEPTID,
                        engineerletdept = schemeMeasureEntity.ENGINEERLETDEPT,
                        engineercode = schemeMeasureEntity.ENGINEERCODE,
                        engineerarea = schemeMeasureEntity.ENGINEERAREA,
                        engineerareaname = schemeMeasureEntity.ENGINEERAREANAME,
                        engineertype = schemeMeasureEntity.ENGINEERTYPE,
                        engineertypename = ENGINEERTYPENAME,
                        engineerlevel = schemeMeasureEntity.ENGINEERLEVEL,
                        engineerlevelname = levename,
                        engineercontent = schemeMeasureEntity.ENGINEERCONTENT,
                        SummitContent = schemeMeasureEntity.SummitContent == "null" ? "" : schemeMeasureEntity.SummitContent,

                        //engineercode = schemeMeasureEntity.ENGINEERCODE,
                        //engineername = schemeMeasureEntity.ENGINEERNAME,
                        //engineertype = schemeMeasureEntity.ENGINEERTYPE,
                        //engineerareaname = schemeMeasureEntity.ENGINEERAREANAME,
                        //engineerlevel = schemeMeasureEntity.ENGINEERLEVEL,
                        //engineerletdept = schemeMeasureEntity.ENGINEERLETDEPT,
                        //engineerletdeptid = schemeMeasureEntity.ENGINEERLETDEPTID,
                        engineerletdeptcode = schemeMeasureEntity.ENGINEERLETDEPTCODE,
                        belongmajor = schemeMeasureEntity.BELONGMAJOR,
                        belongmajorname = belongmajname,
                        belongdeptname = schemeMeasureEntity.BELONGDEPTNAME,
                        belongdeptid = schemeMeasureEntity.BELONGDEPTID,
                        belongcode = schemeMeasureEntity.BELONGCODE,
                        //engineerarea = schemeMeasureEntity.ENGINEERAREA,
                        //engineercontent = schemeMeasureEntity.ENGINEERCONTENT,
                        Files = files,
                        AuditInfo = AptitudeList,
                        nodeList = nodeList
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取审核记录
        /// <summary>
        /// 获取审核记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAuditList([FromBody]JObject json)
        {
            try
            {
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.id;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                List<AptitudeinvestigateauditEntity> data = aptitudeinvestigateauditbll.GetAuditList(id);
                for (int i = 0; i < data.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(data[i].AUDITSIGNIMG.ToString()))
                    {
                        data[i].AUDITSIGNIMG = string.Empty;
                    }
                    else
                    {
                        data[i].AUDITSIGNIMG = webUrl + data[i].AUDITSIGNIMG.ToString().Replace("../../", "/");
                    }
                }
                return new
                {
                    Code = 0,
                    Count = 1,
                    Info = "获取数据成功",
                    data = new
                    {
                        auditList = data
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 提交表单
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SubmitForm()
        {
            try
            {
                string res = HttpContext.Current.Request["json"]; ;
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyValue = dy.data.keyvalue;
                string userid = dy.userid;
                string fileid = dy.data.fileid;
                SchemeMeasureEntity entity = new SchemeMeasureEntity
                {
                    PROJECTID = dy.data.PROJECTID,
                    SUBMITDATE = Convert.ToDateTime(dy.data.SUBMITDATE),
                    SUBMITPERSON = dy.data.SUBMITPERSON,
                    ORGANIZER = dy.data.ORGANIZER,
                    ORGANIZTIME = Convert.ToDateTime(dy.data.ORGANIZTIME),
                    SummitContent = dy.data.SummitContent,

                    ENGINEERCODE = dy.data.ENGINEERCODE,
                    ENGINEERNAME = dy.data.ENGINEERNAME,
                    ENGINEERTYPE = dy.data.ENGINEERTYPE,
                    ENGINEERAREANAME = dy.data.ENGINEERAREANAME,
                    ENGINEERLEVEL = dy.data.ENGINEERLEVEL,
                    ENGINEERLETDEPT = dy.data.ENGINEERLETDEPT,
                    ENGINEERLETDEPTID = dy.data.ENGINEERLETDEPTID,
                    ENGINEERLETDEPTCODE = dy.data.ENGINEERLETDEPTCODE,
                    ENGINEERAREA = dy.data.ENGINEERAREA,
                    ENGINEERCONTENT = dy.data.ENGINEERCONTENT,
                    BELONGMAJOR = dy.data.BELONGMAJOR,
                    BELONGDEPTNAME = dy.data.BELONGDEPTNAME,
                    BELONGDEPTID = dy.data.BELONGDEPTID,
                    BELONGCODE = dy.data.BELONGCODE
                };
                OperatorProvider.AppUserId = userid;
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

                string state = string.Empty;
                string flowid = string.Empty;
                string moduleName = "三措两案";

                string outengineerid = entity.PROJECTID;
                //新增时会根据角色自动审核,此时需根据工程和审核配置查询审核流程Id
                OutsouringengineerEntity engineerEntity = new OutsouringengineerBLL().GetEntity(entity.PROJECTID);
                // 如果工程id是空，说明是输入类型，走新的流程
                if (entity.PROJECTID == null || entity.PROJECTID == "")
                {
                    moduleName = "三措两案_手输工程";
                }
                else
                {
                    if (dataitemdetailbll.GetDataItemListByItemCode("FlowWithRiskLevel").Count() > 0)
                    {
                        switch (engineerEntity.ENGINEERLEVEL)
                        {
                            case "001":
                                moduleName = "三措两案_一级风险";
                                break;
                            case "002":
                                moduleName = "三措两案_二级风险";
                                break;
                            case "003":
                                moduleName = "三措两案_三级风险";
                                break;
                            case "004":
                                moduleName = "三措两案_四级风险";
                                break;
                            default:
                                break;
                        }
                    }
                }



                /// <param name="currUser">当前登录人</param>
                /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                /// <param name="moduleName">模块名称</param>
                /// <param name="outengineerid">工程Id</param>
                ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, outengineerid, true, "", entity.ENGINEERLETDEPTID);


                List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
                List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
                //先查出执行部门编码
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(engineerEntity.ENGINEERLETDEPTID).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    }

                    //外包单位
                    if (powerList[i].CHECKDEPTCODE == "-2" || powerList[i].CHECKDEPTID == "-2")
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(engineerEntity.OUTPROJECTID).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    }
                    //监理单位
                    if (powerList[i].CHECKDEPTCODE == "-6" || powerList[i].CHECKDEPTID == "-6")
                    {
                        if (!string.IsNullOrEmpty(engineerEntity.SupervisorId))
                        {
                            powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(engineerEntity.SupervisorId).EnCode;
                            powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                        }
                        else
                        {
                            powerList[i].CHECKDEPTCODE = "";
                            powerList[i].CHECKDEPTID = "";
                        }

                    }


                }
                //登录人是否有审核权限--有审核权限直接审核通过
                for (int i = 0; i < powerList.Count; i++)
                {

                    //外包单位
                    if (powerList[i].CHECKDEPTCODE == "-2" || powerList[i].CHECKDEPTID == "-2")
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(engineerEntity.OUTPROJECTID).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    }
                    //监理单位
                    if (powerList[i].CHECKDEPTCODE == "-6" || powerList[i].CHECKDEPTID == "-6")
                    {
                        if (!string.IsNullOrEmpty(engineerEntity.SupervisorId))
                        {
                            powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(engineerEntity.SupervisorId).EnCode;
                            powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                        }
                        else
                        {
                            powerList[i].CHECKDEPTCODE = "";
                            powerList[i].CHECKDEPTID = "";
                        }

                    }

                    // 跳级的时候脚本实行需要获取流程id
                    if (powerList[i].ApplyType == "1")
                    {
                        var parameter = new List<DbParameter>();
                        //取脚本，获取账户的范围信息
                        if (powerList[i].ScriptCurcontent.Contains("@outengineerid"))
                        {
                            parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(outengineerid) ? outengineerid : ""));
                        }
                        if (powerList[i].ScriptCurcontent.Contains("@engineerletdeptid"))
                        {
                            parameter.Add(DbParameters.CreateDbParameter("@engineerletdeptid", !string.IsNullOrEmpty(entity.ENGINEERLETDEPTID) ? entity.ENGINEERLETDEPTID : ""));
                        }
                        DbParameter[] arrayparam = parameter.ToArray();
                        var userIds = userbll.FindList(powerList[i].ScriptCurcontent, arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.UserId + ",")).Trim(',');
                        if (userIds.Contains(curUser.UserId))
                        {
                            checkPower.Add(powerList[i]);
                            //break;
                        }
                    }
                }
                if (checkPower.Count > 0)
                {
                    ManyPowerCheckEntity check = checkPower.Last();//当前

                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (check.ID == powerList[i].ID)
                        {
                            flowid = powerList[i].ID;
                        }
                    }
                }

                if (null != mpcEntity)
                {
                    //保存三措两案记录
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.ISSAVED = "1"; //标记已经从登记到审核阶段
                    entity.ISOVER = "0"; //流程未完成，1表示完成
                    entity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "审核中";
                    entity.FlowId = mpcEntity.ID;
                    if (mpcEntity.ApplyType == "1") //如果是脚本运行
                    {
                        var parameter = new List<DbParameter>();
                        //取脚本，获取账户的范围信息
                        if (mpcEntity.ScriptCurcontent.Contains("@outengineerid"))
                        {
                            parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(outengineerid) ? outengineerid : ""));
                        }
                        if (mpcEntity.ScriptCurcontent.Contains("@engineerletdeptid"))
                        {
                            parameter.Add(DbParameters.CreateDbParameter("@engineerletdeptid", !string.IsNullOrEmpty(entity.ENGINEERLETDEPTID) ? entity.ENGINEERLETDEPTID : ""));
                        }
                        DbParameter[] arrayparam = parameter.ToArray();
                        var userIds = userbll.FindList(mpcEntity.ScriptCurcontent, arrayparam);
                        var userAccount = "";
                        var userName = "";
                        int num = 1;
                        foreach (UserEntity info in userIds)
                        {
                            if (userIds.Count() == num)
                            {
                                userAccount += info.UserId;
                                userName += info.RealName;
                            }
                            else
                            {
                                userAccount += info.UserId + ",";
                                userName += info.RealName + ",";
                            }

                            num++;
                        }
                        if (userAccount != "")
                        {
                            JPushApi.PushMessage(userAccount, userName, "WB001", entity.ID);
                        }

                    }
                    else
                    {
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB001", entity.ID);
                    }


                }
                else  //为空则表示已经完成流程
                {
                    entity.FLOWDEPT = "";
                    entity.FLOWDEPTNAME = "";
                    entity.FLOWROLE = "";
                    entity.FLOWROLENAME = "";
                    entity.ISSAVED = "1"; //标记已经从登记到审核阶段
                    entity.ISOVER = "1"; //流程未完成，1表示完成
                    entity.FLOWNAME = "";
                    entity.FlowId = "";
                }
                HttpFileCollection files = HttpContext.Current.Request.Files;
                keyValue = string.IsNullOrEmpty(keyValue) ? Guid.NewGuid().ToString() : keyValue;
                if (!string.IsNullOrEmpty(fileid))
                {
                    DeleteFile(fileid);
                }
                string path = string.Empty;
                UploadifyFile(keyValue, files, ref path);

                schemeMeasurebll.SaveForm(keyValue, entity);

                //添加审核记录
                if (state == "1")
                {
                    //审核信息表
                    AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                    aidEntity.AUDITRESULT = "0"; //通过
                    aidEntity.AUDITTIME = DateTime.Now;
                    aidEntity.AUDITPEOPLE = curUser.UserName;
                    aidEntity.AUDITPEOPLEID = curUser.UserId;
                    aidEntity.APTITUDEID = entity.ID;  //关联的业务ID 
                    aidEntity.AUDITOPINION = ""; //审核意见
                    aidEntity.AUDITSIGNIMG = curUser.SignImg;
                    aidEntity.FlowId = flowid;
                    if (null != mpcEntity)
                    {
                        aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
                    }
                    else
                    {
                        aidEntity.REMARK = "7";
                    }
                    if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户"))
                    {
                        aidEntity.AUDITDEPTID = curUser.OrganizeId;
                        aidEntity.AUDITDEPT = curUser.OrganizeName;
                    }
                    else
                    {
                        aidEntity.AUDITDEPTID = curUser.DeptId;
                        aidEntity.AUDITDEPT = curUser.DeptName;
                    }
                    aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
                }

                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 审核表单
        /// <summary>
        /// 审核表单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ApproveForm()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string keyValue = dy.data.keyvalue;
                SchemeMeasureEntity entity = new SchemeMeasureEntity
                {
                    PROJECTID = dy.data.PROJECTID,
                    SUBMITDATE = Convert.ToDateTime(dy.data.SUBMITDATE),
                    SUBMITPERSON = dy.data.SUBMITPERSON,
                    ORGANIZER = dy.data.ORGANIZER,
                    ORGANIZTIME = Convert.ToDateTime(dy.data.ORGANIZTIME)
                };
                var reqParam = JsonConvert.DeserializeAnonymousType(res, new
                {
                    data = new
                    {
                        AUDITOPINION = string.Empty,
                        AUDITSIGNIMG = string.Empty
                    }
                });
                AptitudeinvestigateauditEntity aentity = new AptitudeinvestigateauditEntity
                {
                    AUDITRESULT = dy.data.AUDITRESULT,
                    AUDITTIME = Convert.ToDateTime(dy.data.AUDITTIME),
                    AUDITPEOPLE = dy.data.AUDITPEOPLE,
                    AUDITPEOPLEID = dy.data.AUDITPEOPLEID,
                    AUDITDEPTID = dy.data.AUDITDEPTID,
                    AUDITDEPT = dy.data.AUDITDEPT,
                    AUDITOPINION = reqParam.data.AUDITOPINION,
                    AUDITSIGNIMG = reqParam.data.AUDITSIGNIMG
                };
                OperatorProvider.AppUserId = userid;
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

                string state = string.Empty;

                string moduleName = "三措两案";
                //新增时会根据角色自动审核,此时需根据工程和审核配置查询审核流程Id
                OutsouringengineerEntity engineerEntity = new OutsouringengineerBLL().GetEntity(entity.PROJECTID);
                if (entity.PROJECTID == null || entity.PROJECTID == "")
                {
                    moduleName = "三措两案_手输工程";
                }
                else
                {
                    if (dataitemdetailbll.GetDataItemListByItemCode("FlowWithRiskLevel").Count() > 0)
                    {
                        switch (engineerEntity.ENGINEERLEVEL)
                        {
                            case "001":
                                moduleName = "三措两案_一级风险";
                                break;
                            case "002":
                                moduleName = "三措两案_二级风险";
                                break;
                            case "003":
                                moduleName = "三措两案_三级风险";
                                break;
                            case "004":
                                moduleName = "三措两案_四级风险";
                                break;
                            default:
                                break;
                        }
                    }
                }



                string outengineerid = entity.PROJECTID;
                var smEntity = schemeMeasurebll.GetEntity(keyValue);
                /// <param name="currUser">当前登录人</param>
                /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                /// <param name="moduleName">模块名称</param>
                /// <param name="outengineerid">工程Id</param>
                ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, outengineerid, true, "", smEntity.ENGINEERLETDEPTID);
                #region //审核信息表
                AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                aidEntity.AUDITRESULT = aentity.AUDITRESULT; //通过
                aidEntity.AUDITTIME = Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //审核时间
                aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //审核人员姓名
                aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//审核人员id
                aidEntity.APTITUDEID = keyValue;  //关联的业务ID 
                aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//审核部门id
                aidEntity.AUDITDEPT = aentity.AUDITDEPT; //审核部门
                aidEntity.AUDITOPINION = aentity.AUDITOPINION; //审核意见
                aidEntity.AUDITSIGNIMG = aentity.AUDITSIGNIMG;
                aidEntity.FlowId = smEntity.FlowId;
                if (null != mpcEntity)
                {
                    aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
                }
                else
                {
                    aidEntity.REMARK = "7";
                }
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fileOverName = System.IO.Path.GetFileName(file.FileName);
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string FileEextension = Path.GetExtension(file.FileName);
                        //if (fileName == aidEntity.ID)
                        //{
                        string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                        string newFileName = fileName + FileEextension;
                        string newFilePath = dir + "\\" + newFileName;
                        file.SaveAs(newFilePath);
                        aidEntity.AUDITSIGNIMG = "/Resource/sign/" + fileOverName;
                        break;
                        //}
                    }
                }
                aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
                #endregion
                #region  //保存三措两案记录

                //审核通过
                if (aentity.AUDITRESULT == "0")
                {
                    //0表示流程未完成，1表示流程结束
                    if (null != mpcEntity)
                    {
                        smEntity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        smEntity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        smEntity.FLOWROLE = mpcEntity.CHECKROLEID;
                        smEntity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        smEntity.ISSAVED = "1";
                        smEntity.ISOVER = "0";
                        smEntity.FlowId = mpcEntity.ID;
                        smEntity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "审核中";
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB001", entity.ID);
                    }
                    else
                    {
                        smEntity.FLOWDEPT = "";
                        smEntity.FLOWDEPTNAME = "";
                        smEntity.FLOWROLE = "";
                        smEntity.FLOWROLENAME = "";
                        smEntity.ISSAVED = "1";
                        smEntity.ISOVER = "1";
                        smEntity.FLOWNAME = "";
                        smEntity.FlowId = "";
                    }
                }
                else //审核不通过 
                {
                    smEntity.FLOWDEPT = "";
                    smEntity.FLOWDEPTNAME = "";
                    smEntity.FLOWROLE = "";
                    smEntity.FLOWROLENAME = "";
                    smEntity.ISSAVED = "0"; //处于登记阶段
                    smEntity.ISOVER = "0"; //是否完成状态赋值为未完成
                    smEntity.FLOWNAME = "";
                    smEntity.FlowId = "";
                    var applyUser = new UserBLL().GetEntity(smEntity.CREATEUSERID);
                    if (applyUser != null)
                    {
                        JPushApi.PushMessage(applyUser.Account, smEntity.CREATEUSERNAME, "WB002", entity.ID);
                    }
                }
                //更新三措两案基本状态信息
                schemeMeasurebll.SaveForm(keyValue, smEntity);
                #endregion

                #region    //审核不通过
                if (aentity.AUDITRESULT == "1")
                {
                    //添加历史记录
                    HistorySchemeMeasureEntity hsentity = new HistorySchemeMeasureEntity();
                    hsentity.CREATEUSERID = smEntity.CREATEUSERID;
                    hsentity.CREATEUSERDEPTCODE = smEntity.CREATEUSERDEPTCODE;
                    hsentity.CREATEUSERORGCODE = smEntity.CREATEUSERORGCODE;
                    hsentity.CREATEDATE = smEntity.CREATEDATE;
                    hsentity.CREATEUSERNAME = smEntity.CREATEUSERNAME;
                    hsentity.MODIFYDATE = smEntity.MODIFYDATE;
                    hsentity.MODIFYUSERID = smEntity.MODIFYUSERID;
                    hsentity.MODIFYUSERNAME = smEntity.MODIFYUSERNAME;
                    hsentity.SUBMITDATE = smEntity.SUBMITDATE;
                    hsentity.SUBMITPERSON = smEntity.SUBMITPERSON;
                    hsentity.PROJECTID = smEntity.PROJECTID;
                    hsentity.CONTRACTID = smEntity.ID; //关联ID
                    hsentity.ORGANIZER = smEntity.ORGANIZER;
                    hsentity.ORGANIZTIME = smEntity.ORGANIZTIME;
                    hsentity.ISOVER = smEntity.ISOVER;
                    hsentity.ISSAVED = smEntity.ISSAVED;
                    hsentity.FLOWDEPTNAME = smEntity.FLOWDEPTNAME;
                    hsentity.FLOWDEPT = smEntity.FLOWDEPT;
                    hsentity.FLOWROLENAME = smEntity.FLOWROLENAME;
                    hsentity.FLOWROLE = smEntity.FLOWROLE;
                    hsentity.FLOWNAME = smEntity.FLOWNAME;
                    hsentity.SummitContent = smEntity.SummitContent;
                    // 康巴什新增字段
                    hsentity.ENGINEERCODE = smEntity.ENGINEERCODE;
                    hsentity.ENGINEERNAME = smEntity.ENGINEERNAME;
                    hsentity.ENGINEERTYPE = smEntity.ENGINEERTYPE;
                    hsentity.ENGINEERAREANAME = smEntity.ENGINEERAREANAME;
                    hsentity.ENGINEERLEVEL = smEntity.ENGINEERLEVEL;
                    hsentity.ENGINEERLETDEPT = smEntity.ENGINEERLETDEPT;
                    hsentity.ENGINEERLETDEPTID = smEntity.ENGINEERLETDEPTID;
                    hsentity.ENGINEERLETDEPTCODE = smEntity.ENGINEERLETDEPTCODE;
                    hsentity.ENGINEERAREA = smEntity.ENGINEERAREA;
                    hsentity.ENGINEERCONTENT = smEntity.ENGINEERCONTENT;
                    hsentity.BELONGMAJOR = smEntity.BELONGMAJOR;
                    hsentity.BELONGDEPTNAME = smEntity.BELONGDEPTNAME;
                    hsentity.BELONGDEPTID = smEntity.BELONGDEPTID;
                    hsentity.BELONGCODE = smEntity.BELONGCODE;
                    hsentity.ID = "";

                    historyschememeasurebll.SaveForm(hsentity.ID, hsentity);

                    //获取当前业务对象的所有审核记录
                    var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                    //批量更新审核记录关联ID
                    foreach (AptitudeinvestigateauditEntity mode in shlist)
                    {
                        mode.APTITUDEID = hsentity.ID; //对应新的ID
                        aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                    }
                    //批量更新附件记录关联ID
                    var flist = fileinfobll.GetFiles(keyValue);
                    foreach (DataRow fmode in flist.Rows)
                    {
                        FileInfoEntity itemFile = new FileInfoEntity();
                        itemFile.FileName = fmode["FileName"].ToString();
                        itemFile.FilePath = fmode["filepath"].ToString();
                        itemFile.FileSize = fmode["filesize"].ToString();
                        itemFile.RecId = hsentity.ID; //对应新的ID
                        fileinfobll.SaveForm("", itemFile);
                    }
                }
                #endregion

                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 上传附件、删除附件
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="foldername"></param>
        /// <param name="fileList"></param>
        public void UploadifyFile(string folderId, HttpFileCollection fileList, ref string path)
        {
            try
            {
                if (fileList.Count > 0)
                {
                    foreach (string key in fileList.AllKeys)
                    {
                        if (key != "sign")
                        {
                            HttpPostedFile file = fileList[key];
                            //获取文件完整文件名(包含绝对路径)
                            //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                            string userId = OperatorProvider.Provider.Current().UserId;
                            string fileGuid = Guid.NewGuid().ToString();
                            long filesize = file.ContentLength;
                            string FileEextension = Path.GetExtension(file.FileName);
                            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\ResourceFile";
                            string newFileName = fileGuid + FileEextension;
                            string newFilePath = dir + "\\" + newFileName;
                            //创建文件夹
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            if (!System.IO.File.Exists(newFilePath))
                            {
                                //保存文件
                                file.SaveAs(newFilePath);
                                //文件信息写入数据库
                                fileInfoEntity.Create();
                                fileInfoEntity.FileId = fileGuid;
                                fileInfoEntity.RecId = folderId; //关联ID
                                fileInfoEntity.FileName = file.FileName;
                                fileInfoEntity.FilePath = "~/Resource/ResourceFile/" + newFileName;
                                fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                fileInfoEntity.FileExtensions = FileEextension;
                                fileInfoEntity.FileType = FileEextension.Replace(".", "");
                                fileinfobll.SaveForm("", fileInfoEntity);
                            }
                        }
                        else
                        {
                            HttpPostedFile file = fileList[key];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            //if (fileName == scEntity.ID)
                            //{
                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                            string newFileName = fileName + FileEextension;
                            string newFilePath = dir + "\\" + newFileName;
                            file.SaveAs(newFilePath);
                            //scEntity.SIGNPIC = "/Resource/sign/" + fileOverName;
                            //    break;
                            //}
                            path = fileOverName;
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
                    var entity = fileinfobll.GetEntity(s);
                    if (entity != null)
                    {
                        var filePath = ctx.Server.MapPath(entity.FilePath);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);
                }
                int count = fileinfobll.DeleteFileForm(ids);

                result = count > 0 ? true : false;
            }

            return result;
        }
        public void DeleteFileByRec(string recId)
        {
            if (!string.IsNullOrWhiteSpace(recId))
            {
                var list = fileinfobll.GetFileList(recId);
                foreach (var file in list)
                {
                    fileinfobll.RemoveForm(file.FileId);
                    var filePath = ctx.Server.MapPath(file.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
        }
        #endregion

        [HttpPost]
        public object GetSummitContent([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                var data = new DataItemDetailBLL().GetDataItemListByItemCode("'SummitContent'").Select(x => new { x.ItemName, x.ItemValue });
                return new
                {
                    Code = 0,
                    Count = data.Count(),
                    Info = "获取数据成功",
                    data = data
                };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
    }
}
