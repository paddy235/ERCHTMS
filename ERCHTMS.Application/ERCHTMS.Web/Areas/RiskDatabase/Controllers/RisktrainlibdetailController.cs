using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Entity.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// 描 述：风险预知训练库管控措施
    /// </summary>
    public class RisktrainlibdetailController : MvcControllerBase
    {
        private RisktrainlibdetailBLL risktrainlibdetailbll = new RisktrainlibdetailBLL();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string workId)
        {
            var data = risktrainlibdetailbll.GetList(workId);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetTrainLibDetail(string workId,string worktask)
        {
            //DataTable dt = new DataTable();
            DataTable result = new DataTable();
            if (!string.IsNullOrEmpty(workId))
            {
                result = risktrainlibdetailbll.GetTrainLibDetail(workId);
                var data = (from t in result.AsEnumerable()
                            select new
                            {
                                Id = t.Field<string>("id"),
                                WorkTask = worktask,
                                WorkProcess=t.Field<string>("process"),
                                AtRisk=t.Field<string>("riskdesc"),
                                Controls=t.Field<string>("content")
                            }).ToList();
                return ToJsonResult(data);
            }
            return ToJsonResult(result);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = risktrainlibdetailbll.GetEntity(keyValue);
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
            risktrainlibdetailbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, RisktrainlibdetailEntity entity)
        {
            risktrainlibdetailbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}