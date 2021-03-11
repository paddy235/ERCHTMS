using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ERCHTMS.Code
{
    public class SocketHelper
    {
        /// <summary>
        /// 每次只发送一帧数据
        /// </summary>
        /// <param name="Msg"></param>
        public static void SendMsg(string Msg, string IP, int Port)
        {
            try
            {

                //int port = 1003;
                //string host = "10.36.1.68";//服务器端ip地址
                IPAddress ip = IPAddress.Parse(IP);
                IPEndPoint ipe = new IPEndPoint(ip, Port);
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(ipe);
                //send message
                byte[] sendBytes = Encoding.UTF8.GetBytes(Msg);
                clientSocket.Send(sendBytes);
                clientSocket.Close();
            }
            catch (Exception ex)
            {

            }

        }


        /// <summary>
        /// 调用海康接口方法
        /// </summary>
        /// <param name="model">接口需要传过去的实体数据</param>
        /// <param name="baseUrl">基础路径例如http://192.168.9.1:9016</param>
        /// <param name="apiUrl">接口地址/xx/xx/xx/xx</param>
        /// <param name="Key">海康平台APIKey</param>
        /// <param name="Signature">海康平台Key对应签名</param>
        /// <returns></returns>
        public static string LoadCameraList(object model, string baseUrl, string apiUrl, string Key, string Signature)
        {
            string message = "";
            try
            {
                SetLog("HikAPILog", "海康接口调用日志,调用接口:" + apiUrl, JsonConvert.SerializeObject(model));
                var client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                var md5 = MD5.Create();
                var content = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model))));

                var body = new StringContent(JsonConvert.SerializeObject(model));
                var str = new StringBuilder();
                str.Append("POST\n");
                str.Append(content + "\n");
                str.Append("application/json\n");
                str.Append(apiUrl);
                // var hmacsha256 = new HMACSHA256(Encoding.Default.GetBytes("LwGgus14ZkZDFk5ir90g"));
                var sign = HmacSHA256(str.ToString(), Signature);
                body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                body.Headers.Add("Content-MD5", content);
                body.Headers.Add("X-Ca-Key", Key);
                body.Headers.Add("X-Ca-Signature", sign);
                body.Headers.Add("C-Ca-Signature-Headers", "x-ca-key");
                var response = client.PostAsync(apiUrl, body);
                message = response.Result.Content.ReadAsStringAsync().Result;
                SetLog("HikAPILog", "海康接口调用日志,调用接口:" + apiUrl + "调用成功，返回", message);

            }
            catch (Exception ex)
            {
                SetLog("HikAPIErrorLog", "海康接口调用日志,调用接口:" + apiUrl + "调用报错，返回", ex.Message);
            }
            return message;
        }

        private static string HmacSHA256(string secret, string signkey)
        {
            var str = string.Empty;
            using (var mac = new HMACSHA256(Encoding.UTF8.GetBytes(signkey)))
            {
                var bash = mac.ComputeHash(Encoding.UTF8.GetBytes(secret));
                str = Convert.ToBase64String(bash);
            }
            return str;
        }


        /// <summary>
        /// 上传单个人脸至海康平台
        /// </summary>
        public static string UploadFace(List<FacedataEntity> FaceList, string baseUrl, string Key, string Signature, int type = 0)
        {
            string msg = string.Empty;
            try
            {
                string url = "/artemis/api/resource/v1/face/single/add";
                //循环调用人脸接口增加
                foreach (var insert in FaceList)
                {
                    var model = new
                    {
                        personId = insert.UserId,
                        faceData = insert.ImgData
                    };
                    if (type == 1)
                    {
                        HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseUrl, 443, true);
                        byte[] result = HttpUtillibKbs.HttpPost(url, JsonConvert.SerializeObject(model), 20);
                        msg = System.Text.Encoding.UTF8.GetString(result);
                    }
                    else
                    {
                        msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                    }
                }
                return msg;
            }
            catch (Exception e)
            {
                return msg = e.Message;
            }
        }

        /// <summary>
        /// 查询任务执行进度
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="baseUrl"></param>
        /// <param name="Key"></param>
        /// <param name="Signature"></param>
        /// <param name="IsJT">是否是京泰电厂</param>
        /// <returns></returns>
        public static string QuerySpeedofprogress(string taskId, string baseUrl, string Key, string Signature, int type = 0, bool IsJT = false)
        {
            string msg = string.Empty;
            try
            {

                var url = "/artemis/api/v1/authConfig/task/progress";
                if (IsJT) url = "/artemis/api/acps/v1/authDownload/task/progress";//京泰电厂任务进度查询接口
                var model = new
                {
                    taskId = taskId
                };
                if (type == 1)
                {
                    //1.4 版本 查询权限配置单进度
                    url = "/artemis/api/acps/v1/auth_config/rate/search";
                    HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseUrl, 443, true);
                    byte[] result = HttpUtillibKbs.HttpPost(url, JsonConvert.SerializeObject(model), 20);
                    msg = System.Text.Encoding.UTF8.GetString(result);
                }
                else
                {
                    msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                }
            }
            catch (Exception)
            {
                msg = string.Empty;
            }
            return msg;
        }

        /// <summary>
        /// 验证上传人脸照片是否合格
        /// </summary>
        /// <param name="facePicData">人脸图片base64格式</param>
        /// <param name="baseUrl"></param>
        /// <param name="Key"></param>
        /// <param name="Signature"></param>
        /// <returns></returns>
        public static string FaceImgIsQualified(string facePicData, string baseUrl, string Key, string Signature, int type = 0)
        {
            string msg = string.Empty;
            try
            {
                var url = "/artemis/api/frs/v1/face/picture/check";
                var model = new
                {
                    facePicBinaryData = facePicData
                };
                if (type == 1)
                {
                    HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseUrl, 443, true);
                    byte[] result = HttpUtillibKbs.HttpPost(url, JsonConvert.SerializeObject(model), 20);
                    msg = System.Text.Encoding.UTF8.GetString(result);
                }
                else
                {
                    msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                }
            }
            catch (Exception)
            {
                msg = string.Empty;
            }
            return msg;
        }


        #region 海康接口
        /// <summary>
        /// 调用海康接口方法(获取图片专用)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="body"></param>
        /// <param name="ip"></param>
        /// <param name="key"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string GetHeader(string baseUrl, string body, string ApiUrl, string key, string sign)
        {

            string str1 = "";

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string str2 = "*/*";
            dictionary.Add("Accept", str2);
            string str3 = "application/json";
            dictionary.Add("Content-Type", str3);
            //if (ex_headers.Count > 0)
            //{
            //    foreach (string key in ex_headers.Keys)
            //        dictionary.Add(key, string.IsNullOrWhiteSpace(ex_headers[key]) ? "" : ex_headers[key]);
            //}
            Request request = new Request(Method.POST_STRING, baseUrl, ApiUrl, key, sign, 20000);
            request.Headers = dictionary;
            request.SignHeaderPrefixList = (List<string>)null;
            request.Querys = null;
            request.StringBody = body;
            return doPoststring(request.Host, request.Path, request.Timeout, request.Headers, request.Querys, request.StringBody, request.SignHeaderPrefixList, request.AppKey, request.AppSecret, false);

            return str1;
        }

        public static string doPoststring(
      string host,
      string path,
      int connectTimeout,
      Dictionary<string, string> headers,
      Dictionary<string, string> querys,
      string body,
      List<string> signHeaderPrefixList,
      string appKey,
      string appSecret,
      bool autoDown)
        {

            headers = initialBasicHeader("POST", path, headers, querys, (Dictionary<string, string>)null, signHeaderPrefixList, appKey, appSecret);
            //https为下载图片所使用 加证书  这里不用 暂时注释
            //if (requestHeader == "https://")
            //{
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidate);
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //}
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(initUrl(host, path, querys));
            httpWebRequest.KeepAlive = false;
            httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            httpWebRequest.AllowAutoRedirect = false;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = connectTimeout;
            string header1 = headers["Accept"];
            httpWebRequest.Accept = header1;
            string header2 = headers["Content-Type"];
            httpWebRequest.ContentType = header2;
            foreach (string key in headers.Keys)
            {
                if (key.Contains("x-ca-"))
                    httpWebRequest.Headers.Add(key + ":" + (string.IsNullOrWhiteSpace(headers[key]) ? "" : headers[key]));
                if (key.Equals("tagId"))
                    httpWebRequest.Headers.Add(key + ":" + (string.IsNullOrWhiteSpace(headers[key]) ? "" : headers[key]));
            }
            if (!string.IsNullOrWhiteSpace(body))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(body);
                httpWebRequest.ContentLength = (long)bytes.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            string picUrl = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    picUrl = streamReader.ReadToEnd();
            }
            else if (response.StatusCode == HttpStatusCode.Found)
            {
                picUrl = response.Headers["Location"].ToString();
                response.Close();
                //if (autoDown)
                //    HttpGetPicByUrl(picUrl);
            }
            return picUrl;

        }

        //图片下载
        //public void HttpGetPicByUrl(string picUrl)
        //{
        //    try
        //    {
        //        WebClient webClient = new WebClient();
        //        string fileName = Application.StartupPath + "\\downloadpics\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
        //        webClient.DownloadFile(picUrl, fileName);
        //        this.log.Info((object)("图片下载成功: " + fileName));
        //    }
        //    catch (Exception ex)
        //    {
        //        int num = (int)MessageBox.Show("图片下载失败:" + ex.Message + "图片可能已被覆盖，请前往平台查看相关图片是否正常！");
        //    }
        //}

        public static string initUrl(string host, string path, Dictionary<string, string> querys)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append(host);
            if (!string.IsNullOrWhiteSpace(path))
                stringBuilder1.Append(path);
            if (querys != null)
            {
                StringBuilder stringBuilder2 = new StringBuilder();
                foreach (string key in querys.Keys)
                {
                    if (0 < stringBuilder2.Length)
                        stringBuilder2.Append("&");
                    if (string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(querys[key]))
                        stringBuilder2.Append(querys[key]);
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        stringBuilder2.Append(key);
                        if (!string.IsNullOrWhiteSpace(querys[key]))
                            stringBuilder2.Append("=").Append(HttpUtility.UrlEncode(querys[key], Encoding.UTF8));
                    }
                }
                if (0 < stringBuilder2.Length)
                    stringBuilder1.Append("?").Append((object)stringBuilder2);
            }
            return stringBuilder1.ToString();
        }

        private static bool RemoteCertificateValidate(
      object sender,
      X509Certificate cert,
      X509Chain chain,
      SslPolicyErrors error)
        {
            return true;
        }


        public static Dictionary<string, string> initialBasicHeader(
      string method,
      string path,
      Dictionary<string, string> headers,
      Dictionary<string, string> querys,
      Dictionary<string, string> bodys,
      List<string> signHeaderPrefixList,
      string appKey,
      string appSecret)
        {
            if (headers == null)
                headers = new Dictionary<string, string>();
            headers["x-ca-timestamp"] = GetTimestamp(DateTime.Now).ToString();
            headers["x-ca-nonce"] = Guid.NewGuid().ToString();
            headers["x-ca-key"] = appKey;
            headers["x-ca-signature"] = sign(appSecret, method, path, headers, querys, bodys, signHeaderPrefixList);
            return headers;
        }

        public static long GetTimestamp(DateTime time)
        {
            DateTime localTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (time.Ticks - localTime.Ticks) / 10000L;
        }

        public static string sign(
      string secret,
      string method,
      string path,
      Dictionary<string, string> headers,
      Dictionary<string, string> querys,
      Dictionary<string, string> bodys,
      List<string> signHeaderPrefixList)
        {

            string sign = buildstringToSign(method, path, headers, querys, bodys, signHeaderPrefixList);
            return HmacSHA256(sign, secret);

        }

        public static string buildstringToSign(
      string method,
      string path,
      Dictionary<string, string> headers,
      Dictionary<string, string> querys,
      Dictionary<string, string> bodys,
      List<string> signHeaderPrefixList)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(method.ToUpper()).Append("\n");
            if (headers != null)
            {
                if (headers["Accept"] != null)
                {
                    stringBuilder.Append(headers["Accept"]);
                    stringBuilder.Append("\n");
                }
                if (headers.Keys.Contains<string>("Content-MD5") && headers["Content-MD5"] != null)
                {
                    stringBuilder.Append(headers["Content-MD5"]);
                    stringBuilder.Append("\n");
                }
                if (headers["Content-Type"] != null)
                {
                    stringBuilder.Append(headers["Content-Type"]);
                    stringBuilder.Append("\n");
                }
                if (headers.Keys.Contains<string>("Date") && headers["Date"] != null)
                {
                    stringBuilder.Append(headers["Date"]);
                    stringBuilder.Append("\n");
                }
            }
            stringBuilder.Append(buildHeaders(headers, signHeaderPrefixList));
            stringBuilder.Append(buildResource(path, querys, bodys));
            return stringBuilder.ToString();
        }

        public static string buildResource(
      string path,
      Dictionary<string, string> querys,
      Dictionary<string, string> bodys)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(path))
                stringBuilder1.Append(path);
            Dictionary<string, string> source = new Dictionary<string, string>();
            if (querys != null)
            {
                foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>)querys.OrderBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>)(objDic => objDic.Key)))
                {
                    if (!string.IsNullOrWhiteSpace(keyValuePair.Key))
                        source[keyValuePair.Key] = keyValuePair.Value;
                }
            }
            if (bodys != null)
            {
                foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>)bodys.OrderBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>)(objDic => objDic.Key)))
                {
                    if (!string.IsNullOrWhiteSpace(keyValuePair.Key))
                        source[keyValuePair.Key] = keyValuePair.Value;
                }
            }
            StringBuilder stringBuilder2 = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>)source.OrderBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>)(objDic => objDic.Key)))
            {
                if (!string.IsNullOrWhiteSpace(keyValuePair.Key))
                {
                    if (stringBuilder2.Length > 0)
                        stringBuilder2.Append("&");
                    stringBuilder2.Append(keyValuePair.Key);
                    if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
                        stringBuilder2.Append("=").Append(keyValuePair.Value);
                }
            }
            if (0 < stringBuilder2.Length)
            {
                stringBuilder1.Append("?");
                stringBuilder1.Append((object)stringBuilder2);
            }
            return stringBuilder1.ToString();
        }


        public static string buildHeaders(
      Dictionary<string, string> headers,
      List<string> signHeaderPrefixList)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            if (signHeaderPrefixList != null)
            {
                signHeaderPrefixList.Remove("x-ca-signature");
                signHeaderPrefixList.Remove("Accept");
                signHeaderPrefixList.Remove("Content-MD5");
                signHeaderPrefixList.Remove("Content-Type");
                signHeaderPrefixList.Remove("Date");
                signHeaderPrefixList.Sort();
            }
            if (headers != null)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                IOrderedEnumerable<KeyValuePair<string, string>> orderedEnumerable = headers.OrderBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>)(objDic => objDic.Key));
                StringBuilder stringBuilder2 = new StringBuilder();
                foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>)orderedEnumerable)
                {
                    if (keyValuePair.Key.Replace(" ", "").Contains("x-ca-"))
                    {
                        stringBuilder1.Append(keyValuePair.Key + ":");
                        if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
                            stringBuilder1.Append(keyValuePair.Value);
                        stringBuilder1.Append("\n");
                        if (stringBuilder2.Length > 0)
                            stringBuilder2.Append(",");
                        stringBuilder2.Append(keyValuePair.Key);
                    }
                }
                headers.Add("x-ca-signature-headers", stringBuilder2.ToString());
            }
            return stringBuilder1.ToString();
        }

        /// <summary>
        /// HTTP Post请求
        /// </summary>
        /// <param name="uri">HTTP接口Url，不带协议和端口，如/artemis/api/resource/v1/org/advance/orgList</param>
        /// <param name="body">请求参数</param>
        /// <param name="timeout">请求超时时间，单位：秒</param>
        /// <return>请求结果</return>
        public static byte[] HikHttpPost(string uri, string body, int timeout)
        {
            Console.WriteLine("进入方法");
            Dictionary<string, string> header = new Dictionary<string, string>();

            // 初始化请求：组装请求头，设置远程证书自动验证通过
            initRequest(header, uri, body, true);

            // build web request object
            StringBuilder sb = new StringBuilder();
            Console.WriteLine(_ip);
            Console.WriteLine(_port);
            Console.WriteLine(uri);
            sb.Append(_isHttps ? "https://" : "http://").Append(_ip).Append(":").Append(_port.ToString()).Append(uri);

            // 创建POST请求
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(sb.ToString());
            req.KeepAlive = false;
            req.ProtocolVersion = HttpVersion.Version11;
            req.AllowAutoRedirect = false;   // 不允许自动重定向
            req.Method = "POST";
            req.Timeout = timeout * 1000;    // 传入是秒，需要转换成毫秒
            req.Accept = header["Accept"];
            req.ContentType = header["Content-Type"];

            foreach (string headerKey in header.Keys)
            {
                if (headerKey.Contains("x-ca-"))
                {
                    req.Headers.Add(headerKey + ":" + header[headerKey]);
                }
            }

            if (!string.IsNullOrWhiteSpace(body))
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(body);
                req.ContentLength = postBytes.Length;
                Stream reqStream = null;

                try
                {
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postBytes, 0, postBytes.Length);
                    reqStream.Close();
                }
                catch (WebException e)
                {
                    if (reqStream != null)
                    {
                        reqStream.Close();
                    }
                    Console.WriteLine("111");
                    return null;
                }
            }

            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
                if (HttpStatusCode.OK == rsp.StatusCode)
                {
                    Stream rspStream = rsp.GetResponseStream();
                    StreamReader sr = new StreamReader(rspStream);
                    string strStream = sr.ReadToEnd();
                    long streamLength = strStream.Length;
                    byte[] response = System.Text.Encoding.UTF8.GetBytes(strStream);
                    rsp.Close();
                    return response;
                }
                else if (HttpStatusCode.Found == rsp.StatusCode || HttpStatusCode.Moved == rsp.StatusCode)  // 302/301 redirect
                {
                    try
                    {
                        string reqUrl = rsp.Headers["Location"].ToString();    // 如需要重定向URL，请自行修改接口返回此参数
                        Console.WriteLine(reqUrl);
                        WebRequest wreq = WebRequest.Create(reqUrl);
                        rsp = (HttpWebResponse)wreq.GetResponse();
                        Stream rspStream = rsp.GetResponseStream();
                        long streamLength = rsp.ContentLength;
                        int offset = 0;
                        byte[] response = new byte[streamLength];
                        while (streamLength > 0)
                        {
                            int n = rspStream.Read(response, offset, (int)streamLength);
                            if (0 == n)
                            {
                                break;
                            }

                            offset += n;
                            streamLength -= n;
                        }

                        return response;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("222");
                        return null;
                    }
                }

                rsp.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                if (rsp != null)
                {
                    rsp.Close();
                }
                Console.WriteLine("333");
            }

            return null;
        }


        private static void initRequest(Dictionary<string, string> header, string url, string body, bool isPost)
        {
            // Accept                
            string accept = "application/json";// "*/*";
            header.Add("Accept", accept);

            // ContentType  
            string contentType = "application/json";
            header.Add("Content-Type", contentType);

            if (isPost)
            {
                // content-md5，be careful it must be lower case.
                string contentMd5 = computeContentMd5(body);
                header.Add("content-md5", contentMd5);
            }

            // x-ca-timestamp
            string timestamp = ((DateTime.Now.Ticks - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks) / 1000).ToString();
            header.Add("x-ca-timestamp", timestamp);

            // x-ca-nonce
            string nonce = System.Guid.NewGuid().ToString();
            header.Add("x-ca-nonce", nonce);

            // x-ca-key
            header.Add("x-ca-key", _appkey);

            // build string to sign
            string strToSign = buildSignString(isPost ? "POST" : "GET", url, header);
            string signedStr = computeForHMACSHA256(strToSign, _secret);

            // x-ca-signature
            header.Add("x-ca-signature", signedStr);

            if (_isHttps)
            {
                // set remote certificate Validation auto pass
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(remoteCertificateValidate);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
        }

        /// <summary>
        /// 计算content-md5
        /// </summary>
        /// <param name="body"></param>
        /// <returns>base64后的content-md5</returns>
        private static string computeContentMd5(string body)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(body));
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 远程证书验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns>验证是否通过，始终通过</returns>
        private static bool remoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        /// <summary>
        /// 计算HMACSHA265
        /// </summary>
        /// <param name="str">待计算字符串</param>
        /// <param name="secret">平台APPSecet</param>
        /// <returns>HMAXH265计算结果字符串</returns>
        private static string computeForHMACSHA256(string str, string secret)
        {
            var encoder = new System.Text.UTF8Encoding();
            byte[] secretBytes = encoder.GetBytes(secret);
            byte[] strBytes = encoder.GetBytes(str);
            var opertor = new HMACSHA256(secretBytes);
            byte[] hashbytes = opertor.ComputeHash(strBytes);
            return Convert.ToBase64String(hashbytes);
        }

        /// <summary>
        /// 计算签名字符串
        /// </summary>
        /// <param name="method">HTTP请求方法，如“POST”</param>
        /// <param name="url">接口Url，如/artemis/api/resource/v1/org/advance/orgList</param>
        /// <param name="header">请求头</param>
        /// <returns>签名字符串</returns>
        private static string buildSignString(string method, string url, Dictionary<string, string> header)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(method.ToUpper()).Append("\n");
            if (null != header)
            {
                if (null != header["Accept"])
                {
                    sb.Append((string)header["Accept"]);
                    sb.Append("\n");
                }

                if (header.Keys.Contains("Content-MD5") && null != header["Content-MD5"])
                {
                    sb.Append((string)header["Content-MD5"]);
                    sb.Append("\n");
                }

                if (null != header["Content-Type"])
                {
                    sb.Append((string)header["Content-Type"]);
                    sb.Append("\n");
                }

                if (header.Keys.Contains("Date") && null != header["Date"])
                {
                    sb.Append((string)header["Date"]);
                    sb.Append("\n");
                }
            }

            // build and add header to sign
            string signHeader = buildSignHeader(header);
            sb.Append(signHeader);
            sb.Append(url);
            return sb.ToString();
        }

        /// <summary>
        /// 计算签名头
        /// </summary>
        /// <param name="header">请求头</param>
        /// <returns>签名头</returns>
        private static string buildSignHeader(Dictionary<string, string> header)
        {
            Dictionary<string, string> sortedDicHeader = new Dictionary<string, string>();
            sortedDicHeader = header;
            var dic = from objDic in sortedDicHeader orderby objDic.Key ascending select objDic;

            StringBuilder sbSignHeader = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                if (kvp.Key.Replace(" ", "").Contains("x-ca-"))
                {
                    sb.Append(kvp.Key + ":");
                    if (!string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        sb.Append(kvp.Value);
                    }
                    sb.Append("\n");
                    if (sbSignHeader.Length > 0)
                    {
                        sbSignHeader.Append(",");
                    }
                    sbSignHeader.Append(kvp.Key);
                }
            }

            header.Add("x-ca-signature-headers", sbSignHeader.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// 设置信息参数
        /// </summary>
        /// <param name="appkey">合作方APPKey</param>
        /// <param name="secret">合作方APPSecret</param>
        /// <param name="ip">平台IP</param>
        /// <param name="port">平台端口，默认HTTPS的443端口</param>
        /// <param name="isHttps">是否启用HTTPS协议，默认HTTPS</param>
        /// <return></return>
        public static void SetPlatformInfo(string appkey, string secret, string ip, int port = 443, bool isHttps = true)
        {
            _appkey = appkey;
            _secret = secret;
            _ip = ip;
            _port = port;
            _isHttps = isHttps;
        }

        /// <summary>
        /// 平台ip
        /// </summary>
        private static string _ip;

        /// <summary>
        /// 平台端口
        /// </summary>
        private static int _port = 443;

        /// <summary>
        /// 平台APPKey
        /// </summary>
        private static string _appkey;

        /// <summary>
        /// 平台APPSecret
        /// </summary>
        private static string _secret;

        /// <summary>
        /// 是否使用HTTPS协议
        /// </summary>
        private static bool _isHttps = true;

        #endregion

        public static void SetLog(string filename, string msg, string log)
        {
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "logs";
            string ImgfileName = filename + DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!System.IO.Directory.Exists(logPath))
            {
                System.IO.Directory.CreateDirectory(logPath);
            }
            System.IO.File.AppendAllText(logPath + ImgfileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + msg + "，Json:" + log + "\r\n");
        }

        /// <summary>
        /// 将海康图片存到服务器上
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static void WriteImg(string imgPath, byte[] result, string filePath)
        {
            string fileurl = imgPath + filePath;
            string url = imgPath + "/Resource/HikInImg/";
            //如果目录不存在则创建
            if (!System.IO.Directory.Exists(url))
            {
                System.IO.Directory.CreateDirectory(url);
            }
            // 转换成json对象异常说明响应是字节流
            File.WriteAllBytes(fileurl, result);
        }

        /// <summary>
        /// 将海康图片存到服务器上
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string WriteImg(string imgPath, byte[] result)
        {
            string Img = "/Resource/HikInImg/" + Guid.NewGuid().ToString() + ".jpeg";
            string fileurl = imgPath + Img;
            string url = imgPath + "/Resource/HikInImg/";
            //如果目录不存在则创建
            if (!System.IO.Directory.Exists(url))
            {
                System.IO.Directory.CreateDirectory(url);
            }
            // 转换成json对象异常说明响应是字节流
            File.WriteAllBytes(fileurl, result);
            return Img;
        }



        /// <summary>
        /// 调用毕节接口方法
        /// </summary>
        /// <param name="model">接口需要传过去的实体数据</param>
        /// <param name="baseUrl">基础路径例如http://192.168.9.1:9016</param>
        /// <param name="apiUrl">接口地址/xx/xx/xx/xx</param>
        /// <param name="Key">毕节平台APIKey</param>
        /// <returns></returns>
        public static string LoadHdgzCameraList(object model, string baseUrl, string apiUrl, string Key)
        {
            string message = "";
            try
            {
                SetLog("HdgzAPILog", "贵州毕节接口调用日志,调用接口:" + apiUrl, JsonConvert.SerializeObject(model));
                var client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                var body = new StringContent(JsonConvert.SerializeObject(model));

                var response = client.PostAsync(apiUrl + "?key=" + Key, body);
                message = response.Result.Content.ReadAsStringAsync().Result;
                SetLog("HdgzAPILog", "贵州毕节接口调用日志,调用接口:" + apiUrl + "调用成功，返回", message);

            }
            catch (Exception ex)
            {
                SetLog("HdgzAPIErrorLog", "贵州毕节接口调用日志,调用接口:" + apiUrl + "调用报错，返回", ex.Message);
            }
            return message;
        }
    }

    /// <summary>
    /// 内蒙康巴什项目（安防1.4版）
    /// </summary>
    public class HttpUtillibKbs
    {
        /// <summary>
        /// 设置信息参数
        /// </summary>
        /// <param name="appkey">合作方APPKey</param>
        /// <param name="secret">合作方APPSecret</param>
        /// <param name="ip">平台IP</param>
        /// <param name="port">平台端口，默认HTTPS的443端口</param>
        /// <param name="isHttps">是否启用HTTPS协议，默认HTTPS</param>
        /// <return></return>
        public static void SetPlatformInfo(string appkey, string secret, string ip, int port = 443, bool isHttps = true)
        {
            _appkey = appkey;
            _secret = secret;
            _ip = ip;
            _port = port;
            _isHttps = isHttps;

            // 设置并发数，如不设置默认为2
            ServicePointManager.DefaultConnectionLimit = 512;
        }

        /// <summary>
        /// HTTP GET请求
        /// </summary>
        /// <param name="uri">HTTP接口Url，不带协议和端口，如/artemis/api/resource/v1/cameras/indexCode?cameraIndexCode=a10cafaa777c49a5af92c165c95970e0</param>
        /// <param name="timeout">请求超时时间，单位：秒</param>
        /// <returns></returns>
        public static byte[] HttpGet(string uri, int timeout)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();

            // 初始化请求：组装请求头，设置远程证书自动验证通过
            initRequest(header, uri, "", false);

            // build web request object
            StringBuilder sb = new StringBuilder();
            sb.Append(_isHttps ? "https://" : "http://").Append(_ip).Append(":").Append(_port.ToString()).Append(uri);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(sb.ToString());
            req.KeepAlive = false;
            req.ProtocolVersion = HttpVersion.Version11;
            req.AllowAutoRedirect = false;   // 不允许自动重定向
            req.Method = "GET";
            req.Timeout = timeout * 1000;    // 传入是秒，需要转换成毫秒
            req.Accept = header["Accept"];
            req.ContentType = header["Content-Type"];

            foreach (string headerKey in header.Keys)
            {
                if (headerKey.Contains("x-ca-"))
                {
                    req.Headers.Add(headerKey + ":" + header[headerKey]);
                }
            }

            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
                if (HttpStatusCode.OK == rsp.StatusCode)
                {
                    Stream rspStream = rsp.GetResponseStream();     // 响应内容字节流
                    StreamReader sr = new StreamReader(rspStream);
                    string strStream = sr.ReadToEnd();
                    long streamLength = strStream.Length;
                    byte[] response = System.Text.Encoding.UTF8.GetBytes(strStream);
                    rsp.Close();
                    return response;
                }
                else if (HttpStatusCode.Found == rsp.StatusCode || HttpStatusCode.Moved == rsp.StatusCode)  // 302/301 redirect
                {
                    string reqUrl = rsp.Headers["Location"].ToString();   // 获取重定向URL
                    WebRequest wreq = WebRequest.Create(reqUrl);          // 重定向请求对象
                    WebResponse wrsp = wreq.GetResponse();                // 重定向响应
                    long streamLength = wrsp.ContentLength;               // 重定向响应内容长度
                    Stream rspStream = wrsp.GetResponseStream();          // 响应内容字节流
                    byte[] response = new byte[streamLength];
                    rspStream.Read(response, 0, (int)streamLength);       // 读取响应内容至byte数组
                    rspStream.Close();
                    rsp.Close();
                    return response;
                }

                rsp.Close();
            }
            catch (WebException e)
            {
                if (rsp != null)
                {
                    rsp.Close();
                }
            }

            return null;
        }
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpThreeDPost(string url, string param = null)
        {
            HttpWebRequest request;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 150000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;
            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();
                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }
            return responseStr;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        /// <summary>
        /// HTTP Post请求
        /// </summary>
        /// <param name="uri">HTTP接口Url，不带协议和端口，如/artemis/api/resource/v1/org/advance/orgList</param>
        /// <param name="body">请求参数</param>
        /// <param name="timeout">请求超时时间，单位：秒</param>
        /// <return>请求结果</return>
        public static byte[] HttpPost(string uri, string body, int timeout)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            SetLog("HikAPILog", "海康接口调用日志,调用接口:" + uri, body);

            // 初始化请求：组装请求头，设置远程证书自动验证通过
            initRequest(header, uri, body, true);

            // build web request object
            StringBuilder sb = new StringBuilder();
            sb.Append(_isHttps ? "https://" : "http://").Append(_ip).Append(":").Append(_port.ToString()).Append(uri);

            // 创建POST请求
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(sb.ToString());
            req.KeepAlive = false;
            req.ProtocolVersion = HttpVersion.Version11;
            req.AllowAutoRedirect = false;   // 不允许自动重定向
            req.Method = "POST";
            req.Timeout = timeout * 1000;    // 传入是秒，需要转换成毫秒
            req.Accept = header["Accept"];
            req.ContentType = header["Content-Type"];

            foreach (string headerKey in header.Keys)
            {
                if (headerKey.Contains("x-ca-"))
                {
                    req.Headers.Add(headerKey + ":" + header[headerKey]);
                }
            }

            if (!string.IsNullOrWhiteSpace(body))
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(body);
                req.ContentLength = postBytes.Length;
                Stream reqStream = null;

                try
                {
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postBytes, 0, postBytes.Length);
                    reqStream.Close();
                }
                catch (WebException e)
                {
                    SetLog("HikAPIErrorLog", "海康接口调用日志,调用接口:" + uri + "调用报错，返回", e.Message);
                    if (reqStream != null)
                    {
                        reqStream.Close();
                    }

                    return null;
                }
            }

            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
                if (HttpStatusCode.OK == rsp.StatusCode)
                {
                    Stream rspStream = rsp.GetResponseStream();
                    StreamReader sr = new StreamReader(rspStream);
                    string strStream = sr.ReadToEnd();
                    long streamLength = strStream.Length;
                    byte[] response = System.Text.Encoding.UTF8.GetBytes(strStream);
                    rsp.Close();
                    return response;
                }
                else if (HttpStatusCode.Found == rsp.StatusCode || HttpStatusCode.Moved == rsp.StatusCode)  // 302/301 redirect
                {
                    try
                    {
                        string reqUrl = rsp.Headers["Location"].ToString();    // 如需要重定向URL，请自行修改接口返回此参数
                        rsp.Close();
                        WebRequest wreq = WebRequest.Create(reqUrl);
                        rsp = (HttpWebResponse)wreq.GetResponse();
                        Stream rspStream = rsp.GetResponseStream();
                        long streamLength = rsp.ContentLength;
                        int offset = 0;
                        byte[] response = new byte[streamLength];
                        while (streamLength > 0)
                        {
                            int n = rspStream.Read(response, offset, (int)streamLength);
                            if (0 == n)
                            {
                                break;
                            }

                            offset += n;
                            streamLength -= n;
                        }

                        return response;
                    }
                    catch (Exception e)
                    {
                        SetLog("HikAPIErrorLog", "海康接口调用日志,调用接口:" + uri + "调用报错，返回", e.Message);
                        rsp.Close();
                        return null;
                    }
                }

                rsp.Close();
            }
            catch (WebException e)
            {
                SetLog("HikAPIErrorLog", "海康接口调用日志,调用接口:" + uri + "调用报错，返回", e.Message);
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
            return null;
        }

        public static void SetLog(string filename, string msg, string log)
        {
            string ImgfileName = filename + DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
            }
            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + ImgfileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + msg + "，Json:" + log + "\r\n");
        }

        private static void initRequest(Dictionary<string, string> header, string url, string body, bool isPost)
        {
            // Accept                
            string accept = "application/json";// "*/*";
            header.Add("Accept", accept);

            // ContentType  
            string contentType = "application/json";
            header.Add("Content-Type", contentType);

            if (isPost)
            {
                // content-md5，be careful it must be lower case.
                string contentMd5 = computeContentMd5(body);
                header.Add("content-md5", contentMd5);
            }

            // x-ca-timestamp
            string timestamp = ((DateTime.Now.Ticks - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks) / 1000).ToString();
            header.Add("x-ca-timestamp", timestamp);

            // x-ca-nonce
            string nonce = System.Guid.NewGuid().ToString();
            header.Add("x-ca-nonce", nonce);

            // x-ca-key
            header.Add("x-ca-key", _appkey);

            // build string to sign
            string strToSign = buildSignString(isPost ? "POST" : "GET", url, header);
            string signedStr = computeForHMACSHA256(strToSign, _secret);

            // x-ca-signature
            header.Add("x-ca-signature", signedStr);

            if (_isHttps)
            {
                // set remote certificate Validation auto pass
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(remoteCertificateValidate);
                // FIX：修复不同.Net版对一些SecurityProtocolType枚举支持情况不一致导致编译失败等问题，这里统一使用数值
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)3072 | (SecurityProtocolType)768 | (SecurityProtocolType)192;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
        }

        /// <summary>
        /// 计算content-md5
        /// </summary>
        /// <param name="body"></param>
        /// <returns>base64后的content-md5</returns>
        private static string computeContentMd5(string body)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(body));
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 远程证书验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns>验证是否通过，始终通过</returns>
        private static bool remoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        /// <summary>
        /// 计算HMACSHA265
        /// </summary>
        /// <param name="str">待计算字符串</param>
        /// <param name="secret">平台APPSecet</param>
        /// <returns>HMAXH265计算结果字符串</returns>
        private static string computeForHMACSHA256(string str, string secret)
        {
            var encoder = new System.Text.UTF8Encoding();
            byte[] secretBytes = encoder.GetBytes(secret);
            byte[] strBytes = encoder.GetBytes(str);
            var opertor = new HMACSHA256(secretBytes);
            byte[] hashbytes = opertor.ComputeHash(strBytes);
            return Convert.ToBase64String(hashbytes);
        }

        /// <summary>
        /// 计算签名字符串
        /// </summary>
        /// <param name="method">HTTP请求方法，如“POST”</param>
        /// <param name="url">接口Url，如/artemis/api/resource/v1/org/advance/orgList</param>
        /// <param name="header">请求头</param>
        /// <returns>签名字符串</returns>
        private static string buildSignString(string method, string url, Dictionary<string, string> header)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(method.ToUpper()).Append("\n");
            if (null != header)
            {
                if (null != header["Accept"])
                {
                    sb.Append((string)header["Accept"]);
                    sb.Append("\n");
                }

                if (header.Keys.Contains("Content-MD5") && null != header["Content-MD5"])
                {
                    sb.Append((string)header["Content-MD5"]);
                    sb.Append("\n");
                }

                if (null != header["Content-Type"])
                {
                    sb.Append((string)header["Content-Type"]);
                    sb.Append("\n");
                }

                if (header.Keys.Contains("Date") && null != header["Date"])
                {
                    sb.Append((string)header["Date"]);
                    sb.Append("\n");
                }
            }

            // build and add header to sign
            string signHeader = buildSignHeader(header);
            sb.Append(signHeader);
            sb.Append(url);
            return sb.ToString();
        }

        /// <summary>
        /// 计算签名头
        /// </summary>
        /// <param name="header">请求头</param>
        /// <returns>签名头</returns>
        private static string buildSignHeader(Dictionary<string, string> header)
        {
            Dictionary<string, string> sortedDicHeader = new Dictionary<string, string>();
            sortedDicHeader = header;
            var dic = from objDic in sortedDicHeader orderby objDic.Key ascending select objDic;

            StringBuilder sbSignHeader = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                if (kvp.Key.Replace(" ", "").Contains("x-ca-"))
                {
                    sb.Append(kvp.Key + ":");
                    if (!string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        sb.Append(kvp.Value);
                    }
                    sb.Append("\n");
                    if (sbSignHeader.Length > 0)
                    {
                        sbSignHeader.Append(",");
                    }
                    sbSignHeader.Append(kvp.Key);
                }
            }

            header.Add("x-ca-signature-headers", sbSignHeader.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// 平台ip
        /// </summary>
        private static string _ip;

        /// <summary>
        /// 平台端口
        /// </summary>
        private static int _port = 443;

        /// <summary>
        /// 平台APPKey
        /// </summary>
        private static string _appkey;

        /// <summary>
        /// 平台APPSecret
        /// </summary>
        private static string _secret;

        /// <summary>
        /// 是否使用HTTPS协议
        /// </summary>
        private static bool _isHttps = true;
    }

    #region 海康辅助类
    public enum Method
    {
        GET,
        POST_FORM,
        POST_STRING,
        POST_BYTES,
        PUT_FORM,
        PUT_STRING,
        PUT_BYTES,
        DELETE,
    }
    public class Request
    {
        private Method method;
        private string host;
        private string path;
        private string appKey;
        private string appSecret;
        private int timeout;
        private Dictionary<string, string> headers;
        private Dictionary<string, string> querys;
        private Dictionary<string, string> bodys;
        private string stringBody;
        private byte[] bytesBody;
        private List<string> signHeaderPrefixList;

        public Request()
        {
        }

        public Request(
          Method method,
          string host,
          string path,
          string appKey,
          string appSecret,
          int timeout)
        {
            this.method = method;
            this.host = host;
            this.path = path;
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.timeout = timeout;
        }

        public Method Method
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }

        public string Host
        {
            get
            {
                return this.host;
            }
            set
            {
                this.host = value;
            }
        }

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }

        public string AppKey
        {
            get
            {
                return this.appKey;
            }
            set
            {
                this.appKey = value;
            }
        }

        public string AppSecret
        {
            get
            {
                return this.appSecret;
            }
            set
            {
                this.appSecret = value;
            }
        }

        public int Timeout
        {
            get
            {
                return this.timeout;
            }
            set
            {
                this.timeout = value;
            }
        }

        public Dictionary<string, string> Headers
        {
            get
            {
                return this.headers;
            }
            set
            {
                this.headers = value;
            }
        }

        public Dictionary<string, string> Querys
        {
            get
            {
                return this.querys;
            }
            set
            {
                this.querys = value;
            }
        }

        public Dictionary<string, string> Bodys
        {
            get
            {
                return this.bodys;
            }
            set
            {
                this.bodys = value;
            }
        }

        public string StringBody
        {
            get
            {
                return this.stringBody;
            }
            set
            {
                this.stringBody = value;
            }
        }

        public byte[] BytesBody
        {
            get
            {
                return this.bytesBody;
            }
            set
            {
                this.bytesBody = value;
            }
        }

        public List<string> SignHeaderPrefixList
        {
            get
            {
                return this.signHeaderPrefixList;
            }
            set
            {
                this.signHeaderPrefixList = value;
            }
        }
    }

    /// <summary>
    /// 上传人脸信息实体
    /// </summary>
    public class FacedataEntity
    {
        public string UserId { get; set; }
        public string ImgData { get; set; }
    }



    #endregion
}
