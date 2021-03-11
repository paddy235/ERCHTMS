using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// �� ����Ӧ��Ԥ��
    /// </summary>
    public class ReserverplanController : MvcControllerBase
    {
        private ReserverplanBLL reserverplanbll = new ReserverplanBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        #region ��ͼ����



        /// <summary>
        /// �ļ�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Files()
        {
            return View();
        }


        public ActionResult Select()
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
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        #endregion

        #region ��ȡ����


        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        //[HandlerMonitor(3, "��ҳ��ѯ�û���Ϣ!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,createuserdeptcode,createuserorgcode, Files,Name,PLANTYPENAME,ISAUDITNAME,DATATIME_BZ,DEPARTNAME_BZ,CREATEUSERNAME,USERID_BZ,DEPARTID_BZ,USERNAME_BZ,FILEPS,PLANTYPE";
            pagination.p_tablename = "(select * from MAE_RESERVERPLAN order by CREATEDATE,DATATIME_BZ) t";
            pagination.conditionJson = "1=1"; Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }

            var watch = CommonHelper.TimerStart();
            var data = reserverplanbll.GetPageList(pagination, queryJson);
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = reserverplanbll.GetList(queryJson);
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
            var data = reserverplanbll.GetEntity(keyValue);
            return ToJsonResult(data);
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
            foreach (var item in keyValue.Split(','))
            {
                reserverplanbll.RemoveForm(item);

            }
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
        public ActionResult SaveForm(string keyValue, ReserverplanEntity entity)
        {
            reserverplanbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion


        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "Ӧ��Ԥ��")]
        public ActionResult ExportReserverplanList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = " name, plantypename,departname_bz,username_bz,datatime_bz,isauditname";
            pagination.p_tablename = "V_MAE_RESERVERPLAN t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "ID";
            #region Ȩ��У��
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            #endregion
            var data = reserverplanbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "Ӧ��Ԥ��";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "Ӧ��Ԥ��.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "name", ExcelColumn = "Ӧ��Ԥ������", Width = 50 });
            listColumnEntity.Add(new ColumnEntity() { Column = "plantypename", ExcelColumn = "Ӧ��Ԥ������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "departname_bz", ExcelColumn = "���Ʋ���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "username_bz", ExcelColumn = "������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "datatime_bz", ExcelColumn = "����ʱ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "isauditname", ExcelColumn = "�Ƿ�����" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }

         [HandlerLogin(LoginMode.Ignore)]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "����Ӧ��Ԥ��")]
        public string ImportReserverplanData()
        {
            try
            {
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    return "��������Ա�޴˲���Ȩ��";
                }
                int error = 0;
                string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";

                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    count = 0;
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
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    var sheet = wb.Worksheets[0];
                    if (sheet.Cells[1, 0].StringValue != "Ӧ��Ԥ������" || sheet.Cells[1, 1].StringValue != "Ӧ��Ԥ������" 
                         || sheet.Cells[1, 2].StringValue != "���Ʋ���" || sheet.Cells[1, 4].StringValue != "����ʱ��"
                         || sheet.Cells[1, 5].StringValue != "��˲���" || sheet.Cells[1, 6].StringValue != "�����"
                         || sheet.Cells[1, 7].StringValue != "���ʱ��" || sheet.Cells[1, 8].StringValue != "��λ����"
                         || sheet.Cells[1, 9].StringValue != "�Ƿ�����" || sheet.Cells[1, 10].StringValue != "����")
                    {
                        return message;
                    }
                    for (int i = 2; i <= sheet.Cells.MaxDataRow; i++)
                    {
                        count ++;
                        if (string.IsNullOrWhiteSpace(sheet.Cells[i, 0].StringValue) || string.IsNullOrWhiteSpace(sheet.Cells[i, 1].StringValue)
                         || string.IsNullOrWhiteSpace(sheet.Cells[i, 2].StringValue) || string.IsNullOrWhiteSpace(sheet.Cells[i, 4].StringValue)
                         || string.IsNullOrWhiteSpace(sheet.Cells[i, 6].StringValue) || string.IsNullOrWhiteSpace(sheet.Cells[i, 6].StringValue)
                         || string.IsNullOrWhiteSpace(sheet.Cells[i, 7].StringValue) || string.IsNullOrWhiteSpace(sheet.Cells[i, 8].StringValue)
                         || string.IsNullOrWhiteSpace(sheet.Cells[i, 9].StringValue) || string.IsNullOrWhiteSpace(sheet.Cells[i, 10].StringValue))
                        {
                            falseMessage += "</br>" + "��" + (i + 1) + "��δ��ģ��Ҫ����д,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        ReserverplanEntity planEntity = new ReserverplanEntity();
                        planEntity.NAME = sheet.Cells[i, 0].StringValue;
                        planEntity.PLANTYPENAME = sheet.Cells[i, 1].StringValue;
                        var dataItem1 = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_PlanType'").Where(x => x.ItemName == sheet.Cells[i, 1].StringValue).ToList().FirstOrDefault();
                        if (dataItem1 != null) {
                            planEntity.PLANTYPE = dataItem1.ItemValue;
                        }
                        //���Ʋ���
                        var dept = new DepartmentBLL().GetList().Where(x => x.FullName == sheet.Cells[i, 2].StringValue).ToList().FirstOrDefault();
                        if (dept == null) {
                            falseMessage += "</br>" + "��" + (i + 1) + "�б��Ʋ��Ų���ȷ,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        planEntity.DEPARTID_BZ = dept.DepartmentId;
                        planEntity.DEPARTNAME_BZ = dept.FullName;
                        //������
                        planEntity.USERNAME_BZ = sheet.Cells[i, 3].StringValue;
                        //var userEntity = new UserBLL().GetList().Where(x => x.RealName == sheet.Cells[2, 3].StringValue).ToList().FirstOrDefault();
                        //if (userEntity != null) {
                        //    planEntity.USERID_BZ = userEntity.UserId;
                        //}
                        //��˲���
                        var auditDept = new DepartmentBLL().GetList().Where(x => x.FullName == sheet.Cells[i, 5].StringValue).ToList().FirstOrDefault();
                        if (auditDept == null)
                        {
                            falseMessage += "</br>" + "��" + (i + 1) + "����˲��Ų���ȷ,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        planEntity.DEPARTID_SH = auditDept.DepartmentId;
                        planEntity.DEPARTNAME_SH = auditDept.FullName;

                        planEntity.USERNAME_SH = sheet.Cells[i, 6].StringValue;
                        var userEntity1 = new UserBLL().GetList().Where(x => x.RealName == sheet.Cells[i, 6].StringValue).ToList().FirstOrDefault();
                        if (userEntity1 != null)
                        {
                            planEntity.USERID_SH = userEntity1.UserId;
                        }
                        DateTime s=new DateTime();
                        try
                        {
                           s= DateTime.Parse(sheet.Cells[i, 4].StringValue);
                            planEntity.DATATIME_BZ = s;
                        }
                        catch (Exception)
                        {

                            falseMessage += "</br>" + "��" + (i + 1) + "�б���ʱ���ʽ����ȷ,δ�ܵ���.";
                            error++;
                            continue;
                        }

                        try
                        {
                            s=DateTime.Parse(sheet.Cells[i, 7].StringValue);
                            planEntity.DATATIME_SH = s;
                        }
                        catch (Exception)
                        {

                            falseMessage += "</br>" + "��" + (i + 1) + "�����ʱ���ʽ����ȷ,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        planEntity.ORGXZTYPE = sheet.Cells[i, 8].StringValue == "��λ�ڲ�" ? 1 : 2;
                        planEntity.ORGXZNAME=sheet.Cells[i, 8].StringValue;
                       
                        planEntity.ISAUDITNAME = sheet.Cells[i, 9].StringValue;
                        var dataItem = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_IsAudit'").Where(x => x.ItemName == sheet.Cells[i, 9].StringValue).ToList().FirstOrDefault();
                        if (dataItem != null)
                        {
                            planEntity.ISAUDIT = dataItem.ItemValue;
                        }
                        planEntity.ID = Guid.NewGuid().ToString();
                        //�ļ�·��
                        string filepath = sheet.Cells[i, 10].StringValue;
                        var fileinfo = new FileInfo(decompressionDirectory + filepath);
                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        string fileguid = Guid.NewGuid().ToString();
                        fileInfoEntity.Create();
                        fileInfoEntity.RecId = planEntity.ID; //����ID
                        fileInfoEntity.FileName = filepath;
                        fileInfoEntity.FilePath = "~/Resource/Reserverplan/" + fileguid + fileinfo.Extension;
                        fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                        fileInfoEntity.FileExtensions = fileinfo.Extension;
                        fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                        TransportRemoteToServer(Server.MapPath("~/Resource/Reserverplan/"), decompressionDirectory + filepath, fileguid + fileinfo.Extension);
                        fileinfobll.SaveForm("", fileInfoEntity);
                        planEntity.FILES = fileInfoEntity.RecId;
                        try
                        {
                            reserverplanbll.SaveForm(planEntity.ID, planEntity);
                        }
                        catch
                        {
                            error++;
                        }
                       
                    }

                    message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "����";
                    if (error > 0)
                    {
                        message += "</br>" + falseMessage;
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
