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
    /// �� ����֪ͨ��������
    /// </summary>
    public class AnnounDetailController : MvcControllerBase
    {
        private AnnounDetailBLL announcementbll = new AnnounDetailBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AnnounDetailIndex()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
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
