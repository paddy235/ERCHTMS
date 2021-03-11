using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.RoutineSafetyWork;
using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class StartProjectCheckController : MvcControllerBase
    {
        private SecurityRedListBLL securityredlistbll = new SecurityRedListBLL();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        public ActionResult GetStartProjectPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,Title,Publisher,PublisherDept,ReleaseTime,IsSend,State";
            pagination.p_tablename = "BIS_SecurityRedList t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            pagination.conditionJson += string.Format(" and (IsSend='0' or createuserid='{0}')", user.UserId);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult StartProjectFrom()
        {
            return View();
        }
    }
}
