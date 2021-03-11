using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System;

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers
{
    /// <summary>
    /// 描 述：隐患评估表
    /// </summary>
    public class HTEstimateController : MvcControllerBase
    {
        private HTEstimateBLL htestimatebll = new HTEstimateBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private UserBLL userbll = new UserBLL();

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
        public ActionResult GetListJson(string queryJson)
        {
            var data = htestimatebll.GetList(queryJson);
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htestimatebll.GetHistoryList(keyCode);
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
            var data = htestimatebll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "删除隐患整改效果评估信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            htestimatebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HTAcceptInfoEntity atEntity, HTChangeInfoEntity chEntity, HTEstimateEntity entity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : ""; //验收ID
            string ESTIMATEID = Request.Form["ESTIMATEID"] != null ? Request.Form["ESTIMATEID"].ToString() : ""; //整改效果评估ID
            string ESTIMATERESULT = Request.Form["ESTIMATERESULT"] != null ? Request.Form["ESTIMATERESULT"].ToString() : ""; //评估情况
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //整改ID
            string participant = string.Empty;  //获取流程下一节点的参与人员
            string wfFlag = string.Empty; //流程标识
            //评估合格
            if (ESTIMATERESULT == "1")
            {
                wfFlag = "3";

                //取当前人
                participant = curUser.Account;

            }
            else
            {
                wfFlag = "2";

                //取整改人
                UserEntity auser = userbll.GetEntity(chEntity.CHANGEPERSON);

                participant = auser.Account;

            }

            //退回后重新新增整改记录及整改效果评估记录
            if (wfFlag == "2")
            {
                //整改记录
                HTChangeInfoEntity cEntity = htchangeinfobll.GetEntity(CHANGEID);
                HTChangeInfoEntity newEntity = new HTChangeInfoEntity(); 
                newEntity = cEntity;
                newEntity.CREATEDATE = DateTime.Now;
                newEntity.MODIFYDATE = DateTime.Now;
                newEntity.ID = null;
                newEntity.AUTOID = cEntity.AUTOID + 1;
                htchangeinfobll.SaveForm("", newEntity);

                HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(cEntity.HIDCODE);
                //验收记录
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

            //保存整改效果评估
            if (!string.IsNullOrEmpty(ESTIMATEID))
            {
                var tempEntity = htestimatebll.GetEntity(ESTIMATEID);
                entity.AUTOID = tempEntity.AUTOID;
            }
            htestimatebll.SaveForm(ESTIMATEID, entity);

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
