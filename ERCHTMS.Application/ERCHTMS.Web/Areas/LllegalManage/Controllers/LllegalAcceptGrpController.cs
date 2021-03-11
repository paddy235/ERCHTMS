using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：违章验收（省公司）信息表
    /// </summary>
    public class LllegalAcceptGrpController : MvcControllerBase
    {
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
        #endregion        
    }
}
