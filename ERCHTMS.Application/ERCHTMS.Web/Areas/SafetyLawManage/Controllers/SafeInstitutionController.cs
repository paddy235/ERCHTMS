using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.SafetyLawManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Web;
using System;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using System.IO;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using ICSharpCode.SharpZipLib.Zip;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// �� ������ȫ�����ƶ�
    /// </summary>
    public class SafeInstitutionController : MvcControllerBase
    {
        private SafeInstitutionBLL safeinstitutionbll = new SafeInstitutionBLL();
        private SafeInstitutionTreeBLL safeinstitutiontreebll = new SafeInstitutionTreeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private DepartmentBLL deptBll = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();

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
        /// �ҵ��ղ�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult myStoreIndex()
        {
            return View();
        }
        /// <summary>
        /// ���б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewIndex()
        {
            return View();
        }
        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TreeForm()
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
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id";
            pagination.p_fields = " a.CreateDate,FileName,IssueDept,FileCode,CarryDate,FilesId,releasedate,revisedate,lawtypename,a.createuserid,a.createuserdeptcode,a.createuserorgcode ";
            pagination.p_tablename = " bis_safeinstitution a ";
            pagination.conditionJson = "1=1";
           
            var data = safeinstitutionbll.GetPageDataTable(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = safeinstitutionbll.GetList(queryJson);
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
            var data = safeinstitutionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ����ڵ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTypeTreeJson(string datatype,string orgcode)
        {
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            string code = user.OrganizeCode;
            if (!string.IsNullOrWhiteSpace(orgcode))
            {
                code = orgcode;
            }
            var where = string.Format(" and CreateUserOrgCode='{0}' and datatype='{1}'", code, datatype);
            
            //var where = string.Format(" and 1=1", user.OrganizeCode);
            var data = safeinstitutiontreebll.GetList(where).OrderBy(t => t.CREATEDATE).ToList();
            foreach (var item in data)
            {
                bool hasChild = data.Where(x => x.ParentId == item.ID).Count() > 0 ? true : false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.TreeName;
                tree.value = item.TreeCode;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "Code";
                tree.AttributeValue = item.TreeCode;

                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("-1"));
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetTreeFormJson(string keyValue)
        {
            var data = safeinstitutiontreebll.GetEntity(keyValue);
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
            safeinstitutionbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveTreeForm(string keyValue)
        {
            safeinstitutiontreebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafeInstitutionEntity entity)
        {
            safeinstitutionbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
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
        public ActionResult SaveTreeForm(string keyValue, SafeInstitutionTreeEntity entity)
        {
            entity.ParentId = !string.IsNullOrWhiteSpace(entity.ParentId) ? entity.ParentId : "-1";
            var parent = safeinstitutiontreebll.GetEntity(keyValue);
            if (parent == null) {
                entity.TreeCode = GetDepartmentCode(entity);
            }
            entity.TreeName = entity.TreeName.Replace("\\", "�v");
            safeinstitutiontreebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        /// <summary>
        /// �ļ�����
        /// </summary>
        /// <param name="treeId"></param>
        /// <param name="treeName"></param>
        /// <param name="treeCode"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportStandard(string treeId, string treeName, string treeCode)
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
                        SafeInstitutionEntity entity = new SafeInstitutionEntity();
                        entity.Id = Guid.NewGuid().ToString();
                        entity.FilesId = Guid.NewGuid().ToString();

                        //�ļ�����
                        string filename = dt.Rows[i][0].ToString();
                        //�ļ����
                        string filecode = dt.Rows[i][1].ToString();
                        //������λ
                        string issuedept = dt.Rows[i][4].ToString();
                        //����ʱ��
                        string releasedate = dt.Rows[i][5].ToString();
                        //�޶�ʱ��
                        string revisedate = dt.Rows[i][6].ToString();
                        //ʵʩʱ��
                        string carrydate = dt.Rows[i][7].ToString();
                        //��ע
                        string Remark = dt.Rows[i][8].ToString();



                        //---****ֵ���ڿ���֤*****--
                        if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(filecode))
                        {
                            falseMessage += "</br>" + "��" + (i + 1) + "��ֵ���ڿ�,δ�ܵ���.";
                            error++;
                            continue;
                        }

                        bool conbool = false;


                        //���ĸ���·��
                        string[] filepaths = dt.Rows[i][2].ToString().Split(';');

                        var filepath = "";
                        for (int j = 0; j < filepaths.Length; j++)
                        {
                            filepath = filepaths[j];

                            if (string.IsNullOrEmpty(filepath))
                            {
                                continue;
                            }
                            string strPath = filepath.Substring(filepath.IndexOf('.'));
                            //---****�ļ���ʽ��֤*****--
                            if (!(strPath.Contains("doc") || strPath.Contains("docx") || strPath.Contains("pdf") ))
                            {
                                falseMessage += "</br>" + "��" + (i + 1) + "��ָ�����ĸ�����ʽ����ȷ,δ�ܵ���.";
                                error++;
                                conbool = true;
                                continue;
                            }

                            //---****�ļ��Ƿ������֤*****--
                            if (!System.IO.File.Exists(decompressionDirectory + filepath))
                            {
                                falseMessage += "</br>" + "��" + (i + 1) + "��ָ�����ĸ���������,δ�ܵ���.";
                                error++;
                                conbool = true;
                                continue;
                            }
                            var fileinfo = new FileInfo(decompressionDirectory + filepath);
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            string fileguid = Guid.NewGuid().ToString();
                            fileInfoEntity.Create();
                            fileInfoEntity.RecId = entity.FilesId; //����ID
                            fileInfoEntity.FileName = filepath;
                            fileInfoEntity.FilePath = "~/Resource/InstitutionSystem/" + fileguid + fileinfo.Extension;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                            fileInfoEntity.FileExtensions = fileinfo.Extension;
                            fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                            TransportRemoteToServer(Server.MapPath("~/Resource/InstitutionSystem/"), decompressionDirectory + filepath, fileguid + fileinfo.Extension);
                            fileinfobll.SaveForm("", fileInfoEntity);

                        }

                        if (conbool)
                        {
                            continue;
                        }
                        //���ĸ���·��
                        filepaths = dt.Rows[i][3].ToString().Split(';');

                        filepath = "";
                        for (int j = 0; j < filepaths.Length; j++)
                        {
                            filepath = filepaths[j];

                            if (string.IsNullOrEmpty(filepath))
                            {
                                continue;
                            }
                            string strPath = filepath.Substring(filepath.IndexOf('.'));
                            //---****�ļ���ʽ��֤*****--
                            if (!(strPath.Contains("doc") || strPath.Contains("docx") || strPath.Contains("pdf") || strPath.Contains("ppt") || strPath.Contains("xlsx") || strPath.Contains("xls") || strPath.Contains("png") || strPath.Contains("jpg") || strPath.Contains("jpeg")))
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
                            fileInfoEntity.RecId = entity.Id; //����ID
                            fileInfoEntity.FileName = filepath;
                            fileInfoEntity.FilePath = "~/Resource/InstitutionSystem/" + fileguid + fileinfo.Extension;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                            fileInfoEntity.FileExtensions = fileinfo.Extension;
                            fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                            TransportRemoteToServer(Server.MapPath("~/Resource/InstitutionSystem/"), decompressionDirectory + filepath, fileguid + fileinfo.Extension);
                            fileinfobll.SaveForm("", fileInfoEntity);

                        }

                        entity.FileName = filename;
                        entity.FileCode = filecode;
                        entity.IssueDept = issuedept;
                        entity.LawTypeId = treeId;
                        entity.LawTypeName = treeName;
                        entity.LawTypeCode = treeCode;
                        if (!string.IsNullOrEmpty(releasedate))
                        {
                            entity.ReleaseDate = Convert.ToDateTime(releasedate);
                        }
                        if (!string.IsNullOrEmpty(revisedate))
                        {
                            entity.ReviseDate = Convert.ToDateTime(revisedate);
                        }
                        if (!string.IsNullOrEmpty(carrydate))
                        {
                            entity.CarryDate = Convert.ToDateTime(carrydate);
                        }

                        entity.Remark = !string.IsNullOrEmpty(Remark) ? Remark : "";

                        try
                        {
                            safeinstitutionbll.SaveForm(entity.Id, entity);
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

        #region ����
        /// <summary>
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "Id";
                pagination.p_fields = "FileName,FileCode,IssueDept,to_char(releasedate,'yyyy-MM-dd') as releasedate,to_char(revisedate,'yyyy-MM-dd') as revisedate,to_char(carrydate,'yyyy-MM-dd') as carrydate,lawtypename";
                pagination.p_tablename = " bis_safeinstitution a ";
                pagination.conditionJson = "1=1";
                pagination.sidx = "a.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                
                DataTable exportTable = safeinstitutionbll.GetPageDataTable(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��ȫ�����ƶ���Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��ȫ�����ƶ���Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "�ļ�����", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "�ļ����", Width = 25 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "������λ(����)", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "releasedate", ExcelColumn = "����ʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "revisedate", ExcelColumn = "�޶�ʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "ʵʩʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "lawtypename", ExcelColumn = "����", Width = 20 });

                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        #endregion

        /// <summary>
        /// ���ݵ�ǰ������ȡ��Ӧ�Ļ�������  �������� 2-6-8-10  λ
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public string GetDepartmentCode(SafeInstitutionTreeEntity Entity)
        {
            string maxCode = string.Empty;
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var maxObj = deptBll.GetDataTable(string.Format("select max(TreeCode) as TreeCode  from bis_safeinstitutiontree t where  parentid='{0}' and CreateUserOrgCode='{1}' and datatype='{2}' ", Entity.ParentId, user.OrganizeCode, Entity.DataType));
            if (maxObj != null && maxObj.Rows.Count > 0 && !string.IsNullOrEmpty(maxObj.Rows[0][0].ToString()))
            {
                string newCode = string.Empty;

                string maxValue = (Convert.ToDecimal(maxObj.Rows[0][0].ToString()) + 1).ToString();

                for (int i = 1; i <= 30; i++)
                {
                    if (maxValue.ToString().Length == i)
                    {
                        newCode = maxObj.Rows[0][0].ToString().Substring(0, maxObj.Rows[0][0].ToString().Length - i) + maxValue;
                        break;
                    }
                }
                maxCode = newCode;
            }
            else
            {
                SafeInstitutionTreeEntity parentEntity = safeinstitutiontreebll.GetEntity(Entity.ParentId);  //��ȡ������
                if (parentEntity != null)
                {
                    maxCode = parentEntity.TreeCode + "001";  //�̶�ֵ,�ǿɱ�
                }
                else
                {
                    maxCode = "001";
                }

            }

            return maxCode;
        }

        /// <summary>
        /// ��ȡ����ڵ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetParentOrgCode()
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var provdata = departmentBLL.GetList().Where(t => user.NewDeptCode.StartsWith(t.DeptCode) && t.Nature == "ʡ��" && string.IsNullOrWhiteSpace(t.Description));
                if (provdata.Count() > 0)
                {
                    DepartmentEntity provEntity = provdata.FirstOrDefault();
                    return Content(provEntity.DeptCode);
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
