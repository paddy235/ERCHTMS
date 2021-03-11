using BSFramework.Util.WebControl;
using ERCHTMS.Busines.StandardSystem;
using ERCHTMS.Code;
using ERCHTMS.Entity.StandardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    public class ElementManagerController : MvcControllerBase
    {
        private ElementBLL elementbll = new ElementBLL();

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
        /// 选择页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
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
            var data = elementbll.GetList(queryJson);
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
            var data = elementbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取机构部门组织树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetElementTreeJson()
        {
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            var data = elementbll.GetList("").OrderBy(t => t.ENCODE).ToList();
            foreach (ElementEntity item in data)
            {
                string hasChild = elementbll.IsHasChild(item.ID) ? "1" : "0";
                TreeEntity tree = new TreeEntity();
                tree.showcheck = true;
                tree.id = item.ID;
                tree.text = item.NAME;
                tree.value = item.ID;
                tree.parentId = item.PARENTID;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                tree.Attribute = "Code";
                tree.AttributeValue = item.ENCODE + "|" + hasChild;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("0"));
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
            elementbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ElementEntity entity)
        {
            elementbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}