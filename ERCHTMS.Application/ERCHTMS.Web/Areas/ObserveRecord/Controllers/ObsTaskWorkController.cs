using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.Observerecord;
using ERCHTMS.Code;
using ERCHTMS.Entity.Observerecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.ObserveRecord.Controllers
{
    public class ObsTaskWorkController : MvcControllerBase
    {
        private ObsTaskworkBLL obsplanworkbll = new ObsTaskworkBLL();
        private ObsTaskBLL obsplanbll = new ObsTaskBLL();
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
        public ActionResult GetPlanWorkList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.id";
            pagination.p_fields = @" t.createuserid,t.createuserdeptcode,t.createuserorgcode,
                                       t.createdate,t.createusername,t.obsperson,
                                       t.obspersonid,t.risklevel,t.obsnum,t.obsnumtext,
                                       t.obsmonth,t.planid,t.workname,t.remark";
            pagination.p_tablename = @"bis_obsTaskwork t";
            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = "1=1";
            var data = obsplanworkbll.GetPageListJson(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = obsplanworkbll.GetEntity(keyValue);
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
            obsplanworkbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ObsTaskworkEntity entity)
        {
            obsplanworkbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 只修改计划月份直接同步到EHS与发布的数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult TbSaveForm(string keyValue, ObsTaskworkEntity entity)
        {
            obsplanworkbll.SaveForm(keyValue, entity);
            obsplanbll.SynchData(entity.PlanId, keyValue);
            return Success("操作成功。");
        }
        #endregion
    }
}