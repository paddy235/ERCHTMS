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
    /// 描 述：合规性评价
    /// </summary>
    public class EvaluateController : MvcControllerBase
    {
        private EvaluateBLL evaluatebll = new EvaluateBLL();
        private FileSpotBLL filespotbll = new FileSpotBLL();
        private StandardsystemBLL standardsystembll = new StandardsystemBLL();
        private EvaluatePlanBLL evaluateplanbll = new EvaluatePlanBLL();
        private EvaluateDetailsBLL evaluatedetailsbll = new EvaluateDetailsBLL();

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
        /// 表单页面(整改)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RectifyForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = evaluatebll.GetList(queryJson);
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
            var data = evaluatebll.GetEntity(keyValue);
            return ToJsonResult(data);
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
            evaluatebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="EvaluateState">是否提交</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, int EvaluateState, EvaluateEntity entity)
        {
            entity.EvaluateState = EvaluateState;
            if (EvaluateState == 2) entity.EvaluateDate = DateTime.Now;//提交时更新评价时间
            evaluatebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存表单（整改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="EvaluateState">是否提交</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm2(string keyValue, int EvaluateState, EvaluateEntity entity)
        {
            entity.RectifyState = EvaluateState;
            evaluatebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存表单（自动）
        /// </summary>
        /// <param name="keyValue">主表ID</param>
        /// <param name="DeptName">评价部门</param>
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
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出评价报告
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出评价报告")]
        public ActionResult ExportWord(string queryJson)
        {
            if (queryJson == null && queryJson == "")
            {
                return Error("导出失败!");
            }
            var queryParam = queryJson.ToJObject();
            //计划ID
            if (queryParam["EvaluatePlanId"].IsEmpty())
            {
                return Error("导出失败!");
            }
            ////工作标题
            //if (!queryParam["WorkTitle"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and WorkTitle like'{0}%'", queryParam["WorkTitle"].ToString());
            //}
            var data = evaluateplanbll.GetEntity(queryParam["EvaluatePlanId"].ToString());

            JObject queryJson1 = new JObject();
            //queryJson1.Add(new JProperty("IsConform", "0"));
            queryJson1.Add(new JProperty("EvaluatePlanId", queryParam["EvaluatePlanId"].ToString()));
            var ListData1 = evaluatedetailsbll.GetList(queryJson1.ToString());//符合 的数据

            //JObject queryJson2 = new JObject();
            //queryJson2.Add(new JProperty("IsConform", "1"));
            //queryJson2.Add(new JProperty("EvaluatePlanId", queryParam["EvaluatePlanId"].ToString()));
            //var ListData2 = evaluatedetailsbll.GetList(queryJson2.ToString());//不符合 的数据
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
            DataTable ListData3 = standardsystembll.GetPageList(pagination3, queryJson3.ToString());//本年度实施的法律法规

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
            DataTable ListData4 = standardsystembll.GetPageList(pagination4, queryJson4.ToString());//废止的法律法规

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
            string fileName = "合规性评价报告_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/合规性评价报告导出模板.doc");

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
                dtrow["IsConform"] = obj.IsConform == 0 ? "符合" : "不符合";
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
            return Success("导出成功!");
        }
        #endregion
    }
}
