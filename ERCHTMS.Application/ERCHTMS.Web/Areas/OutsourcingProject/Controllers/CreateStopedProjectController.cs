using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 创建停工/停工详情主控制器
    /// </summary>
    public class CreateStopedProjectController : MvcControllerBase
    {

        /// <summary>
        /// 创建停工/停工详情主视图 Action
        /// </summary>
        /// <returns>创建停工/停工详情主视图</returns>
        public ActionResult Index()
        {
            return View();
        }

    }
}
