using ERCHTMS.Entity.SafePunish;
using ERCHTMS.Busines.SafePunish;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.SafePunish.Controllers
{
    /// <summary>
    /// 描 述：安全考核详细
    /// </summary>
    public class SafepunishdetailController : MvcControllerBase
    {
        private SafepunishdetailBLL safepunishdetailbll = new SafepunishdetailBLL();

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
        public ActionResult GetListJson(string queryJson)
        {
            var data = safepunishdetailbll.GetList(queryJson);
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
            var data = safepunishdetailbll.GetEntity(keyValue);
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
            safepunishdetailbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafepunishdetailEntity entity)
        {
            safepunishdetailbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="punishId">查询参数</param>
        /// <param name="type">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDataListJson(string punishId, string type)
        {
            var data = safepunishdetailbll.GetListByPunishId(punishId,type);
            return ToJsonResult(data);
        }

        #endregion
    }
}
