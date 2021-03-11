using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Busines.MatterManage;
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
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace ERCHTMS.Web.Areas.HJBPerson.Controllers
{
    /// <summary>
    /// 黄金埠实时人员进出统计
    /// </summary>
    public class PersonListController : MvcControllerBase
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();
        private HikinoutlogBLL hikinoutlogbll = new HikinoutlogBLL();

        #region 视图页面
        // GET: HJBPerson/PersonList
        /// <summary>
        /// 人员进出视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string sql = string.Format(@"select count(distinct(a.deptname)) as num from bis_hikinoutlog a 
                                        left join V_USERINFO b on a.deptid = b.departmentid 
                                        where nature = '承包商' and length(encode) <= 23 and b.ISEPIBOLY = '是' and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0)");
            DataTable dt = operticketmanagerbll.GetDataTable(sql);
            ViewBag.outsourcing = dt.Rows[0]["num"].ToString();
            string sql2 = string.Format(@"select count(distinct(a.userid)) as num from bis_hikinoutlog a
left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0)");
            DataTable dt2 = operticketmanagerbll.GetDataTable(sql2);
            ViewBag.allPerson = dt2.Rows[0]["num"].ToString();
            string sql3 = string.Format(@"select count(distinct(a.userid)) as num from bis_hikinoutlog a
left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0) and b.ISEPIBOLY = '否' and b.DEPTTYPE is null");
            DataTable dt3 = operticketmanagerbll.GetDataTable(sql3);
            ViewBag.inPerson = dt3.Rows[0]["num"].ToString();
            string sql4 = string.Format(@"select count(distinct(a.userid)) as num from bis_hikinoutlog a
left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0) and  b.DEPTTYPE is not null");
            DataTable dt4 = operticketmanagerbll.GetDataTable(sql4);
            ViewBag.outPerson = dt4.Rows[0]["num"].ToString();
            return View();
        }

        /// <summary>
        /// 人员设置视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Set()
        {
            ViewBag.mType = Request["ModuleType"].ToString();
            return View();
        }

        /// <summary>
        /// 新增人员视图
        /// </summary>
        /// <returns></returns>
        public ActionResult add()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取在厂和外包各部门统计数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetList(string queryJson)
        {
            try
            {
                var date = hikinoutlogbll.GetAllDepartment(queryJson);
                return Content(date.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 根据部门名称获取人员数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="deptName"></param>
        /// <param name="personName"></param>
        /// <returns></returns>
        public ActionResult GetTable(Pagination pagination,string deptName,string personName)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "a.userid";
                pagination.p_fields = "b.REALNAME,b.gender,b.dutyname,a.devicename,a.CREATEDATE";
                pagination.p_tablename = @"(select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.userid = b.userid left join (select * from HJB_PERSONSET where MODULETYPE = 0) t on a.userid = t.userid";
                pagination.conditionJson = " a.inout = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0)";
                var data = hikinoutlogbll.GetTableByDeptname(pagination, deptName, personName);
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
        /// 查询人员设置表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        public ActionResult GetPersonSet(Pagination pagination,string ModuleType)
        {
            try {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "a.personsetid";
                pagination.p_fields = "a.USERID,a.MODULETYPE,a.ISREFER,b.realname,b.deptname,b.dutyname";
                pagination.p_tablename = @"HJB_PERSONSET a                                        left join V_USERINFO b on a.USERID = b.USERID";
                pagination.conditionJson = "1=1";
                var data = hikinoutlogbll.GetPersonSet(pagination, ModuleType);
                var jsonData = new
                {
                    rows = data,
                    total = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (Exception ex) {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 服务器端四大浮窗数据
        /// </summary>
        /// <returns></returns>
        public IList<object> GetFloatData()
        {
            string sql = string.Format(@"select count(distinct(a.deptname)) as num from bis_hikinoutlog a 
                                        left join V_USERINFO b on a.deptid = b.departmentid 
                                        where nature = '承包商' and length(encode) <= 23 and b.ISEPIBOLY = '是' and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0)");
            DataTable dt = operticketmanagerbll.GetDataTable(sql);
            object outsourcing = dt.Rows[0]["num"].ToString();
            string sql2 = string.Format(@"select count(distinct(a.userid)) as num from bis_hikinoutlog a
left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0)");
            DataTable dt2 = operticketmanagerbll.GetDataTable(sql2);
            object allPerson = dt2.Rows[0]["num"].ToString();
            string sql3 = string.Format(@"select count(distinct(a.userid)) as num from bis_hikinoutlog a
left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0) and b.ISEPIBOLY = '否' and b.DEPTTYPE is null");
            DataTable dt3 = operticketmanagerbll.GetDataTable(sql3);
            object inPerson = dt3.Rows[0]["num"].ToString();
            string sql4 = string.Format(@"select count(distinct(a.userid)) as num from bis_hikinoutlog a
left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0) and  b.DEPTTYPE is not null");
            DataTable dt4 = operticketmanagerbll.GetDataTable(sql4);
            object outPerson = dt4.Rows[0]["num"].ToString();

            IList<object> data = new List<object>();
            data.Add(outsourcing);
            data.Add(allPerson);
            data.Add(inPerson);
            data.Add(outPerson);
            return data;
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
        [HandlerMonitor(6, "在厂人员离场操作")]
        public ActionResult UpdateByID(string keyValue)
        {
            hikinoutlogbll.UpdateByID(keyValue);
            return Success("离厂成功!");
        }

        /// <summary>
        /// 保存新增人员数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult addUser(string queryJson,string ModuleType)
        {
            hikinoutlogbll.SavePersonSet(queryJson, ModuleType);
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
        public ActionResult delUser(string keyValue)
        {
            hikinoutlogbll.DeletePersonSet(keyValue);
            return Success("删除成功!");
        }
        #endregion

        #region 数据导出

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "实时在厂人员统计")]
        public ActionResult ExportData(string state)
        {
            try {
                var curuser = OperatorProvider.Provider.Current();
                string sql = @"select case when b.NATURE = '班组' then b.PARENTNAME else b.DEPTNAME END as DEPTNAME,ID,a.userid,b.REALNAME,b.gender,b.dutyname,a.devicename,TO_CHAR(a.CREATEDATE,'yyyy-mm-dd hh24:mi:ss') as datetime,case when (length(b.deptcode)>20) then (select d.SORTCODE from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTSORT end as DEPTSORTss from 
                        bis_hikinoutlog a left join V_USERINFO b on a.userid = b.userid left join(select* from HJB_PERSONSET where MODULETYPE = 0) t on a.userid = t.userid
                        where REALNAME is not NULL and a.inout = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE + 0 > a.CREATEDATE + 0)
                        ";
                //判断当前登陆用户是什么级别
                if (!curuser.RoleName.IsEmpty())
                {
                    string RoleName = curuser.RoleName.ToString();
                    if (ViewBag.IsAppointAccount != 1) {
                        if (RoleName.Contains("承包商级用户"))
                        {
                            //承包商级用户只可查看本单位门禁数据
                            sql += string.Format(" and (t.ISREFER is NULL or t.userid = '{0}')", curuser.UserId);
                            sql += string.Format(@" and b.ROLENAME like '%{0}%' and b.DEPTNAME = (select case when a.nature = '班组' then a.parentname else a.DEPTNAME end as bmname 
                                                from v_userinfo a where a.USERID = '{1}')", RoleName, curuser.UserId);
                        }
                    }
                    else if (RoleName.Contains("厂级部门用户") || RoleName.Contains("安全管理员") || RoleName.Contains("公司领导") || RoleName.Contains("公司管理员") || RoleName.Contains("公司级用户") || RoleName.Contains("超级管理员") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //此级别的用户可查看所有数据
                    }
                    else
                    {
                        sql += string.Format(" and (t.ISREFER is NULL or t.userid = '{0}')", curuser.UserId);
                        if (state == "0")
                        {
                            sql += string.Format(@" and (b.DEPTNAME = (select case when a.nature = '班组' then a.parentname else a.DEPTNAME end as bmname 
                                                from v_userinfo a where a.USERID = '{0}') or b.PARENTNAME = (select case when a.nature = '班组' then a.parentname else a.DEPTNAME end as bmname 
                                                from v_userinfo a where a.USERID = '{0}'))", curuser.UserId);
                        }
                    }
                }
                if (state == "0")
                {

                    sql += " and DEPTTYPE is NULL ORDER BY DEPTSORTss, b.deptsort,b.DEPTCODE,b.userid desc";
                }
                else
                {
                    sql += " and DEPTTYPE is not NULL ORDER BY DEPTTYPE,DEPTSORTss,b.deptsort,b.DEPTCODE,b.userid desc";
                }

                DataTable data = operticketmanagerbll.GetDataTable(sql);

                //导出excel
                string title = "在厂人员统计信息";
                HSSFWorkbook workbook = new HSSFWorkbook();//创建Workbook对象
                HSSFSheet sheet = workbook.CreateSheet("Sheet1") as HSSFSheet;
                sheet.DefaultRowHeight = 24 * 20;
                int column = data.Columns.Count;
                int indexRow = 0;

                //标题
                if (!string.IsNullOrEmpty(title))
                {
                    IRow headerRow = sheet.CreateRow(indexRow);
                    headerRow.HeightInPoints = 30;
                    headerRow.CreateCell(0).SetCellValue(title);

                    //合并单元格
                    CellRangeAddress region = new CellRangeAddress(0, 0, 0, 5);
                    sheet.AddMergedRegion(region);

                    ICellStyle cellstyle = workbook.CreateCellStyle();
                    cellstyle.VerticalAlignment = VerticalAlignment.Center;
                    cellstyle.Alignment = HorizontalAlignment.Center;

                    IFont font = workbook.CreateFont();
                    font.FontHeightInPoints = 25;
                    font.FontName = "微软雅黑";
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
                //headerFont.FontHeightInPoints = 4;
                headerFont.FontName = "宋体";
                headerFont.Boldweight = (short)FontBoldWeight.Bold;
                headerStyle.SetFont(headerFont);

                IRow row1 = sheet.CreateRow(indexRow);
                row1.CreateCell(0).SetCellValue("部门名称");
                row1.GetCell(0).CellStyle = headerStyle;
                row1.CreateCell(1).SetCellValue("姓名");
                row1.GetCell(1).CellStyle = headerStyle;
                row1.CreateCell(2).SetCellValue("性别");
                row1.GetCell(2).CellStyle = headerStyle;
                row1.CreateCell(3).SetCellValue("岗位名称");
                row1.GetCell(3).CellStyle = headerStyle;
                row1.CreateCell(4).SetCellValue("门禁通道名称");
                row1.GetCell(4).CellStyle = headerStyle;
                row1.CreateCell(5).SetCellValue("进厂时间");
                row1.GetCell(5).CellStyle = headerStyle;

                //普通单元格样式
                ICellStyle bodyStyle = workbook.CreateCellStyle();
                bodyStyle.Alignment = HorizontalAlignment.Center;
                bodyStyle.VerticalAlignment = VerticalAlignment.Center;
                IFont font1 = workbook.CreateFont();
                font1.Color = HSSFColor.Black.Index;
                //font1.Boldweight = 25;
                //font1.FontHeightInPoints = 12;
                bodyStyle.FillForegroundColor = HSSFColor.White.Index;
                bodyStyle.SetFont(font1);
                //设置格式
                IDataFormat format = workbook.CreateDataFormat();


                //填充数据
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    indexRow++;
                    IRow rowTemp = sheet.CreateRow(indexRow);
                    //rowTemp.Height = 62 * 20;
                    rowTemp.CreateCell(0).SetCellValue(data.Rows[i]["deptname"].ToString());
                    rowTemp.CreateCell(1).SetCellValue(data.Rows[i]["realname"].ToString());
                    rowTemp.CreateCell(2).SetCellValue(data.Rows[i]["gender"].ToString());
                    rowTemp.CreateCell(3).SetCellValue(data.Rows[i]["dutyname"].ToString());
                    rowTemp.CreateCell(4).SetCellValue(data.Rows[i]["devicename"].ToString());
                    rowTemp.CreateCell(5).SetCellValue(data.Rows[i]["datetime"].ToString());

                    rowTemp.GetCell(0).CellStyle = bodyStyle;
                    rowTemp.GetCell(1).CellStyle = bodyStyle;
                    rowTemp.GetCell(2).CellStyle = bodyStyle;
                    rowTemp.GetCell(3).CellStyle = bodyStyle;
                    rowTemp.GetCell(4).CellStyle = bodyStyle;
                    rowTemp.GetCell(5).CellStyle = bodyStyle;
                }
                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(2);
                sheet.AutoSizeColumn(3);
                sheet.AutoSizeColumn(4);
                sheet.AutoSizeColumn(5);
                //合并单元格
                MergeCells(sheet, data, 0, 0, 0);
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);

                return File(ms, "application/vnd.ms-excel", title + ".xls");
            }
            catch (Exception ex) {


            }
            return Success("导出成功。");
            //设置导出格式
            //ExcelConfig excelconfig = new ExcelConfig();
            //excelconfig.Title = (state == "0" ? "内部" : "外部") + "人员统计信息";
            //excelconfig.TitleFont = "微软雅黑";
            //excelconfig.TitlePoint = 25;
            //excelconfig.FileName = "实时在厂人员统计导出.xls";
            //excelconfig.IsAllSizeColumn = true;
            ////每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            //List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            //excelconfig.ColumnEntity = listColumnEntity;
            //ColumnEntity columnentity = new ColumnEntity();
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "部门名称" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realname".ToLower(), ExcelColumn = "姓名" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "gender".ToLower(), ExcelColumn = "性别" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dutyname".ToLower(), ExcelColumn = "岗位名称" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "devicename".ToLower(), ExcelColumn = "门禁通道名称" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "datetime".ToLower(), ExcelColumn = "进厂时间" });
            ////调用导出方法
            //ExcelHelper.ExcelDownload(data, excelconfig);

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

        #endregion
    }
}