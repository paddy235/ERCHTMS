using ERCHTMS.Entity.DangerousJobConfig;
using ERCHTMS.Busines.DangerousJobConfig;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using ERCHTMS.Code;
using System;
using System.Linq;

namespace ERCHTMS.Web.Areas.DangerousJobConfig.Controllers
{
    /// <summary>
    /// 描 述：危险作业安全措施配置
    /// </summary>
    public class SafetyMeasureConfigController : MvcControllerBase
    {
        private SafetyMeasureConfigBLL safetymeasureconfigbll = new SafetyMeasureConfigBLL();
        private SafetyMeasureDetailBLL safetymeasuredetailbll = new SafetyMeasureDetailBLL();

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
                ,a.createdate,worktype,d.itemname as worktypename,a.configtypename,deptid,deptcode,deptname,a.createusername";
                pagination.p_tablename = "dj_safetymeasureconfig a left join base_dataitemdetail d on a.worktype=d.itemvalue and d.itemid =(select itemid from base_dataitem where itemcode='DangerousJobConfig')";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = string.Format("a.createuserid='System'  or a.createuserorgcode='{0}' or a.createuserid='{1}'", user.OrganizeCode, user.UserId);
                }
                var data = safetymeasureconfigbll.GetPageListJson(pagination, queryJson);
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
            var data = safetymeasureconfigbll.GetList(queryJson);
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
            var data = safetymeasureconfigbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取配置集合
        /// </summary>
        /// <param name="WorkType"></param>
        /// <param name="ConfigType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetConfigList(string WorkType, string ConfigType)
        {
            try
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var entity = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == WorkType && t.ConfigType == ConfigType && t.DeptCode == user.OrganizeCode).FirstOrDefault();
                if (entity != null)
                {
                    var data = safetymeasuredetailbll.GetList("").Where(t => t.RecId == entity.Id).OrderBy(t => t.SortNum);
                    return Success("获取成功", data);
                }
                else
                {
                    return Error("请联系系统管理员配置相应项!");
                }
               
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
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
            safetymeasureconfigbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="Detail">配置内容</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, SafetyMeasureConfigEntity entity,string Detail)
        {
            try
            {
                List<SafetyMeasureDetailEntity> list = JsonConvert.DeserializeObject<List<SafetyMeasureDetailEntity>>(Detail);
                safetymeasureconfigbll.SaveForm(keyValue, entity, list);
                return Success("操作成功。");
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        #endregion
    }
}
