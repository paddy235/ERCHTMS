using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ERCHTMS.AppSerivce
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// 启动应用程序
        /// </summary>
        protected void Application_Start(object sender, EventArgs e)
        {
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //GlobalConfiguration.Configuration.Filters.Add(new WebAPIHandlerErrorAttribute());

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //解决DateTime.Now.ToString()默认格式问题。
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN", true) { DateTimeFormat = { ShortDatePattern = "yyyy-MM-dd", FullDateTimePattern = "yyyy-MM-dd HH:mm:ss", LongTimePattern = "HH:mm:ss", LongDatePattern = "yyyy-MM-dd" } };

        }

        /// <summary>
        /// 应用程序错误处理
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
        }
    }

    public class LowercaseContractResolver : DefaultContractResolver
    {
        Dictionary<string, string> dict_props = null;
        string[] props = null;
        bool retain;
        public LowercaseContractResolver()
        {

        }
        /// <summary>
        /// 属性转小写
        /// </summary>
        /// <param name="dictPropertyName"></param>
        public LowercaseContractResolver(Dictionary<string, string> dictPropertyName)
        {
            this.dict_props = dictPropertyName;
        }

        /// <summary>
        /// 是否保留属性
        /// </summary>
        /// <param name="props">传入的属性数组</param>
        /// <param name="retain">true:表示props是需要保留的字段，false:表示props是要排队的字段</param>
        public LowercaseContractResolver(string[] props, bool retain = true)
        {
            this.props = props;
            this.retain = retain;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictPropertyName">需要转换列的键值对</param>
        /// <param name="props">传入的属性数组</param>
        /// <param name="retain">true:表示props是需要保留的字段，false:表示props是要排队的字段</param>
        public LowercaseContractResolver(Dictionary<string, string> dictPropertyName, string[] props, bool retain = true)
        {
            this.dict_props = dictPropertyName;
            this.props = props;
            this.retain = retain;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            string newPropertyName = string.Empty;
            if (dict_props != null && dict_props.TryGetValue(propertyName, out newPropertyName))
            {
                return newPropertyName.ToLower();
            }
            else
            {
                return propertyName.ToLower();
            }
        }
        /// <summary>
        /// 是否排除指定的属性，此方法，只有实体类才会有效，DataTable请用dt.Columns.Remove("列名")删除
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
            if (props != null && props.Length > 0)
            {
                return list.Where(p =>
                {
                    if (retain)
                    {
                        return props.Contains(p.PropertyName);
                    }
                    else
                    {
                        return !props.Contains(p.PropertyName);
                    }

                }).ToList();
            }
            return list;
        }
    }
}