using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.StandardSystem;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    /// <summary>
    /// 描 述：标准修编申请表
    /// </summary>
    public class StandardApproveController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DepartmentBLL deptbll = new DepartmentBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private StandardApplyBLL standardApplybll = new StandardApplyBLL();
        private StandardCheckBLL standardCheckbll = new StandardCheckBLL();

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
        /// 审核分配会签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form3()
        {
            return View();
        }
        /// <summary>
        /// 部门会签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SignForm()
        {
            return View();
        }
        /// <summary>
        /// 分配分委会
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DistributeForm()
        {
            return View();
        }
        /// <summary>
        /// 分委会审核
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CommitteeForm()
        {
            return View();
        }
        /// <summary>
        /// 总经理审批
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PresidentForm()
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
            var baseInfo = standardApplybll.GetEntity(keyValue);
            object checkInfo = new object();// standardCheckbll.GetLastEntityByRecId(keyValue, "1级审核");
            var checkBackType = new List<object>() {
                new { ItemId = "逐级提交", ItemName = "逐级提交" },
                new { ItemId = "驳回人", ItemName = "驳回人" }
            };

            //返回值
            var josnData = new
            {
                baseInfo,
                checkInfo,
                checkBackType
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
            var data = standardCheckbll.GetList(pagination, queryJson);
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
        /// 1级审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, StandardCheckEntity pEntity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";
            string checkUserId = "";
            string checkUserName = "";
            string checkDeptId = "";
            string checkDeptName = "";
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          

            var entity = standardApplybll.GetEntity(pEntity.RecID);
            if (pEntity.CheckResult == "0")
            {//不通过,退回
                var appEntity = standardApplybll.GetEntity(keyValue);                
                wfFlag = "2";
                string createuserid = appEntity.CREATEUSERID;
                var userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //申请用户
                checkUserId = userEntity.UserId;
                checkUserName = userEntity.RealName;
                checkDeptId = userEntity.DepartmentId;
                checkDeptName = deptbll.GetEntity(userEntity.DepartmentId).FullName;
                errorMsg = "申请用户";

                if (pEntity.CheckBackType == "驳回人")
                {
                    entity.CheckBackFlag = "1";
                    entity.CheckBackUserID = curUser.UserId;
                    entity.CheckBackUserName = curUser.UserName;
                    entity.CheckBackDeptID = curUser.DeptId;
                    entity.CheckBackDeptName = curUser.DeptName;
                }
                else
                {
                    entity.CheckBackFlag = entity.CheckBackUserID = entity.CheckBackUserName = entity.CheckBackDeptID = entity.CheckBackDeptName = "";
                }
            }
            else 
            {//副主任/主管，核准通过
                wfFlag = "1";  // 1级审核=>2级审核
                errorMsg = "主任/主管";
                //找当前用户所有部门，不是所在班组或专业。            
                var dept = new DepartmentBLL().GetDepts(curUser.OrganizeId).Where(x => x.Nature == "部门" && curUser.DeptCode.StartsWith(x.EnCode)).FirstOrDefault();
                if (dept != null)
                {
                    var mgUser = userbll.GetUserListByRole(dept.EnCode, "'100104'", "");
                    if (mgUser != null)
                    {
                        participant = string.Join(",", mgUser.Select(x => x.Account));
                        checkUserId = string.Join(",", mgUser.Select(x => x.UserId));
                        checkUserName = string.Join(",", mgUser.Select(x => x.RealName));
                        checkDeptId = curUser.DeptId;
                        checkDeptName = curUser.DeptName;
                    }
                }
            }
            //保存基本信息
            pEntity.ID = Guid.NewGuid().ToString();
            standardCheckbll.SaveForm("", pEntity);
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckDeptID = checkDeptId;
                entity.CheckDeptName = checkDeptName;
                entity.CheckUserID = checkUserId;
                entity.CheckUserName = checkUserName;
                standardApplybll.SaveForm(entity.ID, entity);              

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }            

            return Success("操作成功。");
        }
        /// <summary>
        /// 2级审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm2(string keyValue, StandardCheckEntity pEntity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";
            string checkUserId = "";
            string checkUserName = "";
            string checkDeptId = "";
            string checkDeptName = "";
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          
            //标准化办公室审核用户
            DataItemModel checkuser = dataitemdetailbll.GetDataItemListByItemCode("'CheckUserAccount'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();

            var entity = standardApplybll.GetEntity(pEntity.RecID);
            if (pEntity.CheckResult == "0")
            {//不通过,退回
                var appEntity = standardApplybll.GetEntity(keyValue);
                wfFlag = "2";
                string createuserid = appEntity.CREATEUSERID;
                var userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //申请用户
                checkUserId = userEntity.UserId;
                checkUserName = userEntity.RealName;
                checkDeptId = userEntity.DepartmentId;
                checkDeptName = deptbll.GetEntity(userEntity.DepartmentId).FullName;
                errorMsg = "申请用户";

                if (pEntity.CheckBackType == "驳回人")
                {
                    entity.CheckBackFlag = "2";
                    entity.CheckBackUserID = curUser.UserId;
                    entity.CheckBackUserName = curUser.UserName;
                    entity.CheckBackDeptID = curUser.DeptId;
                    entity.CheckBackDeptName = curUser.DeptName;
                }
                else
                {
                    entity.CheckBackFlag = entity.CheckBackUserID = entity.CheckBackUserName = entity.CheckBackDeptID = entity.CheckBackDeptName = "";
                }
            }
            else
            {//主任/主管，核准通过
                wfFlag = "1";  // 2级审核=>审核分配会签               
                errorMsg = "标准化办公室审核用户";
                if (checkuser != null)
                {
                    var chkStr = checkuser.ItemValue.Split(new char[] { '|' });
                    participant = chkStr[0];
                    var uList = userbll.GetListForCon(x => participant.Contains(x.Account)).ToList();
                    checkUserId = string.Join(",", uList.Select(x => x.UserId));
                    checkUserName = string.Join(",", uList.Select(x => x.RealName));
                    checkDeptId = chkStr.Length >= 3 ? chkStr[1] : "";
                    checkDeptName = chkStr.Length >= 3 ? chkStr[2] : "";
                }
            }
            //保存基本信息
            pEntity.ID = Guid.NewGuid().ToString();
            standardCheckbll.SaveForm("", pEntity);
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckDeptID = checkDeptId;
                entity.CheckDeptName = checkDeptName;
                entity.CheckUserID = checkUserId;
                entity.CheckUserName = checkUserName;
                standardApplybll.SaveForm(entity.ID, entity);

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }

            return Success("操作成功。");
        }
        /// <summary>
        /// 审核分配会签
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm3(string keyValue, StandardCheckEntity pEntity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";
            string checkUserId = "";
            string checkUserName = "";
            string checkDeptId = "";
            string checkDeptName = "";
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          

            var entity = standardApplybll.GetEntity(pEntity.RecID);
            if (pEntity.CheckResult == "0")
            {//不通过,退回
                var appEntity = standardApplybll.GetEntity(keyValue);
                wfFlag = "2";
                string createuserid = appEntity.CREATEUSERID;
                var userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //申请用户
                checkUserId = userEntity.UserId;
                checkUserName = userEntity.RealName;
                checkDeptId = userEntity.DepartmentId;
                checkDeptName = deptbll.GetEntity(userEntity.DepartmentId).FullName;
                errorMsg = "申请用户";

                if (pEntity.CheckBackType == "驳回人")
                {
                    entity.CheckBackFlag = "3";
                    entity.CheckBackUserID = curUser.UserId;
                    entity.CheckBackUserName = curUser.UserName;
                    entity.CheckBackDeptID = curUser.DeptId;
                    entity.CheckBackDeptName = curUser.DeptName;
                }
                else
                {
                    entity.CheckBackFlag = entity.CheckBackUserID = entity.CheckBackUserName = entity.CheckBackDeptID = entity.CheckBackDeptName = "";
                }
            }
            else
            {//标准化办公室审核用户，核准通过
                wfFlag = "1";  // 审核分配会签=>部门会签               
                errorMsg = "部门会签用户";              
                participant = Request["SignUserAccount"];              
                checkUserId = Request["SignUserID"];
                checkUserName = Request["SignUserName"];
                checkDeptId = Request["SignDeptID"];
                checkDeptName = Request["SignDeptName"];
            }
            //保存基本信息
            pEntity.ID = Guid.NewGuid().ToString();
            standardCheckbll.SaveForm("", pEntity);
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckDeptID = checkDeptId;
                entity.CheckDeptName = checkDeptName;
                entity.CheckUserID = checkUserId;
                entity.CheckUserName = checkUserName;
                standardApplybll.SaveForm(entity.ID, entity);

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }

            return Success("操作成功。");
        }
        /// <summary>
        /// 部门会签
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitSign(string keyValue, StandardCheckEntity pEntity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";
            string checkUserId = "";
            string checkUserName = "";
            string checkDeptId = "";
            string checkDeptName = "";
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          
            //标准化办公室审核用户
            DataItemModel checkuser = dataitemdetailbll.GetDataItemListByItemCode("'CheckUserAccount'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();

            var entity = standardApplybll.GetEntity(pEntity.RecID);
            if (pEntity.CheckResult == "0")
            {//不通过,退回
                var appEntity = standardApplybll.GetEntity(keyValue);
                wfFlag = "2";
                string createuserid = appEntity.CREATEUSERID;
                var userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //申请用户
                checkUserId = userEntity.UserId;
                checkUserName = userEntity.RealName;
                checkDeptId = userEntity.DepartmentId;
                checkDeptName = deptbll.GetEntity(userEntity.DepartmentId).FullName;
                errorMsg = "申请用户";

                if (pEntity.CheckBackType == "驳回人")
                {
                    entity.CheckBackFlag = "4";
                    entity.CheckBackUserID = entity.CheckUserID;
                    entity.CheckBackUserName = entity.CheckUserName;
                    entity.CheckBackDeptID = entity.CheckDeptID;
                    entity.CheckBackDeptName = entity.CheckDeptName;
                }
                else
                {
                    entity.CheckBackFlag = entity.CheckBackUserID = entity.CheckBackUserName = entity.CheckBackDeptID = entity.CheckBackDeptName = "";
                }
            }
            else
            {//部门会签，会签通过
                wfFlag = "1";  // 部门会签=>分配分委会               
                errorMsg = "标准化办公室审核用户";
                if (checkuser != null)
                {
                    var chkStr = checkuser.ItemValue.Split(new char[] { '|' });
                    participant = chkStr[0];
                    var uList = userbll.GetListForCon(x => participant.Contains(x.Account)).ToList();
                    checkUserId = string.Join(",", uList.Select(x => x.UserId));
                    checkUserName = string.Join(",", uList.Select(x => x.RealName));
                    checkDeptId = chkStr.Length >= 3 ? chkStr[1] : "";
                    checkDeptName = chkStr.Length >= 3 ? chkStr[2] : "";

                    //更新会签人员信息            
                    entity.CheckUserName = entity.CheckUserName.Replace(curUser.UserName, string.Format("{0}(已签)", curUser.UserName));
                    standardApplybll.SaveForm(entity.ID, entity);
                }                
            }
            //保存基本信息
            pEntity.ID = Guid.NewGuid().ToString();
            standardCheckbll.SaveForm("", pEntity);            
            if (!string.IsNullOrEmpty(participant))
            {
                if (pEntity.CheckResult == "0")
                {
                    entity.CheckDeptID = checkDeptId;
                    entity.CheckDeptName = checkDeptName;
                    entity.CheckUserID = checkUserId;
                    entity.CheckUserName = checkUserName;
                    standardApplybll.SaveForm(entity.ID, entity);

                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                    }
                }
                else
                {
                    if (standardCheckbll.FinishComplete(entity.CheckUserID, entity.CheckUserName, "已签"))
                    {//会签完成才推送流程
                        entity.CheckDeptID = checkDeptId;
                        entity.CheckDeptName = checkDeptName;
                        entity.CheckUserID = checkUserId;
                        entity.CheckUserName = checkUserName;
                        standardApplybll.SaveForm(entity.ID, entity);

                        int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                        }
                    }
                    else
                    {
                        var uList = userbll.GetListForCon(x => entity.CheckUserID.Contains(x.UserId)).ToList();
                        participant = string.Join(",", uList.Select(x => x.Account));
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(keyValue, participant, curUser.UserId);
                    }
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }            

            return Success("操作成功。");
        }
        /// <summary>
        /// 分配分委会
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitDistribute(string keyValue)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";
            string checkUserId = "";
            string checkUserName = "";
            string checkDeptId = "";
            string checkDeptName = "";
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          

            var entity = standardApplybll.GetEntity(keyValue);
           //分配分委会
            wfFlag = "1";  // 分配分委会=>分委会审核
            errorMsg = "分委会用户";
            participant = Request["DistributeUserAccount"];
            checkUserId = Request["DistributeUserID"];
            checkUserName = Request["DistributeUserName"];
            checkDeptId = Request["DistributeDeptID"];
            checkDeptName = Request["DistributeDeptName"];
          
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckDeptID = checkDeptId;
                entity.CheckDeptName = checkDeptName;
                entity.CheckUserID = checkUserId;
                entity.CheckUserName = checkUserName;
                standardApplybll.SaveForm(entity.ID, entity);
              
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }

            return Success("操作成功。");
        }
        /// <summary>
        /// 分委会审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitCommittee(string keyValue, StandardCheckEntity pEntity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";
            string checkUserId = "";
            string checkUserName = "";
            string checkDeptId = "";
            string checkDeptName = "";
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          
            //总经理用户
            DataItemModel president = dataitemdetailbll.GetDataItemListByItemCode("'PresidentApprove'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();

            var entity = standardApplybll.GetEntity(pEntity.RecID);
            if (pEntity.CheckResult == "0")
            {//不通过,退回
                var appEntity = standardApplybll.GetEntity(keyValue);
                wfFlag = "2";
                string createuserid = appEntity.CREATEUSERID;
                var userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //申请用户
                checkUserId = userEntity.UserId;
                checkUserName = userEntity.RealName;
                checkDeptId = userEntity.DepartmentId;
                checkDeptName = deptbll.GetEntity(userEntity.DepartmentId).FullName;
                errorMsg = "申请用户";

                if (pEntity.CheckBackType == "驳回人")
                {
                    entity.CheckBackFlag = "5";
                    entity.CheckBackUserID = entity.CheckUserID;
                    entity.CheckBackUserName = entity.CheckUserName;
                    entity.CheckBackDeptID = entity.CheckDeptID;
                    entity.CheckBackDeptName = entity.CheckDeptName;
                }
                else
                {
                    entity.CheckBackFlag = entity.CheckBackUserID = entity.CheckBackUserName = entity.CheckBackDeptID = entity.CheckBackDeptName = "";
                }
            }
            else
            {//分委会审核，会签通过
                wfFlag = "1";  // 分委会审核=>总经理审核               
                errorMsg = "总经理用户";
                if (president != null)
                {
                    participant = president.ItemValue;
                    var pUser = userbll.GetUserInfoByAccount(participant);
                    if (pUser != null)
                    {
                        checkUserId = pUser.UserId;
                        checkUserName = pUser.RealName;
                        checkDeptId = pUser.DepartmentId;
                        checkDeptName = pUser.DeptName;
                    }
                    //更新会签人员信息            
                    entity.CheckUserName = entity.CheckUserName.Replace(curUser.UserName, string.Format("{0}(已审)", curUser.UserName));
                    standardApplybll.SaveForm(entity.ID, entity);
                }
            }
            //保存基本信息
            pEntity.ID = Guid.NewGuid().ToString();
            standardCheckbll.SaveForm("", pEntity);

            if (!string.IsNullOrEmpty(participant))
            {
                if (pEntity.CheckResult == "0")
                {
                    entity.CheckDeptID = checkDeptId;
                    entity.CheckDeptName = checkDeptName;
                    entity.CheckUserID = checkUserId;
                    entity.CheckUserName = checkUserName;
                    standardApplybll.SaveForm(entity.ID, entity);

                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                    }
                }
                else
                {
                    if (standardCheckbll.FinishComplete(entity.CheckUserID, entity.CheckUserName, "已审"))
                    {//会签完成才推送流程
                        entity.CheckDeptID = checkDeptId;
                        entity.CheckDeptName = checkDeptName;
                        entity.CheckUserID = checkUserId;
                        entity.CheckUserName = checkUserName;
                        standardApplybll.SaveForm(entity.ID, entity);

                        int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                        }
                    }
                    else
                    {
                        var uList = userbll.GetListForCon(x => entity.CheckUserID.Contains(x.UserId)).ToList();
                        participant = string.Join(",", uList.Select(x => x.Account));
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(keyValue, participant, curUser.UserId);
                    }
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }            

            return Success("操作成功。");
        }
        /// <summary>
        /// 总经理审批
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitPresident(string keyValue, StandardCheckEntity pEntity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //参与人员
            var errorMsg = "";
            string checkUserId = "";
            string checkUserName = "";
            string checkDeptId = "";
            string checkDeptName = "";
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员          

            var entity = standardApplybll.GetEntity(pEntity.RecID);
            if (pEntity.CheckResult == "0")
            {//不通过,退回
                var appEntity = standardApplybll.GetEntity(keyValue);
                wfFlag = "2";
                string createuserid = appEntity.CREATEUSERID;
                var userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //申请用户
                checkUserId = userEntity.UserId;
                checkUserName = userEntity.RealName;
                checkDeptId = userEntity.DepartmentId;
                checkDeptName = deptbll.GetEntity(userEntity.DepartmentId).FullName;
                errorMsg = "申请用户";

                if (pEntity.CheckBackType == "驳回人")
                {
                    entity.CheckBackFlag = "6";
                    entity.CheckBackUserID = curUser.UserId;
                    entity.CheckBackUserName = curUser.UserName;
                    entity.CheckBackDeptID = curUser.DeptId;
                    entity.CheckBackDeptName = curUser.DeptName;
                }
                else
                {
                    entity.CheckBackFlag = entity.CheckBackUserID = entity.CheckBackUserName = entity.CheckBackDeptID = entity.CheckBackDeptName = "";
                }
            }
            else
            {//总经理审批，批准通过
                wfFlag = "1";  // 审批=>结束
                errorMsg = "总经理";
                participant = curUser.Account;
                checkUserId = curUser.UserId;
                checkUserName = curUser.UserName;
                checkDeptId = curUser.DeptId;
                checkDeptName = curUser.DeptName;
            }
            //保存基本信息
            pEntity.ID = Guid.NewGuid().ToString();
            standardCheckbll.SaveForm("", pEntity);
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckDeptID = checkDeptId;
                entity.CheckDeptName = checkDeptName;
                entity.CheckUserID = checkUserId;
                entity.CheckUserName = checkUserName;
                standardApplybll.SaveForm(entity.ID, entity);                

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                    if (wfFlag == "1")
                    {
                        SendMessage(entity);
                    }
                }
            }
            else
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }

            return Success("操作成功。");
        }
        /// <summary>
        /// 发送短消息提醒办公室人员发布新标准体系内容。
        /// </summary>
        /// <param name="entity"></param>
        private void SendMessage(StandardApplyEntity entity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            //标准化办公室审核用户
            DataItemModel checkuser = dataitemdetailbll.GetDataItemListByItemCode("'CheckUserAccount'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (checkuser!=null)
            {
                var chkStr = checkuser.ItemValue.Split(new char[] { '|' });
                var userAccount = chkStr[0];
                var officeuser = new UserBLL().GetUserInfoByAccount(userAccount);
                MessageEntity msg = new MessageEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userAccount,
                    UserName = officeuser.RealName,
                    SendTime = DateTime.Now,
                    SendUser = curUser.Account,
                    SendUserName = curUser.UserName,
                    Title = "标准文件发布提醒",
                    Content = string.Format("“{0}”标准修（订）审批流程已完成，请即时发布。", entity.FileName),
                    Category = "其它"
                };
                if (new MessageBLL().SaveForm("", msg))
                {
                    JPushApi.PublicMessage(msg);
                }
            }
        }
        #endregion
    }
}