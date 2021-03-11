using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：数据字典分类
    /// </summary>
    public class DataItemController : MvcControllerBase
    {
        private DataItemBLL dataItemBLL = new DataItemBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
        #region 视图功能
        /// <summary>
        /// 分类管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 分类表单
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
        /// 分类列表 
        /// </summary>
        /// <param name="keyword">关键字查询</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetTreeJson(string keyword)
        {
            var data = dataItemBLL.GetList().ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.ItemName.Contains(keyword), "");
            }
            var treeList = new List<TreeEntity>();
            foreach (DataItemEntity item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.ItemId) == 0 ? false : true;
                tree.id = item.ItemId;
                tree.text = item.ItemName;
                tree.value = item.ItemCode;
                tree.parentId = item.ParentId;
                tree.isexpand = false;
                tree.complete = true;
                tree.Attribute = "isTree";
                tree.AttributeValue = item.IsTree.ToString();
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }
        /// <summary>
        /// 分类列表
        /// </summary>
        /// <param name="keyword">关键字查询</param>
        /// <returns>返回树形列表Json</returns>
        [HttpGet]
        public ActionResult GetTreeListJson(string keyword)
        {
            try
            {
                var data = dataItemBLL.GetList().ToList().Where(t => !string.IsNullOrWhiteSpace(t.ItemName));
                IEnumerable<DataItemEntity> newdata = new List<DataItemEntity>();
                string parentId = "0";
                if (!string.IsNullOrEmpty(keyword))
                {
                    //data = data.Where(t => t.ItemName.Contains(keyword)).ToList();
                    newdata = data.Where(t =>t.ItemName.Contains(keyword.Trim()));
                    data = GetParentId(newdata, data);
                }
                if (data.Count() > 0)
                {
                    parentId = data.OrderBy(t => t.SortCode).FirstOrDefault().ParentId;
                }
                var TreeList = new List<TreeGridEntity>();
                foreach (DataItemEntity item in data)
                {
                    TreeGridEntity tree = new TreeGridEntity();
                    bool hasChildren = data.Count(t => t.ParentId == item.ItemId) == 0 ? false : true;
                    tree.id = item.ItemId;
                    tree.parentId = item.ParentId;
                    tree.expanded = true;
                    tree.hasChildren = hasChildren;
                    tree.entityJson = item.ToJson();
                    TreeList.Add(tree);
                }
                return Content(TreeList.TreeJson());
            }
            catch(System.Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        //找到集合中最上级父节点的id
        public List<DataItemEntity> GetParentId(IEnumerable<DataItemEntity> data, IEnumerable<DataItemEntity> alldata)
        {
            string id = "";
            List<DataItemEntity> newdata = new List<DataItemEntity>();
            if (data.Count() > 0)
            {
                newdata = data.ToList();

                for (int i = 0; i < newdata.Count; i++)
                {
                    id = newdata[i].ParentId;
                    //如果自己表里面没有父级 而查询前的表里面有则加入到表中
                    if (newdata.Where(it => it.ItemId == id).Count() == 0 && alldata.Where(it => it.ItemId == id).Count() > 0)
                    {

                        newdata.Add(alldata.Where(it => it.ItemId == id).FirstOrDefault());
                    }

                }


            }

            return newdata;

        }
        /// <summary>
        /// 分类实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = dataItemBLL.GetEntity(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 分类编号不能重复
        /// </summary>
        /// <param name="ItemCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistItemCode(string ItemCode, string keyValue)
        {
            bool IsOk = dataItemBLL.ExistItemCode(ItemCode, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 分类名称不能重复
        /// </summary>
        /// <param name="ItemName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistItemName(string ItemName, string keyValue)
        {
            bool IsOk = dataItemBLL.ExistItemName(ItemName, keyValue);
            return Content(IsOk.ToString());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除数据字典分类")]
        public ActionResult RemoveForm(string keyValue)
        {
            List<DataItemDetailEntity> details = dataItemDetailBLL.GetList(keyValue).ToList();
            dataItemBLL.RemoveForm(keyValue, details);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存分类表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="dataItemEntity">分类实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存数据字典分类(新增、修改)")]
        public ActionResult SaveForm(string keyValue, DataItemEntity dataItemEntity)
        {
            dataItemBLL.SaveForm(keyValue, dataItemEntity);
            return Success("操作成功。");
        }
        #endregion
    }
}
