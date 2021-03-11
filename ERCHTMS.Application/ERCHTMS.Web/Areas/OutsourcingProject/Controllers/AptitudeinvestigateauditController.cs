using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：资质审查审核表
    /// </summary>
    public class AptitudeinvestigateauditController : MvcControllerBase
    {
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();

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
            var data = aptitudeinvestigateauditbll.GetList(queryJson);
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
            var data = aptitudeinvestigateauditbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetAuditListJson(string recId)
        {
            var data = aptitudeinvestigateauditbll.GetAuditRecList(recId);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetHisAuditListJson(string recId)
        {
            var data = new HistoryAuditBLL().GetHisAuditRecList(recId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 根据关联业务Id查询审核记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.ID";
                pagination.p_fields = @" t.auditresult,t.audittime,t.auditopinion,t.auditpeople,t.auditdept ";
                pagination.p_tablename = @"epg_aptitudeinvestigateaudit t";
                pagination.sidx = "t.audittime";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = " 1=1 ";
                var data = aptitudeinvestigateauditbll.GetPageList(pagination, queryJson);


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
        /// 根据业务id获取对应的审核记录列表 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetAuditList(string keyValue) 
        {
            var data = aptitudeinvestigateauditbll.GetAuditRecList(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 根据业务id获取对应的审核记录列表 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetSpecialAuditList(string keyValue)
        {
            var data = aptitudeinvestigateauditbll.GetAuditList(keyValue);
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
        public ActionResult RemoveForm(string keyValue)
        {
            aptitudeinvestigateauditbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            aptitudeinvestigateauditbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// 资质审查审核通过同步 工程 单位 人员信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSynchrodata(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            aptitudeinvestigateauditbll.SaveSynchrodata(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// 保证金审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSafetyEamestMoney(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            aptitudeinvestigateauditbll.SaveSafetyEamestMoney(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// 复工申请审核:更新工程状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult AuditReturnForWork(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            aptitudeinvestigateauditbll.AuditReturnForWork(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// 开工申请审核:更新工程状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult AuditStartApply(string keyValue, AptitudeinvestigateauditEntity entity, string projectId,string result="",string users="")
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            StartapplyforBLL applyBll = new StartapplyforBLL();
            var sl = applyBll.GetEntity(entity.APTITUDEID);
            string status = "";
            var mp = new PeopleReviewBLL().CheckAuditPower(user, out status, "开工申请", projectId);
            if (mp != null)
            {
                if (sl!=null)
                {
                    sl.NodeName = mp.FLOWNAME;
                    sl.NodeId = mp.ID;
                    sl.AuditRole = mp.CHECKROLEID;
                    applyBll.SaveForm(entity.APTITUDEID, sl);
                }
               
            }
            else
            {
                if (status=="1")
                {
                    sl.NodeName = "审核完成";
                    sl.NodeId = "";
                    sl.IsOver = 1;
                }
            }
            if (entity.AUDITRESULT == "1")
            {
                sl.ISCOMMIT = "0";
                sl.NodeName = "";
                sl.NodeId = "";
                sl.IsOver = 0;
            }
            if (!string.IsNullOrEmpty(result))
            {
                sl.CheckResult = result;
                sl.CheckUsers = users;
            }
           if( applyBll.SaveForm(entity.APTITUDEID, sl))
           {
               aptitudeinvestigateauditbll.AuditStartApply("", entity);
           }
           return Success("操作成功。");
        }


        /// <summary>
        /// 人员资质审查审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult AuditPeopleReview(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            aptitudeinvestigateauditbll.AuditPeopleReview(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
