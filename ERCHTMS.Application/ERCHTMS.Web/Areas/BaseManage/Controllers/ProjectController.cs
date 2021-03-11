using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using System.Data;

namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：外包工程项目信息
    /// </summary>
    public class ProjectController : MvcControllerBase
    {
        private ProjectBLL projectbll = new ProjectBLL();

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
            var data = projectbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 外包工程列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns> 
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ProjectId";
            pagination.p_fields = "t.CreateDate,t.CreateUserID,ProjectName,ProjectDeptName,ProjectDeptCode,ProjectStatus,ProjectStartDate,ProjectEndDate, ProjectContent,b.SendDeptID,b.OrganizeId,OrganizeCode";
            pagination.p_tablename = "bis_project t left join base_department b on b.EnCode=t.ProjectDeptCode";
            string type = new AuthorizeBLL().GetOperAuthorzeType(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = " 1=1 ";
            if (type == "4")
            {
                //本机构
                pagination.conditionJson = string.Format(" 1=1 and b.OrganizeId='{0}' ", user.OrganizeId);
            }
            else if (type == "3")
            {
                //本子部门
                if (user.RoleName.Contains("承包商") || user.RoleName.Contains("分包商"))
                    pagination.conditionJson = string.Format(" 1=1 and ProjectDeptCode like '{0}%' ", user.DeptCode);
                else
                    pagination.conditionJson = string.Format(" 1=1 and SendDeptID= '{0}'", user.DeptId);
            }
            else if (type == "2")
            {
                //本部门
                pagination.conditionJson = string.Format(" 1=1 and ProjectDeptCode = '{0}' ", user.DeptCode);
            }

            //参数，用于选择项目
            if (null != Request.Params["OrgArgs"]) 
            {
                pagination.conditionJson = string.Format(" 1=1 and b.OrganizeId='{0}' ", user.OrganizeId);
            }
            var watch = CommonHelper.TimerStart();
            //var data = projectbll.GetPageList(pagination, queryJson);
            DataTable data = projectbll.GetPageDataTable(pagination, queryJson);
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
            var data = projectbll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "删除外包工程信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            projectbll.RemoveForm(keyValue);
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
        [HandlerMonitor(5, "新增或者修改外包工程信息")]
        public ActionResult SaveForm(string keyValue, ProjectEntity entity)
        {
            projectbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
