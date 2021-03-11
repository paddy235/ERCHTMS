using BSFramework.Util.WebControl;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.SystemManage
{
    public class MessageDetailBLL
    {
        private IMessageDetailService service = new MessageDetailService();
        /// <summary>
        /// 获取短消息详情列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public void SaveForm(string keyValue, MessageDetail ds)
        {
            try
            {
                service.SaveForm(keyValue, ds);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public MessageDetail GetEntity(string keyValue) {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 通过账号和短消息Id获取实体
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="messageid"></param>
        /// <returns></returns>
        public MessageDetail GetEntity(string userAccount, string messageid)
        {
            return service.GetEntity(userAccount, messageid);
        }
    }
}
