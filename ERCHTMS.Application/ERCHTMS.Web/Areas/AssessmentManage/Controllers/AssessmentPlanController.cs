using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.Busines.AssessmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.AssessmentManage.Controllers
{
    /// <summary>
    /// 描 述：自评计划
    /// </summary>
    public class AssessmentPlanController : MvcControllerBase
    {
        private AssessmentPlanBLL assessmentplanbll = new AssessmentPlanBLL();

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
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "Id";
            pagination.p_fields = "CreateDate,PlanName,TeamLeaderName,Status,IsLock,createuserid,createuserdeptcode,createuserorgcode,TeamLeader";
            pagination.p_tablename = " bis_assessmentplan";
            pagination.conditionJson = "1=1 and createuserorgcode='" + user.OrganizeCode + "'";
            var data = assessmentplanbll.GetPageList(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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
            var data = assessmentplanbll.GetList(queryJson);
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
            var data = assessmentplanbll.GetEntity(keyValue);
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
            assessmentplanbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, AssessmentPlanEntity entity, [System.Web.Http.FromBody]string dataJson)
        {
            if (string.IsNullOrEmpty(keyValue))//新增
            {
                entity.Status = "进行中";
                entity.IsLock = "锁定";
            }
            assessmentplanbll.SaveForm(keyValue, entity);
            //保存关联的从表记录
            if (dataJson.Length > 0)
            {
                AssessmentSumBLL safeproductPbll = new AssessmentSumBLL();
                List<AssessmentSumEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AssessmentSumEntity>>(dataJson);
                foreach (AssessmentSumEntity data in list)
                {
                    if (string.IsNullOrEmpty(keyValue))
                    {
                        data.Reserve = "未筛选";//筛选状态
                        data.GradeStatus = "未评分";//评分状态
                        safeproductPbll.SaveForm("", data);
                    }
                    else
                    {
                        var sumentity = safeproductPbll.GetEntity(data.ChapterID);//修改时，sumid为自评总结主键id
                        sumentity.DutyName = data.DutyName;
                        sumentity.DutyID = data.DutyID;
                        safeproductPbll.SaveForm(sumentity.Id, sumentity);
                    }

                }
            }
            return Success("操作成功。");
        }


        /// <summary>
        /// 更改自评计划整体状态
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SavePlanInfo(string planid)
        {
            var message = "";
            AssessmentPlanEntity resultEntity = assessmentplanbll.GetEntity(planid);
            if (resultEntity != null)
            {
                if (resultEntity.IsLock == "锁定")
                {
                    resultEntity.IsLock = "解锁";
                    message = "已锁定该计划。";
                }
                else
                {
                    resultEntity.IsLock = "锁定";
                    message = "已解锁该计划。";
                }
                assessmentplanbll.SaveForm(resultEntity.Id, resultEntity);
            }
            return Success(message);
        }
        #endregion
    }
}
