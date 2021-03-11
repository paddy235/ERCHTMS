using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;


namespace ERCHTMS.Service.SystemManage
{
    public class MessageDetailService : RepositoryFactory<MessageDetail>, IMessageDetailService
    {
        public MessageDetail GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            DatabaseType dataType = DbHelper.DbType;
            if (!queryParam["MessageId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and t.messageid ='{0}'", queryParam["MessageId"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        public void SaveForm(string keyValue, MessageDetail ds)
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

        /// <summary>
        /// 通过账号和短消息Id获取实体
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="messageid"></param>
        /// <returns></returns>
        public MessageDetail GetEntity(string userAccount, string messageid)
        {
            return this.BaseRepository().FindEntity(x=>x.MessageId==messageid&&x.UserAccount==userAccount);
        }
    }
}
