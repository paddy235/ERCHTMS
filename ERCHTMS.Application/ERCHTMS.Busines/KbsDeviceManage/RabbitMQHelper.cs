using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Busines.SystemManage;
using RabbitMQ.Client;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    public class RabbitMQHelper
    {
        private static object obj = new object();
        private static RabbitMQHelper _instance = null;
        private static ConnectionFactory factory;
        public static RabbitMQHelper CreateInstance()
        {
            if (_instance == null)
            {
                lock (obj)
                {
                    if (_instance == null)
                    {
                        _instance = new RabbitMQHelper();

                    }
                }
            }
            return _instance;
        }


        public RabbitMQHelper()
        {

            string MqConfig=new DataItemDetailBLL().GetItemValue("MQConfig", "async");//读取MQ配置
            string[] MqC=MqConfig.Split(',');


            string MQHost = MqC[0];// MQ主机地址
            string MQUser = MqC[1];// MQ用户
            string MQPwd = MqC[2];// MQ密码
            string MQVirHost = MqC[3];//MQ虚拟主机名称
            int port = 5672;

            factory = new ConnectionFactory();
            factory.HostName = MQHost;
            factory.Port = port;
            factory.UserName = MQUser;
            factory.Password = MQPwd;
            factory.VirtualHost = MQVirHost;
        }

        public void SendMessage(string json)
        {
            try
            {
                using (IConnection conn = factory.CreateConnection())
                {
                    using (IModel channel = conn.CreateModel())
                    {
                        //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                        channel.QueueDeclare("BosafeTestQueue", false, false, false, null);
                        string customStr = Console.ReadLine();
                        byte[] bytes = Encoding.UTF8.GetBytes(json);

                        //设置消息持久化
                        IBasicProperties properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2;
                        channel.BasicPublish("", "BosafeTestQueue", properties, bytes);
                        //channel.BasicPublish("", "MyFirstQueue", null, bytes);

                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
