using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Controllers
{
    /// <summary>
    /// 描 述：错误处理
    /// </summary>
    public class ErrorController : Controller
    {
        #region 视图功能
        /// <summary>
        /// 错误页面（异常页面）
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ActionResult ErrorMessage(string message = "对不起,应用出错了")
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (HttpContext.Application["error"]!=null)
            {
                dict = (Dictionary<string, string>)HttpContext.Application["error"];
            }
            else
            {
                dict.Add("Error", message);
            }
            ViewData["Message"] = dict;
            return View();
        }
        public ActionResult ErrorMsg(string message = "对不起,应用出错了")
        {
            ViewData["Message"] = message;
            return View();
        }
        /// <summary>
        /// 错误页面（错误路径404）
        /// </summary>
        /// <returns></returns>
        public ActionResult ErrorPath404()
        {
            return View();
        }
        /// <summary>
        /// 错误页面（升级浏览器软件）
        /// </summary>
        /// <returns></returns>
        public ActionResult Browsers()
        {
            return View();
        }
        #endregion

        #region 提交数据
        #endregion
    }
}
