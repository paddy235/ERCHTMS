using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cn.jpush.api.common;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using cn.jpush.api.common.resp;
using System.Configuration;
using cn.jpush.api;
using cn.jpush.api.schedule;
using System.IO;
using System.Data;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.SystemManage;
using Framework.RabbitMq;
using Framework.RabbitMq.RabbitMqProxyConfig;
using Framework.RabbitMq.Model;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.JPush
{
    /// <summary>
    /// 极光推送消息API 
    /// </summary>
    public class JPushApi
    {

        /// <summary>
        /// 发布消息到队列
        /// </summary>
        /// <param name="obj">消息对象</param>
        /// <param name="mode">对象类型：0>部门，1>用户</param>
        public static void PushMessage(object obj, int mode, string queuePrefix = "")
        {
            string name = "";
            if (string.IsNullOrWhiteSpace(queuePrefix))
            {
                queuePrefix = "xldms";
            }
            if (mode == 0)
            {
                name = string.Format("{0}.dept", queuePrefix);
            }
            if (mode == 1)
            {
                name = string.Format("{0}.user", queuePrefix);
            }
            string MQHost = new DataItemDetailBLL().GetItemValue("host", "MQConfig");// MQ主机地址
            string MQUser = new DataItemDetailBLL().GetItemValue("user", "MQConfig");// MQ用户
            string MQPwd = new DataItemDetailBLL().GetItemValue("pwd", "MQConfig");// MQ密码
            string isPush = new DataItemDetailBLL().GetItemValue("ispush", "MQConfig");//是否推送
            string MQVirHost = new DataItemDetailBLL().GetItemValue("virhost", "MQConfig");//MQ虚拟主机名称
            string port = new DataItemDetailBLL().GetItemValue("port", "MQConfig");//端口号
            port = string.IsNullOrWhiteSpace(port) ? "5672" : port;
            if (!string.IsNullOrWhiteSpace(MQHost))
            {
                var rabbitMqProxy = new RabbitMqService(new MqConfig
                {
                    AutomaticRecoveryEnabled = true,
                    HeartBeat = 60,
                    NetworkRecoveryInterval = new TimeSpan(60),
                    Host = MQHost,
                    UserName = MQUser,
                    Password = MQPwd,
                    VirtualHost = MQVirHost,
                    Port=int.Parse(port)
                });
                rabbitMqProxy.Publish(name, name, name, Newtonsoft.Json.JsonConvert.SerializeObject(obj), true);
            }

        }
        /// <summary>
        /// 发布消息到RabbitMQ
        /// </summary>
        /// <param name="msg"></param>
        public static void PublicMessage(MessageEntity msg)
        {
            var ts = Task.Factory.StartNew(() => {
                string MQHost = new DataItemDetailBLL().GetItemValue("host", "MQConfig");// MQ主机地址
                string MQUser = new DataItemDetailBLL().GetItemValue("user", "MQConfig");// MQ用户
                string MQPwd = new DataItemDetailBLL().GetItemValue("pwd", "MQConfig");// MQ密码
                string isPush = new DataItemDetailBLL().GetItemValue("ispush", "MQConfig");//是否推送
                string MQVirHost = new DataItemDetailBLL().GetItemValue("virhost", "MQConfig");//MQ虚拟主机名称
                string port = new DataItemDetailBLL().GetItemValue("port", "MQConfig");//端口号
                port = string.IsNullOrWhiteSpace(port) ? "5672" : port;
                if (isPush == "1")
                {
                    if (!string.IsNullOrWhiteSpace(MQHost))
                    {
                        var rabbitMqProxy = new RabbitMqService(new MqConfig
                        {
                            AutomaticRecoveryEnabled = true,
                            HeartBeat = 60,
                            NetworkRecoveryInterval = new TimeSpan(60),
                            Host = MQHost,
                            UserName = MQUser,
                            Password = MQPwd,
                            VirtualHost = MQVirHost,
                            Port = int.Parse(port)
                        });
                        var data = new MessageModel
                        {
                            Content=msg.Content,
                            Category=msg.Category,
                            ReciverUser = msg.UserId,
                            SendTime = DateTime.Now,
                            Title = msg.Title, 
                            SendUser = msg.SendUserName,
                            Remark=msg.Remark
                        };
                        rabbitMqProxy.Publish(data);
                    }
                }
            });
           
        }

        /// <summary>
        /// 发布消息到RabbitMQ
        /// </summary>
        /// <param name="msg"></param>
        public static void PublishMessage(MessageModel msg)
        {
            var ts = Task.Factory.StartNew(() =>
            {
                string MQHost = new DataItemDetailBLL().GetItemValue("host", "MQConfig");// MQ主机地址
                string MQUser = new DataItemDetailBLL().GetItemValue("user", "MQConfig");// MQ用户
                string MQPwd = new DataItemDetailBLL().GetItemValue("pwd", "MQConfig");// MQ密码
                string isPush = new DataItemDetailBLL().GetItemValue("ispush", "MQConfig");//是否推送
                string MQVirHost = new DataItemDetailBLL().GetItemValue("virhost", "MQConfig");//MQ虚拟主机名称
                string port = new DataItemDetailBLL().GetItemValue("port", "MQConfig");//端口号
                port = string.IsNullOrWhiteSpace(port) ? "5672" : port;
                if (isPush == "1")
                {
                    if (!string.IsNullOrWhiteSpace(MQHost))
                    {
                        var rabbitMqProxy = new RabbitMqService(new MqConfig
                        {
                            AutomaticRecoveryEnabled = true,
                            HeartBeat = 60,
                            NetworkRecoveryInterval = new TimeSpan(60),
                            Host = MQHost,
                            UserName = MQUser,
                            Password = MQPwd,
                            VirtualHost = MQVirHost,
                            Port = int.Parse(port)
                        });
                        rabbitMqProxy.Publish(msg);
                    }
                }
            });

        }
        /// <summary>
        /// 发送及推送短消息
        /// </summary>
        /// <param name="accounts">接受人账号(多个用英文逗号分隔)</param>
        /// <param name="unames">接受人姓名(多个用英文逗号分隔)</param>
        /// <param name="code">项目编码(对应消息设置中的编码)</param>
        ///  <param name="code">项目编码(对应消息设置中的编码)</param>
        /// <returns></returns>
        public static bool PushMessage(string accounts, string unames, string code, string recId = "")
        {
            try
            {
                var senduser = new UserBLL().GetUserInfoByAccount("System");
                MessageSetEntity ms = new MessageSetBLL().GetEntityByCode(code);
                if (ms != null)
                {
                    if (ms.IsPush==1)
                    {
                        var ts = Task.Factory.StartNew(() =>
                        {
                            SendRequest(accounts.Split(','), ms.Title, ms.Content, code, recId);
                        });
                       
                    }
                    MessageEntity msg = new MessageEntity
                    {
                        Title = ms.Title,
                        Category=ms.Kind,
                        Content = ms.Content,
                        UserId = accounts,
                        UserName = unames,
                        Status = "",
                        Url = string.IsNullOrEmpty(ms.Url) ? "" : ms.Url.Replace("{Id}", recId),
                        SendUser = senduser.Account,
                        SendUserName = senduser.RealName,
                        RecId=recId
                     };
                     if (new MessageBLL().SaveForm("", msg))
                     {
                         PublicMessage(msg);
                     }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 康巴什发送短消息
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="title"></param>
        /// <param name="Category"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static bool KbsPushMeassge(string accounts, string name,  string title, string Category, string Content)
        {
            try
            {
                MessageEntity msg = new MessageEntity
                {
                    Title = title,
                    Category = Category,
                    Content = Content,
                    UserId = accounts,
                    UserName = name,
                    Status = "",
                    Url = "",
                    SendUser = "system",
                    SendUserName = "系统管理员",
                    RecId = ""
                };
                var flag = new MessageBLL().SaveForm("", msg);
                return flag;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        /// <summary>
        /// 发送及推送短消息
        /// </summary>
        /// <param name="accounts">接受人账号(多个用英文逗号分隔)</param>
        /// <param name="unames">接受人姓名(多个用英文逗号分隔)</param>
        /// <param name="code">项目编码(对应消息设置中的编码)</param>
        /// <param name="category">消息类别</param>
        /// <param name="recId">关联业务记录Id(如果是列表可不传)</param>
        /// <returns></returns>
        public static bool PushMessage(string accounts, string unames, string code, string category, string recId = "")
        {
            try
            {
                var senduser = new UserBLL().GetUserInfoByAccount("System");
                MessageSetEntity ms = new MessageSetBLL().GetEntityByCode(code);
                if (ms != null)
                {
                    if (ms.IsPush == 1)
                    {
                        var ts = Task.Factory.StartNew(() =>
                        {
                            SendRequest(accounts.Split(','), ms.Title, ms.Content, code, recId);
                        });
                    }
                    MessageEntity msg = new MessageEntity
                    {
                        Title = ms.Title,
                        Content = ms.Content,
                        UserId = accounts,
                        UserName = unames,
                        Status = "",
                        Url = string.IsNullOrEmpty(ms.Url) ? "" : ms.Url.Replace("{Id}", recId),
                        SendUser = senduser.Account,
                        Category = category,
                        SendUserName = senduser.RealName,
                        RecId=recId
                     };
                     if (new MessageBLL().SaveForm("", msg))
                     {
                          PublicMessage(msg);
                     }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 发送及推送短消息
        /// </summary>
        /// <param name="accounts">接受人账号(多个用英文逗号分隔)</param>
        /// <param name="unames">接受人姓名(多个用英文逗号分隔)</param>
        /// <param name="code">项目编码(对应消息设置中的编码)</param>
        ///  <param name="code">项目编码(对应消息设置中的编码)</param>
        /// <returns></returns>
        public static bool PushMessage(string accounts, string unames, string code, string title, string content, string recId = "")
        {
            try
            {
                var senduser = new UserBLL().GetUserInfoByAccount("System");
                MessageSetEntity ms = new MessageSetBLL().GetEntityByCode(code);
                if (ms != null)
                {
                    if (ms.IsPush == 1)
                    {
                        var ts = Task.Factory.StartNew(() =>
                        {
                            SendRequest(accounts.Split(','), !string.IsNullOrEmpty(title) ? title : ms.Title, !string.IsNullOrEmpty(content) ? content : ms.Content, code, recId);
                        });
                    }
                        MessageEntity msg = new MessageEntity
                        {
                            Title = !string.IsNullOrEmpty(title) ? title : ms.Title,
                            Content = !string.IsNullOrEmpty(content) ? content : ms.Content,
                            UserId = accounts,
                            UserName = unames,
                            Status = "",
                            Url = string.IsNullOrEmpty(ms.Url) ? "" : ms.Url.Replace("{Id}", recId),
                            SendUser = senduser.Account,
                            SendUserName = senduser.RealName,
                            Category=ms.Kind,
                            RecId=recId,
                            Remark = ms.Remark
                        };
                        if (new MessageBLL().SaveForm("", msg))
                        {
                            PublicMessage(msg);
                        }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 根据用户帐号推送极光消息
        /// </summary>
        /// <param name="alias">需消息提醒的用户帐号</param>
        /// <param name="recId">消息编号</param>
        /// <param name="msgType">消息类型</param>
        /// <param name="alert">提示信息</param>
        /// <returns></returns>
        public static bool SendRequest(string[] alias, string alert, string title, string key, string value)
        {
            bool r = false;
            string IsUseJPush = new DataItemDetailBLL().GetItemValue("IsUseJPush");
            if (IsUseJPush == "1")
            {
                string AppKey = new DataItemDetailBLL().GetItemValue("JAppKey");
                string MasterSecret = new DataItemDetailBLL().GetItemValue("JMasterSecret");
                JPushClient client = new JPushClient(AppKey, MasterSecret);

                PushPayload payload = PushObject_All_All_Alert(alias, alert, title, key, value);
                try
                {
                    var result = client.SendPush(payload);
                    r = true;
                }
                catch (APIRequestException e)
                {
                    string str = "Date: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n";
                    str += "===============================================\r\n";
                    str += "推送对象：[" + string.Join("|", alias) + "]。\r\n";
                    str += "推送消息：" + alert + "\r\n";
                    str += "HTTP Status：" + e.Status + "\r\n";
                    str += "Error Code：" + e.ErrorCode + "\r\n";
                    str += "Error Message：" + e.Message + "\r\n";
                    string logAddr = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpFile", "ErrLog", "极光推送" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                    WriteLog(logAddr, str);
                    r = false;
                }
                catch (APIConnectionException e)
                {
                    string str = "Date: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n";
                    str += "===============================================\r\n";
                    str += "推送对象：[" + string.Join("|", alias) + "]。\r\n";
                    str += "推送消息：" + alert + "\r\n";
                    str += "Error Message：" + e.Message + "\r\n";
                    string logAddr = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpFile", "ErrLog", "极光推送" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                    WriteLog(logAddr, str);
                    r = false;
                }
            }

            return r;
        }

        private static PushPayload PushObject_All_All_Alert(string[] alias, String alert, string title, string moduleKey, string moduleValue)
        {
            string DemoKey = new DataItemDetailBLL().GetItemValue("JDemoKey");
            if (!string.IsNullOrWhiteSpace(DemoKey))
            {
                var newAlias = new List<string>();
                Array.ForEach(alias, item => { newAlias.Add(DemoKey + item); });
                alias = newAlias.ToArray();
            }
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();
            pushPayload.audience = Audience.s_alias(alias);
            //var tag = new DataItemDetailBLL().GetItemValue("AppTag");
            //pushPayload.audience = Audience.s_tag(new string[] { tag });
            var notification = new Notification();

            string ModuleKey = "ModuleKey";
            string ModuleValue = string.Format("{0}|{1}", moduleKey, moduleValue);

            if (!string.IsNullOrEmpty(ModuleKey) && !string.IsNullOrEmpty(ModuleValue))
            {
                notification.AndroidNotification = new AndroidNotification().setAlert(alert).AddExtra(ModuleKey, ModuleValue).setTitle(title);
                notification.IosNotification = new IosNotification().setAlert(alert).incrBadge(1).setSound("happy").AddExtra(ModuleKey, ModuleValue);
            }
            else
            {
                notification.AndroidNotification = new AndroidNotification().setAlert(alert).setTitle(title);
                notification.IosNotification = new IosNotification().setAlert(alert).incrBadge(1).setSound("happy");
            }
            pushPayload.notification = notification;
            pushPayload.platform.setAll(true);
            string apns_production = "";
            pushPayload.options.apns_production = (!string.IsNullOrEmpty(apns_production) && apns_production.ToLower() == "true") ? true : false;
            return pushPayload;
        }
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static void WriteLog(string fileName, string content)
        {
            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(content))
            {
                object obj = new object();
                lock (obj)
                {
                    string path = Path.GetDirectoryName(fileName);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Write))
                    {
                        byte[] bytData = Encoding.UTF8.GetBytes(content);
                        fs.Write(bytData, 0, bytData.Length);
                        fs.Flush();
                        fs.Dispose();
                    }
                }
            }
        }
    }
}
