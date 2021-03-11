using Framework.RabbitMq;
using Framework.RabbitMq.RabbitMqProxyConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.SIM
{
    partial class PushUserService : ServiceBase
    {
        public PushUserService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO:  在此处添加代码以启动服务。
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists("PushUserService"))
                {
                    System.Diagnostics.EventLog.CreateEventSource(
                        "PushUserService", "Application");
                }
                eventLog1.Source = "PushUserService";
                eventLog1.Log = "Application";
                string apiUrl = System.Configuration.ConfigurationSettings.AppSettings["ApiUri"];
                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.UseDefaultCredentials = true;
                //获取消息服务器连接配置信息
                string mqConfig = wc.DownloadString(apiUrl + "basedata/get?name=MQConfig");
                if (string.IsNullOrEmpty(mqConfig))
                {
                    eventLog1.WriteEntry("请配置RabbitMQ的连接信息！");
                }
                else
                {
                    try
                    {
                        string port = "5672";
                        string[] arr = mqConfig.Replace("\"", "").ToString().Split(',');
                        if (arr.Length > 4)
                        {
                            port = arr[4];
                        }
                        eventLog1.WriteEntry("RabbitMQ的连接信息：" + mqConfig);
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
                        eventLog1.WriteEntry("RabbitMQ已准备订阅信息……");
                        _rabbitMqProxy.Subscribe<object>("xldms.newUser", true, msg =>
                        {
                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                            NameValueCollection nc = new NameValueCollection();
                            nc.Add("json", msg.ToString());
                            //推送数据到webapi服务
                            wc.UploadValuesCompleted += wc_UploadValuesCompleted1;
                            wc.UploadValuesAsync(new Uri(apiUrl + "basedata/pushUserInfo"), nc);
                            //byte[] bytes = { };
                            //string result = "";
                            //if (bytes.Length > 0)
                            //{
                            //    result = System.Text.Encoding.UTF8.GetString(bytes);
                            //    dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(result);
                            //    if (dy.code == 0)
                            //    {
                            //        result = "推送并修改用户信息成功";
                            //    }
                            //    else
                            //    {
                            //        result = "推送并修改用户信息失败，错误信息：" + dy.message;
                            //    }
                            //}
                           //  eventLog1.WriteEntry(string.Format("{0}：{1}。推送信息>{2}", DateTime.Now.ToString(), result, json));
                            eventLog1.WriteEntry(string.Format("{0}：推送信息>{1}", DateTime.Now.ToString(), json));
                        }, false);
                    }
                    catch (Exception ex)
                    {
                        eventLog1.WriteEntry("从消息队列xldms.newUser中获取推送用户信息发生异常，异常信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                    }
                }
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
            }
        }
        void wc_UploadValuesCompleted1(object sender, UploadValuesCompletedEventArgs e)
        {
             
            //将同步结果写入日志文件
            string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                var error = e.Error;
                if (error == null)
                {
                    byte[] bytes = e.Result;
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
                    eventLog1.WriteEntry(string.Format("{0}：推送用户成功，返回结果：{1}", DateTime.Now.ToString(), result));
                }
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry(string.Format("同步用户发生错误：{0}", ex));
            }

        }

        protected override void OnStop()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
        }
    }
}
