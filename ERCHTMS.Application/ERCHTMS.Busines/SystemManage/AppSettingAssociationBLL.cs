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
   public  class AppSettingAssociationBLL
    {
        private IAppSettingAssociationService service = new AppSettingAssociationService();

        /// <summary>
        /// 取栏目与菜单的配置关系
        /// </summary>
        /// <param name="deptId">单位ID</param>
        /// <returns></returns>
        public List<AppSettingAssociationEntity> GetList(string deptId)
        {
            return service.GetList(deptId);
        }
        /// <summary>
        /// 取授权过的关系
        /// </summary>
        /// <param name="deptId">单位ID</param>
        /// <param name="list">授权过的菜单的Id</param>
        /// <returns></returns>
        public List<AppSettingAssociationEntity> GetList(string deptId, List<string> list)
        {
            return service.GetList(deptId, list);
        }

        public void SaveForm(string keyValue, AppSettingAssociationEntity entity)
        {
            service.SaveForm(keyValue, entity);
        }

        public AppSettingAssociationEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public void SaveList(List<AppSettingAssociationEntity> adds)
        {
             service.SaveList(adds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue">菜单ID</param>
        /// <param name="columnId">栏目ID</param>
        /// <returns></returns>
        public AppSettingAssociationEntity GetEntity(string keyValue, string columnId)
        {
            return service.GetEntity(keyValue,columnId);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="moduleId">moduleId 菜单ID</param>
        /// <param name="columnId">栏目Id</param>
        public void Remove(string moduleId, string columnId)
        {
            service.Remove(moduleId, columnId);
        }
        /// <summary>
        /// 删除栏目与菜单的关联关系
        /// </summary>
        /// <param name="columnId">栏目ID</param>
        public void RemoveByColumnId(string columnId)
        {
            service.RemoveByColumnId(columnId);
        }
        /// <summary>
        /// 删除某单位下所有的关系关系
        /// </summary>
        /// <param name="departId">单位ID</param>
        public void Remove(string departId)
        {
            service.Remove(departId);
        }

        public List<AppSettingAssociationEntity> GetListByColumnId(string columnId, List<string> list)
        {
            return service.GetListByColumnId(columnId, list);
        }

        public void InsertList(List<AppSettingAssociationEntity> insertAssociationEntities)
       {
           service.InsertList(insertAssociationEntities);
       }
        /// <summary>
        /// 根据单位与菜单ID 删除对应的关联关系
        /// </summary>
        /// <param name="departId">单位ID</param>
        /// <param name="moduleIds">菜单Id集合</param>
        public void Remove(string departId, List<string> moduleIds)
        {
            service.Remove(departId,moduleIds);
        }
        /// <summary>
        /// 删除栏目与菜单的关联关系
        /// </summary>
        /// <param name="columnId">栏目ID</param>
        public void Remove(List<string> moduleIds, string columnId)
        {
            service.Remove(moduleIds, columnId);
        }

        public List<AppSettingAssociationEntity> GetListByColumnId(string columnId)
        {
            return service.GetListByColumnId(columnId);
        }
        /// <summary>
        /// 验证菜单是否可取消授权。可以返回true ，不可以返回false
        /// </summary>
        /// <param name="delData">要取消授权的数据</param>
        /// <returns></returns>
        public bool CheckData(List<DeptMenuAuthEntity> delData)
        {
            return service.CheckData(delData);
        }
    }
}
