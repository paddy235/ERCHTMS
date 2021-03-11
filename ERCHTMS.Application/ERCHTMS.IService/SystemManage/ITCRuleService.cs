using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.IService.SystemManage
{
    public interface ITCRuleService
    {
        List<TCRuleEntity> GetList(int infotype, string authId);
        void Update(string keyValue, TCRuleEntity entity);
        void Insert(TCRuleEntity entity);
        TCRuleEntity GetEntity(string keyValue);
        void Delete(string keyValue);
        List<TCRuleEntity> GetList(List<string> authIds);
    }
}
