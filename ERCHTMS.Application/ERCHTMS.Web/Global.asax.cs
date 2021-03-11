using BSFramework.Data.EF;
using System;
using System.Data.Entity;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ERCHTMS.Web.TaskManage;
using FluentScheduler;
using ERCHTMS.Code;
using BSFramework.Util;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace ERCHTMS.Web
{
    /// <summary>
    /// 应用程序全局设置
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        //string StrRegex = @"\b(alert|confirm|prompt)\b|^\+/v(8|9)|<[^>]*?=[^>]*?&#[^>]*?>|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|<\s*script\b|<\s*img\b|\bEXEC\b|UNION.+?SELECT\s+|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT\s+|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";

        string StrRegex = System.Configuration.ConfigurationManager.AppSettings["StrRegex"];
        /// <summary>
        /// 启动应用程序
        /// </summary>
        protected void Application_Start()
        {
            JobManager.Initialize(new CommitmentTaskRegistry());
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleTable.EnableOptimizations = true;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        public  bool PostData()
        {
            bool result = false;
            NameValueCollection npara = HttpContext.Current.Request.Form;
            foreach (string key in npara.AllKeys)
            {
                result = CheckData(npara[key].ToString());
                if (result)
                {
                    break;
                }
            }
            return result;
        }

        public  bool GetData()
        {
            bool result = false;
            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.QueryString[i].ToString());
                if (result)
                {
                    break;
                }
            }
            return result;
        }
        public  bool CookieData()
        {
            bool result = false;
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.Cookies[i].Value.ToLower());
                if (result)
                {
                    break;
                }
            }
            return result;

        }
        public  bool referer()
        {
            bool result = false;
            return result = CheckData(HttpContext.Current.Request.UrlReferrer.ToString());
        }
        public  bool CheckData(string inputData)
        {
            if(string.IsNullOrWhiteSpace(StrRegex))
            {
                string env = System.Configuration.ConfigurationManager.AppSettings["Environment"];
                if (env!="dev")
                {
                    StrRegex = @"\b(alert|confirm|prompt)\b|^\+/v(8|9)|\b(and|or)\b.{1,6}?(=|>|\bin\b|\blike\b)/|script\b|\s*img\b|\bEXEC\b|UNION.+?SELECT\s+|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT\s+|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";
                }
                else
                {
                    return false;
                }
            }
            if (Regex.IsMatch(inputData, StrRegex,RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!(Request.RequestType.ToUpper() == "POST" || Request.RequestType.ToUpper() == "GET"))
            {
                Response.StatusCode = 403;
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                Response.ContentType = "text/html;charset=gb2312";
                Response.Write("您的请求非法,系统拒绝响应!");
                Response.End();
            }
            //解决DateTime.Now.ToString()默认格式问题。
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN", true) { DateTimeFormat = { ShortDatePattern = "yyyy-MM-dd", FullDateTimePattern = "yyyy-MM-dd HH:mm:ss", LongTimePattern = "HH:mm:ss", LongDatePattern = "yyyy-MM-dd" } };
            var url=Request.Url;
            //var env = Config.GetValue("Environment");
            //if (!string.IsNullOrWhiteSpace(exts))
            //{
            //    string fileExt = System.IO.Path.GetExtension(url.AbsolutePath.ToLower());
            //    string key = FormsAuth.GetUserKey();
            //    //if (string.IsNullOrWhiteSpace(key) && !Request.Url.ToString().ToLower().Contains("erchtms/login"))
            //    //{
            //    //    Response.Clear();
            //    //    Response.WriteFile(Server.MapPath("~/Content/images/404_r1_c2.png"));
            //    //    Response.End();
            //    //}
            //    //if (Request.UrlReferrer == null && exts.Contains(fileExt))
            //    //{
            //    //    Response.Clear();
            //    //    Response.WriteFile(Server.MapPath("~/Content/images/404_r1_c2.png"));
            //    //    Response.End();
            //    //}
            //}
            if (Request.UrlReferrer != null)
            {
                if (referer())
                {
                    Response.StatusCode = 403;
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                    Response.ContentType = "text/html;charset=gb2312";
                    Response.Write("您提交的数据有恶意字符！");
                    Response.End();
                }
            }
            //if(env!="dev")
            //{
            //    if (Request.RequestType.ToUpper() == "POST")
            //    {
            //        if (PostData())
            //        {
            //            Response.StatusCode = 403;
            //            Response.Write("您提交的数据有恶意字符！");
            //            Response.End();
            //        }
            //    }
            //}
            if (Request.RequestType.ToUpper() == "GET")
            {
                if (GetData())
                {
                    Response.StatusCode = 403;
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                    Response.ContentType = "text/html;charset=gb2312";
                    Response.Write("您提交的数据有恶意字符！");
                    Response.End();
                }
            }
        }
        /// <summary>
        /// 应用程序错误处理
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
        }
    }
}