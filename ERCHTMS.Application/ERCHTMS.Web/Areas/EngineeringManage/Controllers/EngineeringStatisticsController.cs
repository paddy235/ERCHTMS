using ERCHTMS.Busines.SaftyCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERCHTMS.Busines.EngineeringManage;
using BSFramework.Util.Offices;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.EngineeringManage.Controllers
{
    /// <summary>
    /// 描 述：安全检查统计
    /// </summary>
    public class EngineeringStatisticsController : MvcControllerBase
    {
        private PerilEngineeringBLL perilengineeringbll = new PerilEngineeringBLL();


        /// <summary>
        /// 列表页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 省级列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SJIndex()
        {
            return View();
        }

        /// <summary>
        ///获取统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet]
        public string GetEngineeringCount(string year = "")
        {
            return perilengineeringbll.GetEngineeringCount(year);
        }


        /// <summary>
        ///获取统计数据(列表)
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpPost]
        public string GetEngineeringList(string year = "")
        {
            return perilengineeringbll.GetEngineeringList(year);
        }


        /// <summary>
        ///获取方案、技术交底统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet]
        public string GetEngineeringFile(string year = "")
        {
            return perilengineeringbll.GetEngineeringFile(year);
        }

        /// <summary>
        ///获取方案、技术交底统计数据（表格）
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpPost]
        public string GetEngineeringFileGrid(string year = "")
        {
            return perilengineeringbll.GetEngineeringFileGrid(year);
        }

        /// <summary>
        ///危大工程完成情况统计
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet]
        public string GetEngineeringCase(string year = "")
        {
            return perilengineeringbll.GetEngineeringCase(year);
        }

        /// <summary>
        ///危大工程完成情况统计（表格）
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpPost]
        public string GetEngineeringCaseGrid(string year = "")
        {
            return perilengineeringbll.GetEngineeringCaseGrid(year);
        }

        /// <summary>
        ///单位内部、各外委单位对比
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns></returns>
        [HttpGet]
        public string GetEngineeringContrast(string year = "",string month="")
        {
            return perilengineeringbll.GetEngineeringContrast(year,month);
        }

        /// <summary>
        ///单位内部、各外委单位对比（表格）
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns></returns>
        [HttpPost]
        public string GetEngineeringContrastGrid(string year = "", string month = "")
        {
            return perilengineeringbll.GetEngineeringContrastGrid(year, month);
        }

        #region 获取省级数据
        /// <summary>
        ///各电厂单位对比
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet]
        public string GetEngineeringContrastForSJ(string year = "")
        {
            return perilengineeringbll.GetEngineeringContrastForSJ(year);
        }

        /// <summary>
        /// 各电厂单位对比表格
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpPost]
        public string GetEngineeringContrastGridForSJ(string year = "")
        {
            DataTable dt = new DataTable();
            dt= perilengineeringbll.GetEngineeringContrastGridForSJ(year);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// 导出各电厂单位对比表格
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportEngineeringContrastDataForSJ(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                string year = queryParam["year"].IsEmpty() ? "" : queryParam["year"].ToString();
                DataTable dt = new DataTable();
                dt = perilengineeringbll.GetEngineeringContrastGridForSJ(year);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "单位对比";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "单位对比导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "typename", ExcelColumn = "电厂名称", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num", ExcelColumn = "数量", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "rate", ExcelColumn = "比例", Width = 20 });
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }


        /// <summary>
        /// 工程类别统计表格
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpPost]
        public string GetEngineeringCategoryGridForSJ(string year = "")
        {
            DataTable dt = new DataTable();
            dt = perilengineeringbll.GetEngineeringCategoryGridForSJ(year);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// 导出工程类别统计表格
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportEngineeringCategoryDataForSJ(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                string year = queryParam["year"].IsEmpty() ? "" : queryParam["year"].ToString();
                DataTable dt = new DataTable();
                dt = perilengineeringbll.GetEngineeringCategoryGridForSJ(year);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "工程类别";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "工程类别导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "typename", ExcelColumn = "工程类别", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num", ExcelColumn = "数量", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "rate", ExcelColumn = "比例", Width = 20 });
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }

        /// <summary>
        /// 工程类别图形
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetEngineeringCategoryForSJ(string year = "")
        {
            return perilengineeringbll.GetEngineeringCategoryForSJ(year);
        }

        /// <summary>
        /// 月度趋势图形
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetEngineeringMonthForSJ(string year = "")
        {
            return perilengineeringbll.GetEngineeringMonthForSJ(year);
        }

        /// <summary>
        /// 月度趋势表格
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetEngineeringMonthGridForSJ(string year = "")
        {
            DataTable dt = new DataTable();
            dt = perilengineeringbll.GetEngineeringMonthGridForSJ(year);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// 月度趋势导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportEngineeringMonthDataForSJ(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                string year = queryParam["year"].IsEmpty() ? "" : queryParam["year"].ToString();
                DataTable dt = new DataTable();
                dt = perilengineeringbll.GetEngineeringMonthGridForSJ(year);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "月度趋势";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "月度趋势导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "typeName", ExcelColumn = "工程类别", Width = 20 });
                for (int i = 1; i <= 12; i++)
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num" + i, ExcelColumn = i + "月", Width = 20 });
                }
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }

        #endregion
    }
}
