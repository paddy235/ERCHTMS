using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Entity.AuthorizeManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.AuthorizeManage.Controllers
{
    /// <summary>
    /// 描 述：系统按钮
    /// </summary>
    public class ModuleButtonController : MvcControllerBase
    {
        private ModuleButtonBLL moduleButtonBLL = new ModuleButtonBLL();

        #region 视图功能
        /// <summary>
        /// 按钮表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 选择系统功能
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OptionModule()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 按钮列表 
        /// </summary>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string moduleId)
        {
            var data = moduleButtonBLL.GetList(moduleId);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 按钮列表 
        /// </summary>
        /// <returns>返回树形列表Json</returns>
        [HttpGet]
        public ActionResult GetTreeListJson(string moduleId, string jobId)
        {
            var data = moduleButtonBLL.GetList(moduleId);
            if (data != null)
            {
                var TreeList = new List<TreeGridEntity>();
                foreach (ModuleButtonEntity item in data)
                {
                    TreeGridEntity tree = new TreeGridEntity();
                    bool hasChildren = data.Count(t => t.ParentId == item.ModuleButtonId) == 0 ? false : true;
                    tree.id = item.ModuleButtonId;
                    tree.parentId = string.IsNullOrEmpty(item.ParentId) ? "0" : item.ParentId;
                    tree.expanded = true;
                    tree.hasChildren = hasChildren;
                    DataTable dt = moduleButtonBLL.GetDataList(moduleId, jobId, item.ModuleButtonId);
                    StringBuilder sb = new StringBuilder();
                    foreach(DataRow dr in dt.Rows)
                    {
                        sb.Append(dr[0].ToString()+",");
                    }
                    dt.Dispose();
                    tree.entityJson = new { ModuleButtonId = item.ModuleButtonId, ModuleId = item.ModuleId, EnCode = item.EnCode, FullName = item.FullName, ParentId = item.ParentId,AuthorizeType=sb.ToString().TrimEnd(',') }.ToJson();
                    TreeList.Add(tree);
                }
                return Content(TreeList.TreeJson());
            }
            return null;
        }
        
        #endregion

        #region 提交数据
        /// <summary>
        /// 按钮列表Json转换按钮树形Json 
        /// </summary>
        /// <param name="moduleButtonJson">按钮列表</param>
        /// <returns>返回树形Json</returns>
        [HttpPost]
        public ActionResult ListToTreeJson(string moduleButtonJson)
        {
            var data = from items in moduleButtonJson.ToList<ModuleButtonEntity>() orderby items.SortCode select items;
            var treeList = new List<TreeEntity>();
            foreach (ModuleButtonEntity item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.ModuleButtonId) == 0 ? false : true;
                tree.id = item.ModuleButtonId;
                tree.text = item.FullName;
                tree.value = item.ModuleId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }
        /// <summary>
        /// 按钮列表Json转换按钮树形Json 
        /// </summary>
        /// <param name="moduleButtonJson">按钮列表</param>
        /// <returns>返回树形列表Json</returns>
        [HttpPost]
        public ActionResult ListToListTreeJson(string moduleButtonJson)
        {
            var data = from items in moduleButtonJson.ToList<ModuleButtonEntity>() orderby items.SortCode select items;
            var TreeList = new List<TreeGridEntity>();
            foreach (ModuleButtonEntity item in data)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.ModuleButtonId) == 0 ? false : true;
                tree.id = item.ModuleButtonId;
                tree.parentId = item.ParentId;
                tree.expanded = true;
                tree.hasChildren = hasChildren;
                tree.entityJson = item.ToJson();
                TreeList.Add(tree);
            }
            return Content(TreeList.TreeJson());
        }
        /// <summary>
        /// 复制按钮
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="moduleId">功能主键</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerMonitor(0, "复制按钮信息")]
        public ActionResult CopyForm(string keyValue, string moduleId)
        {
            moduleButtonBLL.CopyForm(keyValue, moduleId);
            return Content(new AjaxResult { type = ResultType.success, message = "复制成功。" }.ToJson());
        }
        [HttpPost]
        public ActionResult CopyOper(string moduleId)
        {
            var data = moduleButtonBLL.GetList(moduleId);
            return Content(new AjaxResult { type = ResultType.success, message ="操作成功", resultdata = data.ToJson() }.ToJson());
        }
        #endregion
    }
}
