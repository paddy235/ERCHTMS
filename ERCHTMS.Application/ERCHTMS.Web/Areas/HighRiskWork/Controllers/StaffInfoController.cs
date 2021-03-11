using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Net;
using ERCHTMS.Busines.SystemManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：任务分配人员
    /// </summary>
    public class StaffInfoController : MvcControllerBase
    {
        private StaffInfoBLL staffinfobll = new StaffInfoBLL();

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
        /// 监督任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaskIndex()
        {
            return View();
        }
        /// <summary>
        /// 监督任务页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaskForm()
        {
            return View();
        }
        /// <summary>
        /// 文件页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowFiles()
        {
            return View();
        }

        /// <summary>
        /// 检查页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckForm()
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
        public ActionResult GetStaffSpecToJson(string queryJson)
        {
            var data = staffinfobll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取监督任务列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTaskTableToJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                var data = staffinfobll.GetDataTable(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = staffinfobll.GetEntity(keyValue);
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
            staffinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, StaffInfoEntity entity)
        {
            staffinfobll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        public ActionResult TestTask()
        {
            try
            {
                new StaffInfoBLL().SendTaskInfo();
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
