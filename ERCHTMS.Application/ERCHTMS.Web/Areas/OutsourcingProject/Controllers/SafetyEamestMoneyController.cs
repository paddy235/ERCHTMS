using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Extension;
using System;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：保证金
    /// </summary>
    public class SafetyEamestMoneyController : MvcControllerBase
    {
        private SafetyEamestMoneyBLL safetyeamestmoneybll = new SafetyEamestMoneyBLL();
        private HistoryMoneyBLL historymoneybll = new HistoryMoneyBLL();
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
        /// <summary>
        /// 历史记录列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }
        /// <summary>
        /// 历史记录表单页面
        /// </summary>
        /// <returns></returns>
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @" r.id as engineerid,
                                               e.fullname,
                                               r.engineername,
                                               t.paymentmoney,
                                               t.paymentperson,r.engineerletdeptid,
                                               t.issend,
                                               to_char(t.paymentdate, 'yyyy-mm-dd') as paymentdate,
                                               d.auditpeople,
                                               d.auditresult,t.sendback,t.sendbackmoney,
                                               case
                                                 when d.auditresult is null then
                                                  '0'
                                                 when d.auditresult = 0 then
                                                  '合格'
                                                 when d.auditresult = 1 then
                                                  '不合格'
                                                 when d.auditresult = 2 then
                                                  '待审核'
                                               end as auditresultname,
                                               t.createuserid,
                                               e.senddeptid ";
            pagination.p_tablename = @"epg_safetyeamestmoney t left join EPG_OutSouringEngineer r on t.projectid=r.id 
                                        left join base_department e on r.outprojectid=e.departmentid 
                                        left join epg_aptitudeinvestigateaudit d on d.APTITUDEID=t.id";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
            {
                pagination.conditionJson = string.Format(" (t.createuserorgcode  = '{0}' and t.issend='1' or t.createuserid='{1}')", user.OrganizeCode, user.UserId);

            }
            else if (role.Contains("承包商"))
            {
                pagination.conditionJson = string.Format(" (e.departmentid = '{0}' or r.supervisorid='{0}' or t.createuserid='{1}' )", user.DeptId, user.UserId);
            }
            else
            {
                var deptentity = departmentbll.GetEntity(user.DeptId);
                while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                {
                    deptentity = departmentbll.GetEntity(deptentity.ParentId);
                }
                pagination.conditionJson = string.Format(" (r.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and t.issend='1' or t.createuserid='{1}') ", deptentity.EnCode, user.UserId);

                //pagination.conditionJson = string.Format(" (r.engineerletdeptid ='{0}' and t.issend='1' or t.createuserid='{1}' )", user.DeptId, user.UserId);
            }
            var watch = CommonHelper.TimerStart();
            var data = safetyeamestmoneybll.GetList(pagination, queryJson);
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
        /// 获取考核列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetExamineListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "id";
                pagination.p_fields = @"examinedept,examinedeptid,examinemoney,examineperson,
                                    examinepersonid,examinetime,examinebasis,examinecontent,
                                    examinetodept,examinetodeptid,state,createdate";
                var queryParam = queryJson.ToJObject();
                var strWhere = string.Empty;
                var strWhere1 = string.Empty;
                if (!queryParam["SafetymoneyId"].IsEmpty())
                {
                    strWhere = string.Format(" and t.safetymoneyid='{0}'", queryParam["SafetymoneyId"].ToString());
                }
                else {
                    strWhere = " and 1!=1";
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strWhere1 = string.Format(" and t.projectid='{0}'", queryParam["ProjectId"].ToString());
                }
                else {
                    strWhere1 = " and 1!=1";
                }
                pagination.p_tablename = string.Format(@"(select t.id,t.examinedept,
                                           t.examinedeptid, t.examinemoney,
                                           t.examineperson,t.examinepersonid,
                                           t.examinetime, t.examinebasis,
                                           t.examinecontent,t.examinetodept,
                                           t.examinetodeptid,0 state,t.createdate
                                      from epg_dailyexamine t where isover=1 {0}
                                    union
                                    select t.id,t.examinedept,
                                           t.examinedeptid,t.examinemoney,
                                           t.examineperson,t.examinepersonid,
                                           t.examinetime,t.examinebasis,
                                           t.examinecontent,t.examinetodept,
                                           t.examinetodeptid, 1 state,t.createdate
                                            from epg_safetymoneyexamine t  where 1=1 {1})p", strWhere1, strWhere);
                pagination.page = 1;
                pagination.rows = 100000;
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = safetyeamestmoneybll.GetList(pagination, queryJson);
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
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
         
        }
        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetHisPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id tid ";
                pagination.p_fields = @" r.id as engineerid,
                                               t.paymentmoney,
                                               t.paymentperson,
                                               to_char(t.paymentdate, 'yyyy-MM-dd') as paymentdate,
                                               d.auditpeople,
                                               d.auditresult,d.id auditid,t.moneyid,
                                               case
                                                 when d.auditresult is null then
                                                  '0'
                                                 when d.auditresult = 0 then
                                                  '合格'
                                                 when d.auditresult = 1 then
                                                  '不合格'
                                                 when d.auditresult = 2 then
                                                  '待审核'
                                               end as auditresultname";
                pagination.p_tablename = @" epg_hissafetyeamestmoney t
                                              left join EPG_OutSouringEngineer r on t.projectid = r.id
                                              left join base_department e on r.outprojectid = e.departmentid
                                              left join epg_historyaudit d on d.aptitudeid = t.id";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                var data = historymoneybll.GetHisPageListJson(pagination, queryJson);
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safetyeamestmoneybll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取历史记录详情
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue, string HisAuditId)
        {
            var hismoneyData = historymoneybll.GetEntity(keyValue);
            var hisauditData = historyauditbll.GetEntity(HisAuditId);
            var hisData = new
            {
                hismoney = hismoneyData,
                hisAudit = hisauditData
            };
            return ToJsonResult(hisData);
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
            try
            {
                safetyeamestmoneybll.RemoveForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveExamineForm(string keyValue)
        {
            try
            {
                safetyeamestmoneybll.RemoveExamineForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
           
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
        public ActionResult SaveForm(string keyValue, string state, string auditid, SafetyEamestMoneyEntity entity,string json)
        {
            try
            {
                List<SafetyMoneyExamineEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafetyMoneyExamineEntity>>(json);
                safetyeamestmoneybll.SaveForm(keyValue, state, auditid, entity, list);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
        }
        #endregion
    }
}
