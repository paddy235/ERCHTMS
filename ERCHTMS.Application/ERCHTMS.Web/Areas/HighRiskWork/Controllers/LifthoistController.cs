using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using Newtonsoft.Json;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using System.Data;
using System.Collections.Generic;
using BSFramework.Util.Offices;
using System.Web;
using Aspose.Words;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� ���������ҵ
    /// </summary>
    public class LifthoistController : MvcControllerBase
    {
        private LifthoistjobBLL lifthoistjobbll = new LifthoistjobBLL();
        private LifthoistcertBLL lifthoistcertbll = new LifthoistcertBLL();
        private LifthoistauditrecordBLL lifthoistauditrecordbll = new LifthoistauditrecordBLL();
        private LifthoistsafetyBLL lifthoistsafetybll = new LifthoistsafetyBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private LifthoistpersonBLL lifthoistpersonbll = new LifthoistpersonBLL();
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
        /// ����ͼҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }
        /// <summary>
        /// ƾ��֤ҳ��
        /// </summary>
        /// <returns></returns>
        public ActionResult CertForm()
        {
            return View();
        }
        /// <summary>
        /// ��ʱ�볡�豸
        /// </summary>
        /// <returns></returns>
        public ActionResult TempEquipentIndex()
        {
            return View();
        }
        /// <summary>
        /// ��ʱ�볡�豸����
        /// </summary>
        /// <returns></returns>
        public ActionResult TempEquipentForm()
        {
            return View();
        }

        /// <summary>
        /// ������Ա��Ϣ
        /// </summary>
        /// <returns></returns>
        public ActionResult OperatePerson()
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
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                LifthoistSearchModel search = null;
                if (!string.IsNullOrEmpty(queryJson))
                {
                    search = JsonConvert.DeserializeObject<LifthoistSearchModel>(queryJson);
                }
                else
                {
                    search = new LifthoistSearchModel();
                }

                #region ����Ȩ��
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    string isAllDataRange = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetEnableItemValue("HighRiskWorkDataRange"); //�����ǣ��߷�����ҵģ���Ƿ�ȫ������
                    if (!string.IsNullOrWhiteSpace(isAllDataRange))
                    {
                        pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                        string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "a.createuserdeptcode", "a.createuserorgcode");
                        if (!string.IsNullOrEmpty(where))
                        {
                            pagination.conditionJson += " and " + where;
                        }
                    }
                    
                }
                #endregion
                //��ƾ��֤
                DataTable dt = null;
                if (search.pagetype == "1")
                {
                    dt = lifthoistcertbll.GetList(pagination, search);
                }
                else
                {
                    dt = lifthoistjobbll.GetList(pagination, search);
                }
                var jsonData = new
                {
                    rows = dt,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }


        /// <summary>
        /// ��ȡ��ʱ�볡�豸�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult getTempEquipentList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = "t.auditstate = 2 and t1.auditstate = 2 ";

                LifthoistSearchModel search = null;
                if (!string.IsNullOrEmpty(queryJson))
                {
                    search = JsonConvert.DeserializeObject<LifthoistSearchModel>(queryJson);
                }
                else
                {
                    search = new LifthoistSearchModel();
                }

                #region ����Ȩ��
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "t.createuserdeptcode", "t.createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
                #endregion
                //��ƾ��֤
                DataTable dt = lifthoistjobbll.getTempEquipentList(pagination, search);
                var jsonData = new
                {
                    rows = dt,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// ��ȡ���ص�װ��ҵʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = lifthoistjobbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡƾ��֤ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetCertFormJson(string keyValue)
        {
            var data = lifthoistcertbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// �õ���˼�¼
        /// </summary>
        /// <param name="businessid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAuditListToJson(string businessid)
        {
            var data = lifthoistauditrecordbll.GetList(businessid);
            return ToJsonResult(data);
        }

        /// <summary>
        /// �õ����ص�װ��ҵ����ͼ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetJobFlow(string keyValue)
        {
            LifthoistjobEntity entity = lifthoistjobbll.GetEntity(keyValue);
            string modulename = string.Empty;

            if (entity.QUALITYTYPE == "0")
            {
                modulename = "(���ص�װ��ҵ30T����)���";
            }
            else
            {
                modulename = "(���ص�װ��ҵ30T����)���";
            }
            
            var josnData = lifthoistjobbll.GetFlow(entity.ID, modulename);
            return Content(josnData.ToJson());
        }

        /// <summary>
        /// �õ�ƾ��֤����ͼ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetCertFlow(string keyValue)
        {
            LifthoistcertEntity entity = lifthoistcertbll.GetEntity(keyValue);
            string modulename = "(���ص�װ׼��֤)���";
            
            var josnData = lifthoistjobbll.GetFlow(entity.ID, modulename);
            return Content(josnData.ToJson());
        }

        /// <summary>
        /// �õ���ȫ��ʩ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSafetyMeasureToJson(string keyValue)
        {
            IEnumerable<LifthoistsafetyEntity> safetys = lifthoistsafetybll.GetList(string.Format("LIFTHOISTCERTID = '{0}'", keyValue));
            return ToJsonResult(safetys);
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
        [HandlerMonitor(6, "���ص�װ��ҵɾ��")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            try
            {
                lifthoistjobbll.RemoveForm(keyValue);
                DeleteFiles(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch (Exception ex)
            {
                return Error("ɾ������������Ϣ��" + ex.Message);
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="jsonData">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "���ص�װ��ҵ����")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string jsonData)
        {
            try
            {
                LifthoistjobEntity entity = JsonConvert.DeserializeObject<LifthoistjobEntity>(jsonData);
                lifthoistjobbll.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                return Error("�������������Ϣ��" + ex.Message);
            }
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="jsonData">���ʵ��</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(8, "���ص�װ��ҵ�ύ")]
        [AjaxOnly]
        public ActionResult AuditSubmit(string keyValue, string jsonData)
        {
            try
            {
                LifthoistauditrecordEntity auditEntity = JsonConvert.DeserializeObject<LifthoistauditrecordEntity>(jsonData);
                lifthoistjobbll.ApplyCheck(keyValue, auditEntity);
            }
            catch (Exception ex)
            {
                return Error("��˳���������Ϣ��" + ex.Message);
            }
            return Success("�ύ�ɹ���");
        }
        /// <summary>
        /// ƾ��֤ɾ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveCertForm(string keyValue)
        {
            try
            {
                lifthoistcertbll.RemoveForm(keyValue);
                DeleteFiles(keyValue);
            }
            catch (Exception ex)
            {
                return Error("ɾ������������Ϣ��" + ex.Message);
            }
            return Success("ɾ���ɹ���");
        }

        /// <summary>
        /// ƾ��֤����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveCertForm(string keyValue, string jsonData)
        {
            try
            {
                LifthoistcertEntity entity = JsonConvert.DeserializeObject<LifthoistcertEntity>(jsonData);
                lifthoistcertbll.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                return Error("�������������Ϣ��" + ex.Message);
            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ƾ��֤���
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="auditEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult AuditCertSubmit(string keyValue, LifthoistcertEntity entity, LifthoistauditrecordEntity auditEntity)
        {
            try
            {
                lifthoistcertbll.ApplyCheck(keyValue, entity, auditEntity);
            }
            catch (Exception ex)
            {
                return Error("��˳���������Ϣ��" + ex.Message);
            }
            return Success("�ύ�ɹ���");
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
                string isHrdl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("IsOpenPassword");
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";
                pagination.sidx = "applydate desc,applycode";//�����ֶ�
                pagination.sord = "desc";//����ʽ

                LifthoistSearchModel search = null;
                if (!string.IsNullOrEmpty(queryJson))
                {
                    search = JsonConvert.DeserializeObject<LifthoistSearchModel>(queryJson);
                }
                else
                {
                    search = new LifthoistSearchModel();
                }

                #region ����Ȩ��
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "a.createuserdeptcode", "a.createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
                #endregion
                //��ƾ��֤
                DataTable dt = null;
                string title = "";
                if (search.pagetype == "1")
                {
                    title = "׼��֤����";
                    dt = lifthoistcertbll.GetList(pagination, search);
                }
                else
                {
                    title = "���ص�װ��ҵ����";
                    dt = lifthoistjobbll.GetList(pagination, search);
                }
                DataTable exportTable = dt;
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("auditstate"));
                excelTable.Columns.Add(new DataColumn("applycodestr"));
                if (search.pagetype != "1")
                {
                    excelTable.Columns.Add(new DataColumn("qualitytype"));
                }
                excelTable.Columns.Add(new DataColumn("toolname"));
                excelTable.Columns.Add(new DataColumn("worktime"));
                excelTable.Columns.Add(new DataColumn("constructionunitname"));
                excelTable.Columns.Add(new DataColumn("applyusername"));
                excelTable.Columns.Add(new DataColumn("applydate"));
                excelTable.Columns.Add(new DataColumn("flowdeptname"));
                excelTable.Columns.Add(new DataColumn("flowname"));

                foreach (DataRow item in dt.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    string auditstate = item["auditstate"].ToString();
                    if (auditstate == "0")
                    {
                        auditstate = "������";
                    }
                    else if (auditstate == "1")
                    {
                        auditstate = "���(��)��";
                    }
                    else if (auditstate == "2")
                    {
                        auditstate = "���(��)ͨ��";
                    }
                    newDr["auditstate"] = auditstate;
                    newDr["applycodestr"] = item["applycodestr"];

                    if (search.pagetype != "1")
                    {
                        string qualitytype = item["qualitytype"].ToString();
                        if (qualitytype == "0")
                        {
                            qualitytype = "30T����";
                        }
                        else if (qualitytype == "1")
                        {
                            qualitytype = "30T����";
                        }
                        else
                        {
                            qualitytype = "2̨�����豸��ͬ���3T������";
                        }
                        newDr["qualitytype"] = qualitytype;
                    }
                    newDr["auditstate"] = auditstate;
                    newDr["toolname"] = item["toolname"];

                    DateTime workstartdate, workenddate, applydate;
                    DateTime.TryParse(item["workstartdate"].ToString(), out workstartdate);
                    DateTime.TryParse(item["workenddate"].ToString(), out workenddate);
                    DateTime.TryParse(item["applydate"].ToString(), out applydate);

                    string worktime = string.Empty;
                    if (workstartdate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        worktime += workstartdate.ToString("yyyy-MM-dd HH:mm");
                    }
                    if (workenddate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        worktime += " - " + workenddate.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["worktime"] = worktime;
                    newDr["constructionunitname"] = item["constructionunitname"];
                    newDr["applyusername"] = item["applyusername"];

                    string adate = string.Empty;
                    if (applydate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        adate = applydate.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["applydate"] = adate;
                    newDr["flowdeptname"] = item["flowdeptname"];
                    newDr["flowname"] = item["flowname"];

                    excelTable.Rows.Add(newDr);
                }
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = title;
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = title + ".xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "auditstate", ExcelColumn = "��ҵ���״̬", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applycodestr", ExcelColumn = "������", Width = 20 });
                if (search.pagetype != "1")
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "qualitytype", ExcelColumn = "�����������", Width = 20 });
                }
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "toolname", ExcelColumn = "��װ��������", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktime", ExcelColumn = isHrdl == "true" ? "��ҵʱ��" : "�ƻ���ҵʱ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "constructionunitname", ExcelColumn = "��ҵ��λ", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyusername", ExcelColumn = "������", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applydate", ExcelColumn = "����ʱ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "flowdeptname", ExcelColumn = "���/������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "flowname", ExcelColumn = "���/������", Width = 20 });
                //���õ�������
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HandlerMonitor(0,"������Ϣ")]
        public ActionResult ExportInfo(string keyValue)
        {
            try
            {
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                //�������

                string fileName = "���ص�װ��ҵ_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\���ص�װ��ҵ����ģ��.docx";
                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                DocumentBuilder builder = new DocumentBuilder(doc);
                DataTable dt = new DataTable();
                dt.Columns.Add("APPLYCODE"); //���
                dt.Columns.Add("CHARGEPERSONNAME"); //����������
                dt.Columns.Add("GUARDIANNAME"); //�໤��
                dt.Columns.Add("QUALITY"); //�����������
                dt.Columns.Add("TOOLNAME"); //��װ��������
                dt.Columns.Add("CONSTRUCTIONUNITNAME"); //��װʩ����λ
                dt.Columns.Add("CONSTRUCTIONADDRESS"); //��װʩ���ص�
                dt.Columns.Add("HOISTCONTENT"); //��װ����
                dt.Columns.Add("WORKDATE"); //��ҵʱ��
                dt.Columns.Add("APPLYCOMPANYNAME"); //���뵥λ
                dt.Columns.Add("APPLYUSERNAME"); //������
                dt.Columns.Add("APPLYDATE"); //��������
                dt.Columns.Add("approve1"); //����֧�ֲ�רҵ
                dt.Columns.Add("approvedate1");
                dt.Columns.Add("approve2"); //����֧�ֲ�
                dt.Columns.Add("approvedate2"); 
                dt.Columns.Add("approve3"); //�ֹ��쵼
                dt.Columns.Add("approvedate3");
                DataRow row = dt.NewRow();


                LifthoistjobEntity entity = lifthoistjobbll.GetEntity(keyValue);
                row["APPLYCODE"] = entity.APPLYCODESTR;
                row["CHARGEPERSONNAME"] = entity.CHARGEPERSONNAME;
                row["GUARDIANNAME"] = entity.GUARDIANNAME;
                row["QUALITY"] = entity.QUALITY;
                row["TOOLNAME"] = dataitemdetailbll.GetItemName("ToolName", entity.TOOLNAME);
                row["CONSTRUCTIONUNITNAME"] = entity.CONSTRUCTIONUNITNAME;
                row["CONSTRUCTIONADDRESS"] = entity.CONSTRUCTIONADDRESS;
                row["HOISTCONTENT"] = entity.HOISTCONTENT;
                row["WORKDATE"] = "��" + Convert.ToDateTime(entity.WORKSTARTDATE).ToString("yyyy��MM��dd��HHʱmm��") + "��" + Convert.ToDateTime(entity.WORKENDDATE).ToString("yyyy��MM��dd��HHʱmm��");
                row["APPLYCOMPANYNAME"] = entity.APPLYCOMPANYNAME;
                row["APPLYUSERNAME"] = entity.APPLYUSERNAME;
                row["APPLYDATE"] =Convert.ToDateTime(entity.APPLYDATE).ToString("yyyy-MM-dd HH:mm");

                IList<LifthoistauditrecordEntity> auditlist = lifthoistauditrecordbll.GetList(keyValue).Where(t => t.DISABLE != 1).OrderBy(t => t.AUDITDATE).ToList();
                for (int i = 0; i < auditlist.Count; i++)
                {
                    var filepath = auditlist[i].AUDITSIGNIMG == null ? Server.MapPath("~/content/Images/no_1.png") : (Server.MapPath("~/") + auditlist[i].AUDITSIGNIMG.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    if (!System.IO.File.Exists(filepath))
                    {
                        filepath = Server.MapPath("~/content/Images/no_1.png");
                    }
                    var stime =Convert.ToDateTime(auditlist[i].AUDITDATE).ToString("yyyy-MM-dd HH:mm");
                    switch (i)
                    {
                        case 0:
                            builder.MoveToMergeField("approve1");
                            builder.InsertImage(filepath, 80, 35);
                            row["approvedate1"] = stime;
                            break;
                        case 1:
                            builder.MoveToMergeField("approve2");
                            builder.InsertImage(filepath, 80, 35);
                            row["approvedate2"] = stime;
                            break;
                        case 2:
                            builder.MoveToMergeField("approve3");
                            builder.InsertImage(filepath, 80, 35);
                            row["approvedate3"] = stime;
                            break;
                        default:
                            break;
                    }
                }

                dt.Rows.Add(row);
                doc.MailMerge.Execute(dt);

                DataTable dtperson = new DataTable("U");
                dtperson.Columns.Add("BelongDeptName");
                dtperson.Columns.Add("CertificateNum");
                dtperson.Columns.Add("PersonName");
                dtperson.Columns.Add("PersonType");
                List<LifthoistpersonEntity> listperson = lifthoistpersonbll.GetRelateList(keyValue).ToList();
                foreach (var item in listperson)
                {
                    DataRow dtrow = dtperson.NewRow();
                    dtrow["BelongDeptName"] = item.BelongDeptName;
                    dtrow["CertificateNum"] = item.CertificateNum;
                    dtrow["PersonName"] = item.PersonName;
                    dtrow["PersonType"] = item.PersonName;
                    DataTable liftfazls = fileInfoBLL.GetFiles(item.Id);
                    if (liftfazls != null && liftfazls.Rows.Count > 0)
                    {
                        foreach (DataRow rowitem in liftfazls.Rows)
                        {
                            string image = "ico,gif,jpeg,jpg,png,psd";
                            if (image.Contains(rowitem["filepath"].ToString().Split('.')[1].ToLower()))
                            {
                                var filepath = rowitem["filepath"] == null ? Server.MapPath("~/content/Images/no_1.png") : (Server.MapPath("~/") + rowitem["filepath"].ToString().Replace("~/", "").ToString()).Replace(@"\/", "/").ToString();
                                if (!System.IO.File.Exists(filepath))
                                {
                                    filepath = Server.MapPath("~/content/Images/no_1.png");
                                }
                                builder.MoveToMergeField("lifthoistpersonfile");
                                builder.InsertImage(filepath, 80, 35);
                            }
                           
                        }
                    }
                    dtperson.Rows.Add(dtrow);
                }
                doc.MailMerge.ExecuteWithRegions(dtperson);
                doc.MailMerge.DeleteFields();
                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
                return Success("�����ɹ�!");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        #endregion
    }
}
