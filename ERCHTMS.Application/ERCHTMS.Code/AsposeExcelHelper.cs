using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ERCHTMS.Code
{
    public static class AsposeExcelHelper
    {
        /// <summary>
        /// 导出Excel Aspose  模板模式
        /// </summary>
        /// <param name="model">要导入的数据</param>
        /// <param name="templateFileName">完整文件路径</param>
        /// <param name="sheetName">表名</param>
        /// <returns></returns>
        private static MemoryStream OuModelFileToStream(DataTable model, string templateFileName, string sheetName)
        {
            WorkbookDesigner designer = new WorkbookDesigner();
            //读取模板文件
            designer.Open(templateFileName);
            //将数据导入进去
            designer.SetDataSource(model);
            designer.Process();
            //判断表名是否为空
            if (!string.IsNullOrEmpty(sheetName))
            {
                designer.Workbook.Worksheets[0].Name = sheetName;
            }
            //返回文件流
            return designer.Workbook.SaveToStream();
        }
        /// <summary>
        /// 导出Excel Aspose  模板模式
        /// </summary>
        /// <param name="model">要导入的数据</param>
        /// <param name="templateFileName">完整文件路径</param>
        /// <param name="sheetName">表名</param>
        /// <returns></returns>
        private static MemoryStream OuModelFileToStreamX(DataSet model, string templateFileName, List<string> sheetNames)
        {
            WorkbookDesigner designer = new WorkbookDesigner();
            //读取模板文件
            designer.Open(templateFileName);
            //将数据导入进去
            designer.SetDataSource(model);
            designer.Process();
            if (designer.Workbook.Worksheets.Count >= sheetNames.Count)
            {
                for (int i = 0; i < sheetNames.Count; i++)
                {
                    string sheetName = sheetNames[i];
                    //判断表名是否为空
                    if (!string.IsNullOrEmpty(sheetName))
                    {
                        designer.Workbook.Worksheets[i].Name = sheetName;
                    }
                }
            }
            //返回文件流
            return designer.Workbook.SaveToStream();
        }
        /// <summary>
        /// 导出Excel 并输出
        /// </summary>
        /// <param name="model">要导入的数据</param>
        /// <param name="templateFileName">ERCHTMS.Web后面的路径</param>
        /// <param name="sheetName">表名</param>
        /// <param name="FileName">导出默认文件名</param>
        public static void ExecuteResult(DataTable model, string templateFileName, string sheetName, string FileName)//ControllerContext context
        {
            string ModelUrl = System.AppDomain.CurrentDomain.BaseDirectory;//获取当前目录路径
            System.IO.MemoryStream ms = OuModelFileToStream(model, ModelUrl + templateFileName, sheetName);
            byte[] bt = ms.ToArray();
            //客户端保存文件名
            string fileName = FileName + ".xls";
            //以字符流形式下载文件
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //通知浏览器下载文件而不是打开
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            HttpContext.Current.Response.BinaryWrite(bt);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 导出Excel 并输出
        /// </summary>
        /// <param name="model">要导入的数据</param>
        /// <param name="templateFileName">ERCHTMS.Web后面的路径</param>
        /// <param name="sheetName">表名</param>
        /// <param name="FileName">导出默认文件名</param>
        public static void ExecuteResultX(DataSet model, string templateFileName, List<string> sheetNames, string FileName)//ControllerContext context
        {
            string ModelUrl = System.AppDomain.CurrentDomain.BaseDirectory;//获取当前目录路径
            System.IO.MemoryStream ms = OuModelFileToStreamX(model, ModelUrl + templateFileName, sheetNames);
            byte[] bt = ms.ToArray();
            //客户端保存文件名
            string fileName = FileName + ".xls";
            //以字符流形式下载文件
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //通知浏览器下载文件而不是打开
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            HttpContext.Current.Response.BinaryWrite(bt);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 将DataTable中相应字段改成String类型
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="column">所有字段</param>
        /// <param name="stringColumn">需要转成String的字段</param>
        /// <returns></returns>
        public static DataTable UpdateDataTable(DataTable dt, string[] column, string[] stringColumn)
        {
            DataTable dtResult = new DataTable();
            dtResult = dt.Clone();//克隆表结构
            for (int i = 0; i < dtResult.Columns.Count; i++)
            {
                foreach (string co in stringColumn)
                {
                    if (dtResult.Columns[i].ColumnName == co.ToLower())//找到对应名字的列
                    {
                        dtResult.Columns[i].DataType = typeof(String);
                    }
                }

            }
            foreach (DataRow dr in dt.Rows)
            {
                DataRow newrow = dtResult.NewRow();
                foreach (string co in column)
                {
                    newrow[co] = dr[co];
                }
                dtResult.Rows.Add(newrow);
            }
            return dtResult;
        }

        /// <summary>
        /// 将DataTable中相应字段改成String类型
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="column">所有字段</param>
        /// <param name="stringColumn">需要转成String的字段</param>
        /// <returns></returns>
        public static DataTable UpdateDataTable(DataTable dt, string[] stringColumn, string type)
        {
            string[] column = new string[dt.Columns.Count];

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                column[i] = dt.Columns[i].ColumnName;
            }


            DataTable dtResult = new DataTable();
            dtResult = dt.Clone();//克隆表结构
            for (int i = 0; i < dtResult.Columns.Count; i++)
            {
                foreach (string co in stringColumn)
                {

                    if (dtResult.Columns[i].ColumnName.ToLower() == co.ToLower())//找到对应名字的列
                    {
                        switch (type.ToLower())
                        {
                            case "string":
                                dtResult.Columns[i].DataType = typeof(String);
                                break;
                            case "datetime":
                                dtResult.Columns[i].DataType = typeof(DateTime);
                                break;
                            default:
                                break;
                        }

                    }
                }

            }
            foreach (DataRow dr in dt.Rows)
            {
                DataRow newrow = dtResult.NewRow();
                foreach (string co in column)
                {
                    if (dr[co] != "")
                    {
                        newrow[co] = dr[co];
                    }
                }
                dtResult.Rows.Add(newrow);
            }
            return dtResult;
        }


        public static void UpdataManage()
        {

           //String oc = ConfigurationManager.ConnectionStrings["conn"].ToString();
            //OracleConnection conn = new OracleConnection(oc);
            //conn.Open();
            //OracleCommand orm = conn.CreateCommand();
            //orm.CommandType = CommandType.StoredProcedure;
            //orm.CommandText = "proc1";
            //orm.ExecuteNonQuery();
            //conn.Close();

        }


    }
}
