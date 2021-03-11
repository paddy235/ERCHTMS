using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：天气预警
    /// </summary>
    public class WeatherController : MvcControllerBase
    {
        private WeatherBLL weatherbll = new WeatherBLL();

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
            var data = weatherbll.GetList(queryJson);
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
            var data = weatherbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ID";
            pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,a.WEATHER,a.REQUIRE,a.INITIATEMODE";
            pagination.p_tablename = @"BIS_WEATHER a";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }


            var watch = CommonHelper.TimerStart();
            var data = weatherbll.GetPageList(pagination, queryJson);
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
        /// 天气预警
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "天气预警")]
        public ActionResult exportExcelData(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_fields = @"a.WEATHER,a.REQUIRE, case  when a.INITIATEMODE ='1' then '是' else '否' end INITIATEMODE";
            pagination.p_tablename = @"BIS_WEATHER a";
            pagination.sord = "CreateDate";
            #region 权限校验
            pagination.conditionJson = "1=1";
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
            #endregion
            var data = weatherbll.GetPageList(pagination, queryJson);

            ////设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "天气预警";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "天气预警" + ".xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            excelconfig.ColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "weather".ToLower(), ExcelColumn = "天气" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "require".ToLower(), ExcelColumn = "作业要求" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "initiatemode".ToLower(), ExcelColumn = "是否启用" });

            //调用导出方法
            //ExcelHelper.ExcelDownload(data, excelconfig);
            //return Success("导出成功。");
            //设置导出格式

            //调用导出方法
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, excelconfig.ColumnEntity);
            return Success("导出成功。");
        }


        /// <summary>
        /// 根据天气获取预警信息
        /// </summary>
        /// <param name="weather">天气</param>
        /// <returns>预警信息Json</returns>
        [HttpGet]
        public string GetRequire(string weather)
        {
            return weatherbll.GetRequire(weather);
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
            weatherbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, WeatherEntity entity)
        {
            weatherbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
