using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SaftProductTargetManage;
using ERCHTMS.Entity.SaftProductTargetManage;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.SaftProductTargetManage.Controllers
{

    /// <summary>
    /// 描 述：安全生产目标
    /// </summary>
    public class SafeproductController : MvcControllerBase
    {
        private SafeProductBLL safeproductbll = new SafeProductBLL();
        private OrganizeBLL organizeBLL = new OrganizeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

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
            var data = safeproductbll.GetList(queryJson);
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
            var data = safeproductbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        [HttpGet]
        public ActionResult GetSafeInfo(string dateYear, string belongId)
        {
            var data = safeproductbll.GetSafeByCondition(dateYear, belongId);
            return ToJsonResult(data);

        }

        /// <summary>
        /// 计算目标值
        /// </summary>
        /// <param name="belongtype"></param>
        /// <returns></returns>
         [HttpGet]
        public string calculateGoal(string belongtype = "",string belongdeptid="",string year="")
        {
            try
            {
                return safeproductbll.calculateGoal(belongtype, belongdeptid, year);
            }
            catch (System.Exception ex)
            {

                throw;
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
            safeproductbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafeProductEntity entity, [System.Web.Http.FromBody]string dataJson, [System.Web.Http.FromBody]string dataJson1)
        {
            if (safeproductbll.SaveForm(keyValue, entity) > 0)
            {
                //保存关联的从表记录(安全目标项目)
                if (dataJson.Length > 0)
                {
                    SafeProductProjectBLL safeproductPbll = new SafeProductProjectBLL();
                    if (safeproductPbll.Remove(entity.Id) > 0)
                    {
                        List<SafeProductProjectEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafeProductProjectEntity>>(dataJson);
                        foreach (SafeProductProjectEntity data in list)
                        {
                            safeproductPbll.SaveForm("", data);
                        }
                    }
                }
                //保存关联的从表记录(安全目标责任书)
                if (dataJson1.Length > 0)
                {
                    SafeProductDutyBookBLL safeBookbll = new SafeProductDutyBookBLL();
                    if (safeBookbll.Remove(entity.Id) > 0)
                    {
                        List<SafeProductDutyBookEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafeProductDutyBookEntity>>(dataJson1);
                        foreach (SafeProductDutyBookEntity data in list)
                        {
                            safeBookbll.SaveForm("", data);
                        }
                    }
                }
            }
            return Success("操作成功。");
        }
        #endregion
    }
}