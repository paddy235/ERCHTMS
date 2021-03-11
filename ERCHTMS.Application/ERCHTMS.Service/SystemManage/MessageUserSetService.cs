using BSFramework.Data.Repository;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.SystemManage
{
    public class MessageUserSetService : RepositoryFactory<MessageUserSetEntity>, IMessageUserSetService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MessageUserSetEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取用户的消息设置
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public MessageUserSetEntity GetUserMessageSet(string userid)
        {
            return this.BaseRepository().FindEntity(x=>x.CreateUserId==userid);
        }

        public void SaveForm(string keyValue, MessageUserSetEntity ds)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                ds.Modify(keyValue);
                this.BaseRepository().Update(ds);
            }
            else
            {
                ds.Create();
                this.BaseRepository().Insert(ds);
            }

        }
    }
}
