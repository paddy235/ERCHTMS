using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Cache.Factory;

namespace ERCHTMS.Cache
{
   public class TokenCache
    {
        /// <summary>
        /// 缓存双控对接SIS平台token
        /// </summary>
        /// <returns></returns>
       public string GetToken()
        {
            try
            {
                string cachevalue = CacheFactory.Cache().GetCache<string>("SIS_Token");
                if (string.IsNullOrEmpty(cachevalue))
                {
                    string token = "";
                    var appID = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("TokenAppID");
                    var secret = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("TokenSecret");
                    var dataExchangeUrl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("GetTokenUrl");
                    string url = dataExchangeUrl + "appid=" + appID + "&secret=" + secret;
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                    wc.Encoding = Encoding.UTF8;
                    //发送请求到web api并获取返回值，默认为post方式
                    string data = wc.DownloadString(new Uri(url));
                    dynamic objectToken = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
                    token = objectToken.token;
                    CacheFactory.Cache().WriteCache(token, "SIS_Token", DateTime.Now.AddSeconds(7000));
                    return token;
                }
                else
                {
                    return cachevalue;
                }
            }
            catch (Exception e)
            {
                return "";
            }
           
        }

        /// <summary>
        /// 缓存双控对接Safety平台token
        /// </summary>
        /// <returns></returns>
       public string GetSafetyToken()
        {
            try
            {
                string cachevalue = CacheFactory.Cache().GetCache<string>("Safety_Token");
                if (string.IsNullOrEmpty(cachevalue))
                {
                    string token = "";
                    var appID = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("SafetyAppID");
                    var secret = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("SafetySecret");
                    var dataExchangeUrl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("GetTokenUrl");
                    string url = dataExchangeUrl + "appid=" + appID + "&secret=" + secret;
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                    wc.Encoding = Encoding.UTF8;
                    //发送请求到web api并获取返回值，默认为post方式
                    string data = wc.DownloadString(new Uri(url));
                    dynamic objectToken = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
                    token = objectToken.token;
                    CacheFactory.Cache().WriteCache(token, "Safety_Token", DateTime.Now.AddSeconds(7000));
                    return token;
                }
                else
                {
                    return cachevalue;
                }
            }
            catch (Exception e)
            {
                return "";
            }

        }
    }
}
