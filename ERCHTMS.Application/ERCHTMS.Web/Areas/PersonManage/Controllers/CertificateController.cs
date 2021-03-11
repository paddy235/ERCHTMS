using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Linq;
using System;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using Aspose.Cells;
using System.Data;
using ERCHTMS.Busines.PublicInfoManage;
using System.Web;
using ERCHTMS.Entity.PublicInfoManage;
namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public class CertificateController : MvcControllerBase
    {
        private CertificateBLL certificatebll = new CertificateBLL();
        UserBLL userBll = new UserBLL();
        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AuditForm()
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
        [HttpGet]
        public ActionResult NewForm()
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
        /// ������ҵ�������豸֤������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewImport()
        {
            return View();
        }
        /// <summary>
        /// �鿴֤����Ƭ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewImage()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string userId, Pagination pag, string certType = "")
        {
            var data = certificatebll.GetList(userId, pag);
            if (!string.IsNullOrWhiteSpace(certType))
            {
                data = data.Where(t => t.CertType == certType);
            }
            DepartmentBLL deptBll = new DepartmentBLL();
            foreach (CertificateEntity dr in data)
            {
                //�ж�֤��������Ƭ
                int count = deptBll.GetDataTable(string.Format("select count(1) from base_fileinfo where recid='{0}'", dr.Id)).Rows[0][0].ToInt();
                if (count > 0)
                {
                    dr.FilePath = "1";
                }
            }
            return ToJsonResult(data);
        }
        /// <summary>
        ///��ȡ֤�鸴���¼
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAuditListJson(string certId)
        {
            var data = certificatebll.GetAuditList(certId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = certificatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAuditFormJson(string keyValue)
        {
            var data = certificatebll.GetAuditEntity(keyValue);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetItemListJson(string type, string code)
        {
            var rows = new DepartmentBLL().GetDataTable(string.Format("select t.ItemName,t.ItemValue,t.itemcode,Description from BASE_DATAITEMDETAIL t where itemcode='{1}' and t.enabledmark = 1 and t.deletemark = 0 and  t.itemid=(select itemid from base_dataitem a where a.itemcode='{0}') order by sortcode", type, code));
            return ToJsonResult(rows);
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
            certificatebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ɾ�������¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveAuditForm(string keyValue)
        {
            certificatebll.RemoveCertAudit(keyValue);
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
        public ActionResult SaveForm(string keyValue, CertificateEntity entity)
        {
            try
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}' and id!='{1}'", entity.CertNum, keyValue)).Rows[0][0].ToString();
                //if (number != "0")
                //{
                //    return Error("֤�����Ѵ��ڣ�");
                //}
                //else
                //{
                certificatebll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
                //}

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// ���渴���¼
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveAuditForm(CertAuditEntity entity)
        {
            try
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                bool result = certificatebll.SaveCertAudit(entity);
                if (result)
                {
                    entity = certificatebll.GetAuditEntity(entity.Id);
                    CertificateEntity cert = certificatebll.GetEntity(entity.CertId);
                    if (cert != null)
                    {
                        cert.SendOrgan = entity.SendOrgan;
                        if (entity.AuditType == "���ڻ�֤")
                        {
                            cert.StartDate = entity.AuditDate;
                            cert.ApplyDate = entity.NextDate;
                            cert.EndDate = entity.EndDate;
                        }
                        if (entity.AuditType == "����")
                        {
                            if (entity.Result == "�ϸ�")
                            {
                                cert.Status = 1;
                            }
                            else
                            {
                                cert.Status = 0;
                            }
                        }
                        if (cert.CertType == "�����豸��ҵ��Ա֤")
                        {
                            cert.EndDate = entity.EndDate;
                        }
                        if (certificatebll.SaveForm(entity.CertId, cert))
                        {
                            if (entity.AuditType == "���ڻ�֤")
                            {
                                int rad = new Random().Next(0, 1000000);
                                deptBll.ExecuteSql(string.Format("begin \r\n delete from base_fileinfo where recid='{2}';\r\n insert into base_fileinfo(fileid,folderid,filename,filepath,filesize,fileextensions,filetype,deletemark,enabledmark,recid) select fileid || '{0}',folderid,filename,filepath,filesize,fileextensions,filetype,0,1,'{2}' from base_fileinfo where recid='{1}';\r\n end \r\n commit;", rad, entity.Id, cert.Id));
                            }

                        }
                    }
                }
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// ��ѹzip�ļ�
        /// </summary>
        /// <param name="zipedFile"></param>
        /// <param name="strDirectory"></param>
        /// <param name="password"></param>
        /// <param name="overWrite"></param>
        public bool UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {
            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();

            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";
            try
            {
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
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// ����֤��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportCert()
        {
            try
            {
                int mode = 1;
                int success = 0;
                string message = "��ѡ����ȷ��Zip�ļ���";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    string zipName = Path.GetFileNameWithoutExtension(file.FileName);

                    string dirName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string path = Server.MapPath("~/Resource/temp/" + dirName + Path.GetExtension(file.FileName));
                    string destPath = Server.MapPath("~/Resource/cert/" + dirName);
                    string dir = "֤����Ƭ";
                    file.SaveAs(path);
                    if (System.IO.File.Exists(path))
                    {
                        if (UnZip(path, destPath, "", true))
                        {
                            FileInfo fi = new DirectoryInfo(destPath + "\\" + zipName).GetFiles("*.*").Where(t => t.Name.ToLower().EndsWith(".xls") || t.Name.ToLower().EndsWith(".xlxs")).FirstOrDefault();
                            if (fi == null)
                            {
                                message = "ѹ������û�м�⵽excel�ļ���";
                            }
                            else
                            {
                                message = "";
                                DirectoryInfo dirs = new DirectoryInfo(destPath + "\\" + zipName).GetDirectories().FirstOrDefault();
                                if (dirs != null)
                                {
                                    dir = dirs.Name;
                                }
                                List<int> lstErrors = new List<int>();
                                string fileName = fi.Name;
                                Workbook wb = new Aspose.Cells.Workbook();
                                wb.Open(destPath + "\\" + zipName + "\\" + fileName);
                                //����������ҵ��Ա֤��
                                DepartmentBLL deptBll = new DepartmentBLL();
                                FileInfoBLL fileinfobll = new FileInfoBLL();
                                string certType = "������ҵ����֤";
                                mode = 1;
                                string kind = "ryzylb";
                                string type = "ryzyxm";
                                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                                DataTable dt = new DataTable();
                                if (cells.MaxDataRow > 1)
                                {
                                    message = "��ʼ����������ҵ��Ա֤��,��Ϣ���£�<br />";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                  
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //����
                                        string deptName = dt.Rows[i]["��λ/����"].ToString().Trim();

                                        //����
                                        string userName = dt.Rows[i]["����"].ToString().Trim();

                                        //����
                                        string workType = mode == 1 ? dt.Rows[i]["��ҵ���"].ToString().Trim() : dt.Rows[i]["����"].ToString().Trim();
                                        //��Ŀ
                                        string workItem = mode == 1 ? dt.Rows[i]["������Ŀ"].ToString().Trim() : dt.Rows[i]["��ҵ��Ŀ"].ToString().Trim();
                                        //��֤����
                                        string sendOrg = dt.Rows[i]["��֤����"].ToString().Trim();
                                        //��֤����
                                        string sendDate = dt.Rows[i]["��������"].ToString().Trim();
                                        //��ʼ����
                                        string startDate = dt.Rows[i]["��Ч�ڿ�ʼ����"].ToString().Trim();
                                        //��������
                                        string endDate = dt.Rows[i]["��Ч�ڽ�������"].ToString().Trim();
                                        //��������
                                        string applyDate = dt.Rows[i]["Ӧ��������"].ToString().Trim();
                                        //֤����
                                        string certNum = dt.Rows[i]["֤����"].ToString().Trim();
                                        //�ֻ���
                                        string mobile = dt.Rows[i]["�ֻ���"].ToString().Trim();
                                        //֤����Ƭ
                                        string photos = dt.Rows[i]["֤����Ƭ"].ToString().Trim().Trim(',');
                                        //��Ч��(��)
                                        string years = dt.Rows[i]["��Ч��(��)"].ToString().Trim();
                                        //֤������
                                        string certName = string.Format("{0}-{1}-{2}", certType, workType, workItem);
                                        //��Ŀ����
                                        string code = "";
                                        bool isOk = true;
                                        if (mode == 2)
                                        {
                                            if (dt.Columns.Contains("��Ŀ����"))
                                            {
                                                code = dt.Rows[i]["��Ŀ����"].ToString().Trim();
                                            }
                                            else
                                            {
                                                falseMessage = "ģ�岻��ȷ��ȱ�١���Ŀ���š���";
                                                isOk = false;
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                continue;
                                            }
                                        }

                                        //---****ֵ���ڿ���֤*****--
                                        if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(workType) || string.IsNullOrEmpty(workItem) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName) || string.IsNullOrEmpty(applyDate))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        int number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzylb')", workType)).Rows[0][0].ToInt();
                                        if (number == 0)
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "����ҵ�����д����ȷ,δ�ܵ���.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and itemcode='{1}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzyxm')", workItem, workType)).Rows[0][0].ToInt();
                                        if (number == 0)
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "�в�����Ŀ��д����ȷ,δ�ܵ���.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        //--�ֻ�����֤
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "���ֻ��Ÿ�ʽ����,δ�ܵ���.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToInt();
                                            //if (number >0)
                                            //{
                                            //    falseMessage += "</br>" + "��" + (i + 3) + "��֤����ţ�" + certNum + "���Ѵ��� ,δ�ܵ���.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.WorkType = workType;
                                                    cert.WorkItem = workItem;
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.ItemNum = code;
                                                    cert.StartDate = DateTime.Parse(startDate);
                                                    cert.ApplyDate = DateTime.Parse(applyDate);
                                                    cert.CertName = certName;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "��" + (i + 3) + "��֤����Ƭ��" + str + "��������!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "��" + (i + 3) + "��ϵͳ�����ڸ���Ա" + userName + "��" + deptName + ")��Ϣ��";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;
                                    message += "����" + count + "����¼,�ɹ�����" + success + "����ʧ��" + lstErrors.Count + "��.";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "������Ϣ���£�" + falseMessage;
                                    }
                                }

                                //���������豸��Ա֤��
                                success = 0;
                                falseMessage = "";
                                lstErrors.Clear();
                                cells = wb.Worksheets[1].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow>1)
                                {
                                    message += "</br></br>��ʼ���������豸��Ա֤��,��Ϣ���£�</br>";
                                    mode = 2;
                                    certType = "�����豸��ҵ��Ա֤";
                                    kind = "tzzlb";
                                    type = "tzsbxm";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //����
                                        string deptName = dt.Rows[i]["��λ/����"].ToString().Trim();
                                        //����
                                        string userName = dt.Rows[i]["����"].ToString().Trim();
                                        //����
                                        string workType = mode == 1 ? dt.Rows[i]["��ҵ���"].ToString().Trim() : dt.Rows[i]["����"].ToString().Trim();
                                        //��Ŀ
                                        string workItem = mode == 1 ? dt.Rows[i]["������Ŀ"].ToString().Trim() : dt.Rows[i]["��ҵ��Ŀ"].ToString().Trim();
                                        //��֤����
                                        string sendOrg = dt.Rows[i]["��֤����"].ToString().Trim();
                                        //��֤����
                                        string sendDate = dt.Rows[i]["��������"].ToString().Trim();
                                        //��Ч��
                                        string endDate = dt.Rows[i]["��Ч����"].ToString().Trim();
                                        //��������
                                        string applyDate = "";
                                        //֤����
                                        string certNum = dt.Rows[i]["֤����"].ToString().Trim();
                                        //�ֻ���
                                        string mobile = dt.Rows[i]["�ֻ���"].ToString().Trim();
                                        //֤����Ƭ
                                        string photos = dt.Rows[i]["֤����Ƭ"].ToString().Trim().Trim(',');
                                        //��Ч��(��)
                                        string years = mode == 1 ? dt.Rows[i]["��Ч��(��)"].ToString().Trim() : dt.Rows[i]["��������(��)"].ToString().Trim();
                                        //��Ŀ����
                                        string code = "";
                                        //֤������
                                        string certName = string.Format("{0}-{1}-{2}", certType, workType, workItem);
                                        bool isOk = true;
                                        if (mode == 2)
                                        {
                                            if (dt.Columns.Contains("��Ŀ����"))
                                            {
                                                code = dt.Rows[i]["��Ŀ����"].ToString().Trim();
                                            }
                                            else
                                            {
                                                falseMessage = "ģ�岻��ȷ��ȱ�١���Ŀ���š���";
                                                isOk = false;
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                continue;
                                            }
                                        }

                                        //---****ֵ���ڿ���֤*****--
                                        if (string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(workType) || string.IsNullOrEmpty(workItem) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        if (mode == 1)
                                        {
                                            int number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzylb')", workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "����ҵ�����д����ȷ,δ�ܵ���.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and itemcode='{1}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzyxm')", workItem, workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "��׼����Ŀ��д����ȷ,δ�ܵ���.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                        }
                                        if (mode == 2)
                                        {
                                            if (string.IsNullOrWhiteSpace(code))
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "����Ŀ����Ϊ��,δ�ܵ���.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            int number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='tzzlb')", workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "��������д����ȷ,δ�ܵ���.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and itemcode='{1}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='tzsbxm')", workItem, workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "����ҵ��Ŀ��д����ȷ,δ�ܵ���.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                        }

                                        //--�ֻ�����֤
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "���ֻ��Ÿ�ʽ����,δ�ܵ���.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "��" + (i + 3) + "��֤����ţ�" + certNum + "���Ѵ��� ,δ�ܵ���.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.WorkType = workType;
                                                    cert.WorkItem = workItem;
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.ItemNum = code;
                                                    cert.CertName = certName;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "��" + (i + 3) + "��֤����Ƭ��" + str + "��������!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "��" + (i + 3) + "��ϵͳ�����ڸ���Ա" + userName + "��" + deptName + ")��Ϣ��";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;
                                    message += "����" + count + "����¼,�ɹ�����" + success + "����ʧ��" + lstErrors.Count + "����";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "������Ϣ���£�" + falseMessage;
                                    }
                                }
                                  //����ְҵ�ʸ�֤
                                cells = wb.Worksheets[2].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow > 1)
                                {
                                    certType = "ְҵ�ʸ�֤";
                                    success = 0;
                                    falseMessage = "";
                                    lstErrors.Clear();
                                    message += "</br></br>��ʼ����ְҵ�ʸ�֤,��Ϣ���£�</br>";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //����
                                        string deptName = dt.Rows[i]["��λ/����"].ToString().Trim();
                                        //����
                                        string userName = dt.Rows[i]["����"].ToString().Trim();

                                        //��֤����
                                        string sendOrg = dt.Rows[i]["��֤����"].ToString().Trim();
                                        //��֤����
                                        string sendDate = dt.Rows[i]["��֤����"].ToString().Trim();
                                        //��Ч��
                                        string endDate = dt.Rows[i]["��Ч����"].ToString().Trim();
                                        //֤����
                                        string certNum = dt.Rows[i]["֤����"].ToString().Trim();
                                        //�ֻ���
                                        string mobile = dt.Rows[i]["�ֻ���"].ToString().Trim();
                                        //֤����Ƭ
                                        string photos = dt.Rows[i]["֤����Ƭ"].ToString().Trim().Trim(',');
                                        //��Ч��(��)
                                        string years = dt.Rows[i]["��Ч��(��)"].ToString().Trim();
                                        //�ȼ�
                                        string grade = dt.Rows[i]["�ȼ�"].ToString().Trim();
                                        //����
                                        string craft = dt.Rows[i]["����"].ToString().Trim();

                                        //֤������
                                        string certName = string.Format("{0}-{1}-{2}", certType, craft, grade);
                                        bool isOk = true;

                                        //---****ֵ���ڿ���֤*****--
                                        if (string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(certType) || string.IsNullOrEmpty(certName) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName) || string.IsNullOrEmpty(craft))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "���ֻ��Ÿ�ʽ����,δ�ܵ���.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "��" + (i + 3) + "��֤����ţ�" + certNum + "���Ѵ��� ,δ�ܵ���.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.CertName = certName;
                                                    cert.Grade = grade;
                                                    cert.CertName = certName;
                                                    cert.Craft = craft;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "��" + (i + 3) + "��֤����Ƭ��" + str + "��������!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "��" + (i + 3) + "��ϵͳ�����ڸ���Ա" + userName + "��" + deptName + ")��Ϣ��";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;
                                    message += "����" + count + "����¼,�ɹ�����" + success + "����ʧ��" + lstErrors.Count + "����";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "������Ϣ���£�" + falseMessage;
                                    }
                                }
                               //����רҵ�����ʸ�֤
                                certType = "רҵ�����ʸ�֤";
                                success = 0;
                                falseMessage = "";
                                lstErrors.Clear();
                                cells = wb.Worksheets[3].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow>1)
                                {
                                    message += "</br></br>��ʼ����רҵ�����ʸ�֤,��Ϣ���£�</br>";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //����
                                        string deptName = dt.Rows[i]["��λ/����"].ToString().Trim();
                                        //����
                                        string userName = dt.Rows[i]["����"].ToString().Trim();

                                        //��֤����
                                        string sendOrg = dt.Rows[i]["��֤����"].ToString().Trim();
                                        //��֤����
                                        string sendDate = dt.Rows[i]["��֤����"].ToString().Trim();
                                        //��Ч��
                                        string endDate = dt.Rows[i]["��Ч����"].ToString().Trim();
                                        //֤����
                                        string certNum = dt.Rows[i]["֤����"].ToString().Trim();
                                        //�ֻ���
                                        string mobile = dt.Rows[i]["�ֻ���"].ToString().Trim();
                                        //֤����Ƭ
                                        string photos = dt.Rows[i]["֤����Ƭ"].ToString().Trim().Trim(',');
                                        //��Ч��(��)
                                        string years = dt.Rows[i]["��Ч��(��)"].ToString().Trim();
                                        //�ʸ�����
                                        string zgname = dt.Rows[i]["�ʸ�����"].ToString().Trim();
                                        //֤������
                                        string certName = string.Format("{0}-{1}", certType, zgname);
                                        bool isOk = true;

                                        //---****ֵ���ڿ���֤*****--
                                        if (string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(certType) || string.IsNullOrEmpty(certName) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        //--�ֻ�����֤
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "���ֻ��Ÿ�ʽ����,δ�ܵ���.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "��" + (i + 3) + "��֤����ţ�" + certNum + "���Ѵ��� ,δ�ܵ���.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.CertName = certName;
                                                    cert.ZGName = zgname;
                                                    cert.CertName = certName;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "��" + (i + 3) + "��֤����Ƭ��" + str + "��������!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "��" + (i + 3) + "��ϵͳ�����ڸ���Ա" + userName + "��" + deptName + ")��Ϣ��";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;

                                    message += "����" + count + "����¼,�ɹ�����" + success + "����ʧ��" + lstErrors.Count + "����";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "������Ϣ���£�" + falseMessage;
                                    }
                                }
                              
                                //���밲ȫ����֪ʶ�͹����������˺ϸ�֤
                                certType = "��ȫ����֪ʶ�͹����������˺ϸ�֤";
                                success = 0;
                                falseMessage = "";
                                lstErrors.Clear();
                                cells = wb.Worksheets[4].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow > 1)
                                {
                                    message += "</br></br>��ʼ���밲ȫ����֪ʶ�͹����������˺ϸ�֤,��Ϣ���£�</br>";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //����
                                        string deptName = dt.Rows[i]["��λ/����"].ToString().Trim();
                                        //����
                                        string userName = dt.Rows[i]["����"].ToString().Trim();

                                        //��֤����
                                        string sendOrg = dt.Rows[i]["��֤����"].ToString().Trim();
                                        //��֤����
                                        string sendDate = dt.Rows[i]["��������"].ToString().Trim();
                                        //��ʼ����
                                        string startDate = dt.Rows[i]["��������"].ToString().Trim();
                                        //��Ч��
                                        string endDate = dt.Rows[i]["��Ч����"].ToString().Trim();
                                        //֤����
                                        string certNum = dt.Rows[i]["֤����"].ToString().Trim();
                                        //�ֻ���
                                        string mobile = dt.Rows[i]["�ֻ���"].ToString().Trim();
                                        //֤����Ƭ
                                        string photos = dt.Rows[i]["֤����Ƭ"].ToString().Trim().Trim(',');
                                        //��Ч��(��)
                                        string years = dt.Rows[i]["��Ч��(��)"].ToString().Trim();
                                        //��Ա����
                                        string userType = dt.Rows[i]["��Ա����"].ToString().Trim();
                                        //��ҵ���
                                        string industry = dt.Rows[i]["��ҵ���"].ToString().Trim();
                                        //֤������
                                        string certName = string.Format("{0}-{1}-{2}", certType, userType, industry);
                                        bool isOk = true;

                                        //---****ֵ���ڿ���֤*****--
                                        if (string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(certType) || string.IsNullOrEmpty(certName) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        //--�ֻ�����֤
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "���ֻ��Ÿ�ʽ����,δ�ܵ���.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "��" + (i + 3) + "��֤����ţ�" + certNum + "���Ѵ��� ,δ�ܵ���.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = cert.StartDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.CertName = certName;
                                                    cert.UserType = userType;
                                                    cert.Industry = industry;
                                                    cert.CertName = certName;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "��" + (i + 3) + "��֤����Ƭ��" + str + "��������!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "��" + (i + 3) + "��ϵͳ�����ڸ���Ա" + userName + "��" + deptName + ")��Ϣ��";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;
                                    message += "����" + count + "����¼,�ɹ�����" + success + "����ʧ��" + lstErrors.Count + "����";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "������Ϣ���£�" + falseMessage;
                                    }
                             
                                }
                                //������������Ա֤��
                                success = 0;
                                falseMessage = "";
                                lstErrors.Clear();

                             
                                cells = wb.Worksheets[5].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow > 1)
                                {
                                    message += "</br></br>��ʼ������������Ա֤��,��Ϣ���£�</br>";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //����
                                        string deptName = dt.Rows[i]["��λ/����"].ToString().Trim();
                                        //����
                                        string userName = dt.Rows[i]["����"].ToString().Trim();
                                        //֤������
                                        certType = dt.Rows[i]["֤������"].ToString().Trim();
                                        //֤������
                                        string certName = dt.Rows[i]["֤������"].ToString().Trim();
                                        //��֤����
                                        string sendOrg = dt.Rows[i]["��֤����"].ToString().Trim();
                                        //��֤����
                                        string sendDate = dt.Rows[i]["��������"].ToString().Trim();
                                        //��Ч��
                                        string endDate = dt.Rows[i]["��Ч����"].ToString().Trim();
                                        //֤����
                                        string certNum = dt.Rows[i]["֤����"].ToString().Trim();
                                        //�ֻ���
                                        string mobile = dt.Rows[i]["�ֻ���"].ToString().Trim();
                                        //֤����Ƭ
                                        string photos = dt.Rows[i]["֤����Ƭ"].ToString().Trim().Trim(',');
                                        //��Ч��(��)
                                        string years = dt.Rows[i]["��Ч��(��)"].ToString().Trim();
                                        bool isOk = true;

                                        //---****ֵ���ڿ���֤*****--
                                        if (string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(certType) || string.IsNullOrEmpty(certName) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        //--�ֻ�����֤

                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "���ֻ��Ÿ�ʽ����,δ�ܵ���.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "��" + (i + 3) + "��֤����ţ�" + certNum + "���Ѵ��� ,δ�ܵ���.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.CertName = certName;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "��" + (i + 3) + "��֤����Ƭ��" + str + "��������!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "��" + (i + 3) + "��ϵͳ�����ڸ���Ա" + userName + "��" + deptName + ")��Ϣ��";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;
                                    message += "����" + count + "����¼,�ɹ�����" + success + "����ʧ��" + lstErrors.Count + "����";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "������Ϣ���£�" + falseMessage;
                                    }
                                }
                            }

                        }
                        else
                        {
                            message = "����֤��ʧ��,���Ժ�����";
                        }
                    }
                    else
                    {
                        message = "����֤��ʧ��,���Ժ�����";
                    }
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// ����������ҵ�������豸֤��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportTZCert(int mode = 1)
        {
            try
            {
                int success = 0;
                string message = "��ѡ����ȷ��Zip�ļ���";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    string zipName = Path.GetFileNameWithoutExtension(file.FileName);
                    string certType = mode == 1 ? "������ҵ����֤" : "�����豸��ҵ��Ա֤";
                    string kind = mode == 1 ? "ryzylb" : "tzzlb";
                    string type = mode == 1 ? "ryzyxm" : "tzsbxm";
                    string dirName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string path = Server.MapPath("~/Resource/temp/" + dirName + Path.GetExtension(file.FileName));
                    string destPath = Server.MapPath("~/Resource/cert/" + dirName);
                    string dir = "֤����Ƭ";
                    file.SaveAs(path);
                    if (System.IO.File.Exists(path))
                    {
                        if (UnZip(path, destPath, "", true))
                        {
                            FileInfo fi = new DirectoryInfo(destPath + "\\" + zipName).GetFiles("*.*").Where(t => t.Name.ToLower().EndsWith(".xls") || t.Name.ToLower().EndsWith(".xlxs")).FirstOrDefault();
                            if (fi == null)
                            {
                                message = "ѹ������û�м�⵽excel�ļ���";
                            }
                            else
                            {
                                DirectoryInfo dirs = new DirectoryInfo(destPath + "\\" + zipName).GetDirectories().FirstOrDefault();
                                if (dirs != null)
                                {
                                    dir = dirs.Name;
                                }
                                List<int> lstErrors = new List<int>();
                                string fileName = fi.Name;
                                Workbook wb = new Aspose.Cells.Workbook();
                                wb.Open(destPath + "\\" + zipName + "\\" + fileName);
                                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow>1)
                                {
                                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    DepartmentBLL deptBll = new DepartmentBLL();
                                    FileInfoBLL fileinfobll = new FileInfoBLL();
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //����
                                        string userName = dt.Rows[i]["����"].ToString().Trim();
                                        //����
                                        string deptName = dt.Rows[i]["��λ/����"].ToString().Trim();

                                        //֤������
                                        string workType = mode == 1 ? dt.Rows[i]["��ҵ���"].ToString().Trim() : dt.Rows[i]["����"].ToString().Trim();
                                        //����
                                        string workItem = mode == 1 ? dt.Rows[i]["������Ŀ"].ToString().Trim() : dt.Rows[i]["��ҵ��Ŀ"].ToString().Trim();
                                        //��֤����
                                        string sendOrg = dt.Rows[i]["��֤����"].ToString().Trim();
                                        //��֤����
                                        string sendDate = dt.Rows[i]["��������"].ToString().Trim();
                                        //��ʼ����
                                        string startDate = "";
                                        if (mode == 1)
                                        {
                                            startDate = dt.Rows[i]["��Ч�ڿ�ʼ����"].ToString().Trim();
                                        }
                                        //��������
                                        string endDate = mode == 1 ? dt.Rows[i]["��Ч�ڽ�������"].ToString().Trim() : dt.Rows[i]["��Ч����"].ToString().Trim();
                                        //��������
                                        string applyDate = mode == 1 ? dt.Rows[i]["Ӧ��������"].ToString().Trim() : "";
                                        //֤����
                                        string certNum = dt.Rows[i]["֤����"].ToString().Trim();
                                        //�ֻ���
                                        string mobile = dt.Rows[i]["�ֻ���"].ToString().Trim();
                                        //֤����Ƭ
                                        string photos = dt.Rows[i]["֤����Ƭ"].ToString().Trim().Trim(',');
                                        //��Ч��(��)
                                        string years = mode == 1 ? dt.Rows[i]["��Ч��(��)"].ToString().Trim() : dt.Rows[i]["��������(��)"].ToString().Trim();
                                        //��Ŀ����
                                        string code = "";
                                        string certName = string.Format("{0}-{1}-{2}", certType, workType, workItem);
                                        bool isOk = true;
                                        if (mode == 2)
                                        {
                                            if (dt.Columns.Contains("��Ŀ����"))
                                            {
                                                code = dt.Rows[i]["��Ŀ����"].ToString().Trim();
                                            }
                                            else
                                            {
                                                falseMessage = "ģ�岻��ȷ��ȱ�١���Ŀ���š���";
                                                isOk = false;
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                continue;
                                            }
                                        }
                                        if (mode == 1)
                                        {
                                            if (string.IsNullOrEmpty(workType) || string.IsNullOrEmpty(workItem) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName) || string.IsNullOrEmpty(applyDate))
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";

                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            int number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzylb')", workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "����ҵ�����д����ȷ,δ�ܵ���.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and itemcode='{1}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzyxm')", workItem, workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "�в�����Ŀ��д����ȷ,δ�ܵ���.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                        }
                                        if (mode == 2)
                                        {
                                            if (string.IsNullOrEmpty(workType) || string.IsNullOrEmpty(workItem) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName))
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";

                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            //if (string.IsNullOrWhiteSpace(code))
                                            //{
                                            //    falseMessage += "</br>" + "��" + (i + 3) + "����Ŀ����Ϊ��,δ�ܵ���.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            int number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='tzzlb')", workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "��������д����ȷ,δ�ܵ���.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and itemcode='{1}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='tzsbxm')", workItem, workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "��" + (i + 3) + "����ҵ��Ŀ��д����ȷ,δ�ܵ���.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                        }

                                        //--�ֻ�����֤
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "���ֻ��Ÿ�ʽ����,δ�ܵ���.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "��" + (i +3) + "��֤����ţ�" + certNum + "���Ѵ��� ,δ�ܵ���.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.WorkType = workType;
                                                    cert.WorkItem = workItem;
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.CertName = certName;
                                                    cert.ItemNum = code;
                                                    if (!string.IsNullOrWhiteSpace(applyDate))
                                                    {
                                                        cert.ApplyDate = DateTime.Parse(applyDate);
                                                    }
                                                    if (!string.IsNullOrWhiteSpace(startDate))
                                                    {
                                                        cert.StartDate = startDate.ToDate();
                                                    }
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "��" + (i + 3) + "��֤����Ƭ��" + str + "��������!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "��" + (i + 3) + "�в����ڸ��ֻ���(" + mobile + ")���û���Ϣ��";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }

                                    count = dt.Rows.Count;
                                    message = "����" + count + "����¼,�ɹ�����" + success + "����ʧ��" + lstErrors.Count + "��";
                                    message += "</br>" + falseMessage;
                                }
                            }

                        }
                        else
                        {
                            message = "����֤��ʧ��,���Ժ�����";
                        }
                    }
                    else
                    {
                        message = "����֤��ʧ��,���Ժ�����";
                    }
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion
    }
}
