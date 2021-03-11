using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using ERCHTMS.Entity.LllegalStandard;
using ERCHTMS.Busines.LllegalStandard;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Busines.SystemManage;
using System.Web;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections;
using ERCHTMS.Entity.SystemManage.ViewModel;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.ManyPowerCheck.Controllers
{
    /// <summary>
    /// 描 述：违章标准表
    /// </summary>
    public class ManyPowerCheckController : MvcControllerBase
    {
        private ManyPowerCheckBLL manyPowerCheckbll = new ManyPowerCheckBLL();

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
            var queryParam = queryJson.ToJObject();
            Operator opertator = new OperatorProvider().Current();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"a.autoid,a.createdate,a.createuserid,a.createuserdeptcode,a.createuserorgcode,a.modifydate,a.modifyuserid,a.belongmodule,a.belongmodulecode,
             a.moduleno,a.modulename,a.flowname,a.checkdeptid,a.checkdeptcode,a.checkdeptname,a.checkroleid,a.checkrolename,a.remark,b.fullname orginezename,a.serialnum";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"bis_manypowercheck a left join base_department b on a.createuserorgcode = b.encode";
            pagination.conditionJson = " 1=1";
            string authWhere = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
            if (!string.IsNullOrEmpty(authWhere))
            {//数据权限,含系统管理员添加的数据。
                pagination.conditionJson += " and (" + authWhere + " or a.CREATEUSERORGCODE='00')";
            }
            else
            {
                pagination.conditionJson += " and a.CREATEUSERORGCODE='" + opertator.OrganizeCode + "'";
            }
            //模块名称
            if (!queryParam["modulename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  a.moduleno = '{0}' ", queryParam["modulename"].ToString());
            }
            //审核部门名称
            if (!queryParam["checkdeptname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and a.checkdeptname like '%{0}%'", queryParam["checkdeptname"].ToString());
            }
            //角色名称 
            if (!queryParam["checkrolename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and a.checkrolename like '%{0}%'", queryParam["checkrolename"].ToString());
            }
            //所属模块
            if (!queryParam["belongmodule"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and a.belongmodulecode = '{0}'", queryParam["belongmodule"].ToString());
            }
            
            var data = manyPowerCheckbll.GetManyPowerCheckEntityPage(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = manyPowerCheckbll.GetEntity(keyValue);
            //返回值
            var josnData = new
            {
                data = data
            };

            return Content(josnData.ToJson());
        }

        /// <summary>
        /// 判断是否配置逐级审核 
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult HasConfiguration(string modulename)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var data = manyPowerCheckbll.GetList(user.OrganizeCode, modulename);

                if (data.Count > 0)
                {
                    return Content("true");
                }
                else
                {
                    return Content("false");
                }
            }
            catch (Exception ex)
            {
                return Content("false");
            }
            
        }
        /// <summary>
        /// 判断是否配置带有指定流程名称的逐级审核 
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult HasConfigurationByFlowName(string modulename, string FlowName = "")
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var data = manyPowerCheckbll.GetList(user.OrganizeCode, modulename);

                if (data.Count > 0)
                {
                    if (data.Where(t => t.FLOWNAME.Contains(FlowName)).Count() > 0)
                    {
                        return Content("true");
                    }
                    else
                    {
                        return Content("false");
                    }
                }
                else
                {
                    return Content("false");
                }
            }
            catch (Exception ex)
            {
                return Content("false");
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
            manyPowerCheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ManyPowerCheckEntity entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.ScriptCurcontent))
            {
                entity.ScriptCurcontent = HttpUtility.UrlDecode(entity.ScriptCurcontent);
            }
            manyPowerCheckbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

    }
}
