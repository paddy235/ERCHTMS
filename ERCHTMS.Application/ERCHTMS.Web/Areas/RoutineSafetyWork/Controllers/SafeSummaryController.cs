using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.TrainPlan;
using ERCHTMS.Code;
using ERCHTMS.Entity.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    public class SafeSummaryController : MvcControllerBase
    {

        private SafeSummaryBLL safeSummaryBLL = new SafeSummaryBLL();
        private SafeMeasureBLL SafeMeasureBLL = new SafeMeasureBLL();

        #region [视图功能]
        // GET: RoutineSafetyWork/SafeSummary
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region [获取数据]
        /// <summary>
        /// 安措计划总结报告
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";
                //根据后台配置查看数据权限
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
                DataTable dt = safeSummaryBLL.GetList(pagination, queryJson);

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
        /// 表单数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var entity = safeSummaryBLL.GetFormJson(keyValue);
            return ToJsonResult(entity);
        }

        #endregion

        #region [提交数据]

        /// <summary>
        /// 保存/提交
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="postState">0:保存 1:提交</param>
        /// <param name="entity"></param>
        /// <param name="dataJson"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveForm(string keyValue, string postState, SafeSummaryEntity entity, [System.Web.Http.FromBody]string dataJson)
        {
            if (!safeSummaryBLL.CheckExists(keyValue, entity))
            {
                //不存在可以提交
                if (dataJson.Length > 0)
                {
                    //总结报告名称
                    entity.ReportName = entity.BelongYear + "年第" + entity.Quarter + "季度安全技术措施计划总结";
                    List<SafeMeasureEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafeMeasureEntity>>(dataJson);
                    safeSummaryBLL.SaveForm(keyValue, postState, entity, list);
                }
                return Success("操作成功。");
            }
            else
            {
                string msg = "【" + entity.DepartmentName + "】" + entity.BelongYear + "年第" + entity.Quarter + "季度报告已存在!";
                return Error(msg);
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteData(string keyValue)
        {
            try
            {
                safeSummaryBLL.DeleteForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message); ;
            }
        }
        #endregion
    }
}