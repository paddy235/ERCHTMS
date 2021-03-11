using ERCHTMS.Entity.Observerecord;
using ERCHTMS.Busines.Observerecord;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Collections.Generic;
using ERCHTMS.Busines.AuthorizeManage;
using System.Web;
using System;
using System.Data;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using System.Text;
using Aspose.Words;
using Aspose.Words.Tables;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Offices;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.IO;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.ObserveRecord.Controllers
{
    /// <summary>
    /// �� �����۲��¼��
    /// </summary>
    public class ObserverecordController : MvcControllerBase
    {
        private ObserverecordBLL observerecordbll = new ObserverecordBLL();
        private ObservesafetyBLL observesafetybll = new ObservesafetyBLL();
        private ObservecategoryBLL observecategorybll = new ObservecategoryBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private ObsAnalysisReportBLL obsanalysisreportbll = new ObsAnalysisReportBLL();

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
        public ActionResult SafetyForm()
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
        /// ��������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AnalysisIndex()
        {
            return View();
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AnalysisReport()
        {
            return View();
        }
        /// <summary>
        /// ͳ�Ʒ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StatForm()
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
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.id";
            pagination.p_fields = @" t.workname,t.workunitid,t.workunit,
                                       t.workpeople,t.workpeopleid,t.createuserid,
                                       t.createuserorgcode,t.createuserdeptcode,createuserdept,
                                       t.createdate,t.workarea,t.workareaid,
                                       t.workplace,t.obsendtime,t.obsstarttime,
                                       t.obsperson,t.obspersonid,t.obsplanid,t.iscommit,t.deptname";
            pagination.p_tablename = @"(
select max(t2.ID) ID, max(t2.workname) workname,
       max(t2.workunitid) workunitid,
       max(t2.workunit) workunit,
       max(t2.workpeople) workpeople,
       max(DBMS_LOB.substr(t2.workpeopleid)) workpeopleid,
       max(t2.createuserid) createuserid,
       max(t2.createuserorgcode) createuserorgcode,
       max(t2.createuserdeptcode) createuserdeptcode,
       max(t2.createuserdept) createuserdept,
       max(t2.createdate) createdate,
       max(t2.workarea) workarea,
       max(t2.workareaid) workareaid,
       max(t2.workplace) workplace,
       max(t2.obsendtime) obsendtime,
       max(t2.obsstarttime) obsstarttime,
       max(t2.obsperson) obsperson,
       max(DBMS_LOB.substr(t2.obspersonid)) obspersonid,
       max(t2.obsplanid) obsplanid,
       max(t2.iscommit) iscommit,
       to_char(wm_concat(t3.fullname)) deptname
       from bis_observerecord t2 left join (
select distinct d.fullname,t1.id from (
select id,REGEXP_SUBSTR(DBMS_LOB.substr(t.obspersonid),'[^,]+',1,LEVEL) as userid from bis_observerecord t
 connect by rownum<=LENGTH(DBMS_LOB.substr(t.obspersonid))-LENGTH(regexp_replace(DBMS_LOB.substr(t.obspersonid),',',''))+1) t1
left join base_user u on t1.userid=u.userid
left join base_department d on d.departmentid=u.departmentid
)t3 on t3.id= t2.id group by t2.id
) t";
            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" (t.iscommit=1 or t.createuserid='{0}') ", currUser.UserId);
            if (!currUser.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var data = observerecordbll.GetPageList(pagination, queryJson);
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
        /// ��ȡ��ȫ��Ϊ���߲���ȫ��Ϊ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSafetyPageList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.id";
            pagination.p_fields = @" t.behaviordesc,t.observetype,t.observetypename,
                                       t.recordid,t.issafety,t.immediatecause,
                                       t.remotecause,t.remedialaction,
                                       t.preventivemeasures,t.personresponsible,t.reformdeadline,
                                       t.createuserid,t.createuserdeptcode,t.createuserorgcode,
                                       t.createdate";
            pagination.p_tablename = @"bis_observesafety t";
            pagination.conditionJson = " 1=1";
            Operator currUser = OperatorProvider.Provider.Current();
            var data = observesafetybll.GetSafetyPageList(pagination, queryJson);
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
        /// ��ȡ���������б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetReportPageJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.id";
            pagination.p_fields = @" t.workzy,t.workzyid,t.workzycode,t.workunitid,t.workunit,t.workunitcode,
                                       t.analysispeople,t.analysispeopleid,t.createuserid,t.reporttype,
                                       t.createuserorgcode,t.createuserdeptcode,t.year,t.quarter,
                                       t.createdate,t.analysistime,t.analysiscontent,
                                       t.ygfy,t.grfhzb,t.cxbz,t.hjzj,
                                       t.ygwz,t.gjsb,t.rtgxx,t.iscommit";
            pagination.p_tablename = @"bis_obsanalysisreport t";
            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" (t.iscommit=1 or t.createuserid='{0}') ", currUser.UserId);
            if (!currUser.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var data = obsanalysisreportbll.GetPageList(pagination, queryJson);
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
        /// ���ݹ۲��¼Id��ȡ�۲����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetObsTypeData(string keyValue)
        {
            var data = observerecordbll.GetObsTypeData(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson()
        {
            var data = observerecordbll.GetList();
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
            var data = observerecordbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ��������ʵ��
        /// </summary>
        [HttpGet]
        public ActionResult GetReportEntity(string keyValue)
        {
            var data = obsanalysisreportbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ��ȫ��Ϊ/����ȫ��Ϊʵ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSafetyDesc(string keyValue)
        {
            var data = observesafetybll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ��ȫ��Ϊ�벻��ȫ��Ϊռ��ͳ��
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="month">�¶�</param>
        /// <returns></returns>
        [HttpGet]
        public string GetSafetyStat(string deptCode, string year = "", string quarter = "", string month = "")
        {
            return observerecordbll.GetSafetyStat(deptCode, year, quarter, month);
        }

        /// <summary>
        /// ��ȡ�۲�����Ա�ͼ
        /// </summary>
        /// <param name="deptCode">��λCode</param>
        /// <param name="year">��</param>
        /// <param name="quarter">����</param>
        /// <param name="month">�¶�</param>
        /// <param name="issafety">issafety 0 ����ȫ��Ϊ 1 ��ȫ��Ϊ</param>
        /// <returns></returns>
        [HttpGet]
        public string GetUntiDbStat(string deptCode, string issafety, string year = "", string quarter = "", string month = "")
        {
            return observerecordbll.GetUntiDbStat(deptCode, issafety, year, quarter, month);
        }
        /// <summary>
        /// ��ȡ����ȫ��������ͼ
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetQsStat(string deptCode, string year = "")
        {
            return observerecordbll.GetQsStat(deptCode, year);
        }
        /// <summary>
        /// ���ݹ۲�ƻ�Id������ֽ�Id��ѯ�Ƿ�����˹۲��¼
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="planfjid"></param>
        /// <returns></returns>

        public ActionResult GetObsRecordByPlanIdAndFjId(string planid, string planfjid)
        {
            var b = observerecordbll.GetObsRecordByPlanIdAndFjId(planid, planfjid);
            return ToJsonResult(b);
        }

        public ActionResult ExportExcel(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "t.id";
                pagination.p_fields = @" t.workname,t.createuserdept,t.workunit,
                                       t.workpeople,t.workarea,to_char(t.obsstarttime,'yyyy-mm-dd hh24:mi') as obsstarttime,
                                       to_char(t.obsendtime,'yyyy-mm-dd hh24:mi') as obsendtime,t.obsperson,t.deptname";
                pagination.p_tablename = @"(
select max(t2.ID) ID, max(t2.workname) workname,
       max(t2.workunitid) workunitid,
       max(t2.workunit) workunit,
       max(t2.workpeople) workpeople,
       max(DBMS_LOB.substr(t2.workpeopleid)) workpeopleid,
       max(t2.createuserid) createuserid,
       max(t2.createuserorgcode) createuserorgcode,
       max(t2.createuserdeptcode) createuserdeptcode,
       max(t2.createuserdept) createuserdept,
       max(t2.createdate) createdate,
       max(t2.workarea) workarea,
       max(t2.workareaid) workareaid,
       max(t2.workplace) workplace,
       max(t2.obsendtime) obsendtime,
       max(t2.obsstarttime) obsstarttime,
       max(t2.obsperson) obsperson,
       max(DBMS_LOB.substr(t2.obspersonid)) obspersonid,
       max(t2.obsplanid) obsplanid,
       max(t2.iscommit) iscommit,
       to_char(wm_concat(t3.fullname)) deptname
       from bis_observerecord t2 left join (
select distinct d.fullname,t1.id from (
select id,REGEXP_SUBSTR(DBMS_LOB.substr(t.obspersonid),'[^,]+',1,LEVEL) as userid from bis_observerecord t
 connect by rownum<=LENGTH(DBMS_LOB.substr(t.obspersonid))-LENGTH(regexp_replace(DBMS_LOB.substr(t.obspersonid),',',''))+1) t1
left join base_user u on t1.userid=u.userid
left join base_department d on d.departmentid=u.departmentid
)t3 on t3.id= t2.id group by t2.id
) t";
                Operator currUser = OperatorProvider.Provider.Current();
                pagination.conditionJson = string.Format(" (t.iscommit=1 or t.createuserid='{0}') ", currUser.UserId);
                if (!currUser.IsSystem)
                {
                    //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
                pagination.sord = "desc";
                pagination.sidx = "createdate";
                var data = observerecordbll.GetPageList(pagination, queryJson);
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "�۲��¼�б�";
                excelconfig.FileName = "�۲��¼�б���.xls";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;

                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workname", ExcelColumn = "��ҵ����", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createuserdept", ExcelColumn = "�Ǽǲ���", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workunit", ExcelColumn = "��ҵ����", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workpeople", ExcelColumn = "��ҵ��Ա", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workarea", ExcelColumn = "��ҵ����", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "obsstarttime", ExcelColumn = "�۲���ʼʱ��", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "obsendtime", ExcelColumn = "�۲����ʱ��", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "obsperson", ExcelColumn = "�۲���Ա", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "�۲���Ա����", Width = 300 });

                //���õ�������
                //ExcelHelper.ExcelDownload(exportTable, excelconfig);
                ExcelHelper.ExportByAspose(data, excelconfig.FileName, excelconfig.ColumnEntity);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
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
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            observerecordbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ɾ����ȫ��Ϊ�򲻰�ȫ��Ϊ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveSafetyForm(string keyValue)
        {
            observesafetybll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ɾ����������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveReportForm(string keyValue)
        {
            obsanalysisreportbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ObserverecordEntity entity, string observecategory, string safetyList)
        {
            List<ObservecategoryEntity> listCategory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ObservecategoryEntity>>(observecategory);
            List<ObservesafetyEntity> listSafety = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ObservesafetyEntity>>(safetyList);
            if (string.IsNullOrWhiteSpace(entity.ObsGist))
            {
                entity.ObsGist = "";
                entity.ObsGistValue = "";
            }
            observerecordbll.SaveForm(keyValue, entity, listCategory, listSafety);
            return Success("�����ɹ���");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSafetyDescForm(string keyValue, ObservesafetyEntity safetyList)
        {
            //List<ObservesafetyEntity> listSafety = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ObservesafetyEntity>>(safetyList);
            observesafetybll.SaveForm(keyValue, safetyList);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="safetyList"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveAnalysisReport(string keyValue, ObsAnalysisReportEntity entity)
        {
            obsanalysisreportbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ���氲ȫ����������
        /// </summary>
        /// <param name="ItemValue">����ֵ</param>
        /// <param name="ItemCode">�糧Code</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult SaveJjx(string ItemValue, string ItemCode)
        {

            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'obsjjx'").Where(x => x.ItemCode == ItemCode && x.EnabledMark == 1).ToList();
            if (itemlist.Count > 0)
            {
                DataItemModel itemmodel = itemlist.FirstOrDefault();
                DataItemDetailEntity detail = dataitemdetailbll.GetEntity(itemmodel.ItemDetailId);
                detail.ItemValue = ItemValue;
                dataitemdetailbll.SaveForm(detail.ItemDetailId, detail);
                return Success("���óɹ�");
            }
            else
            {
                var itemmodel = dataitemdetailbll.GetDataItemListByItemCode("'obsjjx'").ToList();
                if (itemmodel.Count > 0)
                {
                    DataItemDetailEntity detail = new DataItemDetailEntity();
                    detail.ItemName = "��ȫ������";
                    detail.ItemValue = ItemValue;
                    detail.ItemCode = OperatorProvider.Provider.Current().OrganizeCode;
                    detail.ItemId = itemmodel.FirstOrDefault().ItemId;
                    detail.ParentId = itemmodel.FirstOrDefault().ParentId;
                    detail.EnabledMark = itemmodel.FirstOrDefault().EnabledMark;
                    detail.SortCode = itemmodel.Count + 1;
                    dataitemdetailbll.SaveForm(detail.ItemDetailId, detail);
                    return Success("���óɹ�");
                }
                else
                {
                    return Success("����������ʧ��,����ϵ����Ա");
                }

            }

        }
        /// <summary>
        /// �۲��¼��������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportData(string keyValue)
        {
            HttpResponse resp = System.Web.HttpContext.Current.Response;

            string fileName = "��ȫ��Ϊ�۲��¼_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\��ȫ��Ϊ�۲��¼������.doc";
            if (!System.IO.File.Exists(strDocPath)) throw new Exception("ģ�岻����!");
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            Aspose.Words.DocumentBuilder builder = new Aspose.Words.DocumentBuilder(doc);


            #region ������ʽ���úϲ���ͷ��
            builder.InsertCell();
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Font.Size = 22;
            builder.Bold = true;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.Write("��ȫ��Ϊ�۲��¼");
            builder.EndRow();

            var table = builder.StartTable();
            #endregion
            ObserverecordEntity obsRecord = observerecordbll.GetEntity(keyValue);//�۲��¼ʵ��
            #region ��ͷ��ʽ
            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��ҵ����");


            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��ҵ����/��λ");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��ҵ��Ա");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��ҵ����");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("�۲���Ա");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("�۲���ֹʱ��");
            builder.EndRow();

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.WorkName);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.WorkUnit);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.WorkPeople);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.WorkArea);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.ObsPerson);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            var time = obsRecord.ObsStartTime.Value.ToString("yyyy-MM-dd HH:mm") + "-" + obsRecord.ObsEndTime.Value.ToString("yyyy-MM-dd HH:mm");
            builder.Write(time);
            builder.EndRow();

            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.Write("�۲�����");


            builder.InsertCell();
            builder.ParagraphFormat.ClearFormatting();
            builder.CellFormat.ClearFormatting();
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Font.Size = 8;
            builder.Bold = false;
            if (!string.IsNullOrWhiteSpace(obsRecord.ObsGist))
            {
                if (obsRecord.ObsGist.Contains("0"))
                {
                    builder.Write("�̲������/WSWP���Ʊ��");
                }
                else
                {
                    builder.Write("���������/WSWP���Ʊ��");
                }
            }



            builder.InsertCell();
            builder.ParagraphFormat.ClearFormatting();
            builder.CellFormat.ClearFormatting();
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Font.Size = 8;
            builder.Bold = false;
            if (!string.IsNullOrWhiteSpace(obsRecord.ObsGist))
            {
                if (obsRecord.ObsGist.Contains("1"))
                {
                    builder.Write("�̹���Ʊ���߲���Ʊ���Ʊ��");
                }
                else
                {
                    builder.Write("������Ʊ���߲���Ʊ���Ʊ��");
                }
            }


            builder.InsertCell();
            builder.ParagraphFormat.ClearFormatting();
            builder.CellFormat.ClearFormatting();
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Font.Size = 8;
            builder.Bold = false;

            if (!string.IsNullOrWhiteSpace(obsRecord.ObsGist))
            {
                if (obsRecord.ObsGist.Contains("2"))
                {
                    builder.Write("�������ļ����Ʊ��");
                }
                else
                {
                    builder.Write("�������ļ����Ʊ��");
                }
            }

            builder.EndRow();

            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.Previous;

            builder.InsertCell();
            builder.ParagraphFormat.ClearFormatting();
            builder.CellFormat.ClearFormatting();
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Font.Size = 8;
            builder.Bold = false;
            var value = obsRecord.ObsGistValue.Substring(0, obsRecord.ObsGistValue.Length - 1).Split(',');
            var list = obsRecord.ObsGist.Substring(0, obsRecord.ObsGist.Length - 1).Split(',');
            if (!string.IsNullOrWhiteSpace(obsRecord.ObsGistValue))
            {

                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == "0")
                    {
                        builder.Write(value[i].ToString());
                        break;
                    }

                }
            }
            else
            {
                builder.Write("");
            }

            builder.InsertCell();
            builder.ParagraphFormat.ClearFormatting();
            builder.CellFormat.ClearFormatting();
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Font.Size = 8;
            builder.Bold = false;
            if (!string.IsNullOrWhiteSpace(obsRecord.ObsGistValue))
            {

                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == "1")
                    {
                        builder.Write(value[i].ToString());
                        break;
                    }

                }
            }
            else
            {
                builder.Write("");
            }

            builder.InsertCell();
            builder.ParagraphFormat.ClearFormatting();
            builder.CellFormat.ClearFormatting();
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalMerge = CellMerge.None;
            if (!string.IsNullOrWhiteSpace(obsRecord.ObsGistValue))
            {
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == "2")
                    {
                        builder.Write(value[i].ToString());
                        break;
                    }

                }
            }
            else
            {
                builder.Write("");
            }
            builder.EndRow();

            #endregion
            table.AutoFit(AutoFitBehavior.AutoFitToContents);
            var obsdata = dataitemdetailbll.GetDataItemListByItemCode("'ObsType'").ToList();//�۲����7����
            #region 7����
            for (int i = 0; i < obsdata.Count; i++)
            {
                builder.InsertCell();
                builder.CellFormat.ClearFormatting();
                builder.Font.Size = 8;
                builder.Bold = false;
                builder.CellFormat.Width = 65;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Top;
                if (i == 0)
                {
                    builder.CellFormat.VerticalMerge = CellMerge.First;
                }
                builder.Write(obsdata[i].ItemName);
                if (i == obsdata.Count - 1)
                {
                    builder.EndRow();
                }
            }

            #endregion
            #region 7�����������
            var typeData = observerecordbll.GetObsTypeData(keyValue);//�۲����
            //ѭ���ҳ��۲������ѡ���˵���
            for (int i = 0; i < obsdata.Count; i++)
            {

                builder.InsertCell();
                builder.CellFormat.ClearFormatting();
                builder.Font.Size = 8;
                builder.Bold = false;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Top;
                if (i == 0)
                {
                    builder.CellFormat.VerticalMerge = CellMerge.First;
                }
                var sondata = dataitemdetailbll.GetDataItemListByItemCode("'" + obsdata[i].ItemValue + "'").ToList();

                sondata.Insert(0, new Entity.SystemManage.ViewModel.DataItemModel() { ItemName = "ȫ����ȫ", ItemValue = "0", Description = obsdata[i].ItemValue });
                sondata.Insert(1, new Entity.SystemManage.ViewModel.DataItemModel() { ItemName = "��������:", ItemValue = "", Description = obsdata[i].ItemValue });
                for (int j = 0; j < sondata.Count; j++)
                {
                    if (!string.IsNullOrWhiteSpace(sondata[j].ItemValue))
                    {
                        sondata[j].ItemName = "��" + sondata[j].ItemName;
                    }
                    for (int k = 0; k < typeData.Rows.Count; k++)
                    {
                        if (sondata[j].Description == typeData.Rows[k]["observetype"].ToString() && sondata[j].ItemValue == typeData.Rows[k]["existproblemcode"].ToString())
                        {
                            sondata[j].ItemName = "��" + sondata[j].ItemName.Replace("��", "");
                        }

                    }
                    builder.Writeln(sondata[j].ItemName);
                }
                if (i == obsdata.Count - 1)
                {
                    builder.EndRow();
                }
            }
            #endregion
            #region ��ȫ��Ϊ
            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.VerticalMerge = CellMerge.First;
            //builder.Write("��ȫ��Ϊ");
            builder.Writeln("��");
            builder.Writeln("ȫ");
            builder.Writeln("��");
            builder.Writeln("Ϊ");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("���");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��ȫ��Ϊ����");

            builder.EndRow();
            for (int i = 0; i < obsdata.Count; i++)
            {
                var safeData = observesafetybll.GetDataByType("1", obsdata[i].ItemName, keyValue);
                if (safeData.Rows.Count > 0)
                {
                    for (int m = 0; m < safeData.Rows.Count; m++)
                    {
                        builder.InsertCell();
                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                        if (m == 0)
                        {
                            builder.InsertCell();
                            builder.CellFormat.ClearFormatting();
                            builder.Font.Size = 8;
                            builder.Bold = false;
                            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                            builder.CellFormat.VerticalMerge = CellMerge.First;
                            builder.Write(obsdata[i].ItemName);
                        }
                        else
                        {
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.Previous;
                        }
                        builder.InsertCell();
                        builder.CellFormat.ClearFormatting();
                        builder.Font.Size = 8;
                        builder.Bold = false;
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                        builder.Write(safeData.Rows[m]["behaviordesc"].ToString());
                        builder.EndRow();
                    }
                }
            }
            #endregion
            #region ����ȫ��Ϊ

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.Writeln("��");
            builder.Writeln("��");
            builder.Writeln("ȫ");
            builder.Writeln("��");
            builder.Writeln("Ϊ");
            //builder.Writeln("����ȫ��Ϊ");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;

            builder.Write("���");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��������");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("ֱ��ԭ��");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("���ԭ��");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("������ʩ");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("Ԥ����ʩ");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("����������");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��������");

            builder.EndRow();

            for (int i = 0; i < obsdata.Count; i++)
            {
                var notsafeData = observesafetybll.GetDataByType("0", obsdata[i].ItemName, keyValue);
                if (notsafeData.Rows.Count > 0)
                {

                    for (int m = 0; m < notsafeData.Rows.Count; m++)
                    {
                        builder.InsertCell();
                        builder.CellFormat.VerticalMerge = CellMerge.Previous;

                        if (m == 0)
                        {
                            builder.InsertCell();
                            builder.CellFormat.ClearFormatting();
                            builder.Font.Size = 8;
                            builder.Bold = false;
                            builder.CellFormat.VerticalMerge = CellMerge.First;
                            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                            builder.Write(obsdata[i].ItemName);
                        }
                        else
                        {
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.Previous;
                        }
                        builder.InsertCell();
                        builder.CellFormat.ClearFormatting();
                        builder.Font.Size = 8;
                        builder.Bold = false;
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                        builder.Write(notsafeData.Rows[m]["behaviordesc"].ToString());

                        builder.InsertCell();
                        builder.CellFormat.ClearFormatting();
                        builder.Font.Size = 8;
                        builder.Bold = false;
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                        builder.Write(notsafeData.Rows[m]["immediatecause"].ToString());

                        builder.InsertCell();
                        builder.CellFormat.ClearFormatting();
                        builder.Font.Size = 8;
                        builder.Bold = false;
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                        builder.Write(notsafeData.Rows[m]["remotecause"].ToString());

                        builder.InsertCell();
                        builder.CellFormat.ClearFormatting();
                        builder.Font.Size = 8;
                        builder.Bold = false;
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                        builder.Write(notsafeData.Rows[m]["remedialaction"].ToString());

                        builder.InsertCell();
                        builder.CellFormat.ClearFormatting();
                        builder.Font.Size = 8;
                        builder.Bold = false;
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                        builder.Write(notsafeData.Rows[m]["preventivemeasures"].ToString());

                        builder.InsertCell();
                        builder.CellFormat.ClearFormatting();
                        builder.Font.Size = 8;
                        builder.Bold = false;
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                        builder.Write(notsafeData.Rows[m]["personresponsible"].ToString());

                        builder.InsertCell();
                        builder.CellFormat.ClearFormatting();
                        builder.Font.Size = 8;
                        builder.Bold = false;
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                        builder.Write(notsafeData.Rows[m]["reformdeadline"].ToString());
                        builder.EndRow();
                        //if (m == notsafeData.Rows.Count - 1)
                        //{
                        //    builder.EndRow();
                        //}
                    }
                }
            }
            #endregion
            #region ��ͨ��¼

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.Write("��ͨ��¼");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��ͨʱ��");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��ͨ�ص�");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("�μ���Ա");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��ͨ����");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��ͨ���");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��ע");

            builder.EndRow();

            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.Previous;

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.LinkUpTime.Value.ToString("yyyy-MM-dd HH:mm"));

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.LinkUpPlace);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.LinkPeople);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.LinkUpContent);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.LinkUpResult);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.LinkUpRemark == null ? "" : obsRecord.LinkUpRemark);
            builder.EndRow();


            #endregion
            #region ���ռ�¼

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.Write("���ռ�¼");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("���ս���");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("������Ա");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("��֤ʱ��");
            builder.EndRow();


            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.Previous;

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.AcceptResult);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.AcceptPerson);

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.AcceptTime.Value.ToString("yyyy-MM-dd HH:mm"));
            builder.EndRow();

            #endregion
            #region ��ע
            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.FitText = true;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.Write("��ע");

            builder.InsertCell();
            builder.CellFormat.ClearFormatting();
            builder.Font.Size = 8;
            builder.Bold = false;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write(obsRecord.RecordRemark == null ? "" : obsRecord.RecordRemark);
            builder.EndRow();

            #endregion
            builder.EndTable();

            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return Success("�����ɹ�!");
        }
        /// <summary>
        /// ������ȫ�۲��¼ͳ�Ʊ��
        /// </summary>
        /// <param name="type"></param>
        /// <param name="deptcode"></param>
        /// <param name="year"></param>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public ActionResult exportTable(string type, string deptcode, string year, string quarter, string month)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //string path = "~/Resource/Temp";
            string fName = "��ȫ��Ϊ�۲�ͳ�Ʊ��" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

            var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = new DataTable();
            string sql = string.Empty;
            switch (type)
            {
                #region �۲���Ϊռ��ͼ
                case "zb":
                    wb.Open(Server.MapPath("~/Resource/ExcelTemplate/��ȫ��Ϊ�۲�ͳ�Ʊ�񵼳�ģ��2.xlsx"));
                    string strWhere = string.Empty;
                    strWhere = string.Format(" and t.createuserorgcode='{0}'", currUser.OrganizeCode);
                    if (!string.IsNullOrWhiteSpace(deptcode))
                    {
                        strWhere += string.Format(" and t.workunitcode like'{0}%'", deptcode);
                    }
                    if (!string.IsNullOrWhiteSpace(year))
                    {
                        strWhere += string.Format(" and to_char(t.obsstarttime,'yyyy')='{0}'", year);
                    }
                    if (!string.IsNullOrWhiteSpace(quarter))
                    {
                        switch (quarter)
                        {
                            case "1":
                                strWhere += string.Format(" and to_char(t.obsstarttime,'MM') in('01','02','03')");
                                break;
                            case "2":
                                strWhere += string.Format(" and to_char(t.obsstarttime,'MM') in('04','05','06')");
                                break;
                            case "3":
                                strWhere += string.Format(" and to_char(t.obsstarttime,'MM') in('07','08','09')");
                                break;
                            case "4":
                                strWhere += string.Format(" and to_char(t.obsstarttime,'MM') in('10','11','12')");
                                break;
                            default:
                                break;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(month))
                    {
                        strWhere += string.Format(" and to_char(t.obsstarttime,'MM') ='{0}'", month);
                    }
                    sql = string.Format(@"select s.observetypename,sum(case when s.issafety=1 then 1 else 0 end) safenum,'' as safeper,
                                               sum(case when s.issafety=0 then 1 else 0 end) notsafenum,
                                               '' as notsafeper, sum(case when s.issafety=0 then 1 else 0 end) ideanum,'' as ideaper
                                         from bis_observesafety s
                                          left join (select d.itemcode,d.itemname,d.itemvalue,d.sortcode from base_dataitemdetail d 
                                          where d.itemid in(select d.itemid from base_dataitem d where d.itemcode='ObsType'))d on d.itemvalue=s.observetype
                                          where s.recordid in(select id from bis_observerecord t where t.iscommit=1 {0})
                                          group by s.observetypename,s.observetype,d.sortcode
                                          order by d.sortcode ", strWhere);
                    //sql = string.Format(sql, strWhere);
                    dt = observerecordbll.GetTable(sql);
                    var totalNum = dt.Compute("Sum(safenum)", "").ToDouble();
                    var notTotal = dt.Compute("Sum(notsafenum)", "").ToDouble();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (totalNum > 0)
                        {
                            dt.Rows[i]["safeper"] = Math.Round(Convert.ToSingle(dt.Rows[i]["safenum"]) / totalNum, 2) * 100 + "%";
                        }
                        else
                        {
                            dt.Rows[i]["safeper"] = 0 + "%";
                        }
                        if (notTotal > 0)
                        {
                            dt.Rows[i]["notsafeper"] = Math.Round(Convert.ToSingle(dt.Rows[i]["notsafenum"]) / notTotal, 2) * 100 + "%";
                            dt.Rows[i]["ideaper"] = Math.Round(Convert.ToSingle(dt.Rows[i]["notsafenum"]) / notTotal, 2) * 100 + "%";
                        }
                        else
                        {
                            dt.Rows[i]["notsafeper"] = 0 + "%";
                            dt.Rows[i]["ideaper"] = 0 + "%";
                        }
                    }
                    DataRow row = dt.NewRow();
                    row["observetypename"] = "�ܼ�";
                    row["safenum"] = totalNum;
                    row["safeper"] = "100%";
                    row["notsafenum"] = notTotal;
                    row["notsafeper"] = "100%";
                    row["ideanum"] = notTotal;
                    row["ideaper"] = "100%";
                    dt.Rows.Add(row);
                    var allNum = totalNum + notTotal;
                     DataRow row1 = dt.NewRow();
                     row1["observetypename"] = "��Ϊ����";
                     row1["safenum"] = allNum;
                     row1["safeper"] = "��ȫ��Ϊ����:" + (allNum == 0 ? "0%" : Math.Round(totalNum / (totalNum + notTotal), 2) * 100 + "%");
                     //row1["notsafenum"] ="";
                     row1["notsafeper"] = "����ȫ��Ϊ����:" + (allNum == 0 ? "0%" : Math.Round(notTotal / (totalNum + notTotal), 2) * 100 + "%");
                    //row1["ideanum"] ="";
                    row1["ideaper"] = "";
                    dt.Rows.Add(row1);
                    wb.Worksheets[0].Cells.ImportDataTable(dt, false, 4, 0);

                    //wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
                    wb.Save(Server.UrlEncode(fName), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                    return Success("�����ɹ���");
                #endregion
                #region ����ȫ������ͼ
                case "qs":
                    wb.Open(Server.MapPath("~/Resource/ExcelTemplate/��ȫ��Ϊ�۲�ͳ�Ʊ�񵼳�ģ��1.xlsx"));
                    sql = string.Empty;
                    string sqlWhere = string.Empty;
                    string sql1 = string.Empty;
                    string sql2 = string.Empty;
                    string sql3 = string.Empty;
                    string sql4 = string.Empty;

                    sql = @"select t.observetypename,{0} from 
                            (select {4} d.itemname observetypename,d.itemvalue observetype
from (select d.itemvalue,d.itemname,d.sortcode from base_dataitemdetail d where d.itemid in (select itemid from base_dataitem t where t.itemcode='ObsType')) d
    left join(select s.observetype,s.observetypename,
                             {1}
                             from bis_observesafety s
                             left join bis_observerecord b on  b.id=s.recordid
                              where s.issafety=0 {3}
                             group by s.observetype,s.observetypename)t on t.observetype=d.itemvalue)t,
                            (select {2}
                            from bis_observerecord b 
                            left join bis_observesafety s on s.recordid=b.id where 1=1 {3})t1
                             group by t.observetypename";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql1 += string.Format(@"nvl(sum(case
                     when cast(to_char({1}, 'MM') as number) = {0} then
                      1
                     else
                      0
                   end),0){2},", i, "b.obsstarttime", "m" + i);
                        sql2 += string.Format(@"nvl(sum(case
                     when cast(to_char({1}, 'MM') as number) = {0} then
                      1
                     else
                      0
                   end),0){2},", i, "b.obsstarttime", "t" + i);
                        sql3 += string.Format(@"nvl(sum(case
                     when {0}=0 then 0 else round({3}/{1},4)*100 end),0){2},", "t1.t" + i, "t1.t" + i, "p" + i, "t.m" + i);
                        sql4 += string.Format(@"nvl(t.m" + i + ",0) m" + i + ",");
                    }
                    if (!string.IsNullOrWhiteSpace(deptcode))
                    {
                        sqlWhere += string.Format(" and  b.workunitcode like'{0}%'", deptcode);
                    }
                    if (!string.IsNullOrWhiteSpace(year))
                    {
                        sqlWhere += string.Format(" and  to_char(b.obsstarttime, 'yyyy')='{0}'", year);
                    }
                    sql = string.Format(sql, sql3.Substring(0, sql3.Length - 1), sql1.Substring(0, sql1.Length - 1), sql2.Substring(0, sql2.Length - 1), sqlWhere, sql4);
                    dt = observerecordbll.GetTable(sql);
                    string tableSql = @"select
                                       {0}
                                  from bis_observesafety s
                                  left join bis_observerecord b
                                    on b.id = s.recordid
                                 where s.issafety = 0 {2}
                                 union all
                               select {1}
                                  from bis_observerecord b
                                  left join bis_observesafety s
                                    on s.recordid = b.id where 1=1 {2}";
                    tableSql = string.Format(tableSql, sql1.Substring(0, sql1.Length - 1), sql2.Substring(0, sql2.Length - 1), sqlWhere);
                    DataTable dt1 = observerecordbll.GetTable(tableSql);

                    DataTable clone = new DataTable();
                    clone = dt1.Clone();
                    for (int i = 0; i < clone.Columns.Count; i++)
                    {
                        clone.Columns[i].DataType = typeof(string);
                    }
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow newRow = clone.NewRow();
                        newRow["m1"] = dt1.Rows[i]["m1"];
                        newRow["m2"] = dt1.Rows[i]["m2"];
                        newRow["m3"] = dt1.Rows[i]["m3"];
                        newRow["m4"] = dt1.Rows[i]["m4"];
                        newRow["m5"] = dt1.Rows[i]["m5"];
                        newRow["m6"] = dt1.Rows[i]["m6"];
                        newRow["m7"] = dt1.Rows[i]["m7"];
                        newRow["m8"] = dt1.Rows[i]["m8"];
                        newRow["m9"] = dt1.Rows[i]["m9"];
                        newRow["m10"] = dt1.Rows[i]["m10"];
                        newRow["m11"] = dt1.Rows[i]["m11"];
                        newRow["m12"] = dt1.Rows[i]["m12"];
                        clone.Rows.Add(newRow);
                    }
                    DataRow dr = clone.NewRow();
                    dr["m1"] = dt.Compute("Sum(p1)", "") + "%";
                    dr["m2"] = dt.Compute("Sum(p2)", "") + "%";
                    dr["m3"] = dt.Compute("Sum(p3)", "") + "%";
                    dr["m4"] = dt.Compute("Sum(p4)", "") + "%";
                    dr["m5"] = dt.Compute("Sum(p5)", "") + "%";
                    dr["m6"] = dt.Compute("Sum(p6)", "") + "%";
                    dr["m7"] = dt.Compute("Sum(p7)", "") + "%";
                    dr["m8"] = dt.Compute("Sum(p8)", "") + "%";
                    dr["m9"] = dt.Compute("Sum(p9)", "") + "%";
                    dr["m10"] = dt.Compute("Sum(p10)", "") + "%";
                    dr["m11"] = dt.Compute("Sum(p11)", "") + "%";
                    dr["m12"] = dt.Compute("Sum(p12)", "") + "%";

                    clone.Rows.Add(dr);

                    clone.Columns.Add("name", typeof(string));
                    for (int i = 0; i < clone.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            clone.Rows[i]["name"] = "����ȫ��Ϊ����";
                        }
                        else if (i == 1)
                        {
                            clone.Rows[i]["name"] = "��Ϊ����";
                        }
                        else
                        {
                            clone.Rows[i]["name"] = "����ȫ��";
                        }
                    }
                    clone.Columns["name"].SetOrdinal(0);
                    wb.Worksheets[0].Cells.ImportDataTable(clone, false, 3, 0);
                    wb.Save(Server.UrlEncode(fName), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                    return Success("�����ɹ���");
                #endregion
                #region �۲�����Ա�ͼ
                case "db":
                    wb.Open(Server.MapPath("~/Resource/ExcelTemplate/��ȫ��Ϊ�۲�ͳ�Ʒ����Աȱ�񵼳�ģ��.xlsx"));
                    string strdbWhere = string.Empty;
                    string strYear = string.Empty;
                    string strLink = string.Empty;
                    if (!string.IsNullOrWhiteSpace(deptcode))
                    {
                        var dept = new DepartmentBLL().GetEntityByCode(deptcode);
                        if (dept.Nature == "����")
                        {
                            strdbWhere += string.Format(" and d.nature in('����','רҵ','����') and d.encode like'{0}%'", deptcode);
                            strLink += "b.workunitcode=d.encode";

                        }
                        else if (dept.Nature == "����")
                        {
                            strdbWhere += string.Format(" and d.nature in('����') and d.encode like'{0}%'", deptcode);
                            strLink += "substr(b.workunitcode, 0, length(d.encode)) = d.encode";
                        }
                        else
                        {
                            strdbWhere += string.Format(" and d.nature in('" + dept.Nature + "') and d.encode like'{0}%'", deptcode);
                            strLink += "b.workunitcode=d.encode";
                        }
                    }
                    else
                    {
                        strdbWhere += string.Format(" and d.nature in('����') and d.encode like'{0}%' ", currUser.OrganizeCode);
                        strLink += "substr(b.workunitcode, 0, length(d.encode)) = d.encode";
                    }
                    if (!string.IsNullOrWhiteSpace(year))
                    {
                        strYear = string.Format("and to_char(b.obsstarttime, 'yyyy') = '{0}'", year);
                    }
                    if (!string.IsNullOrWhiteSpace(quarter))
                    {
                        switch (quarter)
                        {
                            case "1":
                                strYear += string.Format(" and to_char(b.obsstarttime,'MM') in('01','02','03')");
                                break;
                            case "2":
                                strYear += string.Format(" and to_char(b.obsstarttime,'MM') in('04','05','06')");
                                break;
                            case "3":
                                strYear += string.Format(" and to_char(b.obsstarttime,'MM') in('07','08','09')");
                                break;
                            case "4":
                                strYear += string.Format(" and to_char(b.obsstarttime,'MM') in('10','11','12')");
                                break;
                            default:
                                break;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(month))
                    {
                        strYear += string.Format(" and to_char(b.obsstarttime, 'MM') = '{0}'", month);
                    }
                    sql = string.Format(@"select d.fullname name,nvl(t.total,0) total,'' as per from base_department d
                                    left join (select count(*) total, d.fullname,d.encode
                                                      from bis_observesafety s
                                                      left join bis_observerecord b on b.id = s.recordid
                                                      inner join (select encode, fullname
                                                                   from base_department d
                                                                  where d.description is null    and d.nature != '�а���'
                                                                    {3}
                                                                    and d.organizeid = '{0}') d
                                                        on {4}
                                                     where s.issafety = {2} and b.iscommit=1 {1} group by d.fullname,d.encode) t on t.encode=d.encode
                    where d.organizeid='{0}'  and d.nature!='�а���' and d.description is null {3}", currUser.OrganizeId, strYear, 1, strdbWhere, strLink);

                    dt = observerecordbll.GetTable(sql);
                    var totalSum = dt.Compute("Sum(total)", "").ToDouble();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dt.Rows[i]["total"]) == 0 || totalSum == 0)
                        {
                            dt.Rows[i]["per"] = 0 + "%";
                        }
                        else
                        {
                            dt.Rows[i]["per"] = Math.Round((Convert.ToSingle(dt.Rows[i]["total"]) / totalSum) * 100 ,2)+ "%";
                        }
                    }
                    DataRow drrow1 = dt.NewRow();
                    drrow1["name"] = "����";
                    drrow1["total"] = totalSum;
                    drrow1["per"] = "100%";
                    dt.Rows.Add(drrow1);

                    wb.Worksheets[0].Cells.ImportDataTable(dt, false, 2, 0);
                    sql = string.Format(@"select d.fullname name,nvl(t.total,0) total,'' as per from base_department d
                                    left join (select count(*) total, d.fullname,d.encode
                                                      from bis_observesafety s
                                                      left join bis_observerecord b on b.id = s.recordid
                                                      inner join (select encode, fullname
                                                                   from base_department d
                                                                  where d.description is null    and d.nature != '�а���'
                                                                    {3}
                                                                    and d.organizeid = '{0}') d
                                                        on {4}
                                                     where s.issafety = {2} and b.iscommit=1 {1} group by d.fullname,d.encode) t on t.encode=d.encode
                    where d.organizeid='{0}'  and d.nature!='�а���' and d.description is null {3}", currUser.OrganizeId, strYear, 0, strdbWhere, strLink);
                    DataTable dtnot = observerecordbll.GetTable(sql);
                    var totalNotSum = dtnot.Compute("Sum(total)", "").ToInt();
                    for (int i = 0; i < dtnot.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dtnot.Rows[i]["total"]) == 0 || totalNotSum == 0)
                        {
                            dtnot.Rows[i]["per"] = 0;
                        }
                        else
                        {
                            dtnot.Rows[i]["per"] =Math.Round((Convert.ToSingle(dtnot.Rows[i]["total"]) / totalNotSum) * 100,2) + "%";
                        }
                    }
                    DataRow drrow = dtnot.NewRow();
                    drrow["name"] = "����";
                    drrow["total"] = totalNotSum;
                    drrow["per"] = "100%";
                    dtnot.Rows.Add(drrow);
                    wb.Worksheets[1].Cells.ImportDataTable(dtnot, false, 2, 0);
                    wb.Save(Server.UrlEncode(fName), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                    return Success("�����ɹ���");
                #endregion

            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportAnalysisReport(string keyValue)
        {
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������

            string fileName = "��ȫ��Ϊ�۲��������_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\��ȫ��Ϊ�۲�������浼��ģ��.doc";
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("DeptName");
            dt.Columns.Add("Specialty");
            dt.Columns.Add("ReportType");
            dt.Columns.Add("TimeRange");
            dt.Columns.Add("Assayer");
            dt.Columns.Add("ParsingTime");
            dt.Columns.Add("StatisticAnalysis");
            dt.Columns.Add("ygfy");
            dt.Columns.Add("ygwz");
            dt.Columns.Add("grfhzb");
            dt.Columns.Add("gjsb");
            dt.Columns.Add("cxbz");
            dt.Columns.Add("rtgxx");
            dt.Columns.Add("hjzj");
            DataRow row = dt.NewRow();

            //��ȡ��������ʵ��
            var analysis = obsanalysisreportbll.GetEntity(keyValue);
            row["DeptName"] = analysis.WorkUnit;
            row["Specialty"] = analysis.WorkZy;
            row["ReportType"] = analysis.ReportType == "year" ? "���" : "����";
            switch (analysis.ReportType)
            {
                case "year":
                    row["TimeRange"] = analysis.Year;
                    break;
                case "quarter":
                    row["TimeRange"] = analysis.Year + "��" + analysis.Quarter + "����";
                    break;
                default:
                    break;
            }
            row["Assayer"] = analysis.AnalysisPeople;
            row["ParsingTime"] = analysis.AnalysisTime.Value.ToString("yyyy-MM-dd");
            row["StatisticAnalysis"] = analysis.AnalysisContent;
            row["ygfy"] = analysis.ygfy;
            row["ygwz"] = analysis.ygwz;
            row["grfhzb"] = analysis.grfhzb;
            row["gjsb"] = analysis.gjsb;
            row["cxbz"] = analysis.cxbz;
            row["rtgxx"] = analysis.rtgxx;
            row["hjzj"] = analysis.hjzj;

            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return Success("�����ɹ�!");
        }
        #endregion
    }
}
