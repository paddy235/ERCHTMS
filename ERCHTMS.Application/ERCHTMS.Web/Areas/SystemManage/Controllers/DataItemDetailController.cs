using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：数据字典明细
    /// </summary>
    public class DataItemDetailController : MvcControllerBase
    {
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
        private DataItemCache dataItemCache = new DataItemCache();

        #region 视图功能
        /// <summary>
        /// 明细管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 明细表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 明细详细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Detail()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取检查类型ID
        /// </summary>
        /// <param name="ctype">ctype</param>
        /// <summary>
        [HttpGet]
        public ActionResult GetID(string ctype)
        {
            string ItemId = dataItemCache.GetDataItemList().Where(a => a.ItemValue == ctype).FirstOrDefault().ItemId;
            return Content(ItemId);
        }
        [HttpGet]
        public ActionResult GetListByItemCodeJson(string itemCode)
        {
            var data = dataItemDetailBLL.GetListByItemCode(itemCode);
            return Content(data.ToJson());
        }
        /// 明细列表
        /// </summary>
        /// <param name="itemId">分类Id</param>
        ///<param name="condition">查询字段Id</param> 
        /// <param name="keyword">关键字查询</param>
        /// <returns>返回树形列表Json</returns>
        [HttpGet]
        public ActionResult GetTreeListJson(string itemId, string condition, string keyword)
        {
            var data = dataItemDetailBLL.GetList(itemId).ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                #region 多条件查询
                switch (condition)
                {
                    case "ItemName":        //项目名
                        data = data.TreeWhere(t => t.ItemName.Contains(keyword), "ItemDetailId");
                        break;
                    case "ItemValue":      //项目值
                        data = data.TreeWhere(t => t.ItemValue.Contains(keyword), "ItemDetailId");
                        break;
                    case "ItemCode":      //项目编码
                        data = data.TreeWhere(t => t.ItemCode.Contains(keyword), "ItemDetailId");
                        break;
                    case "SimpleSpelling": //拼音
                        data = data.TreeWhere(t => t.SimpleSpelling.Contains(keyword), "ItemDetailId");
                        break;
                    default:
                        break;
                }
                #endregion
            }
            var TreeList = new List<TreeGridEntity>();
            foreach (DataItemDetailEntity item in data)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? false : true;
                tree.id = item.ItemDetailId;
                tree.parentId = item.ParentId;
                tree.expanded = true;
                tree.hasChildren = hasChildren;
                tree.entityJson = item.ToJson();
                TreeList.Add(tree);
            }
            return Content(TreeList.TreeJson());
        }
        /// <summary>
        /// 明细列表
        /// </summary>
        /// <param name="itemCode">分类Code</param>
        /// <param name="condition">查询字段</param>
        /// <param name="keyword">关键字查询</param>
        /// <returns>返回树形列表Json</returns>
        [HttpGet]
        public ActionResult GetListByCodeJson(string itemCode)
        {
            var data = dataItemDetailBLL.GetListByCode(itemCode);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 明细实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = dataItemDetailBLL.GetEntity(keyValue);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取数据字典列表（绑定控件）
        /// </summary>
        /// <param name="EnCode">代码</param>
        /// <returns>返回列表树Json</returns>
        [HttpGet]
        public ActionResult GetDataItemTreeJson(string EnCode)
        {
            var data = dataItemCache.GetDataItemList(EnCode);
            var treeList = new List<TreeEntity>();
            foreach (DataItemModel item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? false : true;
                tree.id = item.ItemDetailId;
                tree.text = item.ItemName;
                tree.value = item.ItemValue;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// 获取数据字典列表（绑定控件）
        /// </summary>
        /// <param name="EnCode">代码</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDataItemListJson(string EnCode,string Remark="")
        {
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'" + EnCode + "'");
            if (!string.IsNullOrWhiteSpace(Remark)) {
                data = data.Where(x => x.ItemCode == Remark);
            }
            return Content(data.ToJson());
        }

        /// <summary>
        /// 获取数据字典列表（绑定控件）
        /// </summary>
        /// <param name="EnCode">代码</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetAllDataItemListJson(string EnCode, string Remark = "")
        {
            var data = dataItemDetailBLL.GetAllDataItemListByItemCode("'" + EnCode + "'");
            if (!string.IsNullOrWhiteSpace(Remark))
            {
                data = data.Where(x => x.ItemCode == Remark);
            }
            return Content(data.ToJson());
        }


        /// <summary>
        /// 获取数据字典列表根据排序码排序（绑定控件）
        /// </summary>
        /// <param name="EnCode">代码</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDataItemListSortJson(string EnCode)
        {
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'" + EnCode + "'").OrderBy(it=>it.SortCode);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 事故事件统计报表需要的数据
        /// </summary>
        /// <param name="EnCode">代码</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDataItemListJsonForReport(string EnCode, string ItemNameLike)
        {
            var data = dataItemCache.GetDataItemList(EnCode).Where(e => e.ItemName.Contains(ItemNameLike));
            return Content(data.ToJson());
        }

        [HttpGet]
        public ActionResult GetDataItemListJsonForReportYearSJ(string EnCode, string ItemNameLike)
        {
            var data = dataItemCache.GetDataItemList(EnCode).Where(e => e.ItemName.Contains(ItemNameLike)).ToList();
            data.Add(new DataItemModel { ItemDetailId = "-1", ItemValue = "-1", ItemName = "不安全事件", ParentId = "0", EnCode = "-1" });
            data.Add(new DataItemModel { ItemDetailId = "-2", ItemValue = "-2", ItemName = "事故", ParentId = "0", EnCode = "-2" });
            var treeList = new List<TreeEntity>();
            foreach (DataItemModel item in data)
            {
                //包含事件的情况
                if (item.ItemName.Contains("事件") && item.ItemDetailId != "-1")
                {
                    item.ParentId = "-1";
                }
                if (item.ItemName.Contains("事故") && item.ItemDetailId != "-2")
                {
                    item.ParentId = "-2";
                }
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? false : true;
                tree.id = item.ItemDetailId;
                tree.text = item.ItemName;
                tree.value = item.ItemValue;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Item";
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.EnCode;
                treeList.Add(tree);
            }
            var result = treeList.TreeToJson();
            return Content(result);
        }
        /// <summary>
        /// 获取数据字典子列表（绑定控件）
        /// </summary>
        /// <param name="EnCode">代码</param>
        /// <param name="ItemValue">项目值</param>
        /// <returns>返回列表Json</returns>
        public ActionResult GetSubDataItemListJson(string EnCode, string ItemValue)
        {
            var data = dataItemCache.GetSubDataItemList(EnCode, ItemValue);
            return Content(data.ToJson());
        }


        /// <summary>
        /// 获取编码结构树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCodeTreeJson(string EnCode)
        {
            //var data = dataItemCache.GetDataItemList(EnCode);
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'" + EnCode + "'");
            var treeList = new List<TreeEntity>();
            foreach (DataItemModel item in data)
            {

                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? false : true;
                tree.id = item.ItemDetailId;
                tree.text = item.ItemName;
                tree.value = item.ItemValue;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Item";
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.EnCode;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// 获取编码备注
        /// </summary>
        /// <param name="EnCode">代码</param>
        /// <param name="ItemValue">编码值</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDataItemRemark(string EnCode,string ItemValue)
        {
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'" + EnCode + "'").Where(t => t.ItemValue == ItemValue).OrderBy(it => it.SortCode).FirstOrDefault();
            return Content(data.ToJson());
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 项目值不能重复
        /// </summary>
        /// <param name="ItemValue">项目值</param>
        /// <param name="keyValue">主键</param>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistItemValue(string ItemValue, string keyValue, string itemId)
        {
            bool IsOk = dataItemDetailBLL.ExistItemValue(ItemValue, keyValue, itemId);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 项目名不能重复
        /// </summary>
        /// <param name="ItemName">项目名</param>
        /// <param name="keyValue">主键</param>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistItemName(string ItemName, string keyValue, string itemId)
        {
            bool IsOk = dataItemDetailBLL.ExistItemName(ItemName, keyValue, itemId);
            return Content(IsOk.ToString());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除数据字典明细")]
        public ActionResult RemoveForm(string keyValue)
        {
            dataItemDetailBLL.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存明细表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="dataItemDetailEntity">明细实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        //[HandlerMonitor(5, "删除数据字典明细(新增、修改)")]
        public ActionResult SaveForm(string keyValue, DataItemDetailEntity dataItemDetailEntity)
        {
            dataItemDetailBLL.SaveForm(keyValue, dataItemDetailEntity);
            return Success("操作成功。");
        }
        #endregion
    }
}
