using System.Collections.Generic;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// 描 述：区域定位管理
    /// </summary>
    public class AreagpsController : MvcControllerBase
    {
        private AreagpsBLL areagpsbll = new AreagpsBLL();

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
        public ActionResult GetListJson(Pagination pagination)
        {

            var data = areagpsbll.GetTable();

            List<TreeGridEntity> treeList = new List<TreeGridEntity>();
            foreach (DistrictGps item in data)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = data.Count(t => t.ParentID == item.DistrictID) == 0 ? false : true;
                tree.id = item.DistrictID;
                tree.parentId = item.ParentID;
                tree.expanded = false;
                tree.hasChildren = hasChildren;
                string itemJson = item.ToJson();
                tree.entityJson = itemJson;
                treeList.Add(tree);
            }
            return Content(treeList.TreeJson("0"));
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = areagpsbll.GetEntity(keyValue);
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
            areagpsbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, string DistrictId, string PointList)
        {
            areagpsbll.SaveForm(keyValue, DistrictId, PointList);
            return Success("操作成功。");
        }
        #endregion
    }
}
