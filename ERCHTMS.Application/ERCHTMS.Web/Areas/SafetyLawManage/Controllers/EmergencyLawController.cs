using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.SafetyLawManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using System.Web;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using System.IO;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// �� ����Ӧ��Ԥ������
    /// </summary>
    public class EmergencyLawController : MvcControllerBase
    {
        private EmergencyLawBLL emergencylawbll = new EmergencyLawBLL();
        private SafeInstitutionBLL safeinstitutionbll = new SafeInstitutionBLL();
        private AccidentCaseLawBLL accidentcaselawbll = new AccidentCaseLawBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private SafeStandardsBLL safestandardsbll = new SafeStandardsBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();

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
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = emergencylawbll.GetList(queryJson);
            return ToJsonResult(data);
        }

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
            pagination.p_fields = "CreateDate,FileName,EmergencyType,Source,Remark,FilesId,createuserid,createuserdeptcode,createuserorgcode";
            pagination.p_tablename = " bis_emergencylaw";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and (createuserid='" + user.UserId + "' or createuserorgcode='00')";
                            break;
                        case "2":
                            pagination.conditionJson += " and (createuserdeptcode='" + user.DeptCode + "' or createuserorgcode='00')";
                            break;
                        case "3":
                            pagination.conditionJson += " and (createuserdeptcode like'" + user.DeptCode + "%' or createuserorgcode='00')";
                            break;
                        case "4":
                            pagination.conditionJson += " and (createuserorgcode='" + user.OrganizeCode + "' or createuserorgcode='00')";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            var data = emergencylawbll.GetPageDataTable(pagination, queryJson);
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = emergencylawbll.GetEntity(keyValue);
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
            emergencylawbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, EmergencyLawEntity entity)
        {
            emergencylawbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion


        #region ����Ӧ��Ԥ������
        /// <summary>
        /// ����Ӧ��Ԥ������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportCase()
        {
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //�ļ�����
                    string filename = dt.Rows[i][0].ToString();
                    //Ӧ��Ԥ������
                    string ctype = dt.Rows[i][1].ToString();
                    //��Դ
                    string soure = dt.Rows[i][2].ToString();
                    //��ע
                    string remark = dt.Rows[i][3].ToString();
                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(ctype) || string.IsNullOrEmpty(soure))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    EmergencyLawEntity sl = new EmergencyLawEntity();
                    sl.FileName = filename;
                    if (ctype == "�ۺ�Ӧ��Ԥ��")
                        ctype = "1";
                    else if (ctype == "ר��Ӧ��Ԥ��")
                        ctype = "2";
                    else if (ctype == "�ֳ����÷���")
                        ctype = "3";
                    sl.EmergencyType = ctype;
                    sl.Source = soure;
                    sl.Remark = remark;
                    try
                    {
                        emergencylawbll.SaveForm("", sl);
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
        #endregion

        #region ���밲ȫ�����ƶ�
        /// <summary>
        /// ���밲ȫ�����ƶ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportInstitution()
        {
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //�ļ����ƣ����
                    string filename = dt.Rows[i][0].ToString();
                    //������λ
                    string iuusedept = dt.Rows[i][1].ToString();
                    //�ļ����
                    string filecode = dt.Rows[i][2].ToString();
                    //�����ƶ���𣨱��
                    string lawtype = dt.Rows[i][3].ToString();
                    //ʩ�����ڣ����
                    string carrydate = dt.Rows[i][4].ToString();
                    //��Ч�汾�ţ����
                    string validversions = dt.Rows[i][5].ToString();
                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(lawtype) || string.IsNullOrEmpty(carrydate) || string.IsNullOrEmpty(validversions))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    SafeInstitutionEntity sl = new SafeInstitutionEntity();
                    sl.FileName = filename;
                    sl.IssueDept = iuusedept;
                    sl.FileCode = filecode;
                    string itemvalue = dataitemdetailbll.GetItemValue(lawtype, "InstitutionLaw");
                    sl.LawTypeCode = itemvalue;
                    sl.ValidVersions = validversions;
                    try
                    {
                        sl.CarryDate = DateTime.Parse(DateTime.Parse(carrydate).ToString("yyyy-MM-dd"));
                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʱ������,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    try
                    {
                        safeinstitutionbll.SaveForm("", sl);
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
        #endregion

        #region �����¹ʰ���
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportAccidentCase()
        {
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;

                if (Directory.Exists(Server.MapPath("~/Resource/ht/images/channel")) == false)//��������ھʹ���file�ļ���
                {
                    Directory.CreateDirectory(Server.MapPath("~/Resource/ht/images/channel"));
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //�ļ�����
                    string filename = dt.Rows[i][0].ToString();
                    //�¹�ʱ��
                    string time = dt.Rows[i][1].ToString();
                    //��ע
                    string remark = dt.Rows[i][2].ToString();
                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(time))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    AccidentCaseLawEntity sl = new AccidentCaseLawEntity();
                    sl.FileName = filename;
                    sl.AccRange = "2";
                    sl.Remark = remark;
                    sl.FilesId = Guid.NewGuid().ToString();
                    FileInfoEntity fileEntity = new FileInfoEntity();
                    fileEntity.RecId = sl.FilesId;
                    fileEntity.EnabledMark = 1;
                    fileEntity.DeleteMark = 0;
                    fileEntity.FilePath = "~/Resource/ht/images/channel/" + filename;
                    fileEntity.FileName = sl.FileName;
                    fileEntity.FolderId = "ht/images";
                    try
                    {
                        sl.AccTime = DateTime.Parse(DateTime.Parse(time).ToString("yyyy-MM-dd HH:mm"));
                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʱ������,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    try
                    {
                        accidentcaselawbll.SaveForm("", sl);
                        fileInfoBLL.SaveForm("", fileEntity);
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
        #endregion

        #region ���밲ȫ�������
        /// <summary>
        /// ���밲ȫ�������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportStandards()
        {
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //�ļ����ƣ����
                    string filename = dt.Rows[i][0].ToString();
                    //������λ
                    string iuusedept = dt.Rows[i][1].ToString();
                    //�ļ����
                    string filecode = dt.Rows[i][2].ToString();
                    //��λ��𣨱��
                    string lawtype = dt.Rows[i][3].ToString();
                    //ʩ�����ڣ����
                    string carrydate = dt.Rows[i][4].ToString();
                    //��Ч�汾�ţ����
                    string validversions = dt.Rows[i][5].ToString();
                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(lawtype) || string.IsNullOrEmpty(carrydate) || string.IsNullOrEmpty(validversions))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    SafeStandardsEntity sl = new SafeStandardsEntity();
                    sl.FileName = filename;
                    sl.IssueDept = iuusedept;
                    sl.FileCode = filecode;
                    string itemvalue = dataitemdetailbll.GetItemValue(lawtype, "StandardsLaw");
                    sl.LawTypeCode = itemvalue;
                    sl.ValidVersions = validversions;
                    try
                    {
                        sl.CarryDate = DateTime.Parse(DateTime.Parse(carrydate).ToString("yyyy-MM-dd"));
                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʱ������,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    try
                    {
                        safestandardsbll.SaveForm("", sl);
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
        #endregion

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
                pagination.p_fields = "FileName,Source,EmergencyType,Remark";
                pagination.p_tablename = " bis_emergencylaw";
                pagination.conditionJson = "1=1";
                pagination.sidx = "createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and (createuserid='" + user.UserId + "' or createuserorgcode='00')";
                                break;
                            case "2":
                                pagination.conditionJson += " and (createuserdeptcode='" + user.DeptCode + "' or createuserorgcode='00')";
                                break;
                            case "3":
                                pagination.conditionJson += " and (createuserdeptcode like'" + user.DeptCode + "%' or createuserorgcode='00')";
                                break;
                            case "4":
                                pagination.conditionJson += " and (createuserorgcode='" + user.OrganizeCode + "' or createuserorgcode='00')";
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                DataTable exportTable = emergencylawbll.GetPageDataTable(pagination, queryJson);
                foreach (DataRow item in exportTable.Rows)
                {
                    var ctype = item["EmergencyType"].ToString();
                    if (ctype == "1")
                        ctype = "�ۺ�Ӧ��Ԥ��";
                    else if (ctype == "2")
                        ctype = "ר��Ӧ��Ԥ��";
                    else if (ctype == "3")
                        ctype = "�ֳ����÷���";
                    item["EmergencyType"] = ctype;
                }
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "Ӧ��Ԥ��������Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "Ӧ��Ԥ����������.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "�ļ�����������", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "source", ExcelColumn = "��Դ", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "emergencytype", ExcelColumn = "Ӧ��Ԥ������", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "remark", ExcelColumn = "��ע", Width = 60 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        #endregion
    }
}
