using ERCHTMS.IM;
using Framework.RabbitMq;
using Framework.RabbitMq.Model;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using FrameWork.Extension;
using Framework.RabbitMq.RabbitMqProxyConfig;
using Microsoft.Owin.Hosting;
using System.Reflection;
using Owin;
using Microsoft.Owin;
using System.Net;
namespace ERCHTMS.SIM
{
    
    partial class PushService : ServiceBase
    {
        
        public PushService()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            // TODO:  在此处添加代码以启动服务。
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists("SignalRChat"))
                {
                    System.Diagnostics.EventLog.CreateEventSource(
                        "SignalRChat", "Application");
                }
                eventLog1.Source = "SignalRChat";
                eventLog1.Log = "Application";
                string apiUrl = ConfigurationManager.AppSettings["ApiUri"];
                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.UseDefaultCredentials = true;
                string url = wc.DownloadString(apiUrl + "basedata/get?name=SignalRUrl");
                if(string.IsNullOrEmpty(url))
                {
                    eventLog1.WriteEntry("SignalR服务地址未配置！");
                }
                else
                {
                    url = url.Replace("signalr","").Replace("\"","");
                    WebApp.Start(url);
                    eventLog1.WriteEntry("SignalR服务启动");
                }
                //string url = ConfigurationManager.AppSettings["SignalRUrl"].ToString();
              

                //string mqConfig = ConfigurationManager.AppSettings["MQConfig"];
                string mqConfig = wc.DownloadString(apiUrl + "basedata/get?name=MQConfig");
                if (string.IsNullOrEmpty(mqConfig))
                {
                    eventLog1.WriteEntry("请配置RabbitMQ的连接信息！");
                }
                else
                {
                    string port = "5672";
                    string[] arr = mqConfig.Replace("\"", "").ToString().Split(',');
                    if (arr.Length > 4)
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
                                Port = int.Parse(port)
                            });

                            HubConnection hubConnection = null;
                            IHubProxy ChatsHub = null;
                            try
                            {
                                hubConnection = new HubConnection(url);
                                ChatsHub = hubConnection.CreateHubProxy("ChatsHub");
                                eventLog1.WriteEntry("创建SignalR代理成功");
                            }
                            catch (Exception ex)
                            {
                                eventLog1.WriteEntry("创建SingnalR代理出错,异常信息：" + ex.Message);
                            }
                            eventLog1.WriteEntry("RabbitMQ已准备订阅信息……");

                            _rabbitMqProxy.Subscribe<MessageModel>(msg =>
                            {
                                var json = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                                hubConnection.Start().ContinueWith(task =>
                                {
                                    if (!task.IsFaulted)
                                    //连接成功调用服务端方法
                                    {
                                        ChatsHub.Invoke("sendMsg", msg.ReciverUser, msg);
                                        ChatsHub.Invoke("loginMsg", msg.ReciverUser, msg);
                                        //结束连接
                                        //hubConnection.Stop();
                                    }
                                });
                                eventLog1.WriteEntry(json);
                            });
                        }
                        catch (Exception ex)
                        {
                            eventLog1.WriteEntry(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
            }
        }
        private void WriteLog(string msg)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "logs\\" + fileName, string.Format("{0}：{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg));
        }
        protected override void OnStop()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
           
        }
    }
}
