using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    public class StandardHomeController : Controller
    {
        // GET: StandardSystem/StandardHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Select()
        {
            return View();
        }
    }
}