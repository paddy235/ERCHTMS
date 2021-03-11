using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Offices;
using ERCHTMS.Code;
using System.IO;
using System.Text;
using System.Threading;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Util;
using System.Drawing.Imaging;
using System.Drawing;
using ERCHTMS.Busines.RoutineSafetyWork;
using System.Net;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Busines.StandardSystem;
using ERCHTMS.Busines.ComprehensiveManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.MatterManage;
using ERCHTMS.Busines.BaseManage;
using Aspose.Cells;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Cache.Factory;
using Microsoft.AspNet.SignalR.Client;
using ERCHTMS.Cache;

namespace ERCHTMS.Web.Controllers
{
    /// <summary>
    /// 描 述：公共控制器
    /// </summary>
    public class UtilityController : Controller
    {
        VisitcarBLL visitcarbll = new VisitcarBLL();
        CarUserBLL CarUserbll = new CarUserBLL();
        HazardouscarBLL Hazardouscarbll = new HazardouscarBLL();
        DataItemDetailBLL pdata = new DataItemDetailBLL();
        #region 验证对象值不能重复
        #endregion

        #region 导出Excel
        /// <summary>
        /// 请选择要导出的字段页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExcelExportForm()
        {
            return View();
        }
        /// <summary>
        /// 执行导出Excel
        /// </summary>
        /// <param name="JsonColumn">表头</param>
        /// <param name="JsonData">数据</param>
        /// <param name="exportField">导出字段</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public void ExecuteExportExcel(string columnJson, string rowJson, string exportField, string filename)
        {
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(filename);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(filename) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnEntity>();
            //表头
            List<GridColumnModel> columnData = columnJson.ToList<GridColumnModel>();
            //行数据
            DataTable rowData = rowJson.ToTable();
            //写入Excel表头
            string[] fieldInfo = exportField.Split(',');
            foreach (string item in fieldInfo)
            {
                var list = columnData.FindAll(t => t.name == item);
                foreach (GridColumnModel gridcolumnmodel in list)
                {
                    if (gridcolumnmodel.hidden.ToLower() == "false" && gridcolumnmodel.label != null)
                    {
                        string align = gridcolumnmodel.align;
                        excelconfig.ColumnEntity.Add(new ColumnEntity()
                        {
                            Column = gridcolumnmodel.name,
                            ExcelColumn = gridcolumnmodel.label,
                            Width = gridcolumnmodel.width > 255 ? 255 : gridcolumnmodel.width,
                            Alignment = gridcolumnmodel.align,
                        });
                    }
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }
        #endregion

        /// <summary>
        /// 获取当前地址路径
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetUrl()
        {
            return Url.Content("~").Substring(0, Url.Content("~").Length - 1);
        }

        /// <summary>
        /// 获取拜访跳转地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetCarUserUrl()
        {
            DataItemDetailBLL dd = new DataItemDetailBLL();
            string path = dd.GetItemValue("imgUrl");
            return path;
        }

        /// <summary>
        /// 获取一个新的Guid
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetNewId()
        {
            return Guid.NewGuid().ToString();
        }
        public ActionResult HasNetwork(string url)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                string result = wc.DownloadString(url);
                return Success("获取数据成功", 1);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 使用CKEDITOR文本编辑器上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UploadFile()
        {
            try
            {
                //没有文件上传，直接返回
                if (Request.Files.Count == 0)
                {
                    return "No Files";
                }
                HttpPostedFileBase Filedata = Request.Files[0];
                //获取文件完整文件名(包含绝对路径)
                //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                string userId = OperatorProvider.Provider.Current().UserId;
                string fileGuid = Guid.NewGuid().ToString();
                long filesize = Filedata.ContentLength;
                string FileEextension = Path.GetExtension(Filedata.FileName);
                string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                string virtualPath = string.Format("/Resource/temp/{0}/{1}/{2}{3}", userId, uploadDate, fileGuid, FileEextension);
                string fullFileName = this.Server.MapPath("~" + virtualPath);
                //创建文件夹
                string path = Path.GetDirectoryName(fullFileName);

                string callback = Request.Params["CKEditorFuncNum"];
                Directory.CreateDirectory(path);
                if (!System.IO.File.Exists(fullFileName))
                {
                    //保存文件
                    Filedata.SaveAs(fullFileName);
                }
                return "<script type=\"text/javascript\">window.parent.CKEDITOR.tools.callFunction(" + callback + ",'" + Request.ApplicationPath + virtualPath + "');</script>";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// FileDrop组件以流的方式实现文件上传
        /// </summary>
        /// <param name="filePath">文件存储路径</param>
        /// <param name="recId">关联记录Id</param>
        /// <param name="isDate">是否按日期目录存储文件</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostFile([System.Web.Http.FromUri]string filePath, [System.Web.Http.FromUri]string recId, [System.Web.Http.FromUri]int isDate = 0, [System.Web.Http.FromUri]string fileExts = "doc,docx,xls,xlsx,zip,rar,ico,icon,jpg,jpeg,png,gif,bmp,psd,txt,ppt,pptx,pdf,mp3,mp4,avi")//ico,icon,raw,jpg,jpeg,gif,bmp,png,psd
        {
            Operator user = OperatorProvider.Provider.Current();
            if (user == null)
            {
                return Content(new AjaxResult { type = ResultType.error, message = "身份验证不通过,操作被拒绝" }.ToJson());
            }
            string newFilePath = "";
            string message = "";
            if (Request.Files.Count > 0)
            {
                foreach (string key in Request.Files.Keys)
                {
                    HttpPostedFileBase file = Request.Files[key];
                    //原始文件名
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    string fileGuid = Guid.NewGuid().ToString();
                    long filesize = file.ContentLength;
                    string FileEextension = Path.GetExtension(fileName);
                    string ext = FileEextension.Substring(1, FileEextension.Length - 1).ToLower();
                    if (!(fileExts + ",").Contains(ext + ","))
                    {
                        message += fileName + ",";
                        continue;
                    }
                    string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                    string dir = isDate == 0 ? string.Format("~/Resource/{0}", filePath) : string.Format("~/Resource/{0}/{1}", filePath, uploadDate);
                    string newFileName = fileGuid + FileEextension;
                    newFilePath = dir + "/" + newFileName;
                    if (!Directory.Exists(Server.MapPath(dir)))
                    {
                        Directory.CreateDirectory(Server.MapPath(dir));
                    }

                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                    if (!System.IO.File.Exists(Server.MapPath(newFilePath)))
                    {
                        //保存文件
                        file.SaveAs(Server.MapPath(newFilePath));
                        //文件信息写入数据库
                        if (!string.IsNullOrEmpty(recId))
                        {
                            fileInfoEntity.Create();
                            fileInfoEntity.FileId = fileGuid;
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                fileInfoEntity.FolderId = filePath;
                            }
                            else
                            {
                                fileInfoEntity.FolderId = "0";
                            }
                            fileInfoEntity.RecId = recId;
                            fileInfoEntity.FileName = fileName;
                            fileInfoEntity.FilePath = dir + "/" + newFileName;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.TrimStart('.');
                            FileInfoBLL fileInfoBLL = new FileInfoBLL();
                            fileInfoBLL.SaveForm("", fileInfoEntity);
                        }

                    }
                }
            }
            message = message.Length == 0 ? "上传成功" : string.Format("不允许上传以下类型文件:{0}", message.Trim(','));
            return Content(new AjaxResult { type = ResultType.success, message = message, resultdata = newFilePath }.ToJson());
        }


        /// <summary>
        /// FileDrop组件以流的方式实现文件上传
        /// </summary>
        /// <param name="filePath">文件存储路径</param>
        /// <param name="recId">关联记录Id</param>
        /// <param name="isDate">是否按日期目录存储文件</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AllPostFile([System.Web.Http.FromUri]string filePath, [System.Web.Http.FromUri]string recId, [System.Web.Http.FromUri]int isDate = 0)
        {
            string newFilePath = "";
            if (Request.Files.Count > 0)
            {
                foreach (string key in Request.Files.Keys)
                {
                    HttpPostedFileBase file = Request.Files[key];
                    //原始文件名
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    long filesize = file.ContentLength;
                    string FileEextension = Path.GetExtension(fileName);
                    string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                    string dir = isDate == 0 ? string.Format("~/Resource/{0}", filePath) : string.Format("~/Resource/{0}/{1}", filePath, uploadDate);

                    foreach (var item in recId.Split(','))
                    {
                        string fileGuid = Guid.NewGuid().ToString();
                        string newFileName = fileGuid + FileEextension;
                        newFilePath = dir + "/" + newFileName;
                        if (!Directory.Exists(Server.MapPath(dir)))
                        {
                            Directory.CreateDirectory(Server.MapPath(dir));
                        }

                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(Server.MapPath(newFilePath)))
                        {
                            //保存文件
                            file.SaveAs(Server.MapPath(newFilePath));
                            //文件信息写入数据库
                            if (!string.IsNullOrEmpty(item))
                            {
                                fileInfoEntity.Create();
                                fileInfoEntity.FileId = fileGuid;
                                if (!string.IsNullOrEmpty(filePath))
                                {
                                    fileInfoEntity.FolderId = filePath;
                                }
                                else
                                {
                                    fileInfoEntity.FolderId = "0";
                                }
                                fileInfoEntity.RecId = item;
                                fileInfoEntity.FileName = fileName;
                                fileInfoEntity.FilePath = dir + "/" + newFileName;
                                fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                fileInfoEntity.FileExtensions = FileEextension;
                                fileInfoEntity.FileType = FileEextension.TrimStart('.');
                                FileInfoBLL fileInfoBLL = new FileInfoBLL();
                                fileInfoBLL.SaveForm("", fileInfoEntity);
                            }

                        }
                    }
                }
            }
            return Content(new AjaxResult { type = ResultType.success, message = "上传成功", resultdata = newFilePath }.ToJson());
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="filePath">文件路径(如~/Resource/Temp/01.doc)</param>
        /// <param name="speed">下载速度控制，默认512KB</param>
        /// <param name="newFileName">下载后默认文件名称（带扩展名）</param>
        [HttpGet]
        public void DownloadFile(string filePath, long speed = 512000, string newFileName = "")
        {
            if (Request.UrlReferrer == null)
            {
                Response.StatusCode = 401;
                Response.Redirect("../Error/ErrorMsg?message=非法操作", true);
                return;
            }
            bool ret = true;
            filePath = Server.UrlDecode(filePath);
            try
            {
                filePath = filePath.ToLower();
                string newPath = filePath.Replace("\\", "/");
                if (!(newPath.Contains("content/") || newPath.Contains("resource/") || newPath.Contains("upfile/")))
                {
                    Response.StatusCode = 401;
                    Response.Redirect("../Error/ErrorMsg?message=非法路径！", true);
                    return;
                }
                //--验证：HttpMethod
                switch (Request.HttpMethod.ToUpper())
                { //目前只支持GET和HEAD方法
                    case "GET":
                    case "HEAD":
                        break;
                    default:
                        Response.StatusCode = 501;
                        return;
                }
                if (filePath.StartsWith("http://"))
                {
                    try
                    {
                        WebClient wc = new WebClient();
                        wc.Credentials = CredentialCache.DefaultCredentials;
                        wc.DownloadFile(filePath, Server.MapPath("~/Resource/Temp/") + System.IO.Path.GetFileName(filePath));
                        filePath = Server.MapPath("~/Resource/Temp/") + System.IO.Path.GetFileName(filePath);
                    }
                    catch
                    {
                        Response.StatusCode = 404;
                        Response.Redirect("../Error/ErrorPath404", true);
                        return;

                    }
                }
                else
                {
                    if (filePath.Contains("\\"))
                    {
                        Response.StatusCode = 404;
                        Response.Redirect("../Error/ErrorPath404", true);
                        return;
                    }
                    if (filePath.Contains("/"))
                    {
                        filePath = Server.MapPath(filePath);
                    }
                }
                if (!System.IO.File.Exists(filePath))
                {
                    Response.StatusCode = 404;
                    Response.Redirect("../Error/ErrorPath404", true);
                    return;
                }
                Response.Clear();
                long startBytes = 0;
                long stopBytes = 0;
                int packSize = 1024 * 10; //分块读取，每块10K bytes
                string fileName = string.IsNullOrEmpty(newFileName) ? Path.GetFileName(filePath) : Server.HtmlDecode(newFileName);
                FileStream myFile = null;
                BinaryReader br = null;
                long fileLength = 0;
                myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                br = new BinaryReader(myFile);
                fileLength = myFile.Length;
                int sleep = (int)Math.Ceiling(1000.0 * packSize / speed);//毫秒数：读取下一数据块的时间间隔
                string lastUpdateTiemStr = System.IO.File.GetLastWriteTimeUtc(filePath).ToString("r");
                //string lastUpdateTiemStr =DateTime.UtcNow.ToString("r");
                string eTag = HttpUtility.UrlEncode(fileName, Encoding.UTF8) + lastUpdateTiemStr;//便于恢复下载时提取请求头;

                //--验证：文件是否太大，是否是续传，且在上次被请求的日期之后是否被修改过
                if (fileLength > long.MaxValue)
                {//-------文件太大了-------
                    Response.StatusCode = 413;//请求实体太大
                    return;
                }

                if (Request.Headers["If-Range"] != null)//对应响应头ETag：文件名+文件最后修改时间
                {
                    //----------上次被请求的日期之后被修改过--------------
                    if (Request.Headers["If-Range"].Replace("\"", "") != eTag)
                    {//文件修改过
                        Response.StatusCode = 412;//预处理失败
                        return;
                    }
                }


                try
                {
                    //-------添加重要响应头、解析请求头、相关验证
                    Response.Clear();

                    if (Request.Headers["Range"] != null)
                    {//------如果是续传请求，则获取续传的起始位置，即已经下载到客户端的字节数------
                        Response.StatusCode = 206;//重要：续传必须，表示局部范围响应。初始下载时默认为200
                        string[] range = Request.Headers["Range"].Split(new char[] { '=', '-' });//"bytes=1474560-"
                        startBytes = Convert.ToInt64(range[1]);//已经下载的字节数，即本次下载的开始位置  
                        if (startBytes < 0 || startBytes >= fileLength)
                        {//无效的起始位置
                            return;
                        }
                        if (range.Length == 3)
                        {
                            stopBytes = Convert.ToInt64(range[2]);//结束下载的字节数，即本次下载的结束位置  
                            if (startBytes < 0 || startBytes >= fileLength)
                            {
                                return;
                            }
                        }
                    }

                    Response.Buffer = false;
                    //Response.AddHeader("Content-MD5", FileHash.MD5File(filePath));//用于验证文件
                    Response.AddHeader("Accept-Ranges", "bytes");//重要：续传必须
                    Response.AppendHeader("ETag", "\"" + eTag + "\"");//重要：续传必须
                    Response.AppendHeader("Last-Modified", lastUpdateTiemStr);//把最后修改日期写入响应                
                    Response.ContentType = "application/octet-stream";//MIME类型：匹配任意文件类型
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).Replace("+", "%20"));
                    Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    Response.AddHeader("Connection", "Keep-Alive");
                    Response.ContentEncoding = Encoding.UTF8;
                    if (startBytes > 0)
                    {//------如果是续传请求，告诉客户端本次的开始字节数，总长度，以便客户端将续传数据追加到startBytes位置后----------
                        Response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }

                    //-------向客户端发送数据块
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Ceiling((fileLength - startBytes + 0.0) / packSize);//分块下载，剩余部分可分成的块数
                    for (int i = 0; i < maxCount && Response.IsClientConnected; i++)
                    {//客户端中断连接，则暂停
                        Response.BinaryWrite(br.ReadBytes(packSize));
                        Response.Flush();
                        if (sleep > 1) Thread.Sleep(sleep);
                    }

                }
                catch
                {
                    ret = false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                ret = false;
            }
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="filePath">文件路径(支持绝对路径、相对路径及远程文件下载.应用内相对路径相对于根目录，如Resource/Temp/01.doc)</param>
        /// <param name="speed">下载速度控制，默认512KB</param>
        /// <param name="newFileName">下载后默认文件名称（带扩展名）</param>
        [HttpGet]
        public void DownloadFileInsertAreas(string filePath, long speed = 512000, int mode = 0, string chkId = "")
        {

            bool ret = true;
            try
            {
                //--验证：HttpMethod
                switch (Request.HttpMethod.ToUpper())
                { //目前只支持GET和HEAD方法
                    case "GET":
                    case "HEAD":
                        break;
                    default:
                        Response.StatusCode = 501;
                        return;
                }
                if (filePath.ToLower().StartsWith("http://"))
                {
                    try
                    {
                        WebClient wc = new WebClient();
                        wc.Credentials = CredentialCache.DefaultCredentials;
                        wc.DownloadFile(filePath, Server.MapPath("~/Resource/Temp/") + System.IO.Path.GetFileName(filePath));
                        filePath = Server.MapPath("~/Resource/Temp/") + System.IO.Path.GetFileName(filePath);
                    }
                    catch
                    {
                        Response.StatusCode = 404;
                        Response.Redirect("../Error/ErrorPath404", true);
                        return;

                    }
                }
                else
                {
                    if (filePath.Contains("/"))
                    {
                        filePath = Server.MapPath(filePath);
                    }
                }
                if (!System.IO.File.Exists(filePath))
                {
                    Response.StatusCode = 404;
                    Response.Redirect("../Error/ErrorPath404", true);
                    return;
                }
                string fileName = Path.GetFileName(filePath);
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();

                wb.Open(filePath);
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtAreas = deptBll.GetDataTable(string.Format("select case when t.parentid='0' then (t.districtname||'=>'||t.districtcode) else '   ' || (districtname||'=>'||districtcode) end 区域名称 from BIS_DISTRICT t where t.districtcode like '{0}%' order by t.districtcode asc", ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode));
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //隐患导入模板(通用版本)
                if (mode == 1)
                {
                    DataTable dtItems = new DataTable();
                    if (!string.IsNullOrWhiteSpace(chkId))
                    {
                        ERCHTMS.Entity.SaftyCheck.SaftyCheckDataRecordEntity sc = new ERCHTMS.Busines.SaftyCheck.SaftyCheckDataRecordBLL().GetEntity(chkId);
                        if (sc.CheckDataType == 1)
                        {
                            dtItems = deptBll.GetDataTable(string.Format("select t.checkobject 检查对象,t.checkcontent 检查内容 from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}'  order by autoid,CheckObject asc ", chkId));
                        }
                        else
                        {
                            dtItems = deptBll.GetDataTable(string.Format("select t.checkobject 检查对象,t.checkcontent 检查内容 from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}' and instr(',' || t.CHECKMANID || ',',',{1},')>0 and id not in(select detailid from BIS_SAFTYCONTENT where recid='{0}')  order by autoid,CheckObject asc ", chkId, user.Account));
                        }

                        wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 0);
                    }
                    wb.Worksheets[1].Cells.ImportDataTable(dtAreas, true, 0, 2, false);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 隐患级别 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidRank')"));
                    //wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 4, false);
                    SetCellOptions(wb.Worksheets[0], dtItems, "隐患级别", 2, 4, 1000, 4);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 专业分类 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidMajorClassify')"));
                    //wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 3,false);
                    SetCellOptions(wb.Worksheets[0], dtItems, "专业分类", 2, 6, 1000, 6);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 隐患类别 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidType')"));
                    //wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 3,false);
                    SetCellOptions(wb.Worksheets[0], dtItems, "隐患类别", 2, 7, 1000, 7);

                    //SetCellOptions(wb.Worksheets[0], dtAreas, "区域名称", 2, 5, wb.Worksheets[0].Cells.MaxRow, 5);
                }
                //违章导入模板(通用版本)
                if (mode == 2)
                {
                  
                    DataTable dtItems = new DataTable();
                    if (!string.IsNullOrWhiteSpace(chkId))
                    {
                        ERCHTMS.Entity.SaftyCheck.SaftyCheckDataRecordEntity sc = new ERCHTMS.Busines.SaftyCheck.SaftyCheckDataRecordBLL().GetEntity(chkId);
                        if (sc.CheckDataType == 1)
                        {
                            dtItems = deptBll.GetDataTable(string.Format("select t.checkobject 检查对象,t.checkcontent 检查内容 from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}' order by autoid,CheckObject asc ", chkId));
                        }
                        else
                        {
                            dtItems = deptBll.GetDataTable(string.Format("select t.checkobject 检查对象,t.checkcontent 检查内容 from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}' and instr(',' || t.CHECKMANID || ',',',{1},')>0 and id not in(select detailid from BIS_SAFTYCONTENT where recid='{0}')  order by autoid,CheckObject asc ", chkId, user.Account));
                        }
                        if (wb.Worksheets.Count == 2) {
                            wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 0);
                        }
                        if (wb.Worksheets.Count == 3)
                        {
                            var itemlist = new string[] { "考核人员" , "考核部门", "第一联责人员", "第一联责部门", "第二联责人员", "第二联责部门" }; //违章流程状态
                            var khDt = new DataTable();
                            khDt.Columns.Add("考核对象");
                            foreach (string itemname in itemlist)
                            {
                                DataRow khrow = khDt.NewRow();
                                khrow["考核对象"] = itemname;
                                khDt.Rows.Add(khrow);
                            }
                            SetCellOptions(wb.Worksheets[1], khDt, "考核对象", 3, 1, 1000, 1);
                            wb.Worksheets[2].Cells.ImportDataTable(dtItems, true, 0, 0);
                        }
                    }
                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 违章类型 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='LllegalType')"));
                    SetCellOptions(wb.Worksheets[0], dtItems, "违章类型", 2, 3, 1000, 3);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 违章级别 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='LllegalLevel')"));
                    SetCellOptions(wb.Worksheets[0], dtItems, "违章级别", 2, 4, 1000, 4);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 专业分类 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidMajorClassify')"));
                    SetCellOptions(wb.Worksheets[0], dtItems, "专业分类", 2, 5, 1000, 5);
                }
                if (mode == 3)
                {
                    DataTable dtItems = new DataTable();
                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 专业分类 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='SpecialtyType')"));
                    //wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 2, false);
                    SetCellOptions(wb.Worksheets[0], dtItems, "专业分类", 2, 9, 1000, 9);
                }
                if (mode == 4)
                {
                    DataTable dtItems = new DataTable();
                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 专业分类 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='ToolEquipmentType')"));
                    //wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 2, false);
                    SetCellOptions(wb.Worksheets[0], dtItems, "专业分类", 2, 1, 1000, 1);
                }
                //违章导入模板(可门电厂版本)
                if (mode == 5)
                {
                    DataTable dtItems = new DataTable();
                    if (!string.IsNullOrWhiteSpace(chkId))
                    {
                        ERCHTMS.Entity.SaftyCheck.SaftyCheckDataRecordEntity sc = new ERCHTMS.Busines.SaftyCheck.SaftyCheckDataRecordBLL().GetEntity(chkId);
                        if (sc.CheckDataType == 1)
                        {
                            dtItems = deptBll.GetDataTable(string.Format("select t.checkobject 检查对象,t.checkcontent 检查内容 from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}' order by autoid,CheckObject asc ", chkId));
                        }
                        else
                        {
                            dtItems = deptBll.GetDataTable(string.Format("select t.checkobject 检查对象,t.checkcontent 检查内容 from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}' and instr(',' || t.CHECKMANID || ',',',{1},')>0 and id not in(select detailid from BIS_SAFTYCONTENT where recid='{0}')  order by autoid,CheckObject asc ", chkId, user.Account));
                        }
                        wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 0);
                    }
                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 违章类型 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='LllegalType')"));
                    SetCellOptions(wb.Worksheets[0], dtItems, "违章类型", 2, 2, 1000, 2);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 违章级别 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='LllegalLevel')"));
                    SetCellOptions(wb.Worksheets[0], dtItems, "违章级别", 2, 3, 1000, 3);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 专业分类 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidMajorClassify')"));
                    SetCellOptions(wb.Worksheets[0], dtItems, "专业分类", 2, 4, 1000, 4);
                }
                //隐患导入模板(华电江陵版本)
                if (mode == 6)
                {
                    DataTable dtItems = new DataTable();
                    if (!string.IsNullOrWhiteSpace(chkId))
                    {
                        ERCHTMS.Entity.SaftyCheck.SaftyCheckDataRecordEntity sc = new ERCHTMS.Busines.SaftyCheck.SaftyCheckDataRecordBLL().GetEntity(chkId);
                        if (sc.CheckDataType == 1)
                        {
                            dtItems = deptBll.GetDataTable(string.Format("select t.checkobject 检查对象,t.checkcontent 检查内容 from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}'  order by autoid,CheckObject asc ", chkId));
                        }
                        else
                        {
                            dtItems = deptBll.GetDataTable(string.Format("select t.checkobject 检查对象,t.checkcontent 检查内容 from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}' and instr(',' || t.CHECKMANID || ',',',{1},')>0 and id not in(select detailid from BIS_SAFTYCONTENT where recid='{0}')  order by autoid,CheckObject asc ", chkId, user.Account));
                        }

                        wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 0);

                        dtItems = deptBll.GetDataTable(string.Format("select a.itemname 隐患类别 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidType') and a.description like '%{0}%'", sc.CheckDataType));
                        SetCellOptions(wb.Worksheets[0], dtItems, "隐患类别", 2, 6, 1000, 6);
                    }
                    wb.Worksheets[1].Cells.ImportDataTable(dtAreas, true, 0, 2, false);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 隐患级别 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidRank')"));
                    SetCellOptions(wb.Worksheets[0], dtItems, "隐患级别", 2, 4, 1000, 4);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 专业分类 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidMajorClassify')"));
                    SetCellOptions(wb.Worksheets[0], dtItems, "专业分类", 2, 5, 1000, 5);
                }
                if (mode == 7)    //违章导入模板(华电江陵版本)
                {
                    DataTable dtItems = new DataTable();

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 隐患类别 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidType') "));
                    SetCellOptions(wb.Worksheets[0], dtItems, "隐患类别", 2, 3, 1000, 3);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 隐患级别 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidRank')"));
                    SetCellOptions(wb.Worksheets[0], dtItems, "隐患级别", 2, 4, 1000, 4);

                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 专业分类 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='HidMajorClassify')"));
                    SetCellOptions(wb.Worksheets[0], dtItems, "专业分类", 2, 5, 1000, 5);
                }
                if (mode == 0) //系统区域
                {
                    int index = wb.Worksheets.Add();
                    wb.Worksheets[index].Name = "系统区域";
                    wb.Worksheets[index].Cells.ImportDataTable(dtAreas, true, 0, 0);
                }
                if (mode == 8) //通用问题导入(安全检查)
                {
                    DataTable dtItems = new DataTable();
                    if (!string.IsNullOrWhiteSpace(chkId))
                    {
                        ERCHTMS.Entity.SaftyCheck.SaftyCheckDataRecordEntity sc = new ERCHTMS.Busines.SaftyCheck.SaftyCheckDataRecordBLL().GetEntity(chkId);
                        if (sc.CheckDataType == 1)
                        {
                            dtItems = deptBll.GetDataTable(string.Format("select t.checkobject 检查对象,t.checkcontent 检查内容 from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}' order by autoid,CheckObject asc ", chkId));
                        }
                        else
                        {
                            dtItems = deptBll.GetDataTable(string.Format("select t.checkobject 检查对象,t.checkcontent 检查内容 from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}' and instr(',' || t.CHECKMANID || ',',',{1},')>0 and id not in(select detailid from BIS_SAFTYCONTENT where recid='{0}')  order by autoid,CheckObject asc ", chkId, user.Account));
                        }
                        wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 0);
                    }
                }
                //反违章量化指标
                if (mode == 9)
                {
                    try
                    {
                        Worksheet sheet = wb.Worksheets[1] as Aspose.Cells.Worksheet;
                        string orgId = string.Empty;
                        PostCache postCache = new PostCache();
                        IList<RoleEntity> rlist = new List<RoleEntity>();
                        DataTable dlist = deptBll.GetDataTable(string.Format("select *  from base_department where organizeid in (select departmentid from base_department where nature = '厂级' ) order by encode "));
                        if (dlist.Rows.Count > 0)
                        {
                            orgId = dlist.Rows[0]["organizeid"].ToString();
                            rlist = postCache.GetList(orgId).OrderBy(x => x.SortCode).ToList();
                            int indexRow = 1;
                            foreach (DataRow dentity in dlist.Rows)
                            {
                                Aspose.Cells.Cell cell = sheet.Cells[indexRow, 0];
                                if (!string.IsNullOrEmpty(dentity["fullname"].ToString()))
                                {
                                    cell.PutValue(dentity["fullname"].ToString()); //填报单位
                                }
                                var templist = rlist.Where(p => p.DeptId == dentity["departmentid"].ToString()).Select(p => p.FullName).ToList();
                                if (templist.Count() > 0)
                                {
                                    int roleIndex = 0;
                                    foreach (string rolename in templist)
                                    {
                                        Aspose.Cells.Cell rolecell = sheet.Cells[indexRow + roleIndex, 1];
                                        if (!string.IsNullOrEmpty(rolename))
                                        {
                                            rolecell.PutValue(rolename); //填报单位
                                        }
                                        roleIndex++;
                                    }
                                    //合并单位格
                                    Aspose.Cells.Cells cells = sheet.Cells;
                                    cells.Merge(indexRow, 0, templist.Count(), 1);
                                    indexRow += templist.Count();
                                }
                                else
                                {
                                   indexRow++;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                //三种人导入
                if (mode == 10)
                {
                    DataTable dtItems = new DataTable();
                    dtItems = deptBll.GetDataTable(string.Format("select a.itemname 三种人类别 from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem  b where b.itemcode='threepeople')"));
                    //wb.Worksheets[1].Cells.ImportDataTable(dtItems, true, 0, 2, false);
                    SetCellOptions(wb.Worksheets[0], dtItems, "三种人类别", 2, 2, 1000, 2);
                }
                filePath = Server.MapPath("~/Resource/Temp/" + DateTime.Now.ToString("yyyy-MMddHHmmss") + "_" + fileName);
                if (wb.Worksheets.Count == 2)
                {
                    wb.Worksheets[1].AutoFitColumns();
                    wb.Worksheets[1].Cells[0, 0].Style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    wb.Worksheets[1].Cells[0, 0].Style.Font.IsBold = true;
                }
                if (wb.Worksheets.Count == 3)
                {
                    wb.Worksheets[2].AutoFitColumns();
                    wb.Worksheets[2].Cells[0, 0].Style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    wb.Worksheets[2].Cells[0, 0].Style.Font.IsBold = true;
                }
                wb.Save(filePath);

                Response.Clear();
                long startBytes = 0;
                long stopBytes = 0;
                int packSize = 1024 * 10; //分块读取，每块10K bytes
                FileStream myFile = null;
                BinaryReader br = null;
                long fileLength = 0;
                myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                br = new BinaryReader(myFile);
                fileLength = myFile.Length;
                int sleep = (int)Math.Ceiling(1000.0 * packSize / speed);//毫秒数：读取下一数据块的时间间隔
                string lastUpdateTiemStr = System.IO.File.GetLastWriteTimeUtc(filePath).ToString("r");
                //string lastUpdateTiemStr =DateTime.UtcNow.ToString("r");
                string eTag = HttpUtility.UrlEncode(fileName, Encoding.UTF8) + lastUpdateTiemStr;//便于恢复下载时提取请求头;

                //--验证：文件是否太大，是否是续传，且在上次被请求的日期之后是否被修改过
                if (fileLength > long.MaxValue)
                {//-------文件太大了-------
                    Response.StatusCode = 413;//请求实体太大
                    return;
                }

                if (Request.Headers["If-Range"] != null)//对应响应头ETag：文件名+文件最后修改时间
                {
                    //----------上次被请求的日期之后被修改过--------------
                    if (Request.Headers["If-Range"].Replace("\"", "") != eTag)
                    {//文件修改过
                        Response.StatusCode = 412;//预处理失败
                        return;
                    }
                }


                try
                {
                    //-------添加重要响应头、解析请求头、相关验证
                    Response.Clear();

                    if (Request.Headers["Range"] != null)
                    {//------如果是续传请求，则获取续传的起始位置，即已经下载到客户端的字节数------
                        Response.StatusCode = 206;//重要：续传必须，表示局部范围响应。初始下载时默认为200
                        string[] range = Request.Headers["Range"].Split(new char[] { '=', '-' });//"bytes=1474560-"
                        startBytes = Convert.ToInt64(range[1]);//已经下载的字节数，即本次下载的开始位置  
                        if (startBytes < 0 || startBytes >= fileLength)
                        {//无效的起始位置
                            return;
                        }
                        if (range.Length == 3)
                        {
                            stopBytes = Convert.ToInt64(range[2]);//结束下载的字节数，即本次下载的结束位置  
                            if (startBytes < 0 || startBytes >= fileLength)
                            {
                                return;
                            }
                        }
                    }

                    Response.Buffer = false;
                    //Response.AddHeader("Content-MD5", FileHash.MD5File(filePath));//用于验证文件
                    Response.AddHeader("Accept-Ranges", "bytes");//重要：续传必须
                    Response.AppendHeader("ETag", "\"" + eTag + "\"");//重要：续传必须
                    Response.AppendHeader("Last-Modified", lastUpdateTiemStr);//把最后修改日期写入响应                
                    Response.ContentType = "application/octet-stream";//MIME类型：匹配任意文件类型
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).Replace("+", "%20"));
                    Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    Response.AddHeader("Connection", "Keep-Alive");
                    Response.ContentEncoding = Encoding.UTF8;
                    if (startBytes > 0)
                    {//------如果是续传请求，告诉客户端本次的开始字节数，总长度，以便客户端将续传数据追加到startBytes位置后----------
                        Response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }

                    //-------向客户端发送数据块
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Ceiling((fileLength - startBytes + 0.0) / packSize);//分块下载，剩余部分可分成的块数
                    for (int i = 0; i < maxCount && Response.IsClientConnected; i++)
                    {//客户端中断连接，则暂停
                        Response.BinaryWrite(br.ReadBytes(packSize));
                        Response.Flush();
                        if (sleep > 1) Thread.Sleep(sleep);
                    }

                }
                catch
                {
                    ret = false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                ret = false;
            }
        }

        public void SetCellOptions(Aspose.Cells.Worksheet sheet, DataTable dt, string title, int startRow, int startCol, int endRow, int endCol)
        {
            string formulal = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                formulal += row[title].ToString() + ",";
            }
            if (!string.IsNullOrEmpty(formulal))
            {
                formulal = formulal.Substring(0, formulal.Length - 1);
            }
            Validations validations = sheet.Validations;
            // Create a new validation to the validations list.
            Validation validation = validations[validations.Add()];
            // Set the validation type.
            validation.Type = Aspose.Cells.ValidationType.List;
            // Set the operator.
            validation.Operator = OperatorType.None;
            // Set the in cell drop down.
            validation.InCellDropDown = true;
            // Set the formula1.
            validation.Formula1 = formulal;
            CellArea area = new CellArea();
            area.StartRow = startRow;
            area.StartColumn = startCol;
            area.EndColumn = endCol;
            area.EndRow = endRow; //wb.Worksheets[1].Cells.MaxRow
            // Add the validation area.
            validation.AreaList.Add(area);
        }

        /// <summary>
        /// 生成二维码并输出
        /// </summary>
        /// <param name="keyValue">生成二维码的key</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BuilderImage(string keyValue, int mode = 0)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 10;
            qrCodeEncoder.QRCodeScale = 2;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            MemoryStream ms = new MemoryStream();
            Bitmap bmp = qrCodeEncoder.Encode(keyValue, Encoding.UTF8);
            bmp.Save(ms, ImageFormat.Jpeg);
            bmp.Dispose();
            if (mode == 1)
            {
                CacheFactory.Cache().WriteCache("1", keyValue.Replace("|扫码登录", ""), DateTime.Now.AddMinutes(5));
            }
            return File(ms.ToArray(), @"image/Jpeg");
        }

        /// <summary>
        /// 生成二维码并输出(参数少间距疏的)
        /// </summary>
        /// <param name="keyValue">生成二维码的key</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BuilderSmallImage(string keyValue, int length = 6, int mode = 0)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = length;
            qrCodeEncoder.QRCodeScale = 10;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            MemoryStream ms = new MemoryStream();
            Bitmap bmp = qrCodeEncoder.Encode(keyValue, Encoding.UTF8);
            bmp.Save(ms, ImageFormat.Jpeg);
            bmp.Dispose();
            if (mode == 1)
            {
                CacheFactory.Cache().WriteCache("1", keyValue.Replace("|扫码登录", ""), DateTime.Now.AddMinutes(5));
            }
            return File(ms.ToArray(), @"image/Jpeg");
        }
        /// <summary>
        /// 生成并下载二维码
        /// </summary>
        /// <param name="keyValue">生成二维码的key</param>
        /// <returns></returns>
        [HttpGet]
        public void DownloadQrImage(string keyValue)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 10;
            qrCodeEncoder.QRCodeScale = 2;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            MemoryStream ms = new MemoryStream();
            Bitmap bmp = qrCodeEncoder.Encode(keyValue, Encoding.UTF8);
            if (keyValue.Contains("|"))
                keyValue = keyValue.Split('|')[0];
            string fileName = "~/Resource/Temp/" + keyValue + ".jpg";
            bmp.Save(Server.MapPath(fileName), ImageFormat.Jpeg);
            bmp.Dispose();
            DownloadFile(fileName);
        }
        [HttpGet]
        public void DownloadQrImage10(string keyValue, string equipName, string equipNo)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 21;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            MemoryStream ms = new MemoryStream();
            float size = 302, margin = 37.5f;
            System.Drawing.Image image = qrCodeEncoder.Encode(keyValue, Encoding.UTF8);
            int resWidth = (int)(size + 2 * margin);
            int resHeight = (int)(size + 3 * margin);
            // 核心就是这里新建一个bitmap对象然后将image在这里渲染
            Bitmap newBit = new Bitmap(resWidth, resHeight, PixelFormat.Format32bppRgb);
            Graphics gg = Graphics.FromImage(newBit);

            // 设置背景白色
            for (int x = 0; x < resWidth; x++)
            {
                for (int y = 0; y < resHeight; y++)
                {
                    newBit.SetPixel(x, y, System.Drawing.Color.White);
                }
            }

            // 设置黑色边框
            for (int i = 0; i < resWidth; i++)
            {
                newBit.SetPixel(i, 0, System.Drawing.Color.Black);
                newBit.SetPixel(i, resHeight - 1, System.Drawing.Color.Black);

            }

            for (int j = 0; j < resHeight; j++)
            {
                newBit.SetPixel(0, j, System.Drawing.Color.Black);
                newBit.SetPixel(resWidth - 1, j, System.Drawing.Color.Black);

            }
            RectangleF desRect = new RectangleF() { X = margin, Y = margin, Width = size, Height = size };
            RectangleF srcRect = new RectangleF() { X = 0, Y = 0, Width = image.Width, Height = image.Height };
            gg.DrawImage(image, desRect, srcRect, GraphicsUnit.Pixel);
            //gg.DrawImage(image, margin, margin, size, size);
            var font = new System.Drawing.Font(new FontFamily("微软雅黑"), 12.0f);
            //设备名称及位置
            if (!string.IsNullOrWhiteSpace(equipName))
            {
                var fSize = gg.MeasureString(equipName, font);
                float fX = (resWidth - fSize.Width) / 2.0f;
                float fY = size + margin + 5;
                gg.DrawString(equipName, font, Brushes.Black, new PointF(fX, fY));
            }
            //设备编号及位置           
            if (!string.IsNullOrWhiteSpace(equipNo))
            {
                var fSize = gg.MeasureString(equipNo, font);
                float fX = (resWidth - fSize.Width) / 2.0f;
                float fY = size + margin + 5 + fSize.Height;
                gg.DrawString(equipNo, font, Brushes.Black, new PointF(fX, fY));
            }

            if (keyValue.Contains("|"))
                keyValue = keyValue.Split('|')[0];

            string filePath = "~/Resource/Temp/" + keyValue + ".jpg";
            newBit.Save(Server.MapPath(filePath), ImageFormat.Jpeg);
            newBit.Dispose();
            image.Dispose();
            DownloadFile(filePath);
        }

        /// <summary>
        /// 下载并生成 重点部位 二维码
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="Name"></param>
        /// <param name="Dept"></param>
        /// <param name="User"></param>
        [HttpGet]
        public void DownloadQrImageKeypart(string keyValue, string Name, string Dept, string User)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 21;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            MemoryStream ms = new MemoryStream();
            float size = 302, margin = 37.5f;
            System.Drawing.Image image = qrCodeEncoder.Encode(keyValue, Encoding.UTF8);
            int resWidth = (int)(size + 2 * margin);
            int resHeight = (int)(size + 3 * margin);
            // 核心就是这里新建一个bitmap对象然后将image在这里渲染
            Bitmap newBit = new Bitmap(resWidth, resHeight, PixelFormat.Format32bppRgb);
            Graphics gg = Graphics.FromImage(newBit);

            // 设置背景白色
            for (int x = 0; x < resWidth; x++)
            {
                for (int y = 0; y < resHeight; y++)
                {
                    newBit.SetPixel(x, y, System.Drawing.Color.White);
                }
            }

            // 设置黑色边框
            for (int i = 0; i < resWidth; i++)
            {
                newBit.SetPixel(i, 0, System.Drawing.Color.Black);
                newBit.SetPixel(i, resHeight - 1, System.Drawing.Color.Black);

            }

            for (int j = 0; j < resHeight; j++)
            {
                newBit.SetPixel(0, j, System.Drawing.Color.Black);
                newBit.SetPixel(resWidth - 1, j, System.Drawing.Color.Black);

            }
            RectangleF desRect = new RectangleF() { X = margin, Y = margin, Width = size, Height = size };
            RectangleF srcRect = new RectangleF() { X = 0, Y = 0, Width = image.Width, Height = image.Height };
            gg.DrawImage(image, desRect, srcRect, GraphicsUnit.Pixel);
            //gg.DrawImage(image, margin, margin, size, size);
            var font = new System.Drawing.Font(new FontFamily("微软雅黑"), 12.0f);
            //重点部位名称及位置
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var fSize = gg.MeasureString(Name, font);
                float fX = (resWidth - fSize.Width) / 2.0f;
                float fY = size + margin + 5;
                gg.DrawString(Name, font, Brushes.Black, new PointF(fX, fY));
            }
            //责任部门及位置           
            if (!string.IsNullOrWhiteSpace(Dept))
            {
                var fSize = gg.MeasureString(Dept, font);
                float fX = (resWidth - fSize.Width) / 2.0f;
                float fY = size + margin + 5 + fSize.Height;
                gg.DrawString(Dept, font, Brushes.Black, new PointF(fX, fY));
            }
            //责任人及位置           
            if (!string.IsNullOrWhiteSpace(User))
            {
                var fSize = gg.MeasureString(User, font);
                float fX = (resWidth - fSize.Width) / 2.0f;
                float fY = size + margin + 5 + fSize.Height + fSize.Height;
                gg.DrawString(User, font, Brushes.Black, new PointF(fX, fY));
            }

            if (keyValue.Contains("|"))
                keyValue = keyValue.Split('|')[0];
            string fileName = "~/Resource/Temp/" + keyValue + ".jpg";
            newBit.Save(Server.MapPath(fileName), ImageFormat.Jpeg);
            newBit.Dispose();
            image.Dispose();
            DownloadFile(fileName);
        }
        /// <summary>
        /// 获取安全动态、安全红黑榜实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public string GetSecurityDynamics(string keyValue, string state)
        {
            if (state == "0")//安全动态
            {
                SecurityDynamicsBLL securitydynamicsbll = new SecurityDynamicsBLL();
                var data = securitydynamicsbll.GetEntity(keyValue);
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            //安全红黑榜
            else if (state == "1")
            {
                SecurityRedListBLL securityredlistbll = new SecurityRedListBLL();
                var data = securityredlistbll.GetEntity(keyValue);
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            else if (state == "2")//标准化新闻
            {
                var data = new StandardNewsBLL().GetEntity(keyValue);
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            else if (state == "4")//荣誉分享
            {
                var data = new HonoursBLL().GetEntity(keyValue);
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            else
            {
                var data = new StaffMienBLL().GetEntity(keyValue);
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
        }

        /// <summary>
        /// 获取实体和附件信息 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public string GetFormAndFile(string keyValue)
        {
            var data = new Busines.RoutineSafetyWork.AnnouncementBLL().GetEntity(keyValue);
            var fileList = new FileInfoBLL().GetFileList(keyValue);
            fileList.ForEach(x =>
            {
                x.FilePath = Request.ApplicationPath + x.FilePath.Replace("~/", "/");
            });
            var jsonData = new
            {
                data = data,
                fileList = fileList,
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(jsonData);
        }


        /// <summary>
        /// 根据数据字典中配置读取服务器上的txt 显示查看标准
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetTableHtml(string keyValue)
        {
            string table = "";
            string fileUrl = keyValue.Substring(1);//去除前面的~号
            string filePath = Server.MapPath(Request.ApplicationPath + fileUrl);
            //判断文件是否存在
            if (DirFileHelper.IsExistFile(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default))
                {
                    table = sr.ReadToEnd();
                }
            }
            //string table = @"<table><tr><td rowspan='2'>接触时间率</td><td colspan='4'>体力劳动强度</td></tr><tr><td>I</td><td>II</td><td>III</td><td>IV</td></tr><tr><td>100%</td><td>30</td><td>28</td><td>26</td><td>25</td></tr><tr><td>75%</td><td>31</td><td>29</td><td>28</td><td>26</td></tr><tr><td>50%</td><td>32</td><td>30</td><td>29</td><td>28</td></tr><tr><td>25%</td><td>33</td><td>32</td><td>31</td><td>30</td></tr><tr><td colspan='5'>接触时间率：劳动者在一个工作日内实际接触高温作业的累计时间与8h的比率。</td></tr></table>";
            return table;

        }

        #region 按照基础平台规则生成在线预览地址
        /// <summary>
        /// word转PDF
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFileUrl(string keyValue, string type)
        {
            try
            {
                var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
                if (di.GetItemValue("flag") == "1")
                {
                    type = "1";
                }
                if (type == "0")
                {
                    string fid = string.Empty;
                    if (keyValue.Contains('.'))
                    {
                        fid = keyValue;
                    }
                    else
                    {
                        SpecialEquipmentBLL seb = new SpecialEquipmentBLL();
                        //根据法规ID获取FID(带后缀)
                        string sql = string.Format(@"select t.var_fid from ex_attachment t where t.id 
in(select attachment_id from ex_law_attachment t where t.law_id='{0}' and t.law_type='01')", keyValue);
                        DataTable dt = seb.SelectData(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            fid = dt.Rows[0][0].ToString();
                        }
                        else
                        {
                            return Content("0");
                        }
                    }

                    long expiresLong = DateTime.Now.Ticks / 1000 + 180;
                    string expires = expiresLong.ToString();
                    string token = Md5Helper.MD5(di.GetItemValue("antiStealLink.key", "Resource") + fid + expires, 32);
                    string url = string.Format(di.GetItemValue("LawWebUrl", "Resource"), di.GetItemValue("appId", "Resource"), fid, expires, token);
                    return Content(url);
                }
                else
                {
                    FileInfoBLL fileInfoBLL = new FileInfoBLL();
                    DataTable fie = fileInfoBLL.GetFiles(keyValue);
                    if (fie != null && fie.Rows.Count > 0)
                    {
                        string path = fie.Rows[0]["FilePath"].ToString().ToLower();
                        string[] str = path.Split('/');
                        if (str[str.Length - 1].EndsWith(".pdf"))
                        {
                            return Content(path);
                        }
                        else if (str[str.Length - 1].EndsWith(".jpg") || str[str.Length - 1].EndsWith(".png") || str[str.Length - 1].EndsWith(".gif") || str[str.Length - 1].EndsWith(".jpeg"))
                        {
                            return Content(path.Replace("~", Request.ApplicationPath));
                        }
                        else if (str[str.Length - 1].EndsWith(".docx") || str[str.Length - 1].EndsWith(".doc"))
                        {
                            path = Server.MapPath(path);
                            string str1 = "~/Resource/Temp/" + str[str.Length - 1].Replace("docx", "pdf").Replace("doc", "pdf");
                            string savePath = Server.MapPath(str1);
                            if (!System.IO.File.Exists(savePath))
                            {
                                System.IO.File.Copy(path, savePath);
                                Aspose.Words.Document doc = new Aspose.Words.Document(savePath);
                                doc.Save(savePath, Aspose.Words.SaveFormat.Pdf);
                            }
                            return Content(str1);
                        }
                        else
                        {
                            return Content("0");
                        }

                    }
                    return Content("0");
                }


            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 获取程序名称
        /// </summary>
        [HttpGet]
        public string GetSoftName()
        {
            string enName = Request.ApplicationPath;
            //string enName = Config.GetValue("SoftName");
            return enName;
        }
        #endregion



        #region 物料管理系统手机上传司机信息


        /// <summary>
        /// 获取入场开票信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetOperticketJson(string keyValue)
        {
            OperticketmanagerEntity data = new OperticketmanagerBLL().GetEntity(keyValue);
            string sql = "select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='AppSettings' and t.itemname='imgUrl'  order by t.sortcode asc";
            var dt = new OperticketmanagerBLL().GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {//上传图片地址
                string[] str = dt.Rows[0][1].ToString().Split('/');
                data.Remark = str[3];
            }
            return data == null ? "" : data.ToJson();
        }

        /// <summary>
        /// 获取来访车辆信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpPost]
        public string GetVisitJson(string keyValue)
        {
            VisitcarEntity visitcar = visitcarbll.GetEntity(keyValue);
            string sql = string.Format("select d.username,d.userimg from bis_usercarfileimg d where d.baseid='{0}' order by d.ordernum asc", keyValue);
            var dt = new OperticketmanagerBLL().GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {//随行人员信息
                visitcar.AccompanyingPerson = dt.ToJson();
            }
            return visitcar == null ? "" : visitcar.ToJson();
        }

        /// <summary>
        /// 获取拜访人员
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetUserJson(string keyValue)
        {
            CarUserEntity visitcar = CarUserbll.GetEntity(keyValue);
            string sql = string.Format("select d.username,d.userimg from bis_usercarfileimg d where d.baseid='{0}' order by d.ordernum asc", keyValue);
            var dt = new OperticketmanagerBLL().GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {//随行人员信息
                visitcar.AccompanyingPerson = dt.ToJson();
            }
            return visitcar == null ? "" : visitcar.ToJson();
        }

        /// <summary>
        /// 获取来访车辆信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpPost]
        public string GetHazardouscarJson(string keyValue)
        {
            HazardouscarEntity Hazardouscar = Hazardouscarbll.GetEntity(keyValue);
            string sql = string.Format("select d.username,d.userimg from bis_usercarfileimg d where d.baseid='{0}' order by d.ordernum asc", keyValue);
            var dt = new OperticketmanagerBLL().GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {//随行人员信息
                Hazardouscar.AccompanyingPerson = dt.ToJson();
            }
            return Hazardouscar == null ? "" : Hazardouscar.ToJson();
        }

        /// <summary>
        /// 获取更多司机申报记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string ShowShenPaoRecord(string Tel)
        {
            SpecialEquipmentBLL seb = new SpecialEquipmentBLL();
            string sql = string.Format("select Platenumber,Transporttype,modifydate,jsimgpath,xsimgpath,HzWeight,DriverName,identitetiimg from WL_OPERTICKETMANAGER d where d.drivertel='{0}'", Tel);
            DataTable dt = seb.SelectData(sql);
            sql = "select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='AppSettings' and t.itemname='imgUrl'  order by t.sortcode asc";
            var dts = new OperticketmanagerBLL().GetDataTable(sql);
            string strurl = string.Empty;
            if (dts.Rows.Count > 0)
            {//上传图片地址
                string[] str = dts.Rows[0][1].ToString().Split('/');
                strurl = str[3];
            }
            dt.Columns.Add("strurl");
            foreach (DataRow item in dt.Rows)
            {
                item["strurl"] = strurl;
            }
            return dt == null ? "" : dt.ToJson();
        }

        /// <summary>
        /// 通过手机号获取司机上报记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetVisitJsonList(string Phone)
        {
            List<VisitcarEntity> visitcar = visitcarbll.GetList(Phone).ToList();
            return visitcar == null ? "" : visitcar.ToJson();
        }

        /// <summary>
        /// 通过手机号获取申请人上报人员记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetUserJsonList(string Phone)
        {
            List<CarUserEntity> visitcar = CarUserbll.GetList(Phone).ToList();
            return visitcar == null ? "" : visitcar.ToJson();
        }


        /// <summary>
        /// 通过手机号获取司机上报记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetHazardousJsonList(string Phone)
        {
            List<HazardouscarEntity> Hazardouscar = Hazardouscarbll.GetList(Phone).ToList();
            return Hazardouscar == null ? "" : Hazardouscar.ToJson();
        }

        /// <summary>
        /// 获取此危害因素是否配置了检查表
        /// </summary>
        /// <param name="HazardousId"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetHazardous(string HazardousId)
        {
            return Hazardouscarbll.GetHazardous(HazardousId).ToString();
        }

        /// <summary>
        /// 查询是否有重复车牌号拜访车辆/危化品车辆
        /// </summary>
        /// <param name="CarNo">车牌号</param>
        /// <param name="type">3位拜访 5为危化品</param>
        /// <returns></returns>
        [HttpGet]
        public string GetVisitCf(string CarNo, int type)
        {
            return visitcarbll.GetVisitCf(CarNo, type).ToString();
        }
        /// <summary>
        /// 查询人员拜访是否重复提交
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool GetUserVisitCf(string Phone, string name, int type)
        {
            string sql = string.Format("select count(1) from BIS_USERCAR d where d.state<4 and d.dirver='{0}' and d.phone='{1}'", name, Phone);
            var dt = new OperticketmanagerBLL().GetDataTable(sql);
            bool b = false;
            if (dt.Rows[0][0].ToString() != "0")
            {
                b = true;
            }
            return b;
        }


        /// <summary>
        /// 获取数据字典列表（绑定控件）
        /// </summary>
        /// <param name="EnCode">代码</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDataItemListJson(string EnCode, string Remark = "")
        {
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'" + EnCode + "'");
            if (!string.IsNullOrWhiteSpace(Remark))
            {
                data = data.Where(x => x.ItemCode == Remark);
            }
            return Content(data.ToJson());
        }

        /// <summary>
        /// 物料车司机提交完善信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="Name"></param>
        /// <param name="Tel"></param>
        /// <param name="weight"></param>
        public void SaveOperticketJson(string keyValue, string Name, string Tel, string weight, string JsImgpath, string XsImgpath, int ISwharf)
        {
            OperticketmanagerEntity entity = new OperticketmanagerBLL().GetEntity(keyValue);
            if (entity != null)
            {
                entity.DriverName = Name;
                entity.DriverTel = Tel;
                entity.ExamineStatus = 1;
                entity.JsImgpath = JsImgpath;
                entity.ISwharf = ISwharf;
                entity.XsImgpath = XsImgpath;
                if (!string.IsNullOrEmpty(weight))
                {
                    entity.HzWeight = double.Parse(weight);
                }
                new OperticketmanagerBLL().SaveForm(keyValue, entity);
                //CarinfoBLL carinfobll = new CarinfoBLL();
                //var list = carinfobll.GetList("").Where(a => a.CarNo == entity.Platenumber).ToList();
                //if (list != null && list.Count == 0)
                //{//车辆信息同步到海康平台
                //    CarinfoEntity carin = new CarinfoEntity();
                //    carin.CarNo = entity.Platenumber;
                //    carin.Type = 4;
                //    carin.Dirver = entity.DriverName;
                //    carin.Phone = entity.DriverTel;
                //    carin.Deptname = entity.Takegoodsname;
                //    carin.GpsId = entity.GpsId;
                //    carin.GpsName = entity.GpsName;
                //    carinfobll.SaveForm("", carin, "", "");
                //}
            }
        }
        /// <summary>
        /// 给前端推送让其自动刷新
        /// </summary>
        public void SenSignalrdMsg()
        {
            //给前端推送让其自动刷新
            string url = CacheFactory.Cache().GetCache<string>("SignalRUrl");
            if (url.IsNullOrWhiteSpace())
            {
                url = pdata.GetItemValue("SignalRUrl");
                url = url.Replace("signalr", "").Replace("\"", "");
                CacheFactory.Cache().WriteCache<string>(url, "SignalRUrl");
            }
            HubConnection hubConnection = null;
            IHubProxy ChatsHub = null;
            hubConnection = new HubConnection(url);
            ChatsHub = hubConnection.CreateHubProxy("ChatsHub");
            hubConnection.Start().ContinueWith(task =>
            {
                if (!task.IsFaulted)
                //连接成功调用服务端方法
                {
                    var message = new { info = "刷新一下！" };
                    ChatsHub.Invoke("sendMsgKm", "menwei", message.ToJson());
                    //结束连接
                    //hubConnection.Stop();
                }
            });
        }
        /// <summary>
        /// 新增拜访车辆
        /// </summary>
        /// <param name="Dirver"></param>
        /// <param name="CarNo"></param>
        /// <param name="VisitUser"></param>
        /// <param name="VisitUserPhone"></param>
        /// <param name="Phone"></param>
        /// <param name="AccompanyingNumber"></param>
        /// <param name="AccompanyingPerson"></param>
        /// <param name="VisitDept"></param>
        /// <param name="DriverLicenseUrl"></param>
        /// <param name="DrivingLicenseUrl"></param>
        public void SaveVisit(string keyValue, string Dirver, string CarNo, string ComName, string VisitUser, string VisitUserPhone, string Phone,
            string AccompanyingNumber, string AccompanyingPerson, string VisitDept, string DriverLicenseUrl,
            string DrivingLicenseUrl, string ApplyDate, List<CarUserFileImgEntity> userjson)
        {

            VisitcarEntity visitcar = new VisitcarEntity();
            visitcar.ID = keyValue;
            visitcar.AccompanyingNumber = Convert.ToInt32(AccompanyingNumber);
            //visitcar.AccompanyingPerson = AccompanyingPerson;
            visitcar.CarNo = CarNo;
            visitcar.ApplyDate = Convert.ToDateTime(ApplyDate);
            visitcar.ComName = ComName;
            visitcar.Dirver = Dirver;
            visitcar.DriverLicenseUrl = DriverLicenseUrl;
            visitcar.DrivingLicenseUrl = DrivingLicenseUrl;
            visitcar.GPSID = "";
            visitcar.GPSNAME = "";
            visitcar.Note = "";
            visitcar.Phone = Phone;
            visitcar.State = 1;
            visitcar.VisitDept = VisitDept;
            visitcar.VisitUser = VisitUser;
            visitcar.VisitUserPhone = VisitUserPhone;
            visitcar.CreateUserId = "System";
            visitcar.CreateDate = DateTime.Now;
            visitcar.CreateUserDeptCode = "00";
            visitcar.CreateUserOrgCode = "00";
            for (int i = 0; i < userjson.Count; i++)
            {//随行人员人脸图片
                string srcPath = Server.MapPath("~" + userjson[i].Userimg);
                if (System.IO.File.Exists(srcPath))
                { //读图片转为Base64String
                    var base64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(srcPath));
                    userjson[i].Imgdata = base64;
                }
            }
            visitcarbll.SaveFaceUserForm("", visitcar, userjson);
            //CarinfoBLL carinfobll = new CarinfoBLL();
            //var list = carinfobll.GetList("").Where(a => a.CarNo == visitcar.CarNo).ToList();
            //if (list != null && list.Count == 0)
            //{//车辆信息同步到海康平台
            //    CarinfoEntity carin = new CarinfoEntity();
            //    carin.CarNo = visitcar.CarNo;
            //    carin.Type = 3;
            //    carin.Dirver = visitcar.Dirver;
            //    carin.Phone = visitcar.Phone;
            //    carin.Deptname = visitcar.ComName;
            //    carin.GpsId = visitcar.GPSID;
            //    carin.GpsName = visitcar.GPSNAME;
            //    carinfobll.SaveForm("", carin, "", "");
            //}
            SenSignalrdMsg();
        }


        /// <summary>
        /// 新增拜访人员
        /// </summary>
        /// <param name="Dirver"></param>
        /// <param name="CarNo"></param>
        /// <param name="VisitUser"></param>
        /// <param name="VisitUserPhone"></param>
        /// <param name="Phone"></param>
        /// <param name="AccompanyingNumber"></param>
        /// <param name="AccompanyingPerson"></param>
        /// <param name="VisitDept"></param>
        /// <param name="DriverLicenseUrl"></param>
        /// <param name="DrivingLicenseUrl"></param>
        public void SaveUserform(string keyValue, string Dirver, string Identitynum, string CarNo, string VisitUser, string VisitUserPhone, string Phone,
            string AccompanyingNumber, string AccompanyingPerson, string VisitDept,
            string DrivingLicenseUrl, string ApplyDate, List<CarUserFileImgEntity> userjson)
        {

            CarUserEntity visitcar = new CarUserEntity();
            visitcar.ID = keyValue;
            visitcar.AccompanyingNumber = Convert.ToInt32(AccompanyingNumber);
            // visitcar.AccompanyingPerson = AccompanyingPerson;
            visitcar.CarNo = CarNo;
            visitcar.Dirver = Dirver;
            visitcar.ApplyDate = Convert.ToDateTime(ApplyDate);
            visitcar.DrivingLicenseUrl = DrivingLicenseUrl;
            visitcar.GPSID = "";
            visitcar.GPSNAME = "";
            visitcar.Note = "";
            visitcar.Phone = Phone;
            visitcar.Identitynum = Identitynum;
            visitcar.State = 1;
            visitcar.VisitDept = VisitDept;
            visitcar.VisitUser = VisitUser;
            visitcar.VisitUserPhone = VisitUserPhone;
            visitcar.CreateUserId = "System";
            visitcar.CreateDate = DateTime.Now;
            visitcar.CreateUserDeptCode = "00";
            visitcar.CreateUserOrgCode = "00";
            for (int i = 0; i < userjson.Count; i++)
            {
                string srcPath = Server.MapPath("~" + userjson[i].Userimg);
                if (System.IO.File.Exists(srcPath))
                {
                    //读图片转为Base64String
                    var base64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(srcPath));
                    userjson[i].Imgdata = base64;
                }
            }
            CarUserbll.SaveForm("", visitcar, userjson);
            SenSignalrdMsg();
        }


        /// <summary>
        /// 新增拜访车辆(危害品)
        /// </summary>
        /// <param name="Dirver"></param>
        /// <param name="CarNo"></param>
        /// <param name="VisitUser"></param>
        /// <param name="VisitUserPhone"></param>
        /// <param name="Phone"></param>
        /// <param name="AccompanyingNumber"></param>
        /// <param name="AccompanyingPerson"></param>
        /// <param name="VisitDept"></param>
        /// <param name="DriverLicenseUrl"></param>
        /// <param name="DrivingLicenseUrl"></param>
        public void SaveHazardous(string keyValue, string Dirver, string CarNo, string HazardousId, string HazardousName, string Phone,
            string AccompanyingNumber, string AccompanyingPerson, string DriverLicenseUrl,
            string DrivingLicenseUrl, string TheCompany, string ApplyDate, List<CarUserFileImgEntity> userjson)
        {
            try
            {
                HazardouscarEntity Hazardouscar = new HazardouscarEntity();
                Hazardouscar.ID = keyValue;
                Hazardouscar.AccompanyingNumber = Convert.ToInt32(AccompanyingNumber);
                //Hazardouscar.AccompanyingPerson = AccompanyingPerson;
                Hazardouscar.ApplyDate = Convert.ToDateTime(ApplyDate);
                Hazardouscar.CarNo = CarNo;
                Hazardouscar.Dirver = Dirver;
                Hazardouscar.DriverLicenseUrl = DriverLicenseUrl;
                Hazardouscar.DrivingLicenseUrl = DrivingLicenseUrl;
                Hazardouscar.GPSID = "";
                Hazardouscar.GPSNAME = "";
                Hazardouscar.Note = "";
                Hazardouscar.Phone = Phone;
                Hazardouscar.State = 1;
                Hazardouscar.HazardousId = HazardousId;
                Hazardouscar.HazardousName = HazardousName;
                Hazardouscar.CreateUserId = "System";
                Hazardouscar.CreateDate = DateTime.Now;
                Hazardouscar.CreateUserDeptCode = "00";
                Hazardouscar.CreateUserOrgCode = "00";
                Hazardouscar.TheCompany = TheCompany;
                Hazardouscar.DrivingRoute = "正常";
                Hazardouscar.DrivingSpeed = "正常";
                Hazardouscar.ResidenceTime = "正常";
                for (int i = 0; i < userjson.Count; i++)
                {//随行人员人脸图片
                    string srcPath = Server.MapPath("~" + userjson[i].Userimg);
                    if (System.IO.File.Exists(srcPath))
                    { //读图片转为Base64String
                        var base64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(srcPath));
                        userjson[i].Imgdata = base64;
                    }
                }
                Hazardouscarbll.SaveFaceUserForm("", Hazardouscar, userjson);
                //CarinfoBLL carinfobll = new CarinfoBLL();
                //var list = carinfobll.GetList("").Where(a => a.CarNo == Hazardouscar.CarNo).ToList();
                //if (list != null && list.Count == 0)
                //{//车辆信息同步到海康平台
                //    CarinfoEntity carin = new CarinfoEntity();
                //    carin.CarNo = Hazardouscar.CarNo;
                //    carin.Type = 5;
                //    carin.Dirver = Hazardouscar.Dirver;
                //    carin.Phone = Hazardouscar.Phone;
                //    carin.Deptname = Hazardouscar.TheCompany;
                //    carin.GpsId = Hazardouscar.GPSID;
                //    carin.GpsName = Hazardouscar.GPSNAME;
                //    carinfobll.SaveForm("", carin, "", "");
                //}
                SenSignalrdMsg();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 上传驾驶证/行驶证
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadDriverFile(string type)
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //没有文件上传，直接返回
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
            {
                return HttpNotFound();
            }
            string virtualPath = "";
            //string UserId = OperatorProvider.Provider.Current().UserId;

            DataItemDetailBLL dd = new DataItemDetailBLL();
            string path = dd.GetItemValue("imgPath") + "\\Resource\\" + type;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var file = files[0];
            string ext = System.IO.Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + ".png";
            file.SaveAs(path + "\\" + fileName);
            //if (GetPicThumbnail(path + "\\" + fileName, path + "\\s" + fileName, 40, 100, 20))
            //{
            //    virtualPath = "/Resource/" + type + "/s" + fileName;
            //}
            //else
            //{
            virtualPath = "/Resource/" + type + "/" + fileName;
            //}
            return Content(new AjaxResult { type = ResultType.success, message = "上传成功。", resultdata = virtualPath }.ToJson());

        }

        /// <summary>
        /// 上传拜访人员图片
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadUserFile(string type, int Num = 0)
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //没有文件上传，直接返回
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
            {
                return HttpNotFound();
            }
            string virtualPath = "";
            //string UserId = OperatorProvider.Provider.Current().UserId;

            DataItemDetailBLL dd = new DataItemDetailBLL();
            string path = dd.GetItemValue("imgPath") + "\\Resource\\" + type;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var file = files[0];
            string ext = System.IO.Path.GetExtension(file.FileName);
            if (ext.ToLower() == ".jpg" || Num == 1)
            {//人脸必须为jpg格式、身份证随意
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                file.SaveAs(path + "\\" + fileName);
                virtualPath = "/Resource/" + type + "/" + fileName;
                return Content(new AjaxResult { type = ResultType.success, message = "上传成功。", resultdata = virtualPath }.ToJson());
            }
            else
            {
                return Content(new AjaxResult { type = ResultType.success, message = "-1", resultdata = "仅支持jpg格式图片上传！" }.ToJson());
            }

        }


        /// <summary>
        /// 上传头像
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadOperticketPhoto()
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //没有文件上传，直接返回
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
            {
                return Content("");
            }
            string FileEextension = Path.GetExtension(files[0].FileName);
            //string UserId = OperatorProvider.Provider.Current().UserId;
            string virtualPath = string.Format("/Resource/PhotoFile/{0}{1}", Guid.NewGuid().ToString(), FileEextension);
            string fullFileName = Server.MapPath("~" + virtualPath);
            //创建文件夹，保存文件
            string path = Path.GetDirectoryName(fullFileName);
            Directory.CreateDirectory(path);
            files[0].SaveAs(fullFileName);
            return Content(new AjaxResult { type = ResultType.success, message = "上传成功", resultdata = virtualPath }.ToJson());
        }

        /// <summary>
        /// 通过拜访对象对象电话查询所在部门信息
        /// </summary>
        /// <param name="Tel"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetUserDeptInfo(string Tel)
        {
            UserInfoEntity entity = new UserInfoEntity();
            var deptBll = new ERCHTMS.Busines.BaseManage.DepartmentBLL();
            DataTable dt = deptBll.GetDataTable(string.Format("select d.DEPTNAME,d.REALNAME,d.MOBILE from V_USERINFO d where d.MOBILE='{0}'", Tel));
            if (dt.Rows.Count > 0)
            {
                entity.DeptName = dt.Rows[0][0].ToString();
                entity.RealName = dt.Rows[0][1].ToString();
                entity.Mobile = dt.Rows[0][2].ToString();
            }
            return entity.ToJson();
        }


        #endregion


        /// <summary>
        /// 华电毕节风险图
        /// </summary>
        /// <param name="areaCodes"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAreaStatus(string areaCodes, int mode = 1, string orgCode = "")
        {
            try
            {
                var deptBll = new ERCHTMS.Busines.BaseManage.DepartmentBLL();
                DataTable dt = new DataTable();
                dt.Columns.Add("code");
                dt.Columns.Add("status");
                dt.Columns.Add("htnum");
                dt.Columns.Add("fxnum");
                dt.Columns.Add("areacode");
                dt.Columns.Add("wxnum");
                dt.Columns.Add("content");
                dt.Columns.Add("name");
                StringBuilder sb = new StringBuilder();
                foreach (string code in areaCodes.Split(','))
                {
                    int val = 0;
                    string htNum = "";
                    string fxNum = "";
                    string areaCode = "";
                    string areaName = "";
                    DataTable obj = deptBll.GetDataTable(string.Format("select d.districtcode,districtname from bis_district d where d.description='{0}' and d.districtcode like '{1}%'", code, orgCode));
                    if (obj.Rows.Count > 0)
                    {
                        areaCode = obj.Rows[0][0].ToString();
                        areaName = obj.Rows[0][1].ToString();
                        string sql = "";
                        if (mode == 10)
                        {
                            sql = " and risktype in('管理','设备','区域')";
                        }
                        obj = deptBll.GetDataTable(string.Format("select min(gradeval) from BIS_RISKASSESS t where status=1 and deletemark=0 and t.areacode like '{0}%' {1}", areaCode, sql));
                        if (obj.Rows.Count > 0)
                        {
                            if (obj.Rows[0][0] != DBNull.Value)
                            {
                                val = int.Parse(obj.Rows[0][0].ToString());
                            }
                            else
                            {
                                val = 0;
                            }
                        }
                        else
                        {
                            val = 0;
                        }
                        obj = deptBll.GetDataTable(string.Format("select min(t.risktype) from v_xssunderwaywork t where  workareacode='{0}'", areaCode));
                        if (obj.Rows.Count > 0)
                        {
                            if (obj.Rows[0][0] != DBNull.Value)
                            {
                                if (!string.IsNullOrEmpty(obj.Rows[0][0].ToString()))
                                {
                                    int lev = int.Parse(obj.Rows[0][0].ToString()) + 1;
                                    if (lev < val)
                                    {
                                        val = lev;
                                    }
                                }

                            }
                        }
                        if (mode == 1 || mode == 10)
                        {
                            //隐患数量
                            DataTable dtHt = deptBll.GetDataTable(string.Format("select rankname,count(1) num from v_basehiddeninfo t where t.workstream!='整改结束' and t.hidpoint like '{0}%' group by rankname", areaCode));
                            if (dtHt.Rows.Count > 0)
                            {
                                var rows = dtHt.Select("rankname='一般隐患'");
                                if (rows.Length > 0)
                                {
                                    htNum = rows[0][1].ToString();
                                }
                                else
                                {
                                    htNum = "0";
                                }
                                rows = dtHt.Select("rankname='重大隐患'");
                                if (rows.Length > 0)
                                {
                                    htNum += "," + rows[0][1].ToString();
                                }
                                else
                                {
                                    htNum += ",0";
                                }
                            }
                            sb.Clear();
                            //风险数量
                            DataTable dtRisk = deptBll.GetDataTable(string.Format(@"select nvl(num,0) from (select 1 gradeval from dual union all select 2 gradeval from dual union all select 3 gradeval from dual union all select 4 gradeval from dual) a
left join (select gradeval,count(1) num from BIS_RISKASSESS t where status=1 and deletemark=0 and t.areacode like '{0}%' {1} group by grade,gradeval) b
on a.gradeval=b.gradeval order by a.gradeval asc", areaCode, sql));
                            foreach (DataRow dr in dtRisk.Rows)
                            {
                                sb.AppendFormat("{0},", dr[0].ToString());
                            }
                            //重大危险源数量
                            obj = deptBll.GetDataTable(string.Format("select count(1) from HSD_HAZARDSOURCE t where IsDanger=1 and gradeval>0 and deptcode like '{0}%' and t.districtid in(select districtid from bis_district d where d.districtcode like '{1}%')", orgCode, areaCode));
                            int count = 0;
                            if (obj.Rows.Count > 0)
                            {
                                count = int.Parse(obj.Rows[0][0].ToString());
                            }
                            fxNum = sb.ToString().TrimEnd(',');
                            DataRow row = dt.NewRow();
                            row[0] = code;
                            row[1] = val;
                            row[2] = htNum;
                            row[3] = fxNum;
                            row[4] = areaCode;
                            row[5] = count;
                            row["name"] = areaName;
                            dt.Rows.Add(row);

                            //                            sql = string.Format(@"select min(nvl(t.risktype,5))
                            //                                      from v_xssunderwaywork t
                            //                                      left join base_dataitemdetail b
                            //                                        on t.risktype = b.itemvalue
                            //                                       and b.itemid =
                            //                                           (select itemid from base_dataitem where itemcode = 'CommonRiskType')
                            //                                     where  workareaname='{0}'", areaName);
                            //                            obj = deptBll.GetDataTable(sql);
                            //                            if (obj.Rows.Count>0)
                            //                            {
                            //                                if (obj.Rows[0][0]!=DBNull.Value)
                            //                                {
                            //                                    if (obj.Rows[0][0].ToString().Length>0)
                            //                                    {
                            //                                        count = int.Parse(obj.Rows[0][0].ToString()) + 1;
                            //                                        if (val > count)
                            //                                        {
                            //                                            row[1] = count;
                            //                                        }
                            //                                    }

                            //                                }

                            //                            }

                        }
                    }
                }
                return Success("获取数据成功", dt);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { type = ResultType.success, message = message, resultdata = data }.ToJson());
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { type = ResultType.error, message = message }.ToJson());
        }

    }
}
