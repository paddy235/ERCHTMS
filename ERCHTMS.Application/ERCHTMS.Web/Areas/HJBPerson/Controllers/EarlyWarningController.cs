using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.PersonManage;

namespace ERCHTMS.Web.Areas.HJBPerson.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class EarlyWarningController : MvcControllerBase
    {

        EarlyWarningBLL earlyWarningBLL = new EarlyWarningBLL();

        // GET: HJBPerson/EarlyWarning
        /// <summary>
        /// 人员安全行为管控预警视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 预警明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        ///  获取人员安全行为管控预警集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEarlyWaringJson(Pagination pagination, string queryJson)
        {

            var data = earlyWarningBLL.GetPageList(queryJson, pagination);
            var jsonData = new
            {
                rows = data,
                pagination.total,
                pagination.page,
                pagination.records
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        ///  获取人员安全行为管控预警明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDetail(string key)
        {   
            var data = earlyWarningBLL.GetEntity(key);
            return ToJsonResult(data);
        }

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出用户数据")]
        public ActionResult ExportUserList(string condition, string queryJson)
        { 

            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "WarningTime";
            pagination.sord = "desc";
            pagination.rows = 100000000;
            var data = earlyWarningBLL.GetPageList(queryJson, pagination);
            

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "人员行为安全管控预警";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "人员行为安全管控预警.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "PicUrl", ExcelColumn = "抓拍照片", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "WarningContent", ExcelColumn = "预警内容", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "AreaName", ExcelColumn = "区域", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "DutyPerson", ExcelColumn = "责任人", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "DepartName", ExcelColumn = "部门/班组", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "WarningTime", ExcelColumn = "预警时间", Alignment = "center" });
            excelconfig.ColumnEntity = listColumnEntity;
          //  ExcelHelper.ExportByAspose(data, "人员档案", excelconfig.ColumnEntity);

            //ExcelHelper.ExcelDownload()
                return Success("导出成功。");
        }
        #endregion

    }
}