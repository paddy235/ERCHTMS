using Framework.RabbitMq.Model;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IM.Hubs
{
    [HubName("ChatsHub")]
   public class PushHub:Hub
   {
        public override Task OnConnected()
        {
          
            return base.OnConnected();
        }
        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
        public void sendMsg(string userId,MessageModel msg)
        {
           // Clients.All.revMessage(userId, msg);
            string[] arr = msg.ReciverUser.Split(',');
            foreach (string uId in arr)
            {
                Clients.Group(uId).revMessage(userId, msg);
            }
        }
        public void loginMsg(string clientId, MessageModel msg)
        {
            Clients.Group(clientId).pushLogin(clientId, msg);
        }

        /// <summary>
        /// 可门电厂Gps即时数据发送到指定客户端（人员、车辆）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="msg"></param>
        public void sendMsgKm(string userId, string msg)
        {
            createGroup(userId);
            Clients.Group(userId).revMessage("", msg, Context.ConnectionId);
        }


        public void createGroup(string userId)
        {
            string clientId = Context.ConnectionId;
            Groups.Add(clientId, userId);
        }
        public void printMsg(string msg)
        {
            Console.WriteLine(string.Format("{0}:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg));
             
        }
   }
}
