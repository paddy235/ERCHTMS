using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Dynamic;
using BSFramework.Data;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// 描 述：安全红黑榜
    /// </summary>
    public class SecurityRedListController : MvcControllerBase
    {
        private SecurityRedListBLL securityredlistbll = new SecurityRedListBLL();

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
        /// <summary>
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Stat()
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
            pagination.p_fields = "createuserid,Title,Publisher,PublisherDept,ReleaseTime,IsSend,State";
            pagination.p_tablename = "BIS_SecurityRedList t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            
              var queryParam = queryJson.ToJObject();
              //从首页安全过来的
              if (!queryParam["action"].IsEmpty())
              {
                  pagination.conditionJson += string.Format(" issend='0' and publisherdeptcode  like '{0}%'", user.OrganizeCode);
              }
              else 
              {
                  pagination.conditionJson = string.Format(" createuserorgcode = '{0}'", user.OrganizeCode);
                  pagination.conditionJson += string.Format(" and (issend='0' or createuserid='{0}')", user.UserId);
              }

            var watch = CommonHelper.TimerStart();
            var data = securityredlistbll.GetPageList(pagination, queryJson);
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
            var data = securityredlistbll.GetList(queryJson);
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
            var data = securityredlistbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取安全红黑榜统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetSecurityRedListStat(string queryJson)
        {
            object obj = securityredlistbll.GetSecurityRedListStat(queryJson);
            return ToJsonResult(obj);
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
            securityredlistbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SecurityRedListEntity entity)
        {
            securityredlistbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
