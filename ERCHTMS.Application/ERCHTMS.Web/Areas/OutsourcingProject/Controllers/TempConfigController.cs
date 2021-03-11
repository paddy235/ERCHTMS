using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：电厂模板管理
    /// </summary>
    public class TempConfigController : MvcControllerBase
    {
        private TempConfigBLL tempconfigbll = new TempConfigBLL();

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
            var data = tempconfigbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTempConfigPageJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = @" createuserid,createuserdeptcode,createuserorgcode,createdate,createusername,modulename,modulecode,tempname,proessmode,deptname,deptcode,deptid";
            pagination.p_tablename = @" epg_tempmanage t";
            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" 1=1 ");
            if (!currUser.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "deptcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var data = tempconfigbll.GetTempConfigPageJson(pagination, queryJson);
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
            var data = tempconfigbll.GetEntity(keyValue);
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
            tempconfigbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TempConfigEntity entity, string Mode)
        {
            var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (Mode == "add")
            {
                var e = tempconfigbll.GetList("").Where(x => x.ModuleCode == entity.ModuleCode && x.CreateUserOrgCode == currUser.OrganizeCode).ToList().Count;
                if (e > 0)
                {
                    return Error("已添加该模块的模板,请勿重复添加!");
                }
            }
            tempconfigbll.SaveForm(keyValue, entity);
            return Success("操作成功。");


        }
        #endregion
    }
}
