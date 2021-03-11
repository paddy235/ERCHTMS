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
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.PublicInfoManage;
using Aspose.Words.Tables;
using System.Drawing;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：高风险通用作业申请
    /// </summary>
    public class HighRiskCommonApplyController : MvcControllerBase
    {
        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        private HighImportTypeBLL highimporttypebll = new HighImportTypeBLL();
        private ScaffoldprojectBLL scaffoldprojectbll = new ScaffoldprojectBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private ScaffoldauditrecordBLL scaffoldauditrecordbll = new ScaffoldauditrecordBLL();
        private HighRiskRecordBLL highriskrecordbll = new HighRiskRecordBLL();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        private HighRiskApplyMBXXBLL applymbxxbll = new HighRiskApplyMBXXBLL();

        private DataItemCache dataItemCache = new DataItemCache();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();

        private TransferrecordBLL transferrecordbll = new TransferrecordBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();

        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckForm()
        {
            return View();
        }

        /// <summary>
        /// 流程图页面
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
        /// 台账页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LedgerSetting()
        {
            return View();
        }

        /// <summary>
        /// 选择工作任务页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }

        /// <summary>
        /// 作业安全分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AnalyseForm()
        {
            return View();
        }

        /// <summary>
        /// 转交页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TransferForm()
        {
            return View();
        }

        /// <summary>
        /// 盲板信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectMB()
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
            var watch = CommonHelper.TimerStart();
            var data = highriskcommonapplybll.GetPageList(pagination, queryJson);
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
            var data = highriskcommonapplybll.GetList(queryJson);
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
            var data = highriskcommonapplybll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageTableJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "a.Id";
                string isJDZ = new DataItemDetailBLL().GetItemValue("景德镇版本");
                string str = "";
                if (!string.IsNullOrEmpty(isJDZ))
                {
                    str = "(CASE WHEN WORKTYPE='12' THEN (select itemid from base_dataitem where itemcode='CommonWorkType') ELSE (select itemid from base_dataitem where itemcode='CommonRiskType') END)";
                }
                else {
                    str = "(select itemid from base_dataitem where itemcode='CommonRiskType')";
                }
                pagination.p_fields = @"a.FlowDeptName,a.workdeptid,a.engineeringid,a.flowname,a.flowrolename,a.flowdept,a.investigatestate,a.workdepttype,a.workdeptname,a.applynumber,a.CreateDate,a.applystate,a.workdutyuserid,a.worktutelageuserid,
                                 c.itemname as applystatename,b.itemname as worktype,workplace,workcontent,workstarttime,workendtime,applyusername,applydeptname,a.createuserid,a.createuserdeptcode,a.createuserorgcode,d.itemname as risktype,
                                a.flowremark,a.specialtytype,a.flowid,a.nextstepapproveuseraccount,'' as approveuseraccount,a.approvedeptid,e.outtransferuseraccount,e.intransferuseraccount,
                                case when a.workoperate='1' then '作业暂停' when realityworkstarttime is not null and realityworkendtime is null then '作业中' when realityworkendtime is not null then '已结束'  else '即将作业' end ledgertype,a.RealityWorkStartTime,a.RealityWorkEndTime,'' as isoperate";
                pagination.p_tablename =string.Format(@" bis_highriskcommonapply a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='CommonType') left join base_dataitemdetail c on a.applystate=c.itemvalue and c.itemid =(select itemid from base_dataitem where itemcode='CommonStatus') 
                                            left join base_dataitemdetail d on a.risktype=d.itemvalue and d.itemid ={0}
                                            left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and a.flowid=e.flowid and e.num=1",str);
                pagination.conditionJson = "1=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    
                    string isHDGZ = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("贵州毕节版本");
                    string isAllDataRange = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetEnableItemValue("HighRiskWorkDataRange");
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || user.RoleName.Contains("值班管理员") || !string.IsNullOrEmpty(isHDGZ) || !string.IsNullOrEmpty(isAllDataRange) || !string.IsNullOrEmpty(isJDZ))
                    {
                        pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        var deptentity = departmentbll.GetEntity(user.DeptId);
                        while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                        {
                            deptentity = departmentbll.GetEntity(deptentity.ParentId);
                        }
                        pagination.conditionJson += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid in (select departmentid from base_department where encode like '{1}%'))))", user.DeptCode, deptentity.EnCode);
                    }
                }
                DataTable data = highriskcommonapplybll.GetPageDataTable(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string executedept = string.Empty;
                    highriskcommonapplybll.GetExecutedept(data.Rows[i]["workdepttype"].ToString(), data.Rows[i]["workdeptid"].ToString(), data.Rows[i]["engineeringid"].ToString(), out executedept);
                    string createdetpid = departmentbll.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                    string outsouringengineerdept = string.Empty;
                    highriskcommonapplybll.GetOutsouringengineerDept(data.Rows[i]["workdeptid"].ToString(), out outsouringengineerdept);
                    //获取下一步审核人
                    str = manypowercheckbll.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), data.Rows[i]["nextstepapproveuseraccount"].ToString(), data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", data.Rows[i]["approvedeptid"].ToString());
                    data.Rows[i]["approveuseraccount"] = str;
                }
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
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }

        /// <summary>
        /// 获取安全措施的显示模式[手动输入或带入配置数据]
        /// </summary>
        /// <param name="showtype"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetShowPattern(string showtype)
        {
            var curorgcode = ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode;

            string str = "1";//默认为手动输入
            var data = highimporttypebll.GetList(string.Format(" and itype='{0}' and CreateUserOrgCode='{1}'", showtype, curorgcode)).FirstOrDefault();

            if (data != null)
                str = data.IsImport;

            return str;
        }


        /// <summary>
        /// 获取审查记录集合 
        /// </summary>
        /// <param name="scaffoldid">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetMeasureListJson(string scaffoldid)
        {
            var data = scaffoldprojectbll.GetListByCondition(string.Format(" and ScaffoldId='{0}'", scaffoldid)).OrderBy(t => t.CreateDate).ToList();
            if (data.Count > 0)
            {
                return ToJsonResult(data);
            }
            else
            {
                return ToJsonResult(new { });
            }
        }

        /// <summary>
        /// 获取盲板信息
        /// </summary>
        /// <param name="highRiskId">高风险通用作业申请id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetMBListToJson(string highRiskId)
        {
            var data = applymbxxbll.GetList(highRiskId);
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
        [HandlerMonitor(6, "通用高风险作业删除")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            highriskcommonapplybll.RemoveForm(keyValue);
            return Success("删除成功。");
        }



        /// <summary>
        /// 确认中的状态撤销为待提交
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(7, "通用高风险作业撤销")]
        [AjaxOnly]
        public ActionResult UpdateForm(string keyValue)
        {
            try
            {
                var entity = highriskcommonapplybll.GetEntity(keyValue);
                if (entity != null)
                {
                    entity.ApplyState = "0";
                    entity.InvestigateState = "0";
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowId = "";
                    entity.FlowName = "";
                    entity.FlowRemark = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.ApproveAccount = "";
                    entity.FlowApplyType = "";
                    entity.NextStepApproveUserAccount = "";
                    highriskcommonapplybll.UpdateForm(entity);
                    scaffoldprojectbll.RemoveForm(t => t.ScaffoldId == keyValue);
                    var transferlist = transferrecordbll.GetList(t => t.RecId == keyValue && t.Disable == 0);
                    foreach (var item in transferlist)
                    {
                        item.Disable = 1;
                        transferrecordbll.SaveForm(item.Id, item);
                    }
                    return Success("撤销成功。");
                }
                else
                {
                    return Error("数据信息有误,撤销失败。");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "通用高风险作业保存")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string type, HighRiskCommonApplyEntity entity, string RiskRecord, string MBXXRecord)
        {
            try
            {
                List<HighRiskRecordEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HighRiskRecordEntity>>(RiskRecord);
                HighRiskCommonApplyEntity centity = highriskcommonapplybll.GetEntity(keyValue);
                if (centity != null)
                {
                    entity.CreateUserDeptCode = centity.CreateUserDeptCode;
                }
                string isJdz = new DataItemDetailBLL().GetItemValue("景德镇版本");
                List<HighRiskApplyMBXXEntity> mbList = new List<HighRiskApplyMBXXEntity>();
                if (!string.IsNullOrEmpty(isJdz) &&entity.WorkType == "11")
                {
                    //盲板抽堵作业
                    mbList = JsonConvert.DeserializeObject<List<HighRiskApplyMBXXEntity>>(MBXXRecord);
                }
                highriskcommonapplybll.SaveForm(keyValue, type, entity, list, mbList);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }

        /// <summary>
        /// 转交提交
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "转交保存")]
        [AjaxOnly]
        public ActionResult SaveTransferForm(string keyValue, TransferrecordEntity entity)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                entity.OutTransferUserAccount = user.Account;
                entity.OutTransferUserId = user.UserId;
                entity.OutTransferUserName = user.UserName;
                entity.ModuleId = HttpContext.Request.Cookies["currentmoduleId"].Value;
                transferrecordbll.SaveRealForm(keyValue, entity);
                //转交短消息
                highriskcommonapplybll.TransformSendMessage(entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        #endregion

        #region 保存措施内容
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveMeasureForm(string keyValue, string recordData, HighRiskCommonApplyEntity entity)
        {
            JArray arr = (JArray)JsonConvert.DeserializeObject(recordData);
            string isJdz = new DataItemDetailBLL().GetItemValue("景德镇版本");
            if (!string.IsNullOrEmpty(isJdz))
            {
                //更新
                highriskcommonapplybll.UpdateData(string.Format("update BIS_HIGHRISKCOMMONAPPLY set RISKIDENTIFICATION='{0}' where ID='{1}'", entity.RiskIdentification, keyValue));
            }
            //只更新安全措施
            for (int i = 0; i < arr.Count(); i++)
            {
                string id = arr[i]["id"].ToString();  //主键
                string result = arr[i]["result"].ToString(); //结果
                string people = arr[i]["people"].ToString(); //选择的人员
                string peopleid = arr[i]["peopleid"].ToString(); //选择的人员
                string signpic = string.IsNullOrWhiteSpace(arr[i]["signpic"].ToString()) ? "" : arr[i]["signpic"].ToString().Replace("../..", "");//签名
                string projectname = arr[i]["projectname"].ToString();//安全措施
                if (!string.IsNullOrEmpty(id))
                {
                    var scEntity = scaffoldprojectbll.GetEntity(id);
                    scEntity.Result = result;//确认结果
                    scEntity.CheckPersons = people;//确认人
                    scEntity.CheckPersonsId = peopleid;
                    scEntity.SignPic = signpic;//签名
                    if (GetShowPattern("1") == "1")
                    {
                        scEntity.ProjectName = projectname;
                    }
                    scaffoldprojectbll.SaveForm(id, scEntity);
                }
            }

            return Success("操作成功。");
        }
        #endregion


        #region 确认、审核
        /// <summary>
        /// 确认、审核
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(8, "通用高风险作业提交")]
        [AjaxOnly]
        public ActionResult SubmitCheckForm(string keyValue, string state, string recordData, HighRiskCommonApplyEntity entity, ScaffoldauditrecordEntity aentity)
        {
            try
            {
                string isJdz = new DataItemDetailBLL().GetItemValue("景德镇版本");
                if (!string.IsNullOrEmpty(isJdz))
                {
                    //更新
                    highriskcommonapplybll.UpdateData(string.Format("update BIS_HIGHRISKCOMMONAPPLY set RISKIDENTIFICATION='{0}' where ID='{1}'", entity.RiskIdentification, keyValue));
                }
                highriskcommonapplybll.SubmitCheckForm(keyValue, state, recordData, entity, aentity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }
        #endregion

        #region 导出
        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                string isJDZ = new DataItemDetailBLL().GetItemValue("景德镇版本");
                string str = "";
                if (!string.IsNullOrEmpty(isJDZ))
                {
                    str = "(CASE WHEN WORKTYPE='12' THEN (select itemid from base_dataitem where itemcode='CommonWorkType') ELSE (select itemid from base_dataitem where itemcode='CommonRiskType') END)";
                }
                else
                {
                    str = "(select itemid from base_dataitem where itemcode='CommonRiskType')";
                }
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "a.Id";
                pagination.p_fields = "c.itemname as applystatename,applynumber,b.itemname as worktype,d.itemname as risktype,case  when a.workdepttype='0' then '单位内部' when a.workdepttype='1' then '外包单位' end workdepttypename,workplace,to_char(workstarttime,'yyyy-mm-dd hh24:mi') || ' - '||to_char(workendtime,'yyyy-mm-dd hh24:mi') as worktime,workdeptname,applyusername,to_char(a.CreateDate,'yyyy-mm-dd') as applytime,a.workdepttype,a.workdeptid,a.engineeringid,a.createuserdeptcode,a.flowid,a.nextstepapproveuseraccount,a.specialtytype,a.approvedeptid,'' as approveuseraccount,e.outtransferuseraccount,e.intransferuseraccount";
                pagination.p_tablename = string.Format(" bis_highriskcommonapply a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='CommonType') left join base_dataitemdetail c on a.applystate=c.itemvalue and c.itemid =(select itemid from base_dataitem where itemcode='CommonStatus')  left join base_dataitemdetail d on a.risktype=d.itemvalue and d.itemid ={0} left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and a.flowid=e.flowid and e.num=1",str);
                pagination.conditionJson = "1=1";
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and applyuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and workdeptcode='" + user.DeptCode + "'";
                                break;
                            case "3"://本子部门
                                pagination.conditionJson += string.Format("  and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                                break;
                            case "4":
                                pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                DataTable exportTable = highriskcommonapplybll.GetPageDataTable(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "高风险通用信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "高风险通用信息.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applystatename", ExcelColumn = "作业许可状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applynumber", ExcelColumn = "申请编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktype", ExcelColumn = "作业类型", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "risktype", ExcelColumn = "风险等级", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdepttypename", ExcelColumn = "作业单位类别", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workplace", ExcelColumn = "作业地点", Width = 60 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktime", ExcelColumn = "作业时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdeptname", ExcelColumn = "作业单位", Width = 20 });
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
        /// 导出详情
        /// </summary>
        [HandlerMonitor(0, "高风险通用作业申请导出")]
        public void ExportDetails(string keyValue)
        {
            try
            {
                HighRiskCommonApplyEntity commonapply = highriskcommonapplybll.GetEntity(keyValue);
                string isGZBJ = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("贵州毕节版本");
                if (commonapply == null)
                {
                    return;
                }
                string fileName = "";
                if (!string.IsNullOrWhiteSpace(isGZBJ))
                {
                    fileName = Server.MapPath("~/Resource/ExcelTemplate/高风险通用作业申请模板-贵州毕节.doc");
                }
                else
                {
                    fileName = Server.MapPath("~/Resource/ExcelTemplate/高风险通用作业申请模板.doc");
                }

                Document doc = new Document(fileName);
                DocumentBuilder builder = new DocumentBuilder(doc);
                var projectlist = scaffoldprojectbll.GetListByCondition(string.Format(" and ScaffoldId='{0}'", commonapply.Id)).OrderBy(t => t.CreateDate).ToList();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable("U");
                dt.Columns.Add("AX");//序号
                dt.Columns.Add("BX");//安全措施
                dt.Columns.Add("CX");//安全措施情况
                dt.Columns.Add("DX");//确认人

                string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
                int count = 1;
                foreach (var item in projectlist)
                {
                    var row = dt.NewRow();
                    row["AX"] = count.ToString();
                    row["BX"] = item.ProjectName;
                    if (item.Result == "0")//0:不同意 1:同意
                    {
                        row["CX"] = "□" + item.ResultYes + "  ☑" + item.ResultNo;
                    }
                    else
                    {
                        row["CX"] = "☑" + item.ResultYes + "  □" + item.ResultNo;
                    }
                    if (string.IsNullOrWhiteSpace(item.SignPic))
                    {
                        row["DX"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + item.SignPic.ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["DX"] = filepath;
                        }
                        else
                        {
                            row["DX"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    //row["DX"] = string.IsNullOrEmpty(item.SignPic) ? Server.MapPath("~/content/Images/no_1.png") : (Server.MapPath("~/") + item.SignPic.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    dt.Rows.Add(row);
                    count++;
                }
                if (dt.Rows.Count <= 0)
                {
                    DataRow dr1 = dt.NewRow();
                    dt.Rows.Add(dr1);
                }

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("worktypename");
                dt1.Columns.Add("risktypename");
                dt1.Columns.Add("applynumber");
                dt1.Columns.Add("workcontent");
                dt1.Columns.Add("workplace");
                dt1.Columns.Add("workdeptname");
                dt1.Columns.Add("workdutyusername");
                dt1.Columns.Add("worktutelageusername");
                dt1.Columns.Add("workusernames");
                dt1.Columns.Add("starttime");
                dt1.Columns.Add("endtime");
                dt1.Columns.Add("fauditremark");
                dt1.Columns.Add("sauditremark");
                dt1.Columns.Add("aauditremark");
                dt1.Columns.Add("zauditremark");
                dt1.Columns.Add("zgauditremark");
                dt1.Columns.Add("fauditusername");
                dt1.Columns.Add("sauditusername");
                dt1.Columns.Add("aauditusername");
                dt1.Columns.Add("zauditusername");
                dt1.Columns.Add("fauditdate");
                dt1.Columns.Add("sauditdate");
                dt1.Columns.Add("aauditdate");
                dt1.Columns.Add("zauditdate");
                dt1.Columns.Add("zgauditdate");
                DataRow row1 = dt1.NewRow();
                List<ScaffoldauditrecordEntity> list = scaffoldauditrecordbll.GetList(commonapply.Id).OrderBy(x => x.AuditDate).ToList();
                string fauditremark = "", sauditremark = "", aauditremark = "", zauditremark = "", zgauditremark = "";//审核意见
                string fauditdate = "年   月   日", sauditdate = "年   月   日", aauditdate = "年   月   日", zauditdate = "年   月   日", zgauditdate = "年   月   日";//审核时间
                string defaulttimestr = "yyyy年MM月dd日HH时mm分";
                string defaultstr = "yyyy年MM月dd日";
                for (int i = 1; i < list.Count; i++)
                {
                    var filepath = list[i].AuditSignImg == null ? "" : (Server.MapPath("~/") + list[i].AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    var stime = Convert.ToDateTime(list[i].AuditDate);
                    if (string.IsNullOrWhiteSpace(isGZBJ))
                    {
                        if (i == 1)
                        {
                            zauditremark = list[i].AuditRemark;
                            //zauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("zauditusername");
                            zauditdate = stime.ToString(defaultstr);
                        }
                        else if (i == 2)
                        {
                            sauditremark = list[i].AuditRemark;
                            //sauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("sauditusername");
                            sauditdate = stime.ToString(defaultstr);
                        }
                        else if (i == 3)
                        {
                            aauditremark = list[i].AuditRemark;
                            //aauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("aauditusername");
                            aauditdate = stime.ToString(defaultstr);

                        }
                        else if (i == 4)
                        {
                            fauditremark = list[i].AuditRemark;
                            //fauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("fauditusername");
                            fauditdate = stime.ToString(defaultstr);
                        }
                    }
                    else
                    {
                        if (i == 1)
                        {
                            zgauditremark = list[i].AuditRemark;
                            //zauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("zgauditusername");
                            zgauditdate = stime.ToString(defaultstr);
                        }
                        else if (i == 2)
                        {
                            zauditremark = list[i].AuditRemark;
                            //zauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("zauditusername");
                            zauditdate = stime.ToString(defaultstr);

                        }
                        else if (i == 3)
                        {
                            sauditremark = list[i].AuditRemark;
                            //sauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("sauditusername");
                            sauditdate = stime.ToString(defaultstr);


                        }
                        else if (i == 4)
                        {
                            aauditremark = list[i].AuditRemark;
                            //aauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("aauditusername");
                            aauditdate = stime.ToString(defaultstr);

                        }
                        else if (i == 5)
                        {
                            fauditremark = list[i].AuditRemark;
                            //fauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("fauditusername");
                            fauditdate = stime.ToString(defaultstr);
                        }
                    }

                    if (!System.IO.File.Exists(filepath))
                    {
                        filepath = pic;
                    }
                    builder.InsertImage(filepath, 80, 35);

                }
                row1["worktypename"] = scaffoldbll.getName(commonapply.WorkType, "CommonType");
                row1["risktypename"] = scaffoldbll.getName(commonapply.RiskType, "CommonRiskType");
                row1["applynumber"] = commonapply.ApplyNumber;
                row1["workcontent"] = commonapply.WorkContent;
                row1["workplace"] = commonapply.WorkPlace;
                row1["workdeptname"] = commonapply.WorkDeptName;
                row1["workdutyusername"] = commonapply.WorkDutyUserName;
                row1["worktutelageusername"] = commonapply.WorkTutelageUserName;
                row1["workusernames"] = commonapply.WorkUserNames;
                row1["starttime"] = Convert.ToDateTime(commonapply.WorkStartTime).ToString(defaulttimestr);
                row1["endtime"] = Convert.ToDateTime(commonapply.WorkEndTime).ToString(defaulttimestr);
                row1["fauditremark"] = fauditremark;
                row1["sauditremark"] = sauditremark;
                row1["aauditremark"] = aauditremark;
                row1["zauditremark"] = zauditremark;
                row1["zgauditremark"] = zgauditremark;
                row1["fauditdate"] = fauditdate;
                row1["sauditdate"] = sauditdate;
                row1["aauditdate"] = aauditdate;
                row1["zauditdate"] = zauditdate;
                row1["zgauditdate"] = zgauditdate;
                dt1.Rows.Add(row1);
                doc.MailMerge.Execute(dt1);
                doc.MailMerge.ExecuteWithRegions(dt);
                var docStream = new MemoryStream();
                doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
                Response.ContentType = "application/msword";
                Response.AddHeader("content-disposition", "attachment;filename=高风险通用作业申请表.doc");
                Response.BinaryWrite(docStream.ToArray());
                Response.End();
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 导出详情(景德镇盲板抽堵作业)
        /// </summary>
        [HandlerMonitor(0, "高风险通用作业申请导出")]
        public void ExportMBCDDetails(string keyValue)
        {
            try
            {
                HighRiskCommonApplyEntity commonapply = highriskcommonapplybll.GetEntity(keyValue);
                string isJdz = new DataItemDetailBLL().GetItemValue("景德镇版本");
                if (commonapply == null)
                {
                    return;
                }
                string fileName = "";
                if (!string.IsNullOrEmpty(isJdz) && commonapply.WorkType.Equals("11"))
                {
                    //盲板抽堵作业模板
                    fileName = Server.MapPath("~/Resource/ExcelTemplate/盲板抽堵作业导出模板.docx");
                }
                else {
                    return;
                }
                Document doc = new Document(fileName);
                DocumentBuilder builder = new DocumentBuilder(doc);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable("U");
                dt.Columns.Add("equpipelinename");
                dt.Columns.Add("media");
                dt.Columns.Add("temperature");
                dt.Columns.Add("pressure");
                dt.Columns.Add("quality");//材质
                dt.Columns.Add("standard");//规格
                dt.Columns.Add("serialnumber");//编号
                List<HighRiskApplyMBXXEntity> mbList = applymbxxbll.GetList(commonapply.Id);


                foreach (var item in mbList)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["equpipelinename"] = commonapply.PipeLine;
                    newRow["media"] = commonapply.Media;
                    newRow["temperature"] = commonapply.Temperature;
                    newRow["pressure"] = commonapply.Pressure;
                    newRow["quality"] = item.Material;
                    newRow["standard"] = item.Specification;
                    newRow["serialnumber"] = item.SerialCode;
                    dt.Rows.Add(newRow);
                }

                string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
                string defaultstr = "yyyy年MM月dd日";
                string defaultstr1 = "yyyy年MM月dd日HH时";
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("workdept");
                dt1.Columns.Add("applyno");

                dt1.Columns.Add("jobstarttime");
                dt1.Columns.Add("jobendtime");
                dt1.Columns.Add("zdutyuser");
                dt1.Columns.Add("cdutyuser");
                dt1.Columns.Add("jobperson");
                dt1.Columns.Add("custodian");
                dt1.Columns.Add("imageurl");
                dt1.Columns.Add("safetymeasures");
                dt1.Columns.Add("createuser");
                dt1.Columns.Add("createdate");
                dt1.Columns.Add("dutyuser");
                dt1.Columns.Add("dutyuser01");
                dt1.Columns.Add("approvedate");
                dt1.Columns.Add("approveidea1");
                dt1.Columns.Add("dutyuser1");
                dt1.Columns.Add("approvedate1");
                dt1.Columns.Add("approveidea2");
                dt1.Columns.Add("dutyuser2");
                dt1.Columns.Add("approvedate2");
                dt1.Columns.Add("approveidea3");
                dt1.Columns.Add("dutyuser3");
                dt1.Columns.Add("approvedate3");
                dt1.Columns.Add("RiskIdentification");

                DataRow row1 = dt1.NewRow();
                row1["workdept"] = commonapply.WorkDeptName;
                row1["applyno"] = commonapply.ApplyNumber;
                row1["jobstarttime"] = commonapply.WorkStartTime.Value.ToString(defaultstr1);
                row1["jobendtime"] = commonapply.WorkEndTime.Value.ToString(defaultstr1);
                row1["zdutyuser"] = commonapply.ZMBDutyUserName;
                row1["cdutyuser"] = commonapply.CMBDutyUserName;
                row1["jobperson"] = commonapply.WorkUserNames;
                row1["custodian"] = commonapply.WorkTutelageUserName;
                DataTable dtFile = fileInfoBLL.GetFiles(commonapply.Id + "_01");
                if (dtFile.Rows.Count > 0)
                {
                    double width = 0;
                    double height = 0;
                    if (dtFile.Rows.Count < 3)
                    {
                        width = 200;
                        height = 140;
                    }
                    else
                    {
                        width = 100;
                        height = 70;
                    }
                    builder.MoveToMergeField("imageurl");
                    foreach (DataRow row in dtFile.Rows)
                    {
                        string file = Server.MapPath(row["FilePath"].ToString());
                        if (System.IO.File.Exists(file))
                        {
                            builder.InsertImage(file, width, height);
                        }
                    }
                }

                row1["createuser"] = commonapply.CreateUserName;
                row1["createdate"] = commonapply.CreateDate.Value.ToString(defaultstr);
                row1["RiskIdentification"] = commonapply.RiskIdentification;
                var projectlist = scaffoldprojectbll.GetListByCondition(string.Format(" and ScaffoldId='{0}'", commonapply.Id)).OrderBy(t => t.CreateDate).ToList();
                DateTime? approveDate = projectlist.OrderByDescending(s => s.CreateDate).Select(t => t.CreateDate).FirstOrDefault();
                if (!Convert.IsDBNull(approveDate))
                {
                    row1["approvedate"] = approveDate.Value.ToString(defaultstr);
                }
                List<ScaffoldauditrecordEntity> list = scaffoldauditrecordbll.GetList(commonapply.Id).OrderBy(x => x.AuditDate).ToList();
                string filepath = "";
                for (int i = 0; i < list.Count(); i++)
                {
                    filepath = list[i].AuditSignImg == null ? "" : (Server.MapPath("~/") + list[i].AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    var stime = Convert.ToDateTime(list[i].AuditDate);
                    switch (i)
                    {
                        case 1:
                            row1["approveidea1"] = list[i].AuditRemark;
                            //row1["dutyuser1"] = list[i].AuditUserName;//中电检修负责人
                            builder.MoveToMergeField("dutyuser1");
                            row1["approvedate1"] = stime.ToString(defaultstr);
                            break;
                        case 2:
                            row1["approveidea2"] = list[i].AuditRemark;
                            builder.MoveToMergeField("dutyuser2");
                            row1["approvedate2"] = stime.ToString(defaultstr);
                            break;
                        case 3:
                            row1["approveidea3"] = list[i].AuditRemark;
                            builder.MoveToMergeField("dutyuser3");
                            row1["approvedate3"] = stime.ToString(defaultstr);
                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrEmpty(filepath))
                    {
                        if (!System.IO.File.Exists(filepath))
                        {
                            filepath = pic;
                        }
                        builder.InsertImage(filepath, 80, 35);
                    }
                }
                dt1.Rows.Add(row1);

                DataTable dt2 = new DataTable("T");
                dt2.Columns.Add("AX");
                dt2.Columns.Add("BX");
                dt2.Columns.Add("CX");

                int count = 1;
                foreach (var item in projectlist)
                {
                    var row = dt2.NewRow();
                    row["AX"] = count.ToString();
                    row["BX"] = item.ProjectName;
                    if (item.Result == "0")//0:不同意 1:同意
                    {
                        row["CX"] = "□" + item.ResultYes + "   ☑" + item.ResultNo;
                    }
                    else
                    {
                        row["CX"] = "☑" + item.ResultYes + "   □" + item.ResultNo;
                    }
                    dt2.Rows.Add(row);
                    count++;
                }

                builder.MoveToBookmark("dutyuser");
                string[] signpics = projectlist.Where(t => !string.IsNullOrEmpty(t.SignPic)).Select(t => t.SignPic).Distinct().ToArray();
                filepath = "";
                if (signpics.Length > 0)
                {
                    for (int i = 0; i < signpics.Length; i++)
                    {
                        filepath = Server.MapPath("~" + signpics[i]);
                        if (!System.IO.File.Exists(filepath))
                        {
                            filepath = pic;
                        }
                        builder.InsertImage(filepath, 80, 30);
                    }
                }
                filepath = "";
                builder.MoveToBookmark("dutyuser01");
                if (signpics.Length > 0)
                {
                    for (int i = 0; i < signpics.Length; i++)
                    {
                        filepath = Server.MapPath("~" + signpics[i]);
                        if (!System.IO.File.Exists(filepath))
                        {
                            filepath = pic;
                        }
                        builder.InsertImage(filepath, 80, 30);
                    }
                }

                doc.MailMerge.Execute(dt1);
                doc.MailMerge.ExecuteWithRegions(dt);
                Table table1 = (Table)doc.GetChildNodes(NodeType.Table, true)[0];
                if (table1.Rows.Count > 0)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int m = 2;
                        Cell c1 = table1.Rows[m].Cells[j];
                        c1.CellFormat.VerticalMerge = CellMerge.First;//垂直合并，First：当前行为起始行
                        while (m <= dt.Rows.Count)
                        {
                            Cell c2 = table1.Rows[m + 1].Cells[j];
                            c2.CellFormat.VerticalMerge = CellMerge.Previous;//Previous表示向前合并
                            m++;
                        }
                    }
                }
                doc.MailMerge.ExecuteWithRegions(dt2);
                var docStream = new MemoryStream();
                doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
                Response.ContentType = "application/msword";
                Response.AddHeader("content-disposition", "attachment;filename=盲板抽堵安全作业证.doc");
                Response.BinaryWrite(docStream.ToArray());
                Response.End();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 导出详情(景德镇高处作业)
        /// </summary>
        /// <param name="keyValue"></param>
        [HandlerMonitor(0, "高风险通用作业申请导出")]
        public void ExportHighWorkDetails(string keyValue)
        {
            try
            {
                HighRiskCommonApplyEntity commonapply = highriskcommonapplybll.GetEntity(keyValue);
                string isJdz = new DataItemDetailBLL().GetItemValue("景德镇版本");
                if (commonapply == null)
                {
                    return;
                }
                string fileName = "";
                if (!string.IsNullOrEmpty(isJdz) && commonapply.WorkType.Equals("12"))
                {
                    //盲板抽堵作业模板
                    fileName = Server.MapPath("~/Resource/ExcelTemplate/高处作业许可证导出模板.docx");
                }
                else
                {
                    return;
                }
                Document doc = new Document(fileName);
                DocumentBuilder builder = new DocumentBuilder(doc);
                var projectlist = scaffoldprojectbll.GetListByCondition(string.Format(" and ScaffoldId='{0}'", commonapply.Id)).OrderBy(t => t.CreateDate).ToList();
                string defaulttimestr = "yyyy年MM月dd日HH时mm分";
                string defaultstr = "yyyy年MM月dd日HH时";
                string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                #region [dt 申明列]
                dt.Columns.Add("applynumber");//申请编号
                dt.Columns.Add("workdeptname");//申请编号
                dt.Columns.Add("workplace");//作业地点
                dt.Columns.Add("workcontent");//作业内容
                dt.Columns.Add("workticketnocontent");//工作票编号及内容
                dt.Columns.Add("workdutyusername");//作业负责人
                dt.Columns.Add("worktutelageusername");//监护人
                dt.Columns.Add("workusernames");//作业人员
                dt.Columns.Add("dangeranalyse");//工作危险分析（JHA）
                dt.Columns.Add("safetyanalyse");//作业安全分析（JSA）
                dt.Columns.Add("workstarttime");//计划作业开始时间(年月日时)
                dt.Columns.Add("workendtime");//计划作业结束时间(年月日时)
                dt.Columns.Add("safetymeasure1");
                dt.Columns.Add("safetymeasure2");
                dt.Columns.Add("safetymeasure3");
                //dt.Columns.Add("workusernames");//作业人姓名
                dt.Columns.Add("applyusername");//申请人姓名
                dt.Columns.Add("createdate");//申请时间(年月日时)
                dt.Columns.Add("worktutelageusername1");//监护人姓名
                dt.Columns.Add("confirmdate");//措施确认时间(年月日时)
                dt.Columns.Add("approveuser1");//最后一步批准人签名
                dt.Columns.Add("approvedate1");//审批时间(年月日时)
                dt.Columns.Add("deptname");//受影响相关方单位名称
                dt.Columns.Add("effectconfirmername");//确认人
                dt.Columns.Add("approvedate2");//受影响相关方审批时间(年月日时分)

                dt.Columns.Add("workdutyusername1");//作业部门安全监督人员(此处为工作负责人签名)
                dt.Columns.Add("message1");//作业部门安全监督人员审核意见
                dt.Columns.Add("approvedate3");//工作负责人审批时间(年月日时分)

                dt.Columns.Add("workdutyusername2");//作业部门专业管理人员(此处为工作负责人签名)
                //dt.Columns.Add("message1");//作业部门专业管理人员审批意见
                dt.Columns.Add("approvedate4");//工作负责人审批时间(年月日时分)

                dt.Columns.Add("workuser1");//作业部门负责人审批(此处为作业单位专工签名)
                dt.Columns.Add("message2");//作业部门负责人审批意见
                dt.Columns.Add("approvedate5");//审批时间(年月日时分)

                dt.Columns.Add("workuser2");//生产技术部专工审核(此处为生产技术部专工签名)
                dt.Columns.Add("message3");//生产技术部专工审核意见
                dt.Columns.Add("approvedate6");//审批时间(年月日时分)

                dt.Columns.Add("workuser3");//HSE部安全监督人员审核(此处为HSE部专工及安全管理员签名)
                dt.Columns.Add("message4");//HSE部安全监督人员审核意见
                dt.Columns.Add("approvedate7");//审批时间(年月日时分)

                dt.Columns.Add("workuser4");//生产技术部负责人签名
                dt.Columns.Add("message5");//生产技术部负责人审批意见
                dt.Columns.Add("approvedate8");//审批时间(年月日时分)

                dt.Columns.Add("workuser5");//HSE部负责人签名
                dt.Columns.Add("message6");//HSE部负责人审核意见
                dt.Columns.Add("approvedate9");//审批时间(年月日时分)

                dt.Columns.Add("factoryleader");//厂领导签名
                dt.Columns.Add("message7");//厂领导审批意见
                dt.Columns.Add("approvedate10");//审批时间(年月日时分)

                dt.Columns.Add("yxpermitusername");//运行许可人签名
                dt.Columns.Add("approvedate11");//审批时间(年月日时分)

                dt.Columns.Add("watchusername");//值长或值班负责人签名
                dt.Columns.Add("approvedate12");//审批时间(年月日时分)
                #endregion
                DataRow row1 = dt.NewRow();

                #region [安全措施]               
                int index = 0;
                string safetymeasure1 = string.Empty;
                string safetymeasure2 = string.Empty;
                string safetymeasure3 = string.Empty;
                foreach (var item in projectlist)
                {
                    if (index < 7)
                    {
                        if (item.Result == null)
                        {
                            safetymeasure1 += "□" + item.ProjectName + "\r\n";
                        }
                        else if (item.Result == "0")//0:不同意 1:同意
                        {
                            safetymeasure1 += "□" + item.ProjectName + "\r\n";
                        }
                        else
                        {
                            safetymeasure1 += "☑" + item.ProjectName + "\r\n";
                        }
                    }
                    else if (index < 16)
                    {
                        if (item.Result == null)
                        {
                            safetymeasure2 += "□" + item.ProjectName + "\r\n";
                        }
                        else if(item.Result == "0")//0:不同意 1:同意
                        {
                            safetymeasure2 += "□" + item.ProjectName + "\r\n";
                        }
                        else
                        {
                            safetymeasure2 += "☑" + item.ProjectName + "\r\n";
                        }

                    }
                    else
                    {
                        if (item.Result == null)
                        {
                            safetymeasure3 += "□" + item.ProjectName + "\r\n";
                        }
                        else if(item.Result == "0")//0:不同意 1:同意
                        {
                            safetymeasure3 += "□" + item.ProjectName + "\r\n";
                        }
                        else
                        {
                            safetymeasure3 += "☑" + item.ProjectName + "\r\n";
                        }
                    }
                    index++;
                }
                row1["safetymeasure1"] = safetymeasure1;
                row1["safetymeasure2"] = safetymeasure2;
                row1["safetymeasure3"] = safetymeasure3;
                #endregion

                row1["applynumber"] = commonapply.ApplyNumber;
                row1["workdeptname"] = commonapply.WorkDeptName;
                row1["workplace"] = commonapply.WorkPlace;
                row1["workcontent"] = commonapply.WorkContent;
                row1["workticketnocontent"] = commonapply.WorkTicketNoContent;
                row1["workdutyusername"] = commonapply.WorkDutyUserName;
                row1["worktutelageusername"] = commonapply.WorkTutelageUserName;
                row1["workusernames"] = commonapply.WorkUserNames;
                if (commonapply.DangerAnalyse == 1)
                {
                    row1["dangeranalyse"] = "☑有   □无";
                }
                else
                {
                    row1["dangeranalyse"] = "□有   ☑无";
                }
                if (commonapply.SafetyAnalyse == 1)
                {
                    row1["safetyanalyse"] = "☑有   □无";
                }
                else
                {
                    row1["safetyanalyse"] = "□有   ☑无";
                }
                row1["workstarttime"] = commonapply.WorkStartTime.Value.ToString(defaultstr);
                row1["workendtime"] = commonapply.WorkEndTime.Value.ToString(defaultstr);
                row1["applyusername"] = commonapply.ApplyUserName;
                row1["createdate"] = commonapply.CreateDate.Value.ToString(defaultstr);
                string fp1 = string.Empty, fp2 = string.Empty;
                //审批记录
                List<ScaffoldauditrecordEntity> list = scaffoldauditrecordbll.GetList(commonapply.Id).OrderBy(x => x.AuditDate).ToList();
                //row1["approveuser1"] = list.Last().AuditUserName;//作业批准人
                if (list != null && list.Count() > 0)
                {
                    //监护人姓名
                    row1["worktutelageusername1"] = string.Join(",", projectlist.Select(t => t.CheckPersons).Distinct()).TrimEnd(',');
                    DateTime? comfirmDate = projectlist.OrderByDescending(t => t.CreateDate).Select(t => t.CreateDate).FirstOrDefault();
                    if (!Convert.IsDBNull(comfirmDate))
                    {
                        row1["confirmdate"] = comfirmDate.Value.ToString(defaultstr);
                    }
                    fp1 = list.Last().AuditSignImg;
                    fp1 = fp1 == null ? "" : (Server.MapPath("~/") + fp1.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    builder.MoveToBookmark("approveuser1");
                    if (!string.IsNullOrEmpty(fp1))
                    {
                        if (!System.IO.File.Exists(fp1))
                        {
                            fp1 = pic;
                        }
                        builder.InsertImage(fp1, 80, 35);
                    }
                    row1["approvedate1"] = list.Last().AuditDate.Value.ToString(defaultstr);
                    int m = 0, n = 0;
                    fp1 = "";
                    for (int i = 0; i < list.Count(); i++)
                    {
                        switch (commonapply.RiskType)
                        {
                            case "0":
                                //特级高处作业
                                switch (i)
                                {
                                    case 1:
                                        //作业部门安全监督人员审核
                                        row1["approvedate3"] = list[1].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message1"] = list[1].AuditRemark;
                                        fp2 = list[1].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workdutyusername1");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        //作业部门专业管理人员审批
                                        row1["approvedate4"] = list[1].AuditDate.Value.ToString(defaulttimestr);
                                        fp2 = list[1].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workdutyusername2");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 2:
                                        //作业部门负责人审批（作业单位专工签名）
                                        row1["approvedate5"] = list[2].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message2"] = list[2].AuditRemark;
                                        fp2 = list[2].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workuser1");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 4:
                                        //生产技术部专工审核(生产技术部专工签名)
                                        row1["approvedate6"] = list[4].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message3"] = list[4].AuditRemark;
                                        fp2 = list[4].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workuser2");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 5:
                                        //HSE部安全监督人员审核(HSE部专工及安全管理员签名)
                                        row1["approvedate7"] = list[5].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message4"] = list[5].AuditRemark;
                                        fp2 = list[5].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workuser3");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 6:
                                        //生产技术部负责人审批(生产技术部负责人签名)
                                        row1["approvedate8"] = list[6].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message5"] = list[6].AuditRemark;
                                        fp2 = list[6].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workuser4");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 7:
                                        //HSE部负责人审核（HSE部负责人签名）
                                        row1["approvedate9"] = list[7].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message6"] = list[7].AuditRemark;
                                        fp2 = list[7].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workuser5");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 8:
                                        //厂领导审批意见(厂领导审批意见)
                                        row1["approvedate10"] = list[8].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message7"] = list[8].AuditRemark;
                                        fp2 = list[8].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("factoryleader");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 9:
                                        if (!string.IsNullOrEmpty(commonapply.EffectConfimerId))
                                        {
                                            //含受影响相关方
                                            row1["deptname"] = list[9].AuditDeptName;
                                            row1["approvedate2"] = list[9].AuditDate.Value.ToString(defaultstr);
                                            fp1 = list[9].AuditSignImg;

                                            m = 10;
                                            n = 11;
                                        }
                                        else
                                        {
                                            m = 9;
                                            n = 10;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "1":
                                //三级高处作业
                                switch (i)
                                {
                                    case 1:
                                        //作业部门安全监督人员审核
                                        row1["approvedate3"] = list[1].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message1"] = list[1].AuditRemark;
                                        fp2 = list[1].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workdutyusername1");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        //作业部门专业管理人员审批
                                        row1["approvedate4"] = list[1].AuditDate.Value.ToString(defaulttimestr);
                                        fp2 = list[1].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workdutyusername2");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 2:
                                        //作业部门负责人审批（作业单位专工签名）
                                        row1["approvedate5"] = list[2].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message2"] = list[2].AuditRemark;
                                        fp2 = list[2].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workuser1");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 4:
                                        //生产技术部专工审核(生产技术部专工签名)
                                        row1["approvedate6"] = list[4].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message3"] = list[4].AuditRemark;
                                        fp2 = list[4].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workuser2");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 5:
                                        //HSE部安全监督人员审核(HSE部专工及安全管理员签名)
                                        row1["approvedate7"] = list[5].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message4"] = list[5].AuditRemark;
                                        fp2 = list[5].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workuser3");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 6:
                                        //生产技术部负责人审批(生产技术部负责人签名)
                                        row1["approvedate8"] = list[6].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message5"] = list[6].AuditRemark;
                                        fp2 = list[6].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workuser4");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 7:
                                        if (!string.IsNullOrEmpty(commonapply.EffectConfimerId))
                                        {
                                            //受影响相关方
                                            row1["deptname"] = list[7].AuditDeptName;
                                            row1["approvedate2"] = list[7].AuditDate.Value.ToString(defaultstr);
                                            fp1 = list[7].AuditSignImg;
                                            m = 8;
                                            n = 9;
                                        }
                                        else
                                        {
                                            m = 7;
                                            n = 8;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "2":
                                //二级高处作业
                                switch (i)
                                {
                                    case 1:
                                        //作业部门安全监督人员审核
                                        row1["approvedate3"] = list[1].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message1"] = list[1].AuditRemark;
                                        fp2 = list[1].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workdutyusername1");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        //作业部门专业管理人员审批
                                        row1["approvedate4"] = list[1].AuditDate.Value.ToString(defaulttimestr);
                                        fp2 = list[1].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workdutyusername2");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 2:
                                        //作业部门负责人审批（作业单位专工签名）
                                        row1["approvedate5"] = list[2].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message2"] = list[2].AuditRemark;
                                        fp2 = list[2].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workuser1");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 4:
                                        if (!string.IsNullOrEmpty(commonapply.EffectConfimerId))
                                        {
                                            //受影响相关方
                                            row1["deptname"] = list[4].AuditDeptName;
                                            row1["approvedate2"] = list[4].AuditDate.Value.ToString(defaultstr);
                                            fp1 = list[4].AuditSignImg;
                                            m = 5;
                                            n = 6;
                                        }
                                        else
                                        {
                                            m = 4;
                                            n = 5;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "3":
                                //一级高处作业
                                switch (i)
                                {
                                    case 1:
                                        //作业部门安全监督人员审核
                                        row1["approvedate3"] = list[1].AuditDate.Value.ToString(defaulttimestr);
                                        row1["message1"] = list[1].AuditRemark;
                                        fp2 = list[1].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workdutyusername1");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        //作业部门专业管理人员审批
                                        row1["approvedate4"] = list[1].AuditDate.Value.ToString(defaulttimestr);
                                        fp2 = list[1].AuditSignImg;
                                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                                        builder.MoveToBookmark("workdutyusername2");
                                        if (!string.IsNullOrEmpty(fp2))
                                        {
                                            if (!System.IO.File.Exists(fp2))
                                            {
                                                fp2 = pic;
                                            }
                                            builder.InsertImage(fp2, 80, 35);
                                        }
                                        break;
                                    case 2:
                                        if (!string.IsNullOrEmpty(commonapply.EffectConfimerId))
                                        {
                                            //受影响相关方
                                            row1["deptname"] = list[2].AuditDeptName;
                                            row1["approvedate2"] = list[2].AuditDate.Value.ToString(defaultstr);
                                            fp1 = list[2].AuditSignImg;
                                            m = 3;
                                            n = 4;
                                        }
                                        else
                                        {
                                            m = 2;
                                            n = 3;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    //受影响相关方确认人签名
                    if (!string.IsNullOrEmpty(fp1))
                    {
                        builder.MoveToBookmark("effectconfirmername");
                        fp1 = (Server.MapPath("~/") + fp1.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (!System.IO.File.Exists(fp1))
                        {
                            fp1 = pic;
                        }
                        builder.InsertImage(fp1, 80, 35);
                    }
                    if (list.Count() >= m + 1)
                    {
                        //运行许可人
                        row1["approvedate11"] = list[m].AuditDate.Value.ToString(defaulttimestr);
                        fp2 = list[m].AuditSignImg;
                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        builder.MoveToBookmark("yxpermitusername");
                        if (!string.IsNullOrEmpty(fp2))
                        {
                            if (!System.IO.File.Exists(fp2))
                            {
                                fp2 = pic;
                            }
                            builder.InsertImage(fp2, 80, 35);
                        }
                    }

                    if (list.Count() >= n + 1)
                    {
                        //值长或值班负责人签名
                        row1["approvedate12"] = list[n].AuditDate.Value.ToString(defaulttimestr);
                        fp2 = list[n].AuditSignImg;
                        fp2 = fp2 == null ? "" : (Server.MapPath("~/") + fp2.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        builder.MoveToBookmark("watchusername");
                        if (!string.IsNullOrEmpty(fp2))
                        {
                            if (!System.IO.File.Exists(fp2))
                            {
                                fp2 = pic;
                            }
                            builder.InsertImage(fp2, 80, 35);
                        }
                    }
                }
                dt.Rows.Add(row1);
                doc.MailMerge.Execute(dt);
                var docStream = new MemoryStream();
                doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
                Response.ContentType = "application/msword";
                Response.AddHeader("content-disposition", "attachment;filename=高处作业许可证.doc");
                Response.BinaryWrite(docStream.ToArray());
                Response.End();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


        #region 获取高风险通用作业流程图对象
        /// <summary>
        /// 获取流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetWorkActionList(string keyValue)
        {
            try
            {
                HighRiskCommonApplyEntity commonEntity = highriskcommonapplybll.GetEntity(keyValue);
                string moduleName = highriskcommonapplybll.GetModuleName(commonEntity);
                var josnData = highriskcommonapplybll.GetFlow(commonEntity.Id, moduleName);
                return Content(josnData.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

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
                var data = highriskcommonapplybll.GetLedgerList(pagination, queryJson);
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
        public ActionResult ExportCommonLedgerData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_fields = "applynumber,case when a.workoperate='1' then '作业暂停' when realityworkstarttime is not null and realityworkendtime is null then '作业中' when realityworkendtime is not null then '已结束'  else '即将作业' end ledgertype,b.itemname as worktype,workdeptname,case when workdepttype=0 then '单位内部' when workdepttype=1 then '外包单位' end workdepttypename,  to_char(a.workstarttime,'yyyy-MM-dd hh24:mm') || '-' || to_char(a.workendtime,'yyyy-MM-dd hh24:mm') as worktime,to_char(a.realityworkstarttime,'yyyy-MM-dd hh24:mm') || '-' || to_char(a.realityworkendtime,'yyyy-MM-dd hh24:mm') as  realityworktime,workplace,applyusername";
                DataTable data = highriskcommonapplybll.GetLedgerList(pagination, queryJson, false);
                //DataTable excelTable = new DataTable();
                //excelTable.Columns.Add(new DataColumn("applynumber"));
                //excelTable.Columns.Add(new DataColumn("ledgertype"));
                //excelTable.Columns.Add(new DataColumn("worktype"));
                //excelTable.Columns.Add(new DataColumn("workdeptname"));
                //excelTable.Columns.Add(new DataColumn("workdepttypename"));
                //excelTable.Columns.Add(new DataColumn("worktime"));
                //excelTable.Columns.Add(new DataColumn("realityworktime"));
                //excelTable.Columns.Add(new DataColumn("workplace"));
                //excelTable.Columns.Add(new DataColumn("applyusername"));

                //foreach (DataRow item in data.Rows)
                //{
                //    DataRow newDr = excelTable.NewRow();
                //    newDr["applynumber"] = item["applynumber"];
                //    newDr["ledgertype"] = item["ledgertype"];
                //    newDr["worktype"] = item["worktype"];
                //    newDr["workdeptname"] = item["workdeptname"];
                //    newDr["workdepttypename"] = item["workdepttypename"];

                //    DateTime workstarttime, workendtime, realityworkstarttime, realityworkendtime;
                //    DateTime.TryParse(item["workstarttime"].ToString(), out workstarttime);
                //    DateTime.TryParse(item["workendtime"].ToString(), out workendtime);
                //    DateTime.TryParse(item["realityworkstarttime"].ToString(), out realityworkstarttime);
                //    DateTime.TryParse(item["realityworkendtime"].ToString(), out realityworkendtime);

                //    string worktime = string.Empty;
                //    if (workstarttime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                //    {
                //        worktime += workstarttime.ToString("yyyy-MM-dd HH:mm") + " - ";
                //    }
                //    if (workendtime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                //    {
                //        worktime += workendtime.ToString("yyyy-MM-dd HH:mm");
                //    }
                //    newDr["worktime"] = worktime;
                //    string realityworktime = string.Empty;
                //    if (realityworkstarttime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                //    {
                //        realityworktime += realityworkstarttime.ToString("yyyy-MM-dd HH:mm") + " - ";
                //    }
                //    if (realityworkendtime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                //    {
                //        realityworktime += realityworkendtime.ToString("yyyy-MM-dd HH:mm");
                //    }
                //    newDr["realityworktime"] = realityworktime;
                //    newDr["workplace"] = item["workplace"];
                //    newDr["applyusername"] = item["APPLYUSERNAME"];
                //    excelTable.Rows.Add(newDr);
                //}
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "高风险作业台账";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "高风险作业台账.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applynumber", ExcelColumn = "申请编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ledgertype", ExcelColumn = "作业状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktype", ExcelColumn = "作业类型", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdeptname", ExcelColumn = "作业单位", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdepttypename", ExcelColumn = "作业单位类别", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktime", ExcelColumn = "申请作业时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realityworktime", ExcelColumn = "实际作业时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workplace", ExcelColumn = "作业地点", Width = 60 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyusername", ExcelColumn = "作业申请人", Width = 40 });
                //调用导出方法
                ExcelHelper.ExcelDownload(data, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion
        #endregion

        #region 作业安全分析
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetRiskListJson(string workId)
        {
            var data = highriskrecordbll.GetList(workId);
            return ToJsonResult(data);
        }
        #endregion
    }
}
