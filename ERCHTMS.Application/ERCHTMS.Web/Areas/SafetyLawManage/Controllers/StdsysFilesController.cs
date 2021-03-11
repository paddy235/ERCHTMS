using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.NosaManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SafetyLawManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERCHTMS.Entity.BaseManage;
using Aspose.Words;
using Aspose.Cells;
using System.Diagnostics;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// 描 述：标准制度文件
    /// </summary>
    public class StdsysFilesController : MvcControllerBase
    {
        private StdsysFilesBLL stdsysfilesbll = new StdsysFilesBLL();
        private DepartmentBLL deptBll = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        #region 视图功能   
        /// <summary>
        /// 收藏列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MyStoreIndex()
        {
            return View();
        }
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
        /// <summary>
        /// 详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }

        /// <summary>
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
        /// 标准制度管理查看用
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowDetail()
        {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 是否存在相同编号的元素
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <param name="fileName">编号</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult ExistSameFile(string keyValue, string fileName)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var oldList = stdsysfilesbll.GetList(String.Format(" and createuserorgcode='{0}' and filename='{1}' and id<>'{2}'", user.OrganizeCode, fileName, keyValue)).ToList();
            var r = oldList.Count > 0;

            return Success("存在同名文件，请校正。", r);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = stdsysfilesbll.GetList(pagination, queryJson);
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
            var data = stdsysfilesbll.GetEntity(keyValue);          
            //返回值
            var josnData = new
            {
                data               
            };

            return Content(josnData.ToJson());
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
            stdsysfilesbll.RemoveForm(keyValue);
            DeleteFiles(keyValue);
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
        public ActionResult SaveForm(string keyValue, StdsysFilesEntity entity)
        {
            stdsysfilesbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "数据导出")]
        public ActionResult Export(string queryJson, string sortname, string sortorder)
        {
            var pagination = new Pagination()
            {
                page = 1,
                rows = 100000,
                sidx = string.IsNullOrWhiteSpace(sortname) ? "createdate" : sortname,
                sord = string.IsNullOrWhiteSpace(sortorder) ? "asc" : sortorder
            };
            var dt = stdsysfilesbll.GetList(pagination, queryJson);
            string fileUrl = @"\Resource\ExcelTemplate\标准制度_导出模板.xlsx";
            AsposeExcelHelper.ExecuteResult(dt, fileUrl, "标准制度", string.Format("标准制度_{0}",DateTime.Now.ToString("yyyyMMddHHmmss")));

            return Success("导出成功。");
        }
        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="ids">id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult SetStore(string ids)
        {
            if (!string.IsNullOrWhiteSpace(ids))
            {
                StdsysFilesStoreBLL stdsysfsbll = new StdsysFilesStoreBLL();
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var list = ids.Split(',');
                foreach(var id in list)
                {
                    var entity = stdsysfsbll.GetList(string.Format(" and userid='{0}' and stdsysid='{1}'", user.UserId, id)).FirstOrDefault();
                    if (entity == null)
                    {
                        entity = new StdsysFilesStoreEntity()
                        {
                            UserId = user.UserId,
                            StdSysId = id
                        };
                        stdsysfsbll.SaveForm("", entity);
                    }
                }
                return Success("收藏成功。");
            }
            else
            {
                return Error("收藏失败。");
            }
        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="ids">id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult RemoveStore(string ids)
        {
            if (!string.IsNullOrWhiteSpace(ids))
            {
                StdsysFilesStoreBLL stdsysfsbll = new StdsysFilesStoreBLL();
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var list = stdsysfsbll.GetList(string.Format(" and userid='{0}' and stdsysid in ('{1}')", user.UserId, ids.Replace(",", "','"))).ToList();
                stdsysfsbll.RemoveForm(list);                    
                return Success("取消成功。");
            }
            else
            {
                return Error("取消失败。");
            }
        }



        /// <summary>
        /// 标准导入
        /// </summary>
        /// <param name="standardtype"></param>
        /// <param name="categorycode"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportStandard(string refid, string refname, string deptcode)
        {
            try
            {
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    return "超级管理员无此操作权限";
                }
                string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司
                int error = 0;
                int success = 0;
                string message = "请选择文件格式正确的文件再导入!";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    if (HttpContext.Request.Files.Count != 2)
                    {
                        return "请按正确的方式导入两个文件.";
                    }
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    HttpPostedFileBase file2 = HttpContext.Request.Files[1];
                    if (string.IsNullOrEmpty(file.FileName) || string.IsNullOrEmpty(file2.FileName))
                    {
                        return message;
                    }
                    Boolean isZip1 = file.FileName.Substring(file.FileName.IndexOf('.')).Contains("zip");//第一个文件是否为Zip格式
                    Boolean isZip2 = file2.FileName.Substring(file2.FileName.IndexOf('.')).Contains("zip");//第二个文件是否为Zip格式
                    if ((isZip1 || isZip2) == false || (isZip1 && isZip2) == true)
                    {
                        return message;
                    }
                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                    string fileName2 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file2.FileName);
                    file2.SaveAs(Server.MapPath("~/Resource/temp/" + fileName2));
                    string decompressionDirectory = Server.MapPath("~/Resource/decompression/") + DateTime.Now.ToString("yyyyMMddhhmmssfff") + "\\";
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    if (isZip1)
                    {
                        UnZip(Server.MapPath("~/Resource/temp/" + fileName1), decompressionDirectory, "", true);
                        wb.Open(Server.MapPath("~/Resource/temp/" + fileName2));
                    }
                    else
                    {
                        UnZip(Server.MapPath("~/Resource/temp/" + fileName2), decompressionDirectory, "", true);
                        wb.Open(Server.MapPath("~/Resource/temp/" + fileName1));
                    }

                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        StdsysFilesEntity standard = new StdsysFilesEntity();
                        standard.ID = Guid.NewGuid().ToString();

                        //文件名称
                        string filename = dt.Rows[i][0].ToString();
                        //文件编号
                        string fileno = dt.Rows[i][1].ToString();



                        //---****值存在空验证*****--
                        if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(dt.Rows[i][2].ToString()))
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行值存在空,未能导入.";
                            error++;
                            continue;
                        }

                        bool conbool = false;


                        //文件路径
                        string[] filepaths = dt.Rows[i][2].ToString().Split(';');

                        var filepath = "";
                        for (int j = 0; j < filepaths.Length; j++)
                        {
                            filepath = filepaths[j];

                            if (string.IsNullOrEmpty(filepath))
                            {
                                continue;
                            }

                            //---****文件格式验证*****--
                            if (!(filepath.Substring(filepath.IndexOf('.')).Contains("doc") || filepath.Substring(filepath.IndexOf('.')).Contains("docx") || filepath.Substring(filepath.IndexOf('.')).Contains("pdf")))
                            {
                                falseMessage += "</br>" + "第" + (i + 1) + "行指定附件格式不正确,未能导入.";
                                error++;
                                conbool = true;
                                continue;
                            }

                            //---****文件是否存在验证*****--
                            if (!System.IO.File.Exists(decompressionDirectory + filepath))
                            {
                                falseMessage += "</br>" + "第" + (i + 1) + "行指定附件不存在,未能导入.";
                                error++;
                                conbool = true;
                                continue;
                            }
                            var fileinfo = new FileInfo(decompressionDirectory + filepath);
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            string fileguid = Guid.NewGuid().ToString();
                            fileInfoEntity.Create();
                            fileInfoEntity.RecId = standard.ID; //关联ID
                            fileInfoEntity.FileName = filepath;
                            fileInfoEntity.FilePath = "~/Resource/StandardSystem/" + fileguid + fileinfo.Extension;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                            fileInfoEntity.FileExtensions = fileinfo.Extension;
                            fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                            TransportRemoteToServer(Server.MapPath("~/Resource/StandardSystem/"), decompressionDirectory + filepath, fileguid + fileinfo.Extension);
                            fileinfobll.SaveForm("", fileInfoEntity);

                        }

                        if (conbool)
                        {
                            continue;
                        }

                        standard.FileName = filename;
                        standard.FileNo = fileno;
                        standard.RefId = refid;
                        standard.RefName = refname;
                        DepartmentEntity deptEntity = deptBll.GetEntityByCode(deptcode);
                        if (deptEntity != null)
                        {
                            standard.PubDepartId = deptEntity.DepartmentId;
                            standard.PubDepartName = deptEntity.FullName;
                        }
                        else
                        {
                            standard.PubDepartId = OperatorProvider.Provider.Current().DeptId;
                            standard.PubDepartName = OperatorProvider.Provider.Current().DeptName;
                        }


                        if (!string.IsNullOrEmpty(dt.Rows[i][3].ToString()))
                        {
                            standard.PubDate = Convert.ToDateTime(dt.Rows[i][3].ToString());
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                        {
                            standard.ReviseDate = Convert.ToDateTime(dt.Rows[i][4].ToString());
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i][5].ToString()))
                        {
                            standard.UseDate = Convert.ToDateTime(dt.Rows[i][5].ToString());
                        }
                        standard.Remark = !string.IsNullOrEmpty(dt.Rows[i][6].ToString()) ? dt.Rows[i][6].ToString() : "";

                        try
                        {
                            stdsysfilesbll.SaveForm(standard.ID, standard);
                            success++;
                        }
                        catch
                        {
                            error++;
                        }
                    }
                    message = "共有" + dt.Rows.Count + "条记录,成功导入" + success + "条，失败" + error + "条";
                    message += "</br>" + falseMessage;
                }
                return message;
            }
            catch (Exception e)
            {
                return "导入的Excel数据格式不正确，请下载标准模板重新填写！";
            }
           
        }

        /// <summary>
        /// 解压zip文件
        /// </summary>
        /// <param name="zipedFile"></param>
        /// <param name="strDirectory"></param>
        /// <param name="password"></param>
        /// <param name="overWrite"></param>
        public void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {
            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();

            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(System.IO.File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);
                    if (fileName != "")
                    {
                        if ((System.IO.File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!System.IO.File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = System.IO.File.Create(strDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                s.Close();
            }
        }

        /// <summary>  
        /// 
        /// </summary>  
        /// <param name="src">远程服务器路径（共享文件夹路径）</param>  
        /// <param name="dst">本地文件夹路径</param>  
        /// <param name="filename"></param> 
        public static void TransportRemoteToServer(string src, string dst, string filename)
        {
            if (!Directory.Exists(src))
            {
                Directory.CreateDirectory(src);
            }
            FileStream inFileStream = new FileStream(src + filename, FileMode.OpenOrCreate);

            FileStream outFileStream = new FileStream(dst, FileMode.Open);

            byte[] buf = new byte[outFileStream.Length];

            int byteCount;

            while ((byteCount = outFileStream.Read(buf, 0, buf.Length)) > 0)
            {
                inFileStream.Write(buf, 0, byteCount);

            }

            inFileStream.Flush();

            inFileStream.Close();

            outFileStream.Flush();

            outFileStream.Close();

        }

        #endregion

        /// <summary>
        /// 处理文件 word excel转成Pdf在线预览
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public ActionResult ShowActivityFile(string fileId)
        {
            try
            {

                if (Debugger.IsAttached)
                {
                    Office2PDFHelper.DOCConvertToPDF(Server.MapPath("~/Resource/StdsysFilesPDF/111.doc"), Server.MapPath("~/Resource/StdsysFilesPDF/word.pdf"));
                    Office2PDFHelper.XLSConvertToPDF(Server.MapPath("~/Resource/StdsysFilesPDF/111.xlsx"), Server.MapPath("~/Resource/StdsysFilesPDF/xls.pdf"));
                }


                FileInfoEntity file = new FileInfoBLL().GetEntity(fileId);
                string pathDic = "~/Resource/StdsysFilesPDF/";
                if (!Directory.Exists(Server.MapPath(pathDic))) Directory.CreateDirectory(Server.MapPath(pathDic));
                if (file != null)
                {
                    string fileUrl = file.FilePath;
                    bool suceess = true;
                    switch (file.FileType.ToLower())
                    {
                        case "doc":
                        case "docx":
                            //Aspose.cell版本过低 EXCEL转化会打不开
                            //case "xls":
                            //case "xlsx":
                            var filename = file.FileName.Substring(0, file.FileName.LastIndexOf(".")) + ".pdf";
                            if (!System.IO.File.Exists(Server.MapPath(pathDic + file.FileId + "_" + filename)))
                            {
                                string savePath = pathDic + file.FileId + "_" + filename;
                                fileUrl = savePath;
                                switch (file.FileType.ToLower())
                                {
                                    case "doc":
                                    case "docx":
                                        suceess = Office2PDFHelper.DOCConvertToPDF(Server.MapPath(file.FilePath), Server.MapPath(savePath));
                                        break;
                                    case "xls":
                                    case "xlsx":
                                        suceess = Office2PDFHelper.XLSConvertToPDF(Server.MapPath(file.FilePath), Server.MapPath(savePath));
                                        break;
                                }
                            }
                            else
                            {
                                fileUrl = pathDic + file.FileId + "_" + filename;
                            }
                            break;
                        default:
                            break;
                    }
                    if (suceess)
                    {
                        return Success("获取文件成功！", new { FileUrl = Request.Url.Host+ Url.Content(fileUrl), FileType = file.FileType });
                    }
                    else
                    {
                        return Success("获取文件成功！", new { FileUrl = Request.Url.Host + Url.Content(file.FilePath), FileType = file.FileType });
                    }
                }
                else
                {
                    return Error("未找到指定的文件");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
    /// <summary>
    /// Office2Pdf 将Office文档转化为pdf
    /// </summary>
    public class Office2PDFHelper
    {
        /// <summary>
        /// Word转换成pdf
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        public static bool DOCConvertToPDF(string sourcePath, string targetPath)
        {
            try
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(sourcePath);
                doc.Save(targetPath, Aspose.Words.SaveFormat.Pdf);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// 把Excel文件转换成PDF格式文件  
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        public static bool XLSConvertToPDF(string sourcePath, string targetPath)
        {
            try
            {
                var book = new Workbook();
                book.Open(sourcePath);
                book.Save(targetPath, FileFormatType.AsposePdf);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
