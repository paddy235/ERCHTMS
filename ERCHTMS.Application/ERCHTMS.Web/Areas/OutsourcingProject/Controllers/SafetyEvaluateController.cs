using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using System;
using System.Linq;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：安全评价
    /// </summary>
    public class SafetyEvaluateController : MvcControllerBase
    {
        private SafetyEvaluateBLL safetyevaluatebll = new SafetyEvaluateBLL();
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @" e.fullname,r.ENGINEERNAME,t.SiteManagementScore,t.QualityScore,t.ProjectProgressScore,t.FieldServiceScore,
t.EvaluationScore,to_char(t.EvaluationTime,'yyyy-MM-dd') as EvaluationTime,t.modifyuserid,t.issend,t.createuserid ";
            pagination.p_tablename = @"EPG_SafetyEvaluate t left join EPG_OutSouringEngineer r 
on t.projectid=r.id left join base_department e on r.outprojectid=e.departmentid";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
            {
                pagination.conditionJson = string.Format(" (t.createuserorgcode  = '{0}' and t.issend='1' or t.createuserid ='{1}')", user.OrganizeCode,user.UserId);
            }
            else if (role.Contains("承包商级用户"))
            {
                pagination.conditionJson = string.Format(" (e.departmentid = '{0}' or r.SUPERVISORID='{0}' or t.createuserid ='{1}') ", user.DeptId, user.UserId);
            }
            else
            {
                var deptentity = departmentbll.GetEntity(user.DeptId);
                while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                {
                    deptentity = departmentbll.GetEntity(deptentity.ParentId);
                }
                pagination.conditionJson = string.Format(" (r.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and t.issend='1'  or t.createuserid='{1}') ", deptentity.EnCode, user.UserId);

                //pagination.conditionJson = string.Format(" (r.engineerletdeptid = '{0}' and t.issend='1' or t.createuserid ='{1}') ", user.DeptId, user.UserId);
            }
            var queryParam = queryJson.ToJObject();
            //时间范围
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and to_date(to_char(evaluationtime,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            ////查询条件
            //if (!queryParam["txtSearch"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and r.ENGINEERNAME like '%{0}%'", queryParam["txtSearch"].ToString());
            //}
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }
            if (!queryParam["projectid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.projectid='{0}'", queryParam["projectid"].ToString());
            }
            var watch = CommonHelper.TimerStart();
            var data = safetyevaluatebll.GetList(pagination, queryJson);
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
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safetyevaluatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
          [HttpGet]
        public ActionResult GetSafetyByProjectId(string id)
        {
            var data = safetyevaluatebll.GetList().Where(x => x.PROJECTID == id).ToList();
            if (data.Count > 0)
            {
                return ToJsonResult(data.FirstOrDefault());
            }
            else
            {
                return ToJsonResult(null);
            }
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
            safetyevaluatebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyEvaluateEntity entity)
        {
            safetyevaluatebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
