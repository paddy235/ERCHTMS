using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// 描 述：培训文件分类表
    /// </summary>
    public class NosatratypeController : MvcControllerBase
    {
        private NosatratypeBLL nosatratypebll = new NosatratypeBLL();

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
        /// 获取分类节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTypeTreeJson()
        {
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}'", user.OrganizeCode);
            var data = nosatratypebll.GetList(where).OrderBy(t => t.CREATEDATE).ToList();
            foreach (var item in data)
            {
                bool hasChild = data.Where(x => x.ParentId == item.ID).Count() > 0 ? true : false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.Name;
                tree.value = item.ID;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "Code";
                tree.AttributeValue = item.Code;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("-1"));
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = nosatratypebll.GetList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = nosatratypebll.GetEntity(keyValue);
            //返回值
            var josnData = new
            {
                data
            };

            return Content(josnData.ToJson());
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
            nosatratypebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, NosatratypeEntity entity)
        {
            entity.ParentId = !string.IsNullOrWhiteSpace(entity.ParentId) ? entity.ParentId : "-1";
            nosatratypebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
