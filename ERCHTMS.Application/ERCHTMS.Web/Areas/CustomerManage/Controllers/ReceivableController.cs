using ERCHTMS.Entity.CustomerManage;
using ERCHTMS.Busines.CustomerManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// 描 述：应收账款
    /// </summary>
    public class ReceivableController : MvcControllerBase
    {
        private ReceivableBLL receivablebll = new ReceivableBLL();

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
        /// 收款页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReceiptForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取收款单列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPaymentPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = receivablebll.GetPaymentPageList(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }
        /// <summary>
        /// 获取收款记录列表
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPaymentRecordJson(string orderId)
        {
            var data = receivablebll.GetPaymentRecord(orderId);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存表单（新增）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存应收账款")]
        public ActionResult SaveForm(ReceivableEntity entity)
        {
            receivablebll.SaveForm(entity);
            return Success("操作成功。");
        }
        #endregion
    }
}