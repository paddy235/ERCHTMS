using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using System.Data;
using ERCHTMS.Busines.ComprehensiveManage;
using ERCHTMS.Entity.ComprehensiveManage;

namespace ERCHTMS.Web.Areas.ComprehensiveManage.Controllers
{
    /// <summary>
    /// 荣誉分享
    /// </summary>
    public class HonoursController : MvcControllerBase
    {
        private HonoursBLL Honoursbll = new HonoursBLL();

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
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,Title,Publisher,ReleaseTime,IsSend";
            pagination.p_tablename = "HRS_HONOURS t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and createuserdeptcode='" + user.DeptCode + "'";
                            break;
                        case "3":
                            pagination.conditionJson += " and createuserdeptcode like'" + user.DeptCode + "%'";
                            break;
                        case "4":
                            pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            pagination.conditionJson += string.Format(" and (IsSend='0' or createuserid='{0}')", user.UserId);
            var watch = CommonHelper.TimerStart();
            var data = Honoursbll.GetPageList(pagination, queryJson);
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
            var data = Honoursbll.GetList(queryJson);
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
            var data = Honoursbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取荣誉展示(首页)
        /// </summary>
        /// <param name="num">需要显示的条数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTrends(string num)
        {
            try
            {
                DataTable dt = Honoursbll.GetTrends(num);
                foreach (DataRow item in dt.Rows)
                {

                }
                return Success("获取数据成功", dt);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
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
            Honoursbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HonoursEntity entity)
        {
            Honoursbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}