using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 复查验证
    /// </summary>
    public class HTRegisterController : Controller 
    {
        //
        // GET: /HiddenTroubleManage/HTReCheck/
          [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

    }
}
