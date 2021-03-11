using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.IService.SystemManage
{
    public interface IAppMenuSettingService
    {
        List<AppMenuSettingEntity> GetList(string deptId, int themeType, int platform);
        void SaveForm(string keyValue, AppMenuSettingEntity entity);
        AppMenuSettingEntity GetEntity(string keyValue);
        void Remove(string keyValue);
        void RemoveByDeptId(string departId);
        List<AppMenuSettingEntity> GetListByDeptId(string departId);
        void InsertList(List<AppMenuSettingEntity> insertMenuSettingEntities);
    }
}
