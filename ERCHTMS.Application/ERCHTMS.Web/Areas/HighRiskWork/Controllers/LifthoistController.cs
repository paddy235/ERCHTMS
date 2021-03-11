using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using Newtonsoft.Json;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using System.Data;
using System.Collections.Generic;
using BSFramework.Util.Offices;
using System.Web;
using Aspose.Words;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：起吊作业
    /// </summary>
    public class LifthoistController : MvcControllerBase
    {
        private LifthoistjobBLL lifthoistjobbll = new LifthoistjobBLL();
        private LifthoistcertBLL lifthoistcertbll = new LifthoistcertBLL();
        private LifthoistauditrecordBLL lifthoistauditrecordbll = new LifthoistauditrecordBLL();
        private LifthoistsafetyBLL lifthoistsafetybll = new LifthoistsafetyBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private LifthoistpersonBLL lifthoistpersonbll = new LifthoistpersonBLL();
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
        /// 流程图页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }
        /// <summary>
        /// 凭吊证页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CertForm()
        {
            return View();
        }
        /// <summary>
        /// 临时入场设备
        /// </summary>
        /// <returns></returns>
        public ActionResult TempEquipentIndex()
        {
            return View();
        }
        /// <summary>
        /// 临时入场设备详情
        /// </summary>
        /// <returns></returns>
        public ActionResult TempEquipentForm()
        {
            return View();
        }

        /// <summary>
        /// 操作人员信息
        /// </summary>
        /// <returns></returns>
        public ActionResult OperatePerson()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页对象</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                LifthoistSearchModel search = null;
                if (!string.IsNullOrEmpty(queryJson))
                {
                    search = JsonConvert.DeserializeObject<LifthoistSearchModel>(queryJson);
                }
                else
                {
                    search = new LifthoistSearchModel();
                }

                #region 数据权限
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    string isAllDataRange = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetEnableItemValue("HighRiskWorkDataRange"); //特殊标记，高风险作业模块是否看全厂数据
                    if (!string.IsNullOrWhiteSpace(isAllDataRange))
                    {
                        pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        //根据当前用户对模块的权限获取记录
                        string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "a.createuserdeptcode", "a.createuserorgcode");
                        if (!string.IsNullOrEmpty(where))
                        {
                            pagination.conditionJson += " and " + where;
                        }
                    }
                    
                }
                #endregion
                //查凭吊证
                DataTable dt = null;
                if (search.pagetype == "1")
                {
                    dt = lifthoistcertbll.GetList(pagination, search);
                }
                else
                {
                    dt = lifthoistjobbll.GetList(pagination, search);
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
        /// 获取临时入场设备列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult getTempEquipentList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = "t.auditstate = 2 and t1.auditstate = 2 ";

                LifthoistSearchModel search = null;
                if (!string.IsNullOrEmpty(queryJson))
                {
                    search = JsonConvert.DeserializeObject<LifthoistSearchModel>(queryJson);
                }
                else
                {
                    search = new LifthoistSearchModel();
                }

                #region 数据权限
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    //根据当前用户对模块的权限获取记录
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "t.createuserdeptcode", "t.createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
                #endregion
                //查凭吊证
                DataTable dt = lifthoistjobbll.getTempEquipentList(pagination, search);
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
        /// 获取起重吊装作业实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = lifthoistjobbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取凭吊证实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetCertFormJson(string keyValue)
        {
            var data = lifthoistcertbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 得到审核记录
        /// </summary>
        /// <param name="businessid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAuditListToJson(string businessid)
        {
            var data = lifthoistauditrecordbll.GetList(businessid);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 得到起重吊装作业流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetJobFlow(string keyValue)
        {
            LifthoistjobEntity entity = lifthoistjobbll.GetEntity(keyValue);
            string modulename = string.Empty;

            if (entity.QUALITYTYPE == "0")
            {
                modulename = "(起重吊装作业30T以下)审核";
            }
            else
            {
                modulename = "(起重吊装作业30T以上)审核";
            }
            
            var josnData = lifthoistjobbll.GetFlow(entity.ID, modulename);
            return Content(josnData.ToJson());
        }

        /// <summary>
        /// 得到凭吊证流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetCertFlow(string keyValue)
        {
            LifthoistcertEntity entity = lifthoistcertbll.GetEntity(keyValue);
            string modulename = "(起重吊装准吊证)审核";
            
            var josnData = lifthoistjobbll.GetFlow(entity.ID, modulename);
            return Content(josnData.ToJson());
        }

        /// <summary>
        /// 得到安全措施
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSafetyMeasureToJson(string keyValue)
        {
            IEnumerable<LifthoistsafetyEntity> safetys = lifthoistsafetybll.GetList(string.Format("LIFTHOISTCERTID = '{0}'", keyValue));
            return ToJsonResult(safetys);
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
        [HandlerMonitor(6, "起重吊装作业删除")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            try
            {
                lifthoistjobbll.RemoveForm(keyValue);
                DeleteFiles(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                return Error("删除出错，错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="jsonData">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "起重吊装作业保存")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string jsonData)
        {
            try
            {
                LifthoistjobEntity entity = JsonConvert.DeserializeObject<LifthoistjobEntity>(jsonData);
                lifthoistjobbll.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                return Error("保存出错，错误信息：" + ex.Message);
            }
            return Success("操作成功。");
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="jsonData">审核实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(8, "起重吊装作业提交")]
        [AjaxOnly]
        public ActionResult AuditSubmit(string keyValue, string jsonData)
        {
            try
            {
                LifthoistauditrecordEntity auditEntity = JsonConvert.DeserializeObject<LifthoistauditrecordEntity>(jsonData);
                lifthoistjobbll.ApplyCheck(keyValue, auditEntity);
            }
            catch (Exception ex)
            {
                return Error("审核出错，错误信息：" + ex.Message);
            }
            return Success("提交成功。");
        }
        /// <summary>
        /// 凭吊证删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveCertForm(string keyValue)
        {
            try
            {
                lifthoistcertbll.RemoveForm(keyValue);
                DeleteFiles(keyValue);
            }
            catch (Exception ex)
            {
                return Error("删除出错，错误信息：" + ex.Message);
            }
            return Success("删除成功。");
        }

        /// <summary>
        /// 凭吊证保存
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveCertForm(string keyValue, string jsonData)
        {
            try
            {
                LifthoistcertEntity entity = JsonConvert.DeserializeObject<LifthoistcertEntity>(jsonData);
                lifthoistcertbll.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                return Error("保存出错，错误信息：" + ex.Message);
            }
            return Success("操作成功。");
        }

        /// <summary>
        /// 凭吊证审核
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="auditEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult AuditCertSubmit(string keyValue, LifthoistcertEntity entity, LifthoistauditrecordEntity auditEntity)
        {
            try
            {
                lifthoistcertbll.ApplyCheck(keyValue, entity, auditEntity);
            }
            catch (Exception ex)
            {
                return Error("审核出错，错误信息：" + ex.Message);
            }
            return Success("提交成功。");
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
                string isHrdl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("IsOpenPassword");
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";
                pagination.sidx = "applydate desc,applycode";//排序字段
                pagination.sord = "desc";//排序方式

                LifthoistSearchModel search = null;
                if (!string.IsNullOrEmpty(queryJson))
                {
                    search = JsonConvert.DeserializeObject<LifthoistSearchModel>(queryJson);
                }
                else
                {
                    search = new LifthoistSearchModel();
                }

                #region 数据权限
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    //根据当前用户对模块的权限获取记录
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "a.createuserdeptcode", "a.createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
                #endregion
                //查凭吊证
                DataTable dt = null;
                string title = "";
                if (search.pagetype == "1")
                {
                    title = "准吊证申请";
                    dt = lifthoistcertbll.GetList(pagination, search);
                }
                else
                {
                    title = "起重吊装作业申请";
                    dt = lifthoistjobbll.GetList(pagination, search);
                }
                DataTable exportTable = dt;
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("auditstate"));
                excelTable.Columns.Add(new DataColumn("applycodestr"));
                if (search.pagetype != "1")
                {
                    excelTable.Columns.Add(new DataColumn("qualitytype"));
                }
                excelTable.Columns.Add(new DataColumn("toolname"));
                excelTable.Columns.Add(new DataColumn("worktime"));
                excelTable.Columns.Add(new DataColumn("constructionunitname"));
                excelTable.Columns.Add(new DataColumn("applyusername"));
                excelTable.Columns.Add(new DataColumn("applydate"));
                excelTable.Columns.Add(new DataColumn("flowdeptname"));
                excelTable.Columns.Add(new DataColumn("flowname"));

                foreach (DataRow item in dt.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    string auditstate = item["auditstate"].ToString();
                    if (auditstate == "0")
                    {
                        auditstate = "申请中";
                    }
                    else if (auditstate == "1")
                    {
                        auditstate = "审核(批)中";
                    }
                    else if (auditstate == "2")
                    {
                        auditstate = "审核(批)通过";
                    }
                    newDr["auditstate"] = auditstate;
                    newDr["applycodestr"] = item["applycodestr"];

                    if (search.pagetype != "1")
                    {
                        string qualitytype = item["qualitytype"].ToString();
                        if (qualitytype == "0")
                        {
                            qualitytype = "30T以下";
                        }
                        else if (qualitytype == "1")
                        {
                            qualitytype = "30T以上";
                        }
                        else
                        {
                            qualitytype = "2台起重设备共同起吊3T及以上";
                        }
                        newDr["qualitytype"] = qualitytype;
                    }
                    newDr["auditstate"] = auditstate;
                    newDr["toolname"] = item["toolname"];

                    DateTime workstartdate, workenddate, applydate;
                    DateTime.TryParse(item["workstartdate"].ToString(), out workstartdate);
                    DateTime.TryParse(item["workenddate"].ToString(), out workenddate);
                    DateTime.TryParse(item["applydate"].ToString(), out applydate);

                    string worktime = string.Empty;
                    if (workstartdate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        worktime += workstartdate.ToString("yyyy-MM-dd HH:mm");
                    }
                    if (workenddate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        worktime += " - " + workenddate.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["worktime"] = worktime;
                    newDr["constructionunitname"] = item["constructionunitname"];
                    newDr["applyusername"] = item["applyusername"];

                    string adate = string.Empty;
                    if (applydate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        adate = applydate.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["applydate"] = adate;
                    newDr["flowdeptname"] = item["flowdeptname"];
                    newDr["flowname"] = item["flowname"];

                    excelTable.Rows.Add(newDr);
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = title;
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = title + ".xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "auditstate", ExcelColumn = "作业许可状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applycodestr", ExcelColumn = "申请编号", Width = 20 });
                if (search.pagetype != "1")
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "qualitytype", ExcelColumn = "起吊质量描述", Width = 20 });
                }
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "toolname", ExcelColumn = "吊装工具名称", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktime", ExcelColumn = isHrdl == "true" ? "作业时间" : "计划作业时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "constructionunitname", ExcelColumn = "作业单位", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyusername", ExcelColumn = "申请人", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applydate", ExcelColumn = "申请时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "flowdeptname", ExcelColumn = "审核/批部门", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "flowname", ExcelColumn = "审核/批流程", Width = 20 });
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
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HandlerMonitor(0,"导出信息")]
        public ActionResult ExportInfo(string keyValue)
        {
            try
            {
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                //报告对象

                string fileName = "起重吊装作业_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\起重吊装作业导出模板.docx";
                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                DocumentBuilder builder = new DocumentBuilder(doc);
                DataTable dt = new DataTable();
                dt.Columns.Add("APPLYCODE"); //编号
                dt.Columns.Add("CHARGEPERSONNAME"); //工作负责人
                dt.Columns.Add("GUARDIANNAME"); //监护人
                dt.Columns.Add("QUALITY"); //起吊重物质量
                dt.Columns.Add("TOOLNAME"); //吊装工具名称
                dt.Columns.Add("CONSTRUCTIONUNITNAME"); //吊装施工单位
                dt.Columns.Add("CONSTRUCTIONADDRESS"); //吊装施工地点
                dt.Columns.Add("HOISTCONTENT"); //吊装内容
                dt.Columns.Add("WORKDATE"); //作业时间
                dt.Columns.Add("APPLYCOMPANYNAME"); //申请单位
                dt.Columns.Add("APPLYUSERNAME"); //申请人
                dt.Columns.Add("APPLYDATE"); //申请日期
                dt.Columns.Add("approve1"); //技术支持部专业
                dt.Columns.Add("approvedate1");
                dt.Columns.Add("approve2"); //技术支持部
                dt.Columns.Add("approvedate2"); 
                dt.Columns.Add("approve3"); //分管领导
                dt.Columns.Add("approvedate3");
                DataRow row = dt.NewRow();


                LifthoistjobEntity entity = lifthoistjobbll.GetEntity(keyValue);
                row["APPLYCODE"] = entity.APPLYCODESTR;
                row["CHARGEPERSONNAME"] = entity.CHARGEPERSONNAME;
                row["GUARDIANNAME"] = entity.GUARDIANNAME;
                row["QUALITY"] = entity.QUALITY;
                row["TOOLNAME"] = dataitemdetailbll.GetItemName("ToolName", entity.TOOLNAME);
                row["CONSTRUCTIONUNITNAME"] = entity.CONSTRUCTIONUNITNAME;
                row["CONSTRUCTIONADDRESS"] = entity.CONSTRUCTIONADDRESS;
                row["HOISTCONTENT"] = entity.HOISTCONTENT;
                row["WORKDATE"] = "自" + Convert.ToDateTime(entity.WORKSTARTDATE).ToString("yyyy年MM月dd日HH时mm分") + "到" + Convert.ToDateTime(entity.WORKENDDATE).ToString("yyyy年MM月dd日HH时mm分");
                row["APPLYCOMPANYNAME"] = entity.APPLYCOMPANYNAME;
                row["APPLYUSERNAME"] = entity.APPLYUSERNAME;
                row["APPLYDATE"] =Convert.ToDateTime(entity.APPLYDATE).ToString("yyyy-MM-dd HH:mm");

                IList<LifthoistauditrecordEntity> auditlist = lifthoistauditrecordbll.GetList(keyValue).Where(t => t.DISABLE != 1).OrderBy(t => t.AUDITDATE).ToList();
                for (int i = 0; i < auditlist.Count; i++)
                {
                    var filepath = auditlist[i].AUDITSIGNIMG == null ? Server.MapPath("~/content/Images/no_1.png") : (Server.MapPath("~/") + auditlist[i].AUDITSIGNIMG.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    if (!System.IO.File.Exists(filepath))
                    {
                        filepath = Server.MapPath("~/content/Images/no_1.png");
                    }
                    var stime =Convert.ToDateTime(auditlist[i].AUDITDATE).ToString("yyyy-MM-dd HH:mm");
                    switch (i)
                    {
                        case 0:
                            builder.MoveToMergeField("approve1");
                            builder.InsertImage(filepath, 80, 35);
                            row["approvedate1"] = stime;
                            break;
                        case 1:
                            builder.MoveToMergeField("approve2");
                            builder.InsertImage(filepath, 80, 35);
                            row["approvedate2"] = stime;
                            break;
                        case 2:
                            builder.MoveToMergeField("approve3");
                            builder.InsertImage(filepath, 80, 35);
                            row["approvedate3"] = stime;
                            break;
                        default:
                            break;
                    }
                }

                dt.Rows.Add(row);
                doc.MailMerge.Execute(dt);

                DataTable dtperson = new DataTable("U");
                dtperson.Columns.Add("BelongDeptName");
                dtperson.Columns.Add("CertificateNum");
                dtperson.Columns.Add("PersonName");
                dtperson.Columns.Add("PersonType");
                List<LifthoistpersonEntity> listperson = lifthoistpersonbll.GetRelateList(keyValue).ToList();
                foreach (var item in listperson)
                {
                    DataRow dtrow = dtperson.NewRow();
                    dtrow["BelongDeptName"] = item.BelongDeptName;
                    dtrow["CertificateNum"] = item.CertificateNum;
                    dtrow["PersonName"] = item.PersonName;
                    dtrow["PersonType"] = item.PersonName;
                    DataTable liftfazls = fileInfoBLL.GetFiles(item.Id);
                    if (liftfazls != null && liftfazls.Rows.Count > 0)
                    {
                        foreach (DataRow rowitem in liftfazls.Rows)
                        {
                            string image = "ico,gif,jpeg,jpg,png,psd";
                            if (image.Contains(rowitem["filepath"].ToString().Split('.')[1].ToLower()))
                            {
                                var filepath = rowitem["filepath"] == null ? Server.MapPath("~/content/Images/no_1.png") : (Server.MapPath("~/") + rowitem["filepath"].ToString().Replace("~/", "").ToString()).Replace(@"\/", "/").ToString();
                                if (!System.IO.File.Exists(filepath))
                                {
                                    filepath = Server.MapPath("~/content/Images/no_1.png");
                                }
                                builder.MoveToMergeField("lifthoistpersonfile");
                                builder.InsertImage(filepath, 80, 35);
                            }
                           
                        }
                    }
                    dtperson.Rows.Add(dtrow);
                }
                doc.MailMerge.ExecuteWithRegions(dtperson);
                doc.MailMerge.DeleteFields();
                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
                return Success("导出成功!");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        #endregion
    }
}
