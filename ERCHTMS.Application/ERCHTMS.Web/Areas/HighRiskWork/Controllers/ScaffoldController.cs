using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using System.Collections;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System;
using Newtonsoft.Json;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using System.Collections.Generic;
using System.Data;
using BSFramework.Util.Offices;
using Newtonsoft.Json.Linq;
using Aspose.Words;
using System.Web;
using Aspose.Words.Tables;
using System.Text.RegularExpressions;
using ERCHTMS.Entity.BaseManage;
using System.Linq;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：脚手架搭设、验收、拆除申请
    /// </summary>
    public class ScaffoldController : MvcControllerBase
    {
        ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        ScaffoldspecBLL scaffoldspecbll = new ScaffoldspecBLL();
        ScaffoldprojectBLL scaffoldprojectbll = new ScaffoldprojectBLL();
        ScaffoldauditrecordBLL scaffoldauditrecordbll = new ScaffoldauditrecordBLL();


        DataItemBLL dataitembll = new DataItemBLL();
        DepartmentBLL departmentbll = new DepartmentBLL();
        FireWaterBLL firewaterbll = new FireWaterBLL();
        FileInfoBLL fileinfobll = new FileInfoBLL();
        DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

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
        /// 脚手架流程图页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }
        /// <summary>
        /// 台账
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Ledger()
        {
            return View();
        }
        /// <summary>
        /// 搭设申请表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormOne(string keyValue, int scaffoldtype)
        {
            return View();
        }

        /// <summary>
        /// 验收申请表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormTwo(string keyValue, int scaffoldtype)
        {
            return View();
        }
        /// <summary>
        /// 拆除申请表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormThree(string keyValue, int scaffoldtype)
        {
            return View();
        }

        /// <summary>
        /// 架体规格及形式
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectSpec()
        {
            var o = dataitembll.GetList("ScaffoldSpec");
            ViewBag.DataItems = o;
            return View();
        }

        /// <summary>
        /// 选择脚手架信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectScaffold()
        {
            return View();
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
                pagination.conditionJson = " 1=1 ";

                #region 数据权限
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //查看范围数据权限
                /**
                 * 1.作业单位及子部门（下级）
                 * 2.本人创建的高风险作业
                 * 3.发包部门管辖的外包单位
                 * 4.外包单位只能看本单位的
                 * */
                if (!user.IsSystem)
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        string isAllDataRange = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetEnableItemValue("HighRiskWorkDataRange"); //特殊标记，高风险作业模块是否看全厂数据
                        if (!string.IsNullOrWhiteSpace(isAllDataRange))
                        {
                            pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                        }
                        else
                        {
                            switch (authType)
                            {
                                case "1":
                                    pagination.conditionJson += " and a.applyuserid='" + user.UserId + "'";
                                    break;
                                case "2":
                                    pagination.conditionJson += " and a.setupcompanyid='" + user.DeptId + "'";
                                    break;
                                case "3"://本子部门
                                    pagination.conditionJson += string.Format(" and ((a.setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                                    break;
                                case "4":
                                    pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                #endregion

                var data = scaffoldbll.GetList(pagination, queryJson);
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
                pagination.conditionJson = " 1=1 ";

                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                var data = scaffoldbll.GetLedgerList(pagination, queryJson, authType);
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
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.conditionJson = " 1=1 ";
                pagination.page = 1;
                pagination.rows = 1000000000;
                #region 数据权限
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //查看范围数据权限
                /**
                 * 1.作业单位及子部门（下级）
                 * 2.本人创建的高风险作业
                 * 3.发包部门管辖的外包单位
                 * 4.外包单位只能看本单位的
                 * */
                if (!user.IsSystem)
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        string isAllDataRange = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetEnableItemValue("HighRiskWorkDataRange"); //特殊标记，高风险作业模块是否看全厂数据
                        if (!string.IsNullOrWhiteSpace(isAllDataRange))
                        {
                            pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                        }
                        {
                            switch (authType)
                            {
                                case "1":
                                    pagination.conditionJson += " and a.applyuserid='" + user.UserId + "'";
                                    break;
                                case "2":
                                    pagination.conditionJson += " and a.setupcompanyid='" + user.DeptId + "'";
                                    break;
                                case "3"://本子部门
                                    pagination.conditionJson += string.Format("  and ((a.setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                                    break;
                                case "4":
                                    pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                                    break;
                            }
                        }
                        
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                #endregion

                var data = scaffoldbll.GetList(pagination, queryJson);
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("auditstatestr"));
                excelTable.Columns.Add(new DataColumn("applycode"));
                excelTable.Columns.Add(new DataColumn("outprojectname"));
                excelTable.Columns.Add(new DataColumn("setupcompanytypestr"));
                excelTable.Columns.Add(new DataColumn("setupcompanyname"));
                excelTable.Columns.Add(new DataColumn("dismentledate"));
                excelTable.Columns.Add(new DataColumn("setupdate"));
                excelTable.Columns.Add(new DataColumn("applyusername"));
                excelTable.Columns.Add(new DataColumn("applydate"));
                excelTable.Columns.Add(new DataColumn("flowdeptname"));
                excelTable.Columns.Add(new DataColumn("flowname"));
                foreach (DataRow item in data.Rows)
                {
                    int state = 0;
                    int.TryParse(item["auditstate"].ToString(), out state);
                    int setupcompanytype = 0;
                    int.TryParse(item["setupcompanytype"].ToString(), out setupcompanytype);
                    DataRow newDr = excelTable.NewRow();
                    //搭设申请、拆除申请作业状态
                    if (state == 0)
                    {
                        newDr["auditstatestr"] = "申请中";
                    }
                    if (state == 1)
                    {
                        newDr["auditstatestr"] = "审核(批)中";
                    }
                    if (state == 2)
                    {
                        newDr["auditstatestr"] = "审核(批)未通过";
                    }
                    if (state == 3)
                    {
                        newDr["auditstatestr"] = "审核(批)通过";
                    }
                    if (state == 4)
                    {
                        newDr["auditstatestr"] = "验收中";
                    }
                    if (state == 5)
                    {
                        newDr["auditstatestr"] = "验收未通过";
                    }
                    if (state == 6)
                    {
                        newDr["auditstatestr"] = "验收通过";
                    }
                    if (setupcompanytype == 0)
                    {
                        newDr["setupcompanytypestr"] = "单位内部";
                    }
                    else
                    {
                        newDr["setupcompanytypestr"] = "外包单位";
                    }
                    newDr["applycode"] = item["applycode"];
                    newDr["outprojectname"] = item["outprojectname"];
                    newDr["setupcompanyname"] = item["setupcompanyname"];
                    DateTime setupstartdate, setupenddate, dismentlestartdate, dismentleenddate, applydate;

                    DateTime.TryParse(item["setupstartdate"].ToString(), out setupstartdate);
                    DateTime.TryParse(item["setupenddate"].ToString(), out setupenddate);
                    DateTime.TryParse(item["dismentlestartdate"].ToString(), out dismentlestartdate);
                    DateTime.TryParse(item["dismentleenddate"].ToString(), out dismentleenddate);
                    DateTime.TryParse(item["applydate"].ToString(), out applydate);
                    newDr["setupdate"] = setupstartdate.ToString("yyyy-MM-dd HH:mm") + "-" + setupstartdate.ToString("yyyy-MM-dd HH:mm");
                    newDr["dismentledate"] = dismentlestartdate.ToString("yyyy-MM-dd HH:mm") + "-" + dismentleenddate.ToString("yyyy-MM-dd HH:mm");
                    newDr["applyusername"] = item["applyusername"];
                    newDr["applydate"] = applydate.ToString("yyyy-MM-dd HH:mm");
                    newDr["flowdeptname"] = item["flowdeptname"];
                    newDr["flowname"] = item["flowname"];

                    excelTable.Rows.Add(newDr);
                }
                var query = JObject.Parse(queryJson);

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                if (query["ScaffoldType"].ToString() == "0")
                {
                    excelconfig.Title = "搭设申请信息";
                    excelconfig.FileName = "搭设申请信息导出.xls";
                }
                if (query["ScaffoldType"].ToString() == "1")
                {
                    excelconfig.Title = "验收申请信息";
                    excelconfig.FileName = "验收申请信息导出.xls";
                }
                if (query["ScaffoldType"].ToString() == "2")
                {
                    excelconfig.Title = "拆除申请信息";
                    excelconfig.FileName = "拆除申请信息导出.xls";
                }
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "auditstatestr", ExcelColumn = "作业状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applycode", ExcelColumn = "申请编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "outprojectname", ExcelColumn = "工程名称", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "setupcompanytypestr", ExcelColumn = "单位类别", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "setupcompanyname", ExcelColumn = "使用单位", Width = 10 });
                if (query["ScaffoldType"].ToString() == "2")
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dismentledate", ExcelColumn = "拆除时间", Width = 10 });
                }
                else
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "setupdate", ExcelColumn = "搭设时间", Width = 10 });
                }
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applyusername", ExcelColumn = "申请人", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applydate", ExcelColumn = "申请时间", Width = 12 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "flowdeptname", ExcelColumn = "审核部门", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "flowname", ExcelColumn = "审核流程", Width = 15 });
                //调用导出方法
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }

        /// <summary>
        /// 导出台账
        /// </summary>
        [HandlerMonitor(0, "导出台账数据")]
        public ActionResult ExportLedgerData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.conditionJson = " 1=1 ";
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式

                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");

                var data = scaffoldbll.GetLedgerList(pagination, queryJson, authType);
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("ledgertypestr"));
                excelTable.Columns.Add(new DataColumn("outprojectname"));
                excelTable.Columns.Add(new DataColumn("setupcompanyname"));
                excelTable.Columns.Add(new DataColumn("setupdate"));
                excelTable.Columns.Add(new DataColumn("actsetupdate"));
                excelTable.Columns.Add(new DataColumn("checkdate"));
                excelTable.Columns.Add(new DataColumn("dismentlecompanyname"));
                excelTable.Columns.Add(new DataColumn("dismentledate"));
                excelTable.Columns.Add(new DataColumn("realitydismentledate"));
                excelTable.Columns.Add(new DataColumn("setupaddress"));

                foreach (DataRow item in data.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    newDr["ledgertypestr"] = item["ledgertypename"];
                    newDr["outprojectname"] = item["outprojectname"];
                    newDr["setupcompanyname"] = item["setupcompanyname"];
                    newDr["dismentlecompanyname"] = item["dismentlecompanyname"];

                    DateTime actsetupstartdate, actsetupenddate, dismentlestartdate, dismentleenddate, checkdate, setupstartdate, setupenddate, realitydismentlestartdate, realitydismentleenddate;
                    DateTime.TryParse(item["setupstartdate"].ToString(), out setupstartdate);
                    DateTime.TryParse(item["setupenddate"].ToString(), out setupenddate);
                    DateTime.TryParse(item["actsetupstartdate"].ToString(), out actsetupstartdate);
                    DateTime.TryParse(item["actsetupenddate"].ToString(), out actsetupenddate);
                    DateTime.TryParse(item["dismentlestartdate"].ToString(), out dismentlestartdate);
                    DateTime.TryParse(item["dismentleenddate"].ToString(), out dismentleenddate);
                    DateTime.TryParse(item["checkdate"].ToString(), out checkdate);
                    DateTime.TryParse(item["realitydismentlestartdate"].ToString(), out realitydismentlestartdate);
                    DateTime.TryParse(item["realitydismentleenddate"].ToString(), out realitydismentleenddate);

                    string dismentledate = string.Empty;
                    string setupdate = string.Empty;
                    string actsetupdate = string.Empty;
                    string realitydismentledate = string.Empty;
                    if (dismentlestartdate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        dismentledate += dismentlestartdate.ToString("yyyy-MM-dd HH:mm") + " - ";
                    }
                    if (dismentleenddate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        dismentledate += dismentleenddate.ToString("yyyy-MM-dd HH:mm");
                    }
                    if (setupstartdate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        setupdate += setupstartdate.ToString("yyyy-MM-dd HH:mm") + " - ";
                    }
                    if (setupenddate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        setupdate += setupenddate.ToString("yyyy-MM-dd HH:mm");
                    }
                    if (actsetupstartdate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        actsetupdate += actsetupstartdate.ToString("yyyy-MM-dd HH:mm") + " - ";
                    }
                    if (actsetupenddate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        actsetupdate += actsetupenddate.ToString("yyyy-MM-dd HH:mm");
                    }
                    if (realitydismentlestartdate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        realitydismentledate += realitydismentlestartdate.ToString("yyyy-MM-dd HH:mm") + " - ";
                    }
                    if (realitydismentleenddate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00")
                    {
                        realitydismentledate += realitydismentleenddate.ToString("yyyy-MM-dd HH:mm");
                    }
                    newDr["setupdate"] = setupdate;
                    newDr["dismentledate"] = dismentledate;
                    newDr["actsetupdate"] = actsetupdate;
                    newDr["checkdate"] = checkdate.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00" ? checkdate.ToString("yyyy-MM-dd HH:mm") : "";
                    newDr["realitydismentledate"] = realitydismentledate;
                    newDr["setupaddress"] = item["setupaddress"];
                    excelTable.Rows.Add(newDr);
                }
                var query = JObject.Parse(queryJson);

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "脚手架台账";
                excelconfig.FileName = "脚手架台账.xls";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ledgertypestr", ExcelColumn = "使用状态", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "outprojectname", ExcelColumn = "工程名称", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "setupcompanyname", ExcelColumn = "使用单位", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "setupdate", ExcelColumn = "申请搭设时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "actsetupdate", ExcelColumn = "实际搭设时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdate", ExcelColumn = "验收时间", Width = 12 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dismentlecompanyname", ExcelColumn = "拆除单位", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dismentledate", ExcelColumn = "申请拆除时间", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realitydismentledate", ExcelColumn = "实际拆除时间", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "setupaddress", ExcelColumn = "作业地点", Width = 15 });
                //调用导出方法
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {
            }
            return Success("导出成功。");
        }
        /// <summary>
        /// 导出模版 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportDoc(string keyValue)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //取信息数据
                ScaffoldEntity scaffoldEntity = scaffoldbll.GetEntity(keyValue);
                if (scaffoldEntity == null)
                {
                    return Error("无法获取信息，请检查参数是否正确");
                }
                string tempFileName = "";
                //导出文件名称
                string fileName = "";
                if (scaffoldEntity.ScaffoldType == 0)
                {
                    tempFileName = "脚手架搭设申请单_导出模板.docx";
                    fileName = "脚手架搭设申请单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                }
                if (scaffoldEntity.ScaffoldType == 1)
                {
                    tempFileName = "脚手架搭设验收表_导出模板.docx";
                    fileName = "脚手架搭设验收表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                }
                if (scaffoldEntity.ScaffoldType == 2)
                {
                    tempFileName = "脚手架拆除申请表_导出模板.docx";
                    fileName = "脚手架拆除申请表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                }
                //模板文件路径
                string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/" + tempFileName);
                //读取模板文件
                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                HttpResponse resp = System.Web.HttpContext.Current.Response;

                DocumentBuilder docBuilder = new DocumentBuilder(doc);

                string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
                #region 搭设申请架体形式
                if (scaffoldEntity.ScaffoldType == 0)
                {
                    //插入架体规格
                    List<ScaffoldspecEntity> scaffoldSpecs = scaffoldspecbll.GetList(scaffoldEntity.Id);
                    DataTable dt = new DataTable("a");
                    dt.Columns.Add("specindex");
                    dt.Columns.Add("slength");
                    dt.Columns.Add("swidth");
                    dt.Columns.Add("shigh");
                    dt.Columns.Add("lineone");
                    dt.Columns.Add("linetwo");
                    dt.Columns.Add("linethree");
                    dt.Columns.Add("linefour");
                    dt.Columns.Add("linefive");
                    dt.Columns.Add("linesix");
                    dt.Columns.Add("lineseven");
                    if (scaffoldSpecs != null && scaffoldSpecs.Count > 0)
                    {
                        int specRowIndex = 1;
                        foreach (var spec in scaffoldSpecs)
                        {
                            DataRow dr = dt.NewRow();
                            dr["specindex"] = specRowIndex;
                            dr["slength"] = spec.SLength;
                            dr["swidth"] = spec.SWidth;
                            dr["shigh"] = spec.SHigh;
                            dr["lineone"] = spec.SFrameName.Contains("悬吊") == true ? "☑" : "□";
                            dr["linetwo"] = spec.SFrameName.Contains("悬挑") == true ? "☑" : "□";
                            dr["linethree"] = spec.SFrameName.Contains("井字") == true ? "☑" : "□";
                            dr["linefour"] = spec.SFrameName.Contains("承重") == true ? "☑" : "□";
                            dr["linefive"] = spec.SFrameName.Contains("单排") == true ? "☑" : "□";
                            dr["linesix"] = spec.SFrameName.Contains("双排") == true ? "☑" : "□";
                            dr["lineseven"] = spec.SFrameName.Contains("其他") == true ? "☑" : "□";
                            dt.Rows.Add(dr);
                            specRowIndex++;
                        }
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);
                    }
                    docBuilder.MoveToMergeField("hrline");
                    docBuilder.InsertHtml("<hr style=\"width:100%;height:1px;color:black;background:black\"/>");
                    docBuilder.Document.MailMerge.ExecuteWithRegions(dt);

                }
                #endregion

                #region 验收申请验收项目
                if (scaffoldEntity.ScaffoldType == 1)
                {
                    List<ScaffoldprojectEntity> scaffoldPropjects = scaffoldprojectbll.GetList(scaffoldEntity.Id);
                    DataTable dt = new DataTable("a");
                    dt.Columns.Add("proindex");
                    dt.Columns.Add("projectname");
                    dt.Columns.Add("resultyes");
                    dt.Columns.Add("resultno");
                    dt.Columns.Add("checkperson");

                    if (scaffoldPropjects != null && scaffoldPropjects.Count > 0)
                    {
                        int rowIndex = 1;
                        foreach (var pro in scaffoldPropjects)
                        {
                            DataRow dr = dt.NewRow();
                            dr["proindex"] = rowIndex;
                            dr["projectname"] = pro.ProjectName;
                            if (pro.Result == "0")
                            {
                                dr["resultno"] = "☑";
                                dr["resultyes"] = "□";
                            }
                            else
                            {
                                dr["resultyes"] = "☑";
                                dr["resultno"] = "□";
                            }
                            //dr["checkperson"] = pro.CheckPersons;
                            dr["checkperson"] = pro.SignPic == "" ? Server.MapPath("~/content/Images/no_1.png") : (Server.MapPath("~/") + pro.SignPic.Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                            dt.Rows.Add(dr);
                            rowIndex++;
                        }
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);
                    }
                    docBuilder.Document.MailMerge.ExecuteWithRegions(dt);
                }
                #endregion

                string formatDateYear = "yyyy年MM月dd日";
                string formatDateStr = "yyyy年MM月dd日HH时mm分";

                Dictionary<string, string> di_values = new Dictionary<string, string>();
                di_values.Add("projectname", scaffoldEntity.OutProjectName);    //项目名称 
                di_values.Add("setupcompanyname", scaffoldEntity.SetupCompanyName); //搭设单位
                di_values.Add("applyusername", scaffoldEntity.ApplyUserName);       //申请人
                di_values.Add("applycode", scaffoldEntity.ApplyCode);               //申请编号
                di_values.Add("applydate", DateTime.Parse(scaffoldEntity.ApplyDate).ToString(formatDateStr)); //申请时间

                #region 搭设申请
                if (scaffoldEntity.ScaffoldType == 0)
                {
                    di_values.Add("setupstartdate", scaffoldEntity.SetupStartDate.Value.ToString(formatDateStr)); //搭设开始时间
                    di_values.Add("setupenddate", scaffoldEntity.SetupEndDate.Value.ToString(formatDateStr));     //搭设结束时间
                    di_values.Add("purpose", scaffoldEntity.Purpose);                                             //用途
                    di_values.Add("expectdismentledate", scaffoldEntity.ExpectDismentleDate.HasValue ? scaffoldEntity.ExpectDismentleDate.Value.ToString(formatDateStr) : ""); //预计拆除时间 
                    di_values.Add("dimanddismengledate", scaffoldEntity.DemandDismentleDate.HasValue ? scaffoldEntity.DemandDismentleDate.Value.ToString(formatDateStr) : ""); //实际拆除时间
                }
                #endregion

                #region 验收申请
                if (scaffoldEntity.ScaffoldType == 1)
                {
                    di_values.Add("purpose", scaffoldEntity.Purpose);                                             //用途
                    di_values.Add("setupchargeperson", scaffoldEntity.SetupChargePerson);                         //搭设负责人
                    di_values.Add("actsetupstartdate", scaffoldEntity.ActSetupStartDate.Value.ToString(formatDateStr)); //实际搭设开始时间 
                    di_values.Add("actsetupenddate", scaffoldEntity.ActSetupEndDate.Value.ToString(formatDateStr));     //实际搭设结束时间
                    //脚手架形式
                    List<ScaffoldspecEntity> scaffoldSpecs = scaffoldspecbll.GetList(scaffoldEntity.Id);
                    string sframenames = string.Empty;
                    string setuptype = string.Empty;
                    if (scaffoldSpecs != null && scaffoldSpecs.Count > 0)
                    {
                        sframenames = string.Join(";", scaffoldSpecs.Select(x => x.SFrameName));
                        foreach (var item in scaffoldSpecs)
                        {
                            setuptype += item.SHigh + "米;";
                        }
                        if (!string.IsNullOrEmpty(setuptype))
                            setuptype = setuptype.TrimEnd(';');
                    }
                    di_values.Add("sframenames", sframenames);
                    //搭设高度
                    di_values.Add("setuptype", setuptype);


                }
                #endregion

                #region 拆除申请
                if (scaffoldEntity.ScaffoldType == 2)
                {
                    //脚手架形式
                    List<ScaffoldspecEntity> scaffoldSpecs = scaffoldspecbll.GetList(scaffoldEntity.Id);
                    string sframenames = string.Empty;
                    if (scaffoldSpecs != null && scaffoldSpecs.Count > 0)
                    {
                        foreach (var item in scaffoldSpecs)
                        {
                            sframenames += "，" + item.SFrameName;
                        }
                        sframenames = sframenames.Substring(1);
                    }
                    di_values.Add("sframenames", sframenames);
                    di_values.Add("sframematerial", scaffoldEntity.FrameMaterial);
                    di_values.Add("dismentlepart", scaffoldEntity.DismentlePart);
                    di_values.Add("dismentlerason", scaffoldEntity.DismentleReason);
                    di_values.Add("measureplan", scaffoldEntity.MeasurePlan);
                    di_values.Add("measurecarryout", scaffoldEntity.MeasureCarryout);
                }
                #endregion


                DataTable dtscaff = new DataTable();
                dtscaff.Columns.Add("setupcompanzg");
                dtscaff.Columns.Add("setupcompanyaqfzr");
                dtscaff.Columns.Add("setupcompanyjl");
                dtscaff.Columns.Add("fbcompanyaqgly");
                dtscaff.Columns.Add("fbcompanyfzr");
                dtscaff.Columns.Add("sjbaudituser");
                dtscaff.Columns.Add("ahbaudituser");

                DataRow scaffrow = dtscaff.NewRow();
                #region 搭设单位
                DepartmentEntity department = departmentbll.GetEntity(scaffoldEntity.SetupCompanyId);
                List<ScaffoldauditrecordEntity> auditlist = scaffoldauditrecordbll.GetEntitys(scaffoldEntity.Id, department.EnCode).ToList();
                string zgcheckdata = "", fzrcheckdate = "", jlcheckdate = "";
                if (department.Nature == "班组")
                {
                    if (auditlist == null || auditlist.Count() == 0)
                    {
                        department = departmentbll.GetEntity(department.ParentId);
                        auditlist = scaffoldauditrecordbll.GetEntitys(scaffoldEntity.Id, department.EnCode).ToList();
                    }
                }
                if (auditlist != null && auditlist.Count() > 0)
                {
                    for (int i = 0; i < auditlist.Count(); i++)
                    {
                        var filepath = auditlist[i].AuditSignImg == null ? "" : (Server.MapPath("~/") + auditlist[i].AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (i == 0)
                        {
                            //zg = auditlist[0].AuditUserName;
                            if (System.IO.File.Exists(filepath))
                            {
                                scaffrow["setupcompanzg"] = filepath;
                            }
                            else
                            {
                                scaffrow["setupcompanzg"] = pic;
                            }
                            zgcheckdata = auditlist[0].AuditDate.Value.ToString(formatDateYear);
                        }
                        else if (i == 1)
                        {
                            //fzr = auditlist[1].AuditUserName;
                            if (System.IO.File.Exists(filepath))
                            {
                                scaffrow["setupcompanyaqfzr"] = filepath;
                            }
                            else
                            {
                                scaffrow["setupcompanyaqfzr"] = pic;
                            }
                            fzrcheckdate = auditlist[1].AuditDate.Value.ToString(formatDateYear);

                        }
                        else if (i == 2)
                        {
                            //jl = auditlist[2].AuditUserName;
                            if (System.IO.File.Exists(filepath))
                            {
                                scaffrow["setupcompanyjl"] = filepath;
                            }
                            else
                            {
                                scaffrow["setupcompanyjl"] = pic;
                            }
                            jlcheckdate = auditlist[2].AuditDate.Value.ToString(formatDateYear);
                        }
                    }
                }
                di_values.Add("setupcompanzgcheckdate", zgcheckdata);
                di_values.Add("setupcompanyaqcheckdate", fzrcheckdate);
                di_values.Add("setupcompanyjlcheckdate", jlcheckdate);
                #endregion

                #region 发包单位
                if (scaffoldEntity.SetupCompanyType == 1)
                {
                    DepartmentEntity departEntity = departmentbll.GetEntity(new OutsouringengineerBLL().GetEntity(scaffoldEntity.OutProjectId).ENGINEERLETDEPTID);

                    IEnumerable<ScaffoldauditrecordEntity> auditEntityFbfzrs = scaffoldauditrecordbll.GetEntitys(scaffoldEntity.Id, departEntity != null ? departEntity.EnCode : "");
                    if (auditEntityFbfzrs != null && auditEntityFbfzrs.Count() > 0)
                    {
                        ScaffoldauditrecordEntity auditEntityFbfzr = auditEntityFbfzrs.FirstOrDefault();

                        var filepath = auditEntityFbfzr.AuditSignImg == null ? "" : (Server.MapPath("~/") + auditEntityFbfzr.AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            scaffrow["fbcompanyfzr"] = filepath;
                        }
                        else
                        {
                            scaffrow["fbcompanyfzr"] = pic;
                        }
                        //发包单位 单位内部时无，外包单位时取发包单位
                        di_values.Add("fbcompanyfzrremark", auditEntityFbfzr != null ? auditEntityFbfzr.AuditRemark : "");              //发包单位负责人审核意见
                        di_values.Add("fbcompanyfzrcheckdate", auditEntityFbfzr != null ? auditEntityFbfzr.AuditDate.Value.ToString(formatDateYear) : ""); //发包单位审核时间 

                        ScaffoldauditrecordEntity auditEntityFbaqgly = auditEntityFbfzrs.LastOrDefault();

                        var filepathgly = auditEntityFbaqgly.AuditSignImg == null ? "" : (Server.MapPath("~/") + auditEntityFbaqgly.AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        if (System.IO.File.Exists(filepathgly))
                        {
                            scaffrow["fbcompanyaqgly"] = filepathgly;
                        }
                        else
                        {
                            scaffrow["fbcompanyaqgly"] = pic;
                        }
                        //发包单位的负责人或安全管理员
                        di_values.Add("fbcompanyaqglyremark", auditEntityFbaqgly != null ? auditEntityFbaqgly.AuditRemark : (auditEntityFbaqgly != null ? auditEntityFbaqgly.AuditRemark : ""));
                        di_values.Add("fbcompanyaqglycheckdate", auditEntityFbaqgly != null ? auditEntityFbaqgly.AuditDate.Value.ToString(formatDateYear) : ""); //发包单位审核时间 
                    }
                    else
                    {
                        di_values.Add("fbcompanyfzrremark", "");
                        di_values.Add("fbcompanyfzrcheckdate", "");
                        di_values.Add("fbcompanyaqglyremark", "");
                        di_values.Add("fbcompanyaqglycheckdate", "");
                    }
                }
                else
                {
                    di_values.Add("fbcompanyfzrremark", "");
                    di_values.Add("fbcompanyfzrcheckdate", "");
                    di_values.Add("fbcompanyaqglyremark", "");
                    di_values.Add("fbcompanyaqglycheckdate", "");
                }
                #endregion

                #region 生技部和安环部
                var itemdetail = new DataItemDetailBLL().GetItemValue(user.OrganizeCode, "config");
                string[] strarr = itemdetail.Split(',');
                var sjentity = departmentbll.GetEntity(strarr[0]);//生技部
                var aqentity = departmentbll.GetEntity(strarr[1]);//安环部
                ScaffoldauditrecordEntity auditEntitySjbfzr = scaffoldauditrecordbll.GetEntitys(scaffoldEntity.Id, sjentity != null ? sjentity.EnCode : "").FirstOrDefault();
                ScaffoldauditrecordEntity auditEntityAhbfzr = scaffoldauditrecordbll.GetEntitys(scaffoldEntity.Id, aqentity != null ? aqentity.EnCode : "").FirstOrDefault();
                if (auditEntitySjbfzr != null)
                {

                    var filepath = auditEntitySjbfzr.AuditSignImg == null ? "" : (Server.MapPath("~/") + auditEntitySjbfzr.AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        scaffrow["sjbaudituser"] = filepath;
                    }
                    else
                    {
                        scaffrow["sjbaudituser"] = pic;
                    }
                    di_values.Add("sjbauditremark", auditEntitySjbfzr.AuditRemark);    //生技部审核意见
                    di_values.Add("sjbauditdate", auditEntitySjbfzr.AuditDate.Value.ToString(formatDateYear));   //生技部审核时间 
                }
                else
                {
                    di_values.Add("sjbauditremark", "");
                    di_values.Add("sjbauditdate", "");
                }
                if (auditEntityAhbfzr != null)
                {
                    var filepath = auditEntityAhbfzr.AuditSignImg == null ? "" : (Server.MapPath("~/") + auditEntityAhbfzr.AuditSignImg.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        scaffrow["ahbaudituser"] = filepath;
                    }
                    else
                    {
                        scaffrow["ahbaudituser"] = pic;
                    }
                    di_values.Add("ahbauditremark", auditEntityAhbfzr.AuditRemark);    //安环部审核意见
                    di_values.Add("ahbauditdate", auditEntityAhbfzr.AuditDate.Value.ToString(formatDateYear)); //安环部审核时间
                }
                else
                {
                    di_values.Add("ahbauditremark", "");
                    di_values.Add("ahbauditdate", "");
                }
                #endregion

                string[] keys = di_values.Keys.ToArray();
                string[] values = di_values.Values.ToArray();
                docBuilder.Document.MailMerge.Execute(keys, values);
                dtscaff.Rows.Add(scaffrow);
                docBuilder.Document.MailMerge.Execute(dtscaff);

                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
                return Success("导出成功!");
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
            if (string.IsNullOrEmpty(keyValue))
            {
                return Error("缺少参数：keyValue不为能空");
            }
            var data = scaffoldbll.GetEntity(keyValue);
            if (data.WorkOperate == "1")
            {
                IList<FireWaterCondition> conditionlist = firewaterbll.GetConditionList(keyValue).OrderBy(t => t.CreateDate).ToList();
                data.ActSetupStartDate = conditionlist.FirstOrDefault() == null ? null : conditionlist.FirstOrDefault().ConditionTime;
                var endentity = conditionlist.Where(t => t.LedgerType == "1").OrderBy(t => t.CreateDate).LastOrDefault();
                data.ActSetupEndDate = endentity == null ? null : endentity.ConditionTime;
            }
            if (data == null)
            {
                return Error("未找到信息");
            }
            string jsondata = JsonConvert.SerializeObject(data);
            ScaffoldModel model = JsonConvert.DeserializeObject<ScaffoldModel>(jsondata);
            model.ScaffoldSpecs = scaffoldspecbll.GetList(data.Id);
            model.ScaffoldProjects = scaffoldprojectbll.GetList(data.Id);
            model.ScaffoldAudits = scaffoldauditrecordbll.GetList(data.Id);

            return ToJsonResult(model);
        }

        /// <summary>
        /// 跟据脚手架信息ID得到架体规格及形式数据
        /// </summary>
        /// <param name="scaffoldid"></param>
        /// <returns></returns>
        public ActionResult GetScaffSpecToJson(string scaffoldid)
        {
            var data = scaffoldspecbll.GetList(scaffoldid);
            return ToJsonResult(data);
        }

        /// <summary>
        ///获取专业
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSpecialtyToJson(string deptid, string specialtytype, string workdepttype)
        {
            try
            {
                var data = scaffoldbll.GetSpecialtyToJson(deptid, specialtytype, workdepttype);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error("错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 跟据脚手架信息ID得到审核记录
        /// </summary>
        /// <param name="scaffoldid"></param>
        /// <returns></returns>
        public ActionResult GetScaffAuditToJson(string scaffoldid)
        {
            if (!string.IsNullOrEmpty(scaffoldid))
            {
                var data = scaffoldauditrecordbll.GetList(scaffoldid).Where(t => t.AuditRemark != "确认step");
                return ToJsonResult(data);
            }
            return ToJsonResult(null);
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

        #region 获取脚手架流程图对象
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetScaffoldFlow(string keyValue)
        {
            try
            {
                ScaffoldEntity scaffoldEntity = scaffoldbll.GetEntity(keyValue);
                string modulename = string.Empty;
                if (scaffoldEntity.ScaffoldType == 0)
                {
                    if (scaffoldEntity.SetupCompanyType == 0)
                    {
                        modulename = "(搭设申请-内部-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                    else
                    {
                        modulename = "(搭设申请-外包-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                }
                if (scaffoldEntity.ScaffoldType == 1)
                {
                    if (scaffoldEntity.SetupCompanyType == 0)
                    {
                        modulename = "(搭设验收-内部-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                    else
                    {
                        modulename = "(搭设验收-外包-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                }
                if (scaffoldEntity.ScaffoldType == 2)
                {
                    if (scaffoldEntity.SetupCompanyType == 0)
                    {
                        modulename = "(搭设拆除-内部-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                    else
                    {
                        modulename = "(搭设拆除-外包-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                }
                var josnData = scaffoldbll.GetFlow(scaffoldEntity.Id, modulename);
                return Content(josnData.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        #endregion

        /// <summary>
        /// 获取脚手架类型与高度比较
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetScaffodTypeName(string ScaffodSetupType, double high)
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                IList<DataItemModel> dataitemlist = dataitemdetailbll.GetDataItemByDetailValue("ScaffoldType", ScaffodSetupType).ToList();
                if (dataitemlist.Count > 0)
                {
                    string remark = dataitemlist[0].ItemCode;
                    if (!string.IsNullOrEmpty(remark))
                    {
                        string[] higherlist = remark.Split('-');
                        if (high >= Convert.ToDouble(higherlist[0]) && high < Convert.ToDouble(higherlist[1]))
                        {
                            return Success("验证通过");
                        }
                        else
                        {
                            return Error("高度应大于" + higherlist[0] + "或小于" + higherlist[1] + "m");
                        }
                    }
                    else
                    {
                        return Error("获取数据失败");
                    }
                }
                else
                {
                    return Error("获取数据失败");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
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
        [HandlerMonitor(6, "脚手架删除")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            try
            {
                scaffoldbll.RemoveForm(keyValue);
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
        [HandlerMonitor(5, "脚手架保存")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string jsonData)
        {
            try
            {
                ScaffoldModel model = JsonConvert.DeserializeObject<ScaffoldModel>(jsonData);
                scaffoldbll.SaveForm(keyValue, model);
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
        [HandlerMonitor(8, "脚手架提交")]
        [AjaxOnly]
        public ActionResult AuditSubmit(string keyValue, string jsonData)
        {
            try
            {

                var requestParam = JsonConvert.DeserializeAnonymousType(jsonData, new
                {
                    auditEntity = new ScaffoldauditrecordEntity(),
                    projects = new List<ScaffoldprojectEntity>(),
                    checktype = 0,
                });
                if (requestParam.auditEntity == null)
                {
                    return Error("审核出错，错误信息：参数为null");
                }
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                ScaffoldEntity scaffoldEntity = scaffoldbll.GetEntity(keyValue);
                if (requestParam.projects != null)
                {
                    foreach (var item in requestParam.projects)
                    {
                        item.SignPic = string.IsNullOrWhiteSpace(item.SignPic) ? "" : item.SignPic.Replace("../..", "");
                    }
                }
                if (scaffoldEntity.AuditState == 4 && user.RoleName.Contains("专工"))
                {
                    //验收照片
                    string acceptFileId = keyValue + "_acceptfileid";
                    scaffoldEntity.AcceptFileId = acceptFileId;
                    scaffoldbll.SaveForm(keyValue, scaffoldEntity);
                }
                scaffoldbll.ApplyCheck(keyValue, requestParam.auditEntity, requestParam.projects, requestParam.checktype);

            }
            catch (Exception ex)
            {
                return Error("审核出错，错误信息：" + ex.Message);
            }

            return Success("提交成功。");
        }

        /// <summary>
        /// 验收项目保存
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "脚手架验收项目保存")]
        [AjaxOnly]
        public ActionResult AuditSave(string keyValue, string jsonData)
        {
            try
            {

                List<ScaffoldprojectEntity> projects = JsonConvert.DeserializeObject<List<ScaffoldprojectEntity>>(jsonData);

                if (projects == null)
                {
                    return Error("保存出错，错误信息：参数为null");
                }
                if (projects.Count > 0)
                {
                    int m = 0;
                    foreach (var item in projects)
                    {
                        item.SignPic = string.IsNullOrWhiteSpace(item.SignPic) ? "" : item.SignPic.Replace("../..", "");
                        item.CreateDate = DateTime.Now.AddSeconds(0);
                        scaffoldprojectbll.SaveForm(item.Id, item);
                        m++;
                    }
                    //验收照片
                    string acceptFileId = keyValue + "_acceptfileid";
                    ScaffoldEntity scaffoldEntity = scaffoldbll.GetEntity(keyValue);
                    scaffoldEntity.AcceptFileId = acceptFileId;
                    scaffoldbll.SaveForm(keyValue, scaffoldEntity);
                }
            }
            catch (Exception ex)
            {
                return Error("审核出错，错误信息：" + ex.Message);
            }

            return Success("提交成功。");
        }

        /// <summary>
        /// 删除架体规格数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(6, "架体规格数据删除")]
        [AjaxOnly]
        public ActionResult RemoveScaffoldSpec(string keyValue)
        {
            scaffoldspecbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        /// <summary>
        /// 台账状态操作
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ledgerType"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[HandlerMonitor(5, "高风险台账操作")]
        //[AjaxOnly]
        //public ActionResult LedgerOp(string keyValue, string ledgerType, string type, string worktime, string issendmessage,string conditioncontent,string conditionid)
        //{
        //    try
        //    {
        //        scaffoldbll.LedgerOp(keyValue, ledgerType, type, worktime, issendmessage, conditioncontent, conditionid);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Success("操作失败，错误信息：" + ex.Message);
        //    }
        //    return Success("操作成功");
        //}
        /// <summary>
        /// 台账状态操作
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ledgerType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "高风险台账操作")]
        [AjaxOnly]
        public ActionResult LedgerOp(string keyValue, string ledgerType, string type, string worktime, string issendmessage, string conditioncontent, string conditionid, string iscomplete)
        {
            try
            {
                scaffoldbll.LedgerOp(keyValue, ledgerType, type, worktime, issendmessage, conditioncontent, conditionid, iscomplete);
            }
            catch (Exception ex)
            {
                return Success("操作失败，错误信息：" + ex.Message);
            }
            return Success("操作成功");
        }
        #endregion
    }
}
