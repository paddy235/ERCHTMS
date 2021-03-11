using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.Busines.LaborProtectionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.LaborProtectionManage.Controllers
{
    /// <summary>
    /// 描 述：劳动防护回收报废表详情
    /// </summary>
    public class LaborrecyclingController : MvcControllerBase
    {
        private LaborrecyclingBLL laborrecyclingbll = new LaborrecyclingBLL();

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
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SeeIndex()
        {
            return View();
        }

        #endregion

        #region 获取数据

        public ActionResult GetIssueList(string keyValue)
        {

            var data = laborrecyclingbll.GetList(keyValue);
            return ToJsonResult(data);

        }

        /// <summary>
        /// 根据物品表id获取最近发放数据
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetOrderLabor(string keyValue)
        {
            LaborequipmentinfoBLL laborequipmentinfobll = new LaborequipmentinfoBLL();
            LaborissuedetailBLL detail = new LaborissuedetailBLL();
            List<LaborequipmentinfoEntity> list = new List<LaborequipmentinfoEntity>();
            var data = detail.GetOrderLabor(keyValue);
            if (data != null)
            {
                list = laborequipmentinfobll.GetList(data.ID).ToList();
            }

            return ToJsonResult(list);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = laborrecyclingbll.GetList(queryJson);
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
            var data = laborrecyclingbll.GetEntity(keyValue);
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
            laborrecyclingbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 批量保存表单（新增、修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveListForm(string json)
        {
            json = HttpUtility.UrlDecode(json);
            //如果没有数据则直接返回
            if (json == "")
            {
                return Error("无对应内容，操作无效");
            }
            laborrecyclingbll.SaveListForm(json);
            return Success("操作成功。");
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
        public ActionResult SaveForm(string keyValue, LaborrecyclingEntity entity, string json, string InfoId)
        {
            json = HttpUtility.UrlDecode(json);
            if (json == "")
            {
                return Error("无对应内容，操作无效");
            }
            laborrecyclingbll.SaveForm(keyValue, entity, json, InfoId);
            return Success("操作成功。");
        }
        #endregion
    }
}
