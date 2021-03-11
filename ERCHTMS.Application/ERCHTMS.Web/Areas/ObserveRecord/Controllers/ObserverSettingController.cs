using ERCHTMS.Busines.Observerecord;
using ERCHTMS.Entity.Observerecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.ObserveRecord.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ObserverSettingController : Controller
    {
        // GET: ObserveRecord/ObserverSetting
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData()
        {
            var osbll = new ObserverSettingBLL();
            var data = osbll.GetData();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(List<ObserverSettingEntity> models)
        {
            var success = true;
            var message = string.Empty;

            try
            {
                for (int i = 0; i < models.Count; i++)
                {
                    var item = models[i];
                    if (string.IsNullOrEmpty(item.DeptId)) throw new Exception(string.Format("行 {0} 请选择应提交范围！", i + 1));
                    if (string.IsNullOrEmpty(item.Cycle)) throw new Exception(string.Format("行 {0} 请选择应提交周期！", i + 1));
                    if (item.Times == 0) throw new Exception(string.Format("行 {0} 请输入应提交频次！", i + 1));
                }

                var osbll = new ObserverSettingBLL();
                osbll.Edit(models);
            }
            catch (Exception e)
            {
                success = false;
                message = e.Message;
            }

            return Json(new { success, message });
        }
    }
}