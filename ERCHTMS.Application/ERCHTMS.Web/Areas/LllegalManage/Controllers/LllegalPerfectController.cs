using System.Web.Mvc;


namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：电厂安全主管部门完善违章信息
    /// </summary>
    public class LllegalPerfectController : MvcControllerBase
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
        #endregion          
    }         
}
