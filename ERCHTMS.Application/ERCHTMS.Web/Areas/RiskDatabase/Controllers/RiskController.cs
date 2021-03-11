using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
using System.Text;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Web;
using NPOI.SS.UserModel;
using System.IO;
using System;
using System.Drawing;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using ERCHTMS.Code;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// 描 述：安全风险库
    /// </summary>
    public class RiskController : MvcControllerBase
    {
        private RiskBLL riskbll = new RiskBLL();
        private MeasuresBLL measuresBLL = new MeasuresBLL();
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
        [HttpGet]
        public ActionResult Details()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Show()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="dangerId">区域Code</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险等级</param>
        /// <param name="accType">事故类别编码</param>
        /// <param name="deptCode">涉及部门Code</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord)
        {
            var data = riskbll.GetList(areaCode, areaId, grade, accType, deptCode, keyWord);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "id";
            pagination.p_fields = @"areaid,risktype,dangersource,riskdesc,itemr,grade,accidentname,result,worktask,process,equipmentname,parts,levelname,1 as faultordanger,
                                    createuserid,createuserdeptcode,createuserorgcode,deptname,postname,faulttype,
                                    areaname,deptcode,createdate,MajorName,Description,HarmType,HarmProperty";
            pagination.p_tablename = "BIS_RISKDATABASE";
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",id";
            string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            pagination.conditionJson = string.IsNullOrEmpty(where) ? "1=1" : where;
            var watch = CommonHelper.TimerStart();
            var data = riskbll.GetPageList(pagination, queryJson);
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
            var data = riskbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        ///  <param name="workId">作业步骤ID</param>
        /// <param name="dangerId">危险点ID</param>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRiskJson(string workId, string dangerId, string areaId)
        {
            var data = riskbll.GetEntity(workId, dangerId, areaId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetAreas(string OrgId = "")
        {
            DistrictBLL districtBLL = new DistrictBLL();
            var data = districtBLL.GetList(OrgId);
            StringBuilder sb = new StringBuilder();
            foreach (DistrictEntity dist in data)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", dist.DistrictID, dist.DistrictName);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 根据区域获取风险点
        /// </summary>
        /// <param name="parentId">区域ID</param>
        /// <returns></returns>
        [HttpPost]
        public string GetRisks(string parentId)
        {
            DangerSourceBLL dangerBLL = new DangerSourceBLL();
            List<DangerSourceEntity> listDS = dangerBLL.GetList(parentId, "").ToList();
            StringBuilder sb = new StringBuilder();
            foreach (DangerSourceEntity dist in listDS)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", dist.Id, dist.Name);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 首页风险工作指标统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetHomeStat(string orgCode = "", string orgId = "")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("集团用户"))
            {
                if (!string.IsNullOrEmpty(orgCode))
                {
                    user = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = orgCode,
                        RoleName = "公司级用户,公司领导"
                    };
                }
                else
                {
                    user = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = "00",
                        RoleName = "公司级用户,公司领导"
                    };
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(riskbll.GetHomeStat(user));
        }
        /// <summary>
        /// 首页风险排名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetRiskRank()
        {
            return riskbll.GetRiskRank(ERCHTMS.Code.OperatorProvider.Provider.Current());
        }
        /// <summary>
        /// 风险预警
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetRiskWarn(string time = "")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            object data = riskbll.GetRiskWarn(ERCHTMS.Code.OperatorProvider.Provider.Current(), time);
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
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
        [HandlerMonitor(6, "删除风险库信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            riskbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        ///  <param name="keyValue">主键值</param>
        /// <param name="measuresJson">管控措施</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "新增或修改风险库信息")]
        public ActionResult SaveForm(string keyValue, string measuresJson, RiskEntity entity)
        {
            if (entity.RiskType == "设备")
            {
                entity.DangerSource = entity.FaultType;
            }
            int count = riskbll.SaveForm(keyValue, entity);
            //保存关联的管控措施
            if (count > 0 && measuresJson.Length > 0)
            {
                if (measuresBLL.Remove(entity.Id) >= 0)
                {
                    List<MeasuresEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MeasuresEntity>>(measuresJson);
                    HtStandardBLL htstandardBLL = new HtStandardBLL();
                    foreach (MeasuresEntity measure in list)
                    {
                        if (measuresBLL.Save("", measure))
                        {
                            //HtStandardEntity htStandard = new HtStandardEntity();
                            //htStandard.DeptCode = entity.DeptCode;
                            //htStandard.Description = "【"+entity.AreaName+"】"+entity.Description;
                            //htStandard.Measures = measure.Content;
                            //htStandard.AreaId = entity.AreaId;
                            //htStandard.AreaName = entity.AreaName;
                            //htStandard.AreaCode = entity.AreaCode;
                            //htstandardBLL.SaveForm("", htStandard);
                        }
                    }
                }
            }
            return Success("操作成功。");
        }
        #endregion

        #region 导出到excel
        /// <summary>
        /// 风险清单导出(重大风险)
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson, string fileName)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            //string path = "~/Resource/Temp";
            string fName = "安全风险清单" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            var IsGdxy = new DataItemDetailBLL().GetDataItemListByItemCode("'VManager'").ToList();
            if (IsGdxy.Count>0)
            {
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/风险措施库导出模板.xls"));
            }
            else {
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/风险措施库通用导出模板.xls"));
            }
           
            var queryParam = queryJson.ToJObject();
            var riskType = queryParam["riskType"].ToString();
            var IndexState = queryParam["IndexState"].ToString();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            if (string.IsNullOrWhiteSpace(riskType))
            {

                DataSet ds = new DataSet();
                List<string> typeList = new List<string>() { "作业", "设备", "区域", "管理","岗位","工器具及危化品" };
                for (int i = 0; i < typeList.Count; i++)
                {

                    DataTable exportTable = new DataTable();
                    exportTable = riskbll.GetPageExportList(queryJson, typeList[i], authType, IndexState);
                    exportTable.TableName = i.ToString();
                    ds.Tables.Add(exportTable);
                    for (int j = 0; j < wb.Worksheets.Count; j++)
                    {
                        if (wb.Worksheets[j].Name == typeList[i])
                        {
                            wb.Worksheets[j].Cells.ImportDataTable(exportTable, false, 2, 0);
                        }
                    }
                }
                //AsposeExcelHelper.ExecuteResultX(ds, path, typeList, fName);
            }
            else
            {
                DataTable exportTable = riskbll.GetPageExportList(queryJson, riskType, authType, IndexState);
                for (int j = 0; j < wb.Worksheets.Count; j++)
                {
                    if (wb.Worksheets[j].Name == riskType)
                    {
                        wb.Worksheets[j].Cells.ImportDataTable(exportTable, false, 2, 0);
                    }
                }


            }
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            System.Threading.Thread.Sleep(400);
            wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
            //wb.Save(Server.UrlEncode(fName), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
            return Success("导出成功。", fName);
        }
        #endregion
    }
}