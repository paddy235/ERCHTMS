using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.SafeReward;
using ERCHTMS.Busines.SafeReward;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Entity.OutsourcingProject;
using Newtonsoft.Json;
using System.Web;
using Aspose.Words;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.SafeReward.Controllers
{
    /// <summary>
    /// 描 述：安全奖励
    /// </summary>
    public class SaferewardController : MvcControllerBase
    {
        private SaferewardBLL saferewardbll = new SaferewardBLL();
        private SaferewarddetailBLL saferewarddetailbll = new SaferewarddetailBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();

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
        /// 奖惩标准
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RewardStandard()
        {
            return View();
        }

        /// <summary>
        /// 奖励统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RewardStatistics()
        {
            return View();
        }

        /// <summary>
        /// 奖励流程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
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
        /// 奖励详细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RewardDetail()
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
           // Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "Id";
            pagination.p_fields = @"CreateUserId,CreateDate,CreateUserName,ModifyUserId,ModifyDate,ModifyUserName,CreateUserDeptCode,CreateUserOrgCode,FlowState,ApplyUserId,ApproverPeopleIds,
                                case when ApplyState= 0 then '申请中'
                                   when ApplyState= 1 then '专业审核' 
                                      when ApplyState= 2 then '部门审核' 
                                      when ApplyState= 3 then 'EHS部审核' 
                                        when ApplyState= 4 then '分管领导审核' 
                                          when ApplyState= 5 then '总经理审核'
                                            when ApplyState= 6 then '已完成' end as  ApplyState,SafeRewardCode,RewardUserName,ApplyDeptName,ApplyTime,ApplyRewardRmb,RewardRemark,belongdept,applyusername";
            pagination.p_tablename = "Bis_SafeReward";
            pagination.sidx = "CreateDate";//排序字段
            pagination.sord = "desc";//排序方式
            pagination.conditionJson = "1=1";
            var data = saferewardbll.GetPageList(pagination, queryJson);
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
        /// 获取奖励标准
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetStandardJson()
        {
            var data = saferewardbll.GetStandardJson();
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
            var data = saferewardbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        ///获取统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet]
        public string GetRewardStatisticsCount(string year = "")
        {
            return saferewardbll.GetRewardStatisticsCount(year);
        }


        /// <summary>
        ///获取奖励次数统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet]
        public string GetRewardStatisticsTime(string year = "")
        {
            return saferewardbll.GetRewardStatisticsTime(year);
        }


        /// <summary>
        ///获取统计数据(列表)
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRewardStatisticsList(string year = "")
        {
            try
            {
                var curUser = new OperatorProvider().Current(); //当前用户  
                var treeList = new List<TreeGridEntity>();
                var dt = saferewardbll.GetRewardStatisticsList(year);

                foreach (DataRow row in dt.Rows)
                {
                    DataTable newDt = new DataTable();
                    newDt.Columns.Add("changedutydepartcode");
                    newDt.Columns.Add("fullname");
                    newDt.Columns.Add("departmentid");
                    newDt.Columns.Add("parent");
                    newDt.Columns.Add("total");
                    newDt.Columns.Add("haschild", typeof(bool));
                    newDt.Columns.Add("january");
                    newDt.Columns.Add("february");
                    newDt.Columns.Add("march");
                    newDt.Columns.Add("april");
                    newDt.Columns.Add("may");
                    newDt.Columns.Add("june");
                    newDt.Columns.Add("july");
                    newDt.Columns.Add("august");
                    newDt.Columns.Add("september");
                    newDt.Columns.Add("october");
                    newDt.Columns.Add("november");
                    newDt.Columns.Add("december");

                    DataRow newDr = newDt.NewRow();
                    newDr["changedutydepartcode"] = row["encode"].ToString();
                    newDr["fullname"] = row["fullname"].ToString();
                    newDr["departmentid"] = row["departmentid"].ToString();
                    if (row["parentid"].ToString() != "0")
                    {
                        newDr["parent"] = row["parentid"].ToString();
                    }
                    newDr["total"] = row["total"].ToString();
                    newDr["january"] = row["january"].ToString();
                    newDr["february"] = row["february"].ToString();
                    newDr["march"] = row["march"].ToString();
                    newDr["april"] = row["april"].ToString();
                    newDr["may"] = row["may"].ToString();
                    newDr["june"] = row["june"].ToString();
                    newDr["july"] = row["july"].ToString();
                    newDr["august"] = row["august"].ToString();
                    newDr["september"] = row["september"].ToString();
                    newDr["october"] = row["october"].ToString();
                    newDr["november"] = row["november"].ToString();
                    newDr["december"] = row["december"].ToString();
                    TreeGridEntity tree = new TreeGridEntity();
                    bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", row["departmentid"].ToString())).Count() == 0 ? false : true;
                    //string[] january = dt.Select(string.Format(" parentid ='{0}'", row["departmentid"].ToString())).Select(d => d.Field<string>("january")).ToArray();
                    //foreach (var item in january)
                    //{
                    //    newDr["january"] = (Convert.ToInt32(newDr["january"].ToString()) + Convert.ToInt32(item.ToString())).ToString();
                    //}
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
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        /// <summary>
        ///获取奖励次数数据(列表)
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRewardStatisticsTimeList(string year = "")
        {
            try
            {
                var curUser = new OperatorProvider().Current(); //当前用户  
                var treeList = new List<TreeGridEntity>();
                var dt = saferewardbll.GetRewardStatisticsTimeList(year);

                foreach (DataRow row in dt.Rows)
                {
                    DataTable newDt = new DataTable();
                    newDt.Columns.Add("changedutydepartcode");
                    newDt.Columns.Add("fullname");
                    newDt.Columns.Add("departmentid");
                    newDt.Columns.Add("parent");
                    newDt.Columns.Add("total");
                    newDt.Columns.Add("haschild", typeof(bool));
                    newDt.Columns.Add("january");
                    newDt.Columns.Add("february");
                    newDt.Columns.Add("march");
                    newDt.Columns.Add("april");
                    newDt.Columns.Add("may");
                    newDt.Columns.Add("june");
                    newDt.Columns.Add("july");
                    newDt.Columns.Add("august");
                    newDt.Columns.Add("september");
                    newDt.Columns.Add("october");
                    newDt.Columns.Add("november");
                    newDt.Columns.Add("december");

                    DataRow newDr = newDt.NewRow();
                    newDr["changedutydepartcode"] = row["encode"].ToString();
                    newDr["fullname"] = row["fullname"].ToString();
                    newDr["departmentid"] = row["departmentid"].ToString();
                    if (row["parentid"].ToString() != "0")
                    {
                        newDr["parent"] = row["parentid"].ToString();
                    }
                    newDr["total"] = row["total"].ToString();
                    newDr["january"] = row["january"].ToString();
                    newDr["february"] = row["february"].ToString();
                    newDr["march"] = row["march"].ToString();
                    newDr["april"] = row["april"].ToString();
                    newDr["may"] = row["may"].ToString();
                    newDr["june"] = row["june"].ToString();
                    newDr["july"] = row["july"].ToString();
                    newDr["august"] = row["august"].ToString();
                    newDr["september"] = row["september"].ToString(); 
                    newDr["october"] = row["october"].ToString();
                    newDr["november"] = row["november"].ToString();
                    newDr["december"] = row["december"].ToString();
                    TreeGridEntity tree = new TreeGridEntity();
                    bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", row["departmentid"].ToString())).Count() == 0 ? false : true;
                    //string[] january = dt.Select(string.Format(" parentid ='{0}'", row["departmentid"].ToString())).Select(d => d.Field<string>("january")).ToArray();
                    //foreach (var item in january)
                    //{
                    //    newDr["january"] = (Convert.ToInt32(newDr["january"].ToString()) + Convert.ToInt32(item.ToString())).ToString();
                    //}
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
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }



        /// <summary>
        /// 导出安全奖励excel
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出安全奖励excel")]
        public ActionResult ExportSafeRewardExcel(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "Id";
            pagination.p_fields = @"case when ApplyState= 0 then '申请中'
            when ApplyState= 1 then '专业意见' 
            when ApplyState= 2 then '部门意见' 
            when ApplyState= 3 then 'EHS部意见' 
            when ApplyState= 4 then '分管领导审批' 
            when ApplyState= 5 then '总经理审批'
            when ApplyState= 6 then '已完成' end as  applyState,saferewardcode,applyusername,belongdept,to_char(applytime,'yyyy-MM-dd HH24:mi:ss') applytime,applyrewardrmb,rewardremark ";
            pagination.p_tablename = "bis_safereward";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CreateDate";
            pagination.sord = "desc";
            var data = saferewardbll.GetPageList(pagination, queryJson);


            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "安全奖励";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "安全奖励" + ".xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            excelconfig.ColumnEntity = new List<ColumnEntity>()
            {
                new ColumnEntity() {Column = "applyState", ExcelColumn = "流程状态", Alignment = "center"},
                new ColumnEntity() {Column = "saferewardcode", ExcelColumn = "奖励编号", Alignment = "center"},
                new ColumnEntity() {Column = "applyusername", ExcelColumn = "申请人", Alignment = "center"},
                new ColumnEntity() {Column = "belongdept", ExcelColumn = "所属专业(部门)", Alignment = "center"},
                new ColumnEntity() {Column = "applytime", ExcelColumn = "申请时间", Alignment = "center"},             
                new ColumnEntity() {Column = "rewardremark", ExcelColumn = "事件描述", Alignment = "center"}
            };

            //调用导出方法
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, excelconfig.ColumnEntity);
            return Success("导出成功。");
        }


        /// <summary>
        /// 导出安全奖励金额统计excel
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出安全奖励金额统计excel")]
        public ActionResult ExportStatisticsExcel(string year)
        {
            string jsonList = saferewardbll.GetRewardStatisticsExcel(year);

            dynamic dyObj = JsonConvert.DeserializeObject(jsonList);
            ;
            DataTable tb = JsonToDataTable(dyObj.rows.ToString());

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "安全奖励";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = "安全奖励金额统计";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                excelconfig.ColumnEntity = new List<ColumnEntity>()
                {
                    new ColumnEntity() {Column = "DeptName", ExcelColumn = "奖励单位", Alignment = "center"},
                    new ColumnEntity() {Column = "num1", ExcelColumn = "1月", Alignment = "center"},
                    new ColumnEntity() {Column = "num2", ExcelColumn = "2月", Alignment = "center"},
                    new ColumnEntity() {Column = "num3", ExcelColumn = "3月", Alignment = "center"},
                    new ColumnEntity() {Column = "num4", ExcelColumn = "4月", Alignment = "center"},
                    new ColumnEntity() {Column = "num5", ExcelColumn = "5月", Alignment = "center"},
                    new ColumnEntity() {Column = "num6", ExcelColumn = "6月", Alignment = "center"},
                    new ColumnEntity() {Column = "num7", ExcelColumn = "7月", Alignment = "center"},
                    new ColumnEntity() {Column = "num8", ExcelColumn = "8月", Alignment = "center"},
                    new ColumnEntity() {Column = "num9", ExcelColumn = "9月", Alignment = "center"},
                    new ColumnEntity() {Column = "num10", ExcelColumn = "10月", Alignment = "center"},
                    new ColumnEntity() {Column = "num11", ExcelColumn = "11月", Alignment = "center"},
                    new ColumnEntity() {Column = "num12", ExcelColumn = "12月", Alignment = "center"},
                    new ColumnEntity() {Column = "Total", ExcelColumn = "总计(元)", Alignment = "center"},
                };

                //调用导出方法
                ExcelHelper.ExportByAspose(tb, excelconfig.FileName, excelconfig.ColumnEntity);
                return Success("导出成功。");
        }


        /// <summary>
        /// 导出安全次数统计excel
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出安全奖励次数统计excel")]
        public ActionResult ExportStatisticsTimeExcel(string year)
        {
            string jsonList = saferewardbll.GetRewardStatisticsTimeExcel(year);

            dynamic dyObj = JsonConvert.DeserializeObject(jsonList);
            ;
            DataTable tb = JsonToDataTable(dyObj.rows.ToString());

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "安全奖励";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "安全奖励次数统计";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            excelconfig.ColumnEntity = new List<ColumnEntity>()
                {
                    new ColumnEntity() {Column = "DeptName", ExcelColumn = "奖励单位", Alignment = "center"},
                    new ColumnEntity() {Column = "num1", ExcelColumn = "1月", Alignment = "center"},
                    new ColumnEntity() {Column = "num2", ExcelColumn = "2月", Alignment = "center"},
                    new ColumnEntity() {Column = "num3", ExcelColumn = "3月", Alignment = "center"},
                    new ColumnEntity() {Column = "num4", ExcelColumn = "4月", Alignment = "center"},
                    new ColumnEntity() {Column = "num5", ExcelColumn = "5月", Alignment = "center"},
                    new ColumnEntity() {Column = "num6", ExcelColumn = "6月", Alignment = "center"},
                    new ColumnEntity() {Column = "num7", ExcelColumn = "7月", Alignment = "center"},
                    new ColumnEntity() {Column = "num8", ExcelColumn = "8月", Alignment = "center"},
                    new ColumnEntity() {Column = "num9", ExcelColumn = "9月", Alignment = "center"},
                    new ColumnEntity() {Column = "num10", ExcelColumn = "10月", Alignment = "center"},
                    new ColumnEntity() {Column = "num11", ExcelColumn = "11月", Alignment = "center"},
                    new ColumnEntity() {Column = "num12", ExcelColumn = "12月", Alignment = "center"},
                    new ColumnEntity() {Column = "Total", ExcelColumn = "总计(次)", Alignment = "center"},
                };

            //调用导出方法
            ExcelHelper.ExportByAspose(tb, excelconfig.FileName, excelconfig.ColumnEntity);
            return Success("导出成功。");
        }


        #region 获取安全奖励流程图对象
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetWorkActionList(string keyValue)
        {
            var josnData = saferewardbll.GetFlow(keyValue);
            return Content(josnData.ToJson());
        }

        /// <summary>
        /// 导出安全奖励详细
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportSafeRewardInfo(string keyValue)
        {

            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象

            string fileName = "安全奖励审批单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\安全奖励导出模板.docx";
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("RewardCode"); //编号
            dt.Columns.Add("ApplyUserName"); //申请人
            dt.Columns.Add("ApplyTime"); //申请时间
            dt.Columns.Add("BelongDept"); //被奖励部门
            dt.Columns.Add("ReardNum"); //申请奖励金额
            dt.Columns.Add("RewardName"); //奖励对象名称
            dt.Columns.Add("RewardRemark"); //事件描述
            dt.Columns.Add("approve1");//第一步审核意见
            dt.Columns.Add("approvename1");//第一步审核人
            dt.Columns.Add("approvetime1");//第一步审核时间
            dt.Columns.Add("approve2");//第二步审核意见
            dt.Columns.Add("approvename2");//第二步审核人
            dt.Columns.Add("approvetime2");//第二步审核时间
            dt.Columns.Add("approve3");//第三步审核意见
            dt.Columns.Add("approvename3");//第三步审核人
            dt.Columns.Add("approvetime3");//第三步审核时间
            dt.Columns.Add("approve4");//第四步审核意见
            dt.Columns.Add("approvename4");//第四步审核人
            dt.Columns.Add("approvetime4");//第四步审核时间
            dt.Columns.Add("approve5");//第五步审核意见
            dt.Columns.Add("approvename5");//第五步审核人
            dt.Columns.Add("approvetime5");//第五步审核时间
            DataRow row = dt.NewRow();


            //安全奖励信息
            SaferewardEntity saferewardentity = saferewardbll.GetEntity(keyValue);
            row["RewardCode"] = saferewardentity.SafeRewardCode;
            row["ApplyUserName"] = saferewardentity.ApplyUserName;
            row["ApplyTime"] = saferewardentity.ApplyTime.IsEmpty() ? "" : Convert.ToDateTime(saferewardentity.ApplyTime).ToString("yyyy-MM-dd");
            row["RewardRemark"] = saferewardentity.RewardRemark;

            row["approve1"] = saferewardentity.SpecialtyOpinion;
            row["approvetime1"] = saferewardentity.CreateDate.IsEmpty() ? "" : Convert.ToDateTime(saferewardentity.CreateDate).ToString("yyyy-MM-dd");
            UserEntity createuser = new UserBLL().GetEntity(saferewardentity.CreateUserId);
            if (createuser.SignImg.IsEmpty())
            {
                row["approvename1"] = Server.MapPath("~/content/Images/no_1.png");
            }
            else
            {
                var filepath = Server.MapPath("~/") + createuser.SignImg.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    row["approvename1"] = filepath;
                }
                else
                {
                    row["approvename1"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }
            builder.MoveToMergeField("approvename1");
            builder.InsertImage(row["approvename1"].ToString(), 80, 35);
            var flist = fileinfobll.GetImageListByRecid(keyValue);
            builder.MoveToMergeField("RewardImage");
            foreach (FileInfoEntity fmode in flist)
            {
                string path = "";
                if (string.IsNullOrWhiteSpace(fmode.FilePath))
                {
                    path = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + fmode.FilePath.Replace("~/", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        path = filepath;
                    }
                    else
                    {
                        path = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                builder.MoveToMergeField("RewardImage");
                builder.InsertImage(path, 200, 160);
            }

            //获取被考核对象
            SaferewarddetailEntity SaferewarddetailEntity = saferewarddetailbll.GetListByRewardId(keyValue).OrderBy(t => t.CreateDate).FirstOrDefault();
            row["BelongDept"] = departmentbll.GetEntity(SaferewarddetailEntity.BelongDept) == null ? "" : departmentbll.GetEntity(SaferewarddetailEntity.BelongDept).FullName;
            row["ReardNum"] = SaferewarddetailEntity.RewardNum;
            row["RewardName"] = SaferewarddetailEntity.RewardName;
            DataTable dtAptitude = saferewardbll.GetAptitudeInfo(keyValue);
            int count = dtAptitude.Rows.Count;
            for (int i = dtAptitude.Rows.Count - 1; i > 0; i--)
            {
                if (i==(dtAptitude.Rows.Count-2))
                {
                    row["approve5"] = dtAptitude.Rows[i]["auditremark"];
                    row["approvetime5"] = dtAptitude.Rows[i]["auditdate"].IsEmpty() ? "" : Convert.ToDateTime(dtAptitude.Rows[i]["auditdate"]).ToString("yyyy-MM-dd");
                    if (dtAptitude.Rows[i]["auditsignimg"].IsEmpty())
                    {
                        row["approvename5"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + dtAptitude.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["approvename5"] = filepath;
                        }
                        else
                        {
                            row["approvename5"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    builder.MoveToMergeField("approvename5");
                    builder.InsertImage(row["approvename5"].ToString(), 80, 35);
                }
                else if (i==(dtAptitude.Rows.Count-3))
                {
                    row["approve4"] = dtAptitude.Rows[i]["auditremark"];
                    row["approvetime4"] = dtAptitude.Rows[i]["auditdate"].IsEmpty() ? "" : Convert.ToDateTime(dtAptitude.Rows[i]["auditdate"]).ToString("yyyy-MM-dd");
                    if (dtAptitude.Rows[i]["auditsignimg"].IsEmpty())
                    {
                        row["approvename4"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + dtAptitude.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["approvename4"] = filepath;
                        }
                        else
                        {
                            row["approvename4"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    builder.MoveToMergeField("approvename4");
                    builder.InsertImage(row["approvename4"].ToString(), 80, 35);
                }
                else if (i==(dtAptitude.Rows.Count-4))
                {
                    row["approve3"] = dtAptitude.Rows[i]["auditremark"];
                    row["approvetime3"] = dtAptitude.Rows[i]["auditdate"].IsEmpty() ? "" : Convert.ToDateTime(dtAptitude.Rows[i]["auditdate"]).ToString("yyyy-MM-dd");
                    if (dtAptitude.Rows[i]["auditsignimg"].IsEmpty())
                    {
                        row["approvename3"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + dtAptitude.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["approvename3"] = filepath;
                        }
                        else
                        {
                            row["approvename3"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    builder.MoveToMergeField("approvename3");
                    builder.InsertImage(row["approvename3"].ToString(), 80, 35);
                }
                else if (i==(dtAptitude.Rows.Count-5))
                {
                    row["approve2"] = dtAptitude.Rows[i]["auditremark"];
                    row["approvetime2"] = dtAptitude.Rows[i]["auditdate"].IsEmpty() ? "" : Convert.ToDateTime(dtAptitude.Rows[i]["auditdate"]).ToString("yyyy-MM-dd");
                    if (dtAptitude.Rows[i]["auditsignimg"].IsEmpty())
                    {
                        row["approvename2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + dtAptitude.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["approvename2"] = filepath;
                        }
                        else
                        {
                            row["approvename2"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    builder.MoveToMergeField("approvename2");
                    builder.InsertImage(row["approvename2"].ToString(), 80, 35);
                }
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();

            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return Success("导出成功!");
        }

        #endregion


        #region 获取分管领导

        /// <summary>
        /// 获取分管领导
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLeaderList()
        {
            var data = saferewardbll.GetLeaderList();
            return Content(data.ToJson());
        }

        #endregion

        /// <summary>
        /// 根据业务id获取对应的审核记录列表 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetSpecialAuditList(string keyValue)
        {
            var data = new AptitudeinvestigateauditBLL().GetAuditList(keyValue).Where(p=>p.REMARK!="0").ToList();
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetRewardCode()
        {
            var data = saferewardbll.GetRewardCode();
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取部门id(部门层级)
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetDeptPId(string applyDeptId)
        {
            if (!string.IsNullOrEmpty(applyDeptId))
            {
                var data = saferewardbll.GetDeptPId(applyDeptId);
                return ToJsonResult(data);
            }

            return ToJsonResult("");
        }
        

        /// <summary>
        /// 返回待办事项数量
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetRewardNum()
        {
            var data = saferewardbll.GetRewardNum();
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
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            saferewardbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SaferewardEntity entity, [System.Web.Http.FromBody]string dataJson)
        {

            var year = DateTime.Now.ToString("yyyy");
            var month = DateTime.Now.ToString("MM");
            var day = DateTime.Now.ToString("dd");
            var rewardCode = "Q/CRPHZHB 2208.06.01-JL01-" + year + month + day + saferewardbll.GetRewardCode();
            entity.SafeRewardCode = !string.IsNullOrEmpty(entity.SafeRewardCode) ? entity.SafeRewardCode : rewardCode;
            int? rewardmoney = 0;
            if (dataJson.Length > 0)
            {
                if (saferewarddetailbll.Remove(keyValue) > 0)
                {
                    List<SaferewarddetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaferewarddetailEntity>>(dataJson);
                    foreach (SaferewarddetailEntity data in list)
                    {
                        data.RewardId = keyValue;
                        saferewarddetailbll.SaveForm("", data);
                        rewardmoney += data.RewardNum;
                    }
                }
            }
            entity.RewardMoney = rewardmoney;
            saferewardbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 提交表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">审批对象</param>
        /// <param name="rewEntity">奖励信息</param>
        /// <param name="leaderShipId">分管领导</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CommitApply(string keyValue, AptitudeinvestigateauditEntity entity, SaferewardEntity rewEntity, string leaderShipId, [System.Web.Http.FromBody]string dataJson)
        {
            if (rewEntity != null && (string.IsNullOrEmpty(rewEntity.ApplyState) || rewEntity.ApplyState == "0"))
            {
                var year = DateTime.Now.ToString("yyyy");
                var month = DateTime.Now.ToString("MM");
                var day = DateTime.Now.ToString("dd");
                var rewardCode = "Q/CRPHZHB 2208.06.01-JL01-" + year + month + day + saferewardbll.GetRewardCode();
                rewEntity.SafeRewardCode = !string.IsNullOrEmpty(rewEntity.SafeRewardCode) ? rewEntity.SafeRewardCode : rewardCode;
                int? rewardmoney = 0;
                if (dataJson.Length > 0)
                {
                    if (saferewarddetailbll.Remove(keyValue)>0)
                    {
                        List<SaferewarddetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaferewarddetailEntity>>(dataJson);
                        foreach (SaferewarddetailEntity data in list)
                        {
                            data.RewardId = keyValue;
                            saferewarddetailbll.SaveForm("", data);
                            rewardmoney += data.RewardNum;
                        }
                    }
                }
                rewEntity.RewardMoney = rewardmoney;
                saferewardbll.SaveForm(keyValue, rewEntity);
            }

            if (!string.IsNullOrEmpty(keyValue) && entity != null)
            {
                saferewardbll.CommitApply(keyValue, entity, leaderShipId);
            }

            return Success("操作成功。");
        }
        #endregion


        #region Json 字符串 转换为 DataTable数据集合
        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合 格式[{"xxx":"yyy","x1":"yy2"},{"x2":"y2","x3":"y4"}]
        /// </summary>  
        /// <param name="json"></param>
        /// <returns></returns>
        public DataTable JsonToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        #endregion
    }
}
