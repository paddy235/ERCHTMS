using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.IService.SystemManage
{
    public interface IDeptMenuAuthService
    {
        List<DeptMenuAuthEntity> GetList(string departId);
        void Remove(List<DeptMenuAuthEntity> delData);
        void Add(List<DeptMenuAuthEntity> newdata);
        void Remove(string departId);
        DeptMenuAuthEntity GetEntityByModuleId(string moduleId);
        void InsertList(List<DeptMenuAuthEntity> insetDeptMenuAuthList);
        /// <summary>
        /// 判断菜单是否被授权
        /// </summary>
        /// <param name="keyValue">菜单ID</param>
        /// <returns></returns>
        bool HasAuth(string keyValue);
    }
}
