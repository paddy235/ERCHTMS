using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.PublicInfoManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.PublicInfoManage.Controllers
{
    /// <summary>
    /// 描 述：电子公告
    /// </summary>
    public class NoticeController : MvcControllerBase
    {
        private NoticeBLL noticeBLL = new NoticeBLL();

        #region 视图功能
        /// <summary>
        /// 公告管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 公告表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 公告列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = noticeBLL.GetPageList(pagination, queryJson);
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
        /// 公告实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = noticeBLL.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(6, "删除公告信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            noticeBLL.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存公告表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="newsEntity">公告实体</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [ValidateInput(false)]
        [HandlerMonitor(5, "保存公告表单(新增、修改)")]
        public ActionResult SaveForm(string keyValue, NewsEntity newsEntity)
        {
            noticeBLL.SaveForm(keyValue, newsEntity);
            return Success("操作成功。");
        }
        #endregion
    }
}
