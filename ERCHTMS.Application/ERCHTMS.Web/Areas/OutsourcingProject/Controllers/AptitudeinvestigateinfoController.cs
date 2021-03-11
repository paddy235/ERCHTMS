using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using ERCHTMS.Busines.PublicInfoManage;
using System.Data;
using ERCHTMS.Entity.PublicInfoManage;
using System.Web;
using Aspose.Words;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：资质审查基础信息表
    /// </summary>
    public class AptitudeinvestigateinfoController : MvcControllerBase
    {
        private AptitudeinvestigateinfoBLL aptitudeinvestigateinfobll = new AptitudeinvestigateinfoBLL();
        private OutsourcingprojectBLL Outsourcingprojectbll = new OutsourcingprojectBLL();
        private OutsouringengineerBLL Outsouringengineernll = new OutsouringengineerBLL();
        private AptitudeinvestigateauditBLL auditbll = new AptitudeinvestigateauditBLL();
        private FileInfoBLL filebll = new FileInfoBLL();
        private HistoryRecordBLL historyrecordbll = new HistoryRecordBLL();
        private HistoryRecordDetailBLL historyrecorddetailbll = new HistoryRecordDetailBLL();
        private HistoryAuditBLL hisauditbll = new HistoryAuditBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private AptitudeinvestigatepeopleBLL aptitudeinvestigatepeoplebll = new AptitudeinvestigatepeopleBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();


        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 历史记录列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryRecord()
        {
            return View();
        }
        /// <summary>
        /// 历史记录详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryRecordDetail()
        {
            return View();
        }
        /// <summary>
        /// 人员资质审查列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PeopleList()
        {
            return View();
        }
        /// <summary>
        /// 人员资质审查详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PeopleForm()
        {
            var isldap = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("IsOpenPassword");
            return View();
        }
        /// <summary>
        /// 新增体检信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PhyInfoForm()
        {
            return View();
        }
        /// <summary>
        /// 历史记录列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryPeopleList()
        {
            return View();
        }
        /// <summary>
        /// 历史记录页面详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryPeopleForm()
        {
            return View();
        }
        /// <summary>
        /// 审核流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }
        /// <summary>
        /// 新增修改资质证件页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddZzzjForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.outengineerid,e.engineername,e.engineerareaname as districtname,d.itemname engineertype,l.itemname engineerlevel,
                                       e.id eid,t.issaveorcommit,t.nextcheckrolename,
                                       p.outprojectid,t.isauditover,t.nextcheckdeptid,e.engineerarea,
                                       p.outsourcingname,e.engineerstate,
                                       t.createusername,
                                       t.createdate,t.createuserorgcode,t.flowid,
                                       t.createuserid,e.engineerletdept,e.engineerletdeptid,'' as approveuserids ";
                pagination.p_tablename = @" epg_aptitudeinvestigateinfo t
                                          left join epg_outsouringengineer e on e.id = t.outengineerid
                                          left join epg_outsourcingproject p on p.outprojectid = t.outprojectid
                                           left join base_department b on b.departmentid = t.outprojectid 
                                            left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
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
                    pagination.conditionJson = "  1=1 ";
                }
                else if (currUser.RoleName.Contains("省级"))
                {
                    pagination.conditionJson = string.Format(@" t.createuserorgcode  in (select encode from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null)", currUser.NewDeptCode);
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户") || allrangedept.Contains(currUser.DeptId))
                {
                    pagination.conditionJson = string.Format(" (t.issaveorcommit='1' or t.createuserid='{0}') ", currUser.UserId);
                }
                else if (currUser.RoleName.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format(" ( e.outprojectid ='{0}' or e.supervisorid='{0}' or t.createuserid='{1}') ", currUser.DeptId, currUser.UserId);
                }
                else
                {
                    var deptentity = departmentbll.GetEntity(currUser.DeptId);
                    while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson = string.Format(" (e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and t.issaveorcommit='1' or t.createuserid='{1}') ", deptentity.EnCode, currUser.UserId);
                }
                //pagination.conditionJson += string.Format(" or t.createuserid='{0}' ", currUser.UserId);
                var data = aptitudeinvestigateinfobll.GetPageList(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = Outsouringengineernll.GetEntity(data.Rows[i]["outengineerid"].ToString());
                    var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //获取下一步审核人
                    string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
                    data.Rows[i]["approveuserids"] = str;
                }
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }

        }
        /// <summary>
        /// 获取历史记录列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetHistoryPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @" t.createuserid,t.applypeople,t.applypeopleid,t.applydate,
                                           t.createusername,
                                           t.createdate,
                                           t.historyzzscid,
                                           a.auditpeopleid,
                                       a.auditpeople,a.id auditId,
                                       case when a.auditresult='0' then '合格'
                                            when a.auditresult='2' then '待审核'
                                            when a.auditresult='1' then '不合格' else '' end auditresult";
                pagination.p_tablename = @" epg_historyrecord t
                                             left join epg_aptitudeinvestigateaudit a on t.id = a.aptitudeid ";
                pagination.conditionJson = " a.auditresult='1' ";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                var data = historyrecordbll.GetHistoryPageList(pagination, queryJson);
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        [HttpGet]
        public ActionResult GetHistoryPeopleList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @" t.createuserid,t.createdate,t.createusername,
                                           a.auditpeopleid,
                                       a.auditpeople,a.id auditId,
                                       case when a.auditresult='0' then '合格'
                                            when a.auditresult='2' then '待审核'
                                            when a.auditresult='1' then '不合格' else '' end auditresult";
                pagination.p_tablename = @" epg_historypeoplereview t
                                            left join epg_aptitudeinvestigateaudit a on t.id = a.aptitudeid";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = " a.auditresult='1' ";
                var data = historyrecordbll.GetHistoryPeopleList(pagination, queryJson);
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 获取资质审查历史记录详情及审核历史记录
        /// </summary>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue, string HisAuditId)
        {
            var hisrecordDetail = historyrecorddetailbll.GetEntity(keyValue);
            return ToJsonResult(hisrecordDetail);
        }
        /// <summary>
        /// 获取人员资质审查历史记录实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPeopleReviewEntity(string keyValue)
        {
            var data = historyrecordbll.GetPeopleReviewEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = aptitudeinvestigateinfobll.GetList(queryJson);

            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {

            var data = aptitudeinvestigateinfobll.GetEntity(keyValue);
            var OutprojectEntity = Outsourcingprojectbll.GetOutProjectInfo(data.OUTPROJECTID);
            var OutengineerEntity = Outsouringengineernll.GetEntity(data.OUTENGINEERID);
            //var AuditEntity = auditbll.GetAuditEntity(keyValue);
            //if (AuditEntity == null)
            //{
            //    AuditEntity = new AptitudeinvestigateauditEntity();
            //}
            var resultData = new
            {
                data = data,
                OutprojectEntity = OutprojectEntity,
                OutengineerEntity = OutengineerEntity,
                //AuditEntity = AuditEntity
            };
            return ToJsonResult(resultData);

        }

        /// <summary>
        /// 获取资质证件实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetZzzjFormJson(string keyValue) {
            var data = aptitudeinvestigateinfobll.GetZzzjFormJson(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取人员资质审查实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPeopleReview(string keyValue)
        {
            var data = peoplereviewbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 查询审核流程图
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="urltype">查询类型：1 单位资质 2 人员资质 3 特种设备验收 4 电动/安全工器具验收 5三措两案 6入厂许可 7开工申请</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAuditFlowData(string keyValue, string urltype) {
            try
            {
                var data = aptitudeinvestigateinfobll.GetAuditFlowData(keyValue, urltype);
                return ToJsonResult(data);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        /// <summary>
        /// 根据外包单位Id获取最近一次审核通过的资质信息
        /// 外包单位编辑时会获取上一次审核通过的信息
        /// 请忽更改字段,以免前端赋值Form时覆盖其它字段值造成错误
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAptituInfoByOutProject(string outProjectId, string keyValue)
        {
            var zzData = aptitudeinvestigateinfobll.GetListByOutprojectId(outProjectId);
            if (zzData != null)
            {
                var data = new
                {
                    BUSCODE = zzData.BUSCODE,
                    BUSCERTIFICATE = zzData.BUSCERTIFICATE,
                    BUSVALIDSTARTTIME = zzData.BUSVALIDSTARTTIME,
                    BUSVALIDENDTIME = zzData.BUSVALIDENDTIME,
                    SPLCODE = zzData.SPLCODE,
                    SPLCERTIFICATE = zzData.SPLCERTIFICATE,
                    SPLVALIDSTARTTIME = zzData.SPLVALIDSTARTTIME,
                    SPLVALIDENDTIME = zzData.SPLVALIDENDTIME,
                    UnitSuper = zzData.UnitSuper,
                    UnitSuperPhone = zzData.UnitSuperPhone,
                    ProjectManager = zzData.ProjectManager,
                    ProjectManagerTel = zzData.ProjectManagerTel,
                    SafetyModerator = zzData.SafetyModerator,
                    SafetyModeratorTel = zzData.SafetyModeratorTel,
                    //CQCODE = zzData.CQCODE,
                    //CQORG = zzData.CQORG,
                    //CQRANGE = zzData.CQRANGE,
                    //CQLEVEL = zzData.CQLEVEL,
                    //CQVALIDSTARTTIME = zzData.CQVALIDSTARTTIME,
                    //CQVALIDENDTIME = zzData.CQVALIDENDTIME,
                    ID = zzData.ID
                };
                //编辑时同步上次通过审核的单位资质附件
                //如果前面同步过其它的附件先删除后在同步
                //保证与最近一次通过审核的资质审查信息一致
                for (int i = 1; i <= 8; i++)
                {
                    var id = zzData.ID + "0" + i;
                    var file1 = filebll.GetFiles(id);
                    if (file1.Rows.Count > 0)
                    {
                        var key = keyValue + "0" + i;
                        var fileEdit1 = filebll.GetFiles(key);
                        if (fileEdit1.Rows.Count > 0)
                        {
                            foreach (DataRow item in fileEdit1.Rows)
                            {
                                filebll.RemoveForm(item["FileId"].ToString());
                            }

                        }
                        foreach (DataRow item in file1.Rows)
                        {
                            FileInfoEntity itemFile = new FileInfoEntity();
                            //itemFile.FileId = Guid.NewGuid().ToString();
                            itemFile.FileName = item["FileName"].ToString();
                            itemFile.FilePath = item["filepath"].ToString();
                            itemFile.FileSize = item["filesize"].ToString();
                            itemFile.RecId = (keyValue + "0" + i).ToString();
                            filebll.SaveForm(itemFile.FileId, itemFile);
                        }
                    }
                }
                //查询资质证件信息并给当前新增的赋值
                List<QualificationEntity> quaOldList = aptitudeinvestigateinfobll.GetZzzjList().Where(x => x.InfoId == keyValue).ToList();
                if (quaOldList.Count > 0) {
                    for (int i = 0; i < quaOldList.Count; i++)
                    {
                        aptitudeinvestigateinfobll.RemoveZzzjForm(quaOldList[i].ID);
                    }
                }
                List<QualificationEntity> quaList = aptitudeinvestigateinfobll.GetZzzjList().Where(x => x.InfoId == zzData.ID).ToList();
                for (int i = 0; i < quaList.Count; i++)
                {
                    var file1 =new FileInfoBLL().GetFiles(quaList[i].ID);
                    quaList[i].ID = "";
                    quaList[i].InfoId = keyValue;
                    aptitudeinvestigateinfobll.SaveZzzjForm("", quaList[i]);
                    if (file1.Rows.Count > 0)
                    {
                        var key = quaList[i].ID;
                        foreach (DataRow item in file1.Rows)
                        {
                            FileInfoEntity itemFile = new FileInfoEntity();
                            itemFile.FileName = item["FileName"].ToString();
                            itemFile.FilePath = item["filepath"].ToString();
                            itemFile.FileSize = item["filesize"].ToString();
                            itemFile.RecId = key;
                            new FileInfoBLL().SaveForm(itemFile.FileId, itemFile);
                        }
                    }
                }
                return ToJsonResult(data);
            }
            else
            {
                return ToJsonResult(null);
            }


        }
        [HttpGet]
        public ActionResult GetPagePeopleReviewListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"e.engineername,b.senddeptid,b.senddeptname,
                                       e.id eid,t.issaveorcommit,t.nextauditdeptcode,t.nextauditdeptid,t.nextauditrole,
                                       p.outprojectid,t.isauditover,t.createuserid,
                                       p.outsourcingname,e.engineerareaname as districtname,d.itemname engineertype,l.itemname engineerlevel,
                                       t.createusername,e.engineerletdeptid,e.engineerletdept,
                                       t.createdate,'' as approveuserids,t.outengineerid,t.flowid ";
                pagination.p_tablename = @" epg_peoplereview t
                                          left join epg_outsouringengineer e on e.id = t.outengineerid
                                          left join epg_outsourcingproject p on p.outprojectid = e.outprojectid
                                          left join base_department b on b.departmentid = e.outprojectid 
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate";
                if (string.IsNullOrWhiteSpace(pagination.sidx)) {
                    pagination.sidx = "t.createdate";//排序字段
                }
                if (string.IsNullOrWhiteSpace(pagination.sord))
                {
                    pagination.sord = "desc";//排序方式
                }
               
                Operator currUser = OperatorProvider.Provider.Current();
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
                    pagination.conditionJson = "  1=1  ";
                }
                else if (currUser.RoleName.Contains("省级"))
                {
                    pagination.conditionJson = string.Format(@" t.createuserorgcode  in (select encode from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null)", currUser.NewDeptCode);
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户") || allrangedept.Contains(currUser.DeptId))
                {
                    pagination.conditionJson = string.Format("  (t.issaveorcommit='1' or t.createuserid='{0}') ",currUser.UserId);
                }
                else if (currUser.RoleName.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format("  (e.outprojectid ='{0}' or e.supervisorid='{0}' or t.createuserid='{1}') ", currUser.DeptId, currUser.UserId);
                }
                else {
                    var deptentity = departmentbll.GetEntity(currUser.DeptId);
                    while (deptentity.Nature=="班组" || deptentity.Nature=="专业")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson = string.Format(" (e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and t.issaveorcommit='1' or t.createuserid='{1}') ", deptentity.EnCode, currUser.UserId);
                }
                //pagination.conditionJson += string.Format(" or t.createuserid='{0}' ", currUser.UserId);
                var data = peoplereviewbll.GetPagePeopleReviewListJson(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = Outsouringengineernll.GetEntity(data.Rows[i]["outengineerid"].ToString());
                    var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //获取下一步审核人
                    string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
                    data.Rows[i]["approveuserids"] = str;
                }
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        /// <summary>
        /// 获取资质证件列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetZzzjPageJson(Pagination pagination, string queryJson) {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "id";
                pagination.p_fields = @"createuserid,createuserdeptcode,createuserorgcode,createdate,createusername,
                                        modifydate,modifyuserid,modifyusername,cqcode,cqorg,cqrange,cqlevel,
                                        to_char(cqvalidstarttime,'yyyy-MM-dd') starttime,to_char(cqvalidendtime,'yyyy-MM-dd') endtime,
                                        cqvalidstarttime,cqvalidendtime,infoid";
                pagination.p_tablename = @" epg_qualification ";
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                pagination.conditionJson = string.Format(" createuserorgcode='{0}'", currUser.OrganizeCode);
                var data = aptitudeinvestigateinfobll.GetZzzjPageJson(pagination, queryJson);
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            try
            {
                aptitudeinvestigateinfobll.RemoveForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
           
        }

        /// <summary>
        /// 删除人员资质审查数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemovePeopleReview(string keyValue)
        {
            try
            {
                peoplereviewbll.RemoveForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 删除资质证件信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveZzzjForm(string keyValue) {
            try
            {
                aptitudeinvestigateinfobll.RemoveZzzjForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
         
        }
        /// <summary>
        /// 删除人员资质审查id删除人员
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AjaxOnly]
        //public ActionResult RemovePeople(string keyValue)
        //{
        //    peoplereviewbll.RemovePeople(keyValue);
        //    return Success("删除成功。");
        //}

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, AptitudeinvestigateinfoEntity entity)
        {
            try
            {
                aptitudeinvestigateinfobll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SavePeopleReview(string keyValue, PeopleReviewEntity entity)
        {
            try
            {
                peoplereviewbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveZzzjForm(string keyValue, QualificationEntity entity)
        {
            try
            {
                aptitudeinvestigateinfobll.SaveZzzjForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
        }

        public ActionResult ExportPeopleRegister(string keyValue)
        {
            try
            {
                var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
                var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == userInfo.OrganizeCode && x.ModuleCode == "RYZZSC").ToList();
                string tempPath = @"~/Resource/ExcelTemplate/人员登记表.doc";
                var tempEntity = tempconfig.FirstOrDefault();
                string fileName = "人员登记表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                if (tempconfig.Count > 0)
                {
                    if (tempEntity != null)
                    {
                        switch (tempEntity.ProessMode)
                        {
                            case "TY"://通用处理方式
                                tempPath = @"~/Resource/ExcelTemplate/人员登记表.doc";
                                break;
                            case "HRCB"://华润
                                tempPath = @"~/Resource/ExcelTemplate/人员登记表.doc";
                                break;
                            case "GDXY"://国电荥阳
                                tempPath = @"~/Resource/ExcelTemplate/国电荥阳人员登记表.doc";
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    tempPath = @"~/Resource/ExcelTemplate/人员登记表.doc";
                }
                ExportDataByCode(keyValue, tempPath, fileName);
                return Success("导出成功!");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }


        public void ExportReport(string keyValue)
        {
            try
            {
                var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
                string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/人员资质审批表华润版本.doc");
                string fileName = "相关方施工人员进厂申请表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                DocumentBuilder builder = new DocumentBuilder(doc);
                DataSet ds = new DataSet();
                DataTable dtPro = new DataTable("project");
                dtPro.Columns.Add("createdate");//申请时间
                dtPro.Columns.Add("outsourcingproject");//单位名称
                dtPro.Columns.Add("outsouringengineer");//工程名称
                dtPro.Columns.Add("predicttime");//计划工期
                dtPro.Columns.Add("engineerdirector");//施工负责人
                dtPro.Columns.Add("workpeoplecount");  //施工人数
                dtPro.Columns.Add("engineerletdeptid"); //用工部门
                dtPro.Columns.Add("linkman"); //联系人

                dtPro.Columns.Add("ygauditidea");//用工部门审核意见
                dtPro.Columns.Add("ygauditdate");//用工部门审核时间
                dtPro.Columns.Add("ehsauditidea");//EHS部审核意见
                dtPro.Columns.Add("ehsauditdate");//EHS部审核时间
                dtPro.Columns.Add("bgsauditidea");//办公室审核意见
                dtPro.Columns.Add("bgsauditdate");//办公室审核时间

                HttpResponse resp = System.Web.HttpContext.Current.Response;

                PeopleReviewEntity p = peoplereviewbll.GetEntity(keyValue);

                OutsouringengineerEntity eng = Outsouringengineernll.GetEntity(p.OUTENGINEERID);
                OutsourcingprojectEntity pro = Outsourcingprojectbll.GetOutProjectInfo(eng.OUTPROJECTID);

                DataRow row = dtPro.NewRow();
                row["createdate"] =Convert.ToDateTime(p.CREATEDATE).ToString("yyyy年MM月dd日");
                row["outsourcingproject"] = pro.OUTSOURCINGNAME;
                row["outsouringengineer"] = eng.ENGINEERNAME;
                row["predicttime"] = eng.PREDICTTIME;
                row["engineerdirector"] = eng.ENGINEERDIRECTOR;
                row["engineerletdeptid"] = eng.ENGINEERLETDEPT;
                row["linkman"] = pro.LINKMAN;
                row["workpeoplecount"] = aptitudeinvestigatepeoplebll.GetList("").Where(t => t.PEOPLEREVIEWID == keyValue).Count();
                List<AptitudeinvestigateauditEntity> list = auditbll.GetAuditList(keyValue).OrderByDescending(x => x.AUDITTIME).ToList();
                string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var filepath = list[i].AUDITSIGNIMG == null ? "" : (Server.MapPath("~/") + list[i].AUDITSIGNIMG.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        var stime = Convert.ToDateTime(list[i].AUDITTIME);
                        if (i == 0)
                        {
                            row["ygauditidea"] = list[i].AUDITOPINION;
                            //zauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("ygauditusername");
                            row["ygauditdate"] = stime.ToString("yyyy年MM月dd日");
                        }
                        else if (i == 1)
                        {
                            row["ehsauditidea"] = list[i].AUDITOPINION;
                            //sauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("ehsauditusername");
                            row["ehsauditdate"] = stime.ToString("yyyy年MM月dd日");
                        }
                        else if (i == 2)
                        {
                            row["bgsauditidea"] = list[i].AUDITOPINION;
                            //aauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("bgsauditusername");
                            row["bgsauditdate"] = stime.ToString("yyyy年MM月dd日");

                        }

                        if (!System.IO.File.Exists(filepath))
                        {
                            filepath = pic;
                        }
                        builder.InsertImage(filepath, 80, 35);
                    }
                }
                dtPro.Rows.Add(row);
                doc.MailMerge.Execute(dtPro);
                doc.MailMerge.DeleteFields();
                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ExportDataByCode(string keyValue, string tempPath, string fileName)
        {
            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
            string strDocPath = Server.MapPath(tempPath);
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DataSet ds = new DataSet();
            DataTable dtPro = new DataTable("project");
            dtPro.Columns.Add("untilname");//单位名称
            dtPro.Columns.Add("legalperson");//法人代表
            dtPro.Columns.Add("projectname");//工程名称
            dtPro.Columns.Add("nowtime");
            dtPro.Columns.Add("itemopinion");
            dtPro.Columns.Add("itemhead");  //项目负责人
            dtPro.Columns.Add("techhead"); //生成技术负责人
            dtPro.Columns.Add("safehead"); //安全监督负责人

            dtPro.Columns.Add("contractperiod");//合同期限
            dtPro.Columns.Add("dutydept");//项目管理部门
            dtPro.Columns.Add("applyperson");//制表人
            dtPro.Columns.Add("applytime");//制表时间
            dtPro.Columns.Add("outhead"); 
            
            DataTable dt = new DataTable("people");
            dt.Columns.Add("no");
            dt.Columns.Add("name");
            dt.Columns.Add("sex");
            dt.Columns.Add("idcard");
            dt.Columns.Add("worktype");
            dt.Columns.Add("health");
            dt.Columns.Add("workyear");
            dt.Columns.Add("education");
            dt.Columns.Add("birthday");
            dt.Columns.Add("cardno");

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            PeopleReviewEntity p = peoplereviewbll.GetEntity(keyValue);

            OutsouringengineerEntity eng = Outsouringengineernll.GetEntity(p.OUTENGINEERID);
            OutsourcingprojectEntity pro = Outsourcingprojectbll.GetOutProjectInfo(eng.OUTPROJECTID);

            DataRow row = dtPro.NewRow();
            var comList = new CompactBLL().GetComoactTimeByProjectId(eng.ID);
            if (comList.Rows.Count > 0)
            {
                var startTime = string.Empty;
                DateTime r = new DateTime();
                if (DateTime.TryParse(comList.Rows[0]["mintime"].ToString(), out r))
                {
                    startTime = r.ToString("yyyy年MM月dd日");
                }
                var endTime = string.Empty;
                DateTime e= new DateTime();
                if (DateTime.TryParse(comList.Rows[0]["maxtime"].ToString(), out e))
                {
                    endTime = e.ToString("yyyy年MM月dd日");
                }
                row["contractperiod"] = startTime + "至" + endTime;
            }
            row["untilname"] = pro.OUTSOURCINGNAME;
            row["legalperson"] = pro.LEGALREP;

            row["projectname"] = eng.ENGINEERNAME;
            row["nowtime"] = DateTime.Now.ToString("yyyy-MM-dd");
            row["dutydept"] = eng.ENGINEERLETDEPT;
            row["applyperson"] = p.CREATEUSERNAME;
            row["applytime"] = p.CREATEDATE.Value.ToString("yyyy年MM月dd日");
            var sendDeptid = eng.ENGINEERLETDEPTID;
            List<AptitudeinvestigateauditEntity> list = auditbll.GetAuditList(keyValue).OrderByDescending(x => x.AUDITTIME).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (sendDeptid == list[i].AUDITDEPTID)
                    {
                        var person = new UserBLL().GetEntity(list[i].AUDITPEOPLEID);
                        if (person != null)
                        {
                            //if (person.RoleName.Contains("负责人"))
                            //{
                                row["itemopinion"] = list[i].AUDITOPINION;
                                row["itemhead"] = list[i].AUDITPEOPLE;
                            //}
                        }
                    }
                    if (eng.OUTPROJECTID == list[i].AUDITDEPTID)
                    {
                        var person = new UserBLL().GetEntity(list[i].AUDITPEOPLEID);
                        if (person != null)
                        {
                            if (person.RoleName.Contains("负责人"))
                            {
                                row["outhead"] = list[i].AUDITPEOPLE;
                            }
                        }
                    }
                    string val = new DataItemDetailBLL().GetItemValue(userInfo.OrganizeCode);

                    if (!string.IsNullOrEmpty(val))
                    {
                        var deptList = val.Split(',');
                        if (deptList.Length > 1)
                        {
                            if (list[i].AUDITDEPTID.ToString() == deptList[0])
                            {
                                row["techhead"] = list[i].AUDITPEOPLE;
                            }
                            if (list[i].AUDITDEPTID == deptList[1])
                            {
                                row["safehead"] = list[i].AUDITPEOPLE;
                            }
                        }
                    }
                }
            }
            dtPro.Rows.Add(row);
            List<AptitudeinvestigatepeopleEntity> PeopleList = new AptitudeinvestigatepeopleBLL().GetList("").Where(x => x.PEOPLEREVIEWID == keyValue).ToList();
            if (PeopleList.Count > 0)
            {
                for (int i = 0; i < PeopleList.Count; i++)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["no"] = (i + 1);
                    dtrow["name"] = PeopleList[i].REALNAME;
                    dtrow["sex"] = PeopleList[i].GENDER;
                    dtrow["idcard"] = PeopleList[i].IDENTIFYID;
                    dtrow["worktype"] = PeopleList[i].WORKOFTYPE;
                    dtrow["health"] = PeopleList[i].STATEOFHEALTH;
                    dtrow["workyear"] = PeopleList[i].WORKYEAR;
                    dtrow["education"] = PeopleList[i].DEGREESID;
                    dtrow["birthday"] = PeopleList[i].BIRTHDAY == null ? "" : PeopleList[i].BIRTHDAY.Value.ToString("yyyy-MM-dd");
                    string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        UserId = PeopleList[i].ID
                    });
                    var cardlist = new CertificateinspectorsBLL().GetList(queryJson).ToList();
                    for (int j = 0; j < cardlist.Count; j++)
                    {
                        dtrow["cardno"] += cardlist[j].CREDENTIALSCODE + ",";
                    }
                    if (dtrow["cardno"].ToString().Length > 0) {
                        dtrow["cardno"] = dtrow["cardno"].ToString().Substring(0, dtrow["cardno"].ToString().Length - 1);
                    }
                    dt.Rows.Add(dtrow);
                }
            }
            ds.Tables.Add(dt);
            ds.Tables.Add(dtPro);
            doc.MailMerge.Execute(dtPro);
            doc.MailMerge.ExecuteWithRegions(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }
        #endregion
    }
}
