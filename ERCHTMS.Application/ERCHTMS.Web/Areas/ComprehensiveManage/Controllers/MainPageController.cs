using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.ComprehensiveManage.Controllers
{
    public class MainPageController : Controller
    {
        // GET: StandardSystem/StandardHome
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Organ()
        {
            return View();
        }
    }
}