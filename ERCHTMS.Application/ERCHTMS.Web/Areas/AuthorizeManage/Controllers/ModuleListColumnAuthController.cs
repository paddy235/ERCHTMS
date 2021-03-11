using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.AuthorizeManage.Controllers
{
    /// <summary>
    /// 描 述：应用模块列表的列查看权限设置表
    /// </summary>
    public class ModuleListColumnAuthController : MvcControllerBase
    {
        private ModuleListColumnAuthBLL modulelistcolumnauthbll = new ModuleListColumnAuthBLL();

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

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Setting()
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
            var data = modulelistcolumnauthbll.GetList(queryJson);
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
            var data = modulelistcolumnauthbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetListByType(Pagination pagination, string queryJson)
        {
            var data = modulelistcolumnauthbll.GetListByType(pagination, queryJson);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取权限下的列表
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetColumnAuth(string moduleId)
        {
            Operator opertator = new OperatorProvider().Current();
            //系统默认的列表设置
            var defaultdata = modulelistcolumnauthbll.GetEntity(moduleId, "", 0);
            //当前用户的列表设置
            var data = modulelistcolumnauthbll.GetEntity(moduleId, opertator.UserId, 1);


            if (null != data)
            {
                var jsonData = new { data = data, isSystem = false }; //系统默认
                return ToJsonResult(jsonData);
            }
            else
            {
                var jsonData = new { data = defaultdata, isSystem = true }; //非系统默认
                return ToJsonResult(jsonData);
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
            modulelistcolumnauthbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ModuleListColumnAuthEntity entity)
        {
            Operator opertator = new OperatorProvider().Current();
            entity.USERID = opertator.UserId;
            entity.USERNAME = opertator.UserName;
            if (!string.IsNullOrEmpty(keyValue)) 
            {
                modulelistcolumnauthbll.RemoveForm(keyValue);
            }
            if (!string.IsNullOrEmpty(entity.MODULEID)) 
            {

            }
            var defaultdata = modulelistcolumnauthbll.GetEntity(entity.MODULEID, "", 0);
            if (null != defaultdata) 
            {
                entity.LISTCOLUMNFIELDS = defaultdata.LISTCOLUMNFIELDS;
                entity.LISTCOLUMNNAME = defaultdata.LISTCOLUMNNAME;
            }
            modulelistcolumnauthbll.SaveForm("", entity);
            return Success("操作成功。");
        }
        #endregion
    }
}