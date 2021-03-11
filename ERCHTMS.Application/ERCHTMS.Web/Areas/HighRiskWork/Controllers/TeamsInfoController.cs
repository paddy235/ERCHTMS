using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：任务分配班组
    /// </summary>
    public class TeamsInfoController : MvcControllerBase
    {
        private TeamsInfoBLL teamsinfobll = new TeamsInfoBLL();
        private SuperviseWorkInfoBLL superviseworkinfobLL = new SuperviseWorkInfoBLL();
        private TaskShareBLL tasksharebll = new TaskShareBLL();
        private TeamsWorkBLL teamsworkbll = new TeamsWorkBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        #region 视图功能
        /// <summary>
        /// 选择页面
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
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = teamsinfobll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetTeamSpecToJson(string queryJson)
        {
            var data = teamsinfobll.GetList(queryJson);
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
            teamsinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TeamsInfoEntity entity)
        {
            teamsinfobll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        /// <summary>
        /// 结束任务
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult FinishTeamTask(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {

                TeamsInfoEntity u = teamsinfobll.GetEntity(keyValue);
                if (u != null)
                {
                    u.IsAccomplish = "1";
                    teamsinfobll.SaveForm(keyValue, u);
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + user.UserName + "分配任务完成成功，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "\r\n");
                }
            }
            catch (Exception ex)
            {
                //写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：分配任务完成失败，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",异常信息：" + ex.Message + "\r\n");
                return Success("操作失败，错误信息：" + ex.Message);
            }
            return Success("操作成功");
        }
    }
}
