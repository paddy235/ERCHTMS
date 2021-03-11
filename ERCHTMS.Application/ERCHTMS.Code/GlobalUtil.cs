using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Text.RegularExpressions;

namespace ERCHTMS.Code
{
    public struct SitePath
    {
        /// <summary>
        /// 虚拟路径
        /// </summary>
        public readonly string VirtualPath;

        /// <summary>
        /// 获取物理路径
        /// </summary>
        public string LocalPath
        {
            get
            {
                string localPath = HttpContext.Current.Server.MapPath(VirtualPath);

                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }

                return localPath;
            }
        }

        /// <summary>
        /// 获取相对路径
        /// </summary>
        /// <returns></returns>
        public string RelativePath
        {
            get { return VirtualPath.Replace("~/", "../../"); }
        }

        public SitePath(string virtualPath)
        {
            VirtualPath = virtualPath;
        }
    }

    public static class GlobalUtil
    {


        /// <summary>
        /// 导出模板路径
        /// </summary>
        public static SitePath TemplatePath
        {
            get
            {
                return new SitePath("~/Resource/ExcelTemplate/");
            }
        }

        /// <summary>
        /// 临时文件
        /// </summary>
        public static SitePath TempPath
        {
            get
            {
                return new SitePath("~/Resource/Temp/");
            }
        }
        /// <summary>
        /// APP存放路径
        /// </summary>
        public static SitePath AppFile
        {
            get
            {
                return new SitePath("~/Resource/AppFile/");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static SitePath AppPath
        {
            get
            {
                return new SitePath("~/Resource/Upfile/");
            }
        }


        public static string GetApplicationPath
        {
            get { return HttpContext.Current.Request.ApplicationPath; }
        }

        /// <summary>
        /// 读取Web.config配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetAppSettings(string key, string defaultValue = "")
        {
            string value = System.Configuration.ConfigurationManager.AppSettings[key];
            if (value == null || value.Trim().Length == 0) return defaultValue;
            return value;
        }
        /// <summary>
        /// 判断字符串是否为空、Null。
        /// </summary>
        /// <param name="data">字符串</param>
        /// <returns>结果</returns>
        public static bool IsNullOrWhiteSpace(this string data)
        {
            return string.IsNullOrWhiteSpace(data);
        }

        #region 下载/打开文件

        public static void DownLoadFile(string filePath, bool isOnLine)
        {
            HttpResponse response = HttpContext.Current.Response;
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            try
            {
                response.Clear();
                string fn = fi.Name;
                if (fn.StartsWith("_TEMP_")) fn = fn.Remove(0, 6);
                if (isOnLine)
                {
                    // 在线打开方式
                    Uri fileUrl = new Uri("file://" + filePath);
                    System.Net.FileWebRequest myFileWebRequest = (System.Net.FileWebRequest)System.Net.WebRequest.Create(fileUrl);
                    System.Net.FileWebResponse myFileWebResponse = (System.Net.FileWebResponse)myFileWebRequest.GetResponse();
                    response.ContentType = myFileWebResponse.ContentType;
                    response.HeaderEncoding = System.Text.Encoding.UTF8;
                    response.AppendHeader("Content-Disposition", "inline; filename=" + System.Web.HttpUtility.UrlEncode(fn));
                    response.Charset = "GB2312";//解决乱码
                    myFileWebResponse.Close();
                }
                else
                {
                    // 纯下载方式
                    response.ContentType = "application/x-msdownload";
                    response.HeaderEncoding = System.Text.Encoding.UTF8;
                    response.AppendHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(fn));
                    response.Charset = "GB2312";//解决乱码
                }
                response.WriteFile(filePath, true);
            }
            catch (Exception ex)
            {
                response.Write(ex.ToString());
            }
            finally
            {
                response.End();
            }
        }

        public static void DownLoadFile(string sourceFullName, string downloadName = "")
        {
            HttpResponse response = HttpContext.Current.Response;
            try
            {
                if (!File.Exists(sourceFullName)) throw new Exception("文件不存在或已被删除1111111!");
                System.IO.FileInfo fi = new System.IO.FileInfo(sourceFullName);
                response.Clear();
                string fn = string.IsNullOrEmpty(downloadName) ? fi.Name : downloadName;

                // 纯下载方式
                response.ContentType = "application/x-msdownload";
                response.HeaderEncoding = System.Text.Encoding.UTF8;
                response.AppendHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(fn));
                response.Charset = "utf8";//解决乱码

                response.WriteFile(sourceFullName, true);

            }
            catch (Exception ex)
            {
                response.Write(ex.ToString());
                throw ex;
            }
            finally
            {
                ClearTemp();
            }
        }

        public static void ClearTemp()
        {
            var files = new DirectoryInfo(TempPath.LocalPath).GetFiles().Where(f => f.CreationTime < DateTime.Now.AddMinutes(-1));
            foreach (var file in files)
            {
                file.Delete();
            }
        }

        #endregion

        #region 导出
        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="dt">查询结果</param>
        /// <param name="path">获取物理路径</param>
        public static void ExportFile(DataTable dt,string path,string sheetName,string fileName) {
            IWorkbook workbook = null;
            FileStream fs = null;
            IRow row = null;
            ISheet sheet = null;
            ICell cell = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                workbook = new HSSFWorkbook();//这个可以导出到xlsx----XSSFWorkbook
                sheet = workbook.CreateSheet(sheetName);//创建一个名称为Sheet0的表
                int rowCount = dt.Rows.Count;//行数
                int columnCount = dt.Columns.Count;//列数

                //设置列头
                row = sheet.CreateRow(0);//excel第一行设为列头
                for (int c = 0; c < columnCount; c++)
                {
                    cell = row.CreateCell(c);
                    cell.SetCellValue(dt.Columns[c].ColumnName);
                }
                //设置每行每列的单元格,
                for (int i = 0; i < rowCount; i++)
                {
                    row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < columnCount; j++)
                    {
                        cell = row.CreateCell(j);//excel第二行开始写入数据
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }
                path = Path.Combine(GlobalUtil.TemplatePath.LocalPath, fileName);
                using (fs = System.IO.File.OpenWrite(path))
                {
                    workbook.Write(fs);//向打开的这个xls文件中写入数据
                }
            }
        }
        #endregion

        #region Json大小写转换
        /// <summary>
        /// Json所有属性名称改为小写字母，值的大小写不变。
        /// </summary>
        /// <param name="data">原Json字符串</param>
        /// <returns>转换小写后的Json字符串</returns>
        public static string ToLowerProperties(this string data)
        {            
            var reg = new Regex("\\\"\\w+\\\":", RegexOptions.IgnoreCase);
            return reg.Replace(data, new MatchEvaluator(new Func<Match, string>(x => { return x.Value.ToLower(); })));
        }
        /// <summary>
        /// Json所有属性名称改为大写字母，值的大小写不变。
        /// </summary>
        /// <param name="data">原Json字符串</param>
        /// <returns>转换大写后的Json字符串</returns>
        public static string ToUpperProperties(this string data)
        {
            var reg = new Regex("\\\"\\w+\\\":", RegexOptions.IgnoreCase);
            return reg.Replace(data, new MatchEvaluator(new Func<Match, string>(x => { return x.Value.ToUpper(); })));
        }
        /// <summary>
        /// 根据类型匹配大小写
        /// </summary>
        /// <typeparam name="T">指定的类型</typeparam>
        /// <param name="data">原Json字符串</param>
        /// <returns>转换后的Json字符串</returns>
        public static string ToMatchEntity<T>(this string data) where T:class
        {
            var temp = data;
            var oType = typeof(T);            
            var properties = oType.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var item in properties)
            {
                var name = item.Name;
                var pattern = "\\\"" + name + "\\\":";
                var reg = new Regex(pattern, RegexOptions.IgnoreCase);
                temp = reg.Replace(temp, new MatchEvaluator(new Func<Match, string>(x => { return "\"" + name + "\":"; })));
            }

            return temp;     
        }
        #endregion
    }

    /// <summary>
    /// 首页指标对象
    /// </summary>
    public class DesktopPageIndex
    {
        public string code { get; set; } //指标代码
        public string name { get; set; } //指标名称
        public string modulecode { get; set; } //模块代码
        public string modulename { get; set; } //模块名称
        public string title { get; set; }  //标题描述
        public string count { get; set; } //返回的整数结果
        public bool isdecimal { get; set; } //返回的小数结果 

    }
}
