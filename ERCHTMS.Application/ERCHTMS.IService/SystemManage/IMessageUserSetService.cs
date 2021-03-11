using ERCHTMS.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.SystemManage
{
    public interface IMessageUserSetService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        MessageUserSetEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取用户的消息设置
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        MessageUserSetEntity GetUserMessageSet(string userid);
        void SaveForm(string keyValue, MessageUserSetEntity ds);
    }
}
