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
   public class RegistManageBLL
    {
        private IRegistManageService service = new RegistManageService();

        public List<RegistManageEntity> GetList(string keyword)
        {
            return service.GetList(keyword);
        }

        public void SaveForm(string keyValue, RegistManageEntity entity)
        {
            service.SaveForm(keyValue,entity);
        }

        public RegistManageEntity GetForm(string keyValue)
        {
            return service.GetForm(keyValue);
        }

        public void RemoveRegistManage(string keyValue)
        {
             service.RemoveRegistManage(keyValue);
        }

        public RegistManageEntity GetEntity(string registcode)
        {
            return service.GetEntity(registcode);
        }
    }
}
