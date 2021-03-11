using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.IService.SystemManage
{
    public interface IAppSettingAssociationService
    {
        List<AppSettingAssociationEntity> GetList(string deptId);
        List<AppSettingAssociationEntity> GetList(string deptId, List<string> list);
        void SaveForm(string keyValue, AppSettingAssociationEntity entity);
        AppSettingAssociationEntity GetEntity(string keyValue);
        AppSettingAssociationEntity GetEntity(string keyValue, string columnId);
        void Remove(string moduleId, string columnId);
        void RemoveByColumnId(string columnId);
        void SaveList(List<AppSettingAssociationEntity> adds);
        void Remove(string departId);
        void InsertList(List<AppSettingAssociationEntity> insertAssociationEntities);
        void Remove(string departId, List<string> moduleIds);
        void Remove(List<string> moduleIds, string columnId);
        List<AppSettingAssociationEntity> GetListByColumnId(string columnId, List<string> list);
        List<AppSettingAssociationEntity> GetListByColumnId(string columnId);
        bool CheckData(List<DeptMenuAuthEntity> delData);
    }
}
