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
    /// 描 述：自评总结
    /// </summary>
    public class AssessmentSumController : MvcControllerBase
    {
        private AssessmentSumBLL assessmentsumbll = new AssessmentSumBLL();
        private AssessmentPlanBLL assessmentplanbll = new AssessmentPlanBLL();

        /// <summary>
        /// 所有评分项
        /// </summary>
        private int pCount;

        /// <summary>
        /// 所有扣分项
        /// </summary>
        private int kscoreCount;

        /// <summary>
        /// 实得分
        /// </summary>
        private int reallyscore;
        /// <summary>
        /// 不适宜项分
        /// </summary>
        private int noSuitScore;
        /// <summary>
        /// 标准得分
        /// </summary>
        private int bscore;

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 自评综述撰写页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SumIndex()
        {
            return View();
        }


        /// <summary>
        /// 综述撰写页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SumUp()
        {
            return View();
        }

        /// <summary>
        /// 自评报告页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelfReport()
        {
            return View();
        }

        /// <summary>
        /// 在线编辑页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OpenWordReport()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = assessmentsumbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = assessmentsumbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
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

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            assessmentsumbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, AssessmentSumEntity entity)
        {
            assessmentsumbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }


        /// <summary>
        /// 保存每项是否已提交不适宜项筛选
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
                if (resultEntity.Reserve != "已筛选")
                {
                    resultEntity.Reserve = "已筛选";
                    assessmentsumbll.SaveForm(resultEntity.Id, resultEntity);
                    message = "已完成此项不适宜项筛选。";
                }
                else
                {
                    message = "已修订此项不适宜项筛选。";
                }
            }
            return Success(message);
        }


        /// <summary>
        /// 保存每项是否已提交评分
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
                if (resultEntity.Reserve == "已筛选")
                {
                    if (resultEntity.GradeStatus != "已评分")
                    {
                        resultEntity.GradeStatus = "已评分";
                        assessmentsumbll.SaveForm(resultEntity.Id, resultEntity);
                        message = "已完成此项评分。";
                    }
                    else
                    {
                        message = "已修订此项评分。";
                    }
                }
                else
                {
                    message = "请先完成不适宜筛选并提交。";
                }

            }
            return Success(message);
        }



        /// <summary>
        /// 保存自评总结
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
                resultEntity.SelfSum = selfsum;//自评总结
                resultEntity.SelfSumDate = System.DateTime.Now;
                assessmentsumbll.SaveForm(resultEntity.Id, resultEntity);
            }
            return Success("操作成功。");
        }

        /// <summary>
        /// 保存自评综述
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
                resultEntity.LeaderSum = sumdetail;//综述
                assessmentplanbll.SaveForm(resultEntity.Id, resultEntity);
            }
            return Success("操作成功。");
        }


        /// <summary>
        /// 更改自评计划整体状态
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
                    resultEntity.Status = "已完成";//整体状态
                    assessmentplanbll.SaveForm(resultEntity.Id, resultEntity);
                }
            }
            return Success("已成功生成报告。");
        }


        /// <summary>
        /// 获取章节信息
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
        /// 获取综述等相关数据
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
        ///获取统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet]
        public string GetSumDataCount(string planid = "")
        {
            return assessmentsumbll.GetSumDataCount(planid);
        }
        #endregion

        #region 在线编辑
        [HttpPost]
        public void GetWordSelfReprot(string filename)
        {
            filename = HttpUtility.UrlDecode(filename);
            PageOffice.PageOfficeCtrl PageOfficeCtrl1 = new PageOffice.PageOfficeCtrl();
            PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
            PageOfficeCtrl1.SaveFilePage = Request.ApplicationPath + "/AssessmentManage/AssessmentSum/SaveFile?filename=" + HttpUtility.UrlEncode(filename);
            PageOfficeCtrl1.Titlebar = true; //隐藏标题栏
            PageOfficeCtrl1.Menubar = true; //隐藏菜单栏
            PageOfficeCtrl1.CustomToolbar = true; //隐藏自定义工具栏
            PageOfficeCtrl1.OfficeToolbars = true; //隐藏Office工具栏
            PageOfficeCtrl1.Theme = PageOffice.ThemeType.CustomStyle;
            string filePath = Server.MapPath(string.Format("~/Resource/ExcelTemplate/SelfReport/{0}", filename));
            PageOfficeCtrl1.WebOpen(filePath, PageOffice.OpenModeType.docNormalEdit, "lm");
            Response.Write(PageOfficeCtrl1.GetHtmlCode("PageOfficeCtrl1"));
            Response.End();
        }


        /// <summary>
        /// 平台修订
        /// </summary>>
        public void SaveFile(string filename)
        {
            string uploadDir = Server.MapPath(string.Format("~/Resource/ExcelTemplate/SelfReport/{0}",HttpUtility.HtmlDecode(filename)));
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            fs.SaveToFile(uploadDir);
            fs.Close();
        }
        #endregion


        #region  报告
        /// <summary>
        /// 根据模板生成自评报告
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool GenReport(AssessmentPlanEntity entity)
        {
            bool b = true;
            try
            {
                pCount = 0; kscoreCount = 0; reallyscore = 0; noSuitScore = 0; bscore = 0;
                string fileName = Server.MapPath("~/Resource/ExcelTemplate/自评报告模板.doc");
                Document doc = new Document(fileName);
                DataSet ds = GetDataSet(entity);
                doc.MailMerge.ExecuteWithRegions(ds);
                string reportName = Server.MapPath(string.Format("~/Resource/ExcelTemplate/SelfReport/{0}-自评报告.doc", entity.PlanName));
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
        /// 获取模板使用的数据集
        /// </summary>
        /// <returns></returns>
        private DataSet GetDataSet(AssessmentPlanEntity plan)
        {
            DataSet ds = new DataSet();

            //4.自评工作开展情况
            DataTable dtA = GetA(plan);
            ds.Tables.Add(dtA);
            //5.专业查证情况
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
            //6.自评结果
            DataTable rd = GetRD(plan);
            ds.Tables.Add(GetResult(plan));
            ds.Tables.Add(rd);
            ds.Tables.Add(GetRC(rd));
            //附件1：安全生产标准化达标评级自评发现的主要问题、整改措施
            ds.Tables.Add(GetA1(plan));

            return ds;
        }


        #region 4.自评工作开展情况
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

        #region 5.专业查证情况
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
                string str = chapterNo.Count > 0 ? "" : "-1";//不存在
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
            if ((targetScore - num).ToString() == "0")//对被除数为0做处理
                row["Percentage"] = 0;
            else
                row["Percentage"] = Math.Round((float.Parse(row["RealScore"].ToString()) / targetScore) * 100.0f, 2);
            if (!itemName.Contains("Item3"))
                row["NoScorePoints"] = kScoreInfo;
            dtItem.Rows.Add(row);

            return dtItem;
        }
        #endregion

        #region 6.自评结果
        private DataTable GetResult(AssessmentPlanEntity plan)
        {
            DataTable dt = new DataTable("Result");
            dt.Columns.Add("TotalNum");//总查评项
            dt.Columns.Add("StdScore");//标准分
            dt.Columns.Add("NoScoreNum");//扣分项目
            dt.Columns.Add("RealScore");//实际得分
            dt.Columns.Add("Score");//自评综合得分
            int bScore = assessmentsumbll.GetBigChapterScore();//标准分
            DataRow row = dt.NewRow();
            row["TotalNum"] = pCount;
            row["StdScore"] = bScore;
            row["RealScore"] = reallyscore;
            if (reallyscore != 0)
            {
                int dScore = bscore - noSuitScore;
                if (dScore != 0)
                    row["Score"] = Math.Round(((double)reallyscore / (double)(bscore - noSuitScore)) * 100, 2) + "%";//综合得分
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
                row["IX"] = dtChapter.Rows[i][1];//项目序号
                row["IN"] = dtChapter.Rows[i][2];//专业名称
                row["TN"] = dtChapter.Rows[i][4];//评分项=总项-不适宜项
                pCount += int.Parse(dtChapter.Rows[i][4].ToString());
                row["NN"] = dtChapter.Rows[i][3];//不适宜项
                row["NS"] = dtChapter.Rows[i][5];//扣分项
                kscoreCount += int.Parse(dtChapter.Rows[i][5].ToString());
                row["TS"] = dtChapter.Rows[i][6];//应得分
                bscore += int.Parse(dtChapter.Rows[i][6].ToString());
                row["RS"] = dtChapter.Rows[i][8];//实得分
                reallyscore += int.Parse(dtChapter.Rows[i][8].ToString());
                row["PR"] = dtChapter.Rows[i][9];//得分率
                noSuitScore += int.Parse(dtChapter.Rows[i][10].ToString());//得分率
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

        #region 获取附件一
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
