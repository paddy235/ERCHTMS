using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.SystemManage
{
    public class MessageUserSetBLL
    {
        private IMessageUserSetService service = new MessageUserSetService();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MessageUserSetEntity GetEntity(string keyValue) {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取用户的消息设置
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public MessageUserSetEntity GetUserMessageSet(string userid)
        {
            return service.GetUserMessageSet(userid);
        }

        public void SaveUserMessageSet(string keyValue, MessageUserSetEntity entity) {
             service.SaveForm(keyValue, entity);
        }
    }
}
