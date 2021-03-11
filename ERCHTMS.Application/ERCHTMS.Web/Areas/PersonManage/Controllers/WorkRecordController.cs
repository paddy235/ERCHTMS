using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// 描 述：工作记录表
    /// </summary>
    public class WorkRecordController : MvcControllerBase
    {
        private WorkRecordBLL workrecordbll = new WorkRecordBLL();
        private UserBLL userBLL = new UserBLL();
        #region 视图
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
            var data = workrecordbll.GetList(queryJson);
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
            var data = workrecordbll.GetEntity(keyValue);
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
            workrecordbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, WorkRecordEntity entity)
        {
            if (keyValue.Trim() == "")
            {
                //如果是新增 则是手动添加
                entity.WorkType = 0;
                string uid = entity.UserId;//获取到需要转岗的用户id
                UserEntity ue = userBLL.GetEntity(uid);
                entity.UserName = ue.RealName;
            }
            else
            {
                if (entity.DeptName == null)
                {
                    entity.DeptName = "";
                }

                if (entity.JobName == null)
                {
                    entity.JobName = "";
                }
            }

            workrecordbll.NewSaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        #endregion
    }
}