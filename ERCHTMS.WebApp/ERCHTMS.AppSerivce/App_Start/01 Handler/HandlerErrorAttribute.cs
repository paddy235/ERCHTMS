using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util;
using BSFramework.Util.Attributes;
using BSFramework.Util.Extension;
using BSFramework.Util.Log;
using BSFramework.Util.WebControl;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using System.Dynamic;

namespace ERCHTMS.AppSerivce
{
    /// <summary>
    /// 描 述：错误日志（Controller发生异常时会执行这里） 
    /// </summary>
    public class WebAPIHandlerErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 控制器方法中出现异常，会调用该方法捕获异常
        /// </summary>
        /// <param name="context">提供使用</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            WriteLog(context);
            base.OnException(context);
            //context.ActionContext.Response.StatusCode = System.Net.HttpStatusCode.OK;
            //context.Response.Content=System.Net.Http.HttpContent.{ Content = new AjaxResult { type = ResultType.error, message = context.Exception.Message }.ToJson() };
        }
        /// <summary>
        /// 写入日志（log4net）
        /// </summary>
        /// <param name="context">提供使用</param>
        private void WriteLog(HttpActionExecutedContext context)
        {
            string userId = "";
            string json = HttpContext.Current.Request.Params["json"];
            if (!string.IsNullOrEmpty(json))
            {
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                userId = dy.userid;
            }
            var log = LogFactory.GetLogger(context.ActionContext.ControllerContext.Controller.ToString());
            Exception Error = context.Exception;
            LogMessage logMessage = new LogMessage();
            logMessage.OperationTime = DateTime.Now;
            logMessage.Url = HttpContext.Current.Request.RawUrl;
            logMessage.Class = context.ActionContext.ControllerContext.Controller.ToString();
            logMessage.Ip = Net.Ip;
            logMessage.Host = Net.Host;
            logMessage.Browser = Net.Browser;
            LogEntity logEntity = new LogEntity();
            logEntity.Module = "手机App";
            if (!string.IsNullOrEmpty(userId))
            {
                OperatorProvider.AppUserId = userId;
                logMessage.UserName = OperatorProvider.Provider.Current().Account + "（" + OperatorProvider.Provider.Current().UserName + "）";
                logEntity.OperateAccount = logMessage.UserName;
                logEntity.OperateUserId = userId;
            }
            if (Error.InnerException == null)
            {
                logMessage.ExceptionInfo = Error.Message;
            }
            else
            {
                logMessage.ExceptionInfo = Error.InnerException.Message;
            }
            string strMessage = new LogFormat().ExceptionFormat(logMessage);
            log.Error(strMessage);
            logEntity.CategoryId = 4;
            logEntity.OperateTypeId = ((int)OperationType.Exception).ToString();
            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Exception);
            logEntity.ExecuteResult = -1;
            logEntity.ExecuteResultJson = strMessage;
            logEntity.WriteLog();
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        private void SendMail(string body)
        {
            bool ErrorToMail = Config.GetValue("ErrorToMail").ToBool();
            if (ErrorToMail == true)
            {
                string SystemName = Config.GetValue("SystemName");//系统名称
                MailHelper.Send("448941969l@qq.com", SystemName + " - 发生异常", body.Replace("-", ""));
            }
        }
    }
}