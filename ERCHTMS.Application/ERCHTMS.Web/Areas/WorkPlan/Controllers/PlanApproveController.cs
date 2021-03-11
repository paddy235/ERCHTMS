using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.WorkPlan;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.WorkPlan;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.WorkPlan.Controllers
{
    /// <summary>
    /// 描 述：标准修编申请表
    /// </summary>
    public class PlanApproveController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DepartmentBLL deptbll = new DepartmentBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private PlanApplyBLL planApplybll = new PlanApplyBLL();
        private PlanCheckBLL planCheckbll = new PlanCheckBLL();

        #region 视图功能        
        /// <summary>
        /// 1级审核
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 2级审核
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form2()
        {
            return View();
        }
        /// <summary>
        /// 审批
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormFin()
        {
            return View();
        }        
        #endregion

        #region 获取数据        
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var baseInfo = planApplybll.GetEntity(keyValue);           
           
            //返回值
            var josnData = new
            {
                baseInfo
            };

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
            var data = planCheckbll.GetList(pagination, queryJson);
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
        #endregion

        #region 提交数据       
        /// <summary>
        /// 部门计划1级审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitDForm(string keyValue, PlanCheckEntity pEntity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";           
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          

            var entity = planApplybll.GetEntity(pEntity.ApplyId);
            if (pEntity.CheckResult == "0")
            {//不通过,退回
                var appEntity = planApplybll.GetEntity(keyValue);
                wfFlag = "2";
                string createuserid = appEntity.CREATEUSERID;
                var userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //申请用户              
                errorMsg = "上报用户";
            }
            else
            {//部门负责人，核准通过
                wfFlag = "1";  // 1级审核=>2级审核
                errorMsg = "EHS部门负责人";
                //EHS部门encode
                DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
                if (ehsDepart != null)
                {
                    var deptUser = userbll.GetUserListByRole(ehsDepart.ItemValue, "'100104'", "");
                    if (deptUser != null)
                    {
                        participant = string.Join(",", deptUser.Select(x => x.Account));                     
                    }
                }
            }
            //保存基本信息
            pEntity.ID = Guid.NewGuid().ToString();
            planCheckbll.SaveForm("", pEntity);
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckUserAccount = participant;
                planApplybll.SaveForm(keyValue, entity);
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_planapply", "flowstate", keyValue);  //更新业务流程状态
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }            

            return Success("操作成功。");
        }
        /// <summary>
        /// 部门计划2级审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitDForm2(string keyValue, PlanCheckEntity pEntity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          

            var entity = planApplybll.GetEntity(pEntity.ApplyId);
            if (pEntity.CheckResult == "0")
            {//不通过,退回     
                var appEntity = planApplybll.GetEntity(keyValue);
                string createuserid = appEntity.CREATEUSERID;
                var userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //申请用户    
                wfFlag = "3";
                errorMsg = "上报用户";
                if (pEntity.CheckBackType == "回退上一级")
                {
                    var cnt = planCheckbll.GetList(string.Format(" and applyid='{0}' and checktype='1级审核'", pEntity.ApplyId)).Count();
                    if (cnt > 0)
                    {
                        wfFlag = "2";
                        errorMsg = "部门负责人";
                        var deptUser = userbll.GetUserListByRole(appEntity.CREATEUSERDEPTCODE, "'100104'", "");
                        participant = string.Join(",", deptUser.Select(x => x.Account));
                    }
                }
            }
            else
            {//EHS部门负责人，核准通过
                wfFlag = "1";  // 2级审核=>审批
                errorMsg = "分管领导";
                //分管领导
                var fgLeaderUser = userbll.GetListForCon(x => x.RoleName.Contains("公司级用户") && x.RoleName.Contains("安全管理员")).Select(x => x.Account).ToList();
                if (fgLeaderUser != null)
                {
                    participant = string.Join(",", fgLeaderUser);
                }
            }
            //保存基本信息
            pEntity.ID = Guid.NewGuid().ToString();
            planCheckbll.SaveForm("", pEntity);
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckUserAccount = participant;
                planApplybll.SaveForm(keyValue, entity);
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_planapply", "flowstate", keyValue);  //更新业务流程状态
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }

            return Success("操作成功。");
        }
        /// <summary>
        /// 部门计划审批
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitDFormFin(string keyValue, PlanCheckEntity pEntity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          

            var entity = planApplybll.GetEntity(pEntity.ApplyId);
            if (pEntity.CheckResult == "0")
            {//不通过,退回                
                if (pEntity.CheckBackType == "回退申请人")
                {
                    wfFlag = "3";
                    errorMsg = "上报用户";
                    var appEntity = planApplybll.GetEntity(keyValue);
                    string createuserid = appEntity.CREATEUSERID;
                    var userEntity = userbll.GetEntity(createuserid);
                    participant = userEntity.Account;  //申请用户   
                }
                else
                {
                    wfFlag = "2";
                    errorMsg = "EHS部门负责人";
                    //EHS部门encode
                    DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
                    if (ehsDepart != null)
                    {
                        var deptUser = userbll.GetUserListByRole(ehsDepart.ItemValue, "'100104'", "");
                        participant = string.Join(",", deptUser.Select(x => x.Account));
                    }
                }   
            }
            else
            {//分管领导审批通过
                wfFlag = "1";  // 审批=>结束
                errorMsg = "分管领导";
                participant = curUser.Account;
                //更新变化状态
                new PlanDetailsBLL().UpdateChangedData(pEntity.ApplyId);
            }
            //保存基本信息
            pEntity.ID = Guid.NewGuid().ToString();
            planCheckbll.SaveForm("", pEntity);
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckUserAccount = participant;                
                planApplybll.SaveForm(keyValue, entity);
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_planapply", "flowstate", keyValue);  //更新业务流程状态
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }

            return Success("操作成功。");
        }
        /// <summary>
        /// 个人计划上级领导审批
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitPForm(string keyValue, PlanCheckEntity pEntity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          

            var entity = planApplybll.GetEntity(pEntity.ApplyId);
            if (pEntity.CheckResult == "0")
            {//不通过,退回
                var appEntity = planApplybll.GetEntity(keyValue);
                wfFlag = "2";
                string createuserid = appEntity.CREATEUSERID;
                var userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //申请用户              
                errorMsg = "上报用户";
            }
            else
            {
                wfFlag = "1";  // 上级领导审批=>结束
                errorMsg = "上级领导";
                participant = curUser.Account;
                //更新变化状态
                new PlanDetailsBLL().UpdateChangedData(pEntity.ApplyId);                
            }
            //保存基本信息
            pEntity.ID = Guid.NewGuid().ToString();
            planCheckbll.SaveForm("", pEntity);
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckUserAccount = participant;
                planApplybll.SaveForm(keyValue, entity);
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_planapply", "flowstate", keyValue);  //更新业务流程状态
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }

            return Success("操作成功。");
        }
        #endregion
    }
}