using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：外包单位黑名单表
    /// </summary>
    public class OutprojectblacklistController : MvcControllerBase
    {
        private OutprojectblacklistBLL outprojectblacklistbll = new OutprojectblacklistBLL();

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
            var data = outprojectblacklistbll.GetList(queryJson);
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
            var data = outprojectblacklistbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetPageBlackListJson(Pagination pagination, string queryJson) {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "bk.id";
                pagination.p_fields = @" bk.createdate,bk.outprojectid,
                                           t.outsourcingname,
                                           t.legalrep,
                                           bk.inblacklisttime,
                                            bk.inblacklistcause,
                                           bk.outblacklisttime,
                                           bk.outblacklistcause";
                pagination.p_tablename = @" epg_outprojectblacklist bk
                                            left join epg_outsourcingproject t on t.outprojectid = bk.outprojectid 
                                            left join base_department b on b.departmentid=bk.outprojectid ";
                pagination.sidx = "b.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1 ";
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                {
                    pagination.conditionJson = string.Format("  1=1 ");
                }
                else if (currUser.RoleName.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format("  t.OUTPROJECTID ='{0}'", currUser.DeptId);
                }
                else
                {
                    pagination.conditionJson = string.Format(" t.outprojectid in(select distinct(t.outprojectid) from EPG_OUTSOURINGENGINEER t where t.engineerletdeptid='{0}')", currUser.DeptId);
                }
                var data = outprojectblacklistbll.GetPageBlackListJson(pagination, queryJson);

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
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult ToIndexData()
        {
            Operator user = OperatorProvider.Provider.Current();
            var list = outprojectblacklistbll.ToIndexData(user);
            return ToJsonResult(list);
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
            outprojectblacklistbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OutprojectblacklistEntity entity)
        {
            outprojectblacklistbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
