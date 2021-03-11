using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// 描 述：安全会议参会人员表
    /// </summary>
    public class ConferenceUserController : MvcControllerBase
    {
        private ConferenceUserBLL conferenceuserbll = new ConferenceUserBLL();

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
            pagination.p_kid = "r.ID";
            pagination.p_fields = "e.conferencename,e.conferencetime,r.username,r.userid,r.ConferenceID,case when r.reviewstate=1 then '审批' when r.reviewstate=2 then '审批通过' when r.reviewstate=3 then '审批未通过' end as reviewstate";
            pagination.p_tablename = "BIS_ConferenceUser r left join BIS_Conference e on r.conferenceid=e.id";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                pagination.conditionJson = string.Format(" ReviewUserID='{0}' ", user.UserId);
            }

            var watch = CommonHelper.TimerStart();
            var data = conferenceuserbll.GetPageList(pagination, queryJson);
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
            var data = conferenceuserbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue,string UserId)
        {
            var data = conferenceuserbll.GetEntity(keyValue, UserId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取签到表数据
        /// </summary>
        /// <param name="keyValue">会议ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSignTable(string keyValue)
        {
            var data=conferenceuserbll.GetSignTable(keyValue);
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
            conferenceuserbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ConferenceUserEntity entity)
        {
            conferenceuserbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
