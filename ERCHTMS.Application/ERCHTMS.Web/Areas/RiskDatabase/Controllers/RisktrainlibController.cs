using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Web;
using System;
using System.Data;
using System.Linq;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// 描 述：风险预知训练库
    /// </summary>
    public class RisktrainlibController : MvcControllerBase
    {
        private RisktrainlibBLL risktrainlibbll = new RisktrainlibBLL();
        private RisktrainlibdetailBLL risktrainlibdetailbll = new RisktrainlibdetailBLL();
        private DistrictBLL districtBLL = new DistrictBLL();
        private DangerSourceBLL dangerBLL = new DangerSourceBLL();

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
        [HttpGet]
        public ActionResult Import()
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
        [HttpGet]
        public ActionResult SelectTrianLib()
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
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_tablename = "bis_risktrainlib";
            pagination.p_kid = "id";
            pagination.p_fields = @"worktask,worktype,workpost,workarea,workareaid,workdes,usernum,modifynum,
resources,createusername,createdate,createuserdeptcode,createuserorgcode,createuserid,worktypecode,risklevel,risklevelval";
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var data = risktrainlibbll.GetPageListJson(pagination, queryJson);
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
        [HttpGet]
        public ActionResult GetPageJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_tablename = "bis_risktrainlib";
            pagination.p_kid = "id";
            pagination.p_fields = @"worktask,worktype,workpost,workarea,workareaid,workdes,usernum,modifynum,resources,createusername,createdate,createuserdeptcode,createuserorgcode,createuserid,risklevelval";
            pagination.conditionJson += "  createuserorgcode=" + user.OrganizeCode;
            var data = risktrainlibbll.GetPageListJson(pagination, queryJson);
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
            var data = risktrainlibbll.GetList(queryJson);
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
            var data = risktrainlibbll.GetEntity(keyValue);
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
            risktrainlibbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 删除来源风险库数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DelRiskData()
        {
            if (risktrainlibbll.DelRiskData())
            {
                return Success("删除成功。");
            }
            else
            {
                return Error("删除失败。");
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
        public ActionResult SaveForm(string keyValue, RisktrainlibEntity entity, string measuresJson)
        {
            List<RisktrainlibdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RisktrainlibdetailEntity>>(measuresJson);
            entity.DataSources = "3";//本地修改或新增所有数据来源统一修改为3
            risktrainlibbll.SaveForm(keyValue, entity, list);
            return Success("操作成功。");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "导入风险训练库")]
        public string ImportTrainLib()
        {
            try
            {
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    return "超级管理员无此操作权限";
                }
                string orgId = OperatorProvider.Provider.Current().OrganizeId;
                int error = 0;
                string message = "请选择格式正确的文件再导入!";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
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
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName),file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")?Aspose.Cells.FileFormatType.Excel2007Xlsx:Aspose.Cells.FileFormatType.Excel2003);
                    var sheet = wb.Worksheets[0];
                    if (sheet.Cells[1, 1].StringValue != "工作任务" || sheet.Cells[1, 2].StringValue != "风险等级" || sheet.Cells[1, 3].StringValue != "作业类型"
                        || sheet.Cells[1, 4].StringValue != "作业岗位" || sheet.Cells[1, 5].StringValue != "任务描述" || sheet.Cells[1, 6].StringValue != "资源准备"
                        || sheet.Cells[1, 7].StringValue != "作业区域" || sheet.Cells[1, 8].StringValue != "工序" || sheet.Cells[1, 9].StringValue != "潜在危险" || sheet.Cells[1, 10].StringValue != "防范措施")
                    {
                        return message;
                    }
                    var RiskLibList = new List<RisktrainlibEntity>();
                    var RiskDetailList = new List<RisktrainlibdetailEntity>();
                    var date = DateTime.Now;
                    for (int i = 2; i <= sheet.Cells.MaxDataRow; i++)
                    {
                        var entity = new RisktrainlibEntity();
                        entity.Create();
                        entity.WorkTask = sheet.Cells[i, 1].StringValue;
                        entity.RiskLevel = sheet.Cells[i, 2].StringValue;
                        switch (entity.RiskLevel)
                        {
                            case "重大风险":
                                entity.RiskLevelVal = "1";
                                break;
                            case "较大风险":
                                entity.RiskLevelVal = "2";
                                break;
                            case "一般风险":
                                entity.RiskLevelVal = "3";
                                break;
                            case "低风险":
                                entity.RiskLevelVal = "4";
                                break;
                            default:
                                break;
                        }
                        entity.WorkType = sheet.Cells[i, 3].StringValue;
                        entity.WorkPost = sheet.Cells[i, 4].StringValue;
                        entity.WorkDes = sheet.Cells[i, 5].StringValue;
                        entity.Resources = sheet.Cells[i, 6].StringValue;
                        entity.WorkArea = sheet.Cells[i, 7].StringValue;
                        entity.DataSources = "2";
                        if (string.IsNullOrEmpty(sheet.Cells[i, 1].StringValue))
                        {
                            entity.ID = RiskLibList[i - 1 - 2].ID;
                        }
                        RiskLibList.Add(entity);
                    }

                    for (int i = 2; i <= sheet.Cells.MaxDataRow; i++)
                    {
                        var dentity = new RisktrainlibdetailEntity();
                        if (sheet.Cells[i, 8].StringValue.Length > 1000)
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行工序字符长度超长,未能导入.";
                            error++;
                            continue;
                        }
                        if (sheet.Cells[i, 9].StringValue.Length > 1000)
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行存在风险字符长度超长,未能导入.";
                            error++;
                            continue;
                        }
                        if (sheet.Cells[i, 10].StringValue.Length > 1000)
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行管理措施字符长度超长,未能导入.";
                            error++;
                            continue;
                        }
                        dentity.Process = sheet.Cells[i, 8].StringValue;
                        dentity.AtRisk = sheet.Cells[i, 9].StringValue;
                        dentity.Controls = sheet.Cells[i, 10].StringValue;
                        dentity.Create();

                        dentity.WorkId = RiskLibList[i - 2].ID;

                        RiskDetailList.Add(dentity);
                    }
                    RiskLibList = RiskLibList.Where(x => x.WorkTask != "").ToList();
                    for (int i = 0; i < RiskLibList.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(RiskLibList[i].WorkType))
                        {
                            var data = new DataItemDetailBLL().GetDataItemListByItemCode("'StatisticsType'");
                            string worktype = string.Empty;
                            var list = RiskLibList[i].WorkType.Split(',');
                            for (int k = 0; k < list.Length; k++)
                            {
                                var entity = data.Where(x => x.ItemName == list[k].Trim()).FirstOrDefault();
                                if (entity != null)
                                {
                                    if (string.IsNullOrWhiteSpace(worktype))
                                    {

                                        RiskLibList[i].WorkTypeCode += entity.ItemValue + ",";
                                        worktype += entity.ItemName + ",";

                                    }
                                    else
                                    {
                                        if (!worktype.Contains(entity.ItemName))
                                        {
                                            RiskLibList[i].WorkTypeCode += entity.ItemValue + ",";
                                            worktype += entity.ItemName + ",";
                                        }
                                    }
                                }
                                //if (entity != null)
                                //{
                                //    RiskLibList[i].WorkTypeCode += entity.ItemValue + ",";
                                //}
                                //else
                                //{
                                //    RiskLibList[i].WorkType.Replace(list[k], "");
                                //}
                            }
                            if (!string.IsNullOrWhiteSpace(RiskLibList[i].WorkTypeCode))
                            {
                                RiskLibList[i].WorkTypeCode = RiskLibList[i].WorkTypeCode.Substring(0, RiskLibList[i].WorkTypeCode.Length - 1);
                                RiskLibList[i].WorkType = worktype.Substring(0, worktype.Length - 1);
                            }
                        }
                        if (string.IsNullOrWhiteSpace(RiskLibList[i].WorkArea))
                        {
                            //falseMessage += "</br>" + "第" + (i+1) + "行区域为空,未能导入.";
                            //error++;
                            //continue;
                        }
                        else
                        {
                            DistrictEntity disEntity = districtBLL.GetDistrict(orgId, RiskLibList[i].WorkArea);
                            if (disEntity == null)
                            {
                                //电厂没有该区域则不赋值
                                RiskLibList[i].WorkArea = "";
                            }
                            else
                            {
                                RiskLibList[i].WorkAreaId = disEntity.DistrictID;
                            }
                        }
                    }
                    risktrainlibbll.InsertImportData(RiskLibList, RiskDetailList);
                    //risktrainlibbll.InsertRiskTrainLib(RiskLibList);
                    //risktrainlibdetailbll.InsertRiskTrainDetailLib(RiskDetailList);
                    count = RiskDetailList.Count;
                    message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                    message += "</br>" + falseMessage;
                    //DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                }
                return message;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            //return null;
        }
        /// <summary>
        /// 预知风险训练库导出详情
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportData(string queryJson, string fileName)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            //string path = "~/Resource/Temp";
            string fName = "作业安全分析库" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/危险预知训练数据库导入模版.xlsx"));
            var queryParam = queryJson.ToJObject();
            //var riskType = queryParam["riskType"].ToString();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 10000000;
            pagination.sidx = "t.createdate,t.id";
            pagination.sord = "desc";
            pagination.p_tablename = "bis_risktrainlib t left join bis_risktrainlibdetail d on d.workid = t.id";
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.worktask,t.risklevel,t.worktype,t.workpost,t.workdes,
t.resources,t.workarea,d.process,d.atrisk,d.controls";
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "t.createuserdeptcode", "t.createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var data = risktrainlibbll.GetPageListJson(pagination, queryJson);
            var cells = wb.Worksheets[0].Cells;
            int Colnum = data.Columns.Count;
            int Rownum = data.Rows.Count;
            for (int i = 0; i < Rownum; i++)
            {
                for (int k = 0; k < Colnum - 1; k++)
                {
                    if (k == 0)
                    {
                        cells[2 + i, k].PutValue(i + 1);
                    }
                    else
                    {
                        cells[2 + i, k].PutValue(data.Rows[i][k].ToString());
                    }
                }
            }
            int q = 0;
            int RowOrder = 0;
            int m = 1;
            for (int i = 0; i < data.Rows.Count; i = q)
            {
                RowOrder = data.Select(string.Format("id='{0}'", data.Rows[i]["id"].ToString())).ToList().Count;
                cells.Merge(2 + q, 0, RowOrder, 1);
                cells.Merge(2 + q, 1, RowOrder, 1);
                cells.Merge(2 + q, 2, RowOrder, 1);
                cells.Merge(2 + q, 3, RowOrder, 1);
                cells.Merge(2 + q, 4, RowOrder, 1);
                cells.Merge(2 + q, 5, RowOrder, 1);
                cells.Merge(2 + q, 6, RowOrder, 1);
                cells.Merge(2 + q, 7, RowOrder, 1);
                cells[2 + q, 0].PutValue(m);
                m++;
                q += RowOrder;
            }
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            System.Threading.Thread.Sleep(400);
            wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
            return Success("导出成功。", fName);
        }

        #endregion
    }
}
