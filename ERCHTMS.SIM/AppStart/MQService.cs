using System;
using Framework.RabbitMq.Model;
using Framework.RabbitMq.RabbitMqProxyConfig;
using FrameWork.Extension;
using Framework.RabbitMq;

namespace ERCHTMS.IM
{
    public class MQService
    {
        private readonly RabbitMqService _rabbitMqProxy;
        public MQService()
        {
            _rabbitMqProxy = new RabbitMqService(new MqConfig
            {
                AutomaticRecoveryEnabled = true,
                HeartBeat = 60,
                NetworkRecoveryInterval = new TimeSpan(60),
                Host = "localhost",
                UserName = "test",
                Password = "123456",
                VirtualHost="test"
            });
        }

        public bool Start()
        {
            _rabbitMqProxy.Subscribe<MessageModel>(msg =>
            {
                var json = msg.ToJson();
                Console.WriteLine(json);
            });

            return true;
        }

        public bool Stop()
        {
            _rabbitMqProxy.Dispose();
            return true;
        }
    }
}
