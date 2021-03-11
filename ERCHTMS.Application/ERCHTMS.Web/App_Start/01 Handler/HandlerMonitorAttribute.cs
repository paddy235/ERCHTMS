using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util;
using BSFramework.Util.Attributes;
using BSFramework.Util.Log;
using ERCHTMS.Busines.SystemManage;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;


namespace ERCHTMS.Web
{


    /// <summary>
    /// 操作跟踪器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class HandlerMonitorAttribute : ActionFilterAttribute, IExceptionFilter
    {

        private PermissionMode _customMode;
        public string _actionName = string.Empty;
        public int _actionType;
        public HandlerMonitorAttribute(int actionType, string actionName, PermissionMode Mode = PermissionMode.Enforce)
        {
            _actionType = actionType;
            _actionName = actionName;
            _customMode = Mode;
        }

        #region Action时间监控
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //忽略验证
            if (_customMode == PermissionMode.Ignore)
            {
                return;
            }
            //获取控制器名称（配置功能菜单时必须要求模块编码与控制器名称保持一致，否则此处验证会有问题）
            string controlName = filterContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();
            if (OperatorProvider.Provider.Current()==null)
            {
                return;
            }
            //是否超级管理员
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return;
            }
            else
            {
                //ModuleBLL moduleBLL = new ModuleBLL();
                ////获取当前请求的地址
                //string currentUrl = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
                ////根据模块编码获取模块ID
                //string moduleId = moduleBLL.GetModuleIdByCode(controlName);
                //ContentResult Content = new ContentResult();
                //if (HttpContext.Current.Request.HttpMethod == "GET")
                //{
                //    Content.Content = "<script type='text/javascript'>alert('很抱歉！您的权限不足，访问被拒绝！');top.window.location.href='" + HttpContext.Current.Request.ApplicationPath + "/login';</script>";
                //}
                //else
                //{
                //    Content.Content = "很抱歉！您的权限不足，访问被拒绝！";
                //}
                //if (string.IsNullOrEmpty(currentUrl))
                //{
                //    filterContext.Result = Content;
                //    return;
                //}
                //else
                //{
                //    //判断当前用户对模块的功能权限
                //    bool res = new AuthorizeBLL().HasOperAuthority(OperatorProvider.Provider.Current(), moduleId, GetEnCodeByActionType());
                //    if (!res)
                //    {
                //        filterContext.Result = Content;
                //        return;
                //    }
                //}
            }
        }

        /// <summary>
        /// 执行完成之后记录对应的动作事件
        /// </summary>
        /// <param name="filterContext"></param>
        [ValidateInput(false)]
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string argsString = string.Empty;

            string tempValue = "";

            NameValueCollection npara = HttpContext.Current.Request.Form; 

            //NameValueCollection npara = HttpContext.Current.Request.Params;

            foreach (String s in npara.AllKeys)
            {
                if (s.Equals("NewsContent") || s.Equals("EmailContent")) 
                {
                    continue ;
                }
                if (!s.Contains("HTTP") && !s.Contains("ALL_RAW") && !s.Contains("_") && !s.Contains("LoginUserKey")) 
                {
                    if (npara[s] == null)
                    {
                        tempValue = "空值";
                    }
                    else
                    {
                        tempValue = npara[s].ToString();
                    }
                    argsString += s + ":" + tempValue + "|";
                }
            }
            if (argsString.Length>0)
            {
                 argsString = argsString.Substring(0, argsString.Length - 1);
            }

            if (filterContext == null)
                return;
            if (OperatorProvider.Provider.Current() == null)
            {
                return;
            }
            if (OperatorProvider.Provider.IsOverdue())
                return;
            var log = LogFactory.GetLogger(filterContext.Controller.ToString());
            LogMessage logMessage = new LogMessage();
            logMessage.OperationTime = DateTime.Now;
            logMessage.Url = HttpContext.Current.Request.RawUrl;
            logMessage.Class = filterContext.Controller.ToString();
            logMessage.Ip = Net.Ip;
            logMessage.Host = Net.Host;
            logMessage.Browser = Net.Browser;
            logMessage.UserName = OperatorProvider.Provider.Current().Account + "（" + OperatorProvider.Provider.Current().UserName + "）";
            string strMessage = new LogFormat().ExceptionFormat(logMessage);

            LogEntity logEntity = new LogEntity();
            logEntity.CategoryId = _actionType;
            logEntity.OperateTypeId = _actionType.ToString();
            logEntity.OperateType = EnumAttribute.GetDescription(GetOperationType(_actionType.ToString()));
            logEntity.OperateAccount = logMessage.UserName;
            logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
            logEntity.ExecuteResult = 1;
            logEntity.Module = SystemInfo.CurrentModuleName;
            logEntity.ModuleId = SystemInfo.CurrentModuleId;
            logEntity.ExecuteResultJson = "操作信息:" + _actionName + ", 请求引用:" + argsString + " , 其他信息:" + strMessage;
            logEntity.WriteLog();
        }
        #endregion

        #region 错误日志

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {

            }
        }
        //根据操作类型枚举获取功能编码（如新增：add,编辑：edit等）
        private string GetEnCodeByActionType()
        {
            string enCode = "";
            switch (_actionType)
            {
                case 3:
                    enCode = "search";
                    break;
                case 5:
                    enCode = "add";
                    break;
                case 6:
                    enCode = "delete";
                    break;
                case 7:
                    enCode = "edit";
                    break;
                default:
                    enCode = "add";
                    break;
            }
            return enCode;
        }
        #endregion
        /// <summary>
        /// 常用操作枚举
        /// </summary>
        /// <param name="operationType"></param>
        /// <returns></returns>
        public OperationType GetOperationType(string operationType)
        {
            OperationType opera = new OperationType();
            switch (operationType)
            {
                case "0":
                    opera = OperationType.Other;
                    break;
                case "Other":
                    opera = OperationType.Other;
                    break;

                case "1":
                    opera = OperationType.Login;
                    break;
                case "Login":
                    opera = OperationType.Login;
                    break;

                case "2":
                    opera = OperationType.Exit;
                    break;
                case "Exit":
                    opera = OperationType.Exit;
                    break;

                case "3":
                    opera = OperationType.Visit;
                    break;
                case "Visit":
                    opera = OperationType.Visit;
                    break;

                case "4":
                    opera = OperationType.Leave;
                    break;
                case "Leave":
                    opera = OperationType.Leave;
                    break;

                case "5":
                    opera = OperationType.Create;
                    break;
                case "Create":
                    opera = OperationType.Create;
                    break;

                case "6":
                    opera = OperationType.Delete;
                    break;
                case "Delete":
                    opera = OperationType.Delete;
                    break;

                case "7":
                    opera = OperationType.Update;
                    break;
                case "Update":
                    opera = OperationType.Update;
                    break;

                case "8":
                    opera = OperationType.Submit;
                    break;
                case "Submit":
                    opera = OperationType.Submit;
                    break;

                case "9":
                    opera = OperationType.Exception;
                    break;
                case "Exception":
                    opera = OperationType.Exception;
                    break;

                case "10":
                    opera = OperationType.AppLogin;
                    break;
                case "AppLogin":
                    opera = OperationType.AppLogin;
                    break;
            }

            return opera;

        }

    }
   
    /// <summary>
    /// 监控日志对象
    /// </summary>
    public class MonitorLog
    {
        public string AreaName { get; set; }
        public string ControllerName
        {
            get;
            set;
        }
        public string ActionName
        {
            get;
            set;
        }
        public DateTime ExecuteStartTime
        {
            get;
            set;
        }
        public DateTime ExecuteEndTime
        {
            get;
            set;
        }
        /// <summary>
        /// Form 表单数据
        /// </summary>
        public NameValueCollection FormCollections
        {
            get;
            set;
        }
        /// <summary>
        /// URL 参数
        /// </summary>
        public NameValueCollection QueryCollections
        {
            get;
            set;
        }
        /// <summary>
        /// 监控类型
        /// </summary>
        public enum MonitorType
        {
            Action = 1,
            View = 2
        }
        /// <summary>
        /// 获取监控指标日志
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public string GetLoginfo(MonitorType mtype = MonitorType.Action)
        {
            string ActionView = "Action执行时间监控：";
            string Name = "Action";
            if (mtype == MonitorType.View)
            {
                ActionView = "View视图生成时间监控：";
                Name = "View";
            }
            string Msg = @"{0},ControllerName:{1},Controller:{8},Name:{2},开始时间：{3},结束时间：{4},总 时 间：{5}秒,Form表单数据：{6},URL参数：{7}";
            return string.Format(Msg, ActionView, ControllerName, ActionName, ExecuteStartTime, ExecuteEndTime, (ExecuteEndTime - ExecuteStartTime).TotalSeconds, GetCollections(FormCollections),
                GetCollections(QueryCollections), Name);
        }

        /// <summary>
        /// 获取Post 或Get 参数
        /// </summary>
        /// <param name="Collections"></param>
        /// <returns></returns>
        public string GetCollections(NameValueCollection Collections)
        {
            string Parameters = string.Empty;
            if (Collections == null || Collections.Count == 0)
            {
                return Parameters;
            }
            foreach (string key in Collections.Keys)
            {
                Parameters += string.Format("{0}={1}&", key, Collections[key]);
            }
            if (!string.IsNullOrWhiteSpace(Parameters) && Parameters.EndsWith("&"))
            {
                Parameters = Parameters.Substring(0, Parameters.Length - 1);
            }
            return Parameters;
        }
    }
}
