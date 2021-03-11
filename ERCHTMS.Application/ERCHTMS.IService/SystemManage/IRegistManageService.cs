using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.IService.SystemManage
{
    public interface IRegistManageService
    {
        List<RegistManageEntity> GetList(string keyword);
        void SaveForm(string keyValue, RegistManageEntity entity);
        RegistManageEntity GetForm(string keyValue);
        void RemoveRegistManage(string keyValue);
        RegistManageEntity GetEntity(string registcode);
    }
}
