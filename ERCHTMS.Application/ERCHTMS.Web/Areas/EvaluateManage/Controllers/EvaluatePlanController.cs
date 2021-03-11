using ERCHTMS.Entity.EvaluateManage;
using ERCHTMS.Busines.EvaluateManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Code;
using System.Data;

namespace ERCHTMS.Web.Areas.EvaluateManage.Controllers
{
    /// <summary>
    /// 描 述：合规性评价计划
    /// </summary>
    public class EvaluatePlanController : MvcControllerBase
    {
        private EvaluatePlanBLL evaluateplanbll = new EvaluatePlanBLL();

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
            queryJson = queryJson ?? "";
            pagination.p_kid = "Id";
            pagination.p_fields = "WorkTitle,Dept,AbortDate,IsSubmit,createuserid,createuserdeptcode,createuserorgcode,donedeptnum,deptnum,checkstate";
            pagination.p_tablename = "HRS_EVALUATEPLAN";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = evaluateplanbll.GetPageList(pagination, queryJson);
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
            var data = evaluateplanbll.GetList(queryJson);
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
            var data = evaluateplanbll.GetEntity(keyValue);
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
            evaluateplanbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="IsSubmit">是否提交</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, int IsSubmit, int type, EvaluatePlanEntity entity)
        {
            if (type == 1)
            {
                entity.IsSubmit = IsSubmit;
            }
            if (type == 2)
            {
                entity.CheckState = IsSubmit;//0数据已提交 1评价报告保存 2评价报告提交 3审核保存 4审核提交
            }
            evaluateplanbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        /// <summary>
        /// //一键提醒
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult Remind(string keyValue)
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                
                MessageEntity messageEntity = new MessageEntity();
                var data = evaluateplanbll.GetEntity(keyValue);
                messageEntity.Title = data.WorkTitle;
                messageEntity.Content = "请进行" + data.WorkTitle + "，评价截止时间为：" + data.AbortDate.Value.ToString("yyyy-MM-dd") + "。";
                messageEntity.SendUser = curUser.Account;
                messageEntity.SendUserName = curUser.UserName;
                messageEntity.SendTime = DateTime.Now;
                messageEntity.Category = "其它";
                DataTable dt = evaluateplanbll.GetRemindUser(keyValue);

                string userid = "";
                string username = "";
                //插入数据
                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        userid += item["userid"].ToString() + ",";
                        username += item["username"].ToString() + ",";
                    }
                }
                messageEntity.UserId = userid;
                messageEntity.UserName = username;

                new MessageBLL().SaveForm("", messageEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Success("操作失败。");
            }
        }
        
    }
}
