using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using ERCHTMS.Entity.BaseManage;
using System.Data;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using BSFramework.Util.Extension;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.BaseManage;
using System.Collections.Generic;
using ERCHTMS.Busines.JPush;
using System.Web;
using Aspose.Words;
using Aspose.Words.Tables;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：开工申请表
    /// </summary>
    public class StartapplyforController : MvcControllerBase
    {
        private StartapplyforBLL startapplyforbll = new StartapplyforBLL();
        private HistoryStartapplyBLL historystartapplybll = new HistoryStartapplyBLL();
        private HistoryAuditBLL historyauditbll = new HistoryAuditBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private InvestigateBLL investigatebll = new InvestigateBLL();
        private InvestigateContentBLL investigatecontentbll = new InvestigateContentBLL();
        private InvestigateRecordBLL investigaterecordbll = new InvestigateRecordBLL();
        private InvestigateDtRecordBLL investigatedtrecordbll = new InvestigateDtRecordBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL(); //审核记录
        private IntromissionBLL intromissionbll = new IntromissionBLL(); //入厂许可申请操作
        private SchemeMeasureBLL schememeasurebll = new SchemeMeasureBLL();//三措两案
        private TechDisclosureBLL techdisclosurebll = new TechDisclosureBLL();//安全技术交底
        private CompactBLL compactbll = new CompactBLL();//合同
        private ProtocolBLL protocolbll = new ProtocolBLL();//协议
        private FileInfoBLL filebll = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OutsouringengineerBLL Outsouringengineernll = new OutsouringengineerBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private ToolsBLL toolsbll = new ToolsBLL();


        #region 视图功能
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AppForm()
        {
            return View();
        }
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
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult HistoryForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = startapplyforbll.GetList(queryJson);
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
            var data = startapplyforbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取开工时间
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetApplyReturnTime(string outProjectId, string outEngId)
        {
            var data = startapplyforbll.GetApplyReturnTime(outProjectId, outEngId);

            var resultData = new
            {
                APPLYRETURNTIME = data.APPLYRETURNTIME.Value.ToString("yyyy-MM-dd")
            };
            return ToJsonResult(resultData);
        }

        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue, string HisAuditId)
        {
            var hisapplyData = historystartapplybll.GetEntity(keyValue);
            var hisauditData = historyauditbll.GetEntity(HisAuditId);
            string projectId = "";
            var applyInfo = startapplyforbll.GetEntity(hisapplyData.APPLYID);
            if (applyInfo != null)
            {
                projectId = applyInfo.OUTENGINEERID;
            }
            var hisData = new
            {
                hisapply = hisapplyData,
                hisAudit = hisauditData,
                projectId = projectId
            };
            return ToJsonResult(hisData);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]

        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "s.id";
                pagination.p_fields = @" s.outprojectid,e.engineerareaname as districtname,d.itemname engineertype,l.itemname engineerlevel,
                                           s.iscommit,s.outengineerid,b.fullname,b.senddeptid,
                                           b.senddeptname,e.engineerletdept,e.engineername,
                                           s.applypeople,s.applypeopleid,s.applytime,
                                           s.applytype,s.applyreturntime,s.applyno,
                                           s.createuserid,auditrole,isover,
                                           s.isinvestover,s.flowdeptname,s.flowdept,s.flowrolename,
                                           s.flowrole,s.nodename,s.nodeid ,'' as approveuserid,'' as approveusername,'' as approveuserids";
                pagination.p_tablename = @"epg_startapplyfor s
                                          left join epg_outsouringengineer e on e.id = s.outengineerid
                                          left join base_department b on b.departmentid = s.outprojectid
                                           left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate";
                pagination.sidx = "s.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                pagination.conditionJson = "1=1 ";
                //pagination.conditionJson += string.Format(" or (s.createuserid='{0}' and s.iscommit='0') ", currUser.UserId);
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
                    pagination.conditionJson = "1=1 ";
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户") || allrangedept.Contains(currUser.DeptId))
                {
                    pagination.conditionJson += string.Format(" and (s.iscommit='1' or (s.createuserid='{0}' and s.iscommit='0'))", currUser.UserId);
                }
                else if (currUser.RoleName.Contains("承包商级用户"))
                {
                    pagination.conditionJson += string.Format(" and (b.departmentid ='{0}' or e.supervisorid ='{0}' or (s.createuserid='{1}' and s.iscommit='0'))", currUser.DeptId, currUser.UserId);
                }
                else
                {
                    var deptentity = departmentbll.GetEntity(currUser.DeptId);
                    while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson += string.Format(" and ((e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and s.iscommit='1') or (s.createuserid='{1}' and s.iscommit='0')) ", deptentity.EnCode, currUser.UserId);

                    //pagination.conditionJson += string.Format(" and ((e.engineerletdeptid ='{0}' and s.iscommit='1') or (s.createuserid='{1}' and s.iscommit='0'))", currUser.DeptId, currUser.UserId);
                }
                var data = startapplyforbll.GetPageList(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = Outsouringengineernll.GetEntity(data.Rows[i]["outengineerid"].ToString());
                    var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //获取下一步审核人
                    string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["nodeid"].ToString(), data.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
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

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取开工项目完成情况
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]

        public ActionResult GetItemStatusList(string projectId)
        {
            var dt = startapplyforbll.GetStartWorkStatus(projectId);
            return ToJsonResult(dt);
        }
        /// <summary>
        /// 获取工程现场负责人和安全员信息
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]

        public ActionResult GetProjectUserInfo(string projectId)
        {
            var dt = startapplyforbll.GetSafetyUserInfo(projectId);
            return ToJsonResult(dt);
        }
        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]

        public ActionResult GetHistoryPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "s.id";
                pagination.p_fields = @" s.applypeople,s.outproject,
                                           s.applyid,
                                           s.applypeopleid,
                                           s.applytime,s.outengineerid,
                                           s.applytype,
                                           s.applyreturntime,
                                           s.applyno,s.flowdeptname,s.flowdept,s.flowrolename,s.flowrole,s.nodename,s.nodeid,s.isinvestover,s.iscommit,s.isover";
                pagination.p_tablename = @"epg_historystartapplyfor s  ";
                pagination.sidx = "s.createdate";//排序字段
                pagination.sord = "desc";//排序方式

                var data = historystartapplybll.GetHisPageList(pagination, queryJson);
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
        public ActionResult GetStartApplyByProjectId(string id)
        {
            var data = startapplyforbll.GetList("").Where(x => x.OUTENGINEERID == id).ToList();
            if (data.Count > 0)
            {
                return ToJsonResult(data.FirstOrDefault());
            }
            else
            {
                return ToJsonResult(null);
            }
        }

        public ActionResult GetFileByOutProjectId(string proId, string keyValue)
        {
            var file = filebll.GetFiles(keyValue);//如果已经关联过附件就先删除在进行关联
            if (file.Rows.Count > 0)
            {
                foreach (DataRow item in file.Rows)
                {
                    filebll.RemoveForm(item["FileId"].ToString());
                }
            }

            //三措两案--关联最近一次审核的附件
            var three = schememeasurebll.GetSchemeMeasureListByOutengineerId(proId);

            //合同--全部
            var c = compactbll.GetListByProjectId(proId);
            //协议--全部
            var p = protocolbll.GetList().Where(x => x.PROJECTID == proId).ToList();
            //安全技术交底--全部
            var t = techdisclosurebll.GetList().Where(x => x.PROJECTID == proId).ToList();
            if (three != null)
            {
                var file1 = filebll.GetFiles(three.ID);//三措两案附件
                foreach (DataRow item in file1.Rows)
                {
                    FileInfoEntity itemFile = new FileInfoEntity();
                    //itemFile.FileId = Guid.NewGuid().ToString();
                    itemFile.FileName = item["FileName"].ToString();
                    itemFile.FilePath = item["filepath"].ToString();
                    itemFile.FileSize = item["filesize"].ToString();
                    itemFile.RecId = keyValue.ToString();
                    filebll.SaveForm(itemFile.FileId, itemFile);
                }
            }


            if (c.Count > 0)
            {
                for (int i = 0; i < c.Count; i++)
                {
                    var file2 = filebll.GetFiles(c[i].ID);//合同附件

                    foreach (DataRow item in file2.Rows)
                    {
                        FileInfoEntity itemFile = new FileInfoEntity();
                        //itemFile.FileId = Guid.NewGuid().ToString();
                        itemFile.FileName = item["FileName"].ToString();
                        itemFile.FilePath = item["filepath"].ToString();
                        itemFile.FileSize = item["filesize"].ToString();
                        itemFile.RecId = keyValue.ToString();
                        filebll.SaveForm(itemFile.FileId, itemFile);
                    }

                }
            }
            if (p.Count > 0)
            {
                for (int i = 0; i < p.Count; i++)
                {
                    var file2 = filebll.GetFiles(p[i].ID);//协议附件

                    foreach (DataRow item in file2.Rows)
                    {
                        FileInfoEntity itemFile = new FileInfoEntity();
                        //itemFile.FileId = Guid.NewGuid().ToString();
                        itemFile.FileName = item["FileName"].ToString();
                        itemFile.FilePath = item["filepath"].ToString();
                        itemFile.FileSize = item["filesize"].ToString();
                        itemFile.RecId = keyValue.ToString();
                        filebll.SaveForm(itemFile.FileId, itemFile);
                    }

                }
            }
            if (t.Count > 0)
            {
                for (int i = 0; i < t.Count; i++)
                {
                    var file2 = filebll.GetFiles(t[i].ID);//安全技术交底

                    foreach (DataRow item in file2.Rows)
                    {
                        FileInfoEntity itemFile = new FileInfoEntity();
                        //itemFile.FileId = Guid.NewGuid().ToString();
                        itemFile.FileName = item["FileName"].ToString();
                        itemFile.FilePath = item["filepath"].ToString();
                        itemFile.FileSize = item["filesize"].ToString();
                        itemFile.RecId = keyValue.ToString();
                        filebll.SaveForm(itemFile.FileId, itemFile);
                    }
                    var file3 = filebll.GetFiles(t[i].ID + "01");//安全技术交底

                    foreach (DataRow item in file3.Rows)
                    {
                        FileInfoEntity itemFile = new FileInfoEntity();
                        //itemFile.FileId = Guid.NewGuid().ToString();
                        itemFile.FileName = item["FileName"].ToString();
                        itemFile.FilePath = item["filepath"].ToString();
                        itemFile.FileSize = item["filesize"].ToString();
                        itemFile.RecId = keyValue.ToString();
                        filebll.SaveForm(itemFile.FileId, itemFile);
                    }
                }
            }
            return ToJsonResult(keyValue);
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
            startapplyforbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, StartapplyforEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            entity.ID = keyValue;
            if (entity.ISCOMMIT == "0")
            {
                entity.NodeName = "开工申请";
                startapplyforbll.SaveForm(entity.ID, entity);
            }
            else
            {
                var list = investigatebll.GetList(curUser.OrganizeId).Where(p => p.SETTINGTYPE == "开工申请").ToList();
                InvestigateEntity investigateEntity = null;
                ManyPowerCheckEntity mpcEntity = null;
                string moduleName = "开工申请审查";
                //新增时会根据角色自动审核,此时需根据工程和审核配置查询审核流程Id
                OutsouringengineerEntity engineerentity = new OutsouringengineerBLL().GetEntity(entity.OUTENGINEERID);
                if (dataitemdetailbll.GetDataItemListByItemCode("FlowWithRiskLevel").Count() > 0)
                {
                    switch (engineerentity.ENGINEERLEVEL)
                    {
                        case "001":
                            moduleName = "开工申请审查_一级风险";
                            break;
                        case "002":
                            moduleName = "开工申请审查_二级风险";
                            break;
                        case "003":
                            moduleName = "开工申请审查_三级风险";
                            break;
                        case "004":
                            moduleName = "开工申请审查_四级风险";
                            break;
                        default:
                            break;
                    }
                }
                    
                string outengineerid = entity.OUTENGINEERID;
                if (list.Count() > 0)
                {
                    investigateEntity = list.FirstOrDefault();
                }
                bool isUseSetting = true;
                if (null != investigateEntity)
                {
                    //启用审查
                    if (investigateEntity.ISUSE == "是")
                    {
                        entity.NodeName = "审查中";
                        entity.ISINVESTOVER = 0; //审查状态

                    }
                    else  //未启用审查，直接跳转到审核 
                    {
                        entity.NodeName = "审核中";
                        entity.ISINVESTOVER = 1;//审查完成状态
                        entity.IsOver = 0;//审核状态
                    }

                    //更改申请信息状态
                    startapplyforbll.SaveForm(entity.ID, entity);
                    //启用审查
                    if (investigateEntity.ISUSE == "是")
                    {
                        //新增审查记录
                        InvestigateRecordEntity rcEntity = new InvestigateRecordEntity();
                        rcEntity.INTOFACTORYID = entity.ID;
                        rcEntity.INVESTIGATETYPE = "0";//当前记录标识
                        investigaterecordbll.SaveForm("", rcEntity);

                        //获取审查内容
                        var contentList = investigatecontentbll.GetList(investigateEntity.ID).ToList();

                        //批量增加审查内容到审查记录中
                        foreach (InvestigateContentEntity icEntity in contentList)
                        {
                            InvestigateDtRecordEntity dtEntity = new InvestigateDtRecordEntity();
                            dtEntity.INVESTIGATERECORDID = rcEntity.ID;
                            dtEntity.INVESTIGATECONTENT = icEntity.INVESTIGATECONTENT;
                            dtEntity.INVESTIGATECONTENTID = icEntity.ID;
                            investigatedtrecordbll.SaveForm("", dtEntity);
                        }
                    }
                }
                else
                {
                    //如果没有审查配置，直接到审核
                    isUseSetting = false;
                    entity.NodeName = "审核中";
                    entity.ISCOMMIT = "1";
                    entity.ISINVESTOVER = 1; //审核状态
                }
                string status = "";
                mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out status, moduleName, outengineerid, false, "");

                if (null != mpcEntity)
                {
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.NodeId = mpcEntity.ID;
                    entity.NodeName = mpcEntity.FLOWNAME;
                    DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                    var userAccount = dt.Rows[0]["account"].ToString();
                    var userName = dt.Rows[0]["realname"].ToString();
                    JPushApi.PushMessage(userAccount, userName, "WB015", entity.ID);
                }
                else
                {
                    //未配置审核项
                    entity.FLOWDEPT = "";
                    entity.FLOWDEPTNAME = "";
                    entity.FLOWROLE = "";
                    entity.FLOWROLENAME = "";
                    entity.NodeId = "";
                    entity.NodeName = "已完结";
                    entity.IsOver = 1;
                    entity.ISCOMMIT = "1";
                    entity.ISINVESTOVER = 1;
                    OutsouringengineerEntity engineerEntity = new OutsouringengineerBLL().GetEntity(entity.OUTENGINEERID);
                    engineerEntity.ENGINEERSTATE = "002";
                    engineerEntity.PLANENDDATE = entity.APPLYRETURNTIME;
                    new OutsouringengineerBLL().SaveForm(engineerEntity.ID, engineerEntity);
                }
                startapplyforbll.SaveForm(entity.ID, entity);
            }
            return Success("操作成功。");
        }
        [HttpPost]
        public ActionResult ExportData(string keyValue, string user1, string user2)
        {

            try
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //查询是否配置了开工申请的模板处理方式--有配置按配置的处理方式处理,无配置则按通用处理方式处理
                var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == user.OrganizeCode && x.ModuleCode == "KGSQ").ToList();
                string tempPath = "~/Resource/DocumentFile/开工申请审批表.doc";

                //工程基本信息
                DataTable dt = new DataTable("U");
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1;
                pagination.p_kid = "s.id";
                pagination.p_fields = @"b.fullname,s.outengineerid projectid,e.engineerletdept,e.supervisorname,e.engineername,
                                    s.applypeople,e.engineerareaname as engineerarea,to_char(s.applyreturntime,'yyyy-mm-dd') startdate,to_char(s.applyendtime,'yyyy-mm-dd') enddate,
                                    engineercontent,checkresult,checkusers,engineerdirector,
                                    to_char(applytime,'yyyy-mm-dd') applydate,s.flowdept,s.flowrolename,
                                    '' as approveuserid,'' as approveusername,e.unitsuper,e.unitsuperphone,
                                    e.engineerletdeptid,'" + user1 + "' user1,'" + user2 + "' user2";
                pagination.p_tablename = @"epg_startapplyfor s
                                              left join epg_outsouringengineer e on e.id = s.outengineerid
                                              left join base_department b on b.departmentid = s.outprojectid";
                pagination.sidx = "s.createdate";
                pagination.sord = "desc";
                pagination.conditionJson = " s.id='" + keyValue + "'";
                dt = startapplyforbll.GetPageList(pagination, "{}");
                var tempEntity = tempconfig.FirstOrDefault();
                var fileName = string.Empty;
                if (tempconfig.Count > 0)
                {
                    if (tempEntity != null)
                    {
                        switch (tempEntity.ProessMode)
                        {
                            case "TY"://通用处理方式
                                tempPath = "~/Resource/DocumentFile/开工申请审批表.doc";
                                fileName = ExportCommin(dt, tempPath, keyValue, user);
                                break;
                            case "HRCB"://华润
                                tempPath = "~/Resource/ExcelTemplate/华润开工申请审批表.doc";
                                fileName = ExportHrcb(dt, tempPath, keyValue, user);
                                break;
                            case "GDXY"://国电荥阳
                                tempPath = "~/Resource/ExcelTemplate/国电荥阳开工申请审批表.doc";
                                fileName = ExportGdxy(dt, tempPath, keyValue, user);
                                break;
                            case "HJB"://黄金埠
                                tempPath = "~/Resource/ExcelTemplate/黄金埠开工申请审批表.doc";
                                fileName = ExportHjb(dt, tempPath, keyValue, user);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    tempPath = "~/Resource/DocumentFile/开工申请审批表.doc";
                    fileName = ExportCommin(dt, tempPath, keyValue, user);
                }
                return Success("生成成功", new { fileName = fileName });
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        #region 保存审查内容（修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveAppForm(string keyValue, string recordData, StartapplyforEntity entity)
        {
            JArray arr = (JArray)JsonConvert.DeserializeObject(recordData);
            //只更新审查内容
            for (int i = 0; i < arr.Count(); i++)
            {
                string id = arr[i]["id"].ToString();  //主键
                string result = arr[i]["result"].ToString(); //结果
                string people = arr[i]["people"].ToString(); //选择的人员
                string peopleid = arr[i]["peopleid"].ToString(); //选择的人员
                string signpic = string.IsNullOrWhiteSpace(arr[i]["signpic"].ToString()) ? "" : HttpUtility.UrlDecode(arr[i]["signpic"].ToString()).Replace("../..", "");
                var scEntity = investigatedtrecordbll.GetEntity(id); //审查内容项
                scEntity.INVESTIGATERESULT = result;
                scEntity.INVESTIGATEPEOPLE = people;
                scEntity.INVESTIGATEPEOPLEID = peopleid;
                scEntity.SIGNPIC = signpic;
                investigatedtrecordbll.SaveForm(id, scEntity);
            }

            return Success("操作成功。");
        }
        #endregion
        #region 提交审查/审核内容（修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitAppForm(string keyValue, string state, string recordData, StartapplyforEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            int noDoneCount = 0; //未完成个数

            bool isUseSetting = true;

            string newKeyValue = string.Empty;

            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string moduleName = "开工申请审查";
            //新增时会根据角色自动审核,此时需根据工程和审核配置查询审核流程Id
            OutsouringengineerEntity engineerentity = new OutsouringengineerBLL().GetEntity(entity.OUTENGINEERID);
            if (dataitemdetailbll.GetDataItemListByItemCode("FlowWithRiskLevel").Count() > 0)
            {
                switch (engineerentity.ENGINEERLEVEL)
                {
                    case "001":
                        moduleName = "开工申请审查_一级风险";
                        break;
                    case "002":
                        moduleName = "开工申请审查_二级风险";
                        break;
                    case "003":
                        moduleName = "开工申请审查_三级风险";
                        break;
                    case "004":
                        moduleName = "开工申请审查_四级风险";
                        break;
                    default:
                        break;
                }
            }

            string status = "";
            //更改申请信息状态
            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out status, moduleName, entity.OUTENGINEERID, false, entity.NodeId);


            JArray arr = new JArray();

            if (!recordData.IsEmpty())
            {
                arr = (JArray)JsonConvert.DeserializeObject(recordData);
            }

            //审查状态下更新审查内容
            if (state == "1")
            {
                //只更新审查内容
                for (int i = 0; i < arr.Count(); i++)
                {
                    string id = arr[i]["id"].ToString();  //主键
                    string result = arr[i]["result"].ToString(); //结果
                    string people = arr[i]["people"].ToString(); //选择的人员
                    string peopleid = arr[i]["peopleid"].ToString(); //选择的人员
                    string signpic = string.IsNullOrWhiteSpace(arr[i]["signpic"].ToString()) ? "" : HttpUtility.UrlDecode(arr[i]["signpic"].ToString()).Replace("../..", "");
                    var scEntity = investigatedtrecordbll.GetEntity(id); //审查内容项
                    scEntity.INVESTIGATERESULT = result;
                    if (result == "未完成") { noDoneCount += 1; } //存在未完成的则累加
                    scEntity.INVESTIGATEPEOPLE = people;
                    scEntity.INVESTIGATEPEOPLEID = peopleid;
                    scEntity.SIGNPIC = signpic;
                    //更新当前流程进行中的审查内容
                    investigatedtrecordbll.SaveForm(id, scEntity);
                }
                //退回操作
                if (noDoneCount > 0)
                {
                    AddBackData(keyValue, out newKeyValue);
                    entity.FLOWDEPT = " ";
                    entity.FLOWDEPTNAME = " ";
                    entity.FLOWROLE = " ";
                    entity.FLOWROLENAME = " ";
                    entity.NodeId = " ";
                    entity.ISCOMMIT = "0"; //更改状态为登记状态
                    entity.ISINVESTOVER = 0; //更改状态为登记状态
                    entity.NodeName = "";

                }
                else
                {
                    if (null != mpcEntity)
                    {
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.NodeId = mpcEntity.ID;
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB015", entity.ID);
                    }
                    entity.NodeName = "审核中";
                    entity.ISINVESTOVER = 1; //更改状态为审核
                }
            }
            else
            {
                //同意进行下一步
                if (aentity.AUDITRESULT == "0")
                {
                    //下一步流程不为空
                    if (null != mpcEntity)
                    {
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.NodeId = mpcEntity.ID;
                        entity.ISINVESTOVER = 1;
                        entity.ISCOMMIT = "1";
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB015", entity.ID);
                    }
                    else
                    {
                        entity.FLOWDEPT = " ";
                        entity.FLOWDEPTNAME = " ";
                        entity.FLOWROLE = " ";
                        entity.FLOWROLENAME = " ";
                        entity.NodeId = " ";
                        entity.NodeName = "已完结";
                        entity.ISINVESTOVER = 1;
                        entity.ISCOMMIT = "1";
                        entity.IsOver = 1; //更改状态为完结状态
                        //开工申请审核通过更新工程状态为在建
                        OutsouringengineerEntity engineerEntity = new OutsouringengineerBLL().GetEntity(entity.OUTENGINEERID);
                        engineerEntity.ENGINEERSTATE = "002";
                        engineerEntity.PLANENDDATE = entity.APPLYRETURNTIME;
                        new OutsouringengineerBLL().SaveForm(engineerEntity.ID, engineerEntity);
                    }

                    //添加审核记录
                    aentity.APTITUDEID = keyValue; //关联id 
                    aentity.AUDITSIGNIMG = HttpUtility.UrlDecode(aentity.AUDITSIGNIMG);
                    aentity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
                    aptitudeinvestigateauditbll.SaveForm("", aentity);
                }
                else  //退回到申请人
                {
                    AddBackData(keyValue, out newKeyValue);  //添加历史记录
                    entity.FLOWDEPT = " ";
                    entity.FLOWDEPTNAME = " ";
                    entity.FLOWROLE = " ";
                    entity.FLOWROLENAME = " ";
                    entity.NodeId = " ";
                    entity.ISCOMMIT = "0"; //更改状态为登记状态
                    entity.ISINVESTOVER = 0;
                    entity.NodeName = "";
                    var applyUser = new UserBLL().GetEntity(entity.APPLYPEOPLEID);
                    if (applyUser != null)
                    {
                        JPushApi.PushMessage(applyUser.Account, entity.CREATEUSERNAME, "WB014", entity.ID);
                    }
                    //获取当前业务对象的所有历史审核记录
                    var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                    //批量更新审核记录关联ID
                    foreach (AptitudeinvestigateauditEntity mode in shlist)
                    {
                        mode.APTITUDEID = newKeyValue; //对应新的关联ID
                        aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                    }
                    //添加审核记录
                    aentity.APTITUDEID = newKeyValue; //关联id 
                    aentity.AUDITSIGNIMG = HttpUtility.UrlDecode(aentity.AUDITSIGNIMG);
                    aentity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
                    aptitudeinvestigateauditbll.SaveForm("", aentity);
                }
            }
            //更改开工申请申请单
            startapplyforbll.SaveForm(keyValue, entity);

            return Success("操作成功。");
        }
        #endregion
        #region 退回添加到历史记录信息
        /// <summary>
        /// 退回添加到历史记录信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="arr"></param>
        public void AddBackData(string keyValue, out string newKeyValue)
        {
            //退回的同时保存原始的申请记录
            var dentity = startapplyforbll.GetEntity(keyValue); //原始记录
            var startfor = JsonConvert.SerializeObject(dentity);
            HistoryStartapplyEntity hentity = JsonConvert.DeserializeObject<HistoryStartapplyEntity>(startfor);
            hentity.ID = Guid.NewGuid().ToString();
            hentity.APPLYID = dentity.ID;
            var unit = new OutsourcingprojectBLL().GetOutProjectInfo(hentity.OUTPROJECTID);
            if (unit != null)
                hentity.OUTPROJECT = unit.OUTSOURCINGNAME;
            historystartapplybll.SaveForm("", hentity);
            var file1 = new FileInfoBLL().GetFiles(keyValue);
            if (file1.Rows.Count > 0)
            {
                var key = hentity.ID;
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
            newKeyValue = hentity.ID;

            //更新审查记录单关联ID
            InvestigateRecordEntity irEntity = investigaterecordbll.GetEntityByIntroKey(keyValue); //审查记录单
            if (null != irEntity)
            {
                irEntity.INTOFACTORYID = newKeyValue;
                irEntity.INVESTIGATETYPE = "1"; //历史记录标识
                investigaterecordbll.SaveForm(irEntity.ID, irEntity);
            }
        }
        #endregion
        /// <summary>
        /// 通用导出处理
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="tempPath">模板地址</param>
        /// <param name="keyValue">开工申请主键</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        private string ExportCommin(DataTable dt, string tempPath, string keyValue, Operator user)
        {
            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath(tempPath));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            dt.Rows[0]["startdate"] = DateTime.Parse(dt.Rows[0]["startdate"].ToString()).ToString("yyyy年MM月dd日");
            dt.Rows[0]["applydate"] = DateTime.Parse(dt.Rows[0]["applydate"].ToString()).ToString("yyyy年MM月dd日");
            //var areaentity = new DistrictBLL().GetEntity(dt.Rows[0]["engineerarea"].ToString());
            //if (areaentity != null)
            //{
            //    dt.Rows[0]["engineerarea"] = areaentity.DistrictName;
            //}
            //else
            //{
            //    dt.Rows[0]["engineerarea"] = "";
            //}
            doc.MailMerge.Execute(dt);
            string projectId = dt.Rows[0]["projectId"].ToString();
            string deptId = dt.Rows[0]["engineerletdeptid"].ToString();
            //开工条件确认项目
            string status = dt.Rows[0]["checkresult"].ToString();
            string users = dt.Rows[0]["checkusers"].ToString();


            dt = new DataTable("A");
            dt.Columns.Add("num");
            dt.Columns.Add("itemname");
            dt.Columns.Add("status");
            dt.Columns.Add("username");
            dt.Columns.Add("signpic");
            int k = 0;
            DataTable dtItems = startapplyforbll.GetStartForItem(keyValue);
            foreach (DataRow item in dtItems.Rows)
            {
                DataRow dr1 = dt.NewRow();
                dr1[0] = k + 1;
                dr1[1] = item["investigatecontent"].ToString();
                dr1[2] = item["investigateresult"].ToString() == "无此项" ? "/" : item["investigateresult"].ToString();
                dr1[3] = item["investigatepeople"].ToString();
                dr1[4] = item["investigateresult"].ToString() == "无此项" ? Server.MapPath("~/content/Images/no_1.png") : item["signpic"].ToString() == "" ?
                    Server.MapPath("~/content/Images/no_1.png") : System.IO.File.Exists((Server.MapPath("~/") + item["signpic"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString()) ?
                    (Server.MapPath("~/") + item["signpic"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString() : Server.MapPath("~/content/Images/no_1.png");
                dt.Rows.Add(dr1);
                k++;
            }
            doc.MailMerge.ExecuteWithRegions(dt);

            //审核信息
            dt = new AptitudeinvestigateauditBLL().GetAuditRecList(keyValue);
            string fbUser = Server.MapPath("~/content/Images/no_1.png");
            string fbDate = "";
            string fbIdea = "";
            string sjUser = Server.MapPath("~/content/Images/no_1.png");
            string sjDate = "";
            string sjIdea = "";
            string ahUser = Server.MapPath("~/content/Images/no_1.png");
            string ahDate = "";
            string ahIdea = "";
            string ldUser = Server.MapPath("~/content/Images/no_1.png");
            string ldDate = "";
            string ldIdea = "";
            string val = new DataItemDetailBLL().GetItemValue(user.OrganizeCode);
            string sjSql = "AUDITDEPT='生技部'";
            string ahSql = "AUDITDEPT='安环部'";
            if (!string.IsNullOrEmpty(val))
            {
                if (val.Split(',').Length > 1)
                {
                    sjSql = "AUDITDEPTid='" + val.Split(',')[0] + "'";
                    ahSql = "AUDITDEPTid='" + val.Split(',')[1] + "'";
                }

            }
            List<UserEntity> uList = new UserBLL().GetList().Where(x => x.DepartmentId == deptId).ToList();

            for (int j = 0; j < dtItems.Rows.Count; j++)
            {
                for (int i = 0; i < uList.Count; i++)
                {
                    if (dtItems.Rows[j]["investigatepeople"].ToString() == uList[i].RealName)
                    {
                        var filepath = (Server.MapPath("~/") + dtItems.Rows[j]["signpic"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            fbUser = filepath;
                        }
                        else
                        {
                            fbUser = Server.MapPath("~/content/Images/no_1.png");
                        }
                        fbDate = DateTime.Parse(dtItems.Rows[j]["modifydate"].ToString()).ToString("yyyy年MM月dd日");
                        break;
                    }
                }
            }
            DataRow[] drs = dt.Select(sjSql + " and aptitudeid='" + keyValue + "'", "createtime desc");
            if (drs.Length > 0)
            {
                var filepath = (Server.MapPath("~/") + drs[0]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    sjUser = filepath;
                }
                else
                {
                    sjUser = Server.MapPath("~/content/Images/no_1.png");
                }
                sjDate = DateTime.Parse(drs[0]["audittime"].ToString()).ToString("yyyy年MM月dd日");
                sjIdea = drs[0]["auditopinion"].ToString();
            }
            drs = dt.Select(ahSql + " and aptitudeid='" + keyValue + "'", "createtime desc");
            if (drs.Length > 0)
            {
                var filepath = (Server.MapPath("~/") + drs[0]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    ahUser = filepath;
                }
                else
                {
                    ahUser = Server.MapPath("~/content/Images/no_1.png");
                }
                ahDate = DateTime.Parse(drs[0]["audittime"].ToString()).ToString("yyyy年MM月dd日");
                ahIdea = drs[0]["auditopinion"].ToString();
            }
            if (dt.Rows.Count > 0)
            {
                int len = dt.Rows.Count;
                var filepath = (Server.MapPath("~/") + dt.Rows[len - 1]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    ldUser = filepath;
                }
                else
                {
                    ldUser = Server.MapPath("~/content/Images/no_1.png");
                }
                ldDate = DateTime.Parse(dt.Rows[len - 1]["audittime"].ToString()).ToString("yyyy年MM月dd日");
                ldIdea = dt.Rows[len - 1]["auditopinion"].ToString();
            }
            object obj = startapplyforbll.GetContractSno(projectId);
            string sno = "";
            if (obj != DBNull.Value && obj != null)
            {
                sno = obj.ToString();
            }
            doc.MailMerge.Execute(new string[] { "fbUser", "fbDate", "fbIdea", "sjUser", "sjDate", "sjIdea", "ahUser", "ahDate", "ahIdea", "ldUser", "ldDate", "ldIdea", "sno" }, new string[] { fbUser, fbDate, fbIdea, sjUser, sjDate, sjIdea, ahUser, ahDate, ahIdea, ldUser, ldDate, ldIdea, sno });
            string fileName = Guid.NewGuid().ToString() + ".doc";
            doc.MailMerge.DeleteFields();

            doc.Save(Server.MapPath("~/Resource/temp/" + fileName));
            return fileName;
        }
        /// <summary>
        /// 华润导出处理
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="tempPath">模板地址</param>
        /// <param name="keyValue">开工申请主键</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        private string ExportHrcb(DataTable dt, string tempPath, string keyValue, Operator user)
        {
            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath(tempPath));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            dt.Rows[0]["applydate"] = DateTime.Parse(dt.Rows[0]["applydate"].ToString()).ToString("yyyy年MM月dd日");

            doc.MailMerge.Execute(dt);
            string projectId = dt.Rows[0]["projectId"].ToString();
            string engineerletdeptid = dt.Rows[0]["engineerletdeptid"].ToString();
            dt = new DataTable("A");
            dt.Columns.Add("num");
            dt.Columns.Add("itemname");
            dt.Columns.Add("status");
            dt.Columns.Add("username");
            dt.Columns.Add("signpic");
            int k = 0;
            DataTable dtItems = startapplyforbll.GetStartForItem(keyValue);
            foreach (DataRow item in dtItems.Rows)
            {
                DataRow dr1 = dt.NewRow();
                dr1[0] = k + 1 ;
                dr1[1] = item["investigatecontent"].ToString();
                dr1[2] = item["investigateresult"].ToString() == "无此项" ? "/" : item["investigateresult"].ToString();
                dr1[3] = item["investigatepeople"].ToString();
                dr1[4] = item["investigateresult"].ToString() == "无此项" ? Server.MapPath("~/content/Images/no_1.png") : item["signpic"].ToString() == "" ?
                    Server.MapPath("~/content/Images/no_1.png") : System.IO.File.Exists((Server.MapPath("~/") + item["signpic"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString()) ?
                    (Server.MapPath("~/") + item["signpic"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString() : Server.MapPath("~/content/Images/no_1.png");
                dt.Rows.Add(dr1);
                k++;
            }
            doc.MailMerge.ExecuteWithRegions(dt);

            //审核信息
            dt = new AptitudeinvestigateauditBLL().GetAuditRecList(keyValue);

            //根据后台配置找到EHS部门
            var EhsPz = new DataItemDetailBLL().GetDataItemListByItemCode("'EHSDepartment'").FirstOrDefault();
            string ehsSql = string.Empty;
            if (EhsPz != null)
            {
                var EhsDept = new DepartmentBLL().GetList().Where(x => x.ParentId == EhsPz.ItemName && x.EnCode == EhsPz.ItemValue).FirstOrDefault();
                if (EhsDept != null)
                {
                    ehsSql = "auditdeptid='" + EhsDept.DepartmentId + "'";
                }
            }

            string userdept = "auditdeptid='" + engineerletdeptid + "'";
            //用工部门安全专工
            string UserZg = Server.MapPath("~/content/Images/no_1.png");
            string userZgDate = "";
            string userZgIdea = "";
            //用工部门负责人
            string Userfzr = Server.MapPath("~/content/Images/no_1.png");
            string userDate = "";
            string userIdea = "";
            //ehs部门
            string ahUser = Server.MapPath("~/content/Images/no_1.png");
            string ahDate = "";
            string ahIdea = "";
            //公司领导
            string ldUser = Server.MapPath("~/content/Images/no_1.png");
            string ldDate = "";
            string ldIdea = "";

            DataRow[] drs = dt.Select(userdept + " and aptitudeid='" + keyValue + "'", "createtime desc");
            if (drs.Length > 0)
            {
                foreach (DataRow item in drs)
                {
                    var auditPeople = new UserBLL().GetEntity(item["auditpeopleid"].ToString());
                    if (auditPeople.RoleName.Contains("专工"))
                    {
                        var filepath = (Server.MapPath("~/") + item["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            UserZg = filepath;
                        }
                        else
                        {
                            UserZg = Server.MapPath("~/content/Images/no_1.png");
                        }
                        userZgDate = DateTime.Parse(item["audittime"].ToString()).ToString("yyyy年MM月dd日");
                        userZgIdea = item["auditopinion"].ToString();
                    }
                    else
                    {
                        var filepath = (Server.MapPath("~/") + item["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            Userfzr = filepath;
                        }
                        else
                        {
                            Userfzr = Server.MapPath("~/content/Images/no_1.png");
                        }
                        userDate = DateTime.Parse(item["audittime"].ToString()).ToString("yyyy年MM月dd日");
                        userIdea = item["auditopinion"].ToString();
                    }
                }
            }
            drs = dt.Select(ehsSql + " and aptitudeid='" + keyValue + "'", "createtime desc");
            if (drs.Length > 0)
            {
                var filepath = (Server.MapPath("~/") + drs[0]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    ahUser = filepath;
                }
                else
                {
                    ahUser = Server.MapPath("~/content/Images/no_1.png");
                }
                ahDate = DateTime.Parse(drs[0]["audittime"].ToString()).ToString("yyyy年MM月dd日");
                ahIdea = drs[0]["auditopinion"].ToString();
            }
            if (dt.Rows.Count > 0)
            {
                int len = dt.Rows.Count;
                var filepath = (Server.MapPath("~/") + dt.Rows[len - 1]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    ldUser = filepath;
                }
                else
                {
                    ldUser = Server.MapPath("~/content/Images/no_1.png");
                }
                ldDate = DateTime.Parse(dt.Rows[len - 1]["audittime"].ToString()).ToString("yyyy年MM月dd日");
                ldIdea = dt.Rows[len - 1]["auditopinion"].ToString();
            }

            object obj = startapplyforbll.GetContractSno(projectId);
            string sno = "";
            if (obj != DBNull.Value && obj != null)
            {
                sno = obj.ToString();
            }
            doc.MailMerge.Execute(new string[] { "UserZg", "userZgDate", "userZgIdea", "Userfzr", "userDate", "userIdea", "ahUser", "ahDate", "ahIdea", "ldUser", "ldDate", "ldIdea", "sno" },
                new string[] { UserZg, userZgDate, userZgIdea, Userfzr, userDate, userIdea, ahUser, ahDate, ahIdea, ldUser, ldDate, ldIdea, sno });
            string fileName = Guid.NewGuid().ToString() + ".doc";
            doc.MailMerge.DeleteFields();
            doc.Save(Server.MapPath("~/Resource/temp/" + fileName));
            return fileName;
        }
        /// <summary>
        /// 国电荥阳导出处理
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="tempPath">模板地址</param>
        /// <param name="keyValue">开工申请主键</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        private string ExportGdxy(DataTable dt, string tempPath, string keyValue, Operator user)
        {
            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath(tempPath));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            dt.Rows[0]["applydate"] = DateTime.Parse(dt.Rows[0]["applydate"].ToString()).ToString("yyyy年MM月dd日");

            doc.MailMerge.Execute(dt);
            string projectId = dt.Rows[0]["projectId"].ToString();
            string engineerletdeptid = dt.Rows[0]["engineerletdeptid"].ToString();
            dt = new DataTable("A");
            dt.Columns.Add("num");
            dt.Columns.Add("itemname");
            dt.Columns.Add("status");
            dt.Columns.Add("username");
            dt.Columns.Add("signpic");
            int k = 0;
            DataTable dtItems = startapplyforbll.GetStartForItem(keyValue);
            foreach (DataRow item in dtItems.Rows)
            {
                DataRow dr1 = dt.NewRow();
                dr1[0] = k+1;
                dr1[1] = item["investigatecontent"].ToString();
                dr1[2] = item["investigateresult"].ToString() == "无此项" ? "/" : item["investigateresult"].ToString();
                dr1[3] = item["investigatepeople"].ToString();
                dr1[4] = item["investigateresult"].ToString() == "无此项" ? Server.MapPath("~/content/Images/no_1.png") : item["signpic"].ToString() == "" ?
                    Server.MapPath("~/content/Images/no_1.png") : System.IO.File.Exists((Server.MapPath("~/") + item["signpic"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString()) ?
                    (Server.MapPath("~/") + item["signpic"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString() : Server.MapPath("~/content/Images/no_1.png");
                dt.Rows.Add(dr1);
                k++;
            }
            doc.MailMerge.ExecuteWithRegions(dt);

            //审核信息
            dt = new AptitudeinvestigateauditBLL().GetAuditRecList(keyValue);

            //根据后台配置找到安全监察部
            string ehsSql = "AUDITDEPT='安环部'";
            string val = new DataItemDetailBLL().GetItemValue(user.OrganizeCode);
            if (!string.IsNullOrEmpty(val))
            {
                if (val.Split(',').Length > 1)
                {
                    //ehsSql = "AUDITDEPTid='" + val.Split(',')[0] + "'";
                    ehsSql = "auditdeptid='" + val.Split(',')[1] + "'";
                }

            }
            string userdept = "auditdeptid='" + engineerletdeptid + "'";
            //合同监管部门专工
            string UserZg = Server.MapPath("~/content/Images/no_1.png");
            string userZgDate = "";
            string userZgIdea = "";
            //合同监管部门负责人
            string Userfzr = Server.MapPath("~/content/Images/no_1.png");
            string userDate = "";
            string userIdea = "";
            //安全监察部专工
            string ahzgUser = Server.MapPath("~/content/Images/no_1.png");
            string ahzgDate = "";
            string ahzgIdea = "";
            //安全监察部负责人
            string ahUser = Server.MapPath("~/content/Images/no_1.png");
            string ahDate = "";
            string ahIdea = "";
            //副总工程师
            string ldUser = Server.MapPath("~/content/Images/no_1.png");
            string ldDate = "";
            string ldIdea = "";

            DataRow[] drs = dt.Select(userdept + " and aptitudeid='" + keyValue + "'", "createtime desc");
            if (drs.Length > 0)
            {
                foreach (DataRow item in drs)
                {
                    var auditPeople = new UserBLL().GetEntity(item["auditpeopleid"].ToString());
                    if (auditPeople.RoleName.Contains("专工"))
                    {
                        var filepath = (Server.MapPath("~/") + item["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            UserZg = filepath;
                        }
                        else
                        {
                            UserZg = Server.MapPath("~/content/Images/no_1.png");
                        }
                        userZgDate = DateTime.Parse(item["audittime"].ToString()).ToString("yyyy年MM月dd日");
                        userZgIdea = item["auditopinion"].ToString();
                    }
                    else
                    {
                        var filepath = (Server.MapPath("~/") + item["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            Userfzr = filepath;
                        }
                        else
                        {
                            Userfzr = Server.MapPath("~/content/Images/no_1.png");
                        }
                        userDate = DateTime.Parse(item["audittime"].ToString()).ToString("yyyy年MM月dd日");
                        userIdea = item["auditopinion"].ToString();
                    }
                }
            }
            drs = dt.Select(ehsSql + " and aptitudeid='" + keyValue + "'", "createtime desc");
            if (drs.Length > 0)
            {
                foreach (DataRow item in drs)
                {
                    var auditPeople = new UserBLL().GetEntity(item["auditpeopleid"].ToString());
                    if (auditPeople.RoleName.Contains("专工"))
                    {
                        var filepath = (Server.MapPath("~/") + item["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            ahzgUser = filepath;
                        }
                        else
                        {
                            ahzgUser = Server.MapPath("~/content/Images/no_1.png");
                        }
                        ahzgDate = DateTime.Parse(item["audittime"].ToString()).ToString("yyyy年MM月dd日");
                        ahzgIdea = item["auditopinion"].ToString();
                    }
                    else
                    {
                        var filepath = (Server.MapPath("~/") + item["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            ahUser = filepath;
                        }
                        else
                        {
                            ahUser = Server.MapPath("~/content/Images/no_1.png");
                        }
                        ahDate = DateTime.Parse(item["audittime"].ToString()).ToString("yyyy年MM月dd日");
                        ahIdea = item["auditopinion"].ToString();
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                int len = dt.Rows.Count;
                var filepath = (Server.MapPath("~/") + dt.Rows[len - 1]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    ldUser = filepath;
                }
                else
                {
                    ldUser = Server.MapPath("~/content/Images/no_1.png");
                }
                ldDate = DateTime.Parse(dt.Rows[len - 1]["audittime"].ToString()).ToString("yyyy年MM月dd日");
                ldIdea = dt.Rows[len - 1]["auditopinion"].ToString();
            }


            //合同编号
            object obj = startapplyforbll.GetContractSno(projectId);
            string sno = "";
            if (obj != DBNull.Value && obj != null)
            {
                sno = obj.ToString();
            }
            doc.MailMerge.Execute(new string[] { "UserZg", "userZgDate", "userZgIdea", "Userfzr", "userDate", "userIdea", "ahUser", "ahDate", "ahIdea", "ahzgUser", "ahzgDate", "ahzgIdea", "ldUser", "ldDate", "ldIdea", "sno" },
              new string[] { UserZg, userZgDate, userZgIdea, Userfzr, userDate, userIdea, ahUser, ahDate, ahIdea, ahzgUser, ahzgDate, ahzgIdea, ldUser, ldDate, ldIdea, sno });
            string fileName = Guid.NewGuid().ToString() + ".doc";

            doc.MailMerge.DeleteFields();
            doc.Save(Server.MapPath("~/Resource/temp/" + fileName));
            return fileName;
        }
        /// <summary>
        /// 黄金埠导出处理
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="tempPath">模板地址</param>
        /// <param name="keyValue">开工申请主键</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        private string ExportHjb(DataTable dt, string tempPath, string keyValue, Operator user)
        {
            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath(tempPath));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            if (string.IsNullOrWhiteSpace(dt.Rows[0]["supervisorname"].ToString()))
            {
                Table table = (Table)doc.GetChild(NodeType.Table, 0, true);
                table.Rows[14].Remove();
            }
            dt.Rows[0]["applydate"] = DateTime.Parse(dt.Rows[0]["applydate"].ToString()).ToString("yyyy年MM月dd日");
            dt.Rows[0]["startdate"] = string.IsNullOrWhiteSpace(dt.Rows[0]["startdate"].ToString()) ? "" : DateTime.Parse(dt.Rows[0]["startdate"].ToString()).ToString("yyyy年MM月dd日");
            dt.Rows[0]["enddate"] = DateTime.Parse(dt.Rows[0]["enddate"].ToString()).ToString("yyyy年MM月dd日");
            doc.MailMerge.Execute(dt);
            string projectId = dt.Rows[0]["projectId"].ToString();
            string engineerletdeptid = dt.Rows[0]["engineerletdeptid"].ToString();
            dt = new DataTable("A");
            dt.Columns.Add("itemname");
            dt.Columns.Add("status");
            DataTable dtItems = startapplyforbll.GetStartForItem(keyValue);
            foreach (DataRow item in dtItems.Rows)
            {
                DataRow dr1 = dt.NewRow();
                dr1[0] = item["investigatecontent"].ToString();
                dr1[1] = item["investigateresult"].ToString();
                dt.Rows.Add(dr1);
            }
            dt.Rows.RemoveAt(dt.Rows.Count - 1);
            doc.MailMerge.ExecuteWithRegions(dt);

            //审核信息
            dt = new AptitudeinvestigateauditBLL().GetAuditRecList(keyValue);

            //项目组长
            string jszzPerson = Server.MapPath("~/content/Images/no_1.png");
            string jszzIdea = "";
            //专业主管
            string zyzgPerson = Server.MapPath("~/content/Images/no_1.png");
            string zyzgIdea = "";
            //检修技改主管（经办人）
            string xmzzPerson = Server.MapPath("~/content/Images/no_1.png");
            string xmzzIdea = "";
            //项目部门主任/分管主任
            string xmzrPerson = Server.MapPath("~/content/Images/no_1.png");
            string xmzrIdea = "";
            //安全监察员
            string ahzzPerson = Server.MapPath("~/content/Images/no_1.png");
            string ahzzIdea = "";
            //安环部部门主任/分管主任
            string ahzrPerson = Server.MapPath("~/content/Images/no_1.png");
            string ahzrIdea = "";
            //监理工程师
            string jlzzPerson = Server.MapPath("~/content/Images/no_1.png");
            string jlzzIdea = "";
            //总监理师
            string jlzrPerson = Server.MapPath("~/content/Images/no_1.png");
            string jlzrIdea = "";
            if (dt.Rows.Count > 0)
            {
                var filepath = (Server.MapPath("~/") + dt.Rows[0]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    jszzPerson = filepath;
                }
                else
                {
                    jszzPerson = Server.MapPath("~/content/Images/no_1.png");
                }
                jszzIdea = dt.Rows[0]["auditopinion"].ToString();
            }
            if (dt.Rows.Count > 1)
            {
                var filepath = (Server.MapPath("~/") + dt.Rows[1]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    zyzgPerson = filepath;
                }
                else
                {
                    zyzgPerson = Server.MapPath("~/content/Images/no_1.png");
                }
                zyzgIdea = dt.Rows[1]["auditopinion"].ToString();
            }
            if (dt.Rows.Count > 2)
            {
                var filepath = (Server.MapPath("~/") + dt.Rows[2]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    xmzzPerson = filepath;
                }
                else
                {
                    xmzzPerson = Server.MapPath("~/content/Images/no_1.png");
                }
                xmzzIdea = dt.Rows[2]["auditopinion"].ToString();
            }
            if (dt.Rows.Count > 3)
            {
                var filepath = (Server.MapPath("~/") + dt.Rows[3]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    xmzrPerson = filepath;
                }
                else
                {
                    xmzrPerson = Server.MapPath("~/content/Images/no_1.png");
                }
                xmzrIdea = dt.Rows[3]["auditopinion"].ToString();
            }
            if (dt.Rows.Count > 4)
            {
                var filepath = (Server.MapPath("~/") + dt.Rows[4]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    ahzzPerson = filepath;
                }
                else
                {
                    ahzzPerson = Server.MapPath("~/content/Images/no_1.png");
                }
                ahzzIdea = dt.Rows[4]["auditopinion"].ToString();
            }
            if (dt.Rows.Count > 5)
            {
                var filepath = (Server.MapPath("~/") + dt.Rows[5]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    ahzrPerson = filepath;
                }
                else
                {
                    ahzrPerson = Server.MapPath("~/content/Images/no_1.png");
                }
                ahzrIdea = dt.Rows[5]["auditopinion"].ToString();
            }
            if (dt.Rows.Count > 6)
            {
                var filepath = (Server.MapPath("~/") + dt.Rows[6]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    jlzzPerson = filepath;
                }
                else
                {
                    jlzzPerson = Server.MapPath("~/content/Images/no_1.png");
                }
                jlzzIdea = dt.Rows[6]["auditopinion"].ToString();
            }
            if (dt.Rows.Count > 7)
            {
                var filepath = (Server.MapPath("~/") + dt.Rows[7]["auditsignimg"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    jlzrPerson = filepath;
                }
                else
                {
                    jlzrPerson = Server.MapPath("~/content/Images/no_1.png");
                }
                jlzrIdea = dt.Rows[7]["auditopinion"].ToString();
            }
            


            //合同编号
            object obj = startapplyforbll.GetContractSno(projectId);
            string sno = "";
            if (obj != DBNull.Value && obj != null)
            {
                sno = obj.ToString();
            }
            //特种设备
            var equ = toolsbll.GetList(" and equiptype='2' and outengineerid='"+ projectId + "' and isover='1'");
            var tzsb = "";
            if (equ.Count() > 0)
            {
                tzsb = "☑是特种设备，已到政府相关部门办理安装或修前告知手续。□否 ";
            }
            else
            {
                tzsb = "□是特种设备，已到政府相关部门办理安装或修前告知手续。☑否 ";
            }
            doc.MailMerge.Execute(new string[] { "jszzIdea", "jszzPerson", "zyzgIdea", "zyzgPerson", "xmzzIdea", "xmzzPerson", "xmzrIdea", "xmzrPerson", "ahzzIdea", "ahzzPerson", "ahzrIdea", "ahzrPerson", "jlzzIdea", "jlzzPerson", "jlzrIdea", "jlzrPerson", "sno", "tzsb" },
              new string[] { jszzIdea, jszzPerson, zyzgIdea, zyzgPerson, xmzzIdea, xmzzPerson, xmzrIdea, xmzrPerson, ahzzIdea, ahzzPerson, ahzrIdea, ahzrPerson, jlzzIdea, jlzzPerson, jlzrIdea, jlzrPerson, sno, tzsb });
            string fileName = Guid.NewGuid().ToString() + ".doc";

            doc.MailMerge.DeleteFields();
            doc.Save(Server.MapPath("~/Resource/temp/" + fileName));
            return fileName;
        }
        #endregion
    }
}
