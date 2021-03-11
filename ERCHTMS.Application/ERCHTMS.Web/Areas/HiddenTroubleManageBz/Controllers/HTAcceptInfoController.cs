using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers 
{
    /// <summary>
    /// 描 述：隐患验收信息表
    /// </summary>
    public class HTAcceptInfoController : MvcControllerBase
    {
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL();
        private UserBLL userbll = new UserBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTEstimateBLL htestimatebll = new HTEstimateBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
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

        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail() 
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
        public ActionResult GetListJson(string hideCode) 
        {
            var data = htacceptinfobll.GetList(hideCode);
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htacceptinfobll.GetHistoryList(keyCode);
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
            var data = htacceptinfobll.GetEntity(keyValue);
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
        [AjaxOnly]
        [HandlerMonitor(6, "删除隐患验收信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            htacceptinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HTBaseInfoEntity bentity, HTAcceptInfoEntity entity, HTChangeInfoEntity centity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : ""; //验收ID
            string ACCEPTSTATUS = Request.Form["ACCEPTSTATUS"] != null ? Request.Form["ACCEPTSTATUS"].ToString() : ""; //验收情况
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //整改ID
            string participant = string.Empty;  //获取流程下一节点的参与人员
            string wfFlag = string.Empty; //流程标识

            string IIMajorRisks = dataitemdetailbll.GetItemValue("IIMajorRisks"); //II级重大隐患

            string IMajorRisks = dataitemdetailbll.GetItemValue("IMajorRisks"); //I级重大隐患
            //验收通过
            if (ACCEPTSTATUS == "1")
            {
                participant = curUser.Account;

                //重大隐患，则提交到验收评估流程
                if (bentity.HIDRANK == IMajorRisks || bentity.HIDRANK == IIMajorRisks)
                {
                    wfFlag = "2";

                }
                else  //一般隐患，直接整改结束 
                {
                    wfFlag = "4";
                }
            }
            else //验收不通过
            {
                //退回到整改
                UserEntity auser = userbll.GetEntity(centity.CHANGEPERSON);
                //退回到整改
                participant = auser.Account;

                wfFlag = "3";
            }

            if (!string.IsNullOrEmpty(ACCEPTID))
            {
                var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                entity.AUTOID = tempEntity.AUTOID;
            }
            htacceptinfobll.SaveForm(ACCEPTID, entity);

            //退回则重新添加验收记录
            if (wfFlag == "3") 
            {
                //整改记录
                HTChangeInfoEntity chEntity = htchangeinfobll.GetEntity(CHANGEID);
                HTChangeInfoEntity newEntity = new HTChangeInfoEntity();
                newEntity = chEntity;
                newEntity.CREATEDATE = DateTime.Now;
                newEntity.MODIFYDATE = DateTime.Now;
                newEntity.ID = null;
                htchangeinfobll.SaveForm("", newEntity);

                //验收记录
                HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(bentity.HIDCODE);
                HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                accptEntity = htacceptinfoentity;
                accptEntity.ID = null;
                accptEntity.CREATEDATE = DateTime.Now;
                accptEntity.MODIFYDATE = DateTime.Now;
                accptEntity.ACCEPTSTATUS = null;
                accptEntity.ACCEPTIDEA = null;
                accptEntity.ACCEPTPHOTO = null;
                htacceptinfobll.SaveForm("", accptEntity);
            }

            //满足重大隐患，且提交到验收通过,则创建整改验收评估对象
            if ((bentity.HIDRANK == IMajorRisks && wfFlag == "2") || (bentity.HIDRANK == IIMajorRisks && wfFlag == "2"))
            {
               
                //重大隐患下创建新的评估对象
                HTEstimateEntity esEntity = new HTEstimateEntity();
                esEntity.HIDCODE = bentity.HIDCODE;
                esEntity.ESTIMATEPERSON = entity.ACCEPTPERSON;
                esEntity.ESTIMATEPERSONNAME = entity.ACCEPTPERSONNAME;
                esEntity.ESTIMATEDEPARTCODE = entity.ACCEPTDEPARTCODE;
                esEntity.ESTIMATEDEPARTNAME = entity.ACCEPTDEPARTNAME;
                //仅限新增
                htestimatebll.SaveForm("", esEntity);  //添加
            }
            //提交流程
            int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

            if (count > 0)
            {
                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
            }

            return Success("操作成功。");
        }
        #endregion
    }
}
