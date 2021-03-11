using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Linq;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：外包资料说明用户设置表
    /// </summary>
    public class OutcommitfilesettingController : MvcControllerBase
    {
        private OutcommitfilesettingBLL outcommitfilesettingbll = new OutcommitfilesettingBLL();

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
            var data = outcommitfilesettingbll.GetList(queryJson);
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
            var data = outcommitfilesettingbll.GetEntity(keyValue);
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
            outcommitfilesettingbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OutcommitfilesettingEntity entity)
        {
            outcommitfilesettingbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存当前用户的设置
        /// </summary>
        /// <param name="Setting"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SettingUserExplain(int Setting, string FileCommitId)
        {
           
            Operator currUser = OperatorProvider.Provider.Current();
            var s=outcommitfilesettingbll.GetList().Where(x => x.FileCommitId == FileCommitId && x.UserId == currUser.UserId).FirstOrDefault();
            //var s = new OutcommitfilesettingEntity();
            if (s == null)
            {
                s = new OutcommitfilesettingEntity();
                s.IsSetting = Setting;
                s.UserAccount = currUser.Account;
                s.UserId = currUser.UserId;
                s.UserName = currUser.UserName;
                s.FileCommitId = FileCommitId;
            }
            else {
                s.IsSetting = Setting;
            }
            outcommitfilesettingbll.SaveForm(s.ID, s);
            return Success("操作成功。");
        }

        #endregion
    }
}
