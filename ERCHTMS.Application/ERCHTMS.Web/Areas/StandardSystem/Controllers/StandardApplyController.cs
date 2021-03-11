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
    public class StandardApplyController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private StandardApplyBLL standardApplyBll = new StandardApplyBLL();
        private StandardCheckBLL standardCheckBll = new StandardCheckBLL();

        #region 视图功能
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        /// 流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FlowDetail()
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
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取标准修编的流程图对象
        /// </summary>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetStandardApplyActionList(string keyValue)
        {
            var josnData = htworkflowbll.GetStandardApplyActionList(keyValue);
            return Content(josnData.ToJson());
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            var FlowState = standardApplyBll.GetWorkDetailList("76c4c857-a3e1-45eb-9c61-e8e5dd9bf880");//标准修（订）审核（批）流程
            var DataScope = new List<object>() { new { value = "1", text = "我申请的记录" }, new { value = "2", text = "我处理的记录" } };           
            //返回值
            var josnData = new
            {
                FlowState,//流程状态
                DataScope//数据范围           
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
            var data = standardApplyBll.GetList(pagination, queryJson);
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
            var data = standardApplyBll.GetEntity(keyValue);                  
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
            standardApplyBll.RemoveForm(keyValue);//删除申请
            standardCheckBll.RemoveForm(keyValue);//删除审核
            DeleteFiles(keyValue);//删除文件

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
        public ActionResult SaveForm(string keyValue, StandardApplyEntity entity)
        {
            CommonSaveForm(keyValue,"05", entity);
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
        public ActionResult SubmitForm(string keyValue, StandardApplyEntity entity)
        {
            CommonSaveForm(keyValue, "05", entity);
            //创建完流程实例后
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }
            string errorMsg = "";
            if (!string.IsNullOrWhiteSpace(entity.CheckBackFlag))
            {// 回退后重新推送流程
                errorMsg = ForwordFlowAfeterBack(keyValue, entity);
            }
            else
            {//首次推送流程
                errorMsg = ForwordFlowByDirectly(keyValue, entity);
            }
            if(!string.IsNullOrWhiteSpace(errorMsg))
            {
                return Error("请联系系统管理员，确认" + errorMsg + "!");
            }

            return Success("操作成功。");
        }
        /// <summary>
        /// 回退后重新推送流程
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string ForwordFlowAfeterBack(string keyValue, StandardApplyEntity entity)
        {
            var errorMsg = "";

            string wfFlag = entity.CheckBackFlag;           
            string checkUserId = entity.CheckBackUserID;
            string checkUserName = entity.CheckBackUserName;
            string checkDeptId = entity.CheckBackDeptID;
            string checkDeptName = entity.CheckBackDeptName;
            var uList = new UserBLL().GetListForCon(x => checkUserId.Contains(x.UserId)).Select(x => x.Account).ToList();
            string participant = string.Join(",", uList);

            Operator curUser = OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckDeptID = checkDeptId;
                entity.CheckDeptName = checkDeptName;
                entity.CheckUserID = checkUserId;
                entity.CheckUserName = checkUserName;
                standardApplyBll.SaveForm(keyValue, entity);

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                    if (wfFlag == "7")
                        SendMessage(entity);
                }
            }
            else
            {
                errorMsg= "驳回人";
            }

            return errorMsg;
        }
        /// <summary>
        /// 首次推送流程
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string ForwordFlowByDirectly(string keyValue, StandardApplyEntity entity)
        {
            var errorMsg = "";

            //此处需要判断当前人角色。
            string wfFlag = string.Empty;
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            string checkUserId = "";
            string checkUserName = "";
            string checkDeptId = "";
            string checkDeptName = "";
            //参与人员
            string participant = string.Empty;

            //总经理用户
            DataItemModel president = dataitemdetailbll.GetDataItemListByItemCode("'PresidentApprove'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            //标准化办公室审核用户
            DataItemModel checkuser = dataitemdetailbll.GetDataItemListByItemCode("'CheckUserAccount'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            //直接推送2级审核的部门
            DataItemModel check2dept = dataitemdetailbll.GetDataItemListByItemCode("'Check2Dept'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            //分委会用户
            var fwhUser = new List<string>();
            if (president != null)
                fwhUser = userbll.GetListForCon(x => x.Account != president.ItemValue && x.RoleName.Contains("公司级用户")).Select(x => x.Account).ToList();
            else
                fwhUser = userbll.GetListForCon(x => x.RoleName.Contains("公司级用户")).Select(x=>x.Account).ToList();
            //找当前用户所有部门，不是所在班组或专业。            
            var dept = new DepartmentBLL().GetDepts(curUser.OrganizeId).Where(x => x.Nature == "部门" && curUser.DeptCode.StartsWith(x.EnCode)).FirstOrDefault();
            //流程推进
            if (president != null && president.ItemValue.Contains(curUser.Account))
            {//总经理
                wfFlag = "7"; //申请=>结束
                errorMsg = "总经理";
                participant = curUser.Account;
                checkUserId = curUser.UserId;
                checkUserName = curUser.UserName;
                checkDeptId = curUser.DeptId;
                checkDeptName = curUser.DeptName;
            }
            else if (fwhUser != null && fwhUser.Contains(curUser.Account))
            {//分委会
                wfFlag = "6";//申请=>总经理审批
                errorMsg = "总经理";
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
                }
            }
            else if (checkuser != null && checkuser.ItemValue.Contains(curUser.Account))
            {//标准化办公室审核用户
                wfFlag = "3";//申请=>审核分配会签
                errorMsg = "标准化办公室审核用户";
                var chkStr = checkuser.ItemValue.Split(new char[] { '|' });
                participant = chkStr[0];
                var uList = userbll.GetListForCon(x => participant.Contains(x.Account)).ToList();
                checkUserId = string.Join(",", uList.Select(x => x.UserId));
                checkUserName = string.Join(",", uList.Select(x => x.RealName));
                checkDeptId = chkStr.Length >= 3 ? chkStr[1] : "";
                checkDeptName = chkStr.Length >= 3 ? chkStr[2] : "";
            }
            else if (dept != null && dept.DepartmentId == curUser.DeptId && userbll.HaveRoleListByKey(curUser.UserId, "'100104'").Rows.Count > 0)
            {//部门级负责人（主任/主管）
                wfFlag = "3";//申请=>审核分配会签
                errorMsg = "标准化办公室审核用户";
                var chkStr = checkuser.ItemValue.Split(new char[] { '|' });
                participant = chkStr[0];
                var uList = userbll.GetListForCon(x => participant.Contains(x.Account)).ToList();
                checkUserId = string.Join(",", uList.Select(x => x.UserId));
                checkUserName = string.Join(",", uList.Select(x => x.RealName));
                checkDeptId = chkStr.Length >= 3 ? chkStr[1] : "";
                checkDeptName = chkStr.Length >= 3 ? chkStr[2] : "";
            }
            else if (dept != null && dept.DepartmentId == curUser.DeptId && userbll.HaveRoleListByKey(curUser.UserId, "'100114'").Rows.Count > 0)
            { //部门级副管用户（副主任/主管）
                wfFlag = "2";  // 申请=>2级审核
                errorMsg = "主任/主管";
                if (dept != null)
                {
                    var mgUser = userbll.GetUserListByRole(dept.EnCode, "'100104'", "");
                    participant = string.Join(",", mgUser.Select(x => x.Account));
                    checkUserId = string.Join(",", mgUser.Select(x => x.UserId));
                    checkUserName = string.Join(",", mgUser.Select(x => x.RealName));
                    checkDeptId = curUser.DeptId;
                    checkDeptName = curUser.DeptName;
                }
            }
            else if (check2dept != null && check2dept.ItemValue.Contains(curUser.DeptId))
            {//直接推送主任/主管审核的部门
                wfFlag = "2";  // 申请=>2级审核
                errorMsg = "主任/主管";
                if (dept != null)
                {
                    var mgUser = userbll.GetUserListByRole(dept.EnCode, "'100104'", "");
                    participant = string.Join(",", mgUser.Select(x => x.Account));
                    checkUserId = string.Join(",", mgUser.Select(x => x.UserId));
                    checkUserName = string.Join(",", mgUser.Select(x => x.RealName));
                    checkDeptId = curUser.DeptId;
                    checkDeptName = curUser.DeptName;
                }
            }
            else
            { //其他用户
                wfFlag = "1";  // 申请=>1级审核
                errorMsg = "副主任/主管";                    
                if (dept != null)
                {
                    var mgUser = userbll.GetUserListByRole(dept.EnCode, "'100114'", "");
                    participant = string.Join(",", mgUser.Select(x => x.Account));
                    checkUserId = string.Join(",", mgUser.Select(x => x.UserId));
                    checkUserName = string.Join(",", mgUser.Select(x => x.RealName));
                    checkDeptId = curUser.DeptId;
                    checkDeptName = curUser.DeptName;
                }
            }

            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckDeptID = checkDeptId;
                entity.CheckDeptName = checkDeptName;
                entity.CheckUserID = checkUserId;
                entity.CheckUserName = checkUserName;
                standardApplyBll.SaveForm(keyValue, entity);

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //更新业务流程状态
                    if (wfFlag == "7")
                        SendMessage(entity);
                }
                errorMsg = "";
            }

            return errorMsg;
        }
        /// <summary>
        /// 公用方法，保存数据
        /// </summary>
        public void CommonSaveForm(string keyValue, string workFlow, StandardApplyEntity entity)
        {
            //提交通过
            string userId = OperatorProvider.Provider.Current().UserId;

            //保存基本信息
            standardApplyBll.SaveForm(keyValue, entity);

            //创建流程实例
            if (!htworkflowbll.IsHavaWFCurrentObject(entity.ID))
            {
                bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                if (isSucess)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", entity.ID);  //更新业务流程状态
                }
            }
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
            if (checkuser != null)
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
