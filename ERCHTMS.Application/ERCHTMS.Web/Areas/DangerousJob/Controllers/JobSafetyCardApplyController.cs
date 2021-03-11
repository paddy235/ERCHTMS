using Aspose.Words;
using Aspose.Words.Tables;
using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.DangerousJob;
using ERCHTMS.Busines.DangerousJobConfig;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.DangerousJob;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.DangerousJob.Controllers
{
    /// <summary>
    /// 描 述：作业安全证申请
    /// </summary>
    public class JobSafetyCardApplyController : MvcControllerBase
    {
        private JobSafetyCardApplyBLL jobSafetyCardApplybll = new JobSafetyCardApplyBLL();
        private ManyPowerCheckBLL manyPowerCheckbll = new ManyPowerCheckBLL();
        private DangerousJobFlowDetailBLL dangerousjobflowdetailbll = new DangerousJobFlowDetailBLL();
        private DangerousJobOperateBLL dangerousjoboperatebll = new DangerousJobOperateBLL();
        private JobApprovalFormBLL jobapprovalformbll = new JobApprovalFormBLL();
        private SafetyMeasureDetailBLL safetymeasuredetailbll = new SafetyMeasureDetailBLL();
        private SafetyMeasureConfigBLL safetymeasureconfigbll = new SafetyMeasureConfigBLL();
        private WhenHotAnalysisBLL whenhotanalysisbll = new WhenHotAnalysisBLL();
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
        /// 高处作业表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HeightWorkingDetail()
        {
            return View();
        }
        /// <summary>
        /// 起重吊装作业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LiftingDetail()
        {
            return View();
        }
        /// <summary>
        /// 动土作业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DiggingDetail()
        {
            return View();
        }
        /// <summary>
        /// 断路作业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OpenCircuitDetail()
        {
            return View();
        }
        /// <summary>
        /// 动火作业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WhenHotDetail()
        {
            return View();
        }

        /// <summary>
        /// 盲板抽堵作业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BlindPlateWallDetail()
        {
            return View();
        }

        /// <summary>
        /// 受限空间作业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LimitedSpaceDetail()
        {
            return View();
        }

        /// <summary>
        /// 设备检修清理作业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EquOverhaulCleanDetail()
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
        /// 作业安全证台账
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SafetyIndex()
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
            var data = jobSafetyCardApplybll.GetList(queryJson);
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

                var data = jobSafetyCardApplybll.GetPageList(pagination, queryJson);
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
                return Error(ex.ToString());
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
            var data = jobSafetyCardApplybll.GetEntity(keyValue);
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
            var data = jobSafetyCardApplybll.GetEntity(keyValue);
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
                var data = jobSafetyCardApplybll.ConfigurationByWorkList(keyValue, moduleno);

                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Content("false");
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
                var data = jobSafetyCardApplybll.GetFlow(keyValue);
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// 获取审核记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetApproveFlowRecord(string keyValue)
        {
            try
            {
                JobSafetyCardApplyEntity entity = jobSafetyCardApplybll.GetEntity(keyValue);
                var data = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.Status == 1).OrderByDescending(t => t.CreateDate).ToList();
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageViewJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                var data = jobSafetyCardApplybll.GetPageView(pagination, queryJson);
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
                return Error(ex.ToString());
            }

        }
        /// <summary>
        /// 导出台账列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>

        [HandlerMonitor(0, "导出数据")]
        public ActionResult JobSafetyCardExport(string queryJson)
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
                data = jobSafetyCardApplybll.GetPageView(pagination, queryJson);

                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("jobstate"));
                excelTable.Columns.Add(new DataColumn("applyno"));
                excelTable.Columns.Add(new DataColumn("jobtypename"));
                excelTable.Columns.Add(new DataColumn("jobdeptname"));
                excelTable.Columns.Add(new DataColumn("jobcontent"));

                excelTable.Columns.Add(new DataColumn("jobtime"));
                excelTable.Columns.Add(new DataColumn("realityjobtime"));
                excelTable.Columns.Add(new DataColumn("jobplace"));
                excelTable.Columns.Add(new DataColumn("applyusername"));
                excelTable.Columns.Add(new DataColumn("applytime"));
                foreach (DataRow item in data.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    //newDr["jobstate"] = item["jobstate"];
                    newDr["applyno"] = item["applyno"];
                    newDr["jobtypename"] = item["jobtypename"];
                    newDr["jobdeptname"] = item["jobdeptname"];
                    newDr["jobplace"] = item["jobplace"];
                    newDr["applyusername"] = item["applyusername"];
                    newDr["jobcontent"] = item["jobcontent"];
                    string status = "";
                    string jobstate = item["jobstate"].ToString();
                    switch (jobstate)
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
                            status = "开始作业";
                            break;
                        case "9":
                            status = "作业暂停";
                            break;
                        case "10":
                            status = "作业中";
                            break;
                        case "11":
                            status = "已结束";
                            break;
                        default:
                            break;
                    }
                    newDr["jobstate"] = status;

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

                    excelTable.Rows.Add(newDr);
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "作业安全证台账";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "作业安全证台账.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobstate", ExcelColumn = "流程状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyno", ExcelColumn = "编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobtypename", ExcelColumn = "作业类型", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobdeptname", ExcelColumn = "作业单位", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobcontent", ExcelColumn = "作业内容", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobtime", ExcelColumn = "计划作业时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realityjobtime", ExcelColumn = "实际作业时间", Width = 60 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobplace", ExcelColumn = "作业地点", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyusername", ExcelColumn = "申请人", Width = 20 });
                //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applytime", ExcelColumn = "申请时间", Width = 20 });
                //调用导出方法
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
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
            try
            {
                if (jobapprovalformbll.GetList("").Where(t => t.JobSafetyCardId != null && t.JobSafetyCardId.Contains(keyValue)).Count() > 0)
                {
                    return Error("已经关联危险作业审批单，不可以被删除");
                }
                jobSafetyCardApplybll.RemoveForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CancelForm(string keyValue)
        {
            try
            {
                var entity = jobSafetyCardApplybll.GetEntity(keyValue);
                var flowdetail = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.Status == 0).FirstOrDefault();
                if (flowdetail != null)
                {
                    dangerousjobflowdetailbll.RemoveForm(flowdetail.Id);
                }
                entity.JobState = 0;
                entity.IsSubmit = 0;
                entity.ApplyNumber = entity.ApplyNumber + 1;
                jobSafetyCardApplybll.SaveForm(keyValue, entity);
                return Success("撤销成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
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
        public ActionResult SaveCancelReason(string keyValue, JobSafetyCardApplyEntity entity)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var entity1 = jobSafetyCardApplybll.GetEntity(keyValue);
                if (entity1 != null)
                {
                    entity1.CancelReason = entity.CancelReason;
                    entity1.CancelTime = DateTime.Now;
                    entity1.CancelUserId = user.UserId;
                    entity1.CancelUserName = user.UserName;
                    entity1.JobState = 3;//更改状态(作废)
                    jobSafetyCardApplybll.SaveForm(keyValue, entity1);
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
        /// <param name="arrData">盲板抽堵规格</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, JobSafetyCardApplyEntity entity, [System.Web.Http.FromBody]string arr, [System.Web.Http.FromBody]string arrData)
        {
            try
            {
                //获取业务数据关联的逐级审核流程步骤信息
                Operator user = OperatorProvider.Provider.Current();
                var data = manyPowerCheckbll.GetListByModuleNo(user.OrganizeCode, entity.ModuleNo);

                jobSafetyCardApplybll.SaveForm(keyValue, entity, data, arr, arrData);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error("操作失败。" + ex.ToString());
            }

        }

        /// <summary>
        /// 查看时候保存标准
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveStandard(string keyValue, JobSafetyCardApplyEntity entity)
        {
            try
            {
                var oldentity = jobSafetyCardApplybll.GetEntity(keyValue);
                oldentity.OxygenContentStandard = entity.OxygenContentStandard;
                oldentity.DangerousStandard = entity.DangerousStandard;
                oldentity.GasStandard = entity.GasStandard;
                jobSafetyCardApplybll.SaveForm(keyValue, oldentity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error("操作失败。");
            }

        }

        /// <summary>
        /// 措施确认
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ConfirmForm(string keyValue, JobSafetyCardApplyEntity entity)
        {
            try
            {
                var data = jobSafetyCardApplybll.GetEntity(keyValue);
                UserBLL userbll = new UserBLL();
                if (data.JobType == "LimitedSpace")
                {
                    entity.JobState = 1;
                }
                else if (data.JobType == "EquOverhaulClean")
                {
                    entity.JobState = 4;
                }
                entity.ConfirmSignUrl = string.IsNullOrWhiteSpace(entity.ConfirmSignUrl) ? "" : entity.ConfirmSignUrl.Replace("../..", "");
                jobSafetyCardApplybll.SaveForm(keyValue, entity);
                if (data.JobType == "LimitedSpace")
                {
                    DangerousJobFlowDetailEntity flow = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == data.Id && t.Status == 0).FirstOrDefault();
                    string userids = dangerousjobflowdetailbll.GetCurrentStepUser(data.Id, flow.Id);
                    DataTable dt = userbll.GetUserTable(userids.Split(','));
                    JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "ZYAQZ003", data.JobTypeName + "安全证申请待您审批，请您及时处理。", data.CreateUserName + "于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobTypeName + "安全证需要您进行审批，请您及时处理。", data.Id);
                }
                else if (data.JobType == "EquOverhaulClean")
                {
                    DataTable dt = userbll.GetUserTable(data.PowerCutPersonId.Split(','));
                    JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), data.PowerCutPerson, "ZYAQZ008", data.JobTypeName + "安全证待您进行停电，请您及时处理。", data.CreateUserName + "于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobTypeName + "安全证需要您进行停电操作，请您及时处理。", data.Id);
                }
                return Success("操作成功.");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        /// <summary>
        /// 保存作业安全证 备案、验收、停电、送电 操作
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveOperateForm(DangerousJobOperateEntity entity)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                entity.SignImg = string.IsNullOrWhiteSpace(entity.SignImg) ? "" : entity.SignImg.Replace("../..", "");
                dangerousjoboperatebll.SaveForm("", entity);
                var jobSafetyentity = jobSafetyCardApplybll.GetEntity(entity.RecId);
                UserBLL userbll = new UserBLL();
                switch (entity.OperateType)
                {
                    case 0:
                        jobSafetyentity.JobState = 11;
                        break;
                    case 1:
                        jobSafetyentity.JobState = 8;
                        break;
                    case 2:
                        jobSafetyentity.JobState = 1;
                        break;
                    case 3:
                        jobSafetyentity.JobState = 11;
                        break;
                    default:
                        break;
                }
                jobSafetyCardApplybll.SaveForm(jobSafetyentity.Id, jobSafetyentity);
                switch (entity.OperateType)
                {
                    case 1:
                        JPushApi.PushMessage(userbll.GetEntity(jobSafetyentity.CreateUserId).Account, jobSafetyentity.CreateUserName, "ZYAQZ006", jobSafetyentity.JobTypeName + "安全证申请已备案，请您知晓。", "您于" + jobSafetyentity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + jobSafetyentity.JobTypeName + "安全证已备案，请您知晓。", jobSafetyentity.Id);
                        break;
                    case 2:
                        //给申请人发送已经停电消息
                        JPushApi.PushMessage(userbll.GetEntity(jobSafetyentity.CreateUserId).Account, jobSafetyentity.CreateUserName, "ZYAQZ006", jobSafetyentity.JobTypeName + "安全证申请已停电，请您知晓。", "您于" + jobSafetyentity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + jobSafetyentity.JobTypeName + "安全证已停电，请您知晓。", jobSafetyentity.Id);

                        //给审核人发送审核消息
                        DangerousJobFlowDetailEntity flow = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == jobSafetyentity.Id && t.Status == 0).FirstOrDefault();
                        string userids = dangerousjobflowdetailbll.GetCurrentStepUser(jobSafetyentity.Id, flow.Id);
                        DataTable dt = userbll.GetUserTable(userids.Split(','));
                        JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "ZYAQZ003", jobSafetyentity.JobTypeName + "安全证申请待您审批，请您及时处理。", jobSafetyentity.CreateUserName + "于" + jobSafetyentity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + jobSafetyentity.JobTypeName + "安全证需要您进行审批，请您及时处理。", jobSafetyentity.Id);
                        break;
                    case 3:
                        DataTable dt1 = userbll.GetUserTable((jobSafetyentity.CreateUserId + "," + jobSafetyentity.JobPersonId).Split(','));
                        JPushApi.PushMessage(string.Join(",", dt1.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt1.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "ZYAQZ011", jobSafetyentity.JobTypeName + "已结束，现已送电，请您知晓。", "您于" + jobSafetyentity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + jobSafetyentity.JobTypeName + "现已送电，请您知晓。", jobSafetyentity.Id);
                        break;
                    default:
                        break;
                }
                return Success("保存成功");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// 获取操作记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="OperateType"></param>
        /// <returns></returns>
        public ActionResult GetDangerousJobOperate(string keyValue, int OperateType)
        {
            try
            {
                var data = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == OperateType).ToList();
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// 获取停送电记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetElcGridTable(string keyValue)
        {
            try
            {
                var Empty = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 2).FirstOrDefault();
                var Full = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 3).FirstOrDefault();
                var data = new
                {
                    EmptyEqu = Empty == null ? "" : Empty.OperateOpinion,
                    EmptyTime = Empty == null ? "" : Convert.ToDateTime(Empty.OperateTime).ToString("yyyy-MM-dd HH:mm"),
                    EmptyPerson = Empty == null ? "" : Empty.OperatePerson,
                    EmptySignImg = Empty == null ? "" : Empty.SignImg,
                    FullTime = Full == null ? "" : Convert.ToDateTime(Full.OperateTime).ToString("yyyy-MM-dd HH:mm"),
                    FullPerson = Full == null ? "" : Full.OperatePerson,
                    FullSignImg = Full == null ? "" : Full.SignImg,
                };
                List<object> result = new List<object>();
                result.Add(data);
                return ToJsonResult(result);
            }
            catch (Exception)
            {

                throw;
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
                var entity = jobSafetyCardApplybll.GetEntity(BusinessId);
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
                    case 3: //措施确认中状态
                        OperatorName = entity.MeasurePerson;
                        break;
                    case 4: //停电中状态
                        OperatorName = entity.PowerCutPerson;
                        break;
                    case 5: //备案中状态
                        OperatorName = entity.RecordsPerson;
                        break;
                    case 6: //验收中状态
                        OperatorName = entity.CheckPerson;
                        break;
                    case 7: //送电中
                        OperatorName = entity.PowerGivePerson;
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
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                jobSafetyCardApplybll.ExchangeForm(keyValue, TransferUserName, TransferUserAccount, TransferUserId, user);
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";
                pagination.p_fields = @"case when t.jobstate=0 then '申请中' when t.jobstate=1 then '审核中' when t.jobstate=2 then '审核不通过' when t.jobstate=3 then '措施确认中' when t.jobstate=4 then '待停电' when t.jobstate=5 then '备案中' when t.jobstate=6 then '验收中' when t.jobstate=7 then '待送电' when t.jobstate=8 then '开始作业' when t.jobstate=9 then '暂停作业' when t.jobstate=10 then '作业中' when t.jobstate=11 then '已结束' else '' end as jobstatename,t.applyno,t.jobtypename,t.jobdeptname,
                        t.jobplace,(to_char(t.jobstarttime,'yyyy-mm-dd hh24:mi') || '~' || to_char(t.jobendtime,'yyyy-mm-dd hh24:mi')) as jobtime,(to_char(t.realityjobstarttime,'yyyy-mm-dd hh24:mi') || '~' ||to_char(t.realityjobendtime,'yyyy-mm-dd hh24:mi')) as realityjobtime,t.applyusername,to_char(t.applytime,'yyyy-mm-dd hh24:mi') as applytime,t.issubmit,t1.id as flowdetailid,'' as isrole,recordspersonid,checkpersonid,measurepersonid,powercutpersonid,powergivepersonid,t.jobtype,'' as approvename,'' as approveid,'' as approveaccount";
                DataTable exportTable = jobSafetyCardApplybll.GetPageList(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "作业安全证信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "作业安全证信息.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobstatename", ExcelColumn = "作业许可状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyno", ExcelColumn = "编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobtypename", ExcelColumn = "作业类型", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobdeptname", ExcelColumn = "作业单位", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobplace", ExcelColumn = "作业地点", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobtime", ExcelColumn = "作业时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realityjobtime", ExcelColumn = "实际作业时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyusername", ExcelColumn = "申请人", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applytime", ExcelColumn = "申请时间", Width = 20 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        /// <summary>
        /// 导出高风险作业
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportDataForHeightWork(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/高处作业安全证及安全措施.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("applyno"); //编号
            dt.Columns.Add("applydept"); //申请单位
            dt.Columns.Add("applyperson"); //申请人
            dt.Columns.Add("jobtime"); //作业时间
            dt.Columns.Add("jobplace"); //作业地点
            dt.Columns.Add("jobcontent"); //作业内容
            dt.Columns.Add("jobheight"); //作业高度
            dt.Columns.Add("joblevel"); //作业级别
            dt.Columns.Add("custodian"); //监护人
            dt.Columns.Add("safetymeasures"); //安全措施
            dt.Columns.Add("dutyperson"); //作业负责人
            dt.Columns.Add("approveidea1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveperson1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approvedate1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveidea2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approveperson2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approvedate2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("copyidea"); //中心（部门）安全管理组备案
            dt.Columns.Add("copyperson"); //中心（部门）安全管理组备案
            dt.Columns.Add("copydate"); //中心（部门）安全管理组备案
            DataRow row = dt.NewRow();

            JobSafetyCardApplyEntity entity = jobSafetyCardApplybll.GetEntity(keyValue);
            row["applyno"] = entity.ApplyNo;
            row["applydept"] = entity.ApplyDeptName;
            row["applyperson"] = entity.ApplyUserName;
            row["jobtime"] = "自" + entity.JobStartTime.Value.ToString("yyyy年MM月dd日HH时mm分") + "至" + entity.JobEndTime.Value.ToString("yyyy年MM月dd日HH时mm分");
            row["jobplace"] = entity.JobPlace;
            row["jobcontent"] = entity.JobContent;
            row["jobheight"] = entity.JobHeight;
            row["joblevel"] = string.IsNullOrWhiteSpace(entity.JobLevel) ? "" : jobSafetyCardApplybll.getName(entity.JobType, entity.JobLevel, "001");
            row["custodian"] = entity.Custodian;
            row["safetymeasures"] = entity.SafetyMeasures;
            row["dutyperson"] = entity.DutyPerson;
            if (string.IsNullOrWhiteSpace(entity.SignUrl))
            {
                row["dutyperson"] = Server.MapPath("~/content/Images/no_1.png");
            }
            else
            {
                var filepath = Server.MapPath("~") + entity.SignUrl.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    row["dutyperson"] = filepath;
                }
                else
                {
                    row["dutyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }

            //审核记录
            var approvedata = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.ApplyNumber == entity.ApplyNumber && t.Status == 1).ToList();
            if (approvedata.Count > 0)
            {
                row["approveidea1"] = !string.IsNullOrEmpty(approvedata[0].ApproveOpinion) ? approvedata[0].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[0].SignUrl))
                {
                    row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[0].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson1"] = filepath;
                    }
                    else
                    {
                        row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate1"] = approvedata[0].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            if (approvedata.Count > 1)
            {
                row["approveidea2"] = !string.IsNullOrEmpty(approvedata[1].ApproveOpinion) ? approvedata[1].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[1].SignUrl))
                {
                    row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[1].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson2"] = filepath;
                    }
                    else
                    {
                        row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate2"] = approvedata[1].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            //备案信息
            var copydata = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 1).ToList();
            if (copydata.Count > 0)
            {
                row["copyidea"] = !string.IsNullOrEmpty(copydata[0].OperateOpinion) ? copydata[0].OperateOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(copydata[0].SignImg))
                {
                    row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + copydata[0].SignImg.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["copyperson"] = filepath;
                    }
                    else
                    {
                        row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["copydate"] = copydata[0].OperateTime.Value.ToString("yyyy年MM月dd日");
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            Table table = (Table)doc.GetChild(NodeType.Table, 0, true);
            for (int i = 8; i < 12; i++)
            {
                for (int j = 0; j < 9; j += 2)
                {
                    if (("," + entity.DangerousDecipher + ",").IndexOf("," + table.Rows[i].Cells[j].ToString(SaveFormat.Text).Replace("\r\n", "") + ",") >= 0)
                    {
                        Cell lcell = table.Rows[i].Cells[j + 1];
                        lcell.FirstParagraph.Remove();
                        Paragraph p = new Paragraph(doc);
                        string value = "√";
                        p.AppendChild(new Run(doc, value));
                        lcell.AppendChild(p);
                    }
                }
            }
            //安全措施
            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sort");
            dt1.Columns.Add("content");
            dt1.Columns.Add("result");
            var measure = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == "HeightWorking" && t.ConfigType == "0" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
            if (measure != null)
            {
                var measuredata = safetymeasuredetailbll.GetList("").Where(t => t.RecId == measure.Id).OrderBy(t => t.SortNum).ToList();
                for (int i = 0; i < measuredata.Count(); i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sort"] = measuredata[i].SortNum;
                    if (i == measuredata.Count - 1)
                    {
                        row1["content"] = measuredata[i].Content + entity.MeasureContent;
                    }
                    else
                    {
                        row1["content"] = measuredata[i].Content;
                    }
                    if (("/" + entity.SafetyMeasures + "/").IndexOf("/" + measuredata[i].SortNum + "/") >= 0)
                    {
                        row1["result"] = "√";
                    }
                    dt1.Rows.Add(row1);
                }
            }
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("作业安全证高处作业_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        /// <summary>
        /// 受限空间作业安全证及安全措施导出
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportDataForLimitedSpace(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/受限空间作业安全证及安全措施.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("applyno"); //编号
            dt.Columns.Add("applydept"); //申请单位
            dt.Columns.Add("applyperson"); //申请人
            dt.Columns.Add("jobtime"); //作业时间
            dt.Columns.Add("jobplace"); //作业地点
            dt.Columns.Add("signurl"); //负责人签名 
            dt.Columns.Add("jobticketno");  //生产工序
            dt.Columns.Add("limitedspacedept"); //受限空间所在单位
            dt.Columns.Add("limitedspacename"); //受限空间名称
            dt.Columns.Add("jobcontent"); //检修作业内容
            dt.Columns.Add("limitedspacemedia");   //受限空间主要介质
            dt.Columns.Add("custodian"); //监护人
            dt.Columns.Add("safetymeasures"); //安全措施
            dt.Columns.Add("dutyperson"); //作业负责人
            dt.Columns.Add("approveidea1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveperson1"); //中心（部门）设备管理负责人审核 8
            dt.Columns.Add("approvedate1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveidea2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approveperson2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approvedate2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("copyidea"); //中心（部门）安全管理组备案
            dt.Columns.Add("copyperson"); //中心（部门）安全管理组备案
            dt.Columns.Add("copydate"); //中心（部门）安全管理组备案
            dt.Columns.Add("confirmmeasures"); //生产单位安全措施
            DataRow row = dt.NewRow();
            JobSafetyCardApplyEntity entity = jobSafetyCardApplybll.GetEntity(keyValue);
            row["applyno"] = entity.ApplyNo;
            row["applydept"] = entity.ApplyDeptName;
            row["applyperson"] = entity.ApplyUserName;
            row["jobtime"] = "自" + entity.JobStartTime.Value.ToString("yyyy年MM月dd日HH时mm分") + "至" + entity.JobEndTime.Value.ToString("yyyy年MM月dd日HH时mm分");
            row["jobplace"] = entity.JobPlace;
            row["jobticketno"] = entity.JobTicketNo;
            row["applyno"] = entity.ApplyNo;
            row["confirmmeasures"] = entity.ConfirmMeasures;
            if (!string.IsNullOrEmpty(entity.SignUrl))
            {
                var signurl = Server.MapPath("~/") + entity.SignUrl.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(signurl))
                {
                    row["signurl"] = signurl;
                }
                else
                {
                    row["signurl"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }
            row["limitedspacedept"] = entity.LimitedSpaceDept;
            row["limitedspacename"] = entity.LimitedSpaceName;
            row["jobcontent"] = entity.JobContent;
            row["limitedspacemedia"] = entity.LimitedSpaceMedia;
            row["custodian"] = entity.Custodian;
            row["dutyperson"] = entity.DutyPerson;
            row["safetymeasures"] = entity.SafetyMeasures;

            //审核记录
            var approvedata = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.ApplyNumber == entity.ApplyNumber && t.Status == 1).ToList();
            if (approvedata.Count > 0)
            {
                row["approveidea1"] = !string.IsNullOrEmpty(approvedata[0].ApproveOpinion) ? approvedata[0].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[0].SignUrl))
                {
                    row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[0].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson1"] = filepath;
                    }
                    else
                    {
                        row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate1"] = approvedata[0].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            if (approvedata.Count > 1)
            {
                row["approveidea2"] = !string.IsNullOrEmpty(approvedata[1].ApproveOpinion) ? approvedata[1].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[1].SignUrl))
                {
                    row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[1].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson2"] = filepath;
                    }
                    else
                    {
                        row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate2"] = approvedata[1].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            
            var copydata = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 1).ToList();
            if (copydata.Count > 0)
            {
                row["copyidea"] = !string.IsNullOrEmpty(copydata[0].OperateOpinion) ? copydata[0].OperateOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(copydata[0].SignImg))
                {
                    row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + copydata[0].SignImg.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["copyperson"] = filepath;
                    }
                    else
                    {
                        row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["copydate"] = copydata[0].OperateTime.Value.ToString("yyyy年MM月dd日");
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);

            // 采样分析
            WhenHotAnalysisBLL whenhotanalysisbll = new WhenHotAnalysisBLL();
            var data = whenhotanalysisbll.GetList("");
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                data = data.Where(t => t.RecId == keyValue);
                if (data != null)
                {
                    DataTable dtc = new DataTable("C");
                    dtc.Columns.Add("dangerousdata");
                    dtc.Columns.Add("gasdata");
                    dtc.Columns.Add("oxygencontentdata");
                    dtc.Columns.Add("analysisdate");
                    dtc.Columns.Add("samplingplace");
                    dtc.Columns.Add("analysisperson");
                    foreach (WhenHotAnalysisEntity item in data)
                    {
                        DataRow row1 = dtc.NewRow();
                        row1["dangerousdata"] = item.DangerousData;
                        row1["gasdata"] = item.GasData;
                        row1["oxygencontentdata"] = item.OxygenContentData;
                        row1["analysisdate"] = item.AnalysisDate;
                        row1["samplingplace"] = item.SamplingPlace;
                        row1["analysisperson"] = item.AnalysisPerson;
                        dtc.Rows.Add(row1);
                    }
                    doc.MailMerge.ExecuteWithRegions(dtc);
                }
            }






            //受限空间所在单位安全措施
            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sort");
            dt1.Columns.Add("content");
            dt1.Columns.Add("result");
            var measure = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == "LimitedSpace" && t.ConfigType == "1" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
            if (measure != null)
            {
                var measuredata = safetymeasuredetailbll.GetList("").Where(t => t.RecId == measure.Id).OrderBy(t => t.SortNum).ToList();
                for (int i = 0; i < measuredata.Count(); i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sort"] = "A" + measuredata[i].SortNum.ToString();
                    if (i == measuredata.Count - 1)
                    {
                        row1["content"] = measuredata[i].Content + entity.ConfirmMeasureContent;
                    }
                    else
                    {
                        row1["content"] = measuredata[i].Content;
                    }
                    if (("/" + entity.SafetyMeasures + "/").IndexOf("/" + measuredata[i].SortNum + "/") >= 0)
                    {
                        row1["result"] = "√";
                    }
                    dt1.Rows.Add(row1);
                }
            }


            //受限空间所在单位安全措施
            DataTable dt2 = new DataTable("B");
            dt2.Columns.Add("sort");
            dt2.Columns.Add("content");
            dt2.Columns.Add("result");
            var measure2 = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == "LimitedSpace" && t.ConfigType == "0" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
            if (measure2 != null)
            {
                var measuredata = safetymeasuredetailbll.GetList("").Where(t => t.RecId == measure2.Id).OrderBy(t => t.SortNum).ToList();
                for (int i = 0; i < measuredata.Count(); i++)
                {
                    DataRow row1 = dt2.NewRow();
                    row1["sort"] = "B" + measuredata[i].SortNum.ToString();
                    if (i == measuredata.Count - 1)
                    {
                        row1["content"] = measuredata[i].Content + entity.MeasureContent;
                    }
                    else
                    {
                        row1["content"] = measuredata[i].Content;
                    }
                    if (("/" + entity.SafetyMeasures + "/").IndexOf("/" + measuredata[i].SortNum + "/") >= 0)
                    {
                        row1["result"] = "√";
                    }
                    dt2.Rows.Add(row1);
                }
            }
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.ExecuteWithRegions(dt2);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("作业安全证受限空间作业_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        
        /// <summary>
        /// 设备检修清理安全作业证导出
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportDataForEquOverhaulClean(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/设备检修清理导出模板.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("applyno"); //编号
            dt.Columns.Add("applydept"); //申请单位
            dt.Columns.Add("applyperson"); //申请人
            dt.Columns.Add("jobtime"); //作业时间
            dt.Columns.Add("jobplace"); //作业地点
            dt.Columns.Add("signurl"); //作业负责人签名 
            dt.Columns.Add("confirmsignimg"); //执行人签字 
            dt.Columns.Add("jobticketno");  //生产工序
            dt.Columns.Add("jobcontent"); //检修作业内容
            dt.Columns.Add("custodian"); //监护人
            dt.Columns.Add("safetymeasures"); //检修单位安全措施
            dt.Columns.Add("confirmmeasures"); //生产单位安全措施
            dt.Columns.Add("dutyperson"); //作业负责人
            dt.Columns.Add("approveidea1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveperson1"); //中心（部门）设备管理负责人审核 8
            dt.Columns.Add("approvedate1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveidea2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approveperson2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approvedate2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("copyidea"); //中心（部门）安全管理组备案
            dt.Columns.Add("copyperson"); //中心（部门）安全管理组备案
            dt.Columns.Add("copydate"); //中心（部门）安全管理组备案
            dt.Columns.Add("equname");
            dt.Columns.Add("emptydate");
            dt.Columns.Add("emptyperson");
            dt.Columns.Add("fulldate");
            dt.Columns.Add("fullperson");
            DataRow row = dt.NewRow();
            JobSafetyCardApplyEntity entity = jobSafetyCardApplybll.GetEntity(keyValue);
            row["applyno"] = entity.ApplyNo;
            row["applydept"] = entity.ApplyDeptName;
            row["applyperson"] = entity.ApplyUserName;
            row["jobtime"] = "自" + entity.JobStartTime.Value.ToString("yyyy年MM月dd日HH时mm分") + "至" + entity.JobEndTime.Value.ToString("yyyy年MM月dd日HH时mm分");
            row["jobplace"] = entity.JobPlace;
            row["jobticketno"] = entity.JobTicketNo;
            row["applyno"] = entity.ApplyNo;
            row["jobcontent"] = entity.JobContent;
            row["custodian"] = entity.Custodian;
            row["dutyperson"] = entity.DutyPerson;
            row["safetymeasures"] = entity.SafetyMeasures;
            row["confirmmeasures"] = entity.ConfirmMeasures;

            if (!string.IsNullOrEmpty(entity.SignUrl))
            {
                var signurl = Server.MapPath("~/") + entity.SignUrl.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(signurl))
                {
                    row["signurl"] = signurl;
                }
                else
                {
                    row["signurl"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }


            if (!string.IsNullOrEmpty(entity.ConfirmSignUrl))
            {
                var confirmsignimg = Server.MapPath("~/") + entity.ConfirmSignUrl.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(confirmsignimg))
                {
                    row["confirmsignimg"] = confirmsignimg;
                }
                else
                {
                    row["confirmsignimg"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }



            //审核记录
            var approvedata = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.ApplyNumber == entity.ApplyNumber && t.Status == 1).ToList();
            if (approvedata.Count > 0)
            {
                row["approveidea1"] = !string.IsNullOrEmpty(approvedata[0].ApproveOpinion) ? approvedata[0].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[0].SignUrl))
                {
                    row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[0].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson1"] = filepath;
                    }
                    else
                    {
                        row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate1"] = approvedata[0].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            if (approvedata.Count > 1)
            {
                row["approveidea2"] = !string.IsNullOrEmpty(approvedata[1].ApproveOpinion) ? approvedata[1].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[1].SignUrl))
                {
                    row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[1].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson2"] = filepath;
                    }
                    else
                    {
                        row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate2"] = approvedata[1].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            
            var copydata = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 1).ToList();
            if (copydata.Count > 0)
            {
                row["copyidea"] = !string.IsNullOrEmpty(copydata[0].OperateOpinion) ? copydata[0].OperateOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(copydata[0].SignImg))
                {
                    row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + copydata[0].SignImg.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["copyperson"] = filepath;
                    }
                    else
                    {
                        row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["copydate"] = copydata[0].OperateTime.Value.ToString("yyyy年MM月dd日");
            }

            //停送电
            var Empty = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 2).OrderByDescending(t => t.CreateDate).FirstOrDefault(); //获取停电记录
            var Full = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 3).OrderByDescending(t => t.CreateDate).FirstOrDefault(); //获取送电记录
            if (Empty != null)
            {
                row["equname"] = Empty.Remark;
                row["emptydate"] = Empty.OperateTime.Value.ToString("yyyy年MM月dd日HH时mm分");
                if (string.IsNullOrWhiteSpace(Empty.SignImg))
                {
                    row["emptyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + Empty.SignImg.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["emptyperson"] = filepath;
                    }
                    else
                    {
                        row["emptyperson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
            }
            if (Full != null)
            {
                row["fulldate"] = Full.OperateTime.Value.ToString("yyyy年MM月dd日HH时mm分");
                if (string.IsNullOrWhiteSpace(Full.SignImg))
                {
                    row["fullperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + Full.SignImg.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["fullperson"] = filepath;
                    }
                    else
                    {
                        row["fullperson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);

            Table table = (Table)doc.GetChild(NodeType.Table, 0, true);
            for (int i = 6; i < 10; i++)
            {
                for (int j = 0; j < 9; j += 2)
                {
                    if (("," + entity.DangerousDecipher + ",").IndexOf("," + table.Rows[i].Cells[j].ToString(SaveFormat.Text).Replace("\r\n", "") + ",") >= 0)
                    {
                        Cell lcell = table.Rows[i].Cells[j + 1];
                        lcell.FirstParagraph.Remove();
                        Paragraph p = new Paragraph(doc);
                        string value = "√";
                        p.AppendChild(new Run(doc, value));
                        lcell.AppendChild(p);
                    }
                }
            }






            //生产单位安全措施
            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sort");
            dt1.Columns.Add("content");
            dt1.Columns.Add("result");
            var measure = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == "EquOverhaulClean" && t.ConfigType == "1" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
            if (measure != null)
            {
                var measuredata = safetymeasuredetailbll.GetList("").Where(t => t.RecId == measure.Id).OrderBy(t => t.SortNum).ToList();
                for (int i = 0; i < measuredata.Count(); i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sort"] = "A" + measuredata[i].SortNum.ToString();
                    if (i == measuredata.Count - 1)
                    {
                        row1["content"] = measuredata[i].Content + entity.ConfirmMeasureContent;
                    }
                    else
                    {
                        row1["content"] = measuredata[i].Content;
                    }
                    if (("/" + entity.ConfirmMeasures + "/").IndexOf("/" + measuredata[i].SortNum + "/") >= 0)
                    {
                        row1["result"] = "√";
                    }
                    dt1.Rows.Add(row1);
                }
            }


            //检修单位安全措施
            DataTable dt2 = new DataTable("B");
            dt2.Columns.Add("sort");
            dt2.Columns.Add("content");
            dt2.Columns.Add("result");
            var measure2 = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == "EquOverhaulClean" && t.ConfigType == "0" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
            if (measure2 != null)
            {
                var measuredata = safetymeasuredetailbll.GetList("").Where(t => t.RecId == measure2.Id).OrderBy(t => t.SortNum).ToList();
                for (int i = 0; i < measuredata.Count(); i++)
                {
                    DataRow row1 = dt2.NewRow();
                    row1["sort"] = "B" + measuredata[i].SortNum.ToString();
                    if (i == measuredata.Count - 1)
                    {
                        row1["content"] = measuredata[i].Content + entity.MeasureContent;
                    }
                    else
                    {
                        row1["content"] = measuredata[i].Content;
                    }
                    if (("/" + entity.SafetyMeasures + "/").IndexOf("/" + measuredata[i].SortNum + "/") >= 0)
                    {
                        row1["result"] = "√";
                    }
                    dt2.Rows.Add(row1);
                }
            }
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.ExecuteWithRegions(dt2);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("作业安全证设备检修清理作业_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }
        

        /// <summary>
        /// 起重吊装作业安全证及安全措施导出
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportDataForLifting(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/起重吊装作业安全证及安全措施导出模板.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("applyno"); //编号
            dt.Columns.Add("applydept"); //申请单位
            dt.Columns.Add("applyperson"); //申请人
            dt.Columns.Add("jobtime"); //作业时间
            dt.Columns.Add("jobplace"); //作业地点
            dt.Columns.Add("dutyperson"); //作业负责人
            dt.Columns.Add("hoistingequname");
            dt.Columns.Add("jobperson"); 
            dt.Columns.Add("worktypecard");  
            dt.Columns.Add("custodian");   
            dt.Columns.Add("hoistingcommand");
            dt.Columns.Add("weightquality");
            dt.Columns.Add("jobdeptname");
            dt.Columns.Add("signurl");   
            dt.Columns.Add("jobcontent"); //作业内容
            dt.Columns.Add("safetymeasures"); //安全措施
            dt.Columns.Add("approveidea1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveperson1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approvedate1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveidea2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approveperson2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approvedate2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("copyidea"); //中心（部门）安全管理组备案
            dt.Columns.Add("copyperson"); //中心（部门）安全管理组备案
            dt.Columns.Add("copydate"); //中心（部门）安全管理组备案
            DataRow row = dt.NewRow();

            JobSafetyCardApplyEntity entity = jobSafetyCardApplybll.GetEntity(keyValue);
            row["applyno"] = entity.ApplyNo;
            row["applydept"] = entity.ApplyDeptName;
            row["applyperson"] = entity.ApplyUserName;
            row["jobtime"] = "自" + entity.JobStartTime.Value.ToString("yyyy年MM月dd日HH时mm分") + "至" + entity.JobEndTime.Value.ToString("yyyy年MM月dd日HH时mm分");
            row["jobplace"] = entity.JobPlace;
            row["jobcontent"] = entity.JobContent;
            row["safetymeasures"] = entity.SafetyMeasures;
            row["hoistingequname"] = entity.HoistingEquName;
            row["jobperson"] = entity.JobPerson;
            row["worktypecard"] = entity.WorkTypeCard;
            row["custodian"] = entity.Custodian;
            row["hoistingcommand"] = entity.HoistingCommand;
            row["weightquality"] = entity.WeightQuality; 
            row["dutyperson"] = entity.DutyPerson;
            row["jobdeptname"] = entity.JobDeptName;
            if (!string.IsNullOrEmpty(entity.SignUrl))
            {
                var signurl = Server.MapPath("~/") + entity.SignUrl.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(signurl))
                {
                    row["dutyperson"] = signurl;
                }
                else
                {
                    row["dutyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }
            else
            {
                row["dutyperson"] = Server.MapPath("~/content/Images/no_1.png");
            }
            //审核记录
            var approvedata = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.ApplyNumber == entity.ApplyNumber && t.Status == 1).ToList();
            if (approvedata.Count > 0)
            {
                row["approveidea1"] = !string.IsNullOrEmpty(approvedata[0].ApproveOpinion) ? approvedata[0].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[0].SignUrl))
                {
                    row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[0].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson1"] = filepath;
                    }
                    else
                    {
                        row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate1"] = approvedata[0].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            if (approvedata.Count > 1)
            {
                row["approveidea2"] = !string.IsNullOrEmpty(approvedata[1].ApproveOpinion) ? approvedata[1].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[1].SignUrl))
                {
                    row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[1].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson2"] = filepath;
                    }
                    else
                    {
                        row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate2"] = approvedata[1].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            //备案信息
            var copydata = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 1).ToList();
            if (copydata.Count > 0)
            {
                row["copyidea"] = !string.IsNullOrEmpty(copydata[0].OperateOpinion) ? copydata[0].OperateOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(copydata[0].SignImg))
                {
                    row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + copydata[0].SignImg.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["copyperson"] = filepath;
                    }
                    else
                    {
                        row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["copydate"] = copydata[0].OperateTime.Value.ToString("yyyy年MM月dd日");
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            Table table = (Table)doc.GetChild(NodeType.Table, 0, true);
            for (int i = 8; i < 12; i++)
            {
                for (int j = 0; j < 9; j += 2)
                {
                    if (("," + entity.DangerousDecipher + ",").IndexOf("," + table.Rows[i].Cells[j].ToString(SaveFormat.Text).Replace("\r\n", "") + ",") >= 0)
                    {
                        Cell lcell = table.Rows[i].Cells[j + 1];
                        lcell.FirstParagraph.Remove();
                        Paragraph p = new Paragraph(doc);
                        string value = "√";
                        p.AppendChild(new Run(doc, value));
                        lcell.AppendChild(p);
                    }
                }
            }
            //安全措施
            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sort");
            dt1.Columns.Add("content");
            dt1.Columns.Add("result");
            var measure = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == "Lifting" && t.ConfigType == "0" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
            if (measure != null)
            {
                var measuredata = safetymeasuredetailbll.GetList("").Where(t => t.RecId == measure.Id).OrderBy(t => t.SortNum).ToList();
                for (int i = 0; i < measuredata.Count(); i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sort"] = measuredata[i].SortNum;
                    if (i == measuredata.Count - 1)
                    {
                        row1["content"] = measuredata[i].Content + entity.MeasureContent;
                    }
                    else
                    {
                        row1["content"] = measuredata[i].Content;
                    }
                    if (("/" + entity.SafetyMeasures + "/").IndexOf("/" + measuredata[i].SortNum + "/") >= 0)
                    {
                        row1["result"] = "√";
                    }
                    dt1.Rows.Add(row1);
                }
            }
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("作业安全证起重吊装作业_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }


        /// <summary>
        /// 动火作业安全证及安全措施导出
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportDataForWhenHot(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/动火作业安全证导出模板.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("applyno"); //编号
            dt.Columns.Add("applydept"); //申请单位
            dt.Columns.Add("applyperson"); //申请人
            dt.Columns.Add("jobtime"); //作业时间
            dt.Columns.Add("jobplace"); //作业地点
            dt.Columns.Add("dutyperson"); //作业负责人

            dt.Columns.Add("jobperson");
             dt.Columns.Add("custodian");
            dt.Columns.Add("jobdeptname");

            dt.Columns.Add("jobcontent"); //作业内容
            dt.Columns.Add("safetymeasures"); //安全措施
            dt.Columns.Add("approveidea1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveperson1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approvedate1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveidea2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approveperson2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approvedate2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approveidea3"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveperson3"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approvedate3"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveidea4"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approveperson4"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approvedate4"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("copyidea"); //中心（部门）安全管理组备案
            dt.Columns.Add("copyperson"); //中心（部门）安全管理组备案
            dt.Columns.Add("copydate"); //中心（部门）安全管理组备案
            dt.Columns.Add("jobdlevel");
            dt.Columns.Add("AnalysisDate1");
            dt.Columns.Add("SamplingPlace1");
            dt.Columns.Add("AnalysisData1");
            dt.Columns.Add("AnalysisDate2");
            dt.Columns.Add("SamplingPlace2");
            dt.Columns.Add("AnalysisData2");
            dt.Columns.Add("AnalysisDate3");
            dt.Columns.Add("SamplingPlace3");
            dt.Columns.Add("AnalysisData3");
            DataRow row = dt.NewRow();

            JobSafetyCardApplyEntity entity = jobSafetyCardApplybll.GetEntity(keyValue);
            row["applyno"] = entity.ApplyNo;
            row["applydept"] = entity.ApplyDeptName;
            row["applyperson"] = entity.ApplyUserName;
            row["jobtime"] = "自" + entity.JobStartTime.Value.ToString("yyyy年MM月dd日HH时mm分") + "至" + entity.JobEndTime.Value.ToString("yyyy年MM月dd日HH时mm分");
            row["jobplace"] = entity.JobPlace;
            row["jobcontent"] = entity.JobContent;
            row["safetymeasures"] = entity.SafetyMeasures;
            row["dutyperson"] = entity.DutyPerson;
            row["jobperson"] = entity.JobPerson;
            row["custodian"] = entity.Custodian;
            row["jobdeptname"] = entity.JobDeptName;
            entity.JobLevelName = string.IsNullOrWhiteSpace(entity.JobLevel) ? "" : jobSafetyCardApplybll.getName(entity.JobType, entity.JobLevel, "001");
            row["jobdlevel"] = entity.JobLevelName.Replace("动火", "").Replace("级", "");
            if (!string.IsNullOrEmpty(entity.SignUrl))
            {
                var signurl = Server.MapPath("~/") + entity.SignUrl.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(signurl))
                {
                    row["dutyperson"] = signurl;
                }
                else
                {
                    row["dutyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }
            else
            {
                row["dutyperson"] = Server.MapPath("~/content/Images/no_1.png");
            }
            //审核记录
            var approvedata = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.ApplyNumber == entity.ApplyNumber && t.Status == 1).ToList();
            if (approvedata.Count > 0)
            {
                row["approveidea1"] = !string.IsNullOrEmpty(approvedata[0].ApproveOpinion) ? approvedata[0].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[0].SignUrl))
                {
                    row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[0].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson1"] = filepath;
                    }
                    else
                    {
                        row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate1"] = approvedata[0].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            if (approvedata.Count > 1)
            {
                row["approveidea2"] = !string.IsNullOrEmpty(approvedata[1].ApproveOpinion) ? approvedata[1].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[1].SignUrl))
                {
                    row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[1].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson2"] = filepath;
                    }
                    else
                    {
                        row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate2"] = approvedata[1].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            if (approvedata.Count > 2)
            {
                row["approveidea3"] = !string.IsNullOrEmpty(approvedata[2].ApproveOpinion) ? approvedata[2].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[2].SignUrl))
                {
                    row["approveperson3"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[2].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson3"] = filepath;
                    }
                    else
                    {
                        row["approveperson3"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate3"] = approvedata[2].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            if (approvedata.Count > 3)
            {
                row["approveidea4"] = !string.IsNullOrEmpty(approvedata[3].ApproveOpinion) ? approvedata[3].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[3].SignUrl))
                {
                    row["approveperson4"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[3].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson4"] = filepath;
                    }
                    else
                    {
                        row["approveperson4"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate4"] = approvedata[3].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            //备案信息
            var copydata = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 1).ToList();
            if (copydata.Count > 0)
            {
                row["copyidea"] = !string.IsNullOrEmpty(copydata[0].OperateOpinion) ? copydata[0].OperateOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(copydata[0].SignImg))
                {
                    row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + copydata[0].SignImg.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["copyperson"] = filepath;
                    }
                    else
                    {
                        row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["copydate"] = copydata[0].OperateTime.Value.ToString("yyyy年MM月dd日");
            }
            var AnalysisData = whenhotanalysisbll.GetList("").Where(t => t.RecId == keyValue).ToList();
            if (AnalysisData.Count() > 0)
            {
                row["AnalysisDate1"]= AnalysisData[0].AnalysisDate.Value.ToString("yyyy年MM月dd日");
                row["SamplingPlace1"] = AnalysisData[0].SamplingPlace;
                row["AnalysisData1"] = AnalysisData[0].AnalysisData;
            }
            if (AnalysisData.Count() > 1)
            {
                row["AnalysisDate2"] = AnalysisData[1].AnalysisDate.Value.ToString("yyyy年MM月dd日");
                row["SamplingPlace2"] = AnalysisData[1].SamplingPlace;
                row["AnalysisData2"] = AnalysisData[1].AnalysisData;
            }
            if (AnalysisData.Count() > 2)
            {
                row["AnalysisDate3"] = AnalysisData[2].AnalysisDate.Value.ToString("yyyy年MM月dd日");
                row["SamplingPlace3"] = AnalysisData[2].SamplingPlace;
                row["AnalysisData3"] = AnalysisData[2].AnalysisData;
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            Table table = (Table)doc.GetChild(NodeType.Table, 0, true);
            for (int i = 6; i < 10; i++)
            {
                for (int j = 0; j < 9; j += 2)
                {
                    if (("," + entity.DangerousDecipher + ",").IndexOf("," + table.Rows[i].Cells[j].ToString(SaveFormat.Text).Replace("\r\n", "") + ",") >= 0)
                    {
                        Cell lcell = table.Rows[i].Cells[j + 1];
                        lcell.FirstParagraph.Remove();
                        Paragraph p = new Paragraph(doc);
                        string value = "√";
                        p.AppendChild(new Run(doc, value));
                        lcell.AppendChild(p);
                    }
                }
            }
            //安全措施
            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sort");
            dt1.Columns.Add("content");
            dt1.Columns.Add("result");
            var measure = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == "WhenHot" && t.ConfigType == "0" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
            if (measure != null)
            {
                var measuredata = safetymeasuredetailbll.GetList("").Where(t => t.RecId == measure.Id).OrderBy(t => t.SortNum).ToList();
                for (int i = 0; i < measuredata.Count(); i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sort"] = measuredata[i].SortNum;
                    if (i == measuredata.Count - 1)
                    {
                        row1["content"] = measuredata[i].Content + entity.MeasureContent;
                    }
                    else
                    {
                        row1["content"] = measuredata[i].Content;
                    }
                    if (("/" + entity.SafetyMeasures + "/").IndexOf("/" + measuredata[i].SortNum + "/") >= 0)
                    {
                        row1["result"] = "√";
                    }
                    dt1.Rows.Add(row1);
                }
            }
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("作业安全证动火作业_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }


        /// <summary>
        /// 导出动土作业
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportDataForDigging(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/作业安全证动土作业.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("applyno"); //编号
            dt.Columns.Add("applydept"); //申请单位
            dt.Columns.Add("applyperson"); //申请人
            dt.Columns.Add("jobtime"); //作业时间
            dt.Columns.Add("jobplace"); //作业地点
            dt.Columns.Add("jobcontent"); //作业内容
            dt.Columns.Add("safetymeasures"); //安全措施
            dt.Columns.Add("approveidea1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveperson1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approvedate1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveidea2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approveperson2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approvedate2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("copyidea"); //中心（部门）安全管理组备案
            dt.Columns.Add("copyperson"); //中心（部门）安全管理组备案
            dt.Columns.Add("copydate"); //中心（部门）安全管理组备案
            dt.Columns.Add("dutyperson");//作业负责人
            DataRow row = dt.NewRow();

            JobSafetyCardApplyEntity entity = jobSafetyCardApplybll.GetEntity(keyValue);
            row["applyno"] = entity.ApplyNo;
            row["applydept"] = entity.ApplyDeptName;
            row["applyperson"] = entity.ApplyUserName;
            row["jobtime"] = "自" + entity.JobStartTime.Value.ToString("yyyy年MM月dd日HH时mm分") + "至" + entity.JobEndTime.Value.ToString("yyyy年MM月dd日HH时mm分");
            row["jobplace"] = entity.JobPlace;
            row["jobcontent"] = entity.JobContent;
            row["safetymeasures"] = entity.SafetyMeasures;
            if (!string.IsNullOrEmpty(entity.SignUrl))
            {
                var signurl = Server.MapPath("~/") + entity.SignUrl.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(signurl))
                {
                    row["dutyperson"] = signurl;
                }
                else
                {
                    row["dutyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }
            else
            {
                row["dutyperson"] = Server.MapPath("~/content/Images/no_1.png");
            }

            //审核记录
            var approvedata = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.ApplyNumber == entity.ApplyNumber && t.Status == 1).ToList();
            if (approvedata.Count > 0)
            {
                row["approveidea1"] = !string.IsNullOrEmpty(approvedata[0].ApproveOpinion) ? approvedata[0].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[0].SignUrl))
                {
                    row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[0].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson1"] = filepath;
                    }
                    else
                    {
                        row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate1"] = approvedata[0].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            if (approvedata.Count > 1)
            {
                row["approveidea2"] = !string.IsNullOrEmpty(approvedata[1].ApproveOpinion) ? approvedata[1].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[1].SignUrl))
                {
                    row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[1].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson2"] = filepath;
                    }
                    else
                    {
                        row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate2"] = approvedata[1].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            //备案信息
            var copydata = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 1).ToList();
            if (copydata.Count > 0)
            {
                row["copyidea"] = !string.IsNullOrEmpty(copydata[0].OperateOpinion) ? copydata[0].OperateOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(copydata[0].SignImg))
                {
                    row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + copydata[0].SignImg.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["copyperson"] = filepath;
                    }
                    else
                    {
                        row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["copydate"] = copydata[0].OperateTime.Value.ToString("yyyy年MM月dd日");
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            Table table = (Table)doc.GetChild(NodeType.Table, 0, true);
            for (int i = 6; i < 9; i++)
            {
                for (int j = 0; j < 9; j += 2)
                {
                    if (("," + entity.DangerousDecipher + ",").IndexOf("," + table.Rows[i].Cells[j].ToString(SaveFormat.Text).Replace("\r\n", "") + ",") >= 0)
                    {
                        Cell lcell = table.Rows[i].Cells[j + 1];
                        lcell.FirstParagraph.Remove();
                        Paragraph p = new Paragraph(doc);
                        string value = "√";
                        p.AppendChild(new Run(doc, value));
                        lcell.AppendChild(p);
                    }
                }
            }
            //安全措施
            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sort");
            dt1.Columns.Add("content");
            dt1.Columns.Add("result");
            var measure = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == "Digging" && t.ConfigType == "0" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
            if (measure != null)
            {
                var measuredata = safetymeasuredetailbll.GetList("").Where(t => t.RecId == measure.Id).OrderBy(t => t.SortNum).ToList();
                for (int i = 0; i < measuredata.Count(); i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sort"] = measuredata[i].SortNum;
                    if (i == measuredata.Count - 1)
                    {
                        row1["content"] = measuredata[i].Content + entity.MeasureContent;
                    }
                    else
                    {
                        row1["content"] = measuredata[i].Content;
                    }
                    if (("/" + entity.SafetyMeasures + "/").IndexOf("/" + measuredata[i].SortNum + "/") >= 0)
                    {
                        row1["result"] = "√";
                    }
                    dt1.Rows.Add(row1);
                }
            }
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("作业安全证动土作业_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        /// <summary>
        /// 导出断路作业
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportDataForOpenCircuit(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/作业安全证断路作业.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("jobdeptname"); //作业单位
            dt.Columns.Add("jobtime"); //作业时间
            dt.Columns.Add("jobcontent"); //作业内容
            dt.Columns.Add("pic");//示意图
            dt.Columns.Add("safetymeasures"); //安全措施
            dt.Columns.Add("signurl");//安全措施签名
            dt.Columns.Add("approveidea1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveperson1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approvedate1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("recheckidea"); //中心（部门）安全管理组备案
            dt.Columns.Add("recheckperson"); //中心（部门）安全管理组备案
            dt.Columns.Add("recheckdate"); //中心（部门）安全管理组备案
            DataRow row = dt.NewRow();

            JobSafetyCardApplyEntity entity = jobSafetyCardApplybll.GetEntity(keyValue);
            row["jobdeptname"] = entity.JobDeptName;
            row["jobtime"] = "自" + entity.JobStartTime.Value.ToString("yyyy年MM月dd日HH时mm分") + "至" + entity.JobEndTime.Value.ToString("yyyy年MM月dd日HH时mm分");
            row["jobcontent"] = entity.JobContent;
            row["safetymeasures"] = entity.SafetyMeasures;
            if (string.IsNullOrWhiteSpace(entity.SignUrl))
            {
                row["signurl"] = Server.MapPath("~/content/Images/no_1.png");
            }
            else
            {
                var filepath = Server.MapPath("~/") + entity.SignUrl.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    row["signurl"] = filepath;
                }
                else
                {
                    row["signurl"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }

            //审核记录
            var approvedata = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.ApplyNumber == entity.ApplyNumber && t.Status == 1).ToList();
            if (approvedata.Count > 0)
            {
                row["approveidea1"] = !string.IsNullOrEmpty(approvedata[0].ApproveOpinion) ? approvedata[0].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[0].SignUrl))
                {
                    row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[0].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson1"] = filepath;
                    }
                    else
                    {
                        row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate1"] = approvedata[0].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            //验收信息
            var recheckdata = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 0).ToList();
            if (recheckdata.Count > 0)
            {
                row["recheckidea"] = !string.IsNullOrEmpty(recheckdata[0].OperateOpinion) ? recheckdata[0].OperateOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(recheckdata[0].SignImg))
                {
                    row["recheckperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + recheckdata[0].SignImg.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["recheckperson"] = filepath;
                    }
                    else
                    {
                        row["recheckperson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["recheckdate"] = recheckdata[0].OperateTime.Value.ToString("yyyy年MM月dd日");
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            //安全措施
            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sort");
            dt1.Columns.Add("content");
            dt1.Columns.Add("result");
            var measure = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == "OpenCircuit" && t.ConfigType == "0" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
            if (measure != null)
            {
                var measuredata = safetymeasuredetailbll.GetList("").Where(t => t.RecId == measure.Id).OrderBy(t => t.SortNum).ToList();
                for (int i = 0; i < measuredata.Count(); i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sort"] = measuredata[i].SortNum;
                    if (i == measuredata.Count - 1)
                    {
                        row1["content"] = measuredata[i].Content + entity.MeasureContent;
                    }
                    else
                    {
                        row1["content"] = measuredata[i].Content;
                    }
                    if (("/" + entity.SafetyMeasures + "/").IndexOf("/" + measuredata[i].SortNum + "/") >= 0)
                    {
                        row1["result"] = "√";
                    }
                    dt1.Rows.Add(row1);
                }
            }
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("作业安全证断路作业_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }


        /// <summary>
        /// 盲板抽堵作业安全证及安全措施导出
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportDataForBlindPlateWall(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/盲板抽堵作业安全证导出模板.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("applyno"); //编号
            dt.Columns.Add("applytime"); //申请时间 
            dt.Columns.Add("createuser"); //编制人 
            dt.Columns.Add("createtime"); //编制时间 
            dt.Columns.Add("jobticketno");  //生产工序
            dt.Columns.Add("applydept"); //申请单位
            dt.Columns.Add("applyperson"); //申请人
            dt.Columns.Add("jobtime"); //作业时间
            dt.Columns.Add("jobplace"); //作业地点
            dt.Columns.Add("dutyperson"); //作业负责人
            dt.Columns.Add("jobdeptname");
            dt.Columns.Add("signurl");
            dt.Columns.Add("jobcontent"); //作业内容
            dt.Columns.Add("safetymeasures"); //安全措施           
            dt.Columns.Add("approveidea1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveperson1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approvedate1"); //中心（部门）设备管理负责人审核
            dt.Columns.Add("approveidea2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approveperson2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("approvedate2"); //中心（部门）生产管理负责人审核
            dt.Columns.Add("copyidea"); //中心（部门）安全管理组备案
            dt.Columns.Add("copyperson"); //中心（部门）安全管理组备案
            dt.Columns.Add("copydate"); //中心（部门）安全管理组备案
            DataRow row = dt.NewRow();

            JobSafetyCardApplyEntity entity = jobSafetyCardApplybll.GetEntity(keyValue);
            row["applyno"] = entity.ApplyNo;
            row["jobticketno"] = entity.JobTicketNo;
            row["applydept"] = entity.ApplyDeptName;
            row["applyperson"] = entity.ApplyUserName;
            row["jobtime"] = "自" + entity.JobStartTime.Value.ToString("yyyy年MM月dd日HH时mm分") + "至" + entity.JobEndTime.Value.ToString("yyyy年MM月dd日HH时mm分");
            row["jobplace"] = entity.JobPlace;
            row["jobcontent"] = entity.JobContent;
            row["safetymeasures"] = entity.SafetyMeasures;
            row["dutyperson"] = entity.DutyPerson;
            row["jobdeptname"] = entity.JobDeptName;
            row["createuser"] = entity.CreateUserName;
            row["createtime"] = entity.CreateDate.Value.ToString("yyyy年MM月dd日"); ;
            if (!string.IsNullOrEmpty(entity.SignUrl))
            {
                var signurl = Server.MapPath("~/") + entity.SignUrl.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(signurl))
                {
                    row["signurl"] = signurl;
                }
                else
                {
                    row["signurl"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }


            var fileData = new FileInfoBLL().GetFiles(entity.Id + "_02");
            if (fileData != null)
            {
                foreach (DataRow item in fileData.Rows)
                {
                    dt.Columns.Add("imageurl");//盲板位置图

                    var filepath = Server.MapPath("~/") + item["filepath"].ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["imageurl"] = filepath;
                    }
                    else
                    {
                        row["imageurl"] = Server.MapPath("~/content/Images/no_1.png");
                    }

                }

            }
            


            // 盲板
            BlindPlateWallSpecBLL blindplatewallspecbll = new BlindPlateWallSpecBLL();
            var data = blindplatewallspecbll.GetList("");
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                data = data.Where(t => t.RecId == keyValue);
                if (data != null)
                {
                    DataTable dtc = new DataTable("C");
                    dtc.Columns.Add("equpipelinename");//设备管道名称        
                    dtc.Columns.Add("media");//介质
                    dtc.Columns.Add("temperature");//温度
                    dtc.Columns.Add("pressure");//压力
                    dtc.Columns.Add("quality");//材质
                    dtc.Columns.Add("standard");//规格
                    dtc.Columns.Add("serialnumber");//编号
                    dtc.Columns.Add("jobstarttime");
                    dtc.Columns.Add("jobendtime");  
                    dtc.Columns.Add("jobperson");  
                    dtc.Columns.Add("jobperson2");  
                    dtc.Columns.Add("custodian");
                    dtc.Columns.Add("custodian2");  
                    foreach (BlindPlateWallSpecEntity item in data)
                    {
                        DataRow row1 = dtc.NewRow();
                        row1["equpipelinename"] = entity.EquPipelineName;
                        row1["media"] = entity.Media;
                        row1["temperature"] = entity.Temperature;
                        row1["pressure"] = entity.Pressure;
                        row1["quality"] = item.Quality;
                        row1["standard"] = item.Standard;
                        row1["serialnumber"] = item.SerialNumber;
                        row1["jobstarttime"] = entity.JobStartTime;
                        row1["jobendtime"] = entity.JobEndTime;
                        row1["jobperson"] = entity.JobPerson;
                        row1["jobperson2"] = entity.JobPerson2;
                        row1["custodian"] = entity.Custodian;
                        row1["custodian2"] = entity.Custodian2;
                      
                        dtc.Rows.Add(row1);
                    }
                    doc.MailMerge.ExecuteWithRegions(dtc);
                }
            }




            //审核记录
            var approvedata = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.ApplyNumber == entity.ApplyNumber && t.Status == 1).ToList();
            if (approvedata.Count > 0)
            {
                row["approveidea1"] = !string.IsNullOrEmpty(approvedata[0].ApproveOpinion) ? approvedata[0].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[0].SignUrl))
                {
                    row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[0].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson1"] = filepath;
                    }
                    else
                    {
                        row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate1"] = approvedata[0].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            if (approvedata.Count > 1)
            {
                row["approveidea2"] = !string.IsNullOrEmpty(approvedata[1].ApproveOpinion) ? approvedata[1].ApproveOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(approvedata[1].SignUrl))
                {
                    row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + approvedata[1].SignUrl.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson2"] = filepath;
                    }
                    else
                    {
                        row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["approvedate2"] = approvedata[1].ApproveTime.Value.ToString("yyyy年MM月dd日");
            }
            //备案信息
            var copydata = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 1).ToList();
            if (copydata.Count > 0)
            {
                row["copyidea"] = !string.IsNullOrEmpty(copydata[0].OperateOpinion) ? copydata[0].OperateOpinion.ToString() : "";
                if (string.IsNullOrWhiteSpace(copydata[0].SignImg))
                {
                    row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + copydata[0].SignImg.ToString().Replace("../../", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["copyperson"] = filepath;
                    }
                    else
                    {
                        row["copyperson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                row["copydate"] = copydata[0].OperateTime.Value.ToString("yyyy年MM月dd日");
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
 
            //安全措施
            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sort");
            dt1.Columns.Add("content");
            dt1.Columns.Add("result");
            var measure = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == "BlindPlateWall" && t.ConfigType == "0" && t.DeptCode == user.OrganizeCode).FirstOrDefault();
            if (measure != null)
            {
                var measuredata = safetymeasuredetailbll.GetList("").Where(t => t.RecId == measure.Id).OrderBy(t => t.SortNum).ToList();
                for (int i = 0; i < measuredata.Count(); i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sort"] = measuredata[i].SortNum;
                    if (i == measuredata.Count - 1)
                    {
                        row1["content"] = measuredata[i].Content + entity.MeasureContent;
                    }
                    else
                    {
                        row1["content"] = measuredata[i].Content;
                    }
                    if (("/" + entity.SafetyMeasures + "/").IndexOf("/" + measuredata[i].SortNum + "/") >= 0)
                    {
                        row1["result"] = "√";
                    }
                    dt1.Rows.Add(row1);
                }
            }
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("作业安全证盲板抽堵作业_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }


        #endregion
    }
}