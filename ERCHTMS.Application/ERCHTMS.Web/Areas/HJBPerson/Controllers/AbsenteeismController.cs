using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Code;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
namespace ERCHTMS.Web.Areas.HJBPerson.Controllers
{
    public class AbsenteeismController : MvcControllerBase
    {
        private HikinoutlogBLL hikinoutlogbll = new HikinoutlogBLL();
        // GET: HJBPerson/Absenteeism
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Set()
        {
            return View();
        }

        [HttpGet]
        public ActionResult add()
        {
            return View();
        }

        /// <summary>
        /// 连续缺勤统计
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
                       pagination.p_kid = "u.USERID";
                        pagination.p_fields = @"  
            u.DEPARTMENTID,u.DEPARTMENTCODE, u.REALNAME,u.DUTYID,u.DUTYNAME,u.depttype,
case when u.nature = '班组' then u.parentname else u.DEPTNAME end FULLNAME,
COUNT(u.USERID) OVER(partition by case when u.nature = '班组' then u.parentname else u.DEPTNAME end) AS personcount,
  case when(length(u.deptcode) > 20) then(select d.SORTCODE from base_department d where d.deptcode = substr(u.deptcode, 1, 20)) else u.DEPTSORT end as DEPTSORTss ";
                       pagination.p_tablename = @" V_USERINFO u
            left JOIN BIS_HIKINOUTLOG bh on bh.USERID = u.USERID   ";
            pagination.conditionJson = @" u.DEPARTMENTID not in ('0')  AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd') is NULL 
								AND u.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE=3) ";

            var data = hikinoutlogbll.GetAbsenteeismPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {

                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "u.USERID";
                pagination.p_fields = @"  
            u.DEPARTMENTID,u.DEPARTMENTCODE, u.REALNAME,u.DUTYID,u.DUTYNAME,u.depttype,
case when u.nature = '班组' then u.parentname else u.DEPTNAME end FULLNAME,
COUNT(u.USERID) OVER(partition by case when u.nature = '班组' then u.parentname else u.DEPTNAME end) AS personcount,
  case when(length(u.deptcode) > 20) then(select d.SORTCODE from base_department d where d.deptcode = substr(u.deptcode, 1, 20)) else u.DEPTSORT end as DEPTSORTss ";
                pagination.p_tablename = @" V_USERINFO u
            left JOIN BIS_HIKINOUTLOG bh on bh.USERID = u.USERID   ";
                pagination.conditionJson = @" u.DEPARTMENTID not in ('0')  AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd') is NULL 
								AND u.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE=3) ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DataTable exportTable = hikinoutlogbll.GetAbsenteeismPageList(pagination, queryJson);

                //导出excel
                string title = "连续缺勤统计";
                HSSFWorkbook workbook = new HSSFWorkbook();//创建Workbook对象
                HSSFSheet sheet = workbook.CreateSheet("Sheet1") as HSSFSheet;
                sheet.DefaultRowHeight = 30 * 20;
                int column = exportTable.Columns.Count;
                int indexRow = 0;

                //标题
                if (!string.IsNullOrEmpty(title))
                {
                    IRow headerRow = sheet.CreateRow(indexRow);

                    headerRow.CreateCell(0).SetCellValue(title);

                    //合并单元格
                    CellRangeAddress region = new CellRangeAddress(0, 0, 0, 2);
                    sheet.AddMergedRegion(region);

                    ICellStyle cellstyle = workbook.CreateCellStyle();
                    cellstyle.VerticalAlignment = VerticalAlignment.Center;
                    cellstyle.Alignment = HorizontalAlignment.Center;

                    IFont font = workbook.CreateFont();
                    font.FontHeightInPoints = 22;
                    font.FontName = "宋体";
                    font.Boldweight = (short)FontBoldWeight.Bold;
                    cellstyle.SetFont(font);

                    var cell = sheet.GetRow(0).GetCell(0);
                    cell.CellStyle = cellstyle;

                    HSSFRegionUtil.SetBorderBottom(BorderStyle.Thin, region, sheet, workbook);//下边框  
                    HSSFRegionUtil.SetBorderLeft(BorderStyle.Thin, region, sheet, workbook);//左边框  
                    HSSFRegionUtil.SetBorderRight(BorderStyle.Thin, region, sheet, workbook);//右边框  
                    HSSFRegionUtil.SetBorderTop(BorderStyle.Thin, region, sheet, workbook);//上边框
                    indexRow++;
                }

                //列头样式
                ICellStyle headerStyle = workbook.CreateCellStyle();
                headerStyle.Alignment = HorizontalAlignment.Center;
                headerStyle.VerticalAlignment = VerticalAlignment.Center;
                headerStyle.BorderBottom = BorderStyle.Thin;
                headerStyle.BorderLeft = BorderStyle.Thin;
                headerStyle.BorderRight = BorderStyle.Thin;
                headerStyle.BorderTop = BorderStyle.Thin;
                
                IFont headerFont = workbook.CreateFont();
                headerFont.FontHeightInPoints = 16;
                headerFont.FontName = "宋体";
                headerFont.Boldweight = (short)FontBoldWeight.Bold;
                headerStyle.SetFont(headerFont);
                
                IRow row1 = sheet.CreateRow(indexRow);
                row1.CreateCell(0).SetCellValue("单位名称");
                row1.GetCell(0).CellStyle = headerStyle;
                row1.CreateCell(1).SetCellValue("姓名");
                row1.GetCell(1).CellStyle = headerStyle;
                row1.CreateCell(2).SetCellValue("岗位名称");
                row1.GetCell(2).CellStyle = headerStyle;
                
                //普通单元格样式
                ICellStyle bodyStyle = workbook.CreateCellStyle();
                bodyStyle.Alignment = HorizontalAlignment.Center;
                bodyStyle.VerticalAlignment = VerticalAlignment.Center;
                IFont font1 = workbook.CreateFont();
                font1.Color = HSSFColor.Black.Index;
                font1.Boldweight = 25;
                font1.FontHeightInPoints = 12;
                bodyStyle.FillForegroundColor = HSSFColor.White.Index;
                bodyStyle.SetFont(font1);
                //设置格式
                IDataFormat format = workbook.CreateDataFormat();
                
                //填充数据
                for (int i = 0; i < exportTable.Rows.Count; i++)
                {
                    indexRow++;
                    IRow rowTemp = sheet.CreateRow(indexRow);
                    rowTemp.Height = 62 * 20;
                    rowTemp.CreateCell(0).SetCellValue(exportTable.Rows[i]["fullname"].ToString()+"("+ exportTable.Rows[i]["personcount"].ToString() + ")");
                    rowTemp.CreateCell(1).SetCellValue(exportTable.Rows[i]["realname"].ToString());
                    rowTemp.CreateCell(2).SetCellValue(exportTable.Rows[i]["dutyname"].ToString());

                    rowTemp.GetCell(0).CellStyle = bodyStyle;
                    rowTemp.GetCell(1).CellStyle = bodyStyle;
                    rowTemp.GetCell(2).CellStyle = bodyStyle;
                }
                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(2);
                //合并单元格
                MergeCells(sheet, exportTable, 0, 0, 0);
           
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);

                return File(ms, "application/vnd.ms-excel", title + ".xls");

            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }

        /// <summary>
        // 合并单元格
        // </summary>
        // <param name="sheet">当前sheet</param>
        // <param name="dt">数据源</param>
        // <param name="cellIndex">要合并的第几列</param>
        // <param name="startIndex">起始列号</param>
        // <param name="endIndex">终止列号</param>
        public void MergeCells(ISheet sheet, DataTable dt, int cellIndex, int startIndex, int endIndex)
        {
            for (int i = 1; i < dt.Rows.Count + 2; i++)
            {
                string value = sheet.GetRow(i).GetCell(cellIndex).StringCellValue;
                int end = i;
                for (int j = i + 1; j < dt.Rows.Count + 2; j++)
                {
                    string value1 = sheet.GetRow(j).GetCell(cellIndex).StringCellValue;
                    if (value.Trim() != value1.Trim())
                    {
                        end = j - 1;
                        break;
                    }
                    else if (value.Trim() == value1.Trim() && j == (dt.Rows.Count + 1))
                    {
                        end = j;
                        break;
                    }
                }
                sheet.AddMergedRegion(new CellRangeAddress(i, end, startIndex, endIndex));
                i = end;
            }
        }


        /// <summary>
        /// 查询人员设置表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        public ActionResult GetAbsenteeismPersonSet(Pagination pagination, string ModuleType)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "a.personsetid";
                pagination.p_fields = "a.USERID,a.MODULETYPE,a.ISREFER,b.realname,b.deptname,b.dutyname";
                pagination.p_tablename = @"HJB_PERSONSET a
                                        left join V_USERINFO b on a.USERID = b.USERID";
                pagination.conditionJson = "1=1";
                var data = hikinoutlogbll.GetAbsenteeismPersonSet(pagination, ModuleType);
                var jsonData = new
                {
                    rows = data,
                    total = pagination.page,
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
        /// 保存新增人员数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult addAbsenteeismUser(string queryJson, string ModuleType)
        {
            hikinoutlogbll.SaveAbsenteeismPersonSet(queryJson, ModuleType);
            return Success("保存成功!");
        }

        /// <summary>
        /// 删除新增人员数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult delAbsenteeismUser(string keyValue)
        {
            hikinoutlogbll.DeleteAbsenteeismPersonSet(keyValue);
            return Success("删除成功!");
        }

    }
}