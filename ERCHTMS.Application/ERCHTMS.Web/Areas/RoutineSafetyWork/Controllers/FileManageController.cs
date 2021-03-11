using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Web;
using System.Data;
using System.IO;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ICSharpCode.SharpZipLib.Zip;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// �� �����ļ�����
    /// </summary>
    public class FileManageController : MvcControllerBase
    {
        private FileManageBLL filemanagebll = new FileManageBLL();
        private FileTreeManageBLL filetreemanagebll = new FileTreeManageBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private DepartmentBLL deptBll = new DepartmentBLL();
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
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TreeForm()
        {
            return View();
        }
        /// <summary>
        /// �ղ��б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FileStoreIndex()
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
        /// ��ȡ����ڵ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTypeTreeJson()
        {
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}'", user.OrganizeCode);
            //var where = string.Format(" and 1=1", user.OrganizeCode);
            var data = filetreemanagebll.GetList(where).OrderBy(t => t.CREATEDATE).ToList();
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
        /// ��ȡ��ɫ����ڵ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRoleTypeTreeJson(string keyword)
        {
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}'", user.OrganizeCode);
            var hasKeyword = !string.IsNullOrWhiteSpace(keyword);
            if (hasKeyword)
            {
                where += string.Format(" and TreeName like '%{0}%'", keyword);
            }
            var data = filetreemanagebll.GetList(where).OrderBy(t => t.CREATEDATE).ToList();
            foreach (var item in data)
            {
                bool hasChild = data.Where(x => x.ParentId == item.ID).Count() > 0 ? true : false;
                hasChild = hasKeyword ? false : hasChild;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.TreeName;
                tree.value = item.TreeCode;
                tree.parentId = hasKeyword ? "-1" : item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "Code";
                tree.AttributeValue = item.TreeCode;

                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("-1"));
            //var parentId = hasKeyword ? "-1" : data[0].ParentId;

            //return Content(treeList.TreeToJson(parentId));
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ѯ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = filemanagebll.GetList(pagination, queryJson);
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
            var data = filemanagebll.GetEntity(keyValue);
            //����ֵ
            var josnData = new
            {
                data
            };

            return Content(josnData.ToJson());
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetTreeFormJson(string keyValue)
        {
            var data = filetreemanagebll.GetEntity(keyValue);
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
            filemanagebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FileManageEntity entity)
        {
            filemanagebll.SaveForm(keyValue, entity);
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
        public ActionResult SaveTreeForm(string keyValue, FileManageTreeEntity entity)
        {
            entity.ParentId = !string.IsNullOrWhiteSpace(entity.ParentId) ? entity.ParentId : "-1";
            var parent = filetreemanagebll.GetEntity(keyValue);
            if (parent == null)
            {
                entity.TreeCode = GetDepartmentCode(entity);
            }
            entity.TreeName = entity.TreeName.Replace("\\", "�v");
            filetreemanagebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
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
            filetreemanagebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        #endregion
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
            var dt = filemanagebll.GetList(pagination, queryJson);
            string fileUrl = @"\Resource\ExcelTemplate\�ļ�����_����ģ��.xlsx";
            AsposeExcelHelper.ExecuteResult(dt, fileUrl, "�ļ�����", string.Format("�ļ�����_{0}", DateTime.Now.ToString("yyyyMMddHHmmss")));

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
                FileManageStoreBLL fms = new FileManageStoreBLL();
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var list = ids.Split(',');
                foreach (var id in list)
                {
                    var entity = fms.GetList(string.Format(" and userid='{0}' and FileManageId='{1}'", user.UserId, id)).FirstOrDefault();
                    if (entity == null)
                    {
                        entity = new FileManageStoreEntity()
                        {
                            UserId = user.UserId,
                            FileManageId = id
                        };
                        fms.SaveForm("", entity);
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
                FileManageStoreBLL fms = new FileManageStoreBLL();
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var list = fms.GetList(string.Format(" and userid='{0}' and FileManageId in ('{1}')", user.UserId, ids.Replace(",", "','"))).ToList();
                fms.RemoveForm(list);
                return Success("ȡ���ɹ���");
            }
            else
            {
                return Error("ȡ��ʧ�ܡ�");
            }
        }

       /// <summary>
       /// �ļ�����
       /// </summary>
       /// <param name="refid"></param>
       /// <param name="refname"></param>
       /// <param name="deptcode"></param>
       /// <param name="refcode"></param>
       /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportStandard(string refid, string refname, string deptcode, string refcode)
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
                        FileManageEntity standard = new FileManageEntity();
                        standard.ID = Guid.NewGuid().ToString();

                        //�ļ�����
                        string filename = dt.Rows[i][0].ToString();
                        //�ļ����
                        string fileno = dt.Rows[i][1].ToString();



                        //---****ֵ���ڿ���֤*****--
                        if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(dt.Rows[i][2].ToString()))
                        {
                            falseMessage += "</br>" + "��" + (i + 1) + "��ֵ���ڿ�,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        var oldList = filemanagebll.GetList(String.Format(" and createuserorgcode='{0}' and fileno='{1}' and id<>'{2}'", user.OrganizeCode, fileno, standard.ID)).ToList();
                        var r = oldList.Count > 0;
                        if (r) {
                            falseMessage += "</br>" + "��" + (i + 1) + "�д�����ͬ�ļ����,δ�ܵ���.";
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
                            fileInfoEntity.RecId = standard.ID; //����ID
                            fileInfoEntity.FileName = filepath;
                            fileInfoEntity.FilePath = "~/Resource/FileManageSystem/" + fileguid + fileinfo.Extension;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                            fileInfoEntity.FileExtensions = fileinfo.Extension;
                            fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                            TransportRemoteToServer(Server.MapPath("~/Resource/FileManageSystem/"), decompressionDirectory + filepath, fileguid + fileinfo.Extension);
                            fileinfobll.SaveForm("", fileInfoEntity);

                        }

                        if (conbool)
                        {
                            continue;
                        }

                        standard.FileName = filename;
                        standard.FileNo = fileno;
                        standard.FileTypeId = refid;
                        standard.FileTypeName = refname;
                        standard.FileTypeCode = refcode;
                        DepartmentEntity deptEntity = deptBll.GetEntityByCode(deptcode);
                        if (deptEntity != null)
                        {
                            standard.ReleaseDeptId = deptEntity.DepartmentId;
                            standard.ReleaseDeptName = deptEntity.FullName;
                        }
                        else
                        {
                            standard.ReleaseDeptId = OperatorProvider.Provider.Current().DeptId;
                            standard.ReleaseDeptName = OperatorProvider.Provider.Current().DeptName;
                        }


                        if (!string.IsNullOrEmpty(dt.Rows[i][3].ToString()))
                        {
                            standard.ReleaseTime = Convert.ToDateTime(dt.Rows[i][3].ToString());
                        }
                       
                        standard.Remark = !string.IsNullOrEmpty(dt.Rows[i][4].ToString()) ? dt.Rows[i][4].ToString() : "";

                        try
                        {
                            filemanagebll.SaveForm(standard.ID, standard);
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
        /// <summary>
        /// �Ƿ������ͬ��ŵ�Ԫ��
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <param name="fileName">���</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult ExistSameFile(string keyValue, string fileNo)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var oldList = filemanagebll.GetList(String.Format(" and createuserorgcode='{0}' and fileno='{1}' and id<>'{2}'", user.OrganizeCode, fileNo, keyValue)).ToList();
            var r = oldList.Count > 0;

            return Success("����ͬ���ļ�����У����", r);
        }

        /// <summary>
        /// ���ݵ�ǰ������ȡ��Ӧ�Ļ�������  �������� 2-6-8-10  λ
        /// </summary>
        /// <param name="districtEntity"></param>
        /// <returns></returns>
        public string GetDepartmentCode(FileManageTreeEntity Entity)
        {
            string maxCode = string.Empty;
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var maxObj = deptBll.GetDataTable(string.Format("select max(TreeCode) as TreeCode  from BIS_FileManageTree t where  parentid='{0}' and CreateUserOrgCode='{1}'", Entity.ParentId, user.OrganizeCode));
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
                FileManageTreeEntity parentEntity = filetreemanagebll.GetEntity(Entity.ParentId);  //��ȡ������
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

    }
}
