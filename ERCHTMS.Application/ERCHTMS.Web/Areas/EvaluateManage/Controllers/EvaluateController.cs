using ERCHTMS.Entity.EvaluateManage;
using ERCHTMS.Busines.EvaluateManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Web;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Busines.StandardSystem;
using Aspose.Words;


namespace ERCHTMS.Web.Areas.EvaluateManage.Controllers
{
    /// <summary>
    /// �� �����Ϲ�������
    /// </summary>
    public class EvaluateController : MvcControllerBase
    {
        private EvaluateBLL evaluatebll = new EvaluateBLL();
        private FileSpotBLL filespotbll = new FileSpotBLL();
        private StandardsystemBLL standardsystembll = new StandardsystemBLL();
        private EvaluatePlanBLL evaluateplanbll = new EvaluatePlanBLL();
        private EvaluateDetailsBLL evaluatedetailsbll = new EvaluateDetailsBLL();

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
        /// ��ҳ��(����)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RectifyForm()
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
            pagination.p_fields = "WorkTitle,AppraiserUser,AppraiserUserId,DutyDept,DutyDeptCode,EvaluateState,EvaluateDate,CreateUserId,CreateUserDeptCode,CreateUserOrgCode,EvaluatePlanId,RectifyState,RectifyPerson,RectifyPersonId";
            pagination.p_tablename = "HRS_EVALUATE";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = evaluatebll.GetPageList(pagination, queryJson);
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
            var data = evaluatebll.GetList(queryJson);
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
            var data = evaluatebll.GetEntity(keyValue);
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
            evaluatebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="EvaluateState">�Ƿ��ύ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, int EvaluateState, EvaluateEntity entity)
        {
            entity.EvaluateState = EvaluateState;
            if (EvaluateState == 2) entity.EvaluateDate = DateTime.Now;//�ύʱ��������ʱ��
            evaluatebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ����������ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="EvaluateState">�Ƿ��ύ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm2(string keyValue, int EvaluateState, EvaluateEntity entity)
        {
            entity.RectifyState = EvaluateState;
            evaluatebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ��������Զ���
        /// </summary>
        /// <param name="keyValue">����ID</param>
        /// <param name="DeptName">���۲���</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm3(string keyValue, string DeptName,string EvaluatePlanId)
        {
            JObject queryJson = new JObject();
            queryJson.Add(new JProperty("DeptName", DeptName));
            var filespotlist = filespotbll.GetList(queryJson.ToString());
            evaluatebll.SaveForm3(keyValue, filespotlist, EvaluatePlanId);
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �������۱���
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "�������۱���")]
        public ActionResult ExportWord(string queryJson)
        {
            if (queryJson == null && queryJson == "")
            {
                return Error("����ʧ��!");
            }
            var queryParam = queryJson.ToJObject();
            //�ƻ�ID
            if (queryParam["EvaluatePlanId"].IsEmpty())
            {
                return Error("����ʧ��!");
            }
            ////��������
            //if (!queryParam["WorkTitle"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and WorkTitle like'{0}%'", queryParam["WorkTitle"].ToString());
            //}
            var data = evaluateplanbll.GetEntity(queryParam["EvaluatePlanId"].ToString());

            JObject queryJson1 = new JObject();
            //queryJson1.Add(new JProperty("IsConform", "0"));
            queryJson1.Add(new JProperty("EvaluatePlanId", queryParam["EvaluatePlanId"].ToString()));
            var ListData1 = evaluatedetailsbll.GetList(queryJson1.ToString());//���� ������

            //JObject queryJson2 = new JObject();
            //queryJson2.Add(new JProperty("IsConform", "1"));
            //queryJson2.Add(new JProperty("EvaluatePlanId", queryParam["EvaluatePlanId"].ToString()));
            //var ListData2 = evaluatedetailsbll.GetList(queryJson2.ToString());//������ ������
            Operator user = OperatorProvider.Provider.Current();
            Pagination pagination3 = new Pagination
            {
                page = 1,
                rows = 100000000,
                p_kid = "a.ID",
                p_fields = "a.filename,b.name as categorycode,b.id as categorycodeid,relevantelementname,relevantelementid,stationid,stationname,to_char(carrydate,'yyyy-MM-dd') as carrydate,a.createdate,consultnum,d.fullname as createuserdeptname,a.standardtype,a.createuserid,a.createuserdeptcode,a.createuserorgcode,(case  when  c.recid is null then '0' else '1' end) as isnew,a.Publishdept,e.name as maincategory,e.id as maincategoryid,filelist.filenum",
                p_tablename = @" hrs_standardsystem a left join hrs_stcategory b on a.categorycode =b.id left join hrs_standardreadrecord c on a.id =c.recid and c.createuserid ='" + user.UserId + "' left join base_department d on a.createuserdeptcode = d.encode left join hrs_stcategory e on e.id =b.parentid"
                + " left join (select id,count(fileid) as filenum from hrs_standardsystem standard left join base_fileinfo fileinfo on standard.id=fileinfo.recid group by standard.id) filelist on a.id=filelist.id ",
                conditionJson = "1=1",
                sidx = "isnew,a.createdate",
                sord = "desc"
            };
            JObject queryJson3 = new JObject();
            queryJson3.Add(new JProperty("standardtypestr", "5,6"));
            queryJson3.Add(new JProperty("carrydate", DateTime.Now.Year));
            DataTable ListData3 = standardsystembll.GetPageList(pagination3, queryJson3.ToString());//�����ʵʩ�ķ��ɷ���

            Pagination pagination4 = new Pagination
            {
                page = 1,
                rows = 100000000,
                p_kid = "a.ID",
                p_fields = "a.filename,b.name as categorycode,b.id as categorycodeid,relevantelementname,relevantelementid,stationid,stationname,to_char(carrydate,'yyyy-MM-dd') as carrydate,a.createdate,consultnum,d.fullname as createuserdeptname,a.standardtype,a.createuserid,a.createuserdeptcode,a.createuserorgcode,(case  when  c.recid is null then '0' else '1' end) as isnew,a.Publishdept,e.name as maincategory,e.id as maincategoryid,filelist.filenum",
                p_tablename = @" hrs_standardsystem a left join hrs_stcategory b on a.categorycode =b.id left join hrs_standardreadrecord c on a.id =c.recid and c.createuserid ='" + user.UserId + "' left join base_department d on a.createuserdeptcode = d.encode left join hrs_stcategory e on e.id =b.parentid"
                + " left join (select id,count(fileid) as filenum from hrs_standardsystem standard left join base_fileinfo fileinfo on standard.id=fileinfo.recid group by standard.id) filelist on a.id=filelist.id ",
                conditionJson = "1=1",
                sidx = "isnew,a.createdate",
                sord = "desc"
            };
            JObject queryJson4 = new JObject();
            queryJson4.Add(new JProperty("standardtypestr", "5,6"));
            queryJson4.Add(new JProperty("timeliness", "FZ"));
            DataTable ListData4 = standardsystembll.GetPageList(pagination4, queryJson4.ToString());//��ֹ�ķ��ɷ���

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�
            string fileName = "�Ϲ������۱���_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/�Ϲ������۱��浼��ģ��.doc");

            DataSet ds = new DataSet();
            DataTable dtPro = new DataTable("project");
            dtPro.Columns.Add("Year");
            dtPro.Columns.Add("IssueDate");
            dtPro.Columns.Add("ReviewDate");
            dtPro.Columns.Add("InMember");
            dtPro.Columns.Add("EvaluateScope");
            dtPro.Columns.Add("EvaluateGist");
            dtPro.Columns.Add("RecognizeCondition");
            dtPro.Columns.Add("EvaluateSummarize");
            dtPro.Columns.Add("EvaluateVerdict");
            //dtPro.Columns.Add("ByDept");
            //dtPro.Columns.Add("DutyUser");
            //dtPro.Columns.Add("Signature1");
            //dtPro.Columns.Add("Signature2");
            //dtPro.Columns.Add("Signature3");

            DataTable dt = new DataTable("list1");
            dt.Columns.Add("no");
            dt.Columns.Add("CategoryName");
            dt.Columns.Add("RankName");
            dt.Columns.Add("FileName");
            dt.Columns.Add("DutyDept");
            dt.Columns.Add("PutDate");
            dt.Columns.Add("NormName");
            dt.Columns.Add("Clause");
            dt.Columns.Add("ApplyScope");
            dt.Columns.Add("IsConform");
            dt.Columns.Add("Describe");
            dt.Columns.Add("Opinion");
            dt.Columns.Add("FinishTime");
            dt.Columns.Add("EvaluatePerson");

            DataTable dt2 = new DataTable("list2");
            dt2.Columns.Add("no");
            dt2.Columns.Add("filename");
            dt2.Columns.Add("publishdept");
            dt2.Columns.Add("carrydate");

            DataTable dt3 = new DataTable("list3");
            dt3.Columns.Add("no");
            dt3.Columns.Add("filename");
            dt3.Columns.Add("publishdept");
            dt3.Columns.Add("carrydate");

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            DataRow row = dtPro.NewRow();
            
            row["Year"] = data.Year;
            row["IssueDate"] = null != data ? (null != data.IssueDate ? data.IssueDate.Value.ToString("yyyy-MM-dd") : "") : "";//data.IssueDate;
            row["ReviewDate"] = data.ReviewDate;
            row["InMember"] = data.InMember;
            row["EvaluateScope"] = data.EvaluateScope;
            row["EvaluateGist"] = data.EvaluateGist;
            row["RecognizeCondition"] = data.RecognizeCondition;
            row["EvaluateSummarize"] = data.EvaluateSummarize;
            row["EvaluateVerdict"] = data.EvaluateVerdict;
            dtPro.Rows.Add(row);

            int s = 0;
            foreach (var obj in ListData1)
            {
                DataRow dtrow = dt.NewRow();
                s = (s + 1);
                dtrow["no"] = s;
                dtrow["CategoryName"] = obj.CategoryName;
                dtrow["RankName"] = obj.RankName;
                dtrow["FileName"] = obj.FileName;
                dtrow["DutyDept"] = obj.DutyDept;
                dtrow["PutDate"] = null != obj ? (null != obj.PutDate ? obj.PutDate.Value.ToString("yyyy-MM-dd") : "") : "";//obj.PutDate.Value.ToString("yyyy-MM-dd");
                dtrow["NormName"] = obj.NormName;
                dtrow["Clause"] = obj.Clause;
                dtrow["ApplyScope"] = obj.ApplyScope;
                dtrow["IsConform"] = obj.IsConform == 0 ? "����" : "������";
                dtrow["Describe"] = obj.Describe;
                dtrow["Opinion"] = obj.Opinion;
                dtrow["FinishTime"] = obj.FinishTime;
                dtrow["EvaluatePerson"] = obj.EvaluatePerson;
                dt.Rows.Add(dtrow);
            }
            ds.Tables.Add(dt);

            for (int i = 0; i < ListData3.Rows.Count; i++)
            {
                DataRow dtrow = dt2.NewRow();
                dtrow["no"] = (i + 1);
                dtrow["filename"] = ListData3.Rows[i]["filename"];
                dtrow["publishdept"] = ListData3.Rows[i]["publishdept"];
                dtrow["carrydate"] = ListData3.Rows[i]["carrydate"];
                dt2.Rows.Add(dtrow);
            }
            ds.Tables.Add(dt2);

            for (int i = 0; i < ListData4.Rows.Count; i++)
            {
                DataRow dtrow = dt3.NewRow();
                dtrow["no"] = (i + 1);
                dtrow["filename"] = ListData4.Rows[i]["filename"];
                dtrow["publishdept"] = ListData4.Rows[i]["publishdept"];
                dtrow["carrydate"] = ListData4.Rows[i]["carrydate"];
                dt3.Rows.Add(dtrow);
            }
            ds.Tables.Add(dt3);

            ds.Tables.Add(dtPro);

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            doc.MailMerge.Execute(dtPro);
            doc.MailMerge.ExecuteWithRegions(dt);
            doc.MailMerge.ExecuteWithRegions(dt2);
            doc.MailMerge.ExecuteWithRegions(dt3);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return Success("�����ɹ�!");
        }
        #endregion
    }
}
