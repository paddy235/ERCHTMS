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
   public class DeptMenuAuthBLL
    {
        private IDeptMenuAuthService service = new DeptMenuAuthService();

        public List<DeptMenuAuthEntity> GetList(string departId)
        {
            return service.GetList(departId);
        }

        public void Remove(List<DeptMenuAuthEntity>  delData)
        {
            service.Remove(delData);
        }

        public void Add(List<DeptMenuAuthEntity> newdata)
        {
            service.Add(newdata);
        }

        public void Remove(string departId)
        {
            service.Remove(departId);
        }

        public DeptMenuAuthEntity GetEntityByModuleId(string moduleId)
        {
            return service.GetEntityByModuleId(moduleId);
        }

       public void InsertList(List<DeptMenuAuthEntity> insetDeptMenuAuthList)
       {
           service.InsertList(insetDeptMenuAuthList);
       }
        /// <summary>
        /// 判断菜单是否被授权
        /// </summary>
        /// <param name="moduleId">菜单ID</param>
        /// <returns></returns>
        public bool HasAuth(string moduleId)
        {
            return service.HasAuth(moduleId);
        }
    }
}
