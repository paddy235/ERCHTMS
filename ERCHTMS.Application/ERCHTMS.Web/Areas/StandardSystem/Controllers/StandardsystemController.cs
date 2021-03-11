using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Busines.StandardSystem;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using ERCHTMS.Code;
using System.Data;
using System.IO;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PublicInfoManage;
using System.Web;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;
using System.Linq;
using ERCHTMS.Entity.PublicInfoManage;
using ICSharpCode.SharpZipLib.Zip;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    /// <summary>
    /// �� ������׼��ϵ
    /// </summary>
    public class StandardsystemController : MvcControllerBase
    {
        private StandardsystemBLL standardsystembll = new StandardsystemBLL();
        private StcategoryBLL stcategorybll = new StcategoryBLL();
        private DepartmentBLL DepartmentBLL = new DepartmentBLL();
        private PostBLL postBLL = new PostBLL();
        private ElementBLL elementBLL = new ElementBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private StandardreadrecordBLL standardreadrecordbll = new StandardreadrecordBLL();

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
        /// �ҵ��ղ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Mystore()
        {
            return View();
        }

        /// <summary>
        /// ����
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
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "a.ID";
            pagination.p_fields = "a.filename,b.name as categorycode,b.id as categorycodeid,relevantelementname,relevantelementid,stationid,stationname,carrydate,a.createdate,consultnum,d.fullname as createuserdeptname,a.standardtype,a.createuserid,a.createuserdeptcode,a.createuserorgcode,(case  when  c.recid is null then '0' else '1' end) as isnew,a.Publishdept,e.name as maincategory,e.id as maincategoryid,e.parentid as mainparentid,filelist.filenum";
            pagination.p_tablename = @" hrs_standardsystem a left join hrs_stcategory b on a.categorycode =b.id left join hrs_standardreadrecord c on a.id =c.recid and c.createuserid ='" + user.UserId + "' left join base_department d on a.createuserdeptcode = d.encode left join hrs_stcategory e on e.id =b.parentid"
                + " left join (select  count(fileid) filenum,recid  from  base_fileinfo f  group by recid) filelist on a.id=filelist.recid ";
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
            }
            var data = standardsystembll.GetPageList(pagination, queryJson);
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
            var data = standardsystembll.GetList(queryJson);
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
            var data = standardsystembll.GetEntity(keyValue);
            if (!string.IsNullOrEmpty(data.CATEGORYCODE))
            {
                var catory = stcategorybll.GetEntity(data.CATEGORYCODE);
                data.CATEGORYNAME = string.IsNullOrEmpty(catory.NAME) ? "" : catory.NAME;
            }
            data.CREATEUSERDEPTNAME = DepartmentBLL.GetEntityByCode(data.CREATEUSERDEPTCODE).FullName;
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetStandardCount()
        {
            var watch = CommonHelper.TimerStart();
            var data = standardsystembll.GetStandardCount();
            var jsonData = new
            {
                data = data,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "a.ID";
                if (queryParam["standardtype"].ToString() == "1" || queryParam["standardtype"].ToString() == "2" || queryParam["standardtype"].ToString() == "3" || queryParam["standardtype"].ToString() == "4" || queryParam["standardtype"].ToString() == "5" || queryParam["standardtype"].ToString() == "6")
                {
                    pagination.p_fields = "filename,b.name as categorycode,relevantelementname,to_char(carrydate,'yyyy-MM-dd') as carrydate,to_char(a.createdate,'yyyy-MM-dd') as createdate,consultnum";
                }
                else
                {
                    pagination.p_fields = "filename,to_char(a.createdate,'yyyy-MM-dd') as createdate,c.fullname as createuserdeptname,consultnum";
                }
                
                pagination.p_tablename = " hrs_standardsystem a left join hrs_stcategory b on a.categorycode=b.id left join base_department c on a.createuserdeptcode = c.encode";
                pagination.conditionJson = "1=1";
                pagination.sidx = "a.createdate";
                pagination.sord = "desc";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DataTable exportTable = standardsystembll.GetPageList(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                
                if (!queryParam["standardtype"].IsEmpty())
                {
                    switch (queryParam["standardtype"].ToString())
                    {
                        case "1":
                            excelconfig.Title = "������׼��ϵ";
                            excelconfig.FileName = "������׼��ϵ��Ϣ����.xls";
                            break;
                        case "2":
                            excelconfig.Title = "�����׼��ϵ";
                            excelconfig.FileName = "�����׼��ϵ��Ϣ����.xls";
                            break;
                        case "3":
                            excelconfig.Title = "��λ��׼��ϵ";
                            excelconfig.FileName = "��λ��׼��ϵ��Ϣ����";
                            break;
                        case "4":
                            excelconfig.Title = "�ϼ���׼���ļ�";
                            excelconfig.FileName = "�ϼ���׼���ļ���Ϣ����.xls";
                            break;
                        case "5":
                            excelconfig.Title = "ָ����׼";
                            excelconfig.FileName = "ָ����׼��Ϣ����.xls";
                            break;
                        case "6":
                            excelconfig.Title = "���ɷ���";
                            excelconfig.FileName = "���ɷ�����Ϣ����.xls";
                            break;
                        case "7":
                            excelconfig.Title = "��׼��ϵ�߻��빹��";
                            excelconfig.FileName = "��׼��ϵ�߻��빹����Ϣ����.xls";
                            break;
                        case "8":
                            excelconfig.Title = "��׼��ϵ������Ľ�";
                            excelconfig.FileName = "��׼��ϵ������Ľ���Ϣ����.xls";
                            break;
                        case "9":
                            excelconfig.Title = "��׼����ѵ";
                            excelconfig.FileName = "��׼����ѵ��Ϣ����.xls";
                            break;
                        default:
                            break;
                    }
                }
                
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "�ļ�����", Width = 300 });
                if (queryParam["standardtype"].ToString() == "1" || queryParam["standardtype"].ToString() == "2" || queryParam["standardtype"].ToString() == "3" || queryParam["standardtype"].ToString() == "4" || queryParam["standardtype"].ToString() == "5" || queryParam["standardtype"].ToString() == "6")
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "categorycode", ExcelColumn = "���", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "relevantelementname", ExcelColumn = "��ӦԪ��", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "ʩ������", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "��������", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "consultnum", ExcelColumn = "����Ƶ��", Width = 300 });
                }
                else
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "��������", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createuserdeptname", ExcelColumn = "������λ/����", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "consultnum", ExcelColumn = "����Ƶ��", Width = 300 });
                }
                
                //���õ�������
                //ExcelHelper.ExcelDownload(exportTable, excelconfig);
                ExcelHelper.ExportByAspose(exportTable, excelconfig.FileName, excelconfig.ColumnEntity);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
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
        [HandlerMonitor(6, "ɾ����׼��ϵ")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            standardsystembll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ����ɾ������
        /// </summary>
        /// <param name="idsData">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(6, "����ɾ����׼��ϵ")]
        [AjaxOnly]
        public ActionResult RemoveListForm(string idsData)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(idsData))
            {
                if (idsData.Contains(","))
                {
                    string[] array = idsData.TrimEnd(',').Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        standardsystembll.RemoveForm(array[i].ToString());
                    }
                }
                else
                {
                    standardsystembll.RemoveForm(idsData);
                }
                return Success("ɾ���ɹ���");
            }
            return Error("������ɾ����");
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
        public ActionResult SaveForm(string keyValue, StandardsystemEntity entity)
        {
            try
            {
                standardsystembll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {

                throw;
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
        public string ImportStandard(string standardtype, string categorycode)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "��������Ա�޴˲���Ȩ��";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾
            int error = 0;
            string message = "��ѡ���ļ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                if (HttpContext.Request.Files.Count !=2)
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
                DataTable dt = cells.ExportDataTable(2, 0, cells.MaxDataRow - 1, cells.MaxColumn + 1, false);
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    //�ļ�����
                    string filename = dt.Rows[i][0].ToString();
                    //�ļ�·��
                    string filepath = dt.Rows[i][1].ToString();
                    //��ӦԪ��
                    string relevantelement = "";
                    string relevantelementname = "";
                    string relevantelementid = "";
                    //ʵʩ����
                    string carrydate = "";
                    if (standardtype == "1" || standardtype == "2" || standardtype == "3" || standardtype == "4" || standardtype == "5" || standardtype == "6")
                    {
                        relevantelement = dt.Rows[i][2].ToString();
                        carrydate = dt.Rows[i][3].ToString();
                    }

                    //��ѧ�ֺ�
                    string dispatchcode = "";
                    //�䲼����
                    string publishdept = "";
                    if (standardtype == "6")
                    {
                        dispatchcode = dt.Rows[i][4].ToString();
                        publishdept = dt.Rows[i][5].ToString();
                    }


                    string dutyid = "";
                    string dutyName = "";

                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(filepath))
                    {
                        falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    //---****�ļ���ʽ��֤*****--
                    if (!(filepath.Substring(filepath.IndexOf('.')).Contains("doc") || filepath.Substring(filepath.IndexOf('.')).Contains("docx") || filepath.Substring(filepath.IndexOf('.')).Contains("pdf")))
                    {
                        falseMessage += "</br>" + "��" + (i + 3) + "�и�����ʽ����ȷ,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    //---****�ļ��Ƿ������֤*****--
                    if (!System.IO.File.Exists(decompressionDirectory + filepath))
                    {
                        falseMessage += "</br>" + "��" + (i + 3) + "�и���������,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    //--**��֤��λ�Ƿ���� * *--
                    int startnum = 4;
                    if (standardtype == "1" || standardtype == "2" || standardtype == "3" || standardtype == "4" || standardtype == "5")
                    {
                        startnum = 4;
                    }
                    else if (standardtype == "6")
                    {
                        startnum = 6;
                    }
                    else if (standardtype == "7" || standardtype == "8" || standardtype == "9")
                    {
                        startnum = 2;
                    }
                    for (int j = startnum; j < dt.Columns.Count; j++)
                    {
                        if (!dt.Rows[i][j].IsEmpty())
                        {

                            foreach (var item in dt.Rows[i][j].ToString().Split(','))
                            {
                                DepartmentEntity dept = DepartmentBLL.GetList().Where(t => t.OrganizeId == orgId && t.FullName == dt.Rows[0][j].ToString()).FirstOrDefault();
                                if (dept == null)
                                {
                                    continue;
                                }
                                RoleEntity re = postBLL.GetList().Where(a => a.FullName == item.ToString() && a.OrganizeId == orgId && a.DeleteMark == 0 && a.EnabledMark == 1 && a.DeptId == dept.DepartmentId).FirstOrDefault();
                                if (re == null)
                                {
                                    //falseMessage += "</br>" + "��" + (i + 3) + "�и�λ����,δ�ܵ���.";
                                    //error++;
                                    continue;
                                }
                                else
                                {
                                    dutyid += re.RoleId + ",";
                                    dutyName += re.FullName + ",";
                                }
                            }
                        }
                    }

                    dutyid = dutyid.Length > 0 ? dutyid.Substring(0, dutyid.Length - 1) : "";
                    dutyName = dutyName.Length > 0 ? dutyName.Substring(0, dutyName.Length - 1) : "";
                    StandardsystemEntity standard = new StandardsystemEntity();
                    try
                    {
                        if (!string.IsNullOrEmpty(carrydate))
                        {
                            standard.CARRYDATE = DateTime.Parse(DateTime.Parse(carrydate).ToString("yyyy-MM-dd"));
                        }
                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 3) + "��ʱ������,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    if (!string.IsNullOrEmpty(relevantelement))
                    {
                        foreach (var item in relevantelement.Split(','))
                        {

                            ElementEntity re = elementBLL.GetList("").Where(a => a.NAME == item.ToString()).FirstOrDefault();
                            if (re == null)
                            {
                                //falseMessage += "</br>" + "��" + (i + 2) + "����ӦԪ������,δ�ܵ���.";
                                //error++;
                                continue;
                            }
                            else
                            {
                                relevantelementname += re.NAME + ",";
                                relevantelementid += re.ID + ",";
                            }
                        }
                    }
                    relevantelementname = string.IsNullOrEmpty(relevantelementname) ? "" : relevantelementname.Substring(0, relevantelementname.Length - 1);
                    relevantelementid = string.IsNullOrEmpty(relevantelementid) ? "" : relevantelementid.Substring(0, relevantelementid.Length - 1);
                    standard.FILENAME = filename;
                    standard.STATIONID = dutyid;
                    standard.STATIONNAME = dutyName;
                    standard.RELEVANTELEMENTNAME = relevantelementname;
                    standard.RELEVANTELEMENTID = relevantelementid;
                    standard.DISPATCHCODE = dispatchcode;
                    standard.PUBLISHDEPT = publishdept;
                    standard.STANDARDTYPE = standardtype;
                    standard.CATEGORYCODE = categorycode;
                    standard.CONSULTNUM = 0;
                    standard.ID = Guid.NewGuid().ToString();
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
                    try
                    {
                        standardsystembll.SaveForm(standard.ID, standard);
                    }
                    catch
                    {
                        error++;
                    }
                }
                count = dt.Rows.Count - 1;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }
            return message;
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
        /// ��Ӳ���Ƶ��
        /// </summary>
        /// <param name="keyValue"></param>
        [AjaxOnly]
        [HttpPost]
        public ActionResult AddConsultNum(string keyValue)
        {
            Operator user = OperatorProvider.Provider.Current();
            var data = standardsystembll.GetEntity(keyValue);
            data.CONSULTNUM += 1;
            standardsystembll.SaveForm(keyValue, data);
            var standardreadrecordentity = standardreadrecordbll.GetList("").Where(t => t.RecId == keyValue && t.CreateUserId == user.UserId).FirstOrDefault();
            standardreadrecordentity = standardreadrecordentity == null ? new StandardreadrecordEntity() : standardreadrecordentity;
            standardreadrecordentity.RecId = keyValue;
            standardreadrecordbll.SaveForm(standardreadrecordentity.ID, standardreadrecordentity);
            
            return Success("�����ɹ���");
        }


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

        #endregion
    }
}
