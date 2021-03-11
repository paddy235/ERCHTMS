using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.Busines.ComprehensiveManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.ComprehensiveManage.Controllers
{
    /// <summary>
    /// 描 述：通知公告
    /// </summary>
    public class BriefReportController : MvcControllerBase
    {
        private BriefReportBLL briefreportbll = new BriefReportBLL();

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
            var data = briefreportbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "Id";
            pagination.p_fields = "DeptName,ReportDate,Periods,IssuerName,IssueTime,ReadUserIdList,IsSend,CreateUserId";
            pagination.p_tablename = "HRS_BRIEFREPORT";
            pagination.conditionJson = "1=1";

            var watch = CommonHelper.TimerStart();
            var data = briefreportbll.GetPageList(pagination, queryJson);
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
            var data = briefreportbll.GetEntity(keyValue);
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
            briefreportbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue,string isSend, BriefReportEntity entity)
        {
            entity.IsSend = isSend;//是否发送
            briefreportbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出通知公告数据")]
        public ActionResult ExportData(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "IsImportant,Title,PublisherDept,Publisher,to_char(ReleaseTime,'yyyy-MM-dd hh24:mi') as ReleaseTime";
            pagination.p_tablename = "BIS_Announcement t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                pagination.conditionJson = string.Format("((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}')", user.UserId);
            }

            var watch = CommonHelper.TimerStart();
            var data = briefreportbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "通知公告";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "通知公告.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isimportant", ExcelColumn = "重要", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title", ExcelColumn = "标题", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "publisherdept", ExcelColumn = "发布部门", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "publisher", ExcelColumn = "发布人", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "releasetime", ExcelColumn = "发布时间", Alignment = "center" });
            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }
        #endregion
    }
}
