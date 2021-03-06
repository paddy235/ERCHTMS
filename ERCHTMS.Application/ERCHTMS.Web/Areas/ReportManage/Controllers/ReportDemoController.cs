using ERCHTMS.Code;
using BSFramework.Util.Offices;
using BSFramework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.ReportManage.Controllers
{
    /// <summary>
    /// 描 述：报表Demo
    /// </summary>
    public class ReportDemoController : MvcControllerBase
    {
        #region 视图功能
        /// <summary>
        /// 采购报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Purchase()
        {
            return View();
        }
        /// <summary>
        /// 销售报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Sales()
        {
            return View();
        }
        /// <summary>
        /// 仓库报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Store()
        {
            return View();
        }
        /// <summary>
        /// 对账报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Reconciliation()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取采购报表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPurchaseJson()
        {
            var data = ExcelHelper.ExcelImport(Server.MapPath("~/Areas/ReportManage/Views/ReportDemo/data/Purchase.xlsx"));
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取销售报表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSalesJson()
        {
            var data = ExcelHelper.ExcelImport(Server.MapPath("~/Areas/ReportManage/Views/ReportDemo/data/Sales.xlsx"));
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取仓库报表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStoreJson()
        {
            var data = ExcelHelper.ExcelImport(Server.MapPath("~/Areas/ReportManage/Views/ReportDemo/data/Store.xlsx"));
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取对账报表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetReconciliationJson()
        {
            var data = ExcelHelper.ExcelImport(Server.MapPath("~/Areas/ReportManage/Views/ReportDemo/data/Reconciliation.xlsx"));
            return Content(data.ToJson());
        }
        #endregion
    }
}
