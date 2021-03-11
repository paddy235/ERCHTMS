using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Busines.SafetyLawManage;
using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// 描 述：标准制度分类表
    /// </summary>
    public class StdsysTypeController : MvcControllerBase
    {
        private StdsysTypeBLL stdsystypebll = new StdsysTypeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

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
        /// 获取分类节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTypeTreeJson()
        {
            Operator user = OperatorProvider.Provider.Current();

            stdsystypebll.SynchroOrg();//更新电厂班组信息

            var root = departmentBLL.GetCompany(user.DeptId);
            var depts = departmentBLL.GetSubDepartments(root.DepartmentId, "省级,厂级,集团");

            var where = string.Format(" and CreateUserOrgCode in ('{0}')", string.Join("','", depts.Select(x => x.EnCode)));
            var data = stdsystypebll.GetList(where).OrderBy(t => t.CREATEDATE).ToList();

            var treeList = new List<TreeEntity>();

            foreach (var item in depts)
            {
                bool hasChild = depts.Where(x => x.ParentId == item.DepartmentId).Count() > 0 ? true : false || data.Where(x => x.CREATEUSERORGCODE == item.EnCode).Count() > 0 ? true : false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "Code";
                tree.AttributeValue = item.EnCode;
                //tree.AttributeA = "Scope";
                //tree.AttributeValueA = item.Scope;
                //tree.AttributeB = "Dept";
                //tree.AttributeValueB = item.Scope;

                treeList.Add(tree);
            }

            foreach (var item in data)
            {
                //bool hasChild = data.Where(x => x.ParentId == item.ID).Count() > 0 ? true : false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.Name;
                tree.value = item.ID;
                tree.parentId = item.ParentId == "-1" ? depts.FirstOrDefault(x => x.EnCode == item.CREATEUSERORGCODE).DepartmentId : item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = data.Where(x => x.ParentId == tree.id).Count() > 0;
                tree.Attribute = "Code";
                tree.AttributeValue = item.Code;
                tree.AttributeA = "Scope";
                tree.AttributeValueA = item.Scope;
                tree.AttributeB = "Dept";
                tree.AttributeValueB = item.Scope;

                treeList.Add(tree);
            }

            return Content(treeList.TreeToJson(root.ParentId));
        }


        /// <summary>
        /// 获取角色分类节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRoleTypeTreeJson(string keyword)
        {
            Operator user = OperatorProvider.Provider.Current();

            stdsystypebll.SynchroOrg();//更新电厂班组信息

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}'", user.OrganizeCode);
            if (!(user.IsSystem || user.RoleName.Contains("公司管理员") || user.RoleName.Contains("厂级部门用户")))
            {
                where += string.Format(" and Scope like '{0}%'", user.DeptCode);
            }
            var hasKeyword = !string.IsNullOrWhiteSpace(keyword);
            if (hasKeyword)
            {
                where += string.Format(" and Name like '%{0}%'", keyword);
            }
            var data = stdsystypebll.GetList(where).OrderBy(t => t.CREATEDATE).ToList();
            foreach (var item in data)
            {
                bool hasChild = data.Where(x => x.ParentId == item.ID).Count() > 0 ? true : false;
                hasChild = hasKeyword ? false : hasChild;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.Name;
                tree.value = item.ID;
                tree.parentId = hasKeyword ? "-1" : item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "Code";
                tree.AttributeValue = item.Code;
                tree.AttributeA = "Scope";
                tree.AttributeValueA = item.Scope;
                tree.AttributeB = "Dept";
                tree.AttributeValueB = item.Scope;

                treeList.Add(tree);
            }
            var parentId = hasKeyword ? "-1" : data[0].ParentId;

            return Content(treeList.TreeToJson(parentId));
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
            var data = stdsystypebll.GetList(pagination, queryJson);
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
            var data = stdsystypebll.GetEntity(keyValue);
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
            stdsystypebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, StdsysTypeEntity entity)
        {
            entity.ParentId = !string.IsNullOrWhiteSpace(entity.ParentId) ? entity.ParentId : "-1";
            var parent = stdsystypebll.GetEntity(entity.ParentId);
            if (parent != null)
                entity.Scope = parent.Scope;

            if (string.IsNullOrWhiteSpace(entity.Scope) || entity.Scope == "05")
            {
                entity.Scope = ERCHTMS.Code.OperatorProvider.Provider.Current().DeptCode;
            }
            stdsystypebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
