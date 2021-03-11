using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.Busines.AssessmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using Aspose.Words;
using System.Data;
using Aspose.Words.Saving;
using System;
using System.Collections.Generic;
using System.Web;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.AssessmentManage.Controllers
{
    /// <summary>
    /// �� ���������ܽ�
    /// </summary>
    public class AssessmentSumController : MvcControllerBase
    {
        private AssessmentSumBLL assessmentsumbll = new AssessmentSumBLL();
        private AssessmentPlanBLL assessmentplanbll = new AssessmentPlanBLL();

        /// <summary>
        /// ����������
        /// </summary>
        private int pCount;

        /// <summary>
        /// ���п۷���
        /// </summary>
        private int kscoreCount;

        /// <summary>
        /// ʵ�÷�
        /// </summary>
        private int reallyscore;
        /// <summary>
        /// ���������
        /// </summary>
        private int noSuitScore;
        /// <summary>
        /// ��׼�÷�
        /// </summary>
        private int bscore;

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
        /// ��������׫дҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SumIndex()
        {
            return View();
        }


        /// <summary>
        /// ����׫дҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SumUp()
        {
            return View();
        }

        /// <summary>
        /// ��������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelfReport()
        {
            return View();
        }

        /// <summary>
        /// ���߱༭ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OpenWordReport()
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
            var data = assessmentsumbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListDuty(Pagination pagination, string queryJson, string kid)
        {
            pagination.p_kid = "Id as sumid";
            pagination.p_fields = "DutyID,DutyName,SumName";
            pagination.p_tablename = "bis_assessmentsum";
            pagination.conditionJson = string.Format("AssessmentPlanID='{0}'", kid);
            pagination.sidx = @"cast(replace(regexp_substr(SumName,'\d+.\d+'),'.','') as number)";
            pagination.sord = "asc";
            var data = assessmentsumbll.GetPageList(pagination, queryJson);
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
            var data = assessmentsumbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetSumUpPageJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "Id";
            pagination.p_fields = "PlanName,TeamLeader,TeamLeaderName,'' as progress,'' as status,LeaderSum,'' as isUpdate,'' as report";
            pagination.p_tablename = "bis_assessmentplan";
            pagination.conditionJson = "1=1 and createuserorgcode='" + user.OrganizeCode + "'";
            var data = assessmentsumbll.GetSumUpPageJson(pagination, queryJson);
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
            assessmentsumbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, AssessmentSumEntity entity)
        {
            assessmentsumbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ����ÿ���Ƿ����ύ��������ɸѡ
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chaperid"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSumInfo(string planid, string chaperid)
        {
            var message = "";
            AssessmentSumEntity resultEntity = assessmentsumbll.GetSumByPlanOrChapID(planid, chaperid);
            if (resultEntity != null)
            {
                if (resultEntity.Reserve != "��ɸѡ")
                {
                    resultEntity.Reserve = "��ɸѡ";
                    assessmentsumbll.SaveForm(resultEntity.Id, resultEntity);
                    message = "����ɴ��������ɸѡ��";
                }
                else
                {
                    message = "���޶����������ɸѡ��";
                }
            }
            return Success(message);
        }


        /// <summary>
        /// ����ÿ���Ƿ����ύ����
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chaperid"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSumKSocreInfo(string planid, string chaperid)
        {
            var message = "";
            AssessmentSumEntity resultEntity = assessmentsumbll.GetSumByPlanOrChapID(planid, chaperid);
            if (resultEntity != null)
            {
                if (resultEntity.Reserve == "��ɸѡ")
                {
                    if (resultEntity.GradeStatus != "������")
                    {
                        resultEntity.GradeStatus = "������";
                        assessmentsumbll.SaveForm(resultEntity.Id, resultEntity);
                        message = "����ɴ������֡�";
                    }
                    else
                    {
                        message = "���޶��������֡�";
                    }
                }
                else
                {
                    message = "������ɲ�����ɸѡ���ύ��";
                }

            }
            return Success(message);
        }



        /// <summary>
        /// ���������ܽ�
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chaperid"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSelfSumInfo(string planid, string chaperid, string selfsum)
        {
            AssessmentSumEntity resultEntity = assessmentsumbll.GetSumByPlanOrChapID(planid, chaperid);
            if (resultEntity != null)
            {
                resultEntity.SelfSum = selfsum;//�����ܽ�
                resultEntity.SelfSumDate = System.DateTime.Now;
                assessmentsumbll.SaveForm(resultEntity.Id, resultEntity);
            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="sumdetail"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSummarizeInfo(string planid, string sumdetail)
        {
            AssessmentPlanEntity resultEntity = assessmentplanbll.GetEntity(planid);
            if (resultEntity != null)
            {
                resultEntity.LeaderSum = sumdetail;//����
                assessmentplanbll.SaveForm(resultEntity.Id, resultEntity);
            }
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ���������ƻ�����״̬
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SavePlanStatusInfo(string planid)
        {
            AssessmentPlanEntity resultEntity = assessmentplanbll.GetEntity(planid);
            if (resultEntity != null)
            {
                if (GenReport(resultEntity))
                {
                    resultEntity.Status = "�����";//����״̬
                    assessmentplanbll.SaveForm(resultEntity.Id, resultEntity);
                }
            }
            return Success("�ѳɹ����ɱ��档");
        }


        /// <summary>
        /// ��ȡ�½���Ϣ
        /// </summary>
        /// <param name="chaperid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSummarizeInfo(string planid, string chaperid)
        {
            var data = assessmentsumbll.GetSummarizeInfo(planid, chaperid);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�������������
        /// </summary>
        /// <param name="chaperid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSumDataInfo(string planid)
        {
            var data = assessmentsumbll.GetSumDataInfo(planid);
            return ToJsonResult(data);
        }

        /// <summary>
        ///��ȡͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        [HttpGet]
        public string GetSumDataCount(string planid = "")
        {
            return assessmentsumbll.GetSumDataCount(planid);
        }
        #endregion

        #region ���߱༭
        [HttpPost]
        public void GetWordSelfReprot(string filename)
        {
            filename = HttpUtility.UrlDecode(filename);
            PageOffice.PageOfficeCtrl PageOfficeCtrl1 = new PageOffice.PageOfficeCtrl();
            PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
            PageOfficeCtrl1.SaveFilePage = Request.ApplicationPath + "/AssessmentManage/AssessmentSum/SaveFile?filename=" + HttpUtility.UrlEncode(filename);
            PageOfficeCtrl1.Titlebar = true; //���ر�����
            PageOfficeCtrl1.Menubar = true; //���ز˵���
            PageOfficeCtrl1.CustomToolbar = true; //�����Զ��幤����
            PageOfficeCtrl1.OfficeToolbars = true; //����Office������
            PageOfficeCtrl1.Theme = PageOffice.ThemeType.CustomStyle;
            string filePath = Server.MapPath(string.Format("~/Resource/ExcelTemplate/SelfReport/{0}", filename));
            PageOfficeCtrl1.WebOpen(filePath, PageOffice.OpenModeType.docNormalEdit, "lm");
            Response.Write(PageOfficeCtrl1.GetHtmlCode("PageOfficeCtrl1"));
            Response.End();
        }


        /// <summary>
        /// ƽ̨�޶�
        /// </summary>>
        public void SaveFile(string filename)
        {
            string uploadDir = Server.MapPath(string.Format("~/Resource/ExcelTemplate/SelfReport/{0}",HttpUtility.HtmlDecode(filename)));
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            fs.SaveToFile(uploadDir);
            fs.Close();
        }
        #endregion


        #region  ����
        /// <summary>
        /// ����ģ��������������
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool GenReport(AssessmentPlanEntity entity)
        {
            bool b = true;
            try
            {
                pCount = 0; kscoreCount = 0; reallyscore = 0; noSuitScore = 0; bscore = 0;
                string fileName = Server.MapPath("~/Resource/ExcelTemplate/��������ģ��.doc");
                Document doc = new Document(fileName);
                DataSet ds = GetDataSet(entity);
                doc.MailMerge.ExecuteWithRegions(ds);
                string reportName = Server.MapPath(string.Format("~/Resource/ExcelTemplate/SelfReport/{0}-��������.doc", entity.PlanName));
                doc.Save(reportName, SaveOptions.CreateSaveOptions(SaveFormat.Doc));
                b = true;
            }
            catch (Exception ex)
            {
                b = false;
                throw ex;
            }

            return b;
        }



        /// <summary>
        /// ��ȡģ��ʹ�õ����ݼ�
        /// </summary>
        /// <returns></returns>
        private DataSet GetDataSet(AssessmentPlanEntity plan)
        {
            DataSet ds = new DataSet();

            //4.����������չ���
            DataTable dtA = GetA(plan);
            ds.Tables.Add(dtA);
            //5.רҵ��֤���
            ds.Tables.Add(GetItems(plan, "Item1", 500, new List<string>() { "5.1", "5.2", "5.3", "5.4", "5.5", "5.8", "5.9", "5.10", "5.11", "5.12", "5.13" }));
            ds.Tables.Add(GetItems(plan, "Item2", 500, new List<string>() { "5.7", "5.10" }));
            ds.Tables.Add(GetItems(plan, "Item3", 500, new List<string>() {
                           "'5.6.3.4'", "'5.6.4.3.1'", "'5.6.4.3.2'", "'5.6.4.3.3'", "'5.6.4.3.4'", "'5.6.4.3.5'", "'5.6.4.3.6'", "'5.6.3.7'", "'5.6.3.8'", "'5.6.4.6.2'",
                           "'5.6.3.5'", "'5.6.4.4.1'","'5.6.4.4.2'","'5.6.4.4.3'","'5.6.4.4.4'", "'5.6.4.8.1'","'5.6.4.8.2'","'5.6.4.8.3'", "'5.6.3.6'",
                           "'5.6.3.1'", "'5.6.3.2'", "'5.6.4.1.1'","'5.6.4.1.2'","'5.6.4.1.3'","'5.6.4.1.4'","'5.6.4.1.5'","'5.6.4.1.6'","'5.6.4.1.7'",
                           "'5.6.3.3'", "'5.6.3.9'", "'5.6.4.2.1'","'5.6.4.2.2'",
                           "'5.6.1.1'","'5.6.1.2'","'5.6.1.3'","'5.6.1.4'","'5.6.1.5'","'5.6.1.6'","'5.6.2.1'","'5.6.2.2'","'5.6.2.3'","'5.6.2.4'", "'5.6.5.1'","'5.6.5.2'","'5.6.5.3'","'5.6.5.4'"
             }));
            ds.Tables.Add(GetItems(plan, "Item3-1", 100, new List<string>() { "'5.6.3.4'", "'5.6.4.3.1'", "'5.6.4.3.2'", "'5.6.4.3.3'", "'5.6.4.3.4'", "'5.6.4.3.5'", "'5.6.4.3.6'", "'5.6.3.7'", "'5.6.3.8'", "'5.6.4.6.2'" }));
            ds.Tables.Add(GetItems(plan, "Item3-2", 100, new List<string>() { "'5.6.3.5'", "'5.6.4.4.1'", "'5.6.4.4.2'", "'5.6.4.4.3'", "'5.6.4.4.4'", "'5.6.4.8.1'", "'5.6.4.8.2'", "'5.6.4.8.3'", "'5.6.3.6'" }));
            ds.Tables.Add(GetItems(plan, "Item3-3", 90, new List<string>() { "'5.6.3.1'", "'5.6.3.2'", "'5.6.4.1.1'", "'5.6.4.1.2'", "'5.6.4.1.3'", "'5.6.4.1.4'", "'5.6.4.1.5'", "'5.6.4.1.6'", "'5.6.4.1.7'" }));
            ds.Tables.Add(GetItems(plan, "Item3-4", 50, new List<string>() { "'5.6.3.3'", "'5.6.3.9'", "'5.6.4.2.1'", "'5.6.4.2.2'" }));
            ds.Tables.Add(GetItems(plan, "Item3-5", 160, new List<string>() { "'5.6.1.1'", "'5.6.1.2'", "'5.6.1.3'", "'5.6.1.4'", "'5.6.1.5'", "'5.6.1.6'", "'5.6.2.1'", "'5.6.2.2'", "'5.6.2.3'", "'5.6.2.4'", "'5.6.5.1'", "'5.6.5.2'", "'5.6.5.3'", "'5.6.5.4'" }));
            ds.Tables.Add(GetItems(plan, "Item4", 100, new List<string>() { "'5.6.3.4'", "'5.6.4.3.1'", "'5.6.4.3.2'", "'5.6.4.3.3'", "'5.6.4.3.4'", "'5.6.4.3.5'", "'5.6.4.3.6'", "'5.6.3.7'", "'5.6.3.8'", "'5.6.4.6.2'" }));
            ds.Tables.Add(GetItems(plan, "Item5", 100, new List<string>() { "'5.6.3.5'", "'5.6.4.4.1'", "'5.6.4.4.2'", "'5.6.4.4.3'", "'5.6.4.4.4'", "'5.6.4.8.1'", "'5.6.4.8.2'", "'5.6.4.8.3'", "'5.6.3.6'" }));
            ds.Tables.Add(GetItems(plan, "Item6", 90, new List<string>() { "'5.6.3.1'", "'5.6.3.2'", "'5.6.4.1.1'", "'5.6.4.1.2'", "'5.6.4.1.3'", "'5.6.4.1.4'", "'5.6.4.1.5'", "'5.6.4.1.6'", "'5.6.4.1.7'" }));
            ds.Tables.Add(GetItems(plan, "Item7", 50, new List<string>() { "'5.6.3.3'", "'5.6.3.9'", "'5.6.4.2.1'", "'5.6.4.2.2'" }));
            ds.Tables.Add(GetItems(plan, "Item8", 160, new List<string>() { "'5.6.1.1'", "'5.6.1.2'", "'5.6.1.3'", "'5.6.1.4'", "'5.6.1.5'", "'5.6.1.6'", "'5.6.2.1'", "'5.6.2.2'", "'5.6.2.3'", "'5.6.2.4'", "'5.6.5.1'", "'5.6.5.2'", "'5.6.5.3'", "'5.6.5.4'" }));
            //6.�������
            DataTable rd = GetRD(plan);
            ds.Tables.Add(GetResult(plan));
            ds.Tables.Add(rd);
            ds.Tables.Add(GetRC(rd));
            //����1����ȫ������׼����������������ֵ���Ҫ���⡢���Ĵ�ʩ
            ds.Tables.Add(GetA1(plan));

            return ds;
        }


        #region 4.����������չ���
        private DataTable GetA(AssessmentPlanEntity plan)
        {
            DataTable dtA = new DataTable("A");
            Random rnd = new Random((int)DateTime.Now.Ticks);

            dtA.Columns.Add("Headman");
            dtA.Columns.Add("Member1");
            dtA.Columns.Add("Member2");
            dtA.Columns.Add("Member31");
            dtA.Columns.Add("Member32");
            dtA.Columns.Add("Member33");
            dtA.Columns.Add("Member34");
            dtA.Columns.Add("Member35");

            string str = assessmentsumbll.GetEveryBigPerson(plan.Id, "1");
            string strD = assessmentsumbll.GetEveryBigPerson(plan.Id, "2");
            string strOne = assessmentsumbll.GetEveryBigPerson(plan.Id, "3");
            DataRow row = dtA.NewRow();
            row["Headman"] = plan.TeamLeaderName;
            row["Member1"] = str;
            row["Member2"] = strD;
            row["Member31"] = strOne;
            row["Member32"] = strOne;
            row["Member33"] = strOne;
            row["Member34"] = strOne;
            row["Member35"] = strOne;
            dtA.Rows.Add(row);

            return dtA;
        }
        #endregion

        #region 5.רҵ��֤���
        private DataTable GetItems(AssessmentPlanEntity plan, string itemName, int targetScore, List<string> chapterNo)
        {
            DataTable dtItem = new DataTable(itemName);

            dtItem.Columns.Add("RealScore");
            dtItem.Columns.Add("Percentage");
            if (!itemName.Contains("Item3"))
                dtItem.Columns.Add("NoScorePoints");

            int num = 0;
            string kScoreInfo = "";
            if (itemName == "Item1")
            {
                num = assessmentsumbll.GetEverySumScore(plan.Id);
                kScoreInfo = assessmentsumbll.GetEveryResonAndScore(plan.Id);
            }
            else if (itemName == "Item2")
            {
                num = assessmentsumbll.GetEverySumScore2(plan.Id);
                kScoreInfo = assessmentsumbll.GetEveryResonAndScore2(plan.Id);
            }
            else
            {
                string str = chapterNo.Count > 0 ? "" : "-1";//������
                for (int i = 0; i < chapterNo.Count; i++)
                {
                    str += chapterNo[i] + ",";
                }
                if (str.Contains(","))
                    str = str.TrimEnd(',');
                num = assessmentsumbll.GetEverySumScore3(plan.Id, str);
                if (!itemName.Contains("Item3"))
                    kScoreInfo = assessmentsumbll.GetEveryResonAndScore3(plan.Id, str);
            }
            DataRow row = dtItem.NewRow();
            row["RealScore"] = (targetScore - num).ToString();
            if ((targetScore - num).ToString() == "0")//�Ա�����Ϊ0������
                row["Percentage"] = 0;
            else
                row["Percentage"] = Math.Round((float.Parse(row["RealScore"].ToString()) / targetScore) * 100.0f, 2);
            if (!itemName.Contains("Item3"))
                row["NoScorePoints"] = kScoreInfo;
            dtItem.Rows.Add(row);

            return dtItem;
        }
        #endregion

        #region 6.�������
        private DataTable GetResult(AssessmentPlanEntity plan)
        {
            DataTable dt = new DataTable("Result");
            dt.Columns.Add("TotalNum");//�ܲ�����
            dt.Columns.Add("StdScore");//��׼��
            dt.Columns.Add("NoScoreNum");//�۷���Ŀ
            dt.Columns.Add("RealScore");//ʵ�ʵ÷�
            dt.Columns.Add("Score");//�����ۺϵ÷�
            int bScore = assessmentsumbll.GetBigChapterScore();//��׼��
            DataRow row = dt.NewRow();
            row["TotalNum"] = pCount;
            row["StdScore"] = bScore;
            row["RealScore"] = reallyscore;
            if (reallyscore != 0)
            {
                int dScore = bscore - noSuitScore;
                if (dScore != 0)
                    row["Score"] = Math.Round(((double)reallyscore / (double)(bscore - noSuitScore)) * 100, 2) + "%";//�ۺϵ÷�
                else
                    row["Score"] = 0;
            }
            else
                row["Score"] = 0;
            row["NoScoreNum"] = kscoreCount;
            dt.Rows.Add(row);

            return dt;
        }
        private DataTable GetRD(AssessmentPlanEntity plan)
        {
            DataTable dt = new DataTable("RD");


            dt.Columns.Add("IX");
            dt.Columns.Add("IN");
            dt.Columns.Add("TN");
            dt.Columns.Add("NN");
            dt.Columns.Add("NS");
            dt.Columns.Add("TS");
            dt.Columns.Add("RS");
            dt.Columns.Add("PR");

            DataTable dtChapter = assessmentsumbll.GetEveryBigNoSuitScore(plan.Id);
            for (int i = 0; i < dtChapter.Rows.Count; i++)
            {
                DataRow row = dt.NewRow();
                row["IX"] = dtChapter.Rows[i][1];//��Ŀ���
                row["IN"] = dtChapter.Rows[i][2];//רҵ����
                row["TN"] = dtChapter.Rows[i][4];//������=����-��������
                pCount += int.Parse(dtChapter.Rows[i][4].ToString());
                row["NN"] = dtChapter.Rows[i][3];//��������
                row["NS"] = dtChapter.Rows[i][5];//�۷���
                kscoreCount += int.Parse(dtChapter.Rows[i][5].ToString());
                row["TS"] = dtChapter.Rows[i][6];//Ӧ�÷�
                bscore += int.Parse(dtChapter.Rows[i][6].ToString());
                row["RS"] = dtChapter.Rows[i][8];//ʵ�÷�
                reallyscore += int.Parse(dtChapter.Rows[i][8].ToString());
                row["PR"] = dtChapter.Rows[i][9];//�÷���
                noSuitScore += int.Parse(dtChapter.Rows[i][10].ToString());//�÷���
                dt.Rows.Add(row);
            }

            return dt;
        }
        private DataTable GetRC(DataTable rd)
        {
            DataTable dt = new DataTable("RC");

            dt.Columns.Add("CTN");
            dt.Columns.Add("CNN");
            dt.Columns.Add("CNS");
            dt.Columns.Add("CTS");
            dt.Columns.Add("CRS");
            dt.Columns.Add("CPR");
            int ctn = 0, cnn = 0, cns = 0, cts = 0, crs = 0; float cpr = 0;

            foreach (DataRow row in rd.Rows)
            {
                ctn += int.Parse(row["TN"].ToString());
                cnn += int.Parse(row["NN"].ToString());
                cns += int.Parse(row["NS"].ToString());
                cts += int.Parse(row["TS"].ToString());
                crs += int.Parse(row["RS"].ToString());
                cpr += float.Parse(row["PR"].ToString());
            }
            DataRow item = dt.NewRow();
            item["CTN"] = ctn.ToString();
            item["CNN"] = cnn.ToString();
            item["CNS"] = cns.ToString();
            item["CTS"] = cts.ToString();
            item["CRS"] = crs.ToString();
            item["CPR"] = Math.Round(cpr / rd.Rows.Count, 2);
            dt.Rows.Add(item);

            return dt;
        }
        #endregion

        #region ��ȡ����һ
        private DataTable GetA1(AssessmentPlanEntity plan)
        {
            DataTable dt = new DataTable("A1");

            Random rnd = new Random((int)DateTime.Now.Ticks);

            dt.Columns.Add("Index");
            dt.Columns.Add("NoAndName");
            dt.Columns.Add("Problem");
            dt.Columns.Add("TS");
            dt.Columns.Add("NS");
            dt.Columns.Add("RS");
            dt.Columns.Add("CorrectMeasures");
            DataTable dtAddUp = assessmentsumbll.GetAffixOne(plan.Id);
            for (int i = 0; i < dtAddUp.Rows.Count; i++)
            {
                DataRow row = dt.NewRow();
                row["Index"] = (i + 1).ToString();
                row["NoAndName"] = dtAddUp.Rows[i]["majorNumber"].ToString() + dtAddUp.Rows[i]["ChaptersName"].ToString();
                row["Problem"] = dtAddUp.Rows[i]["kScoreReason"].ToString();
                row["TS"] = dtAddUp.Rows[i]["score"].ToString();
                row["NS"] = dtAddUp.Rows[i]["kScore"].ToString();
                row["RS"] = dtAddUp.Rows[i]["rScore"].ToString();
                row["CorrectMeasures"] = dtAddUp.Rows[i]["Measure"].ToString();
                dt.Rows.Add(row);
            }

            return dt;
        }
        #endregion
        #endregion
    }
}
