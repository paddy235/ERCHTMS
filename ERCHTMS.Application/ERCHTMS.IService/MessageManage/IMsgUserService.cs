using ERCHTMS.Entity.MessageManage;
using System.Collections.Generic;

namespace ERCHTMS.IService.MessageManage
{
    /// <summary>
    /// 描 述：即时通信用户管理
    /// </summary>
    public interface IMsgUserService
    {
        /// <summary>
        /// 获取联系人列表（即时通信）
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMUserModel> GetList(string OrganizeId);
    }
}
