using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Data;
using BSFramework.Util.Offices;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// 描 述：辨识评估计划表
    /// </summary>
    public class RiskPlanController : MvcControllerBase
    {
        private RiskPlanBLL riskplanbll = new RiskPlanBLL();

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
        public ActionResult List()
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
        /// 获取用户辨识评估的区域
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpPost]
        public string GetCurrUserArea(string planId, string userAccount = "")
        {
            if (string.IsNullOrEmpty(userAccount))
            {
                userAccount = OperatorProvider.Provider.Current().Account;
            }
            string data = riskplanbll.GetCurrUserAreaId(planId, userAccount);
            return data;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = riskplanbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson, string mode = "")
        {
            pagination.p_kid = "ID";
            pagination.p_fields = "PLANNAME,startdate,enddate,0 num1,0 num2,0 num3,0 num4,0 num5,0 num6,areaid,'' bsusers,'' pgusers,deptcode,status,createuserid,deptname,createuserorgcode";
            pagination.p_tablename = "BIS_RISKPLAN t";
            pagination.conditionJson = "1=1";
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",id";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(mode))
            {
                //当前用户待辨识或评估的计划
                if (mode == "0")
                {
                    pagination.conditionJson += string.Format(" and status=0 and (',' || userids || ',' like '%,{0},%' or createuserid='{1}')", user.Account, user.UserId);
                }
                //当前用户所在部门待辨识或评估的计划
                else
                {
                    pagination.conditionJson += "  and status=0";
                }
            }
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            if (!string.IsNullOrEmpty(authType))
            {
                switch (authType)
                {
                    case "1":
                        pagination.conditionJson += " and (createuserid='" + user.UserId + "'";
                        break;
                    case "2":
                        pagination.conditionJson += string.Format(" and (deptcode='{0}' or id in(select planid from BIS_RISKPPLANDATA where deptcode='{0}')", user.DeptCode);
                        break;
                    case "3":
                        pagination.conditionJson += string.Format(" and (deptcode like '{0}%' or id in(select planid from BIS_RISKPPLANDATA where deptcode='{0}')", user.DeptCode);
                        break;
                    case "4":
                        pagination.conditionJson += " and (deptcode like '" + user.OrganizeCode + "%'";
                        break;
                    case "5":
                        pagination.conditionJson += " and (1=1 ";
                        break;
                }
                pagination.conditionJson += " or (',' || userids || ',') like '%," + user.Account + ",%'";

                pagination.conditionJson += string.Format(" or id in(select planid from BIS_RISKPPLANDATA where deptcode='{0}')) ", user.DeptCode);
            }
            else
            {
                pagination.conditionJson = " (',' || userids || ',') like '%," + user.Account + ",%' or createuserid='" + user.UserId + "'";
            }

            var watch = CommonHelper.TimerStart();
            DataTable data = riskplanbll.GetPageList(pagination, queryJson);
            foreach (DataRow dr in data.Rows)
            {
                List<int> list = riskplanbll.GetNumbers(dr[0].ToString(), DateTime.Parse(dr[2].ToString()).ToString("yyyy-MM-dd"), DateTime.Parse(dr[3].ToString()).ToString("yyyy-MM-dd"), int.Parse(dr["status"].ToString()), dr["areaid"].ToString());
                dr["num1"] = list[0]; dr["num2"] = list[1];
                dr["num3"] = list[2]; dr["num4"] = list[3]; dr["num5"] = list[4];
                dr["num6"] = list[5];
                string[] users = riskplanbll.GetUsers(dr[0].ToString()).Split('|');
                dr["bsusers"] = users[0]; dr["pgusers"] = users[1];
            }
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
        public ActionResult GetDataListJson(int dataType, string planId)
        {
            RiskPlanDataBLL riskplandatabll = new RiskPlanDataBLL();
            var data = riskplandatabll.GetList(dataType, planId);
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
            var data = riskplanbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetPlanDataFormJson(string keyValue)
        {
            RiskPlanDataBLL riskplandatabll = new RiskPlanDataBLL();
            var data = riskplandatabll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "删除辨识评估计划信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            riskplanbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 删除计划实施组织机构及人员信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除辨识计划关联的辨识或评估信息")]
        public ActionResult Remove(string keyValue)
        {
            RiskPlanDataBLL riskplandatabll = new RiskPlanDataBLL();
            riskplandatabll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="dataJson">从表josn数组字符串</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "新增或修改辨识计划信息")]
        public ActionResult SaveForm(string keyValue, RiskPlanEntity entity, [System.Web.Http.FromBody]string dataJson)
        {
            StringBuilder sb = new StringBuilder();
            List<string> listIds = new List<string>();
            string[] arr = entity.AreaId.Split(',');
            foreach (string str in arr)
            {
                if (!listIds.Contains(str))
                {
                    listIds.Add(str);
                }
            }
            foreach (string str in listIds)
            {
                sb.Append(str + ",");
            }
            entity.AreaId = sb.ToString().TrimEnd(',');
            
            if (riskplanbll.SaveForm(keyValue, entity) > 0)
            {
                //保存关联的从表记录
                if (dataJson.Length > 0)
                {
                    RiskPlanDataBLL riskplandatabll = new RiskPlanDataBLL();
                    if (riskplandatabll.Remove(entity.Id) > 0)
                    {
                        List<RiskPlanDataEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RiskPlanDataEntity>>(dataJson);
                        foreach (RiskPlanDataEntity data in list)
                        {
                            riskplandatabll.SaveForm("", data);
                        }
                    }
                }
                entity.RiskNumbers = riskplanbll.GetRiskNumbers(entity.AreaId, entity.StartDate.ToString(),entity.Id);
                if (riskplanbll.SaveForm(entity.Id, entity) <= 0)
                {
                    return Error("操作失败");
                }

            }
            return Success("操作成功。");
        }
        /// <summary>
        /// 设置计划完成
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "设置完成辨识计划状态")]
        public ActionResult SetComplate(string keyValue, string areaId)
        {
            bool result = riskplanbll.SetComplate(keyValue, areaId);
            return result ? Success("操作成功。") : Error("对不起，操作失败");
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
        [HandlerMonitor(5, "新增或修改辨识计划关联的辨识或评估信息")]
        public ActionResult SavePlanData(string keyValue, RiskPlanDataEntity entity)
        {
            RiskPlanDataBLL riskplandatabll = new RiskPlanDataBLL();
            riskplandatabll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult Export(string queryJson, string mode = "")
        {
            Pagination pagination = new Pagination();
            pagination.p_kid = "ID";
            pagination.p_fields = "PLANNAME,startdate,enddate,deptname,0 num1,0 num2,0 num3,0 num4,0 num5,case WHEN  status=0 then '未完成' else '已完成' end  as statusstr,status,areaid,'' bsusers,'' pgusers";
            pagination.p_tablename = " (select * from BIS_RISKPLAN order by CreateDate desc) t";
            pagination.conditionJson = "1=1";
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(mode))
            {
                //当前用户待辨识或评估的计划
                if (mode == "0")
                {
                    pagination.conditionJson += string.Format(" and status=0 and (',' || userids || ',' like '%,{0},%' or createuserid='{1}')", user.Account, user.UserId);
                }
                //当前用户所在部门待辨识或评估的计划
                else
                {
                    pagination.conditionJson += "  and status=0";
                }
            }
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            if (!string.IsNullOrEmpty(authType))
            {
                switch (authType)
                {
                    case "1":
                        pagination.conditionJson += " and (createuserid='" + user.UserId + "'";
                        break;
                    case "2":
                        pagination.conditionJson += string.Format(" and (deptcode='{0}' or id in(select planid from BIS_RISKPPLANDATA where deptcode='{0}')", user.DeptCode);
                        break;
                    case "3":
                        pagination.conditionJson += string.Format(" and (deptcode like '{0}%' or id in(select planid from BIS_RISKPPLANDATA where deptcode='{0}')", user.DeptCode);
                        break;
                    case "4":
                        pagination.conditionJson += " and (deptcode like '" + user.OrganizeCode + "%'";
                        break;
                    case "5":
                        pagination.conditionJson += " and (1=1 ";
                        break;
                }
                pagination.conditionJson += " or (',' || userids || ',') like '%," + user.Account + ",%'";

                pagination.conditionJson += string.Format(" or id in(select planid from BIS_RISKPPLANDATA where deptcode='{0}')) ", user.DeptCode);
            }
            else
            {
                pagination.conditionJson = " (',' || userids || ',') like '%," + user.Account + ",%' or createuserid='" + user.UserId + "'";
            }

            var watch = CommonHelper.TimerStart();
            DataTable data = riskplanbll.GetPageList(pagination, queryJson);
            foreach (DataRow dr in data.Rows)
            {
                List<int> list = riskplanbll.GetNumbers(dr[0].ToString(), DateTime.Parse(dr[2].ToString()).ToString("yyyy-MM-dd"), DateTime.Parse(dr[3].ToString()).ToString("yyyy-MM-dd"), int.Parse(dr["status"].ToString()), dr["areaid"].ToString());
                dr["num1"] = list[0]; dr["num2"] = list[1];
                dr["num3"] = list[2]; dr["num4"] = list[3]; dr["num5"] = list[4];
                string[] users = riskplanbll.GetUsers(dr[0].ToString()).Split('|');
                dr["bsusers"] = users[0]; dr["pgusers"] = users[1];
            }
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "风险辨识评估";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "风险辨识评估.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "PLANNAME".ToLower(), ExcelColumn = "计划名称", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "startdate".ToLower(), ExcelColumn = "计划开始时间", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "enddate".ToLower(), ExcelColumn = "计划结束时间", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "制定部门", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "num1".ToLower(), ExcelColumn = "原有风险数量", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "num2".ToLower(), ExcelColumn = "新增风险数量", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "num3".ToLower(), ExcelColumn = "完善风险数量", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "num4".ToLower(), ExcelColumn = "消除风险数量", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "num5".ToLower(), ExcelColumn = "现有风险数量", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "statusstr".ToLower(), ExcelColumn = "状态", Alignment = "Center" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");

        }
        #endregion
    }
}
