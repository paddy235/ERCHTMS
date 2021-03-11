using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.Busines.LaborProtectionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.LaborProtectionManage.Controllers
{
    /// <summary>
    /// 描 述：劳动防护发放记录表
    /// </summary>
    public class LaborissueController : MvcControllerBase
    {
        private LaborissueBLL laborissuebll = new LaborissueBLL();
        private LaborinfoBLL laborinfobll = new LaborinfoBLL();
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "LABOROPERATIONUSERNAME,LABOROPERATIONTIME";//注：此处要替换成需要查询的列
            pagination.p_tablename = "BIS_LABORISSUE";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CREATEDATE";

            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                if (laborinfobll.GetPer())
                {

                    pagination.conditionJson += " and CREATEUSERORGCODE='" + user.OrganizeCode + "'";
                }
                else
                {
                    string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                    pagination.conditionJson += " and " + where;
                }

            }

            var data = laborissuebll.GetPageListByProc(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = laborissuebll.GetEntity(keyValue);
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
            laborissuebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, LaborissueEntity entity)
        {
            laborissuebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
