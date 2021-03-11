using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using Newtonsoft.Json;
using ERCHTMS.Busines.SystemManage;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：任务分配表
    /// </summary>
    public class TaskShareController : MvcControllerBase
    {
        private TaskShareBLL tasksharebll = new TaskShareBLL();
        private TeamsInfoBLL teamsinfobll = new TeamsInfoBLL();
        private StaffInfoBLL staffinfobll = new StaffInfoBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.IsTeams = new DataItemDetailBLL().GetItemValue("是否启动定时服务");
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormOne()
        {
            ViewBag.IsTeams = new DataItemDetailBLL().GetItemValue("是否启动定时服务");
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormTwo()
        {
            ViewBag.IsTeams = new DataItemDetailBLL().GetItemValue("是否启动定时服务");
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormThree()
        {
            return View();
        }


        /// <summary>
        /// 旁站监督统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Statistics()
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
            var data = tasksharebll.GetList(queryJson);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取分配任务列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDataTableJson(Pagination pagination, string queryJson)
        {

            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                var data = tasksharebll.GetDataTable(pagination, queryJson, authType);
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
            var data = tasksharebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 监督任务数量对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryHidNumberComparison(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string deptCode = queryParam["deptCode"].ToString();  //部门
                string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
                string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
                var curUser = new OperatorProvider().Current(); //当前用户

                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
                hentity.startDate = startDate;
                hentity.endDate = endDate;
                hentity.sAction = "1";   //对比图分析
                hentity.sMark = 0;
                
                //当前用户是厂级
                if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                //列表
                var dt = tasksharebll.QueryStatisticsByAction(hentity);

                //x 轴Title 
                List<dseries> xdata = new List<dseries>();

                //x 轴Title 
                List<dseries> sdata = new List<dseries>();
                //未监督
                List<dseries_child> yblist = new List<dseries_child>();
                //已监督
                List<dseries_child> zdlist = new List<dseries_child>();

                dseries s1 = new dseries();
                s1.name = "需监督";
                s1.id = "ybyh";
                dseries s2 = new dseries();
                s2.name = "已监督";
                s2.id = "zdyh";
                //图表分析
                foreach (DataRow row in dt.Rows)
                {
                    string dname = row["fullname"].ToString();
                    string drillId = row["createuserdeptcode"].ToString();
                    //需监督
                    dseries_child ybyh = new dseries_child();
                    ybyh.name = dname;
                    ybyh.y = Convert.ToInt32(row["total"].ToString());
                    ybyh.drilldown = "yb" + drillId;//部门编码
                    yblist.Add(ybyh);

                    //已监督
                    dseries_child zdyh = new dseries_child();
                    zdyh.name = row["fullname"].ToString();
                    zdyh.y = Convert.ToInt32(row["ImportanHid"].ToString());
                    zdyh.drilldown = "zd" + drillId;//部门编码
                    zdlist.Add(zdyh);

                    //获取各部门机构的数据
                    List<dseries_child> cyblist = new List<dseries_child>();
                    List<dseries_child> czdlist = new List<dseries_child>();
                    hentity.sDeptCode = row["createuserdeptcode"].ToString();
                    hentity.sHidRank = "需监督,已监督";
                    hentity.sMark = 1;
                    var yhdt = tasksharebll.QueryStatisticsByAction(hentity);
                    foreach (DataRow crow in yhdt.Rows)
                    {
                        //需监督
                        dseries_child cybmodel = new dseries_child();
                        cybmodel.name = crow["fullname"].ToString();
                        cybmodel.y = Convert.ToInt32(crow["total"].ToString());
                        cybmodel.drilldown = "next_yb_" + crow["pteamcode"].ToString(); ;//部门编码
                        cyblist.Add(cybmodel);

                        //已监督
                        dseries_child czdmodel = new dseries_child();
                        czdmodel.name = crow["fullname"].ToString();
                        czdmodel.y = Convert.ToInt32(crow["ImportanHid"].ToString());
                        czdmodel.drilldown = "next_zd_" + crow["pteamcode"].ToString(); ;//部门编码
                        czdlist.Add(czdmodel);
                    }
                    //需监督子项目
                    dseries cybdseries = new dseries();
                    cybdseries.name = "需监督";
                    cybdseries.id = "yb" + drillId;
                    cybdseries.data = cyblist;
                    sdata.Add(cybdseries);


                    //已监督子项目
                    dseries czddseries = new dseries();
                    czddseries.name = "已监督";
                    czddseries.id = "zd" + drillId;
                    czddseries.data = czdlist;
                    sdata.Add(czddseries);
                }
                s1.data = yblist; //需监督
                xdata.Add(s1);
                s2.data = zdlist; //已监督
                xdata.Add(s2);
                //结果集合
                var jsonData = new { tdata = dt, xdata = xdata, sdata = sdata, iscompany = hentity.isCompany ? 1 : 0 };

                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        /// <summary>
        /// 监督任务数量表格
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryHidNUmberComparisonList(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string deptCode = queryParam["deptCode"].ToString();  //部门
                string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
                string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
                var curUser = new OperatorProvider().Current(); //当前用户

                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
                hentity.startDate = startDate;
                hentity.endDate = endDate;
                hentity.sAction = "1";   //对比图分析
                hentity.sMark = 2;
                //当前用户是厂级
                if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                var treeList = new List<TreeGridEntity>();
                //列表
                var dt = tasksharebll.QueryStatisticsByAction(hentity);

                foreach (DataRow row in dt.Rows)
                {
                    TreeListForHidden tentity = new TreeListForHidden();
                    tentity.createuserdeptcode = row["createuserdeptcode"].ToString();
                    tentity.fullname = row["fullname"].ToString();
                    tentity.sortcode = row["sortcode"].ToString();
                    tentity.departmentid = row["departmentid"].ToString();
                    if (row["parentid"].ToString() != "0")
                    {
                        tentity.parent = row["parentid"].ToString();
                    }
                    tentity.importanhid = Convert.ToDecimal(row["importanhid"].ToString());
                    tentity.ordinaryhid = Convert.ToDecimal(row["ordinaryhid"].ToString());
                    tentity.total = Convert.ToDecimal(row["total"].ToString());
                    TreeGridEntity tree = new TreeGridEntity();
                    bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", tentity.departmentid)).Count() == 0 ? false : true;
                    tentity.haschild = hasChildren;
                    tree.id = row["departmentid"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    string itemJson = tentity.ToJson();
                    tree.entityJson = itemJson;
                    tree.expanded = false;
                    tree.hasChildren = hasChildren;
                    treeList.Add(tree);
                }

                //结果集合
                return Content(treeList.TreeJson("0"));
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// 旁站监管数量对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QuerySupervisonNumberComparison(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string deptCode = queryParam["deptCode"].ToString();  //部门
                string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
                string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
                var curUser = new OperatorProvider().Current(); //当前用户

                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
                hentity.startDate = startDate;
                hentity.endDate = endDate;
                hentity.sAction = "2";   //对比图分析
                hentity.sMark = 0;

                //当前用户是厂级
                if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                //列表
                var dt = tasksharebll.QueryStatisticsByAction(hentity);

                //x 轴Title 
                List<dseries> xdata = new List<dseries>();

                //x 轴Title 
                List<dseries> sdata = new List<dseries>();
                //未监督
                List<dseries_child> yblist = new List<dseries_child>();
                //已监督
                List<dseries_child> zdlist = new List<dseries_child>();

                dseries s1 = new dseries();
                s1.name = "需监管";
                s1.id = "ybyh";
                dseries s2 = new dseries();
                s2.name = "已监管";
                s2.id = "zdyh";
                //图表分析
                foreach (DataRow row in dt.Rows)
                {
                    string dname = row["fullname"].ToString();
                    string drillId = row["createuserdeptcode"].ToString();
                    //需监管
                    dseries_child ybyh = new dseries_child();
                    ybyh.name = dname;
                    ybyh.y = Convert.ToInt32(row["ordinaryhid"].ToString());
                    ybyh.drilldown = "yb" + drillId;//部门编码
                    yblist.Add(ybyh);

                    //已监管
                    dseries_child zdyh = new dseries_child();
                    zdyh.name = row["fullname"].ToString();
                    zdyh.y = Convert.ToInt32(row["ImportanHid"].ToString());
                    zdyh.drilldown = "zd" + drillId;//部门编码
                    zdlist.Add(zdyh);
                }
                s1.data = yblist; //需监管
                xdata.Add(s1);
                s2.data = zdlist; //已监管
                xdata.Add(s2);
                //结果集合
                var jsonData = new { tdata = dt, xdata = xdata, sdata = sdata, iscompany = hentity.isCompany ? 1 : 0 };

                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 监管任务数量表格
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QuerySupervisonNumberComparisonList(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string deptCode = queryParam["deptCode"].ToString();  //部门
                string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
                string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
                var curUser = new OperatorProvider().Current(); //当前用户

                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
                hentity.startDate = startDate;
                hentity.endDate = endDate;
                hentity.sAction = "2";   //对比图分析
                hentity.sMark = 0;
                //当前用户是厂级
                if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                var treeList = new List<TreeGridEntity>();
                //列表
                var dt = tasksharebll.QueryStatisticsByAction(hentity);

                //结果集合
                return ToJsonResult(dt);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        
        /// <summary>
        /// 导出统计列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult ExportStatisticExcel(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string deptCode = queryParam["deptCode"].ToString();  //部门
                string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
                string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
                string stype = queryJson.Contains("stype") ? queryParam["stype"].ToString() : ""; //导出类型
                var curUser = new OperatorProvider().Current(); //当前用户

                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
                hentity.startDate = startDate;
                hentity.endDate = endDate;
                if (stype == "监管图")
                {
                    hentity.sAction = "2";   //对比图分析
                    hentity.sMark = 0;
                }
                else if (stype == "监督图")
                {
                    hentity.sAction = "1";   //对比图分析
                    hentity.sMark = 2;
                }
                
                //当前用户是厂级
                if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                var treeList = new List<TreeGridEntity>();
                //列表
                var dt = tasksharebll.QueryStatisticsByAction(hentity);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                if (stype == "监管图")
                {
                    excelconfig.Title = "旁站监管统计";
                    excelconfig.FileName = "旁站监管统计.xls";
                    //需跟数据源列顺序保持一致
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "部门名称", Width = 40 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ordinaryhid", ExcelColumn = "需监管", Width = 40 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "importanhid", ExcelColumn = "已监管", Width = 40 });
                }
                else if (stype == "监督图")
                {
                    excelconfig.Title = "旁站监督统计";
                    excelconfig.FileName = "旁站监督统计.xls";
                    //需跟数据源列顺序保持一致
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "部门名称", Width = 40 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "importanhid", ExcelColumn = "已监督", Width = 40 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "需监督", Width = 40 });
                }
               
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
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
            tasksharebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ManageDelForm(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var dept = new DepartmentBLL().GetEntity(keyValue);
            try
            {
                var taskshareid = "";
                var teamentity = teamsinfobll.GetEntity(keyValue);
                if (teamentity != null)
                {
                    taskshareid = teamentity.TaskShareId;
                    new TeamsInfoBLL().RemoveForm(keyValue);
                    var single = new
                    {
                        taskshareid = teamentity.TaskShareId,
                        teamid = teamentity.TeamId
                    };
                    List<StaffInfoEntity> slist = new StaffInfoBLL().GetList(JsonConvert.SerializeObject(single)).ToList();
                    foreach (var item in slist)
                    {
                        staffinfobll.RemoveForm(item.Id);
                    }
                    //写入日志文件
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + user.UserName + "删除" + dept.FullName + "分配任务成功，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",班组信息:" + Newtonsoft.Json.JsonConvert.SerializeObject(teamentity) + ",人员任务:" + Newtonsoft.Json.JsonConvert.SerializeObject(slist.Where(t => t.TaskLevel == "1")) + "\r\n");
                }
            }
            catch (Exception ex)
            {
                //写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + user.UserName + "删除" + dept.FullName + "分配任务失败，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",异常信息：" + ex.Message + "\r\n");
                return Success("操作失败，错误信息：" + ex.Message);
            }
            return Success("删除成功。");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ManageRemoveForm(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                tasksharebll.RemoveForm(keyValue);
                var single = new
                {
                    taskshareid = keyValue,
                    tasklevel = 1
                };
                List<StaffInfoEntity> slist = new StaffInfoBLL().GetList(JsonConvert.SerializeObject(single)).ToList();
                //写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + user.UserName + "分配任务删除成功，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "人员任务:" + Newtonsoft.Json.JsonConvert.SerializeObject(slist) + "\r\n");
            }
            catch (Exception ex)
            {
                //写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + user.UserName + "分配任务删除失败，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",异常信息：" + ex.Message + "\r\n");
                return Success("操作失败，错误信息：" + ex.Message);
            }
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
        public ActionResult SaveForm(string keyValue, string jsonData)
        {
            TaskShareEntity model = JsonConvert.DeserializeObject<TaskShareEntity>(jsonData);
            tasksharebll.SaveForm(keyValue, model);
            return Success("操作成功。");
        }

        /// <summary>
        /// 结束任务
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult FinishTask(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {

                TaskShareEntity u = tasksharebll.GetEntity(keyValue);
                if (u != null)
                {
                    u.FlowDept = "";
                    u.FlowRoleName = "";
                    u.FlowStep = "3";
                    tasksharebll.SaveOnlyShare(keyValue, u);
                    //将公司管理员操作分配完成写入日志文件
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + user.UserName + "分配任务完成成功，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "\r\n");
                }
            }
            catch (Exception ex)
            {
                //将公司管理员操作分配完成写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：分配任务完成失败，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",异常信息：" + ex.Message + "\r\n");
                return Success("操作失败，错误信息：" + ex.Message);
            }
            return Success("操作成功");
        }
        #endregion

        #region 导出
        /// <summary>
        /// 导出列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = " 1=1 ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                DataTable data = tasksharebll.GetDataTable(pagination, queryJson, authType);
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("tasktype"));
                excelTable.Columns.Add(new DataColumn("fullname"));
                excelTable.Columns.Add(new DataColumn("createusername"));
                excelTable.Columns.Add(new DataColumn("createdate"));
                excelTable.Columns.Add(new DataColumn("flowdeptname"));
                excelTable.Columns.Add(new DataColumn("flowstep"));
                foreach (DataRow item in data.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    var tasktype = item["tasktype"].ToString();
                    if (tasktype == "0")
                    {
                        tasktype = "部门任务";
                    }
                    else if (tasktype == "1")
                    {
                        tasktype = "班组任务";
                    }
                    else if (tasktype == "2")
                    {
                        tasktype = "人员任务";
                    }
                    newDr["tasktype"] = tasktype;
                    newDr["fullname"] = item["fullname"];
                    newDr["createusername"] = item["createusername"];
                    DateTime createdate;
                    DateTime.TryParse(item["createdate"].ToString(), out createdate);
                    newDr["createdate"] = createdate.ToString("yyyy-MM-dd");
                    newDr["flowdeptname"] = item["flowdeptname"];
                    var flowstep = item["flowstep"].ToString();
                    switch (flowstep)
                    {
                        case "0":
                            flowstep = "厂级分配中";
                            break;
                        case "1":
                            flowstep = "部门分配中";
                            break;
                        case "2":
                            if (item["tasktype"].ToString() != "2")
                            {
                                flowstep = "班组分配中";
                            }
                            else
                            {
                                flowstep = "分配中";
                            }
                            break;
                        case "3":
                            flowstep = "分配完成";
                            break;
                    }
                    newDr["flowstep"] = flowstep;
                    excelTable.Rows.Add(newDr);
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "旁站监督信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "旁站监督信息.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "tasktype", ExcelColumn = "任务类型", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "创建单位", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createusername", ExcelColumn = "创建人", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "创建时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "flowdeptname", ExcelColumn = "分配部门", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "flowstep", ExcelColumn = "任务分配", Width = 40 });
                //调用导出方法
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion
    }
}
