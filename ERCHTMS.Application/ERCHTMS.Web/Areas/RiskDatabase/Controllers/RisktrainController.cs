using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Data;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// 描 述：风险预知训练
    /// </summary>
    public class RisktrainController : MvcControllerBase
    {
        private RisktrainBLL risktrainbll = new RisktrainBLL();
        private RiskEvaluateBLL riskevaluatebll = new RiskEvaluateBLL();
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
        /// 详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Show()
        {
            return View();
        }
        /// <summary>
        /// 详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddForm()
        {
            return View();
        }
        /// <summary>
        /// 评价页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EvaluateForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_tablename = "BIS_RISKTRAIN";
            pagination.p_kid = "id";
            pagination.p_fields = @"taskname,worktype,postname,taskcontent,Status,userids,createuserid,workusers,workfzr,workunit,to_char(workstarttime, 'yyyy-MM-dd HH:mm') workstarttime,
       to_char(workendtime, 'yyyy-MM-dd HH:mm')  workendtime,iscommit,workfzrid ";
            pagination.conditionJson = "(((userids like '%," + user.Account + ",%' and iscommit=1) or (createuserid='" + user.UserId + "') or (workfzrid='" + user.Account + "' and iscommit=1))";
            if (!user.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " or (" + where + " and iscommit=1)";
                }
                
            }
            pagination.conditionJson += ")";
            var data = risktrainbll.GetPageListJson(pagination, queryJson);
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = risktrainbll.GetList(queryJson);
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
            var data = risktrainbll.GetEntity(keyValue);
            
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取风险措施
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetMeasures(string workId)
        {
            TrainmeasuresBLL tm = new TrainmeasuresBLL();
            var data = tm.GetListByWorkId(workId);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetPageMeasures(Pagination pagination, string queryJson)
        {
            TrainmeasuresBLL tm = new TrainmeasuresBLL();
            var watch = CommonHelper.TimerStart();
            pagination.p_tablename = "bis_trainmeasures";
            pagination.p_kid = "id";
            pagination.p_fields = @"riskcontent,measure,workid,status,createdate,lspeople ";
            pagination.conditionJson = "1=1";
            pagination.rows = 1000000;
            pagination.page = 1;
            var data = tm.GetPageListByWorkId(pagination,queryJson);
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
        public ActionResult GetEvaluatePageList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_tablename = "BIS_EVALUATE";
            pagination.p_kid = "id";
            pagination.p_fields = @"createuserid,createdate,createusername,workid,evaluatescore,evaluatecontent ";
            pagination.conditionJson = "1=1";
            var data = riskevaluatebll.GetPageList(pagination, queryJson);
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
            risktrainbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 删除评价数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveEvaluate(string keyValue)
        {
            riskevaluatebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="listMesures">相关工作任务及措施</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, RisktrainEntity entity,string measuresJson)
        {
            List<TrainmeasuresEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrainmeasuresEntity>>(measuresJson);
            risktrainbll.SaveForm(keyValue, entity, list);
            return Success("操作成功。");
        }
        ///// <summary>
        ///// 保存表单（新增、修改）
        ///// </summary>
        ///// <param name="keyValue">主键值</param>
        ///// <param name="entity">实体对象</param>
        ///// <param name="listMesures">相关工作任务及措施</param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AjaxOnly]
        //public ActionResult CommitForm(string keyValue, RisktrainEntity entity, string measuresJson)
        //{
        //    List<TrainmeasuresEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrainmeasuresEntity>>(measuresJson);
        //    risktrainbll.SaveForm(keyValue, entity, list);
        //    return Success("操作成功。");
        //}

         [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CommitEvaluate(string keyValue, RiskEvaluate entity)
        {
            riskevaluatebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
         public ActionResult ExportExcel(string queryJson, string fileName)
         {
             Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
             var watch = CommonHelper.TimerStart();
             Pagination pagination = new Pagination();
             pagination.page = 1;
             pagination.rows = 1000000000;
             pagination.p_tablename = "bis_risktrain";
             pagination.p_kid = "id";
             pagination.p_fields = @"taskname,workunit,workfzr,workusers,to_char(workstarttime, 'yyyy-MM-dd HH:mm') workstarttime,to_char(workendtime, 'yyyy-MM-dd HH:mm')  workendtime,taskcontent,decode(status,1,'已完成',0,'未完成','','未完成')status ";
             pagination.conditionJson = "((userids like '%," + user.Account + ",%' or createuserid='" + user.UserId + "' or workfzrid='" + user.Account + "')";
             if (!user.IsSystem)
             {
                 //根据当前用户对模块的权限获取记录
                 string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                 if (!string.IsNullOrEmpty(where))
                 {
                     pagination.conditionJson += " or " + where;
                 }

             }
             pagination.conditionJson += ")";
             DataTable data = risktrainbll.GetPageListJson(pagination, queryJson);
             //设置导出格式
             ExcelConfig excelconfig = new ExcelConfig();
             excelconfig.Title = "风险预知训练";
             excelconfig.FileName = fileName + ".xls";
             //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
             List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
             excelconfig.ColumnEntity = listColumnEntity;
             ColumnEntity columnentity = new ColumnEntity();
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "taskname", ExcelColumn = "工作任务", Width = 50 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workunit", ExcelColumn = "作业单位", Width = 20 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workfzr", ExcelColumn = "作业负责人", Width = 20 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workusers", ExcelColumn = "作业人员", Width = 50 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workstarttime", ExcelColumn = "作业开始时间", Width = 20 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workendtime", ExcelColumn = "作业结束时间", Width = 20 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "taskcontent", ExcelColumn = "作业任务描述", Width = 50 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "status", ExcelColumn = "状态", Width = 10 });
             //调用导出方法
             ExcelHelper.ExcelDownload(data, excelconfig);

             return Success("导出成功。");
         }
        #endregion
    }
}
