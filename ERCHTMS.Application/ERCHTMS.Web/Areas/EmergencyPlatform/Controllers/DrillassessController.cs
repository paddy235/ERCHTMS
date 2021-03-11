using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERCHTMS.Busines.EmergencyPlatform;
using ERCHTMS.Entity.EmergencyPlatform;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// 应急评估
    /// </summary>
    public class DrillassessController : MvcControllerBase
    {
        private DrillassessBLL drillassessbll = new DrillassessBLL();
        // GET: EmergencyPlatform/Drillassess
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFormJson(string keyValue)
        {
            var data = drillassessbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="drillId"></param>
        /// <returns></returns>
        public ActionResult GetFormJsonByDrillId(string drillId)
        {
            var data = drillassessbll.GetList("").Where(t => t.DrillId == drillId).FirstOrDefault();
            return ToJsonResult(data);
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, DrillassessEntity entity)
        {
            var data = drillassessbll.GetList("").Where(t => t.DrillId == entity.DrillId).FirstOrDefault();
            if (data != null)
            {
                entity.Id = data.Id;
            }
            drillassessbll.SaveForm(entity.Id, entity);
            return Success("操作成功。");
        }
    }
}