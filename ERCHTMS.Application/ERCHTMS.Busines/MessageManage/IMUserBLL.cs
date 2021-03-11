using ERCHTMS.Entity.MessageManage;
using ERCHTMS.IService.MessageManage;
using ERCHTMS.Service.MessageManage;
using System.Collections.Generic;

namespace ERCHTMS.Busines.MessageManage
{
    /// <summary>
    /// 描 述：用户管理
    /// </summary>
    public class IMUserBLL
    {
        private IMsgUserService service = new IMUserService();
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IMUserModel> GetList(string OrganizeId)
        {
            return service.GetList(OrganizeId);
        }
    }
}
