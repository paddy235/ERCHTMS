using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.Busines.RiskDataBaseConfig;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using System.Linq;

namespace ERCHTMS.Web.Areas.RiskDataBaseConfig.Controllers
{
    /// <summary>
    /// 描 述：工作任务清单说明表
    /// </summary>
    public class WorkfileController : MvcControllerBase
    {
        private WorkfileBLL workfilebll = new WorkfileBLL();

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
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">查询语句</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination,string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "id";
                pagination.p_fields = @"createuserid,createuserdeptcode,createuserorgcode
,createdate,title,issend,sendtime,deptname,deptcode,deptid,createusername";
                pagination.p_tablename = "bis_workfile";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = string.Format("((createuserid='System' and issend='1') or (createuserorgcode='{0}' and issend='1') or (createuserid='{1}'))", user.OrganizeCode, user.UserId);
                }

                var data = workfilebll.GetPageList(pagination, queryJson);
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
        /// 根据机构Code查询本机构是否已经添加
        /// </summary>
        /// <param name="orgCode">机构Code</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetIsExist(string orgCode)
        {
            try
            {
                var data = workfilebll.GetIsExist(orgCode);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
            
        }
        /// <summary>
        /// 查询是否存在默认配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetWrokFileData() {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //查询本单位是否有默认配置,没有则查询系统默认配置,没有则返回null
                var data = workfilebll.GetList("").Where(x => x.Issend == 1 && x.DeptCode == user.OrganizeCode).ToList();
                if (data.Count > 0)
                {
                    return ToJsonResult(data);
                }
                else
                {
                    data = workfilebll.GetList("").Where(x => x.Issend == 1 && x.DeptCode == "0").ToList();
                    return ToJsonResult(data);
                }
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            try
            {
                var data = workfilebll.GetEntity(keyValue);
                return ToJsonResult(data);
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
            try
            {
                workfilebll.RemoveForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
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
        public ActionResult SaveForm(string keyValue, WorkfileEntity entity)
        {
            try
            {
                workfilebll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
        }
        #endregion
    }
}
