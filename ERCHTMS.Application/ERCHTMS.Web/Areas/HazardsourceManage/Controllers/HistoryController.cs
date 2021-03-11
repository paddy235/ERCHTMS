using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.Busines.HazardsourceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.HazardsourceManage.Controllers
{
    /// <summary>
    /// 描 述：历史记录
    /// </summary>
    public class HistoryController : MvcControllerBase
    {
        private HistoryBLL historybll = new HistoryBLL();
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();

        #region 视图功能

        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryRecord()
        {
            return View();
        }
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
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "DANGERSOURCENAME, CREATEDATE, CREATEUSERNAME";
            pagination.p_tablename = "HSD_HISTORY t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                ////登陆人可查看本机构的所有数据
                //pagination.conditionJson = " CreateUserOrgCode='" + user.OrganizeCode + "'";


                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
                else
                {
                    pagination.conditionJson += " and CreateUserId='" + user.UserId + "'";

                }

            }


            var watch = CommonHelper.TimerStart();
            var data = historybll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetHistoryRecordPageListJson(Pagination pagination, string queryJson)
        {
            var historyId = Request["historyId"] ?? "";
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "MeaSureNum,HAZARDSOURCEID,RISKASSESSID,historyid,districtname, DANGERSOURCE, ACCIDENTNAME,MEASURE,DEPTNAME,JDGLZRRFULLNAME,ISDANGER,case WHEN  ISDANGER>0 then '是' else '否' end as ISDANGERNAME";
            pagination.p_tablename = "hsd_hisrelationhd t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.DeptCode + "%')";

            }
            if (historyId.Length > 0)
                pagination.conditionJson += " and historyid='" + historyId + "'";
            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = historybll.GetList(queryJson);
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

            var data = historybll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        public ActionResult GetFormJsonForRecord(string keyValue)
        {
            Hisrelationhd_qdBLL hqbbll = new Hisrelationhd_qdBLL();
            var data = hqbbll.GetListForRecord(keyValue).FirstOrDefault();
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
            historybll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HistoryEntity entity)
        {
            historybll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
