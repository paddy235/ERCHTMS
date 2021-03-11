using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.Busines.EquipmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.EquipmentManage.Controllers
{
    /// <summary>
    /// 描 述：运行故障记录表
    /// </summary>
    public class OperationFailureController : MvcControllerBase
    {
        private OperationFailureBLL operationfailurebll = new OperationFailureBLL();

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
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //设备ID
            string equipmentid = queryParam["equipmentid"].ToString();
            pagination.p_kid = "ID";
            pagination.p_fields = "recordname,RegisterUser,RegisterDate,FailureNature,FailureReason,TakeSteps,HandleResult";
            pagination.p_tablename = "BIS_operationFailure t";
            pagination.conditionJson = string.Format(@" equipmentid='{0}'", equipmentid);
            var watch = CommonHelper.TimerStart();
            var data = operationfailurebll.GetPageList(pagination);
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
            var data = operationfailurebll.GetList(queryJson);
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
            var data = operationfailurebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取设备关联事故记录列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetBulletinPageListJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //设备ID
            string equipmentid = queryParam["equipmentid"].ToString();
            pagination.p_kid = "t.id";
            pagination.p_fields = "t.sgname_deal,t.happentime_deal,t.sglevelname_deal,y.jyjg,t.dcbgfiles,t.bulletinid";
            pagination.p_tablename = "AEM_BULLETIN_DEAL t left join AEM_BULLETIN y on t.bulletinid=y.id";
            pagination.conditionJson = string.Format(@" y.equipmentid='{0}'", equipmentid);
            var watch = CommonHelper.TimerStart();
            var data = operationfailurebll.GetPageList(pagination);
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
        /// 获取设备关联隐患记录列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GethiddenbasePageListJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //设备ID
            string equipmentid = queryParam["equipmentid"].ToString();
            pagination.p_kid = "id";
            pagination.p_fields = "CHECKMANNAME,CHECKDATE,HIDDESCRIBE,CHANGEMEASURE,case when  ACCEPTSTATUS=1 then '验收通过' when ACCEPTSTATUS=1 then '验收不通过' end as ACCEPTSTATUS,addtype,workstream";
            pagination.p_tablename = "v_hiddenbasedata t";
            pagination.conditionJson = string.Format(@" deviceid='{0}'", equipmentid);
            var watch = CommonHelper.TimerStart();
            var data = operationfailurebll.GetPageList(pagination);
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
        /// 获取设备操作人员关联数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetUserPageListJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //设备ID
            string UserIds = queryParam["UserIds"].ToString();
            pagination.p_kid = "a.userid";
            pagination.p_fields = "certname,a.Gender,certnum,senddate,sendorgan,years,a.realname,b.identifyid,a.deptname,a.mobile,enddate";
            pagination.p_tablename = "v_userinfo a left join (select t.userid,certname,Gender,certnum,senddate,sendorgan,years,realname,identifyid,deptname,enddate from BIS_CERTIFICATE t left join v_userinfo u on t.userid=u.userid) b on a.userid=b.userid";
            pagination.conditionJson = string.Format(" instr('{0}',a.userid)>0",UserIds);
            var watch = CommonHelper.TimerStart();
            var data = operationfailurebll.GetPageList(pagination);
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
        /// 获取省级运行故障统计记录
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetOperationFailureRecordForSJ(string queryJson)
        {
            DataTable dt= operationfailurebll.GetOperationFailureRecordForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// 导出省级运行故障记录
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出省级运行故障记录")]
        public ActionResult ExportOperationFailureRecordForSJ(string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            var data = operationfailurebll.GetOperationFailureRecordForSJ(queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "省级特种设备运行故障统计";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "省级特种设备运行故障记录.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "failurenature", ExcelColumn = "故障性质及经过", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "failurereason", ExcelColumn = "故障原因", Alignment = "center", Width = 100 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "takesteps", ExcelColumn = "采取的措施", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "handleresult", ExcelColumn = "处理结果", Alignment = "center" });
            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
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
            operationfailurebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OperationFailureEntity entity)
        {
            operationfailurebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
