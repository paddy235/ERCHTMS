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
    /// �� ������׼�ƶ��ļ�
    /// </summary>
    public class StdsysFilesController : MvcControllerBase
    {
        private StdsysFilesBLL stdsysfilesbll = new StdsysFilesBLL();
        private DepartmentBLL deptBll = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        #region ��ͼ����   
        /// <summary>
        /// �ղ��б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MyStoreIndex()
        {
            return View();
        }
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }

        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
        /// ��׼�ƶȹ���鿴��
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowDetail()
        {
            return View();
        }

        #endregion

        #region ��ȡ����
        /// <summary>
        /// �Ƿ������ͬ��ŵ�Ԫ��
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <param name="fileName">���</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult ExistSameFile(string keyValue, string fileName)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var oldList = stdsysfilesbll.GetList(String.Format(" and createuserorgcode='{0}' and filename='{1}' and id<>'{2}'", user.OrganizeCode, fileName, keyValue)).ToList();
            var r = oldList.Count > 0;

            return Success("����ͬ���ļ�����У����", r);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = stdsysfilesbll.GetEntity(keyValue);          
            //����ֵ
            var josnData = new
            {
                data               
            };

            return Content(josnData.ToJson());
        }        
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            stdsysfilesbll.RemoveForm(keyValue);
            DeleteFiles(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, StdsysFilesEntity entity)
        {
            stdsysfilesbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "���ݵ���")]
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
            string fileUrl = @"\Resource\ExcelTemplate\��׼�ƶ�_����ģ��.xlsx";
            AsposeExcelHelper.ExecuteResult(dt, fileUrl, "��׼�ƶ�", string.Format("��׼�ƶ�_{0}",DateTime.Now.ToString("yyyyMMddHHmmss")));

            return Success("�����ɹ���");
        }
        /// <summary>
        /// �ղ�
        /// </summary>
        /// <param name="ids">id</param>
        /// <returns>�����б�Json</returns>
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
                return Success("�ղسɹ���");
            }
            else
            {
                return Error("�ղ�ʧ�ܡ�");
            }
        }
        /// <summary>
        /// ȡ���ղ�
        /// </summary>
        /// <param name="ids">id</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult RemoveStore(string ids)
        {
            if (!string.IsNullOrWhiteSpace(ids))
            {
                StdsysFilesStoreBLL stdsysfsbll = new StdsysFilesStoreBLL();
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var list = stdsysfsbll.GetList(string.Format(" and userid='{0}' and stdsysid in ('{1}')", user.UserId, ids.Replace(",", "','"))).ToList();
                stdsysfsbll.RemoveForm(list);                    
                return Success("ȡ���ɹ���");
            }
            else
            {
                return Error("ȡ��ʧ�ܡ�");
            }
        }



        /// <summary>
        /// ��׼����
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
                    return "��������Ա�޴˲���Ȩ��";
                }
                string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾
                int error = 0;
                int success = 0;
                string message = "��ѡ���ļ���ʽ��ȷ���ļ��ٵ���!";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    if (HttpContext.Request.Files.Count != 2)
                    {
                        return "�밴��ȷ�ķ�ʽ���������ļ�.";
                    }
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    HttpPostedFileBase file2 = HttpContext.Request.Files[1];
                    if (string.IsNullOrEmpty(file.FileName) || string.IsNullOrEmpty(file2.FileName))
                    {
                        return message;
                    }
                    Boolean isZip1 = file.FileName.Substring(file.FileName.IndexOf('.')).Contains("zip");//��һ���ļ��Ƿ�ΪZip��ʽ
                    Boolean isZip2 = file2.FileName.Substring(file2.FileName.IndexOf('.')).Contains("zip");//�ڶ����ļ��Ƿ�ΪZip��ʽ
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

                        //�ļ�����
                        string filename = dt.Rows[i][0].ToString();
                        //�ļ����
                        string fileno = dt.Rows[i][1].ToString();



                        //---****ֵ���ڿ���֤*****--
                        if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(dt.Rows[i][2].ToString()))
                        {
                            falseMessage += "</br>" + "��" + (i + 1) + "��ֵ���ڿ�,δ�ܵ���.";
                            error++;
                            continue;
                        }

                        bool conbool = false;


                        //�ļ�·��
                        string[] filepaths = dt.Rows[i][2].ToString().Split(';');

                        var filepath = "";
                        for (int j = 0; j < filepaths.Length; j++)
                        {
                            filepath = filepaths[j];

                            if (string.IsNullOrEmpty(filepath))
                            {
                                continue;
                            }

                            //---****�ļ���ʽ��֤*****--
                            if (!(filepath.Substring(filepath.IndexOf('.')).Contains("doc") || filepath.Substring(filepath.IndexOf('.')).Contains("docx") || filepath.Substring(filepath.IndexOf('.')).Contains("pdf")))
                            {
                                falseMessage += "</br>" + "��" + (i + 1) + "��ָ��������ʽ����ȷ,δ�ܵ���.";
                                error++;
                                conbool = true;
                                continue;
                            }

                            //---****�ļ��Ƿ������֤*****--
                            if (!System.IO.File.Exists(decompressionDirectory + filepath))
                            {
                                falseMessage += "</br>" + "��" + (i + 1) + "��ָ������������,δ�ܵ���.";
                                error++;
                                conbool = true;
                                continue;
                            }
                            var fileinfo = new FileInfo(decompressionDirectory + filepath);
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            string fileguid = Guid.NewGuid().ToString();
                            fileInfoEntity.Create();
                            fileInfoEntity.RecId = standard.ID; //����ID
                            fileInfoEntity.FileName = filepath;
                            fileInfoEntity.FilePath = "~/Resource/StandardSystem/" + fileguid + fileinfo.Extension;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
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
                    message = "����" + dt.Rows.Count + "����¼,�ɹ�����" + success + "����ʧ��" + error + "��";
                    message += "</br>" + falseMessage;
                }
                return message;
            }
            catch (Exception e)
            {
                return "�����Excel���ݸ�ʽ����ȷ�������ر�׼ģ��������д��";
            }
           
        }

        /// <summary>
        /// ��ѹzip�ļ�
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
        /// <param name="src">Զ�̷�����·���������ļ���·����</param>  
        /// <param name="dst">�����ļ���·��</param>  
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
        /// �����ļ� word excelת��Pdf����Ԥ��
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
                            //Aspose.cell�汾���� EXCELת����򲻿�
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
                        return Success("��ȡ�ļ��ɹ���", new { FileUrl = Request.Url.Host+ Url.Content(fileUrl), FileType = file.FileType });
                    }
                    else
                    {
                        return Success("��ȡ�ļ��ɹ���", new { FileUrl = Request.Url.Host + Url.Content(file.FilePath), FileType = file.FileType });
                    }
                }
                else
                {
                    return Error("δ�ҵ�ָ�����ļ�");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
    /// <summary>
    /// Office2Pdf ��Office�ĵ�ת��Ϊpdf
    /// </summary>
    public class Office2PDFHelper
    {
        /// <summary>
        /// Wordת����pdf
        /// </summary>
        /// <param name="sourcePath">Դ�ļ�·��</param>
        /// <param name="targetPath">Ŀ���ļ�·��</param>
        /// <returns>true=ת���ɹ�</returns>
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
        /// ��Excel�ļ�ת����PDF��ʽ�ļ�  
        /// </summary>
        /// <param name="sourcePath">Դ�ļ�·��</param>
        /// <param name="targetPath">Ŀ���ļ�·��</param>
        /// <returns>true=ת���ɹ�</returns>
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
