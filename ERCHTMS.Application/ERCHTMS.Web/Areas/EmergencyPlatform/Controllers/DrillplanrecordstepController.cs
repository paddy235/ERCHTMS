using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERCHTMS.Busines.EmergencyPlatform;
using ERCHTMS.Entity.EmergencyPlatform;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    public class DrillplanrecordstepController : MvcControllerBase
    {
        private DrillplanrecordstepBLL drillplanrecordstepbll = new DrillplanrecordstepBLL();
        // GET: EmergencyPlatform/Drillplanrecordstep
        public ActionResult Index()
        {
            return View();
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
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "id as stepid,content,dutypersonname,dutyperson,sortid";
            pagination.p_tablename = " mae_drillplanrecordstep";
            pagination.conditionJson = "1=1";
            pagination.sidx = "sortid";
            pagination.sord = "asc";
            pagination.rows = 10000;
            pagination.page = 1;
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
            }
            var data = drillplanrecordstepbll.GetPageList(pagination, queryJson);
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
            drillplanrecordstepbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, DrillplanrecordstepEntity entity)
        {
            drillplanrecordstepbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }


        [HttpPost]
        public ActionResult SaveListForm()
        {
            string data = Request["param"];
            var list = data.ToObject<List<DrillplanrecordstepEntity>>();
            foreach (var item in list)
            {
                drillplanrecordstepbll.SaveForm(item.Id, item);
            }

            return Success("操作成功。");
        }
        #endregion
    }
}