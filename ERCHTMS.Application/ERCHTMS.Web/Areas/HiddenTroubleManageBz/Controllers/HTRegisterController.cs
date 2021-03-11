using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers
{
    public class HTRegisterController : Controller
    {
        //
        // GET: /HiddenTroubleManage/HTRegister/
          [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

    }
}
