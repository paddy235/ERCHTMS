using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：方案措施管理
    /// </summary>
    public class ProjectFilesController : MvcControllerBase
    {
        private ProtocolBLL protocolbll = new ProtocolBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();

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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetProFilesList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @"e.departmentid,t.engineername,e.fullname,t.planenddate,t.actualenddate,t.createuserorgcode";
            pagination.p_tablename = @" epg_outsouringengineer t left join base_department e on t.outprojectid=e.departmentid";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            if (!user.IsSystem)
            {
                pagination.conditionJson = " t.isdeptadd=1 ";
                if (role.Contains("省级"))
                {
                    pagination.conditionJson += string.Format(@" and t.createuserorgcode  in (select encode
                from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null)", user.NewDeptCode);
                }
                else if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
                {
                    pagination.conditionJson += string.Format(" and t.createuserorgcode  = '{0}'", user.OrganizeCode);
                }
                else if (role.Contains("承包商级用户") || role.Contains("分包商级用户"))
                {
                    pagination.conditionJson += string.Format(" and (t.outprojectid ='{0}' or t.supervisorid='{0}')", user.DeptId);
                }
                else 
                {
                    var deptentity = departmentbll.GetEntity(user.DeptId);
                    while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson += string.Format(" and t.engineerletdeptid in (select departmentid from base_department where encode like '{0}%')", deptentity.EnCode);
                }
            }
            else
            {
                pagination.conditionJson = " 1=1 ";
            }
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.engineername like '%{0}%'  or e.fullname like'%{1}%')", queryParam["txtSearch"].ToString(), queryParam["txtSearch"].ToString());
            }
            var watch = CommonHelper.TimerStart();
            var data = protocolbll.GetProFilesList(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @"e.departmentid,t.engineername,e.fullname,t.planenddate,t.actualenddate,t.createuserorgcode";
            pagination.p_tablename = @" epg_outsouringengineer t left join base_department e on t.outprojectid=e.departmentid";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            if (role.Contains("省级"))
            {
                pagination.conditionJson = string.Format(@" t.createuserorgcode  in (select encode
                from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null)", user.NewDeptCode);
            }
            else if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
            {
                pagination.conditionJson = string.Format(" t.createuserorgcode  = '{0}'", user.OrganizeCode);
            }
            else
            {
                pagination.conditionJson = string.Format(" t.engineerletdeptid = '{0}'", user.DeptId);
            }
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.engineername like '%{0}%'", queryParam["txtSearch"].ToString());
            }
            var watch = CommonHelper.TimerStart();
            var data = protocolbll.GetList(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }
        #endregion
    }
}
