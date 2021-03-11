using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.TrainPlan;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    public class SafeMeasureController : MvcControllerBase
    {
        private SafeMeasureBLL safeMeasureBLL = new SafeMeasureBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private SafeAdjustmentBLL safeAdjustmentBLL = new SafeAdjustmentBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditBLL = new AptitudeinvestigateauditBLL();


        #region [视图功能]
        // GET: RoutineSafetyWork/SafeMeasure
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 导入安措计划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Import()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 审核流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }

        /// <summary>
        /// 调整申请
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApproveRecord()
        {
            return View();
        }

        /// <summary>
        /// 安措计划详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }

        #endregion

        #region [获取数据]
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.conditionJson = " 1=1 ";
                var queryParam = queryJson.ToJObject();
                //根据后台配置查看数据权限
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "deptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
                var watch = CommonHelper.TimerStart();
                DataTable dt = safeMeasureBLL.GetList(pagination, queryJson);
                //获取下一步审核人
                foreach (DataRow row in dt.Rows)
                {
                    //获取下一步审核人
                    string userNames = string.Empty;
                    row["approveuserids"] = safeMeasureBLL.GetNextStepUser(row["flowrolename"].ToString(), row["flowdept"].ToString(), out userNames);
                    string stauts = row["Stauts"].ToString();
                    if (!stauts.Equals("无调整") && !stauts.Equals("审批通过") && !stauts.Equals("审批不通过"))
                    {
                        if (userNames.Contains(","))
                        {
                            row["Stauts"] = stauts + userNames.Split(',')[0] + "...审批中";
                        }
                        else
                        {
                            row["Stauts"] = stauts + userNames + "审批中";

                        }
                        row["approveusernames"] = stauts + userNames + "审批中";
                    }

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
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue,string mode,string adjustId)
        {
            //按错计划调整申请
            var dataContent = new
            {
                //安措计划
                plan = safeMeasureBLL.GetEntity(keyValue),
                //调整申请
                apply = safeAdjustmentBLL.GetAdjustInfo(keyValue,mode, adjustId),
                feedback = safeMeasureBLL.GetFeedbackInfo(keyValue)
            };
            return Content(dataContent.ToJson());
        }

        /// <summary>
        /// 表单审批信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetApproveList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string keyValue = queryParam["keyValue"].ToString();
            string adjustId = queryParam["adjustId"].ToString();
            var data = aptitudeinvestigateauditBLL.GetAuditList(keyValue).Where(t=>t.APPLYID.Equals(adjustId)).ToList();
            var JsonData = new
            {
                rows = data,
                total = data.Count(),
                page = pagination.page,
                records = data.Count()
            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSafeMeasureFlowData(string keyValue,string adjustId)
        {
            var data = safeMeasureBLL.GetFlow("安措计划调整审批", keyValue, adjustId);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取审批记录
        /// </summary>
        /// <param name="measureId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetApproveListJson(Pagination pagination, string measureId)
        {
            DataTable dt = safeAdjustmentBLL.GetAdjustList(measureId);
            string stauts = string.Empty;
            string str = string.Empty;
            string userNames = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                stauts=row["Stauts"].ToString();
                if (!stauts.Equals("无调整") && !stauts.Equals("审批通过") && !stauts.Equals("审批不通过"))
                {
                    str = safeMeasureBLL.GetNextStepUser(row["flowrolename"].ToString(), row["flowdept"].ToString(), out userNames);
                    if (userNames.Contains(","))
                    {
                        row["Stauts"] = stauts+ userNames.Split(',')[0] + "...审批中";
                    }
                    else {
                        row["Stauts"] = stauts+userNames + "审批中";
                    }
                    row["approveusernames"] = stauts+ userNames + "审批中";
                }
            }
            var JsonData = new
            {
                rows = dt,
                total = dt.Rows.Count,
                page = pagination.page,
                records = dt.Rows.Count
            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 获取安措计划季度数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="beyongYear"></param>
        /// <param name="quarter"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSafeMeasureData(Pagination pagination, string queryJson)
        {
            try
            {
                var data = safeMeasureBLL.GetSafeMeasureData(queryJson);
                string stauts = string.Empty;
                string str = string.Empty;
                string userNames = string.Empty;
                foreach (DataRow row in data.Rows)
                {
                   string approveuserids = safeMeasureBLL.GetNextStepUser(row["flowrolename"].ToString(), row["flowdept"].ToString(), out userNames);
                    stauts = row["Stauts"].ToString();
                    if (!stauts.Equals("无调整") && !stauts.Equals("审批通过") && !stauts.Equals("审批不通过"))
                    {
                        str = safeMeasureBLL.GetNextStepUser(row["flowrolename"].ToString(), row["flowdept"].ToString(), out userNames);
                        if (userNames.Contains(","))
                        {
                            row["Stauts"] = stauts + userNames.Split(',')[0] + "...审批中";
                        }
                        else
                        {
                            row["Stauts"] = stauts + userNames + "审批中";
                        }
                        row["approveusernames"] = stauts + userNames + "审批中";
                    }
                }
                var JsonData = new
                {
                    rows = data,
                    total = data.Rows.Count,
                    page = pagination.page,
                    records = data.Rows.Count
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Error("获取列表失败!");
            }
            
            
        }
        #endregion

        #region [提交数据]
        /// <summary>
        /// 导入安措计划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public string ImportSafeMeasure()
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            string message = "请选择格式正确的文件再导入!";
            try
            {
                int error = 0;
                
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                var currUser = OperatorProvider.Provider.Current();
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
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    if (cells.MaxDataRow <= 1)
                    {
                        return "共有0条记录,成功导入0条";
                    }
                    DataTable dt = cells.ExportDataTable(1, 1, cells.MaxDataRow, cells.MaxColumn + 1, true);
                    int order = 1;
                    List<SafeMeasureEntity> list = new List<SafeMeasureEntity>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        order = i;
                        //类别
                        string type = dt.Rows[i]["类别"].ToString().Trim();
                        //项目
                        string projetName = dt.Rows[i]["项目"].ToString().Trim();
                        //责任部门
                        string deptName = dt.Rows[i]["责任部门"].ToString().Trim();
                        //计划费用
                        string cost = dt.Rows[i]["计划费用(万)"].ToString().Trim();
                        //计划完成时间
                        string planFinishTime = dt.Rows[i]["计划完成时间"].ToString().Trim();
                        //部门验收人
                        string checkUser = dt.Rows[i]["部门验收人"].ToString().Trim();
                        //---****值存在空验证*****--
                        if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(projetName) || string.IsNullOrEmpty(deptName) || string.IsNullOrEmpty(cost) || string.IsNullOrEmpty(planFinishTime) || string.IsNullOrEmpty(checkUser))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                            error++;
                            continue;
                        }
                        //验证所填部门是否存在
                        var entity = departmentBLL.GetList()
                            .Where(t => t.EnCode.StartsWith(currUser.OrganizeCode) && t.EnCode != currUser.OrganizeCode
                            && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商" || t.Nature == "班组" || t.Nature == "专业") && t.FullName == deptName)
                            .FirstOrDefault();
                        //var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == deptName).FirstOrDefault();

                        string deptCode = string.Empty, deptId = string.Empty;
                        if (entity == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行部门信息不存在,未能导入.";
                            error++;
                            continue;
                        }
                        else
                        {
                            deptId = entity.DepartmentId;
                            deptCode = entity.EnCode;
                        }
                        //验证小数
                        if (!string.IsNullOrEmpty(cost))
                        {
                            if (!Regex.IsMatch(cost, @"(^[1-9](\d+)?(\.\d{1,9})?$)|(^-?0$)|(^\d\.\d{1,9}$)", RegexOptions.IgnoreCase))
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行计划费用格式有误,未能导入.";
                                error++;
                                continue;
                            }
                        }
                        //保留六位小数
                        string cost1 = Convert.ToDouble(cost).ToString("f6");

                        //验证日期格式
                        DateTime? planFinishTime1 = null;
                        try
                        {
                            planFinishTime = planFinishTime.Replace('.', '-');
                            if (planFinishTime.Split('-').Length != 3)
                            {
                                falseMessage += "</br>" + "第" + (i + 1) + "行计划完成时间格式不正确,未能导入.";
                                error++;
                                continue;
                            }
                            planFinishTime1 = Convert.ToDateTime(planFinishTime);
                        }
                        catch (Exception ex)
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行计划完成时间格式不正确,未能导入.";
                            error++;
                            continue;
                        }
                        //根据 类别、项目、责任部门、计划完成时间 查重
                        SafeMeasureEntity safe = new SafeMeasureEntity()
                        {
                            Id = Guid.NewGuid().ToString(),
                            PlanType = type,
                            ProjectName = projetName,
                            IsOver = 0,
                            DeptCode = deptCode,
                            DepartmentId = deptId,
                            DepartmentName = deptName,
                            Cost = Convert.ToDouble(cost1),
                            PlanFinishDate = planFinishTime1,
                            CheckUserName = checkUser,
                            Stauts = "无调整",
                            ProcessState = 0,
                            PublishState=0
                        };
                        //判断是否存在
                        bool flag = safeMeasureBLL.ExistSafeMeasure(safe);
                        if (!flag)
                        {
                            //数据重复
                            falseMessage += "</br>" + "第" + (i + 2) + "行数据已存在,未能导入.";
                            error++;
                        }
                        else
                        {
                            safe.Create();
                            list.Add(safe);
                        }
                    }
                    if (list.Count() > 0)
                    {
                        //保存
                        safeMeasureBLL.SaveForm(list);
                    }
                    count = dt.Rows.Count;
                    message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                    message += "</br>" + falseMessage;
                }
            }
            catch (Exception ex)
            {
                return "导入失败！";
            }
            return message;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="safeData"></param>
        /// <returns></returns>
        public ActionResult SaveForm(string keyValue, string safeData)
        {
            SafeMeasureEntity entity = safeData.ToObject<SafeMeasureEntity>();
            entity.Id = keyValue;
            bool flag = safeMeasureBLL.ExistSafeMeasure(entity);
            if (flag)
            {
                safeMeasureBLL.UpdateSafeMeasure(entity);
            }
            return Success("保存成功。");
        }

        /// <summary>
        /// 删除安措计划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue,string iscommit)
        {
            try
            {
                safeMeasureBLL.RemoveForm(keyValue, iscommit);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 批量下发
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IssueData(string userId)
        {
            try
            {
                bool flag = safeMeasureBLL.CheckUnPublish(userId);
                if (!flag)
                {
                    return Content("不存在待下发数据。");
                }
                else
                {
                    safeMeasureBLL.IssueData(userId);
                    return Content("下发成功。");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 安措计划调整申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string safeData)
        {
            try
            {
                var adjustmentEntity = safeData.ToObject<SafeAdjustmentEntity>();
                safeAdjustmentBLL.SubmitForm(adjustmentEntity);
                return Success("操作成功!");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 审核信息
        /// </summary>
        /// <param name="aentity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApproveForm(string keyValue, string applyId, AptitudeinvestigateauditEntity aentity)
        {
            try
            {
                bool flag = safeAdjustmentBLL.ApproveForm(keyValue, applyId, aentity);
                if (flag)
                {
                    return Success("操作成功!");
                }
                else
                {
                    return Success("没有操作权限!");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }


        }
        #endregion
    }
}