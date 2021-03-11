using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：停复工管理表
    /// </summary>
    public class StopreturnworkController : MvcControllerBase
    {
        private StopreturnworkBLL stopreturnworkbll = new StopreturnworkBLL();
        private ReturntoworkBLL returnbll = new ReturntoworkBLL();
        private StartapplyforBLL startapplyforbll = new StartapplyforBLL();
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
        public ActionResult GetListJson(string queryJson)
        {
            var data = stopreturnworkbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = stopreturnworkbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取复工时间
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetApplyRetrunTime(string outProjectId, string outEngId) {
            var applyReturnTime = "";
            var data = returnbll.GetApplyRetrunTime(outProjectId, outEngId);

            if (data == null) {
                var startData = startapplyforbll.GetApplyReturnTime(outProjectId, outEngId);
                if (startData != null)
                    applyReturnTime = startData.APPLYRETURNTIME.Value.ToString("yyyy-MM-dd");
            }else
                applyReturnTime = data.APPLYRETURNTIME.Value.ToString("yyyy-MM-dd");
            var resultData = new
            {
                APPLYRETURNTIME = applyReturnTime
            };
            return ToJsonResult(resultData);
        }
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.createuserid,t.iscommit,
                                           t.outprojectid,
                                           t.outengineerid,
                                           t.stoptime,
                                           t.transmittime,
                                           t.transmitpeople,
                                           t.transmitpeopleid,
                                           t.acceptpeople,
                                           t.acceptpeopleid,
                                           b.fullname outprojectname,
                                           b.senddeptname,b.senddeptid,
                                           e.engineerletdept,
                                           e.engineername";
                pagination.p_tablename = @" epg_stopreturnwork t
                                              left join epg_outsouringengineer e on e.id=t.outengineerid
                                              left join base_department b on b.departmentid=t.outprojectid";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = " 1=1  ";
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                {
                    pagination.conditionJson =string.Format("  (t.iscommit='1'or t.createuserid='{0}') ",currUser.UserId);
                }
                else if (currUser.RoleName.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format("  (t.OUTPROJECTID ='{0}' or e.supervisorid='{0}' or t.createuserid='{1}' )", currUser.DeptId, currUser.UserId);
                }
                else
                {
                    var deptentity = departmentbll.GetEntity(currUser.DeptId);
                    while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson = string.Format(" (e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and  t.iscommit='1' or t.createuserid='{1}') ", deptentity.EnCode, currUser.UserId);

                    //pagination.conditionJson = string.Format("  (e.engineerletdeptid ='{0}' and t.iscommit='1' or t.createuserid='{1}') ", currUser.DeptId, currUser.UserId);
                }

                var data = stopreturnworkbll.GetPageList(pagination, queryJson);
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
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
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
            stopreturnworkbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, StopreturnworkEntity entity)
        {
            stopreturnworkbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
