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
    /// 隐患完善
    /// </summary>
    public class HTPerfectionController : MvcControllerBase
    {
        //
        // GET: /HiddenTroubleManage/HTPerfection/
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

    }
}
