using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// 描 述：风险管控措施表
    /// </summary>
    public class MeasuresController : MvcControllerBase
    {
        private MeasuresBLL measuresbll = new MeasuresBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {

            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="areaId">区域ID</param>
        /// <param name="riskId">风险库记录ID</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string typeName, string riskId)
        {
            var data = measuresbll.GetDTList(riskId, typeName);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="areaId">区域ID</param>
        /// <param name="riskId">风险库记录ID</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJsonForRiskId(string riskId1)
        {
            var data = measuresbll.GetList(" and riskId='" + riskId1 + "'");
            var JsonData = new
            {
                rows = data,
            };
            return Content(JsonData.ToJson());
        }

        public string GetCountByRiskId(string riskId1)
        {
            var data = measuresbll.GetList(" and riskId='" + riskId1 + "'");
            return data.Count().ToString();
        }


        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = measuresbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="riskId">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public string GetMeasures(string riskId)
        {

            var data = measuresbll.GetMeasures(riskId);
            return data;
        }
        [HttpGet]
        public ActionResult GetMeasuresDetail(string worktask, string areaid)
        {

            var data = measuresbll.GetMeasuresDetail(worktask, areaid);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetLinkAreaById(string AreaId)
        {

            var data = measuresbll.GetLinkAreaById(AreaId);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            measuresbll.RemoveForm(keyValue);
            return Success("删除成功。");
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
        public ActionResult SaveForm(string keyValue, List<MeasuresEntity> entity)
        {
            measuresbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Save(string keyValue, MeasuresEntity entity)
        {
            measuresbll.Save(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
