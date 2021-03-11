using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.TrainPlan;
using ERCHTMS.Code;
using ERCHTMS.Entity.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    public class SafeTrainPlanController : MvcControllerBase
    {
        private SafeTrainPlanBLL safeTrainPlanBLL = new SafeTrainPlanBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private UserBLL userBLL = new UserBLL();

        #region [视图功能]
        // GET: RoutineSafetyWork/SafeTrainPlan
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Import()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region [获取数据]
        /// <summary>
        /// 获取安全培训计划列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";
                //根据后台配置查看数据权限
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "createuserorgcode");
                //if (!string.IsNullOrEmpty(where) && (queryParam["code"].IsEmpty() || !queryJson.Contains("code")))
                //{
                //    pagination.conditionJson += " and " + where;
                //}
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
                DataTable dt = safeTrainPlanBLL.GetList(pagination, queryJson);

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
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safeTrainPlanBLL.GetEntity(keyValue);
            return ToJsonResult(data);

        }
        #endregion

        #region [保存数据]
        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public string ImportTrainPlan()
        {
            var user = OperatorProvider.Provider.Current();
            if (user.IsSystem)
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
                    List<SafeTrainPlanEntity> list = new List<SafeTrainPlanEntity>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        order = i;
                        //培训项目
                        string projectName = dt.Rows[i]["培训项目"].ToString().Trim();
                        //培训内容
                        string content = dt.Rows[i]["培训内容"].ToString().Trim();
                        //培训对象
                        string participants = dt.Rows[i]["培训对象"].ToString().Trim();
                        //培训时间
                        string trainDate = dt.Rows[i]["培训时间"].ToString().Trim();
                        //责任部门
                        string dutyDeptName = dt.Rows[i]["责任部门"].ToString().Trim();
                        //责任人
                        string dutyUserName = dt.Rows[i]["责任人"].ToString().Trim();
                        //执行人/监督人
                        string executeUserName = dt.Rows[i]["执行人/监督人"].ToString().Trim();
                        //---****值存在空验证*****--
                        if (string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(participants) || string.IsNullOrEmpty(trainDate)
                            || string.IsNullOrEmpty(dutyDeptName) || string.IsNullOrEmpty(dutyUserName) || string.IsNullOrEmpty(executeUserName))
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行值存在空,未能导入.";
                            error++;
                            continue;
                        }

                        //验证所填部门是否存在 电厂整个组织机构
                        var entity = departmentBLL.GetList()
                           .Where(t => t.EnCode.StartsWith(currUser.OrganizeCode) && t.EnCode != currUser.OrganizeCode && t.FullName == dutyDeptName )
                           .FirstOrDefault();
                        string deptCode = string.Empty, deptId = string.Empty;
                        if (entity == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行部门信息不存在,未能导入.";
                            error++;
                            continue;
                        }
                        else
                        {
                            deptId = entity.DepartmentId;
                            deptCode = entity.EnCode;
                        }
                        string dutyUserId = string.Empty;
                        string executeUserId = string.Empty;
                        //验证责任人是否存在责任部门下
                        var uentity = userBLL.GetList().Where(t => t.RealName.Equals(dutyUserName) && t.DepartmentId== entity.DepartmentId).FirstOrDefault();
                        if (uentity == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行责任人不存在,未能导入.";
                            error++;
                            continue;
                        }
                        else
                        {
                            dutyUserId = uentity.UserId;
                        }
                        //验证执行人/监督人是否存在责任部门下
                        uentity = userBLL.GetList().Where(t => t.RealName.Equals(executeUserName) && t.DepartmentId == entity.DepartmentId).FirstOrDefault();
                        if (uentity == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行执行人/监督人不存在,未能导入.";
                            error++;
                            continue;
                        }
                        else
                        {
                            executeUserId= uentity.UserId;
                        }

                        //验证日期格式
                        DateTime? trainDate1=null;
                        try
                        {
                            //yyyy-MM或yyyy.MM
                            trainDate = trainDate.Replace(".", "-");
                            if (trainDate.Split('-').Length != 2 || trainDate.Split('-')[0].Length!=4)
                            {
                                falseMessage += "</br>" + "第" + (i + 1) + "行培训时间格式不正确,未能导入.";
                                error++;
                                continue;
                            }
                            trainDate1 = Convert.ToDateTime(trainDate);
                        }
                        catch (Exception ex)
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行培训时间格式不正确,未能导入.";
                            error++;
                            continue;
                        }
                        
                        SafeTrainPlanEntity safeTrainPlan = new SafeTrainPlanEntity()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProjectName = projectName,
                            TrainDate = trainDate1,
                            TrainContent = content,
                            Participants = participants,
                            DepartmentId = deptId,
                            DepartmentName = dutyDeptName,
                            DepartmentCode = deptCode,
                            DutyUserId = dutyUserId,
                            DutyUserName = dutyUserName,
                            ExecuteUserId = executeUserId,
                            ExecuteUserName=executeUserName,
                            ProcessState=0
                        };
                        //数据查重 根据培训项目、培训内容、培训对象、培训时间以及责任部门
                        bool flag = safeTrainPlanBLL.CheckDataExists(safeTrainPlan);
                        if (flag)
                        {
                            //数据重复
                            falseMessage += "</br>" + "第" + (i + 1) + "行数据已存在,未能导入.";
                            error++;
                        }
                        else
                        {
                            safeTrainPlan.Create();
                            list.Add(safeTrainPlan);
                        }
                    }
                    if (list.Count() > 0)
                    {
                        //保存
                        safeTrainPlanBLL.InsertSafeTrainPlan(list);
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
        /// 下发
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IssueData(string userId)
        {
            try
            {
                //判断是否存在待下发数据
                bool flag= safeTrainPlanBLL.CheckUnpublishPlan(userId);
                if (flag)
                {
                    safeTrainPlanBLL.IssueData(userId);
                }
                else {
                    return Content("不存在待下发数据。");
                }
                return Content("下发成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveForm(string keyValue,string mode, SafeTrainPlanEntity entity)
        {
            try
            {
                if (mode.Contains(mode) && mode.Equals("feedback") )
                {
                    entity.ProcessState = 2;
                }
                safeTrainPlanBLL.SaveForm(keyValue, entity);
                return Success("保存成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Remove(string keyValue)
        {
            try
            {
                safeTrainPlanBLL.Remove(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
    }
}