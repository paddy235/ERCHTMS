using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：工作流
    /// </summary>
    public class WorkFlowController : MvcControllerBase
    {

        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //隐患流程

        #region 视图功能
        /// <summary>
        /// 流程页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 流程详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Detail()
        {
            return View();
        }
        #endregion

        #region 获取隐患流程图对象
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetActionList(string keyValue)
        {
            var josnData = htworkflowbll.GetActionList(keyValue);
            return Content(josnData.ToJson());
        }
        #endregion

        #region 获取违章流程图对象
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetLllegalActionList(string keyValue)
        {
            var josnData = htworkflowbll.GetLllegalActionList(keyValue); 
            return Content(josnData.ToJson());
        }
        #endregion

        #region 获取问题流程图对象
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetQuestionActionList(string keyValue)
        {
            var josnData = htworkflowbll.GetQuestionActionList(keyValue);
            return Content(josnData.ToJson());
        }
        #endregion

        #region 获取公共流程图对象
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetCommonFlow(string title, int initnum, string keyValue)
        {
            Flow flow = new Flow();
            flow.title = title;
            flow.initNum = initnum;
            var josnData = htworkflowbll.GetCommonFlow(flow, keyValue);
            return Content(josnData.ToJson());
        }
        #endregion
    }
}
