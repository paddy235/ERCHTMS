using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using System;
using Newtonsoft.Json.Linq;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// 描 述：通知公告详情
    /// </summary>
    public class AnnounDetailController : MvcControllerBase
    {
        private AnnounDetailBLL announcementbll = new AnnounDetailBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AnnounDetailIndex()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AnnounDetailForm()
        {
            return View();
        }
        #endregion
    }
}
