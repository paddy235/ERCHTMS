using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.FireManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.FireManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// �� ����������ʩ
    /// </summary>
    public class FirefightingController : MvcControllerBase
    {
        private FirefightingBLL firefightingbll = new FirefightingBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private readonly DepartmentBLL departBLL = new DepartmentBLL();
        private readonly UserBLL userBLL = new UserBLL();
        private readonly OrganizeBLL orgBLL = new OrganizeBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
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
        /// ���ɶ�ά��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImage()
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

        /// <summary>
        /// ���������ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MHQForm()
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
            queryJson = queryJson ?? "";
            pagination.p_kid = "Id";
            pagination.p_fields = "EquipmentName,EquipmentNameNo,EquipmentCode,ExtinguisherType,ExtinguisherTypeNo,Specifications,District,DutyUser,dutyuserid,DutyDept,NextExamineDate,NextDetectionDate,NextFillDate,createuserid,createuserdeptcode,createuserorgcode,CreateDate,ExamineUser,ExamineUserId,TerminalDetectionDate,TerminalDetectionUnit,TerminalDetectionVerdict,TerminalDetectionPeriod,NextTerminalDetectionDate";
            pagination.p_tablename = "HRS_FIREFIGHTING";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            System.Diagnostics.Stopwatch watch = CommonHelper.TimerStart();
            DataTable data = firefightingbll.GetPageList(pagination, queryJson);
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
            System.Collections.Generic.IEnumerable<FirefightingEntity> data = firefightingbll.GetList(queryJson);
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
            FirefightingEntity data = firefightingbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡͳ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StatisticsData(string queryJson)
        {
            DataTable data = firefightingbll.StatisticsData(queryJson);
            var jsonData = new
            {
                rows = data
            };
            return ToJsonResult(jsonData);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ����ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Remove(string keyValue)
        {
            firefightingbll.Remove(keyValue);
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
        public ActionResult RemoveForm(string keyValue)
        {
            firefightingbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FirefightingEntity entity)
        {
            firefightingbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        /// <summary>
        /// ��Ų����ظ�
        /// </summary>
        /// <param name="Type">��ʩ����</param>
        /// <param name="EquipmentCode">���</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistCode(string Type, string EquipmentCode, string keyValue)
        {
            bool IsOk = firefightingbll.ExistCode(Type, EquipmentCode, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// ���ݸ���
        /// </summary>
        /// <param name="Type">��ʩ����</param>
        /// <param name="EquipmentCode">���</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DataCopy(string keyValue)
        {
            FirefightingEntity data = firefightingbll.GetEntity(keyValue);
            data.Id = null;
            data.EquipmentCode = null;
            firefightingbll.SaveForm(data.Id, data);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// �������ɶ�ά�벢������word
        /// </summary>
        /// <param name="equipIds"></param>
        /// <param name="equipNames"></param>
        /// <param name="equipNos"></param>
        /// <param name="equiptype"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImg(string equipIds, string equipNames, string equipNos, string equiptype)
        {
            if (!System.IO.Directory.Exists(Server.MapPath("~/Resource/qrcode")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Resource/qrcode"));
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/������ʩ��ά���ӡ.doc"));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            DataTable dt = new DataTable("U");
            dt.Columns.Add("BigEWM2");
            dt.Columns.Add("PersonName");
            dt.Columns.Add("PersonNo");
            int i = 0;
            string fileName = "";
            string[] equipNameArr = equipNames.Split(',');
            string[] equipNoArr = equipNos.Split(',');
            foreach (string code in equipIds.Split(','))
            {
                DataRow dr = dt.NewRow();
                dr[1] = equipNameArr[i];
                dr[2] = equipNoArr[i];

                fileName = code + ".jpg";
                string filePath = Server.MapPath("~/Resource/qrcode/" + fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    BuilderImg10(code, filePath, equiptype);
                }
                dr[0] = Server.MapPath("~/Resource/qrcode/" + fileName);
                dt.Rows.Add(dr);
                i++;
            }

            doc.MailMerge.Execute(dt);
            fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            doc.Save(Server.MapPath("~/Resource/temp/" + fileName));
            return Success("���ɳɹ�", new { fileName = fileName });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="filePath"></param>
        /// <param name="equiptype"></param>
        public void BuilderImg10(string keyValue, string filePath, string equiptype)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 10;
            qrCodeEncoder.QRCodeScale = 2;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            System.Drawing.Image image = qrCodeEncoder.Encode(keyValue + "|" + equiptype, Encoding.UTF8);
            image.Save(filePath, ImageFormat.Jpeg);
            image.Dispose();
        }


        /// <summary>
        /// ������ʩ����
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        //[HandlerMonitor(0, "����������ʩexcel")]
        public ActionResult ExportFirefightingExcel(string condition, string queryJson)
        {
            string colunmStr = @"EquipmentName,EquipmentNameNo,EquipmentCode,ExtinguisherType,ExtinguisherTypeNo,Specifications,District,DutyDept,DutyUser,
to_char(NextExamineDate,'yyyy-MM-dd') as NextExamineDate,
to_char(NextDetectionDate,'yyyy-MM-dd') as NextDetectionDate,
to_char(NextFillDate,'yyyy-MM-dd') as NextFillDate,
to_char(NextTerminalDetectionDate,'yyyy-MM-dd') as NextTerminalDetectionDate";

            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();

                //��ѯ���� ����
                if (!queryParam["EquipmentNameNo"].IsEmpty())
                {
                    if (queryParam["EquipmentNameNo"].ToString() == "MHQ")
                    {
                        colunmStr = @"EquipmentName,EquipmentNameNo,EquipmentCode,ExtinguisherType,ExtinguisherTypeNo,Specifications,District,DutyDept,DutyUser,
to_char(NextExamineDate,'yyyy-MM-dd') as NextExamineDate,
to_char(NextFillDate,'yyyy-MM-dd') as NextFillDate,
to_char(NextDetectionDate,'yyyy-MM-dd') as NextDetectionDate";
                    }
                    else {
                        colunmStr = @"EquipmentName,EquipmentNameNo,EquipmentCode,ExtinguisherType,ExtinguisherTypeNo,Specifications,District,DutyDept,DutyUser,
to_char(NextExamineDate,'yyyy-MM-dd') as NextExamineDate,
to_char(NextDetectionDate,'yyyy-MM-dd') as NextDetectionDate,
to_char(NextTerminalDetectionDate,'yyyy-MM-dd') as NextTerminalDetectionDate";
                    }
                }
            }

            Pagination pagination = new Pagination
            {
                page = 1,
                rows = 100000000,
                p_kid = "Id",
                p_fields = colunmStr,
                p_tablename = "HRS_FIREFIGHTING",
                conditionJson = "1=1",
                sidx = "CreateDate",
                sord = "desc"
            };
            DataTable data = firefightingbll.GetPageList(pagination, queryJson);


            //���õ�����ʽ
            //ExcelConfig excelconfig = new ExcelConfig
            //{
            //    Title = "������ʩ",
            //    TitleFont = "΢���ź�",
            //    TitlePoint = 25,
            //    FileName = "������ʩ" + ".xls",
            //    IsAllSizeColumn = true,
            //    //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            //    ColumnEntity = new List<ColumnEntity>()
            //{
            //    new ColumnEntity() {Column = "equipmentname", ExcelColumn = "�豸����", Alignment = "center"},
            //    new ColumnEntity() {Column = "equipmentcode", ExcelColumn = "�豸���", Alignment = "center"},
            //    new ColumnEntity() {Column = "extinguishertype", ExcelColumn = "����", Alignment = "center"},
            //    new ColumnEntity() {Column = "specifications", ExcelColumn = "����ͺ�", Alignment = "center"},
            //    new ColumnEntity() {Column = "district", ExcelColumn = "��������", Alignment = "center"},
            //    new ColumnEntity() {Column = "dutydept", ExcelColumn = "���β���", Alignment = "center"},
            //    new ColumnEntity() {Column = "dutyuser", ExcelColumn = "������",  Alignment = "center"},
            //    new ColumnEntity() {Column = "nextexaminedate", ExcelColumn = "�´μ��ʱ��", Alignment = "center"},
            //    new ColumnEntity() {Column = "nextfilldate", ExcelColumn = "�´γ�װ/����ʱ��", Alignment = "center"},
            //    new ColumnEntity() {Column = "nextdetectiondate", ExcelColumn = "�´μ��ʱ��", Alignment = "center"}
            //}
            //};

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "������ʩ";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "������ʩ.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "equipmentname", ExcelColumn = "�豸����", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "equipmentcode", ExcelColumn = "�豸���", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "extinguishertype", ExcelColumn = "����", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "����ͺ�", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "��������", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "dutydept", ExcelColumn = "���β���", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "dutyuser", ExcelColumn = "������", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "nextexaminedate", ExcelColumn = "�´μ��ʱ��", Alignment = "center" });

            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();

                //��ѯ���� ����
                if (!queryParam["EquipmentNameNo"].IsEmpty())
                {
                    if (queryParam["EquipmentNameNo"].ToString() == "MHQ")
                    {
                        listColumnEntity.Add(new ColumnEntity() { Column = "nextfilldate", ExcelColumn = "�´γ�װ/����ʱ��", Alignment = "center" });
                    }
                    else
                    {
                        listColumnEntity.Add(new ColumnEntity() { Column = "nextdetectiondate", ExcelColumn = "�´�ά��ʱ��", Alignment = "center" });
                        listColumnEntity.Add(new ColumnEntity() { Column = "nextterminaldetectiondate", ExcelColumn = "�´μ��ʱ��", Alignment = "center" });
                    }
                }
                else
                {
                    listColumnEntity.Add(new ColumnEntity() { Column = "nextdetectiondate", ExcelColumn = "�´�ά��ʱ��", Alignment = "center" });
                    listColumnEntity.Add(new ColumnEntity() { Column = "nextfilldate", ExcelColumn = "�´γ�װ/����ʱ��", Alignment = "center" });
                    listColumnEntity.Add(new ColumnEntity() { Column = "nextterminaldetectiondate", ExcelColumn = "�´μ��ʱ��", Alignment = "center" });
                }
            }
            else
            {
                listColumnEntity.Add(new ColumnEntity() { Column = "nextdetectiondate", ExcelColumn = "�´�ά��ʱ��", Alignment = "center" });
                listColumnEntity.Add(new ColumnEntity() { Column = "nextfilldate", ExcelColumn = "�´γ�װ/����ʱ��", Alignment = "center" });
                listColumnEntity.Add(new ColumnEntity() { Column = "nextterminaldetectiondate", ExcelColumn = "�´μ��ʱ��", Alignment = "center" });
            }
            
            
            

            excelconfig.ColumnEntity = listColumnEntity;
            
            //���õ�������
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, listColumnEntity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ����������ʩ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        // [ValidateAntiForgeryToken]
        public string ImportFirefighting()
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "��������Ա�޴˲���Ȩ��";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾          
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
                //string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                //file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                //DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));

                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow+1, cells.MaxColumn+1, true);

                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                int order = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FirefightingEntity item = new FirefightingEntity();
                    order = i + 1;
                    #region �豸����
                    string equipmentName = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(equipmentName))
                    {
                        item.EquipmentName = equipmentName;
                        var data = new DataItemCache().ToItemValue("EquipmentName", equipmentName);
                        if (data != null && !string.IsNullOrEmpty(data))
                            item.EquipmentNameNo = data;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�豸���Ʋ����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�豸���Ʋ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    if (item.EquipmentNameNo == "MHQ")
                    {
                        #region �豸���
                        string equipmentNo = dt.Rows[i][1].ToString();
                        if (!string.IsNullOrEmpty(equipmentNo))
                            item.EquipmentCode = equipmentNo;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�豸��Ų���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ����
                        string extinguisherType = dt.Rows[i][2].ToString();
                        if (!string.IsNullOrEmpty(extinguisherType))
                        {
                            item.ExtinguisherType = extinguisherType;
                            if (equipmentName == "�����")
                            {
                                var data = new DataItemCache().ToItemValue("ExtinguisherType", extinguisherType);
                                if (data != null && !string.IsNullOrEmpty(data))
                                    item.ExtinguisherTypeNo = data;
                                else
                                {
                                    falseMessage += string.Format(@"��{0}�е���ʧ��,��������Ͳ����ڣ�</br>", order);
                                    error++;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���Ͳ���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ����
                        string Amount = dt.Rows[i][3].ToString();
                        int tempAmount;
                        if (!string.IsNullOrEmpty(Amount))
                            if (int.TryParse(Amount, out tempAmount))
                                item.Amount = tempAmount;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,��������Ϊ���֣�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,��������Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ������λ
                        string AmountUnit = dt.Rows[i][4].ToString();
                        if (!string.IsNullOrEmpty(AmountUnit))
                        {
                            var data = new DataItemCache().ToItemValue("AmountUnit", AmountUnit);
                            if (data != null && !string.IsNullOrEmpty(data))
                                item.AmountUnit = data;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,������λ�����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,������λ����Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ����ͺ�
                        string specifications = dt.Rows[i][3 + 2].ToString();
                        if (!string.IsNullOrEmpty(specifications))
                            item.Specifications = specifications;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,����ͺŲ���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ����ʱ��
                        string leaveDate = dt.Rows[i][4 + 2].ToString();
                        DateTime tempDate1;
                        if (!string.IsNullOrEmpty(leaveDate))
                            if (DateTime.TryParse(leaveDate, out tempDate1))
                                item.LeaveDate = tempDate1;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,����ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,����ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ������ڣ��죩
                        string examinePeriod = dt.Rows[i][5 + 2].ToString();
                        int tempPeriod;
                        if (!string.IsNullOrEmpty(examinePeriod))
                            if (int.TryParse(examinePeriod, out tempPeriod))
                                item.ExaminePeriod = tempPeriod;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,������ڱ���Ϊ���֣�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,������ڲ���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ���ʱ��
                        string examineDate = dt.Rows[i][6 + 2].ToString();
                        DateTime tempDate2;
                        if (!string.IsNullOrEmpty(examineDate))
                            if (DateTime.TryParse(examineDate, out tempDate2))
                                item.ExamineDate = tempDate2;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,���ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �´μ��ʱ�䣨�����գ�
                        string nextExamineDate = dt.Rows[i][7 + 2].ToString();
                        DateTime tempDate3;
                        if (!string.IsNullOrEmpty(nextExamineDate))
                            if (DateTime.TryParse(nextExamineDate, out tempDate3))
                                item.NextExamineDate = tempDate3;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�´μ��ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�´μ��ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ������ڣ��죩
                        string fillPeriod = dt.Rows[i][8 + 2].ToString();
                        int tempFillPeriod;
                        if (!string.IsNullOrEmpty(fillPeriod))
                            if (int.TryParse(fillPeriod, out tempFillPeriod))
                                item.FillPeriod = tempFillPeriod;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,��װ/�������ڱ���Ϊ���֣�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,��װ/�������ڲ���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �ϴγ�װʱ�䣨���£�   new CultureInfo("zh-CN")
                        string lastFillDate = dt.Rows[i][9 + 2].ToString();
                        DateTime tempDate4;
                        if (!string.IsNullOrEmpty(lastFillDate))
                            if (DateTime.TryParse(lastFillDate, out tempDate4))
                                item.LastFillDate = tempDate4;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�ϴγ�װ/����ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�ϴγ�װ/����ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �´γ�װʱ�䣨���£�
                        string nextFillDate = dt.Rows[i][10 + 2].ToString();
                        DateTime tempDate5;
                        if (!string.IsNullOrEmpty(nextFillDate))
                            // if (DateTime.TryParseExact(nextFillDate, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime tempDate))
                            if (DateTime.TryParse(nextFillDate, out tempDate5))
                                item.NextFillDate = tempDate5;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�´γ�װ/����ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�´γ�װ/����ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ��������
                        string district = dt.Rows[i][11 + 2].ToString();
                        if (!string.IsNullOrEmpty(district))
                        {
                            var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == district && x.OrganizeId == orgid).FirstOrDefault();
                            if (disItem != null)
                            {
                                item.DistrictId = disItem.DistrictID;
                                item.DistrictCode = disItem.DistrictCode;
                                item.District = district;
                            }
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�������򲻴��ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,����������Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ����λ��
                        string location = dt.Rows[i][12 + 2].ToString();
                        if (!string.IsNullOrEmpty(location))
                            item.Location = location;
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,����λ�ò���Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region ���β���
                        string dutydept = dt.Rows[i][13 + 2].ToString();
                        if (!string.IsNullOrEmpty(dutydept))
                        {
                            var data = departBLL.GetList().FirstOrDefault(e => e.FullName == dutydept && e.OrganizeId == orgid);
                            if (data != null)
                            {
                                item.DutyDept = dutydept;
                                item.DutyDeptCode = data.EnCode;
                            }

                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,���β��Ų����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���β��Ų���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ������
                        string dutyUser = dt.Rows[i][14 + 2].ToString();
                        if (!string.IsNullOrEmpty(dutyUser))
                        {
                            var userEntity = userBLL.GetListForCon(x => x.RealName == dutyUser && x.OrganizeId == orgid).FirstOrDefault();
                            if (userEntity != null)
                            {
                                item.DutyUserId = userEntity.UserId;
                                item.DutyUser = dutyUser;
                            }
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �����˵绰
                        string dutyTel = dt.Rows[i][15 + 2].ToString();
                        if (!string.IsNullOrEmpty(dutyTel))
                        {
                            item.DutyTel = dutyTel;
                        }
                        #endregion

                        #region ��鲿��
                        string examineDept = dt.Rows[i][16 + 2].ToString();
                        if (!string.IsNullOrEmpty(examineDept))
                        {
                            var data = departBLL.GetList().FirstOrDefault(e => e.FullName == examineDept && e.OrganizeId == orgid);
                            if (data != null)
                            {
                                item.ExamineDept = examineDept;
                                item.ExamineDeptCode = data.EnCode;
                            }

                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,��鲿�Ų����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,��鲿�Ų���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �����
                        string examineUser = dt.Rows[i][17 + 2].ToString();
                        if (!string.IsNullOrEmpty(examineUser))
                        {
                            var userEntity = userBLL.GetListForCon(x => x.RealName == examineUser && x.OrganizeId == orgid).FirstOrDefault();
                            if (userEntity != null)
                            {
                                item.ExamineUserId = userEntity.UserId;
                                item.ExamineUser = examineUser;
                            }
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,����˲����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,����˲���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ��ע
                        string remark = dt.Rows[i][18 + 2].ToString();
                        if (!string.IsNullOrEmpty(remark))
                        {
                            item.Remark = remark;
                        }
                        #endregion
                    }
                    else
                    {
                        #region �豸���
                        string equipmentNo = dt.Rows[i][1].ToString();
                        if (!string.IsNullOrEmpty(equipmentNo))
                            item.EquipmentCode = equipmentNo;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�豸��Ų���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ����
                        string Amount = dt.Rows[i][2].ToString();
                        int tempAmount;
                        if (!string.IsNullOrEmpty(Amount))
                            if (int.TryParse(Amount, out tempAmount))
                                item.Amount = tempAmount;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,��������Ϊ���֣�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,��������Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        int index = 0;
                        if (item.EquipmentNameNo == "SNXHS" || item.EquipmentNameNo == "SWXHS") {
                            index = 1;
                        }

                        #region ����
                        string extinguisherType = dt.Rows[i][2 + index].ToString();
                        if (!string.IsNullOrEmpty(extinguisherType))
                        {
                            item.ExtinguisherType = extinguisherType;
                            var data = new DataItemCache().ToItemValue("ExtinguisherType", extinguisherType);
                            if (data != null && !string.IsNullOrEmpty(data))
                                item.ExtinguisherTypeNo = data;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,���Ͳ����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        #endregion

                        #region ������λ
                        string AmountUnit = dt.Rows[i][3 + index].ToString();
                        if (!string.IsNullOrEmpty(AmountUnit))
                        {
                            var data = new DataItemCache().ToItemValue("AmountUnit", AmountUnit);
                            if (data != null && !string.IsNullOrEmpty(data))
                                item.AmountUnit = data;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,������λ�����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,������λ����Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ����ͺ�
                        string specifications = dt.Rows[i][2 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(specifications))
                            item.Specifications = specifications;
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,����ͺŲ���Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region ���β���
                        string dutydept = dt.Rows[i][3 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(dutydept))
                        {
                            var data = departBLL.GetList().FirstOrDefault(e => e.FullName == dutydept && e.OrganizeId == orgid);
                            if (data != null)
                            {
                                item.DutyDept = dutydept;
                                item.DutyDeptCode = data.EnCode;
                            }

                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,���β��Ų����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���β��Ų���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ������
                        string dutyUser = dt.Rows[i][4 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(dutyUser))
                        {
                            var userEntity = userBLL.GetListForCon(x => x.RealName == dutyUser && x.OrganizeId == orgid).FirstOrDefault();
                            if (userEntity != null)
                            {
                                item.DutyUserId = userEntity.UserId;
                                item.DutyUser = dutyUser;
                            }
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �����˵绰
                        string dutyTel = dt.Rows[i][5 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(dutyTel))
                        {
                            item.DutyTel = dutyTel;
                        }
                        #endregion

                        #region ��鲿��
                        string examineDept = dt.Rows[i][6 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(examineDept))
                        {
                            var data = departBLL.GetList().FirstOrDefault(e => e.FullName == examineDept && e.OrganizeId == orgid);
                            if (data != null)
                            {
                                item.ExamineDept = examineDept;
                                item.ExamineDeptCode = data.EnCode;
                            }

                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,��鲿�Ų����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,��鲿�Ų���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �����
                        string examineUser = dt.Rows[i][7 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(examineUser))
                        {
                            var userEntity = userBLL.GetListForCon(x => x.RealName == examineUser && x.OrganizeId == orgid).FirstOrDefault();
                            if (userEntity != null)
                            {
                                item.ExamineUserId = userEntity.UserId;
                                item.ExamineUser = examineUser;
                            }
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,����˲����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,����˲���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ��������
                        string district = dt.Rows[i][8 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(district))
                        {
                            var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == district && x.OrganizeId == orgid).FirstOrDefault();
                            if (disItem != null)
                            {
                                item.DistrictId = disItem.DistrictID;
                                item.DistrictCode = disItem.DistrictCode;
                                item.District = district;
                            }
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�������򲻴��ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,����������Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ����λ��
                        string location = dt.Rows[i][9 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(location))
                            item.Location = location;
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,����λ�ò���Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        string examineDate = null;
                        string examinePeriod = null;
                        string nextExamineDate = null;
                        string protectobject = null;
                        string mainparameter = null;
                        string designunit = null;
                        string installunit = null;
                        string donedate = null;
                        string safeguardunit = null;
                        string detectionunit = null;
                        string detectionDate = null;
                        string detectionVerdict = null;
                        string detectionPeriod = null;
                        string nextDetectionDate = null;
                        string terminalDetectionunit = null;
                        string terminalDetectionDate = null;
                        string terminalDetectionVerdict = null;
                        string terminalDetectionPeriod = null;
                        string nextTerminalDetectionDate = null;
                        string remark = null;
                        if (item.EquipmentNameNo == "SNXHS" || item.EquipmentNameNo == "SWXHS")
                        {

                            #region ǹͷ��
                            string spearhead = dt.Rows[i][10 + 2 + index].ToString();
                            int tempSpearhead;
                            if (!string.IsNullOrEmpty(spearhead))
                                if (int.TryParse(spearhead, out tempSpearhead))
                                    item.Spearhead = tempSpearhead;
                                else
                                {
                                    falseMessage += string.Format(@"��{0}�е���ʧ��,ǹͷ������Ϊ���֣�</br>", order);
                                    error++;
                                    continue;
                                }
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,ǹͷ������Ϊ�գ�</br>", order);
                                error++;
                                continue;
                            }
                            #endregion

                            #region ˮ����
                            string waterbelt = dt.Rows[i][11 + 2 + index].ToString();
                            int tempWaterBelt;
                            if (!string.IsNullOrEmpty(waterbelt))
                                if (int.TryParse(waterbelt, out tempWaterBelt))
                                    item.WaterBelt = tempWaterBelt;
                                else
                                {
                                    falseMessage += string.Format(@"��{0}�е���ʧ��,ˮ��������Ϊ���֣�</br>", order);
                                    error++;
                                    continue;
                                }
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,ˮ��������Ϊ�գ�</br>", order);
                                error++;
                                continue;
                            }
                            #endregion

                            examineDate = dt.Rows[i][12 + 2 + index].ToString();
                            examinePeriod = dt.Rows[i][13 + 2 + index].ToString();
                            nextExamineDate = dt.Rows[i][14 + 2 + index].ToString();
                            protectobject = dt.Rows[i][15 + 2 + index].ToString();
                            mainparameter = dt.Rows[i][16 + 2 + index].ToString();
                            designunit = dt.Rows[i][17 + 2 + index].ToString();
                            installunit = dt.Rows[i][18 + 2 + index].ToString();
                            donedate = dt.Rows[i][19 + 2 + index].ToString();
                            safeguardunit = dt.Rows[i][20 + 2 + index].ToString();
                            detectionunit = dt.Rows[i][21 + 2 + index].ToString();
                            detectionDate = dt.Rows[i][22 + 2 + index].ToString();
                            detectionVerdict = dt.Rows[i][23 + 2 + index].ToString();
                            detectionPeriod = dt.Rows[i][24 + 2 + index].ToString();
                            nextDetectionDate = dt.Rows[i][25 + 2 + index].ToString();
                            terminalDetectionunit = dt.Rows[i][26 + 2 + index].ToString();
                            terminalDetectionDate = dt.Rows[i][27 + 2 + index].ToString();
                            terminalDetectionVerdict = dt.Rows[i][28 + 2 + index].ToString();
                            terminalDetectionPeriod = dt.Rows[i][29 + 2 + index].ToString();
                            nextTerminalDetectionDate = dt.Rows[i][30 + 2 + index].ToString();
                            remark = dt.Rows[i][31 + 2 + index].ToString();
                        }
                        else
                        {
                            examineDate = dt.Rows[i][10 + 2 + index].ToString();
                            examinePeriod = dt.Rows[i][11 + 2 + index].ToString();
                            nextExamineDate = dt.Rows[i][12 + 2 + index].ToString();
                            protectobject = dt.Rows[i][13 + 2 + index].ToString();
                            mainparameter = dt.Rows[i][14 + 2 + index].ToString();
                            designunit = dt.Rows[i][15 + 2 + index].ToString();
                            installunit = dt.Rows[i][16 + 2 + index].ToString();
                            donedate = dt.Rows[i][17 + 2 + index].ToString();
                            safeguardunit = dt.Rows[i][18 + 2 + index].ToString();
                            detectionunit = dt.Rows[i][19 + 2 + index].ToString();
                            detectionDate = dt.Rows[i][20 + 2 + index].ToString();
                            detectionVerdict = dt.Rows[i][21 + 2 + index].ToString();
                            detectionPeriod = dt.Rows[i][22 + 2 + index].ToString();
                            nextDetectionDate = dt.Rows[i][23 + 2 + index].ToString();
                            terminalDetectionunit = dt.Rows[i][24 + 2 + index].ToString();
                            terminalDetectionDate = dt.Rows[i][25 + 2 + index].ToString();
                            terminalDetectionVerdict = dt.Rows[i][26 + 2 + index].ToString();
                            terminalDetectionPeriod = dt.Rows[i][27 + 2 + index].ToString();
                            nextTerminalDetectionDate = dt.Rows[i][28 + 2 + index].ToString();
                            remark = dt.Rows[i][29 + 2 + index].ToString();
                        }

                        #region ���ʱ��
                        //string examineDate = dt.Rows[i][10].ToString();
                        DateTime tempDate2;
                        if (!string.IsNullOrEmpty(examineDate))
                            if (DateTime.TryParse(examineDate, out tempDate2))
                                item.ExamineDate = tempDate2;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,���ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ������ڣ��죩
                        //string examinePeriod = dt.Rows[i][11].ToString();
                        int tempPeriod;
                        if (!string.IsNullOrEmpty(examinePeriod))
                            if (int.TryParse(examinePeriod, out tempPeriod))
                                item.ExaminePeriod = tempPeriod;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,������ڱ���Ϊ���֣�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,������ڲ���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �´μ��ʱ�䣨�����գ�
                        //string nextExamineDate = dt.Rows[i][12].ToString();
                        DateTime tempDate3;
                        if (!string.IsNullOrEmpty(nextExamineDate))
                            if (DateTime.TryParse(nextExamineDate, out tempDate3))
                                item.NextExamineDate = tempDate3;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�´μ��ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�´μ��ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ��������
                        //string protectobject = dt.Rows[i][13].ToString();
                        if (!string.IsNullOrEmpty(protectobject))
                            item.ProtectObject = protectobject;
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,����������Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region ��Ҫ����
                        //string mainparameter = dt.Rows[i][14].ToString();
                        if (!string.IsNullOrEmpty(mainparameter))
                            item.MainParameter = mainparameter;
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,��Ҫ��������Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region ��Ƶ�λ
                        //string designunit = dt.Rows[i][15].ToString();
                        if (!string.IsNullOrEmpty(designunit))
                            item.DesignUnit = designunit;
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,��Ƶ�λ����Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region ��װ��λ
                        //string installunit = dt.Rows[i][16].ToString();
                        if (!string.IsNullOrEmpty(installunit))
                            item.InstallUnit = installunit;
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,��װ��λ����Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region ����ʱ��
                        //string donedate = dt.Rows[i][17].ToString();
                        DateTime tempDoneDate;
                        if (!string.IsNullOrEmpty(donedate))
                            if (DateTime.TryParse(donedate, out tempDoneDate))
                                item.DoneDate = tempDoneDate;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,����ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,����ʱ�䲻��Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region ά����λ
                        //string safeguardunit = dt.Rows[i][18].ToString();
                        if (!string.IsNullOrEmpty(safeguardunit))
                            item.SafeguardUnit = safeguardunit;
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,ά����λ����Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region ά����λ
                        //string detectionunit = dt.Rows[i][19].ToString();
                        if (!string.IsNullOrEmpty(detectionunit))
                            item.DetectionUnit = detectionunit;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,ά����λ����Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ά��ʱ��
                        //string detectionDate = dt.Rows[i][20].ToString();
                        DateTime tempDetectionDate;
                        if (!string.IsNullOrEmpty(detectionDate))
                            if (DateTime.TryParse(detectionDate, out tempDetectionDate))
                                item.DetectionDate = tempDetectionDate;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,ά��ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,ά��ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ά������
                        //string detectionVerdict = dt.Rows[i][21].ToString();
                        if (!string.IsNullOrEmpty(detectionVerdict))
                        {
                            if (detectionVerdict == "�ϸ�") item.DetectionVerdict = 1;
                            else if (detectionVerdict == "���ϸ�") item.DetectionVerdict = 0;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,ά�����۲����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,ά�����۲���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ά�����ڣ��죩
                        //string detectionPeriod = dt.Rows[i][22].ToString();
                        int tempDetectionPeriod;
                        if (!string.IsNullOrEmpty(detectionPeriod))
                            if (int.TryParse(detectionPeriod, out tempDetectionPeriod))
                                item.DetectionPeriod = tempDetectionPeriod;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,ά�����ڱ���Ϊ���֣�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,ά�����ڲ���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �´�ά��ʱ�䣨�����գ�
                        //string nextDetectionDate = dt.Rows[i][23].ToString();
                        DateTime tempNextDetectionDate;
                        if (!string.IsNullOrEmpty(nextDetectionDate))
                            if (DateTime.TryParse(nextDetectionDate, out tempNextDetectionDate))
                                item.NextDetectionDate = tempNextDetectionDate;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�´�ά��ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�´�ά��ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ��ⵥλ
                        //string detectionunit = dt.Rows[i][19].ToString();
                        if (!string.IsNullOrEmpty(terminalDetectionunit))
                            item.TerminalDetectionUnit = terminalDetectionunit;
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,��ⵥλ����Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region ���ʱ��
                        //string detectionDate = dt.Rows[i][20].ToString();
                        DateTime tempTerminalDetectionDate;
                        if (!string.IsNullOrEmpty(terminalDetectionDate))
                            if (DateTime.TryParse(terminalDetectionDate, out tempTerminalDetectionDate))
                                item.TerminalDetectionDate = tempTerminalDetectionDate;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,���ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ������
                        //string detectionVerdict = dt.Rows[i][21].ToString();
                        if (!string.IsNullOrEmpty(terminalDetectionVerdict))
                        {
                            if (terminalDetectionVerdict == "�ϸ�") item.TerminalDetectionVerdict = 0;
                            else if (terminalDetectionVerdict == "���ϸ�") item.TerminalDetectionVerdict = 1;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�����۲����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�����۲���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ������ڣ��죩
                        //string detectionPeriod = dt.Rows[i][22].ToString();
                        int tempTerminalDetectionPeriod;
                        if (!string.IsNullOrEmpty(terminalDetectionPeriod))
                            if (int.TryParse(terminalDetectionPeriod, out tempTerminalDetectionPeriod))
                                item.TerminalDetectionPeriod = tempTerminalDetectionPeriod;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,������ڱ���Ϊ���֣�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,������ڲ���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �´μ��ʱ�䣨�����գ�
                        //string nextDetectionDate = dt.Rows[i][23].ToString();
                        DateTime tempNextTerminalDetectionDate;
                        if (!string.IsNullOrEmpty(nextTerminalDetectionDate))
                            if (DateTime.TryParse(nextTerminalDetectionDate, out tempNextTerminalDetectionDate))
                                item.NextTerminalDetectionDate = tempNextTerminalDetectionDate;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�´μ��ʱ�䲻�ԣ�</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�´μ��ʱ�䲻��Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ��ע
                        //string remark = dt.Rows[i][24].ToString();
                        if (!string.IsNullOrEmpty(remark))
                        {
                            item.Remark = remark;
                        }
                        #endregion
                    }


                    if (!firefightingbll.ExistCode(item.EquipmentNameNo, item.EquipmentCode, ""))
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�豸���Ϊ{1}��{2}�Ѵ��ڣ�</br>", order, item.EquipmentCode, item.EquipmentName);
                        error++;
                        continue;
                    }

                    try
                    {
                        firefightingbll.SaveForm("", item);
                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }

            return message;
        }
    }
}
