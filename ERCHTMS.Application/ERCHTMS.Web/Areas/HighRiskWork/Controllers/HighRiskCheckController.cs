using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：高危险作业审核/审批表
    /// </summary>
    public class HighRiskCheckController : MvcControllerBase
    {
        private HighRiskCheckBLL highriskcheckbll = new HighRiskCheckBLL();
        private HighRiskApplyBLL highriskapplybll = new HighRiskApplyBLL();

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
            var data = highriskcheckbll.GetList(queryJson);
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
            var data = highriskcheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 根据申请id获取审核记录
        /// </summary>
        /// <param name="applyid">申请表id</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetCheckListInfo(string applyid)
        {
            var data = highriskcheckbll.GetCheckListInfo(applyid);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 根据申请id获取审批记录
        /// </summary>
        /// <param name="applyid">申请表id</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetApproveInfo(string applyid)
        {
            var data = highriskcheckbll.GetApproveInfo(applyid);
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
            highriskcheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string approveid, [System.Web.Http.FromBody]string dataJson)
        {
            var nochecknum=highriskcheckbll.GetNoCheckNum(approveid);
            var applyentity = highriskapplybll.GetEntity(approveid);
            if (applyentity.ApplyState == "4")//审批
            {
                if (dataJson.Length > 0)
                {
                    List<HighRiskCheckEntity> vchecklist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HighRiskCheckEntity>>(dataJson);
                    var vcheckentity = highriskcheckbll.GetNeedCheck(approveid);
                    foreach (HighRiskCheckEntity item in vchecklist)
                    {
                        vcheckentity.ApproveReason = item.ApproveReason;
                        vcheckentity.ApproveState = item.ApproveState;
                        if (item.ApproveState == "2")
                            applyentity.ApplyState = "5";//审批未通过
                        if (item.ApproveState == "1")
                            applyentity.ApplyState = "6";//审批完成
                        highriskcheckbll.SaveForm(vcheckentity.Id, vcheckentity);
                    }
                    highriskapplybll.SaveForm(approveid, applyentity);
                }
            }
            if (applyentity.ApplyState == "2")//审核
            {
                if (nochecknum > 0)
                {
                    if (dataJson.Length > 0)
                    {
                        List<HighRiskCheckEntity> vchecklist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HighRiskCheckEntity>>(dataJson);
                        var vcheckentity = highriskcheckbll.GetNeedCheck(approveid);
                        foreach (HighRiskCheckEntity item in vchecklist)
                        {
                            vcheckentity.ApproveReason = item.ApproveReason;
                            vcheckentity.ApproveState = item.ApproveState;
                            if (item.ApproveState == "2")
                            {
                                applyentity.ApplyState = "3";//审核未通过
                            }
                            if (nochecknum == 1 && item.ApproveState == "1")
                            {
                                applyentity.ApplyState = "4";//审批中
                            }
                            highriskcheckbll.SaveForm(vcheckentity.Id, vcheckentity);
                        }
                        highriskapplybll.SaveForm(approveid, applyentity);
                    }
                }
            }
            return Success("操作成功。");
        }
        #endregion
    }
}
