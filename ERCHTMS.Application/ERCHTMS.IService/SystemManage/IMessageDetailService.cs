using BSFramework.Util.WebControl;
using ERCHTMS.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.SystemManage
{
    public interface IMessageDetailService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        MessageDetail GetEntity(string keyValue);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        void SaveForm(string keyValue, MessageDetail ds);
        /// <summary>
        /// 通过账号和短消息Id获取实体
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="messageid"></param>
        /// <returns></returns>
        MessageDetail GetEntity(string userAccount,string messageid);
    }
}
