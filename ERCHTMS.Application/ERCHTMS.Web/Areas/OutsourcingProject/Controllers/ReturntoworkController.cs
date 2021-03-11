using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：复工申请表
    /// </summary>
    public class ReturntoworkController : MvcControllerBase
    {
        private ReturntoworkBLL returntoworkbll = new ReturntoworkBLL();
        private AptitudeinvestigateauditBLL auditbll = new AptitudeinvestigateauditBLL();
        private HisReturnWorkBLL hisreturnworkbll = new HisReturnWorkBLL();
        private HistoryAuditBLL historyauditbll = new HistoryAuditBLL();
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
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult HistoryForm()
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
            var data = returntoworkbll.GetList(queryJson);
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
            var data = returntoworkbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetAuditEntity(string keyValue) {
            var data = auditbll.GetAuditEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取实体--历史记录详情 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue, string HisAuditId)
        {
            var hisReturn = hisreturnworkbll.GetEntity(keyValue);
            var hisauditData = historyauditbll.GetEntity(HisAuditId);
            var hisData = new
            {
                hisReturn = hisReturn,
                hisAudit = hisauditData
            };
            return ToJsonResult(hisData);
           
        }

       
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson) {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id tid";
                pagination.p_fields = @" t.applypeople,t.createuserid,
                                           t.applypeopleid,
                                           t.applytime,
                                           t.applyno,
                                           t.applytype,
                                           t.applyreturntime,
                                           b.fullname,
                                           b.senddeptid,
                                           t.iscommit,
                                           e.engineername,
                                           e.engineerletdept,
                                           e.engineerletdeptid,
                                             decode(a.auditresult, '0', '同意', '1', '不同意', '2', '待审核', '') auditresult,
                                           a.id aid";
                pagination.p_tablename = @"epg_returntowork t
                                              left join epg_outsouringengineer e on e.id = t.outengineerid
                                              left join base_department b on b.departmentid = t.outprojectid
                                              left join epg_aptitudeinvestigateaudit a on a.aptitudeid=t.id";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = " 1=1  ";
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                {
                    pagination.conditionJson = string.Format("  (t.iscommit='1'or t.createuserid='{0}') ", currUser.UserId);
                }
                else if (currUser.RoleName.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format("  (t.outprojectid ='{0}' or e.supervisorid='{0}' or t.createuserid='{1}' )", currUser.DeptId, currUser.UserId);
                }
                else
                {
                    var deptentity = departmentbll.GetEntity(currUser.DeptId);
                    while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson = string.Format(" (e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and t.iscommit='1' or t.createuserid='{1}') ", deptentity.EnCode, currUser.UserId);

                    //pagination.conditionJson = string.Format("  (e.engineerletdeptid ='{0}' and t.iscommit='1' or t.createuserid='{1}') ", currUser.DeptId, currUser.UserId);
                }

                var data = returntoworkbll.GetPageList(pagination, queryJson);
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
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetHistoryPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id tid";
                pagination.p_fields = @" t.applypeople,
                                           t.applypeopleid,
                                           t.applytime,
                                           t.applyno,
                                           t.applytype,
                                           t.applyreturntime,
                                           b.fullname,
                                           b.senddeptid,
                                           t.iscommit,
                                           e.engineername,
                                           e.engineerletdept,
                                           e.engineerletdeptid,
                                             decode(a.auditresult, '0', '同意', '1', '不同意', '2', '待审核', '') auditresult,
                                           a.id auditid";
                pagination.p_tablename = @"epg_historyreturntowork t
                                          left join epg_outsouringengineer e on e.id = t.outengineerid
                                          left join base_department b on b.departmentid = t.outprojectid
                                          left join epg_historyaudit a on a.aptitudeid = t.id";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                var data = hisreturnworkbll.GetHistoryPageList(pagination, queryJson);
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
            returntoworkbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ReturntoworkEntity entity)
        {
            returntoworkbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
