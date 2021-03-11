using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.WorkPlan;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.WorkPlan;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.WorkPlan.Controllers
{
    /// <summary>
    /// 描 述：EHS计划申请表
    /// </summary>
    public class PlanApplyController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private PlanApplyBLL planApplyBll = new PlanApplyBLL();

        #region 视图功能
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        /// 部门工作计划流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DepartFlowDetail()
        {
            return View();
        }
        /// <summary>
        /// 个人工作计划流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PersonFlowDetail()
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
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
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
        /// 变更工作计划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangeForm()
        {
            return View();
        }
        /// <summary>
        /// 变更历史
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult History()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取部门流程图对象
        /// </summary>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetDepartPlanApplyActionList(string keyValue)
        {
            var josnData = htworkflowbll.GetDepartPlanApplyActionList(keyValue);
            return Content(josnData.ToJson());
        }
        /// <summary>
        /// 获取个人流程图对象
        /// </summary>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetPersonPlanApplyActionList(string keyValue)
        {
            var josnData = htworkflowbll.GetPersonPlanApplyActionList(keyValue);
            return Content(josnData.ToJson());
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();            
            var data = planApplyBll.GetList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = planApplyBll.GetEntity(keyValue);                  
            //返回值
            var josnData = new
            {
                data = data
            };

            return Content(josnData.ToJson());
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
            planApplyBll.RemoveForm(keyValue);//删除申请
            new PlanDetailsBLL().RemoveFormByApplyId(keyValue);//删除详情
            new PlanCheckBLL().RemoveForm(keyValue);//删除审核           
            htworkflowbll.DeleteWorkFlowObj(keyValue);//删除流程

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
        public ActionResult SaveForm(string keyValue, PlanApplyEntity entity)
        {
            string workflow = entity.ApplyType == "部门工作计划" ? "06" : "07";
            CommonSaveForm(keyValue, workflow, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, PlanApplyEntity entity)
        {
            string workflow = entity.ApplyType == "部门工作计划" ? "06" : "07";
            CommonSaveForm(keyValue, workflow, entity);
            //创建完流程实例后
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }
            string errorMsg = "";
            if (workflow == "06")
            {//部门工作计划
                errorMsg = DepartFlow(keyValue, entity);
            }
            else
            {//个人工作计划
                errorMsg = PersonFlow(keyValue, entity);
            }

            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }

            return Success("操作成功。");
        }
        /// <summary>
        /// 变更工作计划
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitChangeForm(string keyValue, PlanApplyEntity entity)
        {
            string workflow = entity.ApplyType == "部门工作计划" ? "06" : "07";  
            //保存历史记录
            var oldEntity = planApplyBll.GetEntity(keyValue);
            if (oldEntity != null)
            {
                var newApplyId = Request["NewId"];
                oldEntity.ID = newApplyId;
                oldEntity.BaseId = entity.ID;                
                planApplyBll.SaveForm("", oldEntity);

                var planDetailsBll = new PlanDetailsBLL();
                var list = planDetailsBll.GetList(string.Format(" and baseid in(select id from hrs_plandetails d where d.applyid='{0}') and not exists(select 1 from hrs_planapply a where a.id=hrs_plandetails.applyid)", keyValue)).ToList();
                foreach(var epd in list)
                {//纠正因未提交丢失的历史数据。
                    epd.ApplyId = newApplyId;
                    planDetailsBll.SaveForm(epd.ID, epd);
                }
            }
            //删除旧流程创建新流程   
            htworkflowbll.DeleteWorkFlowObj(keyValue);
            CommonSaveForm(keyValue, workflow, entity);
            //创建完流程实例后
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }
            string errorMsg = "";
            if (workflow == "06")
            {//部门工作计划
                errorMsg = DepartFlow(keyValue, entity);
            }
            else
            {//个人工作计划
                errorMsg = PersonFlow(keyValue, entity);
            }

            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }            

            return Success("操作成功。");
        }
        /// <summary>
        /// 部门工作计划推送流程
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string DepartFlow(string keyValue, PlanApplyEntity entity)
        {
            var errorMsg = "";

            //此处需要判断当前人角色。
            string wfFlag = string.Empty;
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            string participant = string.Empty;

            //EHS部门encode
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();           
            //分管领导
            //var fgLeaderUser = userbll.GetListForCon(x => x.RoleName.Contains("公司级用户") && x.RoleName.Contains("安全管理员")).Select(x => x.Account).ToList();        
            //安全管理员、部门负责人
            //var isSafer = userbll.HaveRoleListByKey(curUser.UserId, "'100105'").Rows.Count > 0;
            var isManager = userbll.HaveRoleListByKey(curUser.UserId, "'100104'").Rows.Count > 0;
            //流程推进
            if (ehsDepart != null && ehsDepart.ItemValue == curUser.DeptCode&&isManager == true) {
                wfFlag = "3"; //申请=>审批
                errorMsg = ">分管领导";
                //分管领导
                var fgLeaderUser = userbll.GetListForCon(x => x.RoleName.Contains("公司级用户") && x.RoleName.Contains("安全管理员")).Select(x => x.Account).ToList();
                participant = string.Join(",", fgLeaderUser);
            }
            else if ((ehsDepart != null && ehsDepart.ItemValue == curUser.DeptCode) || (curUser.RoleName.Contains("部门级") && isManager == true))
            {//EHS部
                wfFlag = "2"; //申请=>EHS负责人审核
                errorMsg = ">EHS负责人";
                var deptUser = userbll.GetUserListByRole(ehsDepart.ItemValue, "'100104'", "");
                participant = string.Join(",", deptUser.Select(x => x.Account));
            }
            else if (curUser.RoleName.Contains("承包商"))
            {
                //承包商走所有工程的责任部门
                wfFlag = "1";//申请=>部门负责人审核
                errorMsg = "工程责任部门负责人";
                var projectList = new OutsouringengineerBLL().GetList().Where(x => x.OUTPROJECTID == curUser.DeptId).ToList() ;
                var deptUser = new List<UserEntity>();
                for (int i = 0; i < projectList.Count; i++)
                {
                    var pDept = new DepartmentBLL().GetEntity(projectList[i].ENGINEERLETDEPTID);
                    if (pDept != null) {
                        deptUser = userbll.GetUserListByRole(pDept.EnCode, "'100104'", "").ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(participant))
                    {
                        participant += "," + string.Join(",", deptUser.Select(x => x.Account));
                    }
                    else {
                        participant +=  string.Join(",", deptUser.Select(x => x.Account));
                    }
               
                }
            }
            else {
                //安全管理员
                wfFlag = "1";//申请=>部门负责人审核
                errorMsg = "部门负责人";
                if (curUser.RoleName.Contains("部门级"))
                {
                    var deptUser = userbll.GetUserListByRole(curUser.DeptCode, "'100104'", "");
                    participant = string.Join(",", deptUser.Select(x => x.Account));
                }
                else {
                    var pDept = new DepartmentBLL().GetParentDeptBySpecialArgs(curUser.ParentId, "部门");
                    var deptUser = userbll.GetUserListByRole(pDept.DeptCode, "'100104'", "");
                    participant = string.Join(",", deptUser.Select(x => x.Account));
                }
                
            }

            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckUserAccount = participant;
                planApplyBll.SaveForm(keyValue, entity);
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_PlanApply", "flowstate", keyValue);  //更新业务流程状态
                }
                errorMsg = "";
            }

            return errorMsg;
        }
        /// <summary>
        /// 个人工作计划推送流程
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string PersonFlow(string keyValue, PlanApplyEntity entity)
        {
            var errorMsg = "";

            //此处需要判断当前人角色。
            string wfFlag = string.Empty;
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();        
            //参与人员
            string participant = string.Empty;
            //流程推进
            if (userbll.HaveRoleListByKey(curUser.UserId, "'100108'").Rows.Count > 0)
            {//公司级用户
                wfFlag = "1";  // 申请=>公司负责人审批
                errorMsg = "公司负责人";
                var deptUser = userbll.GetUserListByRole(curUser.DeptCode, "'100104'", "");
                participant = string.Join(",", deptUser.Select(x => x.Account));
            }
            else if (userbll.HaveRoleListByKey(curUser.UserId, "'100104'").Rows.Count > 0)
            { //部门负责人
                wfFlag = "1";  // 申请=>上级领导审批
                errorMsg = "上级领导";
                //找当前用户上级部门负责人。            
                var dept = new DepartmentBLL().GetEntity(curUser.ParentId);
                if (dept != null)
                {
                    var deptUser = userbll.GetUserListByRole(dept.EnCode, "'100104'", "");
                    participant = string.Join(",", deptUser.Select(x => x.Account));
                }
            }            
            else
            { //其他用户
                wfFlag = "1";  // 申请=>本部门负责人审批
                errorMsg = "部门负责人";                
                var deptUser = userbll.GetUserListByRole(curUser.DeptCode, "'100104'", "");
                participant = string.Join(",", deptUser.Select(x => x.Account));
            }

            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckUserAccount = participant;
                planApplyBll.SaveForm(keyValue, entity);

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_planapply", "flowstate", keyValue);  //更新业务流程状态
                }
                errorMsg = "";
            }

            return errorMsg;
        }
        /// <summary>
        /// 公用方法，保存数据
        /// </summary>
        public void CommonSaveForm(string keyValue, string workFlow, PlanApplyEntity entity)
        {
            //提交通过
            string userId = OperatorProvider.Provider.Current().UserId;

            //保存基本信息
            planApplyBll.SaveForm(keyValue, entity);

            //创建流程实例
            if (!htworkflowbll.IsHavaWFCurrentObject(entity.ID))
            {
                bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                if (isSucess)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_planapply", "flowstate", entity.ID);  //更新业务流程状态
                }
            }
        }
        #endregion
    }
}
