using BSFramework.Util;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Code;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.SaftyCheck.Controllers
{
    /// <summary>
    /// 描 述：安全检查统计
    /// </summary>
    public class SaftyCheckStatisticsController : MvcControllerBase
    {
        private SaftyCheckDataRecordBLL srbll = new SaftyCheckDataRecordBLL();
        //
        // GET: /SaftyCheck/SaftyCheckStatistics/
        /// <summary>
        /// 列表页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 省公司统计页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GrpIndex()
        {
            return View();
        }
        public ActionResult Stat()
        {
            return View();
        }
        /// <summary>
        /// 导出统计图
        /// </summary>
        /// <param name="queryJson"></param>
        public ActionResult ExportNumber1(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            var deptCode = queryParam["deptCode"].ToString();
            var year = queryParam["year"].ToString();
            var belongdistrict = queryParam["belongdistrict"].ToString();
            var ctype = queryParam["ctype"].ToString();
            //取出数据源
            var json = srbll.GetGrpSaftyList(deptCode, year, belongdistrict, ctype);
            dynamic jObj = JsonConvert.DeserializeObject(json);
            DataTable exportTable = new DataTable();
            ////设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "安全检查统计数量";
            excelconfig.HeadHeight = 12;
            excelconfig.FileName = "安全检查统计数量.xls";
            ////每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            excelconfig.ColumnEntity = new List<ColumnEntity>();
            string[] columns = new string[] { "月份", "日常安全检查", "专项安全检查", "季节性安全检查", "节假日前后安全检查", "综合安全检查", "省公司安全检查", "检查次数" };
            foreach(var col in columns)
            {
                exportTable.Columns.Add(col);
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = col, ExcelColumn = col, Width = 20 });
            }           
            foreach(var row in jObj.rows)
            {
                DataRow dr = exportTable.NewRow();
                dr["月份"] = row.month;
                dr["日常安全检查"] = row.rc;
                dr["专项安全检查"] = row.zx;
                dr["季节性安全检查"] = row.jj;
                dr["节假日前后安全检查"] = row.jjr;
                dr["综合安全检查"] = row.zh;
                dr["省公司安全检查"] = row.sj;
                dr["检查次数"] = row.sum;
                exportTable.Rows.Add(dr);
            }
            ////调用导出方法
            ExcelHelper.ExcelDownload(exportTable, excelconfig);

            return Success("导出成功。");
        }       
        /// <summary>
        /// 导出对比图
        /// </summary>
        /// <param name="queryJson"></param>
        public ActionResult ExportNumber3(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            var deptCode = queryParam["deptCode"].ToString();
            var year = queryParam["year"].ToString();
            var belongdistrict = queryParam["belongdistrict"].ToString();
            var ctype = queryParam["ctype"].ToString();
            //取出数据源
            var json = srbll.GetGrpSaftyListDB(deptCode, year, belongdistrict, ctype);
            dynamic jObj = JsonConvert.DeserializeObject(json);
            DataTable exportTable = new DataTable();
            ////设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "安全检查统计对比数量";
            excelconfig.HeadHeight = 12;
            excelconfig.FileName = "安全检查统计对比数量.xls";
            ////每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            excelconfig.ColumnEntity = new List<ColumnEntity>();
            string[] columns = new string[] { "部门", "日常安全检查", "专项安全检查", "季节性安全检查", "节假日前后安全检查", "综合安全检查", "省公司安全检查", "检查次数" };
            foreach (var col in columns)
            {
                exportTable.Columns.Add(col);
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = col, ExcelColumn = col, Width = 20 });
            }
            foreach (var row in jObj.rows)
            {
                DataRow dr = exportTable.NewRow();
                dr["部门"] = row.month;
                dr["日常安全检查"] = row.rc;
                dr["专项安全检查"] = row.zx;
                dr["季节性安全检查"] = row.jj;
                dr["节假日前后安全检查"] = row.jjr;
                dr["综合安全检查"] = row.zh;
                dr["省公司安全检查"] = row.sj;
                dr["检查次数"] = row.sum;
                exportTable.Rows.Add(dr);
            }
            ////调用导出方法
            ExcelHelper.ExcelDownload(exportTable, excelconfig);

            return Content("导出成功。");
        }
        /// <summary>
        ///获取统计表格数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        [HttpPost]
        public string GetSaftyList(string deptCode, string year = "", string belongdistrict = "",string ctype="")
        {
            return srbll.GetSaftyList(deptCode, year, belongdistrict, ctype);
        }
        [HttpPost]
        public string GetGrpSaftyList(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return srbll.GetGrpSaftyList(deptCode, year, belongdistrict, ctype);
        }
        /// <summary>
        ///获取统计表格数据(对比)
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        [HttpPost]
        public string GetSaftyListDB(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return srbll.GetSaftyListDB(deptCode, year, belongdistrict, ctype);
        }
        [HttpPost]
        public string GetGrpSaftyListDB(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return srbll.GetGrpSaftyListDB(deptCode, year, belongdistrict, ctype);
        }
        /// <summary>
        /// 获取统计图标数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        [HttpGet]
        public string getRatherCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return srbll.getRatherCheckCount(deptCode, year, belongdistrict, ctype);
        }
        /// <summary>
        /// 获取统计图标数据
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="belongdistrict"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        [HttpGet]
        public string getGrpRatherCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return srbll.getGrpRatherCheckCount(deptCode, year, belongdistrict, ctype);
        }
        /// <summary>
        /// 统计对下属各电厂下发的任务次数统计
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="belongdistrict"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        [HttpGet]
        public string getCheckTaskCount(string startDate="",string endDate="")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            return srbll.getCheckTaskCount(curUser, startDate, endDate);
        }

        public ActionResult getCheckTaskData(string startDate = "", string endDate = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            return Content(srbll.getCheckTaskData(curUser, startDate, endDate).ToJson());
           
        }
        /// <summary>
        /// 获取对比数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        [HttpGet]
        public string GetAreaSaftyState(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return srbll.GetAreaSaftyState(deptCode, year, belongdistrict, ctype);
        }
        [HttpGet]
        public string GetGrpAreaSaftyState(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return srbll.GetGrpAreaSaftyState(deptCode, year, belongdistrict, ctype);
        }
        /// <summary>
        /// 获取趋势数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        [HttpGet]
        public string getMonthCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return srbll.getMonthCheckCount(deptCode, year, belongdistrict, ctype);
        }
        [HttpGet]
        public string getGrpMonthCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return srbll.getGrpMonthCheckCount(deptCode, year, belongdistrict, ctype);
        }
    }
}
