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
   public class TCRuleBLL
    {
        private ITCRuleService service = new TCRuleService();

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="infotype">
        /// 数据类型 ： 1、主题 2、文化墙地址 3：首页地址
        /// </param>
        /// <param name="authId">授权表（BASE_MENUAUTHORIZE）的主键</param>
        /// <returns></returns>
        public List<TCRuleEntity> GetList(int infotype, string authId)
        {
            return service.GetList(infotype, authId);
        }

        public void Update(string keyValue, TCRuleEntity entity)
        {
            service.Update(keyValue, entity);
        }

        public void Insert(TCRuleEntity entity)
        {
            service.Insert( entity);
        }

        public TCRuleEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public void Delete(string keyValue)
        {
            service.Delete(keyValue);
        }

        public List<TCRuleEntity> GetList(List<string> authIds)
        {
            return service.GetList(authIds);
        }
    }
}
