using Framework.RabbitMq;
using Framework.RabbitMq.Model;
using Framework.RabbitMq.RabbitMqProxyConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using FrameWork.Extension;
using System.Threading;
using Microsoft.AspNet.SignalR.Client;
using System.Configuration;
using System.ServiceProcess;
using ERCHTMS.SIM;
using Microsoft.Owin.Hosting;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Dynamic;
namespace ERCHTMS.IM
{
    class Program
    {
        static void Main(string[] args)
        {
            //start();
            ServiceBase[] ServicesToRun = new ServiceBase[] 
            { 
                new PushService(),
                new PushUserService()
                //new GJXSyncService()
               
            };
            ServiceBase.Run(ServicesToRun);
        }
        static void start()
        {
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists("SignalRChat"))
                {
                    System.Diagnostics.EventLog.CreateEventSource(
                        "SignalRChat", "Application");
                }
                //eventLog1.Source = "SignalRChat";
                //eventLog1.Log = "Application";
                string apiUrl = ConfigurationManager.AppSettings["ApiUri"];
                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.UseDefaultCredentials = true;
                string url = wc.DownloadString(apiUrl + "basedata/get?name=SignalRUrl");
                if (string.IsNullOrEmpty(url))
                {
                    Console.WriteLine("SignalR服务地址未配置！");
                }
                else
                {
                    //url = url.Replace("signalr", "").Replace("\"", "");
                    //WebApp.Start(url);
                    Console.WriteLine("SignalR服务启动");
                }
                //string url = ConfigurationManager.AppSettings["SignalRUrl"].ToString();


                //string mqConfig = ConfigurationManager.AppSettings["MQConfig"];
                string mqConfig = wc.DownloadString(apiUrl + "basedata/get?name=MQConfig");
                if (string.IsNullOrEmpty(mqConfig))
                {
                    //eventLog1.WriteEntry("请配置RabbitMQ的连接信息！");
                }
                else
                {
                    string port = "5672";
                    string[] arr = mqConfig.Replace("\"", "").ToString().Split(',');
                    if (arr.Length>4)
                    {
                        port = arr[4];
                    }
                    else
                    {
                        try
                        {
                            RabbitMqService _rabbitMqProxy = new RabbitMqService(new MqConfig
                            {
                                AutomaticRecoveryEnabled = true,
                                HeartBeat = 60,
                                NetworkRecoveryInterval = new TimeSpan(60),
                                Host = arr[0],
                                UserName = arr[1],
                                Password = arr[2],
                                VirtualHost = arr[3],
                                Port=int.Parse(port)
                            });

                            HubConnection hubConnection = null;
                            IHubProxy ChatsHub = null;
                            try
                            {
                               // hubConnection = new HubConnection(url);
                                //ChatsHub = hubConnection.CreateHubProxy("ChatsHub");
                                Console.WriteLine("创建SignalR代理成功");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("创建SingnalR代理出错,异常信息：" + ex.Message);
                            }
                            Console.WriteLine("RabbitMQ已准备订阅信息……");

                            _rabbitMqProxy.Subscribe<object>("xldms.newUser", true, msg =>
                            {
                                var json = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                                NameValueCollection nc = new NameValueCollection();
                                nc.Add("json", msg.ToString());
                                //推送数据到webapi服务
                                byte[] bytes = wc.UploadValues(apiUrl + "basedata/pushUserInfo", "post", nc);
                                string result = "";
                                if (bytes.Length > 0)
                                {
                                    result = System.Text.Encoding.UTF8.GetString(bytes);
                                    dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(result);
                                    if (dy.code == 0)
                                    {
                                        result = "推送并修改用户信息成功";
                                    }
                                    else
                                    {
                                        result = "推送并修改用户信息失败，错误信息：" + dy.message;
                                    }
                                }
                                Console.WriteLine(result);
                            }, false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //eventLog1.WriteEntry(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
            }
        }
    }
}
