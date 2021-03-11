using ERCHTMS.Busines.DangerousJob;
using ERCHTMS.Entity.DangerousJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.DangerousJob.Controllers
{
    /// <summary>
    /// 描 述：危险作业流程流转表
    /// </summary>
    public class DangerousJobFlowDetailController : MvcControllerBase
    {
        private DangerousJobFlowDetailBLL dangerousJobFlowDetailbll = new DangerousJobFlowDetailBLL();

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
        /// 高处作业表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HeightWorkingDetail()
        {
            return View();
        }
        /// <summary>
        /// 流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
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
            dangerousJobFlowDetailbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, DangerousJobFlowDetailEntity entity)
        {
            try
            {
                dangerousJobFlowDetailbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Success("操作失败。");
            }

        }
        /// <summary>
        /// 审核保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主表主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CheckSaveForm(string keyValue, DangerousJobFlowDetailEntity entity)
        {
            try
            {
                dangerousJobFlowDetailbll.CheckSaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Success("操作失败。");
            }
        }

        /// <summary>
        ///风险审批单审核保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主表主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApprovalFormCheckSaveForm(string keyValue, DangerousJobFlowDetailEntity entity)
        {
            try
            {
                dangerousJobFlowDetailbll.ApprovalFormCheckSaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Success("操作失败。");
            }

        }
        #endregion
    }
}