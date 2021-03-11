using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.SafePunish;
using ERCHTMS.Busines.SafePunish;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Code;
using Newtonsoft.Json;
using Aspose.Words;
using System.Web;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.SafePunish.Controllers
{
    /// <summary>
    /// 描 述：安全惩罚
    /// </summary>
    public class SafepunishController : MvcControllerBase
    {
        private SafepunishBLL safepunishbll = new SafepunishBLL();
        private SafekpidataBLL safekpidatabll = new SafekpidataBLL();
        private SafepunishdetailBLL safepunishdetailbll = new SafepunishdetailBLL();
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
        public ActionResult PunishStandard()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
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
        public ActionResult PunishStatistics()
        {
            return View();
        }

        /// <summary>
        /// 考核详细页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PunishDetail()
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
               when (amercetype='1' or amercetype='2') and ApplyState= 1 then '部门审核' 
               when (amercetype='1' or amercetype='2') and ApplyState= 2 then '分管领导审核' 
                when (amercetype='3' or amercetype='4') and ApplyState= 1 then '专业审核' 
                when (amercetype='3' or amercetype='4') and ApplyState= 2 then '部门审核'              
                        when ApplyState= 3 then '已完成' end as  ApplyState,SafePunishCode,ApplyTime,PunishObjectNames,PunishType,PunishRemark,amercetype,ApplyUserName";
            pagination.p_tablename = "BIS_SAFEPUNISH";
            pagination.sidx = "CreateDate";//排序字段
            pagination.sord = "desc";//排序方式
            //pagination.conditionJson = "instr(ApproverPeopleIds,'"+user.UserId+"' )> 0";
            pagination.conditionJson = "1=1";
            var data = safepunishbll.GetPageList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            SafepunishEntity data = safepunishbll.GetEntity(keyValue);
            if (data.AmerceType == "2")
            {
                data.PunishType = "";
            }
            var kpidata = safekpidatabll.GetList("").Where(p => p.SafePunishId == keyValue).FirstOrDefault();
            object obj = new
            {
                punishdata = data,
                kpidata = kpidata
            };
            return ToJsonResult(obj);
        }



        /// <summary>
        ///获取统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet]
        public string GetPunishStatisticsCount(string year,  string statMode)
        {
            return safepunishbll.GetPunishStatisticsCount(year, statMode);
        }


        /// <summary>
        ///获取统计数据(列表)
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpPost]
        public string GetPunishStatisticsList(string year,  string statMode)
        {
            return safepunishbll.GetPunishStatisticsList(year, statMode);
        }

        /// <summary>
        /// 导出安全惩罚统计
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出安全惩罚统计")]
        public ActionResult ExportStatisticsExcel(string queryJson)
        {
            string jsonList = "";
            string mode = "";
            if (!string.IsNullOrEmpty(queryJson) && queryJson != "\"\"")
            {
                var queryParam = queryJson.ToJObject();
                mode = queryParam["statMode"].ToString();
                jsonList = safepunishbll.GetPunishStatisticsList(queryParam["year"].ToString(), queryParam["statMode"].ToString());
            }


            if (jsonList != "")
            {
                dynamic dyObj = JsonConvert.DeserializeObject(jsonList);
                DataTable tb = JsonToDataTable(dyObj.rows.ToString());

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "安全惩罚";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                if (mode == "0")
                {
                    excelconfig.FileName = "安全惩罚次数";
                }
                else if (mode == "1")
                {
                    excelconfig.FileName = "安全惩罚金额";
                }
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                excelconfig.ColumnEntity = new List<ColumnEntity>()
                {
                    new ColumnEntity() {Column = "TypeName", ExcelColumn = "事件类型", Alignment = "center"},
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
                    new ColumnEntity() {Column = "num12", ExcelColumn = "12月", Alignment = "center"}
                };
                if (mode == "0")
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Total", ExcelColumn = "总计(次)", Alignment = "center" });
                }
                else
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Total", ExcelColumn = "总计(元)", Alignment = "center" });
                }

                //调用导出方法
                ExcelHelper.ExportByAspose(tb, excelconfig.FileName, excelconfig.ColumnEntity);
                return Success("导出成功。");
            }
            else
            {
                return Success("未查询到数据。");
            }

        }
        

        /// <summary>
        /// 导出安全惩罚excel
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出安全惩罚excel")]
        public ActionResult ExportSafePunishExcel(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "Id";
            pagination.p_fields = @"case when ApplyState= 0 then '申请中'
            when (amercetype='1' or amercetype='2') and ApplyState= 1 then '部门审核' 
               when (amercetype='1' or amercetype='2') and ApplyState= 2 then '分管领导审核' 
                when (amercetype='3' or amercetype='4') and ApplyState= 1 then '专业审核' 
                when (amercetype='3' or amercetype='4') and ApplyState= 2 then '部门审核'                   
            when ApplyState= 3 then '已完成' end as  applystate,case when amercetype=1 then '事故事件' when amercetype=2 then '其他' when  amercetype=3 then '隐患排查治理' when amercetype=4 then '日常考核' end amercetype,safepunishcode,punishobjectnames,to_char(applytime,'yyyy-MM-dd HH24:mi:ss') applytime,punishremark ";
            pagination.p_tablename = "bis_safepunish a";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CreateDate";
            pagination.sord = "desc";
            var data = safepunishbll.GetPageList(pagination, queryJson);


            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "安全惩罚";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "安全惩罚" + ".xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            excelconfig.ColumnEntity = new List<ColumnEntity>()
            {
                new ColumnEntity() {Column = "applystate", ExcelColumn = "流程状态", Alignment = "center"},
                new ColumnEntity() {Column = "amercetype", ExcelColumn = "考核类型", Alignment = "center"},
                new ColumnEntity() {Column = "safepunishcode", ExcelColumn = "考核编号", Alignment = "center"},
                new ColumnEntity() {Column = "applytime", ExcelColumn = "考核时间", Alignment = "center"},
                new ColumnEntity() {Column = "punishremark", ExcelColumn = "考核具体事项", Alignment = "center"}
            };

            //调用导出方法
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, excelconfig.ColumnEntity);
            return Success("导出成功。");
        }


        /// <summary>
        /// 导出安全惩罚详细
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportSafePunishInfo(string keyValue)
        {

            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象

            string fileName = "安全惩罚审批单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\安全考核导出模板.docx";
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("PunishCode"); //编号
            dt.Columns.Add("CreateDept"); //所属专业(部门)
            dt.Columns.Add("ApplyTime"); //申请时间
            dt.Columns.Add("BelongDept"); //被考核部门
            dt.Columns.Add("PunishNum"); //考核金额
            dt.Columns.Add("PunishRemark"); //事件描述
            dt.Columns.Add("filed1"); //第一个区域名称
            dt.Columns.Add("filed2"); //第二个区域名称
            dt.Columns.Add("approve1");//第一步审核意见
            dt.Columns.Add("approvename1");//第二步审核意见
            dt.Columns.Add("approvetime1");//第一步审核意见
            dt.Columns.Add("approve2");//第二步审核意见
            dt.Columns.Add("approvename2");//第一步审核意见
            dt.Columns.Add("approvetime2");//第二步审核意见
            DataRow row = dt.NewRow();


            //安全考核信息
            SafepunishEntity safepunishentity = safepunishbll.GetEntity(keyValue);
            row["PunishCode"] = safepunishentity.SafePunishCode;
            row["PunishRemark"] = safepunishentity.PunishRemark;
            row["CreateDept"] = safepunishentity.BelongDept;
            row["ApplyTime"] = safepunishentity.ApplyTime.IsEmpty() ? "" : Convert.ToDateTime(safepunishentity.ApplyTime).ToString("yyyy-MM-dd");
            if (safepunishentity.AmerceType == "1" || safepunishentity.AmerceType == "2")
            {
                row["filed1"] = "部门意见";
                row["filed2"] = "分管领导意见";
            }
            else if (safepunishentity.AmerceType == "3" || safepunishentity.AmerceType == "4")
            {
                row["filed1"] = "专业意见";
                row["filed2"] = "部门意见";
            }

            var flist = fileinfobll.GetImageListByRecid(keyValue);
            builder.MoveToMergeField("PunishImage");
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


            row["approve1"] = safepunishentity.SpecialtyOpinion;
            row["approvetime1"] = safepunishentity.CreateDate.IsEmpty() ? "" : Convert.ToDateTime(safepunishentity.CreateDate).ToString("yyyy-MM-dd");
            UserEntity createuser = new UserBLL().GetEntity(safepunishentity.CreateUserId);
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

            //获取被考核对象
            SafepunishdetailEntity safepunishdetailentity = safepunishdetailbll.GetListByPunishId(keyValue, "0").OrderBy(t => t.CreateDate).FirstOrDefault();
            row["BelongDept"] = departmentbll.GetEntity(safepunishdetailentity.BelongDept) == null ? "" : departmentbll.GetEntity(safepunishdetailentity.BelongDept).FullName;
            row["PunishNum"] = safepunishdetailentity.PunishNum;
            DataTable dtAptitude = safepunishbll.GetAptitudeInfo(keyValue);
            for (int i = dtAptitude.Rows.Count -1; i > 0; i--)
            {
                if (i == (dtAptitude.Rows.Count - 2))
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

        #region 获取安全惩罚流程图对象
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetWorkActionList(string keyValue)
        {
            var josnData = safepunishbll.GetFlow(keyValue);
            return Content(josnData.ToJson());
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
            var data = new AptitudeinvestigateauditBLL().GetAuditList(keyValue).Where(p => p.REMARK != "0").ToList();
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetPunishCode()
        {
            var data = safepunishbll.GetPunishCode();
            return ToJsonResult(data);
        }

        /// <summary>
        /// 待办事项数量
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetPunishNum()
        {
            var data = safepunishbll.GetPunishNum();
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
            try
            {
                safepunishbll.RemoveForm(keyValue);
                return Success("删除成功。");
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
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, SafepunishEntity entity, SafekpidataEntity kpiEntity, [System.Web.Http.FromBody]string punishJson, [System.Web.Http.FromBody]string relatedpunishJson)
        {
            var year = DateTime.Now.ToString("yyyy");
            var month = DateTime.Now.ToString("MM");
            var day = DateTime.Now.ToString("dd");
            var punishCode = "Q/CRPHZHB 2208.06.01-JL02-" + year + month + day + safepunishbll.GetPunishCode();
            entity.SafePunishCode = !string.IsNullOrEmpty(entity.SafePunishCode) ? entity.SafePunishCode : punishCode;
            safepunishbll.SaveForm(keyValue, entity, kpiEntity);
            if (entity.AmerceType =="2" || entity.AmerceType=="3" || entity.AmerceType=="4")
            {
                if (punishJson.Length > 0)
                {
                    if (safepunishdetailbll.Remove(keyValue,"0") > 0)
                    {
                        List<SafepunishdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafepunishdetailEntity>>(punishJson);
                        foreach (SafepunishdetailEntity data in list)
                        {
                            safepunishdetailbll.SaveForm("", data);
                        }
                    }
                }

                if (relatedpunishJson.Length > 0)
                {
                    if (safepunishdetailbll.Remove(keyValue, "1") > 0)
                    {
                        List<SafepunishdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafepunishdetailEntity>>(relatedpunishJson);
                        foreach (SafepunishdetailEntity data in list)
                        {
                            safepunishdetailbll.SaveForm("", data);
                        }
                    }
                }
            }
            return Success("操作成功。");
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
        public ActionResult CommitApply(string keyValue, AptitudeinvestigateauditEntity entity,SafepunishEntity punEntity, SafekpidataEntity kpiEntity, [System.Web.Http.FromBody]string punishJson, [System.Web.Http.FromBody]string relatedpunishJson)
        {
            if (punEntity != null && (string.IsNullOrEmpty(punEntity.ApplyState) || punEntity.ApplyState == "0"))
            {
                var year = DateTime.Now.ToString("yyyy");
                var month = DateTime.Now.ToString("MM");
                var day = DateTime.Now.ToString("dd");
                var punishCode = "Q/CRPHZHB 2208.06.01-JL02-" + year + month + day + safepunishbll.GetPunishCode();
                punEntity.SafePunishCode = !string.IsNullOrEmpty(punEntity.SafePunishCode) ? punEntity.SafePunishCode : punishCode;
                safepunishbll.SaveForm(keyValue, punEntity, kpiEntity);
                if (punishJson.Length > 0)
                {
                    if (safepunishdetailbll.Remove(keyValue, "0") > 0)
                    {
                        List<SafepunishdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafepunishdetailEntity>>(punishJson);
                        foreach (SafepunishdetailEntity data in list)
                        {
                            safepunishdetailbll.SaveForm("", data);
                        }
                    }
                }

                if (relatedpunishJson.Length > 0)
                {
                    if (safepunishdetailbll.Remove(keyValue, "1") > 0)
                    {
                        List<SafepunishdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafepunishdetailEntity>>(relatedpunishJson);
                        foreach (SafepunishdetailEntity data in list)
                        {
                            safepunishdetailbll.SaveForm("", data);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(keyValue) && entity != null)
            {
                safepunishbll.CommitApply(keyValue, entity);
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
