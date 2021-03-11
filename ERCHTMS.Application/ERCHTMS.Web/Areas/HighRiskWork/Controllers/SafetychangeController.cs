using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using ERCHTMS.Busines.BaseManage;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using Aspose.Words;
using System.Web;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：安全措施变动申请表
    /// </summary>
    public class SafetychangeController : MvcControllerBase
    {
        private SafetychangeBLL safetychangebll = new SafetychangeBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private ScaffoldauditrecordBLL scaffoldauditrecordbll = new ScaffoldauditrecordBLL();
        private HighRiskRecordBLL highriskrecordbll = new HighRiskRecordBLL();
        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();



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

        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }

        /// <summary>
        /// 台账界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Ledger()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        public ActionResult GetApplyAuditList(string keyValue, int AuditType)
        {
            var data = scaffoldauditrecordbll.GetApplyAuditList(keyValue, AuditType);
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
            var data = safetychangebll.GetList(queryJson);
            return ToJsonResult(data);
        }
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.createuserid,
                                           t.createdate,t.changetype,t.projectname,t.changename,t.workunittype,t.projectid,
                                           t.applyunit,t.applychangetime,t.returntime,
                                           t.applyunitid,t.applypeople,t.applypeopleid,
                                           t.applytime, t.workunit,t.workunitid,t.applyno,
                                           t.iscommit,t.isaccepover,t.isapplyover,t.acceppeople,
                                           t.flowdept,t.nodename,t.nodeid,t.isaccpcommit,t.createuserdeptcode,
                                           t.flowrole,t.flowdeptname,t.flowrolename,t.flowremark,t.specialtytype,t.accspecialtytype,'' as approveuserid,b.outtransferuseraccount,b.intransferuseraccount,'' as approveuseraccount,
                                        case 
                                                 when t.iscommit=0 then 0
                                                 when t.iscommit=1 and t.isapplyover=0  then 1
                                                 when t.iscommit=1 and t.isapplyover=2  then 2
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=0 and t.isaccepover=0 then 3
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=0 then 4
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=2 then 5
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=1 then 6 else 7 end currstate,
                                         case 
                                                 when t.iscommit=0 then '变动申请中'
                                                 when t.iscommit=1 and t.isapplyover=0  then '变动审核(批)中'
                                                 when t.iscommit=1 and t.isapplyover=2  then '变动审核(批)未通过'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=0 and t.isaccepover=0 then '变动审核(批)通过待验收'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=0 then '验收审核中'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=2 then '验收审核(批)未通过'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=1 then '验收审核(批)通过' else '' end resultstate";
                pagination.p_tablename = @"   bis_safetychange t left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) b on t.id=b.recid and t.nodeid=b.flowid and b.num=1";
                //pagination.sidx = "t.createdate";//排序字段
                //pagination.sord = "desc";//排序方式
                pagination.conditionJson = "1=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        string isAllDataRange = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetEnableItemValue("HighRiskWorkDataRange"); //特殊标记，高风险作业模块是否看全厂数据
                        if (!string.IsNullOrEmpty(isAllDataRange))
                        {
                            pagination.conditionJson += " and t.createuserorgcode='" + user.OrganizeCode + "'";
                        }
                        else
                        {
                            switch (authType)
                            {
                                case "1":
                                    pagination.conditionJson += " and applypeopleid='" + user.UserId + "'";
                                    break;
                                case "2":
                                    pagination.conditionJson += " and workunitcode='" + user.DeptCode + "'";
                                    break;
                                case "3"://本子部门
                                    pagination.conditionJson += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                                    break;
                                case "4":
                                    pagination.conditionJson += " and t.createuserorgcode='" + user.OrganizeCode + "'";
                                    break;
                            }
                        }
                        
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                DataTable data = safetychangebll.GetPageList(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string executedept = string.Empty;
                    highriskcommonapplybll.GetExecutedept(data.Rows[i]["workunittype"].ToString(), data.Rows[i]["workunitid"].ToString(), data.Rows[i]["projectid"].ToString(), out executedept); //获取执行部门
                    string createdetpid = departmentbll.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentbll.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId; //获取创建部门
                    string outsouringengineerdept = string.Empty;
                    highriskcommonapplybll.GetOutsouringengineerDept(data.Rows[i]["workunitid"].ToString(), out outsouringengineerdept);
                    string str = manypowercheckbll.GetApproveUserAccount(data.Rows[i]["nodeid"].ToString(), data.Rows[i]["id"].ToString(), "", data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                    data.Rows[i]["approveuseraccount"] = str;
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safetychangebll.GetEntity(keyValue);
            return ToJsonResult(data);
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
        [HandlerMonitor(6, "安全设施变动删除")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            safetychangebll.RemoveForm(keyValue);
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
        [HandlerMonitor(5, "安全设施变动申请保存")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, SafetychangeEntity entity, string RiskRecord)
        {
            List<HighRiskRecordEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HighRiskRecordEntity>>(RiskRecord);
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            ManyPowerCheckEntity mpcEntity = null;
            entity.ISACCEPOVER = 0;
            entity.ISACCPCOMMIT = 0;
            entity.ISAPPLYOVER = 0;
            if (entity.ISCOMMIT == 0)
            {
                safetychangebll.SaveForm(keyValue, entity);
            }
            else
            {
                //单位内部审核流程
                if (entity.WORKUNITTYPE == "0")
                {
                    string moduleName = "(内部)设施变动申请审核";
                    mpcEntity = peoplereviewbll.CheckAuditForNextByWorkUnit(currUser, moduleName, entity.WORKUNITID, entity.NodeId, false);
                }
                else
                {
                    //外包审核流程
                    string moduleName = "(外包)设施变动申请审核";
                    mpcEntity = peoplereviewbll.CheckAuditForNextByOutsourcing(currUser, moduleName, entity.WORKUNITID, entity.NodeId, false, true, entity.PROJECTID);
                }

                if (null != mpcEntity)
                {
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.NodeId = mpcEntity.ID;
                    entity.NodeName = mpcEntity.FLOWNAME;
                    entity.FLOWREMARK = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                    string type = entity.FLOWREMARK != "1" ? "0" : "1";
                    new ScaffoldBLL().SendMessage(entity.FLOWDEPT, entity.FLOWROLE, "ZY011", keyValue, "", "", type, !string.IsNullOrEmpty(entity.SPECIALTYTYPE) ? entity.SPECIALTYTYPE : "");
                }
                else
                {
                    entity.FLOWREMARK = "";
                    //未配置审核项
                    entity.FLOWDEPT = "";
                    entity.FLOWDEPTNAME = "";
                    entity.FLOWROLE = "";
                    entity.FLOWROLENAME = "";
                    entity.NodeId = "";
                    entity.NodeName = "已完结";
                    //entity.ISACCEPOVER = 1;
                    entity.ISCOMMIT = 1;
                    entity.ISAPPLYOVER = 1;//申请审核完成
                }
                safetychangebll.SaveForm(keyValue, entity);
            }
            highriskrecordbll.RemoveFormByWorkId(keyValue);
            var num = 0;
            foreach (var item in list)
            {
                item.CreateDate = DateTime.Now.AddSeconds(-num);
                highriskrecordbll.SaveForm("", item);
                num++;
            }
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "安全设施变动验收保存")]
        [AjaxOnly]
        public ActionResult SaveAccpForm(string keyValue, SafetychangeEntity entity)
        {
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            ManyPowerCheckEntity mpcEntity = null;
            entity.ACCEPDEPT = currUser.DeptName;
            entity.ACCEPDEPTID = currUser.DeptId;
            entity.ACCEPPEOPLE = currUser.UserName;
            entity.ACCEPPEOPLEID = currUser.UserId;
            entity.ACCEPTIME = DateTime.Now;
            if (entity.ISACCPCOMMIT == 0)
            {
                safetychangebll.SaveForm(keyValue, entity);
            }
            else
            {
                //string moduleName = "设施变动验收审核";
                //单位内部验收审核流程
                if (entity.WORKUNITTYPE == "0")
                {
                    string moduleName = "(内部)设施变动验收审核";
                    mpcEntity = peoplereviewbll.CheckAuditForNextByWorkUnit(currUser, moduleName, entity.WORKUNITID, entity.NodeId, false);
                }
                else
                {
                    string moduleName = "(外包)设施变动验收审核";
                    //外包验收审核流程
                    mpcEntity = peoplereviewbll.CheckAuditForNextByOutsourcing(currUser, moduleName, entity.WORKUNITID, entity.NodeId, false, true, entity.PROJECTID);
                }

                if (null != mpcEntity)
                {
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.NodeId = mpcEntity.ID;
                    entity.NodeName = mpcEntity.FLOWNAME;
                    entity.ISCOMMIT = 1;
                    entity.ISAPPLYOVER = 1;//申请审核完成
                    entity.FLOWREMARK = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                    string type = entity.FLOWREMARK != "1" ? "0" : "1";
                    new ScaffoldBLL().SendMessage(entity.FLOWDEPT, entity.FLOWROLE, "ZY012", keyValue, "", "", type, !string.IsNullOrEmpty(entity.ACCSPECIALTYTYPE) ? entity.ACCSPECIALTYTYPE : "");
                }
                else
                {
                    entity.FLOWREMARK = "";
                    //未配置审核项
                    entity.FLOWDEPT = "";
                    entity.FLOWDEPTNAME = "";
                    entity.FLOWROLE = "";
                    entity.FLOWROLENAME = "";
                    entity.NodeId = "";
                    entity.NodeName = "已完结";
                    //entity.ISACCEPOVER = 1;
                    entity.ISCOMMIT = 1;
                    entity.ISACCPCOMMIT = 1;
                    entity.ISACCEPOVER = 1;//验收审核完成
                    entity.ISAPPLYOVER = 1;//申请审核完成
                }
                safetychangebll.SaveForm(keyValue, entity);
            }

            return Success("操作成功。");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(8, "安全设施变动申请提交")]
        [AjaxOnly]
        public ActionResult SubmitAppLyAudit(string keyValue, SafetychangeEntity change, ScaffoldauditrecordEntity entity)
        {
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = string.Empty;
            if (change.WORKUNITTYPE == "0")
            {
                moduleName = "(内部)设施变动申请审核";
            }
            else
            {
                moduleName = "(外包)设施变动申请审核";
            }
            entity.FlowId = change.NodeId;
            //更改申请信息状态
            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditForNextByWorkUnit(currUser, moduleName, change.WORKUNITID, change.NodeId, false);
            //同意进行下一步
            if (entity.AuditState == 0)
            {
                //下一步流程不为空
                if (null != mpcEntity)
                {
                    change.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    change.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    change.FLOWROLE = mpcEntity.CHECKROLEID;
                    change.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    change.NodeId = mpcEntity.ID;
                    change.NodeName = mpcEntity.FLOWNAME;
                    change.ISCOMMIT = 1;
                    change.FLOWREMARK = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                    string type = change.FLOWREMARK != "1" ? "0" : "1";
                    new ScaffoldBLL().SendMessage(change.FLOWDEPT, change.FLOWROLE, "ZY011", keyValue, "", "", type, !string.IsNullOrEmpty(change.SPECIALTYTYPE) ? change.SPECIALTYTYPE : "");
                }
                else
                {
                    change.FLOWREMARK = "";
                    change.FLOWDEPT = " ";
                    change.FLOWDEPTNAME = " ";
                    change.FLOWROLE = " ";
                    change.FLOWROLENAME = " ";
                    //change.NodeId = " ";
                    change.NodeName = "已完结";
                    change.ISAPPLYOVER = 1;
                    change.ISCOMMIT = 1;
                }
            }
            else
            {
                change.FLOWREMARK = "";
                change.FLOWDEPT = " ";
                change.FLOWDEPTNAME = " ";
                change.FLOWROLE = " ";
                change.FLOWROLENAME = " ";
                //change.NodeId = " ";
                change.NodeName = "已完结";
                change.ISAPPLYOVER = 2;
                change.ISCOMMIT = 1;

                //审批不通过,推消息到申请人
                var high = safetychangebll.GetEntity(keyValue);
                if (high != null)
                {
                    UserEntity userEntity = new UserBLL().GetEntity(high.CREATEUSERID);
                    if (userEntity != null)
                    {
                        JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY013", keyValue);
                    }
                }
            }
            safetychangebll.SaveForm(keyValue, change);
            entity.ScaffoldId = keyValue;
            entity.AuditSignImg = string.IsNullOrWhiteSpace(entity.AuditSignImg) ? "" : entity.AuditSignImg.ToString().Replace("../..", "");
            scaffoldauditrecordbll.SaveForm(entity.Id, entity);
            return Success("操作成功。");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(8, "安全设施变动验收提交")]
        [AjaxOnly]
        public ActionResult SubmitAccpAudit(string keyValue, SafetychangeEntity change, ScaffoldauditrecordEntity entity)
        {
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = string.Empty;
            if (change.WORKUNITTYPE == "0")
            {
                moduleName = "(内部)设施变动验收审核";
            }
            else
            {
                moduleName = "(外包)设施变动验收审核";
            }
            entity.FlowId = change.NodeId;
            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditForNextByWorkUnit(currUser, moduleName, change.WORKUNITID, change.NodeId, false);
            //同意进行下一步
            if (entity.AuditState == 0)
            {
                //下一步流程不为空
                if (null != mpcEntity)
                {
                    change.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    change.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    change.FLOWROLE = mpcEntity.CHECKROLEID;
                    change.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    change.NodeId = mpcEntity.ID;
                    change.NodeName = mpcEntity.FLOWNAME;
                    change.ISCOMMIT = 1;
                    change.ISACCPCOMMIT = 1;

                    change.FLOWREMARK = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                    string type = change.FLOWREMARK != "1" ? "0" : "1";
                    new ScaffoldBLL().SendMessage(change.FLOWDEPT, change.FLOWROLE, "ZY012", keyValue, "", "", type, !string.IsNullOrEmpty(change.ACCSPECIALTYTYPE) ? change.ACCSPECIALTYTYPE : "");
                }
                else
                {
                    change.FLOWREMARK = "";
                    change.FLOWDEPT = " ";
                    change.FLOWDEPTNAME = " ";
                    change.FLOWROLE = " ";
                    change.FLOWROLENAME = " ";
                    //change.NodeId = " ";
                    change.NodeName = "已完结";
                    change.ISAPPLYOVER = 1;
                    change.ISCOMMIT = 1;
                    change.ISACCPCOMMIT = 1;
                    change.ISACCEPOVER = 1;
                }
            }
            else
            {
                change.FLOWREMARK = "";
                change.FLOWDEPT = " ";
                change.FLOWDEPTNAME = " ";
                change.FLOWROLE = " ";
                change.FLOWROLENAME = " ";
                //change.NodeId = " ";
                change.NodeName = "已完结";
                change.ISAPPLYOVER = 1;
                change.ISCOMMIT = 1;
                change.ISACCPCOMMIT = 1;
                change.ISACCEPOVER = 2;

                //审批不通过,推消息到申请人
                var high = safetychangebll.GetEntity(keyValue);
                if (high != null)
                {
                    UserEntity userEntity = new UserBLL().GetEntity(high.CREATEUSERID);
                    if (userEntity != null)
                    {
                        JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY014", keyValue);
                    }
                }
            }
            safetychangebll.SaveForm(keyValue, change);
            entity.AuditSignImg = string.IsNullOrWhiteSpace(entity.AuditSignImg) ? "" : entity.AuditSignImg.ToString().Replace("../..", "");
            entity.ScaffoldId = keyValue;
            scaffoldauditrecordbll.SaveForm(entity.Id, entity);
            return Success("操作成功。");
        }


        [HttpPost]
        [AjaxOnly]
        public ActionResult GetFlowData(string keyValue)
        {
            try
            {
                SafetychangeEntity entity = safetychangebll.GetEntity(keyValue);
                string projectid = "";
                List<string> modulename = new List<string>();
                //0：单位内部 1外包单位
                if (entity.WORKUNITTYPE == "0")
                {
                    modulename.Add("(内部)设施变动申请审核");
                    modulename.Add("(内部)设施变动验收审核");
                }
                else
                {
                    modulename.Add("(外包)设施变动申请审核");
                    modulename.Add("(外包)设施变动验收审核");
                    projectid = entity.PROJECTID;
                }

                var josnData = safetychangebll.GetFlow(entity.ID, modulename);
                return Content(josnData.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        #endregion


        #region 导出
        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "Id";
                pagination.p_fields = @"  case 
                                                 when t.iscommit=0 then '变动申请中'
                                                 when t.iscommit=1 and t.isapplyover=0  then '变动审核(批)中'
                                                 when t.iscommit=1 and t.isapplyover=2  then '变动审核(批)未通过'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=0 and t.isaccepover=0 then '变动审核(批)通过待验收'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=0 then '验收审核中'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=2 then '验收审核(批)未通过'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=1 then '验收审核(批)通过' else '' end resultstate,applyno,projectname,changename,changetype,applychangetime,returntime,applypeople,applytime,acceppeople";
                pagination.p_tablename = @"   bis_safetychange t";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = "1=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and applypeopleid='" + user.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and workunitcode='" + user.DeptCode + "'";
                                break;
                            case "3"://本子部门
                                pagination.conditionJson += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                                break;
                            case "4":
                                pagination.conditionJson += " and t.createuserorgcode='" + user.OrganizeCode + "'";
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }

                DataTable exportTable = safetychangebll.GetPageList(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "安全设施变动(验收)信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "安全设施变动(验收)信息.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "resultstate", ExcelColumn = "作业许可状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyno", ExcelColumn = "申请编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "projectname", ExcelColumn = "工程名称", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changename", ExcelColumn = "需变动的安全设施名称", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changetype", ExcelColumn = "变动形式", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applychangetime", ExcelColumn = "申请变动时间", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "returntime", ExcelColumn = "恢复时间", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applypeople", ExcelColumn = "变动申请人", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applytime", ExcelColumn = "变动申请时间", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "acceppeople", ExcelColumn = "验收申请人", Width = 20 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        /// <summary>
        /// 导出详情
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出详情")]
        public ActionResult ExportDetails(string keyValue)
        {
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/安全设施变动模板.docx"));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            var entity = new SafetychangeBLL().GetEntity(keyValue);
            var applyaudit = scaffoldauditrecordbll.GetApplyAuditList(keyValue, 0);
            var accpaudit = scaffoldauditrecordbll.GetApplyAuditList(keyValue, 1);
            DataTable dt = new DataTable();
            dt.Columns.Add("applytime");
            dt.Columns.Add("applyno");
            dt.Columns.Add("workunit");
            dt.Columns.Add("workfzr");
            dt.Columns.Add("workcontent");
            dt.Columns.Add("changename");
            dt.Columns.Add("procedures");
            dt.Columns.Add("applyfirst");
            dt.Columns.Add("onetime");
            dt.Columns.Add("applysecond");
            dt.Columns.Add("twotime");
            dt.Columns.Add("applythree");
            dt.Columns.Add("threetime");
            dt.Columns.Add("acceptance");
            dt.Columns.Add("accpfirst");
            dt.Columns.Add("fourtime");
            dt.Columns.Add("accpsecond");
            dt.Columns.Add("fivetime");
            dt.Columns.Add("accpthree");
            dt.Columns.Add("sixtime");
            DataRow row = dt.NewRow();

            string time = "年   月   日   时   分";
            string defaulttimestr = "yyyy年MM月dd日HH时mm分";
            string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
            string onetime = time, twotime = time, threetime = time, fourtime = time, fivetime = time, sixtime = time;

            if (applyaudit.Count > 0)
            {
                for (int i = 0; i < applyaudit.Count; i++)
                {
                    var filepath = applyaudit[i].AuditSignImg == null ? "" : (Server.MapPath("~/") + applyaudit[i].AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    var stime = Convert.ToDateTime(applyaudit[i].AuditDate);
                    if (i == 0)
                    {
                        //applyfirst = applyaudit[i].AuditUserName;
                        if (System.IO.File.Exists(filepath))
                        {
                            row["applyfirst"] = filepath;
                        }
                        else
                        {
                            row["applyfirst"] = pic;
                        }
                        onetime = stime.ToString(defaulttimestr);
                    }
                    else if (i == 1)
                    {
                        if (System.IO.File.Exists(filepath))
                        {
                            row["applysecond"] = filepath;
                        }
                        else
                        {
                            row["applysecond"] = pic;
                        }
                        //applysecond = applyaudit[i].AuditUserName;
                        twotime = stime.ToString(defaulttimestr);
                    }
                    else
                    {
                        if (System.IO.File.Exists(filepath))
                        {
                            row["applythree"] = filepath;
                        }
                        else
                        {
                            row["applythree"] = pic;
                        }
                        //applythree = applyaudit[i].AuditUserName;
                        threetime = stime.ToString(defaulttimestr);
                    }
                }
            }
            if (accpaudit.Count > 0)
            {
                for (int i = 0; i < accpaudit.Count; i++)
                {
                    var filepath = accpaudit[i].AuditSignImg == null ? "" : (Server.MapPath("~/") + accpaudit[i].AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    var stime = Convert.ToDateTime(accpaudit[i].AuditDate);
                    if (i == 0)
                    {
                        if (System.IO.File.Exists(filepath))
                        {
                            row["accpfirst"] = filepath;
                        }
                        else
                        {
                            row["accpfirst"] = pic;
                        }
                        //accpfirst = accpaudit[i].AuditUserName;
                        fourtime = stime.ToString(defaulttimestr);
                    }
                    else if (i == 1)
                    {
                        if (System.IO.File.Exists(filepath))
                        {
                            row["accpsecond"] = filepath;
                        }
                        else
                        {
                            row["accpsecond"] = pic;
                        }
                        //accpsecond = accpaudit[i].AuditUserName;
                        fivetime = stime.ToString(defaulttimestr);
                    }
                    else
                    {
                        if (System.IO.File.Exists(filepath))
                        {
                            row["accpthree"] = filepath;
                        }
                        else
                        {
                            row["accpthree"] = pic;
                        }
                        //accpthree = accpaudit[i].AuditUserName;
                        sixtime = stime.ToString(defaulttimestr);
                    }
                }
            }
            row["applytime"] = entity.APPLYTIME.Value.Year + "年" + entity.APPLYTIME.Value.Month + "月" + entity.APPLYTIME.Value.Day + "日";
            row["applyno"] = entity.APPLYNO;
            row["workunit"] = entity.WORKUNIT;
            row["workfzr"] = entity.WORKFZR;
            row["workcontent"] = entity.WORKCONTENT;
            row["changename"] = entity.CHANGENAME + entity.CHANGETYPE;
            row["procedures"] = entity.PROCEDURES;
            row["acceptance"] = entity.ACCEPTANCE;
            row["onetime"] = onetime;
            row["twotime"] = twotime;
            row["threetime"] = threetime;
            row["fourtime"] = fourtime;
            row["fivetime"] = fivetime;
            row["sixtime"] = sixtime;
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            //doc.MailMerge.Execute(new string[] { "applytime", "applyno", "workunit", "workfzr", "workcontent", "changename", "procedures", "applyfirst", "onetime", "applysecond", "twotime", "applythree", "threetime", "acceptance", "accpfirst", "fourtime", "accpsecond", "fivetime", "accpthree", "sixtime" }
            //, new string[] { applytime, applyno, workunit, workfzr, workcontent, changename, procedures, applyfirst, onetime, applysecond, twotime, applythree, threetime, acceptance, accpfirst, fourtime, accpsecond, fivetime, accpthree, sixtime });
            string fileName = "安全设施变动（验收）审批单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return Success("导出成功!");
        }
        #endregion


        #region 安全设施变动台账
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetLedgerListJson(Pagination pagination, string queryJson)
        {
            var data = safetychangebll.GetLedgerList(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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

        #region 导出
        /// <summary>
        /// 导出台账
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportChangeLedgerData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable data = safetychangebll.GetLedgerList(pagination, queryJson);
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("applyno"));
                excelTable.Columns.Add(new DataColumn("ledgertype"));
                excelTable.Columns.Add(new DataColumn("workunit"));
                excelTable.Columns.Add(new DataColumn("workunittypename"));
                excelTable.Columns.Add(new DataColumn("changename"));
                excelTable.Columns.Add(new DataColumn("changetype"));
                excelTable.Columns.Add(new DataColumn("worktime"));
                excelTable.Columns.Add(new DataColumn("realityworktime"));
                excelTable.Columns.Add(new DataColumn("workplace"));



                foreach (DataRow item in data.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    newDr["applyno"] = item["applyno"];
                    newDr["ledgertype"] = item["ledgertype"];
                    newDr["workunit"] = item["workunit"];
                    newDr["workunittypename"] = item["workunittypename"];
                    newDr["changename"] = item["changename"];
                    newDr["changetype"] = item["changetype"];


                    DateTime applychangetime, returntime, realitychangetime, checkdate;
                    DateTime.TryParse(item["applychangetime"].ToString(), out applychangetime);
                    DateTime.TryParse(item["returntime"].ToString(), out returntime);
                    DateTime.TryParse(item["realitychangetime"].ToString(), out realitychangetime);
                    DateTime.TryParse(item["checkdate"].ToString(), out checkdate);

                    string worktime = string.Empty;
                    if (applychangetime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        worktime += applychangetime.ToString("yyyy-MM-dd HH:mm") + " - ";
                    }
                    if (returntime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        worktime += returntime.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["worktime"] = worktime;
                    string realityworktime = string.Empty;
                    if (realitychangetime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        realityworktime += realitychangetime.ToString("yyyy-MM-dd HH:mm") + " - ";
                    }
                    if (checkdate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        realityworktime += checkdate.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["realityworktime"] = realityworktime;
                    newDr["workplace"] = item["workplace"];

                    excelTable.Rows.Add(newDr);
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "安全设施变动台账";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "安全设施变动台账.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyno", ExcelColumn = "申请编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ledgertype", ExcelColumn = "变动状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workunit", ExcelColumn = "作业单位", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workunittypename", ExcelColumn = "作业单位类别", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changename", ExcelColumn = "变动安全设施名称", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changetype", ExcelColumn = "变动形式", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktime", ExcelColumn = "申请变动时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realityworktime", ExcelColumn = "实际变动时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workplace", ExcelColumn = "作业地点", Width = 60 });
                //调用导出方法
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion
        #endregion
    }
}
