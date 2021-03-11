using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：辨识评估计划相关联的机构和人员信息
    /// </summary>
    public class RiskPlanDataController : MvcControllerBase
    {
        private RiskPlanDataBLL riskplandatabll = new RiskPlanDataBLL();

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
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(int dataType, string planId)
        {
            var data = riskplandatabll.GetList(dataType, planId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = riskplandatabll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "删除辨识计划关联的辨识或评估详情")]
        public ActionResult RemoveForm(string keyValue)
        {
            riskplandatabll.RemoveForm(keyValue);
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
        [HandlerMonitor(5, "新增或修改辨识计划关联的辨识或评估详情")]
        public ActionResult SaveForm(string keyValue, RiskPlanDataEntity entity)
        {
            riskplandatabll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}