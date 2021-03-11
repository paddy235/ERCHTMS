using ERCHTMS.Entity.DangerousJobConfig;
using ERCHTMS.Busines.DangerousJobConfig;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using System.Linq;

namespace ERCHTMS.Web.Areas.DangerousJobConfig.Controllers
{
    /// <summary>
    /// 描 述：危险作业分级标准配置
    /// </summary>
    public class ClassStandardConfigController : MvcControllerBase
    {
        private ClassStandardConfigBLL classstandardconfigbll = new ClassStandardConfigBLL();

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
            try
            {
                var watch = CommonHelper.TimerStart();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "a.id";
                pagination.p_fields = @"a.createuserid,a.createuserdeptcode,a.createuserorgcode
                ,a.createdate,worktype,d.itemname as worktypename,deptid,deptcode,deptname,a.createusername";
                pagination.p_tablename = "dj_classstandardconfig a left join base_dataitemdetail d on a.worktype=d.itemvalue and d.itemid =(select itemid from base_dataitem where itemcode='DangerousJobConfig')";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = string.Format("a.createuserid='System'  or a.createuserorgcode='{0}' or a.createuserid='{1}'", user.OrganizeCode, user.UserId);
                }
                var data = classstandardconfigbll.GetPageList(pagination, queryJson);
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
            catch (Exception ex)
            {

                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = classstandardconfigbll.GetList(queryJson);
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
            var data = classstandardconfigbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJsonByWorkType(string WorkType)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var data = classstandardconfigbll.GetList("").Where(t => t.WorkType == WorkType && t.DeptCode == user.OrganizeCode).FirstOrDefault();
                return Success("获取成功", data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
        /// <summary>
        /// 是否存在相同类型数据
        /// </summary>
        /// <param RiskType="风险类型"></param>
        /// <param WayType="取值类型"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsExistDataByType(string WorkType)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = classstandardconfigbll.GetList("").Where(x => x.WorkType == WorkType && x.DeptCode == user.OrganizeCode).ToList();
                if (data.Count > 0)
                {
                    return ToJsonResult(false);
                }
                else
                {
                    return ToJsonResult(true);
                }
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
            classstandardconfigbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ClassStandardConfigEntity entity)
        {
            classstandardconfigbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
