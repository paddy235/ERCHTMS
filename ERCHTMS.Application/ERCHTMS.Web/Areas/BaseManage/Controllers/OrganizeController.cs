using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：机构管理
    /// </summary>
    public class OrganizeController : MvcControllerBase
    {
        private OrganizeBLL organizeBLL = new OrganizeBLL();
        private OrganizeCache organizeCache = new OrganizeCache();

        #region 视图功能
        /// <summary>
        /// 机构管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 机构表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 机构列表 
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        [HandlerMonitor(3, "获取机构信息")]
        public ActionResult GetTreeJson(string keyword,int mode=0)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string parentId = "0";
            var deptBll = new DepartmentBLL();
            IEnumerable<DepartmentEntity> data = null;
            //if (user.IsSystem)
            //{
            //    data = deptBll.GetList();
            //}
            //else
            //{
                data = deptBll.GetList().Where(t => t.DepartmentId != "0").Where(t => "集团,省级,厂级".Contains(t.Nature));
            //}
             IEnumerable<DepartmentEntity> newdata = new List<DepartmentEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                
                newdata = data.Where(t => t.FullName.Contains(keyword.Trim()));
                data = GetParentId(newdata, data);
            }
           
            if(!user.IsSystem)
            {
                if (data.Count() > 0)
                {
                    parentId = deptBll.GetEntity(user.OrganizeId).ParentId;
                }
                if (mode == 1)
                {
                    data = data.Where(t => t.OrganizeId == user.OrganizeId);
                }
            }
            var treeList = new List<TreeEntity>();
            foreach (DepartmentEntity item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                tree.Attribute = "Code";
                tree.AttributeValue = item.EnCode;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.AttributeA = "Nature";
                tree.AttributeValueA = item.Nature;
                tree.AttributeB = "IsTrain";
                tree.AttributeValueB = item.IsTrain == null ? "0" : item.IsTrain.ToString();
                tree.AttributeD = "IsDept";
                tree.AttributeValueD = !string.IsNullOrWhiteSpace(item.Description)?"0":"1";
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson(parentId));
           
        }
        //找到集合中最上级父节点的id
        public List<DepartmentEntity> GetParentId(IEnumerable<DepartmentEntity> data, IEnumerable<DepartmentEntity> alldata)
        {
            string id = "";
            List<DepartmentEntity> newdata = new List<DepartmentEntity>();
            if (data.Count() > 0)
            {
                newdata = data.ToList();

                for (int i = 0; i < newdata.Count; i++)
                {
                    id = newdata[i].ParentId;
                    //如果自己表里面没有父级 而查询前的表里面有则加入到表中
                    if (newdata.Where(it => it.DepartmentId == id).Count() == 0 && alldata.Where(it => it.DepartmentId == id).Count() > 0)
                    {

                        newdata.Add(alldata.Where(it => it.DepartmentId == id).FirstOrDefault());
                    }

                }


            }

            return newdata;

        }
        /// <summary>
        /// 机构列表 
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回树形列表Json</returns>
        [HttpGet]
        public ActionResult GetTreeListJson(string condition, string keyword)
        {
            var data = organizeBLL.GetList().OrderByDescending(x => x.CreateDate).ToList();

            if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyword))
            {
                #region 多条件查询
                switch (condition)
                {
                    case "FullName":    //公司名称
                        data = data.TreeWhere(t => t.FullName.Contains(keyword), "OrganizeId");
                        break;
                    case "EnCode":      //外文名称
                        data = data.TreeWhere(t => t.EnCode.Contains(keyword), "OrganizeId");
                        break;
                    case "ShortName":   //中文名称
                        data = data.TreeWhere(t => t.ShortName.Contains(keyword), "OrganizeId");
                        break;
                    case "Manager":     //负责人
                        data = data.TreeWhere(t => t.Manager != null && t.Manager.Contains(keyword), "OrganizeId");
                        break;
                    default:
                        break;
                }
                #endregion
            }
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, Request.Cookies["currentmoduleId"].Value, "search");
            if (!string.IsNullOrEmpty(authType))
            {
                switch (authType)
                {
                    case "1":
                        data = data.TreeWhere(t => t.CreateUserId == user.UserId, "OrganizeId");
                        break;
                    case "2":
                        data = data.TreeWhere(t => t.OrganizeId == user.OrganizeId, "OrganizeId");
                        break;
                    case "3":
                        data = data.TreeWhere(t => t.EnCode.StartsWith(user.OrganizeCode), "OrganizeId");
                        break;
                    case "4":
                        data = data.TreeWhere(t => t.OrganizeId == user.OrganizeId, "OrganizeId");
                        break;
                }
            }
            var treeList = new List<TreeGridEntity>();
            foreach (OrganizeEntity item in data)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.OrganizeId) == 0 ? false : true;
                tree.id = item.OrganizeId;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.expanded = true;
                tree.entityJson = item.ToJson();
                treeList.Add(tree);
            }
            return Content(treeList.TreeJson());
        }
        /// <summary>
        /// 机构实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = organizeBLL.GetEntity(keyValue);
            return Content(data.ToJson());
        }
        public ActionResult GetDTList()
        {
            var data = organizeBLL.GetDTList();
            return Content(data.ToJson());
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 公司名称不能重复
        /// </summary>
        /// <param name="organizeName">公司名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistFullName(string FullName, string keyValue)
        {
            bool IsOk = organizeBLL.ExistFullName(FullName, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 外文名称不能重复
        /// </summary>
        /// <param name="enCode">外文名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistEnCode(string EnCode, string keyValue)
        {
            bool IsOk = organizeBLL.ExistEnCode(EnCode, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 中文名称不能重复
        /// </summary>
        /// <param name="shortName">中文名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistShortName(string ShortName, string keyValue)
        {
            bool IsOk = organizeBLL.ExistShortName(ShortName, keyValue);
            return Content(IsOk.ToString());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除机构信息")]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult RemoveForm(string keyValue)
        {
            organizeBLL.RemoveForm(keyValue);
            //删除初始化的考核内容
            new ClassificationBLL().DeleteClassification(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存机构表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="organizeEntity">机构实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存(新增或修改)机构信息")]
        public ActionResult SaveForm(string keyValue, OrganizeEntity organizeEntity)
        {

            organizeBLL.SaveForm(keyValue, organizeEntity);
            //同时初始化考核内容
            if (string.IsNullOrEmpty(keyValue))
            {
                new ClassificationBLL().AddClassificationList(organizeEntity.OrganizeId);
            }
            return Success("操作成功。");
        }
        #endregion
    }
}
