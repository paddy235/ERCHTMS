using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERCHTMS.Busines.StandardSystem;
using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Code;
using BSFramework.Util.WebControl;
using System.Text;
using System.Data;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    /// <summary>
    /// 标准分类
    /// </summary>
    public class StandardCatoryController : MvcControllerBase
    {
        private StcategoryBLL StcategoryBLL = new StcategoryBLL();
        private StandardsystemBLL standardsystembll = new StandardsystemBLL();
        // GET: StandardSystem/StandardCatory
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Form()
        {
            return View();
        }

        #region 获取数据

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = StcategoryBLL.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取机构部门组织树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCatoryTreeJson(string typecode, string tid = "", string keyword = "")
        {
            Operator user = OperatorProvider.Provider.Current();

            StringBuilder sb = new StringBuilder();
            string parentId = "0";
            if (!string.IsNullOrEmpty(tid))
            {
                StcategoryEntity hs = StcategoryBLL.GetEntity(tid);
                if (hs != null)
                {
                    DataTable dt = new ERCHTMS.Busines.BaseManage.DepartmentBLL().GetDataTable(string.Format("select id from HRS_STCATEGORY t where instr('{0}',encode)=1 and typecode='{1}'", hs.ENCODE, typecode));
                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.AppendFormat("{0},", dr[0].ToString());
                    }
                }
            }
            var treeList = new List<TreeEntity>();
            var data = StcategoryBLL.GetList("").Where(t => t.TYPECODE == typecode ).OrderBy(t => t.ENCODE).ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = StcategoryBLL.GetList("").Where(t => t.TYPECODE == typecode && t.NAME.Contains(keyword.Trim())).OrderBy(t => t.ENCODE).ToList();
                if (data.Count()>0)
                {
                    parentId = data.OrderBy(t => t.ENCODE).FirstOrDefault().PARENTID;
                }
            }
            foreach (StcategoryEntity item in data)
            {
                string hasChild = StcategoryBLL.IsHasChild(item.ID) ? "1" : "0";
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.NAME;
                tree.value = item.ID;
                tree.parentId = item.PARENTID;
                tree.complete = true;
                tree.hasChildren = true;
                tree.Attribute = "Code";
                tree.AttributeValue = item.ENCODE + "|" + hasChild;
                if (!string.IsNullOrEmpty(tid))
                {
                    if (sb.ToString().Contains(item.ID))
                    {
                        tree.isexpand = true;
                    }
                }
                else if (tree.parentId == parentId)
                {
                    tree.isexpand = true;
                }
                treeList.Add(tree);

            }
            return Content(treeList.TreeToJson(parentId));
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(6, "删除标准体系分类")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue,string encode)
        {
            var data = StcategoryBLL.GetList("").Where(t => t.ENCODE.StartsWith(encode)).ToList();
            string ids = "";
            foreach (StcategoryEntity item in data)
            {
                StcategoryBLL.RemoveForm(item.ID);
                ids += "'" + item.ID + "',";
            }
            if (!string.IsNullOrEmpty(ids))
            {

                ids = ids.Substring(0, ids.Length - 1);
                standardsystembll.RemoveCategoryForms(ids);
            }
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
        public ActionResult SaveForm(string keyValue, StcategoryEntity entity)
        {
            StcategoryBLL.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}