using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 制定整改计划
    /// </summary>
    public class HTChangePlanController : MvcControllerBase
    {
        //
        // GET: /HiddenTroubleManage/HTChangePlan/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //详情
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

        /// <summary>
        /// 整改计划详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PlanForm()
        {
            return View();
        }
    }
}
