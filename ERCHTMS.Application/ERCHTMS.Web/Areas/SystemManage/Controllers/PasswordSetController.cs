using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：密码规则
    /// </summary>
    public class PasswordSetController : MvcControllerBase
    {
        private PasswordSetBLL psBLL = new PasswordSetBLL();

        #region 视图功能
        /// <summary>
        /// 区域管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 区域表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 区域详细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Detail()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 区域列表
        /// </summary>
        /// <param name="value">当前主键</param>
        /// <param name="keyword">关键字查询</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string orgCode)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var data = psBLL.GetList(user.OrganizeCode).ToList();
                return Content(data.ToJson());
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        /// <summary>
        /// 区域实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            try
            {
               var data = psBLL.GetEntity(keyValue);
               return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HandlerMonitor(6, "删除区域信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            psBLL.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存区域表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="areaEntity">区域实体</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存区域表单(新增、修改)")]
        public ActionResult SaveForm(string keyValue, PasswordSetEntity areaEntity)
        {
            try
            {
                psBLL.SaveForm(keyValue, areaEntity);
                return Success("操作成功。");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
          
        }
        #endregion
    }
}
