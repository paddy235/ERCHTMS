using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.FlowManage.Controllers
{
    /// <summary>
    /// 描 述:我的流程
    /// </summary>
    public class FlowMyProcessController : MvcControllerBase
    {
        #region 视图功能
        //
        // GET: /FlowManage/FlowMyProcess/
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 进度查看
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProcessLookForm()
        {
            return View();
        }
        /// <summary>
        /// 进程再次提交
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProcessAgainNewForm()
        {
            return View();
        }
        #endregion
    }
}
