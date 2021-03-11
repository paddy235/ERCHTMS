using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using System.Web;
using Aspose.Words;
using Aspose.Words.Saving;
using System.IO;
using ERCHTMS.Cache;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：使用消防水
    /// </summary>
    public class FireWaterController : MvcControllerBase
    {
        private FireWaterBLL firewaterbll = new FireWaterBLL();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        private ScaffoldauditrecordBLL scaffoldauditrecordbll = new ScaffoldauditrecordBLL();

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
        /// 流程页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }

        /// <summary>
        /// 台账页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Ledger()
        {
            return View();
        }

        /// <summary>
        /// 选择消防工具
        /// </summary>
        /// <returns></returns>
        public ActionResult ToolSelect()
        {
            return View();
        }
        #endregion


        #region 获取消防水使用流程图对象
        /// <summary>
        /// 获取消防水使用流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetFireWaterFlow(string keyValue)
        {
            try
            {
                FireWaterEntity fireWaterEntity = firewaterbll.GetEntity(keyValue);
                string moduleName = string.Empty;
                string projectid = "";
                if (fireWaterEntity.WorkDeptType == "0")//单位内部
                {
                    moduleName = "消防水使用-内部审核";
                }
                else
                {
                    moduleName = "消防水使用-外部审核";
                    projectid = fireWaterEntity.EngineeringId;
                }
                var josnData = firewaterbll.GetFlow(fireWaterEntity.Id, moduleName);
                return Content(josnData.ToJson());
            }
            catch (Exception ex)
            {
                return Error("操作出错，错误信息：" + ex.Message);
            }
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                #region 数据权限
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //查看范围数据权限
                /**
                 * 1.作业单位及子部门（下级）
                 * 2.本人创建的作业
                 * 3.发包部门管辖的外包单位
                 * 4.外包单位只能看本单位的
                 * */
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                #endregion
                var data = firewaterbll.GetList(pagination, queryJson, authType, user);
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
                return Error(ex.Message);
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
            try
            {
                var data = firewaterbll.GetEntity(keyValue);
                var conditionData = firewaterbll.GetConditionEntity(keyValue);
                var jsonData = new
                {
                    data = data,
                    conditionData = conditionData
                };
                return ToJsonResult(jsonData);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
           
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
        [HandlerMonitor(6, "消防水使用删除")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            firewaterbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="jsonData">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "消防水使用保存")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string jsonData)
        {
            try
            {
                FireWaterModel model = JsonConvert.DeserializeObject<FireWaterModel>(jsonData);
                firewaterbll.SaveForm(keyValue, model);
            }
            catch (Exception ex)
            {
                return Error("操作出错，错误信息：" + ex.Message);
            }

            return Success("操作成功。");
        }

        /// <summary>
        /// 用户审核流程提交
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(8, "消防水使用提交")]
        [AjaxOnly]
        public ActionResult AuditSubmit(string keyValue, string jsonData)
        {
            try
            {
                var requestParam = JsonConvert.DeserializeAnonymousType(jsonData, new
                {
                    auditEntity = new ScaffoldauditrecordEntity()
                });
                if (requestParam.auditEntity == null)
                {
                    return Error("审核出错，错误信息：参数为null");
                }
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                FireWaterEntity fireWaterEntity = firewaterbll.GetEntity(keyValue);
                firewaterbll.ApplyCheck(keyValue, requestParam.auditEntity);

            }
            catch (Exception ex)
            {
                return Error("审核出错，错误信息：" + ex.Message);
            }
            return Success("提交成功。");
        }
        /// <summary>
        /// 提交执行情况
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(8, "消防水提交执行情况")]
        [AjaxOnly]
        public ActionResult SubmitCondition(string keyValue, string jsonData) {
            try
            {
                var requestParam = JsonConvert.DeserializeAnonymousType(jsonData, new
                {
                    ConditionEntity = new FireWaterCondition()
                });
                if (requestParam.ConditionEntity == null)
                {
                    return Error("审核出错，错误信息：参数为null");
                }
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                firewaterbll.SubmitCondition(keyValue, requestParam.ConditionEntity);
                FireWaterEntity fireWaterEntity = firewaterbll.GetEntity(requestParam.ConditionEntity.FireWaterId);
                fireWaterEntity.ConditionState = "1";
                firewaterbll.SaveForm(fireWaterEntity.Id, fireWaterEntity);

            }
            catch (Exception ex)
            {
                return Error("审核出错，错误信息：" + ex.Message);
            }
            return Success("提交成功。");
        }
        #endregion

        #region 列表导出
        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                string isHrdl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("IsOpenPassword");
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                #region 数据权限
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //查看范围数据权限
                /**
                 * 1.作业单位及子部门（下级）
                 * 2.本人创建的作业
                 * 3.发包部门管辖的外包单位
                 * 4.外包单位只能看本单位的
                 * */
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                #endregion
                var data = firewaterbll.GetList(pagination, queryJson, authType, user);
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("applystatestr"));
                excelTable.Columns.Add(new DataColumn("applynumber"));
                excelTable.Columns.Add(new DataColumn("workdepttypestr"));
                excelTable.Columns.Add(new DataColumn("workplace"));
                excelTable.Columns.Add(new DataColumn("worktime"));
                excelTable.Columns.Add(new DataColumn("workdeptname"));
                excelTable.Columns.Add(new DataColumn("applyusername"));
                excelTable.Columns.Add(new DataColumn("applytime"));
                foreach (DataRow item in data.Rows)
                {
                    int state = 0;
                    int.TryParse(item["applystate"].ToString(), out state);
                    int workdepttype = 0;
                    int.TryParse(item["workdepttype"].ToString(), out workdepttype);
                    DataRow newDr = excelTable.NewRow();
                    //搭设申请、拆除申请作业状态
                    if (state == 0)
                    {
                        newDr["applystatestr"] = "申请中";
                    }
                    if (state == 1)
                    {
                        newDr["applystatestr"] = "审核(批)中";
                    }
                    if (state == 2)
                    {
                        newDr["applystatestr"] = "审核(批)未通过";
                    }
                    if (state == 3)
                    {
                        newDr["applystatestr"] = "审核(批)通过";
                    }
                    if (workdepttype == 0)
                    {
                        newDr["workdepttypestr"] = "单位内部";
                    }
                    else
                    {
                        newDr["workdepttypestr"] = "外包单位";
                    }
                    newDr["applynumber"] = item["applynumber"];
                    newDr["workplace"] = item["workplace"];
                    newDr["workdeptname"] = item["workdeptname"];
                    DateTime applytime, workstartdate, workenddate;

                    DateTime.TryParse(item["createdate"].ToString(), out applytime);
                    DateTime.TryParse(item["workstarttime"].ToString(), out workstartdate);
                    DateTime.TryParse(item["workendtime"].ToString(), out workenddate);
                    newDr["worktime"] = workstartdate.ToString("yyyy-MM-dd HH:mm") + "-" + workenddate.ToString("yyyy-MM-dd HH:mm");
                    newDr["applyusername"] = item["applyusername"];
                    newDr["applytime"] = applytime.ToString("yyyy-MM-dd HH:mm");

                    excelTable.Rows.Add(newDr);
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "消防水使用许可申请信息";
                excelconfig.FileName = "消防水使用许可申请信息导出.xls";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applystatestr", ExcelColumn = "许可状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applynumber", ExcelColumn = "申请编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdepttypestr", ExcelColumn = "使用消防水单位类别", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workplace", ExcelColumn = "使用消防水地点", Width = 60 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktime", ExcelColumn = isHrdl=="true"? "使用消防水时间" : "计划使用消防水时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdeptname", ExcelColumn = "使用消防水单位", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyusername", ExcelColumn = "申请人", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applytime", ExcelColumn = "申请时间", Width = 20 });
                //调用导出方法
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion

        #region 详情导出
        /// <summary>
        /// 导出详情
        /// </summary>
        [HandlerMonitor(0, "消防水许可申请导出")]
        public void ExportDetails(string keyValue)
        {
            try
            {
                var IsHrdl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("IsOpenPassword");
                if (IsHrdl=="true")
                {
                    ExportHrdlTemp(keyValue);
                }
                else {
                    ExportCommonTemp(keyValue);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void ExportHrdlTemp(string keyValue) {
            FireWaterEntity firewaterentity = firewaterbll.GetEntity(keyValue);
            if (firewaterentity == null)
            {
                return;
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("ApplyTime");
            dt.Columns.Add("ApplyDept");
            dt.Columns.Add("ApplyPerson");
            dt.Columns.Add("WorkContent");
            dt.Columns.Add("PlanTime");
            dt.Columns.Add("ConditionIdea");
            dt.Columns.Add("WorkUse");

            dt.Columns.Add("OfficePerson");
            dt.Columns.Add("OfficeIdea");

            dt.Columns.Add("SafetyPerson");
            dt.Columns.Add("SafetyIdea");
            //dt.Columns.Add("Safety");

            dt.Columns.Add("LeaderPerson");
            dt.Columns.Add("LeaderIdea");
            //dt.Columns.Add("Leader");


            string fileName = Server.MapPath("~/Resource/ExcelTemplate/动用消防设施申请单.docx");
            Document doc = new Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
            string defaulttimestr = "yyyy年MM月dd日HH时mm分";

            DataRow row = dt.NewRow();
            List<ScaffoldauditrecordEntity> list = scaffoldauditrecordbll.GetList(firewaterentity.Id).OrderBy(x => x.AuditDate).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                var filepath = list[i].AuditSignImg == null ? Server.MapPath("~/content/Images/no_1.png") : (Server.MapPath("~/") + list[i].AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                
                if (i == 0) {
                    row["OfficePerson"]=list[i].AuditUserName;
                    row["OfficeIdea"] = list[i].AuditRemark;
                    builder.MoveToMergeField("Office");
                } else if (i == list.Count - 1) {
                    row["LeaderPerson"] = list[i].AuditUserName;
                    row["LeaderIdea"] = list[i].AuditRemark;
                    builder.MoveToMergeField("Leader");
                } else {
                    row["SafetyPerson"] = list[i].AuditUserName;
                    row["SafetyIdea"] = list[i].AuditRemark;
                    builder.MoveToMergeField("Safety");
                }
                if (System.IO.File.Exists(filepath))
                {
                    builder.InsertImage(filepath, 80, 35);
                }
                else {
                    filepath = Server.MapPath("~/content/Images/no_1.png");
                    builder.InsertImage(filepath, 80, 35);
                }
            
            }
            row["WorkUse"] = firewaterentity.WorkUse;
            row["ApplyPerson"] = firewaterentity.ApplyUserName;
            row["WorkContent"] = firewaterentity.WorkContent;
            row["ApplyTime"] = firewaterentity.CreateDate;
            row["ApplyDept"] = firewaterentity.ApplyDeptName;
            row["PlanTime"] = Convert.ToDateTime(firewaterentity.WorkStartTime).ToString(defaulttimestr) + "至" + Convert.ToDateTime(firewaterentity.WorkEndTime).ToString(defaulttimestr);
            row["ConditionIdea"] = firewaterbll.GetConditionEntity(firewaterentity.Id) != null ? firewaterbll.GetConditionEntity(firewaterentity.Id).ConditionContent : "";
            
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            var docStream = new MemoryStream();
            doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
            Response.ContentType = "application/msword";
            Response.AddHeader("content-disposition", "attachment;filename=消防水使用申请单_" + firewaterentity.ApplyNumber + ".doc");
            Response.BinaryWrite(docStream.ToArray());
            Response.End();
        }
        /// <summary>
        /// 通用消防水导出
        /// </summary>
        /// <param name="keyValue"></param>
        private void ExportCommonTemp(string keyValue)
        {
            FireWaterEntity firewaterentity = firewaterbll.GetEntity(keyValue);
            if (firewaterentity == null)
            {
                return;
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("applynumber");
            dt.Columns.Add("workdeptname");
            dt.Columns.Add("workplace");
            dt.Columns.Add("starttime");
            dt.Columns.Add("endtime");
            dt.Columns.Add("workcontent");
            dt.Columns.Add("measure");

            dt.Columns.Add("aaudituser");
            dt.Columns.Add("baudituser");
            dt.Columns.Add("caudituser");
            dt.Columns.Add("daudituser");
            dt.Columns.Add("eaudituser");
            dt.Columns.Add("eauditdept");
            dt.Columns.Add("faudituser");
            dt.Columns.Add("fauditdept");
            dt.Columns.Add("gaudituser");

            string fileName = Server.MapPath("~/Resource/ExcelTemplate/消防水使用申请模板.doc");
            Document doc = new Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
            string defaulttimestr = "yyyy年MM月dd日HH时mm分";
            string eauditdept = "", fauditdept = "";
            DataRow row = dt.NewRow();
            List<ScaffoldauditrecordEntity> list = scaffoldauditrecordbll.GetList(firewaterentity.Id).OrderBy(x => x.AuditDate).ToList();
            var count = 1;
            for (int i = 1; i <= list.Count; i++)
            {
                var filepath = list[i - 1].AuditSignImg == null ? "" : (Server.MapPath("~/") + list[i - 1].AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                if (firewaterentity.WorkDeptType == "0" && count == 1)
                {
                    count = i + 1;
                }
                if (count == 1)
                {
                    builder.MoveToMergeField("aaudituser");
                }
                else if (count == 2)
                {
                    builder.MoveToMergeField("baudituser");
                    if (System.IO.File.Exists(filepath))
                        builder.InsertImage(filepath, 80, 35);
                    else
                        builder.InsertImage(pic, 80, 35);
                    builder.MoveToMergeField("bauditdept");
                }
                else if (count == 3)
                {
                    builder.MoveToMergeField("caudituser");
                }
                else if (count == 4)
                {
                    builder.MoveToMergeField("daudituser");
                }
                else if (count == 5)
                {
                    builder.MoveToMergeField("eaudituser");
                }
                else if (count == 6)
                {
                    builder.MoveToMergeField("faudituser");
                    fauditdept = list[i].AuditDeptName;
                }
                else if (count == 7)
                {
                    builder.MoveToMergeField("gaudituser");
                }
                if (System.IO.File.Exists(filepath))
                    builder.InsertImage(filepath, 80, 35);
                else
                    builder.InsertImage(pic, 80, 35);
                count++;
            }
            row["applynumber"] = firewaterentity.ApplyNumber;
            row["workcontent"] = firewaterentity.WorkContent;
            row["workplace"] = firewaterentity.WorkPlace;
            row["workdeptname"] = firewaterentity.WorkDeptName;
            row["starttime"] = Convert.ToDateTime(firewaterentity.WorkStartTime).ToString(defaulttimestr);
            row["endtime"] = Convert.ToDateTime(firewaterentity.WorkEndTime).ToString(defaulttimestr);
            row["measure"] = firewaterentity.Measure;//措施
            row["eauditdept"] = eauditdept;
            row["fauditdept"] = fauditdept;
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            var docStream = new MemoryStream();
            doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
            Response.ContentType = "application/msword";
            Response.AddHeader("content-disposition", "attachment;filename=消防水使用申请单_" + firewaterentity.ApplyNumber + ".doc");
            Response.BinaryWrite(docStream.ToArray());
            Response.End();
        }
        #endregion

        #region 台账
        #region 台账列表
        /// <summary>
        /// 获取台账列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetLedgerListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = firewaterbll.GetLedgerList(pagination, queryJson, user);
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
                return Error(ex.Message);
            }
        }
        #endregion


        #region 导出台账
        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportLedgerData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

                DataTable data = firewaterbll.GetLedgerList(pagination, queryJson, user);
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("applynumber"));
                excelTable.Columns.Add(new DataColumn("ledgertype"));
                excelTable.Columns.Add(new DataColumn("workdeptname"));
                excelTable.Columns.Add(new DataColumn("workdepttypename"));
                excelTable.Columns.Add(new DataColumn("worktime"));
                excelTable.Columns.Add(new DataColumn("realityworktime"));
                excelTable.Columns.Add(new DataColumn("workplace"));

                foreach (DataRow item in data.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    newDr["applynumber"] = item["applynumber"];
                    newDr["ledgertype"] = item["ledgertype"];
                    newDr["workdeptname"] = item["workdeptname"];
                    newDr["workdepttypename"] = item["workdepttypename"];

                    DateTime workstarttime, workendtime, realityworkstarttime, realityworkendtime;
                    DateTime.TryParse(item["workstarttime"].ToString(), out workstarttime);
                    DateTime.TryParse(item["workendtime"].ToString(), out workendtime);
                    DateTime.TryParse(item["realityworkstarttime"].ToString(), out realityworkstarttime);
                    DateTime.TryParse(item["realityworkendtime"].ToString(), out realityworkendtime);

                    string worktime = string.Empty;
                    if (workstarttime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        worktime += workstarttime.ToString("yyyy-MM-dd HH:mm") + " - ";
                    }
                    if (workendtime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        worktime += workendtime.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["worktime"] = worktime;
                    string realityworktime = string.Empty;
                    if (realityworkstarttime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        realityworktime += realityworkstarttime.ToString("yyyy-MM-dd HH:mm") + " - ";
                    }
                    if (realityworkendtime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        realityworktime += realityworkendtime.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["realityworktime"] = realityworktime;
                    newDr["workplace"] = item["workplace"];

                    excelTable.Rows.Add(newDr);
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "消防水使用台账";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "消防水使用台账.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applynumber", ExcelColumn = "申请编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ledgertype", ExcelColumn = "作业状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdeptname", ExcelColumn = "使用消防水单位", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdepttypename", ExcelColumn = "使用消防水单位类别", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktime", ExcelColumn = "使用消防水申请时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realityworktime", ExcelColumn = "使用消防水实际时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workplace", ExcelColumn = "使用消防水地点", Width = 60 });
                //调用导出方法
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion
        #endregion
    }
}
