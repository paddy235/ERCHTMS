using Aspose.Words;
using Aspose.Words.Saving;
using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.DangerousJob;
using ERCHTMS.Busines.DangerousJobConfig;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Entity.PublicInfoManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.DangerousJob.Controllers
{
    /// <summary>
    /// 描 述：作业安全证申请
    /// </summary>
    public class JobApprovalFormController : MvcControllerBase
    {
        private JobApprovalFormBLL JobApprovalFormbll = new JobApprovalFormBLL();
        private JobSafetyCardApplyBLL jobSafetyCardApplybll = new JobSafetyCardApplyBLL();
        private ManyPowerCheckBLL manyPowerCheckbll = new ManyPowerCheckBLL();
        private DangerousJobFlowDetailBLL dangerousjobflowdetailbll = new DangerousJobFlowDetailBLL();
        private FireWaterBLL firewaterbll = new FireWaterBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private ClassStandardConfigBLL classstandardconfigbll = new ClassStandardConfigBLL();
        private DataItemCache dataItemCache = new DataItemCache();
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
        /// 审批台账列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApproverIndex()
        {
            return View();
        }
        /// <summary>
        /// 高处作业表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApprovalDetail()
        {
            return View();
        }
        /// <summary>
        /// 流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }
        /// <summary>
        /// 获取作业证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 获取作业证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CordForm()
        {
            return View();
        }
        
        /// <summary>
        /// 作业台账页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LedgerSetting()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckDetail()
        {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取作业安全证关联审批单数据
        /// </summary>
        /// <param name="RecId">作业安全证Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCardRecData(string RecId)
        {
            try
            {
                var data = JobApprovalFormbll.GetList("").Where(t => t.JobSafetyCardId != null && t.JobSafetyCardId.Contains(RecId)).FirstOrDefault();
                if (data == null)
                {
                    return Success("获取成功", new { count = -1 });
                }
                else
                {
                    return Success("获取成功", new { count = 1, data = data.Id });
                }
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = JobApprovalFormbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        public ActionResult SafetyCardIndex(string queryJson)
        {
            var data = JobApprovalFormbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                var data = JobApprovalFormbll.GetPageList(pagination, queryJson);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 获取台账列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageViewJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                var data = JobApprovalFormbll.GetPageView(pagination, queryJson);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetCardPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                string key = Request["key"] ?? "";
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                var data = JobApprovalFormbll.GetCardPageList(pagination, key);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = JobApprovalFormbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetEntity(string keyValue)
        {
            var data = JobApprovalFormbll.GetEntity(keyValue);
            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.JobSafetyCardId))
                {
                    var JobSafetyCardId = data.JobSafetyCardId.Split(',');
                    var d = JobApprovalFormbll.GetSafetyCardTable(JobSafetyCardId);
                    data.JobSafetyCard = "";
                    data.JobSafetyCardId = "";
                    var status = "";
                    for (int i = 0; i < d.Rows.Count; i++)
                    {
                        switch (d.Rows[i]["jobstate"].ToString())
                        {
                            case "0":
                                status = "申请中";
                                break;
                            case "1":
                                status = "审批中";
                                break;
                            case "2":
                                status = "审核不通过";
                                break;
                            case "3":
                                status = "措施确认中";
                                break;
                            case "4":
                                status = "停电中";
                                break;
                            case "5":
                                status = "备案中";
                                break;
                            case "6":
                                status = "验收中";
                                break;
                            case "7":
                                status = "送电中";
                                break;
                            case "8":
                                status = "即将作业";
                                break;
                            case "9":
                                status = "作业暂停";
                                break;
                            case "10":
                                status = "作业中";
                                break;
                            case "11":
                                status = "流程结束";
                                break;
                            default:
                                break;
                        }
                        data.JobSafetyCard += d.Rows[i]["jobtypename"] + "(" + status + "),";
                        data.JobSafetyCardId += d.Rows[i]["Id"].ToString() + ",";
                    }
                    data.JobSafetyCardId = data.JobSafetyCardId.TrimEnd(',');
                    data.JobSafetyCard = data.JobSafetyCard.TrimEnd(',');
                }
            }
            return ToJsonResult(data);
        }
        /// <summary>
        /// 新增时获取逐级审核流程配置 
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult ConfigurationList(string modulename)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var data = manyPowerCheckbll.GetList(user.OrganizeCode, modulename);

                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Content("false");
            }

        }

        /// <summary>
        /// 修改时获取逐级审核流程配置（结合已保存到危险作业表里的数据）
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult ConfigurationByWorkList(string keyValue, string moduleno)
        {
            try
            {
                var data = JobApprovalFormbll.ConfigurationByWorkList(keyValue, moduleno);

                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Content("false");
            }

        }
        /// <summary>
        /// 导出台账列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>

        [HandlerMonitor(0, "导出数据")]
        public ActionResult JobApprovalForm(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.sidx = "t.createdate";//排序字段
                pagination.conditionJson = " 1=1 ";
                pagination.sord = "desc";//排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var queryParam = queryJson.ToJObject();
                DataTable data = new DataTable();

                if (queryParam["isapprover"].ToString() == "是")
                    data = JobApprovalFormbll.GetPageView(pagination, queryJson);
                else
                    data = JobApprovalFormbll.GetPageList(pagination, queryJson);
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("jobstate"));
                excelTable.Columns.Add(new DataColumn("applyno"));
                excelTable.Columns.Add(new DataColumn("jobtypename"));
                excelTable.Columns.Add(new DataColumn("joblevel"));
                excelTable.Columns.Add(new DataColumn("jobplace"));
                excelTable.Columns.Add(new DataColumn("jobtime"));
                excelTable.Columns.Add(new DataColumn("realityjobtime"));
                excelTable.Columns.Add(new DataColumn("jobdeptname"));
                excelTable.Columns.Add(new DataColumn("applyusername"));
                excelTable.Columns.Add(new DataColumn("applytime"));
                excelTable.Columns.Add(new DataColumn("jobsafetycard"));
                foreach (DataRow item in data.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    //newDr["jobstate"] = item["jobstate"];
                    newDr["applyno"] = item["applyno"];
                    newDr["jobtypename"] = item["jobtypename"];
                    newDr["jobplace"] = item["jobplace"];
                    newDr["jobdeptname"] = item["jobdeptname"];
                    newDr["applyusername"] = item["applyusername"];

                    string jobstate = item["jobstate"].ToString();
                    string status = "";
                    if (item["ledgertype"].ToString() == "即将作业")
                        jobstate = "2";
                    if (item["ledgertype"].ToString() == "作业中")
                    {
                        jobstate = "6";
                    }
                    if (item["ledgertype"].ToString() == "作业暂停")
                    {
                        jobstate = "5";
                    }

                    switch (jobstate)
                    {
                        case "0":
                            status = "申请中";
                            break;
                        case "1":
                            status = "审批中";
                            break;
                        case "2":
                            status = "即将作业";
                            break;
                        case "4":
                            status = "审批不通过";
                            break;
                        case "5":
                            status = "作业暂停";
                            break;
                        case "6":
                            status = "作业中";
                            break;
                        case "7":
                            status = "已结束";
                            break;
                        default:
                            break;
                    }
                    newDr["jobstate"] = status;
                    string levelname = string.Empty;
                    if (item["joblevel"].ToString() == "0")
                        levelname = "一级风险作业";
                    if (item["joblevel"].ToString() == "1")
                        levelname = "二级风险作业";
                    if (item["joblevel"].ToString() == "2")
                        levelname = "三级风险作业";
                    newDr["joblevel"] = levelname;
                    DateTime realityjobstarttime, realityjobendtime, jobstarttime, jobendtime, applytime;
                    DateTime.TryParse(item["realityjobstarttime"].ToString(), out realityjobstarttime);
                    DateTime.TryParse(item["realityjobendtime"].ToString(), out realityjobendtime);
                    DateTime.TryParse(item["jobstarttime"].ToString(), out jobstarttime);
                    DateTime.TryParse(item["jobendtime"].ToString(), out jobendtime);
                    DateTime.TryParse(item["applytime"].ToString(), out applytime);
                    string applyttime = string.Empty;
                    if (jobstarttime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        applyttime = applytime.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["applytime"] = applyttime;
                    string jobtime = string.Empty;
                    if (jobstarttime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        jobtime += jobstarttime.ToString("yyyy-MM-dd HH:mm") + " ~ ";
                    }
                    if (jobendtime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        jobtime += jobendtime.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["jobtime"] = jobtime;
                    string realityjobtime = string.Empty;
                    if (realityjobstarttime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        realityjobtime += realityjobstarttime.ToString("yyyy-MM-dd HH:mm") + " ~ ";
                    }
                    if (realityjobendtime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        realityjobtime += realityjobendtime.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["realityjobtime"] = realityjobtime;
                    if (string.IsNullOrEmpty(item["jobsafetycardid"].ToString()))
                        newDr["jobsafetycard"] = "否";
                    else newDr["jobsafetycard"] = "是";
                    excelTable.Rows.Add(newDr);
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "危险作业审批单台账";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "危险作业审批台账.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobstate", ExcelColumn = "流程状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyno", ExcelColumn = "编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobtypename", ExcelColumn = "危险作业类型", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "joblevel", ExcelColumn = "危险作业级别", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobplace", ExcelColumn = "作业地点", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobtime", ExcelColumn = "计划作业时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realityjobtime", ExcelColumn = "实际作业时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobdeptname", ExcelColumn = "作业单位", Width = 60 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyusername", ExcelColumn = "申请人", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applytime", ExcelColumn = "申请时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobsafetycard", ExcelColumn = "是否关联作业安全证", Width = 40 });
                //调用导出方法
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="KeyValue"></param>
        [HandlerMonitor(0, "危险作业审批单导出")]
        public void ExportDetails(string KeyValue)
        {
            string fileName = Str.Empty;
            JobApprovalFormEntity jobapprovalformentity = JobApprovalFormbll.GetEntity(KeyValue);
            if (jobapprovalformentity == null)
            {
                return;
            }
            else
            {
                if (jobapprovalformentity.JobLevel == "0")
                    fileName = Server.MapPath("~/Resource/ExcelTemplate/一级危险作业审批单.docx");
                if (jobapprovalformentity.JobLevel == "1")
                    fileName = Server.MapPath("~/Resource/ExcelTemplate/二级危险作业审批单.docx");
                if (jobapprovalformentity.JobLevel == "2")
                    fileName = Server.MapPath("~/Resource/ExcelTemplate/三级危险作业审批单.docx");

                Document doc = new Document(fileName);
                DocumentBuilder builder = new DocumentBuilder(doc);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable("");
                dt.Columns.Add("JobDeptName");//作业单位
                dt.Columns.Add("JobContent");//作业内容
                dt.Columns.Add("JobStartTime");//作业开始时间
                dt.Columns.Add("JobEndTime");//作业结束时间
                dt.Columns.Add("JobPlace");//作业地点
                dt.Columns.Add("JobNum");//作业人数
                dt.Columns.Add("JobType");//危险作业类型
                dt.Columns.Add("DangerousDecipher");//主要危险、危害因素
                dt.Columns.Add("SafetyMeasures");//安全措施（可另附）
                dt.Columns.Add("AuditRemark1");//中心（部门）安全管理人员：
                dt.Columns.Add("AuditRemark2");//现场安全负责人
                dt.Columns.Add("AuditRemark3");//作业单位主管意见
                dt.Columns.Add("AuditRemark4");//工序确认意见
                dt.Columns.Add("AuditRemark5");//中心（部门）安全管理人员意见
                dt.Columns.Add("AuditRemark6");//中心（部门）负责人意见
                dt.Columns.Add("AuditRemark7");//作业单位负责人意见
                dt.Columns.Add("AuditRemark8");//公司安全环保健康部意见
                dt.Columns.Add("AuditRemark9");//公司分管负责人意见

                dt.Columns.Add("AuditUserName1");//中心（部门）安全管理人员：
                dt.Columns.Add("AuditUserName2");//现场安全负责人
                dt.Columns.Add("AuditUserName3");//作业单位主管意见
                dt.Columns.Add("AuditUserName4");//工序确认意见
                dt.Columns.Add("AuditUserName5");//中心（部门）安全管理人员意见
                dt.Columns.Add("AuditUserName6");//中心（部门）负责人意见
                dt.Columns.Add("AuditUserName7");//作业单位负责人意见
                dt.Columns.Add("AuditUserName8");//公司安全环保健康部意见
                dt.Columns.Add("AuditUserName9");//公司分管负责人意见

                dt.Columns.Add("Tel1");//中心（部门）安全管理人员：
                dt.Columns.Add("Tel2");//现场安全负责人

                dt.Columns.Add("AuditDate3");//作业单位主管意见
                dt.Columns.Add("AuditDate4");//工序确认意见
                dt.Columns.Add("AuditDate5");//中心（部门）安全管理人员意见
                dt.Columns.Add("AuditDate6");//中心（部门）负责人意见
                dt.Columns.Add("AuditDate7");//作业单位负责人意见
                dt.Columns.Add("AuditDate8");//公司安全环保健康部意见
                dt.Columns.Add("AuditDate9");//公司分管负责人意见
                dt.Columns.Add("SignUrl");//编制人
                dt.Columns.Add("SignUrl1");//审批人
                DataRow row1 = dt.NewRow();
                string auditdate3 = "年   月   日", //作业单位主管意见
                       auditdate4 = "年   月   日",//工序确认意见
                       auditdate5 = "年   月   日",//中心（部门）安全管理人员意见
                       auditdate6 = "年   月   日",//中心（部门）负责人意见
                       auditdate7 = "年   月   日",//作业单位负责人意见
                       auditdate8 = "年   月   日",//公司安全环保健康部意见
                       auditdate9 = "年   月   日";//公司分管负责人意见
                string defaultstr = "yyyy年MM月dd日";
                var table = JobApprovalFormbll.GetCheckInfo(jobapprovalformentity.Id);

                row1["JobDeptName"] = jobapprovalformentity.JobDeptName;
                row1["JobContent"] = jobapprovalformentity.JobContent;
                row1["JobStartTime"] = jobapprovalformentity.JobStartTime.HasValue ? jobapprovalformentity.JobStartTime.Value.ToString("yyyy-MM-dd HH:mm") : "";
                row1["JobEndTime"] = jobapprovalformentity.JobEndTime.HasValue ? jobapprovalformentity.JobEndTime.Value.ToString("yyyy-MM-dd HH:mm") : "";
                row1["JobPlace"] = jobapprovalformentity.JobPlace;
                row1["JobNum"] = jobapprovalformentity.JobNum;

                //row1["JobType"] = jobapprovalformentity.JobTypeName;
                var JobLevelList = dataItemCache.GetDataItemList("DangerousJobType");
                foreach (var item in JobLevelList)
                {
                    if (jobapprovalformentity.JobType.Contains(item.ItemValue))
                        row1["JobType"] += " ☑" + item.ItemName;
                    else row1["JobType"] += "  □" + item.ItemName;
                }
                //row1["JobType"]+=""
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = classstandardconfigbll.GetList("").Where(t => t.WorkType == "DangerousJobCheck" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
                if (data != null)
                {
                    var newdata = data.Whbs.Split('$');
                    foreach (var item in newdata)
                    {
                        if (jobapprovalformentity.DangerousDecipher.Contains(item))
                            row1["DangerousDecipher"] += " ☑" + item;
                        else row1["DangerousDecipher"] += "  □" + item;
                    }
                }
                row1["SafetyMeasures"] = jobapprovalformentity.SafetyMeasures;
                string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
                UserBLL userbll = new UserBLL();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (i + 1 < 3)
                    {
                        var users = userbll.GetEntity(table.Rows[i]["approvepersonid"].ToString());
                        row1["Tel" + (i + 1)] = users != null ? users.Mobile : "";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(table.Rows[i]["approvetime"].ToString()))
                        {
                            var stime = Convert.ToDateTime(table.Rows[i]["approvetime"]);
                            row1["AuditDate" + (i + 1)] = stime.ToString(defaultstr);
                        }
                    }
                    var filepath = table.Rows[i]["SignUrl"] == null ? "" : (Server.MapPath("~/") + table.Rows[i]["SignUrl"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();

                    row1["AuditRemark" + (i + 1)] = table.Rows[i]["approveopinion"];
                    row1["AuditUserName" + (i + 1)] = builder.MoveToMergeField("AuditUserName" + (i + 1)); ;//中心（部门）安全管理人员：
                                                                                                            //builder.InsertImage(row["Person1"].ToString(), 80, 35);
                    if (!System.IO.File.Exists(filepath))
                    {
                        filepath = pic;
                    }
                    builder.InsertImage(filepath, 80, 35);
                }
                var signurl = string.IsNullOrEmpty(jobapprovalformentity.SignUrl) ? "" : (Server.MapPath("~/") + jobapprovalformentity.SignUrl.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                row1["SignUrl"] = builder.MoveToMergeField("SignUrl");
                //row1["SignUrl1"] = builder.MoveToMergeField("SignUrl1");
                if (!System.IO.File.Exists(signurl))
                {
                    signurl = pic;
                }
                builder.InsertImage(signurl, 80, 35);
                row1["SignUrl1"] = builder.MoveToMergeField("SignUrl1");
                if (!System.IO.File.Exists(signurl))
                {
                    signurl = pic;
                }
                builder.InsertImage(signurl, 80, 35);
                dt.Rows.Add(row1);
                doc.MailMerge.Execute(dt);
                doc.MailMerge.ExecuteWithRegions(dt);
                var docStream = new MemoryStream();
                doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
                Response.ContentType = "application/msword";
                if (jobapprovalformentity.JobLevel == "0")
                    Response.AddHeader("content-disposition", "attachment;filename=一级危险作业审批单.doc");
                if (jobapprovalformentity.JobLevel == "1")
                    Response.AddHeader("content-disposition", "attachment;filename=二级危险作业审批单.doc");
                if (jobapprovalformentity.JobLevel == "2")
                    Response.AddHeader("content-disposition", "attachment;filename=三级危险作业审批单.doc");
                Response.BinaryWrite(docStream.ToArray());
                Response.End();



            }
        }
        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSafetyCardJson(string UserIDs)
        {

            DataTable dt = JobApprovalFormbll.GetSafetyCardTable(UserIDs.Split(','));
            //DataTable dt = new DataTable();
            return Content(dt.ToJson());
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
            JobApprovalFormbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AjaxOnly]
        public ActionResult BackForm(string keyValue)
        {

            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var entity = JobApprovalFormbll.GetEntity(keyValue);
                if (entity != null)
                {
                    //int ApplyNumber = entity.ApplyNumber + 1;
                    //entity.ApplyNumber = ApplyNumber;//更新申请次数
                    entity.JobState = 0;//更改状态(撤销)
                    entity.IsSubmit = 0;
                    entity.ApplyNumber = entity.ApplyNumber + 1;
                    JobApprovalFormbll.SaveForm(keyValue, entity);
                    var DetailService = dangerousjobflowdetailbll.GetList().Where(x => x.BusinessId == keyValue).ToList();
                    if (DetailService != null && DetailService.Count > 0)
                    {
                        foreach (var item in DetailService)
                        {
                            dangerousjobflowdetailbll.RemoveForm(item.Id);
                        }
                    }
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error("操作失败。" + ex.Message);
            }

        }


        /// <summary>
        /// 保存作废表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveCancelReason(string keyValue, JobApprovalFormEntity entity)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var entity1 = JobApprovalFormbll.GetEntity(keyValue);
                if (entity1 != null)
                {
                    entity1.CancelReason = entity.CancelReason;
                    entity1.CancelTime = DateTime.Now;
                    entity1.CancelUserId = user.UserId;
                    entity1.CancelUserName = user.UserName;
                    entity1.JobState = 3;//更改状态(作废)
                    JobApprovalFormbll.SaveForm(keyValue, entity1);
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error("操作失败。");
            }

        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="arr">页面手动选择的流程审批人json</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, JobApprovalFormEntity entity, [System.Web.Http.FromBody]string arr)
        {
            try
            {
                var str = 0;
                var msg = "";
                if (entity.IsSubmit == 1)
                {
                    var list = JobApprovalFormbll.GetList("").Where(x => x.IsSubmit == 1).Where(x => x.Id != keyValue).ToList();
                    var JobSafetyCardId = string.Join(",", list.Select(x => x.JobSafetyCardId).ToArray()).Replace(",", "','");
                    if (!string.IsNullOrEmpty(entity.JobSafetyCardId))
                    {
                        var cardId = entity.JobSafetyCardId.TrimEnd(',').Split(',');
                        foreach (var item in cardId)
                        {
                            if (JobSafetyCardId.Contains(item))
                            {
                                str = 1;
                                var Applyentity = jobSafetyCardApplybll.GetEntity(item);
                                msg += Applyentity.JobTypeName + "(" + Applyentity.ApplyNo + "),";
                            }
                        }
                        if (str == 1)
                        {
                            return Error(msg.TrimEnd(',') + "作业安全证已被关联，请重新选择。");
                        }
                        else
                        {
                            //获取业务数据关联的逐级审核流程步骤信息
                            Operator user = OperatorProvider.Provider.Current();
                            var data = manyPowerCheckbll.GetListByModuleNo(user.OrganizeCode, entity.ModuleName);

                            JobApprovalFormbll.SaveForm(keyValue, entity, data, arr);
                            return Success("操作成功。");
                        }
                    }
                    else {
                        //获取业务数据关联的逐级审核流程步骤信息
                        Operator user = OperatorProvider.Provider.Current();
                        var data = manyPowerCheckbll.GetListByModuleNo(user.OrganizeCode, entity.ModuleName);

                        JobApprovalFormbll.SaveForm(keyValue, entity, data, arr);
                        return Success("操作成功。");
                    }
                }
                else
                {

                    //获取业务数据关联的逐级审核流程步骤信息
                    Operator user = OperatorProvider.Provider.Current();
                    var data = manyPowerCheckbll.GetListByModuleNo(user.OrganizeCode, entity.ModuleName);

                    JobApprovalFormbll.SaveForm(keyValue, entity, data, arr);
                    return Success("操作成功。");
                }
            }
            catch (Exception ex)
            {
                return Error("操作失败。" + ex.Message);
            }

        }
        #endregion
        /// <summary>
        /// 选择用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// 作业安全证申请列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSafetyCardListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                var data = JobApprovalFormbll.GetJobSafetyCardApplyPageList(pagination, queryJson);
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFlow(string keyValue)
        {
            try
            {
                var data = JobApprovalFormbll.GetFlow(keyValue);
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// 操作变更
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="TransferUserName"></param>
        /// <param name="TransferUserAccount"></param>
        /// <param name="TransferUserId"></param>
        /// <returns></returns>
        public ActionResult ExchangeForm(string keyValue, string TransferUserName, string TransferUserAccount, string TransferUserId)
        {
            try
            {
                JobApprovalFormbll.ExchangeForm(keyValue, TransferUserName, TransferUserAccount, TransferUserId);
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }


        /// <summary>
        /// 获取当前步骤操作人（变更操作显示当前操作人）
        /// </summary>
        /// <param name="BusinessId"></param>
        /// <returns></returns>
        public string GetCurrentStepOperator(string BusinessId)
        {
            try
            {
                string OperatorName = "";
                var entity = JobApprovalFormbll.GetEntity(BusinessId);
                switch (entity.JobState)
                {
                    case 1: //审核中状态
                        var flow = dangerousjobflowdetailbll.GetList().Where(x => x.BusinessId == BusinessId && x.Status == 0).ToList().FirstOrDefault();
                        if (flow != null)
                        {
                            if (flow.ProcessorFlag == "3")
                            {
                                OperatorName = flow.UserName;
                            }
                        }
                        break;
                    default:
                        break;
                }
                return OperatorName;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        /// <summary>
        /// 根据ID得到审核记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFormAuditToJson(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var entity = JobApprovalFormbll.GetEntity(keyValue);
                var data = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.Status == 1).OrderBy(x => x.ApproveTime).ToList();

                return ToJsonResult(data);
            }
            else
            {
                return ToJsonResult(null);
            }
        }
        /// <summary>
        /// 判断当前数据能否开始作业
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>

        public string IsLedgerSetting(string keyValue)
        {
            var msg = JobApprovalFormbll.IsLedgerSetting(keyValue);
            return msg;
        }

        /// <summary>
        /// 开始作业
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ledgerType"></param>
        /// <param name="type"></param>
        /// <param name="worktime"></param>
        /// <param name="issendmessage"></param>
        /// <param name="conditioncontent"></param>
        /// <param name="conditionid"></param>
        /// <param name="iscomplete"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult LedgerOp(string keyValue, string ledgerType, string type, string worktime, string issendmessage, string conditioncontent, string conditionid, string iscomplete)
        {
            try
            {
                JobApprovalFormbll.LedgerOp(keyValue, ledgerType, type, worktime, issendmessage, conditioncontent, conditionid, iscomplete);
            }
            catch (Exception ex)
            {
                return Error("操作失败，错误信息：" + ex.Message);
            }
            return Success("操作成功");
        }

        /// <summary>
        /// 开始作业
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ledgerType"></param>
        /// <param name="type"></param>
        /// <param name="worktime"></param>
        /// <param name="issendmessage"></param>
        /// <param name="conditioncontent"></param>
        /// <param name="conditionid"></param>
        /// <param name="iscomplete"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult JobFormLedgerOp(string keyValue, string ledgerType, string type, string worktime, string issendmessage, string conditioncontent, string conditionid, string iscomplete)
        {
            try
            {
                var str = 0;
                var msg = "";
                if (ledgerType == "1" && iscomplete == "1" && type == "5")
                {
                  
                    var entity = JobApprovalFormbll.GetEntity(keyValue);
                    if (!string.IsNullOrEmpty(entity.JobSafetyCardId))
                    {
                        var JobSafetyCardId = entity.JobSafetyCardId.Split(',');
                        var d = JobApprovalFormbll.GetSafetyCardTable(JobSafetyCardId);
                        for (int i = 0; i < d.Rows.Count; i++)
                        {
                            var rows = d.Rows[i];
                            if (rows["jobstate"].ToString() == "9")
                            {
                                str = 1;
                                msg += rows["jobtypename"] + ",";
                            }
                        }
                        if (str == 1)
                            return Error("用户" + msg.TrimEnd(',') + "作业安全证处于作业暂停中，需点击开始作业方可结束所有作业");
                        else
                            JobApprovalFormbll.LedgerOp(keyValue, ledgerType, type, worktime, issendmessage, conditioncontent, conditionid, iscomplete);
                    }
                    else
                    {
                        JobApprovalFormbll.LedgerOp(keyValue, ledgerType, type, worktime, issendmessage, conditioncontent, conditionid, iscomplete);

                    }
                }
                else
                {

                    JobApprovalFormbll.LedgerOp(keyValue, ledgerType, type, worktime, issendmessage, conditioncontent, conditionid, iscomplete);

                }

            }
            catch (Exception ex)
            {
                return Error("操作失败，错误信息：" + ex.Message);
            }
            return Success("操作成功");
        }
        /// <summary>
        /// 获取执行信息记录
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        public ActionResult GetConditonToJson(string keyvalue)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyvalue))
                {
                    IList<FireWaterCondition> data = firewaterbll.GetConditionList(keyvalue).OrderBy(t => t.CreateDate).ToList();
                    for (int i = 0; i < data.Count; i++)
                    {
                        List<FileInfoEntity> filelist = fileinfobll.GetFileList(data[i].Id); //现场图片
                        if (filelist.Count > 0)
                        {
                            data[i].ScenePicPath = filelist[0].FilePath;
                        }
                        List<FileInfoEntity> filelist2 = fileinfobll.GetFileList(data[i].Id + "_02"); //附件
                        data[i].filenum = filelist2.Count().ToString();
                        data[i].num = i / 2 + 1;
                    }
                    return ToJsonResult(data);
                }
                return ToJsonResult(null);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }


    }
}