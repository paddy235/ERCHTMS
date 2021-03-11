using BSFramework.Util.WebControl;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Code;
using ERCHTMS.Entity.SafetyLawManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    public class SafetyLawClassController : MvcControllerBase
    {

        SafetyLawClassBLL safetyLawClassBLL = new SafetyLawClassBLL();

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
        /// 获取机构部门组织树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTreeJson()
        {
            //暂时取所有，后期用户权限开发完成，数据将通过权限下来进行展示 
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}' or Id='0'", user.OrganizeCode);
            var data = safetyLawClassBLL.GetList(where).OrderBy(t => t.EnCode).ToList();
            foreach (SafetyLawClassEntity item in data)
            {
                string hasChild = safetyLawClassBLL.IsHasChild(item.Id) ? "1" : "0";
                TreeEntity tree = new TreeEntity();
                tree.id = item.Id;
                tree.text = item.Name;
                tree.value = item.EnCode;
                tree.parentId = item.Parentid;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                tree.Attribute = "Code";
                tree.AttributeValue = item.EnCode + "|" + hasChild;
                treeList.Add(tree);

            }
            return Content(treeList.TreeToJson("-1"));
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safetyLawClassBLL.GetEntity(keyValue);
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
            safetyLawClassBLL.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyLawClassEntity entity)
        {
            safetyLawClassBLL.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        #endregion
    }
}