using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Collections.Generic;
using BSFramework.Util.Offices;
using System.Linq;
using ERCHTMS.Busines.BaseManage;
using System.Data;
using System.Web;
using System;
using ERCHTMS.Cache;
using System.Linq.Expressions;
using System.Text;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// 描 述：应急演练
    /// </summary>
    public class DrillplanController : MvcControllerBase
    {
        private TeamBLL teambll = new TeamBLL();
        private UserBLL userBLL = new UserBLL();
        private PostBLL postBLL = new PostBLL();
        private DepartmentBLL departBLL = new DepartmentBLL();
        private DrillplanBLL drillplanbll = new DrillplanBLL();
        private DrillplanrecordBLL drillplanrecordbll = new DrillplanrecordBLL();
        private DataItemCache dataItemCache = new DataItemCache();

        #region 视图功能
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }



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
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region 获取数据

        /// <summary> 
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = @"createuserid,createuserdeptcode,createuserorgcode, DEPARTID, DEPARTNAME,DRILLTYPE,DRILLMODE,to_char(PLANTIME,'yyyy-MM') as PLANTIME,
            DRILLTYPENAME,DRILLMODENAME,NAME,RPLANID,orgdeptid,orgdept,orgdeptcode,(select count(1) from MAE_DRILLPLANRECORD d where d.DRILLPLANId=t.id) recordnum,executepersonname,executepersonid,DrillCost";
            pagination.p_tablename = "mae_drillplan t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "orgdeptcode", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }


            var watch = CommonHelper.TimerStart();
            var data = drillplanbll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = drillplanbll.GetList(queryJson);
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
            var data = drillplanbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportDriplan(string PostId)
        {
            var user = OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                //return "超级管理员无此操作权限";
            }
            List<UserEntity> ulist = userBLL.GetList().ToList();
            string orgId = user.OrganizeId;//所属公司
            string deptId = PostId;//所属部门
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            try
            {
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
                    DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                    int order = 1;
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        order = i;
                        //---****值存在空验证*****--
                        if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()) ||
                            string.IsNullOrEmpty(dt.Rows[i][1].ToString()) ||
                            string.IsNullOrEmpty(dt.Rows[i][2].ToString()) ||
                            string.IsNullOrEmpty(dt.Rows[i][3].ToString()) ||
                            string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                            error++;
                            continue;
                        }
                        //组织部门
                        string orgdept = dt.Rows[i][0].ToString();
                        var orgDepart = departBLL.GetList().Where(e => e.FullName == orgdept && e.OrganizeId == orgId).FirstOrDefault();
                        if (orgDepart == null)
                        {
                            falseMessage += "第" + i + "行导入失败,该组织部门不存在！</br>";
                            error++;
                            continue;
                        }
                        string deptName = string.Empty;
                        string yldeptId = string.Empty;
                        string departname = dt.Rows[i][1].ToString();
                        var deptList = departname.Split(',');
                        for (int k = 0; k < deptList.Length; k++)
                        {
                            var Depart = departBLL.GetList().Where(e => e.FullName == deptList[k].ToString()).FirstOrDefault();
                            if (Depart == null)
                            {
                                continue;
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(deptName))
                                {
                                    deptName += Depart.FullName + ",";
                                    yldeptId += Depart.DepartmentId + ",";
                                }
                                else
                                {
                                    if (!yldeptId.Contains(Depart.DepartmentId))
                                    {
                                        deptName += Depart.FullName + ",";
                                        yldeptId += Depart.DepartmentId + ",";
                                    }
                                }
                            }
                        }

                        if (string.IsNullOrWhiteSpace(deptName))
                        {
                            falseMessage += "第" + i + "行导入失败,该演练部门不存在！</br>";
                            error++;
                            continue;
                        }
                        else
                        {
                            deptName = deptName.Substring(0, deptName.Length - 1);
                            yldeptId = yldeptId.Substring(0, yldeptId.Length - 1);
                        }
                        //演练预案名称
                        string name = dt.Rows[i][2].ToString();
                        //演练预案类型
                        string yzTypeName = dt.Rows[i][3].ToString();
                        var yzType = dataItemCache.GetDataItemList("MAE_DirllPlanType").Where(e => e.ItemName == yzTypeName).FirstOrDefault();
                        if (yzType == null)
                        {
                            falseMessage += "第" + i + "行导入失败,该演练预案类型不存在！</br>";
                            error++;
                            continue;
                        }
                        //演练方式
                        string yzModeName = dt.Rows[i][4].ToString();
                        var yzMode = dataItemCache.GetDataItemList("MAE_DirllMode").Where(e => e.ItemName == yzModeName).FirstOrDefault();
                        if (yzMode == null)
                        {
                            falseMessage += "第" + i + "行导入失败,该演练方式不存在！</br>";
                            error++;
                            continue;
                        }
                        //演练计划费用
                        string yzDrillCost = dt.Rows[i][5].ToString();
                        decimal DrillCost;
                        if (!string.IsNullOrEmpty(yzDrillCost))
                        {
                            if (!decimal.TryParse(yzDrillCost, out DrillCost))
                            {
                                falseMessage += "第" + i + "行导入失败,演练计划费用格式不正确！</br>";
                                error++;
                                continue;
                            }
                        }
                        //执行人
                        string executepersonname = string.Empty;
                        string executepersonid = string.Empty;
                        string tempusername = dt.Rows[i][6].ToString();
                        if (!string.IsNullOrEmpty(tempusername))
                        {
                            var tpuserlist = ulist.Where(p => p.RealName == tempusername).ToList();
                            if (tpuserlist.Count() > 0)
                            {
                                executepersonname = tempusername;
                                executepersonid = tpuserlist.FirstOrDefault().UserId;
                            }
                        }
                        //计划时间
                        DateTime PLANTIME = new DateTime();
                        try
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i][7].ToString()))
                            {
                                PLANTIME = DateTime.Parse(DateTime.Parse(dt.Rows[i][7].ToString()).ToString("yyyy-MM"));
                            }
                        }
                        catch
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行时间有误,未能导入.";
                            error++;
                            continue;
                        }
                        try
                        {
                            drillplanbll.SaveForm("", new DrillplanEntity
                            {
                                NAME = name,
                                OrgDeptId = orgDepart.DepartmentId,
                                OrgDept = orgDepart.FullName,
                                OrgDeptCode=orgDepart.EnCode,
                                DEPARTID = yldeptId,
                                DEPARTNAME = deptName,
                                DRILLTYPE = yzType.ItemValue,
                                DRILLTYPENAME = yzType.ItemName,
                                DRILLMODE = yzMode.ItemValue,
                                DRILLMODENAME = yzMode.ItemName,
                                DrillCost = yzDrillCost,
                                EXECUTEPERSONNAME = executepersonname,
                                EXECUTEPERSONID = executepersonid,
                                PLANTIME = PLANTIME
                            });
                        }
                        catch
                        {
                            error++;
                        }

                    }
                    count = dt.Rows.Count - 1;
                    message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                    message += "</br>" + falseMessage;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return message;
        }

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
            drillplanbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, DrillplanEntity entity)
        {
            drillplanbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        [HttpPost]
        public ActionResult SaveListForm()
        {
            string data = Request["param"];
            var list = data.ToObject<List<DrillplanEntity>>();

            foreach (var item in list)
            {
                drillplanbll.SaveForm(item.ID, item);
            }

            return Success("操作成功。");
        }

        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出应急演练计划")]
        public ActionResult ExportDrillplanList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "Orgdept,DEPARTNAME,NAME,DRILLTYPENAME,DRILLMODENAME,DrillCost,PLANTIME,EXECUTEPERSONNAME";
            pagination.p_tablename = "V_mae_drillplan t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "createdate";//排序字段
            pagination.sord = "desc";//排序方式  
            #region 权限校验
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            var data = drillplanbll.GetPageList(pagination, queryJson);
            #endregion
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "应急演练计划";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "应急演练计划.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "orgdept", ExcelColumn = "组织部门" });
            listColumnEntity.Add(new ColumnEntity() { Column = "departname", ExcelColumn = "演练部门" });
            listColumnEntity.Add(new ColumnEntity() { Column = "name", ExcelColumn = "演练预案名称", Width = 50 });
            listColumnEntity.Add(new ColumnEntity() { Column = "drilltypename", ExcelColumn = "演练预案类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "drillmodename", ExcelColumn = "演练方式" });
            listColumnEntity.Add(new ColumnEntity() { Column = "drillcost", ExcelColumn = "演练计划费用（元）" });
            listColumnEntity.Add(new ColumnEntity() { Column = "executepersonname", ExcelColumn = "执行人" });
            listColumnEntity.Add(new ColumnEntity() { Column = "plantime", ExcelColumn = "计划时间" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion

        #region 报表统计
        /// <summary>
        /// 应急演练计划完成率
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DrillplanFinish()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var year = int.Parse(Request["year"] ?? "0");
            var deptId = Request["deptId"] ?? "";
            var jd = int.Parse(Request["jd"] ?? "0");
            var monthCK = int.Parse(Request["month"] ?? "0");
            var type = int.Parse(Request["type"] ?? "0");
            var starttime = Request["starttime"] ?? "";
            var endtime = Request["endtime"] ?? "";
            var returnList = new List<Object>();

            //权限
            #region 权限
            string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
            string sqlwhere = " and 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sqlwhere += " and " + where;
            }
            else
            {

                sqlwhere += string.Format(" and CREATEUSERDEPTCODE like '{0}%'", user.DeptCode);
            }
            #endregion
            string cwhere = string.Empty;
            string cyear = Request["year"] ?? "";
            string cdeptid = Request["deptId"] ?? "";
            string cmonth = Request["month"] ?? "";

            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                cwhere += string.Format("  and  a.createuserdeptcode like '{0}%'", user.OrganizeCode);
            }
            else
            {
                cwhere += string.Format("  and  a.createuserdeptcode like '{0}%'", user.DeptCode);
            }

            if (!string.IsNullOrEmpty(cyear))
            {
                cwhere += string.Format(@" and to_char(a.drilltime,'yyyy') = '{0}' ", cyear);
            }
            if (!string.IsNullOrEmpty(cdeptid))
            {
                cwhere += string.Format(@" and a.departid  like  '%{0}%' ", cdeptid);
            }
            if (!string.IsNullOrEmpty(starttime))
            {
                cwhere += string.Format(@" and a.drilltime >=  to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                cwhere += string.Format(@" and a.drilltime  <=  to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", endtime);
            }
            if (type == 0) //应急演练预案类型统计
            {
                var seriesList = new List<Object>();
                var drilldownList = new List<Object>();

                // var stable = new List<Object>();
                var dt = drillplanrecordbll.GetDrillPlanRecordTypeSta(cwhere, 0);

                foreach (DataRow row in dt.Rows)
                {
                    // stable.Add(new { id =  row["itemname"].ToString() ,name = row["itemname"].ToString(), value = int.Parse(row["num"].ToString()) }); 
                    if (row["itemname"].ToString() == "现场处置方案")
                    {
                        string twhere = cwhere + string.Format("  and a.drilltypename ='{0}' ", row["itemname"].ToString());

                        var drilldt = drillplanrecordbll.GetDrillPlanRecordTypeSta(twhere, 1);

                        var drilldowndata = new List<object>();

                        foreach (DataRow drow in drilldt.Rows)
                        {
                            // stable.Add(new { id = drow["itemname"].ToString(), name = drow["itemname"].ToString(), value = int.Parse(drow["num"].ToString()), parentid = row["itemname"].ToString() }); 
                            drilldowndata.Add(new { name = drow["itemname"].ToString(), y = int.Parse(drow["num"].ToString()), p = 0 });
                        }

                        drilldownList.Add(new { name = row["itemname"].ToString(), id = row["itemname"].ToString(), data = drilldowndata });

                        seriesList.Add(new { name = row["itemname"].ToString(), y = int.Parse(row["num"].ToString()), p = 0, drilldown = row["itemname"].ToString() });
                    }
                    else
                    {
                        seriesList.Add(new { name = row["itemname"].ToString(), y = int.Parse(row["num"].ToString()), p = 0 });
                    }
                }

                #region MyRegion
                //var treeList = new List<TreeGridEntity>();
                //foreach (DataRow row in dt.Rows)
                //{
                //    //TreeListForHidden tentity = new TreeListForHidden();
                //    //tentity.createuserdeptcode = row["createuserdeptcode"].ToString();
                //    //tentity.fullname = row["fullname"].ToString();
                //    //tentity.sortcode = row["sortcode"].ToString();
                //    //tentity.departmentid = row["departmentid"].ToString();
                //    //if (row["parentid"].ToString() != "0")
                //    //{
                //    //    tentity.parent = row["parentid"].ToString();
                //    //}
                //    //tentity.importanhid = Convert.ToDecimal(row["importanhid"].ToString());
                //    //tentity.ordinaryhid = Convert.ToDecimal(row["ordinaryhid"].ToString());
                //    //tentity.total = Convert.ToDecimal(row["total"].ToString());
                //    TreeGridEntity tree = new TreeGridEntity();
                //    bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", tentity.departmentid)).Count() == 0 ? false : true;
                //    tentity.haschild = hasChildren;
                //    tree.id = row["departmentid"].ToString();
                //    tree.parentId = row["parentid"].ToString();
                //    string itemJson = tentity.ToJson();
                //    tree.entityJson = itemJson;
                //    tree.expanded = false;
                //    tree.hasChildren = hasChildren;
                //    treeList.Add(tree);
                //} 
                #endregion

                returnList.Add(new { seriesdata = seriesList, drilldowndata = drilldownList });
            }
            else if (type == 1) //参加演练人次统计
            {
                var dt = drillplanrecordbll.GetDrillPlanRecordTypeSta(cwhere, 2);
                foreach (DataRow row in dt.Rows)
                {
                    returnList.Add(new { name = row["itemname"].ToString(), y = int.Parse(row["num"].ToString()), p = 0 });
                }
            }
            else if (type == 5) //外委跟单位内部分析
            {
                var bll = new ReserverplanBLL();
                var list = bll.GetList(sqlwhere).GroupBy(e => e.ORGXZNAME);
                foreach (var item in list)
                {
                    returnList.Add(new { text = item.Key, value = item.Count() });
                }
            }
            else
            {
                //计划的数量
                var list_JH = drillplanbll.GetList(sqlwhere).Where(e => e.PLANTIME.Value.Year == year && (deptId.Length > 0 ? e.DEPARTID == deptId : 1 == 1));
                DrillplanrecordBLL dbll = new DrillplanrecordBLL();
                //统计实际计划数量
                var list_SJ = dbll.GetList(sqlwhere).Where(e => e.DRILLTIME.Value.Year == year && (deptId.Length > 0 ? e.DEPARTID == deptId : 1 == 1));
                if (type == 2 || type == 3)
                {
                    #region  季度统计
                    //开始统计,计划的数量
                    decimal[] jd_JH = { 0, 0, 0, 0 };
                    foreach (var item in list_JH)
                    {
                        if (deptId.Length > 0 && item.DEPARTID != deptId)
                            continue;
                        if (year > 0 && item.PLANTIME.Value.Year != year)
                            continue;
                        var month = item.PLANTIME.Value.Month;

                        if (month >= 1 && month <= 3)
                            jd_JH[0]++;

                        if (month >= 4 && month <= 6)
                            jd_JH[1]++;

                        if (month >= 7 && month <= 9)
                            jd_JH[2]++;

                        if (month >= 10 && month <= 12)
                            jd_JH[3]++;
                    }

                    //开始统计，实际的数量
                    decimal[] jd_SJ = { 0, 0, 0, 0 };
                    foreach (var item in list_SJ)
                    {
                        if (deptId.Length > 0 && item.DEPARTID != deptId)
                            continue;
                        if (year > 0 && item.DRILLTIME.Value.Year != year)
                            continue;
                        var month = item.DRILLTIME.Value.Month;

                        if (month >= 1 && month <= 3)
                            jd_SJ[0]++;

                        if (month >= 4 && month <= 6)
                            jd_SJ[1]++;

                        if (month >= 7 && month <= 9)
                            jd_SJ[2]++;

                        if (month >= 10 && month <= 12)
                            jd_SJ[3]++;

                    }
                    List<string> xValues = new List<string>();

                    if (jd == 0)
                    {

                        for (int i = 0; i < 4; i++)
                        {
                            xValues.Add("第" + (i + 1) + "季度");
                        }
                        for (int i = 0; i < xValues.Count; i++)
                        {

                            var jhNum = 0M;
                            var value = 0M;

                            if (jd_JH[i] == 0)
                            {
                                jhNum = 0;
                                value = 0;
                            }
                            else
                            {
                                jhNum = jd_JH[i];
                                value = (jd_SJ[i] / jd_JH[i]) * 100;
                            }
                            var finish = new { text = xValues[i], jhNum = jhNum, value = decimal.Round(value, 2), sjNum = jd_SJ[i] };
                            returnList.Add(finish);
                        }
                    }
                    else
                    {

                        var jhNum = 0M;
                        var value = 0M;

                        if (jd_JH[jd - 1] == 0)
                        {
                            jhNum = 0;
                            value = 0;
                        }
                        else
                        {
                            jhNum = jd_JH[jd - 1];
                            value = (jd_SJ[jd - 1] / jd_JH[jd - 1]) * 100;
                        }
                        var finish = new { text = "第" + jd + "季度", jhNum = jhNum, value = decimal.Round(value, 2), sjNum = jd_SJ[jd - 1] };
                        returnList.Add(finish);
                    }
                    #endregion
                }
                if (type == 4)
                {
                    #region 方式统计

                    ////判断月份
                    var drillmode = dataItemCache.GetDataItemList("MAE_DirllMode");
                    foreach (var item in drillmode)
                    {
                        var dpf = new { text = item.ItemName, jhNum = list_JH.Where(e => (year > 0 ? e.PLANTIME.Value.Year == year : 1 == 1) && (monthCK > 0 ? e.PLANTIME.Value.Month == monthCK : 1 == 1) && e.DRILLMODENAME == item.ItemName).Count(), sjNum = list_SJ.Where(e => (year > 0 ? e.DRILLTIME.Value.Year == year : 1 == 1) && (monthCK > 0 ? e.DRILLTIME.Value.Month == monthCK : 1 == 1) && e.DRILLMODENAME == item.ItemName).Count() };
                        returnList.Add(dpf);
                    }
                    #endregion
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(returnList);
        }

        [HttpPost]
        public ActionResult DrillplanStat()
        {
            var curUser = new OperatorProvider().Current(); //当前用户

            var deptCode = Request["deptCode"] ?? "";

            var starttime = Request["starttime"] ?? "";
            var endtime = Request["endtime"] ?? "";
            var isCompany = false;
            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
            {
                isCompany = true;
            }
            else
            {
                isCompany = false;
            }
            deptCode = deptCode == "" ? curUser.DeptCode : deptCode;
            //x 轴Title 
            List<dseries> xdata = new List<dseries>();

            //x 轴Title 
            List<dseries> sdata = new List<dseries>();

            var DirllMode = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_DirllMode'").ToList();
            for (int i = 0; i < DirllMode.Count; i++)
            {
                var dt = drillplanrecordbll.DrillplanStat(DirllMode[i].ItemName, isCompany, deptCode, starttime, endtime);

                List<dseries_child> mode = new List<dseries_child>();

                dseries s1 = new dseries();

                s1.name = DirllMode[i].ItemName;
                s1.id = DirllMode[i].ItemValue;
                //图表分析
                foreach (DataRow row in dt.Rows)
                {
                    dseries_child ybyh = new dseries_child();
                    ybyh.name = row["fullname"].ToString();
                    ybyh.y = Convert.ToInt32(row["recordnum"].ToString());
                    ybyh.drilldown = DirllMode[i].ItemDetailId + row["encode"].ToString();//部门编码
                    mode.Add(ybyh);
                    List<dseries_child> cyblist = new List<dseries_child>();
                    var cdeptCode = row["encode"].ToString();
                    var dept = new DepartmentBLL().GetEntityByCode(cdeptCode);
                    if (dept != null)
                    {
                        if (dept.Nature == "厂级")
                        {
                            continue;
                        }
                    }
                    var dtChild = drillplanrecordbll.DrillplanStatDetail(DirllMode[i].ItemName, false, cdeptCode, "", "");
                    foreach (DataRow childRow in dtChild.Rows)
                    {
                        dseries_child cybmodel = new dseries_child();
                        cybmodel.name = childRow["fullname"].ToString();
                        cybmodel.y = Convert.ToInt32(childRow["recordnum"].ToString());
                        cyblist.Add(cybmodel);
                    }
                    dseries cybdseries = new dseries();
                    cybdseries.name = DirllMode[i].ItemName;
                    cybdseries.id = DirllMode[i].ItemDetailId + row["encode"].ToString();
                    cybdseries.data = cyblist;
                    sdata.Add(cybdseries);
                }
                s1.data = mode;
                xdata.Add(s1);
            }
            var jsonData = new { xdata = xdata, sdata = sdata };

            return Content(jsonData.ToJson());
        }


        public ActionResult DrillplanStatList(string queryJson)
        {
            var curUser = new OperatorProvider().Current(); //当前用户
            var queryParam = queryJson.ToJObject();

            string startTime = queryJson.Contains("startTime") ? queryParam["startTime"].ToString() : "";  //起始日期 
            string endTime = queryJson.Contains("endTime") ? queryParam["endTime"].ToString() : "";//截止日期
            var isCompany = false;
            var code = string.Empty;
            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
            {
                isCompany = true;
                code = curUser.OrganizeCode;
            }
            else
            {
                isCompany = false;
                code = curUser.DeptCode;
            }
            string deptCode = queryJson.Contains("deptCode") ? queryParam["deptCode"].ToString() == "" ? code : queryParam["deptCode"].ToString() : "";  //部门编码
            //string deptCode = curUser.DeptCode;
            var treeList = new List<TreeGridEntity>();
            var dt = drillplanrecordbll.DrillplanStatList("", isCompany, deptCode, startTime, endTime);

            var datamode = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_DirllMode'").ToList();

            foreach (DataRow row in dt.Rows)
            {
                DataTable newDt = new DataTable();
                newDt.Columns.Add("changedutydepartcode");
                newDt.Columns.Add("fullname");
                newDt.Columns.Add("departmentid");
                newDt.Columns.Add("parent");
                newDt.Columns.Add("total");
                newDt.Columns.Add("haschild", typeof(bool));
                if (datamode.Count > 0)
                {
                    for (int i = 0; i < datamode.Count; i++)
                    {
                        newDt.Columns.Add("recordnum" + (i + 1));
                    }
                }
                else
                {
                    newDt.Columns.Add("recordnum1");
                    newDt.Columns.Add("recordnum2");
                }
                DataRow newDr = newDt.NewRow();
                newDr["changedutydepartcode"] = row["encode"].ToString();
                newDr["fullname"] = row["fullname"].ToString();
                newDr["departmentid"] = row["departmentid"].ToString();
                if (row["parentid"].ToString() != "0")
                {
                    newDr["parent"] = row["parentid"].ToString();
                }

                newDr["total"] = row["total"].ToString();
                if (datamode.Count > 0)
                {
                    for (int i = 0; i < datamode.Count; i++)
                    {
                        newDr["recordnum" + (i + 1)] = row["recordnum" + (i + 1)].ToString();
                    }
                }
                else
                {
                    newDr["recordnum1"] = row["recordnum1"].ToString();
                    newDr["recordnum2"] = row["recordnum2"].ToString();
                }
                //if (datamode.Count > 0) {
                //    var tentity = new
                //    {
                //        changedutydepartcode = row["encode"].ToString(),
                //        fullname = row["fullname"].ToString(),
                //        departmentid = row["departmentid"].ToString(),
                //        parent = row["parentid"].ToString(),
                //        total=Convert.ToDecimal(row["total"].ToString())
                //    };
                //    for (int i = 0; i < datamode.Count; i++)
                //    {

                //    }
                //}

                //TreeListForHiddenSituation tentity = new TreeListForHiddenSituation();
                //tentity.changedutydepartcode = row["encode"].ToString();
                //tentity.fullname = row["fullname"].ToString();
                //tentity.departmentid = row["departmentid"].ToString();
                //if (row["parentid"].ToString() != "0")
                //{
                //    tentity.parent = row["parentid"].ToString();
                //}
                //tentity.total = Convert.ToDecimal(row["total"].ToString());
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", row["departmentid"].ToString())).Count() == 0 ? false : true;
                newDr["haschild"] = hasChildren;
                newDt.Rows.Add(newDr);
                //tentity.haschild = hasChildren;
                tree.id = row["departmentid"].ToString();
                tree.parentId = row["parentid"].ToString();
                string itemJson = newDt.ToJson().Replace("[", "").Replace("]", "");
                //string itemJson = tentity.ToJson();
                tree.entityJson = itemJson;
                tree.expanded = false;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }

            //结果集合
            return Content(treeList.TreeJson(curUser.ParentId));
        }
        #endregion
        public void DrillplanStatExportExcel(string queryJson)
        {
            DataTable dt = new DataTable();
            var queryParam = queryJson.ToJObject();
            var curUser = new OperatorProvider().Current(); //当前用户
            var isCompany = false;
            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
            {
                isCompany = true;
            }
            else
            {
                isCompany = false;
            }
            string deptCode = queryJson.Contains("deptCode") ? queryParam["deptCode"].ToString() == "" ? curUser.DeptCode : queryParam["deptCode"].ToString() : "";  //部门编码
            string startTime = queryJson.Contains("startTime") ? queryParam["startTime"].ToString() : "";  //起始日期 
            string endTime = queryJson.Contains("endTime") ? queryParam["endTime"].ToString() : "";//截止日期


            ////设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            ////每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.Title = "应急演练次数统计";
            excelconfig.FileName = "应急演练次数统计.xls";
            var datamode = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_DirllMode'").ToList();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "部门名称", Width = 20 });
            if (datamode.Count > 0)
            {
                for (int i = 0; i < datamode.Count; i++)
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "recordnum" + (i + 1), ExcelColumn = datamode[i].ItemName, Width = 20 });
                }
            }
            else
            {
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "recordnum1", ExcelColumn = "桌面演练", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "recordnum2", ExcelColumn = "实战演练", Width = 20 });
            }
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "合计", Width = 20 });
            dt = drillplanrecordbll.DrillplanStatList("", isCompany, deptCode, startTime, endTime);
            ////调用导出方法
            ExcelHelper.ExcelDownload(dt, excelconfig);
        }

        [HttpGet]
        public static string GetResult(string keyword)
        {

            string[] key = keyword.Split(' ');
            StringBuilder sql = new StringBuilder();
            string returnStr = "";
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    returnStr += i + ";";
                }
            }
            catch { }
            return returnStr;
        }

        public class dseries
        {
            public string name { get; set; }
            public string id { get; set; }
            public List<dseries_child> data { get; set; }
        }

        public class dseries_child
        {
            public string name { get; set; }
            public int y { get; set; }
            public string drilldown { get; set; }
        }
    }
}
