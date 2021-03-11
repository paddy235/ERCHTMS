using System.Web.Mvc;


namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：违章制定整改计划
    /// </summary>
    public class LllegalPlanReformController : MvcControllerBase
    {
        #region 视图
        /// <summary>
        /// 列表页面  各流程页面使用
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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeliverForm()
        {
            return View();
        }
        #endregion
    }
}
