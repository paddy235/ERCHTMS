using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Log;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.PublicInfoManage;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace ERCHTMS.Web
{
    /// <summary>
    /// 描 述：控制器基类
    /// </summary>
    [HandlerLogin(LoginMode.Enforce)]
    //[HandlerAuthorize(PermissionMode.Enforce)]
    public abstract class MvcControllerBase : Controller    
    {
        /// <summary>
        /// 删除记录关联的附件及物理文件
        /// </summary>
        /// <param name="recId"></param>
        public void DeleteFiles(string recId)
        {
            if (!string.IsNullOrWhiteSpace(recId)){
                FileInfoBLL fileBll = new FileInfoBLL();
                var list = fileBll.GetFileList(recId);
                foreach (var file in list)
                {
                    fileBll.ThoroughRemoveForm(file.FileId);
                    var filePath = HttpContext.Server.MapPath(file.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
        }
        private Log _logger;
        /// <summary>
        /// 日志操作
        /// </summary>
        public Log Logger
        {
            get { return _logger ?? (_logger = LogFactory.GetLogger(this.GetType().ToString())); }
        }
        [ValidateInput(false)]
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string env = System.Configuration.ConfigurationManager.AppSettings["Environment"];
            if (env != "dev" && HttpContext.Request.HttpMethod.ToUpper()=="POST")
            {
                string StrRegex = System.Configuration.ConfigurationManager.AppSettings["StrRegex"];
                if(string.IsNullOrWhiteSpace(StrRegex))
                {
                    StrRegex = @"\b(alert|confirm|prompt)\b|^\+/v(8|9)|\b(and|or)\b.{1,6}?(=|>|\bin\b|\blike\b)/|script\b|\bEXEC\b|UNION.+?SELECT\s+|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT\s+|DELETE).+?INTO|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";
                    //            StrRegex = @"\b(alert|confirm|prompt)\b|^\+/v(8|9)|\b(and|or)\b.{1,6}?(=|>|\bin\b|\blike\b)/|script\b|\s*img\b|\bEXEC\b|UNION.+?SELECT\s+|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT\s+|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";
                }
                NameValueCollection npara = HttpContext.Request.Form;
                foreach (string s in npara.AllKeys)
                {
                    if (Regex.IsMatch(npara[s], StrRegex, RegexOptions.IgnoreCase))
                    {
                        filterContext.Result = Error("您提交的数据有恶意字符");
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual ActionResult ToJsonResult(object data)
        {
            return Content(data.ToJson());
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { type = ResultType.success, message = message }.ToJson());
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { type = ResultType.success, message = message, resultdata = data }.ToJson());
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Error(string message)
        {
            string env = System.Configuration.ConfigurationManager.AppSettings["Environment"];
            if(env!="dev")
            {
                if(message.Contains("ORA-"))
                {
                    message = "对不起,出错了。请提交正确合法的数据";
                }
            }
            return Content(new AjaxResult { type = ResultType.error, message = message }.ToJson());
        }
    }
}
