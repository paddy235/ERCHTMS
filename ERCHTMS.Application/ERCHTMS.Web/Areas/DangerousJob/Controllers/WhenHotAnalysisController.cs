using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.Busines.DangerousJob;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Linq;

namespace ERCHTMS.Web.Areas.DangerousJob.Controllers
{
    /// <summary>
    /// 描 述：动火作业分析表
    /// </summary>
    public class WhenHotAnalysisController : MvcControllerBase
    {
        private WhenHotAnalysisBLL whenhotanalysisbll = new WhenHotAnalysisBLL();

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
        /// <param name="keyValue">关联动火作业id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson,string keyValue)
        {
            try
            {
                var data = whenhotanalysisbll.GetList(queryJson);
                if (!string.IsNullOrWhiteSpace(keyValue))
                {
                    data = data.Where(t => t.RecId == keyValue);
                }
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = whenhotanalysisbll.GetEntity(keyValue);
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
            whenhotanalysisbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, WhenHotAnalysisEntity entity)
        {
            whenhotanalysisbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
