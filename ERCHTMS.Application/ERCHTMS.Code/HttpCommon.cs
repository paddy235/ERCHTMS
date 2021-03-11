using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace ERCHTMS.Code
{
    /// <summary>
    /// 用于后台自定义请求及接收
    /// </summary>
    public class HttpCommon
    {
        #region 辅助工具方法

        /// <summary>  
        /// Http同步Get同步请求  
        /// </summary>  
        /// <param name="url">Url地址</param>  
        /// <param name="encode">编码(默认UTF8)</param>  
        /// <returns></returns>  
        public static string HttpGet(string url, Encoding encode = null)
        {
            string result;

            try
            {
                var webClient = new WebClient { Encoding = Encoding.UTF8 };

                if (encode != null)
                    webClient.Encoding = encode;

                result = webClient.DownloadString(url);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>  
        /// Http同步Get异步请求  
        /// </summary>  
        /// <param name="url">Url地址</param>  
        /// <param name="callBackDownStringCompleted">回调事件</param>  
        /// <param name="encode">编码(默认UTF8)</param>  
        public static void HttpGetAsync(string url,
            DownloadStringCompletedEventHandler callBackDownStringCompleted = null, Encoding encode = null)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };

            if (encode != null)
                webClient.Encoding = encode;

            if (callBackDownStringCompleted != null)
                webClient.DownloadStringCompleted += callBackDownStringCompleted;

            webClient.DownloadStringAsync(new Uri(url));
        }

        /// <summary>  
        ///  Http同步Post同步请求  
        /// </summary>  
        /// <param name="url">Url地址</param>  
        /// <param name="postStr">请求Url数据</param>  
        /// <param name="encode">编码(默认UTF8)</param>  
        /// <returns></returns>  
        public static string HttpPost(string url, string postStr = "", Encoding encode = null)
        {
            string result;

            try
            {
                var webClient = new WebClient { Encoding = Encoding.UTF8 };

                if (encode != null)
                {
                    webClient.Encoding = encode;
                }

                var sendData = Encoding.GetEncoding("UTF-8").GetBytes(postStr);
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                webClient.Headers.Add("ContentLength", sendData.Length.ToString(CultureInfo.InvariantCulture));

                var readData = webClient.UploadData(url, "POST", sendData);

                result = Encoding.GetEncoding("UTF-8").GetString(readData);

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }


        /// <summary>  
        ///  Http同步Post同步请求  
        /// </summary>  
        /// <param name="url">Url地址</param>  
        /// <param name="postStr">请求Url数据</param>  
        /// <param name="encode">编码(默认UTF8)</param>  
        /// <returns></returns>  
        public static string HttpPostJson(string url, string postStr = "", Encoding encode = null)
        {
            string result;

            try
            {
                var webClient = new WebClient { Encoding = Encoding.UTF8 };

                if (encode != null)
                {
                    webClient.Encoding = encode;
                }

                var sendData = Encoding.GetEncoding("UTF-8").GetBytes(postStr);
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("ContentLength", sendData.Length.ToString(CultureInfo.InvariantCulture));

                var readData = webClient.UploadData(url, "POST", sendData);

                result = Encoding.GetEncoding("UTF-8").GetString(readData);

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>  
        /// Http同步Post异步请求  
        /// </summary>  
        /// <param name="url">Url地址</param>  
        /// <param name="postStr">请求Url数据</param>  
        /// <param name="callBackUploadDataCompleted">回调事件</param>  
        /// <param name="encode"></param>  
        public static void HttpPostAsync(string url, string postStr = "",
            UploadDataCompletedEventHandler callBackUploadDataCompleted = null, Encoding encode = null)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };

            if (encode != null)
                webClient.Encoding = encode;

            var sendData = Encoding.GetEncoding("UTF-8").GetBytes(postStr);

            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.Headers.Add("ContentLength", sendData.Length.ToString(CultureInfo.InvariantCulture));

            if (callBackUploadDataCompleted != null)
                webClient.UploadDataCompleted += callBackUploadDataCompleted;

            webClient.UploadDataAsync(new Uri(url), "POST", sendData);
        }

        #endregion

        #region 加密与解密  AES
        public static string AesEncrypt(string str, string key)
        {
            //秘钥取md5
            //MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            byte[] t = UTF8Encoding.UTF8.GetBytes(key);
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            RijndaelManaged rm = new RijndaelManaged
            {
                Key = t,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            //MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            byte[] t = UTF8Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = Convert.FromBase64String(str);
            RijndaelManaged rm = new RijndaelManaged
            {
                Key = t,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        #region MD5加密
        public static string MD5EncryptFor32(string str)
        {
            var md5Helper = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Helper.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            foreach (var d in data)
            {
                sb.Append(d.ToString("x2"));
            }
            return sb.ToString();
        }


        public static string Decrypt(string Text, string sKey, string sIV)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            int num = Text.Length / 2;
            byte[] buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                int num3 = Convert.ToInt32(Text.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num3;
            }
            provider.Key = Encoding.ASCII.GetBytes(sKey);
            provider.IV = Encoding.ASCII.GetBytes(sIV);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            return Encoding.Default.GetString(stream.ToArray());
        }


        public static string MD5Encrypt(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }
        #endregion

    }



    public class SingleUser
    {
        public string account { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string mobileNo { get; set; }
        public string sex { get; set; }
        public string idType { get; set; }
        public string idNumber { get; set; }
        public string email { get; set; }
        public string isValid { get; set; }

    }

    public class RequestObj
    {
        public string version { get; set; }
        public string appCode { get; set; }
        public string data { get; set; }
    }




    /// <summary>
    ///  用户权限
    /// </summary>
    public class GetAccountObj
    {
        public string account { get; set; }
        public string appCode { get; set; }
        public string secretKey { get; set; }
        public string version { get; set; }
    }

    /// <summary>
    /// 注册对象
    /// </summary>
    public class RegisterObj
    {
        public string appCode { get; set; }
        public string data { get; set; }
        public string secretKey { get; set; }
        public string version { get; set; }
    }


    /// <summary>
    /// 登录对象
    /// </summary>
    public class LoginObj
    {
        public string account { get; set; }
        public string appCode { get; set; }
        public string password { get; set; }
        public string secretKey { get; set; }
        public string version { get; set; }
    }
}