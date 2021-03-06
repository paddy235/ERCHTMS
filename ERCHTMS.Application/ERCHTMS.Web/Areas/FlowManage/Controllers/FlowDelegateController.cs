using ERCHTMS.Busines.FlowManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.FlowManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.FlowManage.Controllers
{
    /// <summary>
    /// 描 述:工作委托
    /// </summary>
    public class FlowDelegateController : MvcControllerBase
    {
        private WFDelegate wfDelegate = new WFDelegate();

        #region 视图功能
        //
        // GET: /FlowManage/FlowToOther/
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新增编辑
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
        /// 委托规则列表(分页)
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetRulePageListJson(Pagination pagination, string queryJson)
        {
            string _userId = "";
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                _userId = OperatorProvider.Provider.Current().UserId;
            }
            var watch = CommonHelper.TimerStart();
            var data = wfDelegate.GetRulePageList(pagination, queryJson, _userId);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 委托记录列表(分页)(type 1：委托记录，其他：被委托记录)
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <param name="type">1：委托记录，其他：被委托记录</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetRecordPageListJson(Pagination pagination, string queryJson,int type)
        {
            string _userId = "";
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                _userId = OperatorProvider.Provider.Current().UserId;
            }
            var watch = CommonHelper.TimerStart();
            var data = wfDelegate.GetRecordPageList(pagination, queryJson,type, _userId);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 流程模板信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchemeInfoList(string ruleId)
        {
            var data = wfDelegate.GetSchemeInfoList(ruleId);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取委托规则实体对象
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRuleEntityJson(string keyValue)
        {
            var data = wfDelegate.GetRuleEntity(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存委托规则
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="rlueEntity"></param>
        /// <param name="shcemeInfoIds"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存(新增或修改)委托规则信息")]
        public ActionResult SaveDelegateRule(string keyValue, string rlueStr, string shcemeInfoIds)
        {
            WFDelegateRuleEntity entity = rlueStr.ToObject<WFDelegateRuleEntity>();
            wfDelegate.SaveDelegateRule(keyValue, entity, shcemeInfoIds.Split(','));
            return Success("操作成功。");
        }
        /// <summary>
        /// 删除委托规则
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除委托规则信息")]
        public ActionResult DeleteRule(string keyValue)
        {
            wfDelegate.DeleteRule(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 启用/停止委托规则
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="enableMark"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(0, "启用/停止委托规则")]
        public ActionResult UpdateRuleEnable(string keyValue, int enableMark)
        {
            wfDelegate.UpdateRuleEnable(keyValue, enableMark);
            return Success("操作成功。");
        }  
        #endregion
    }
}
