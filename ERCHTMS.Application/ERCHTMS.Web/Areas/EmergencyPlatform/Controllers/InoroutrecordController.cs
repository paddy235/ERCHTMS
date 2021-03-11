using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// 描 述：出入库记录
    /// </summary>
    public class InoroutrecordController : MvcControllerBase
    {
        private InoroutrecordBLL inoroutrecordbll = new InoroutrecordBLL();
        private SuppliesBLL suppliesbll = new SuppliesBLL();

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
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var SUPPLIESID = Request["SUPPLIESID"] ?? "";
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = " STATUNAME,NUM,SUPPLIESUNTILNAME,USERNAME,INOROUTTIME";
            pagination.p_tablename = "MAE_INOROUTRECORD t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }

            if (SUPPLIESID.Length > 0)
                pagination.conditionJson += string.Format(" and SUPPLIESID='{0}'", SUPPLIESID);
            var watch = CommonHelper.TimerStart();
            var data = inoroutrecordbll.GetPageList(pagination, queryJson);
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = inoroutrecordbll.GetList(queryJson);
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
            var data = inoroutrecordbll.GetEntity(keyValue);
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
            inoroutrecordbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, InoroutrecordEntity entity)
        {
            inoroutrecordbll.SaveForm(keyValue, entity);
            var entitySup = suppliesbll.GetEntity(entity.SUPPLIESID);
            //入库
            if (entity.STATUS == 2)
                entitySup.NUM += entity.NUM;
            else
                entitySup.NUM -= entity.NUM;
            suppliesbll.SaveForm(entitySup.ID, entitySup);
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "出入库记录")]
        public ActionResult ExportInoroutrecordList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            var SUPPLIESID = Request["SUPPLIESID"] ?? "";
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "CREATEUSERID, STATUNAME,NUM,SUPPLIESUNTILNAME,USERNAME,INOROUTTIME";
            pagination.p_tablename = "MAE_INOROUTRECORD t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "STATUNAME";
            //string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
            //pagination.conditionJson = string.IsNullOrEmpty(where) ? "1=1" : where;
            pagination.conditionJson += string.Format(" and SUPPLIESID='{0}'", SUPPLIESID);
            var data = inoroutrecordbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "出入库记录";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "出入库记录.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "STATUNAME".ToLower(), ExcelColumn = "方式" });
            listColumnEntity.Add(new ColumnEntity() { Column = "NUM".ToLower(), ExcelColumn = "数量" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESUNTILNAME".ToLower(), ExcelColumn = "单位" });
            listColumnEntity.Add(new ColumnEntity() { Column = "USERNAME".ToLower(), ExcelColumn = "执行人" });
            listColumnEntity.Add(new ColumnEntity() { Column = "INOROUTTIME".ToLower(), ExcelColumn = "时间" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion
    }
}
