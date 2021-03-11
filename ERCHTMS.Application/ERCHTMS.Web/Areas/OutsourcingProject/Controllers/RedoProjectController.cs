using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 复工申请/详情主控制器
    /// </summary>
    public class RedoProjectController : MvcControllerBase
    {
        /// <summary>
        /// 复工申请/详情主视图 Action
        /// </summary>
        /// <returns>复工申请/详情主视图</returns>
        public ActionResult Index()
        {
            return View();
        }

    }
}
