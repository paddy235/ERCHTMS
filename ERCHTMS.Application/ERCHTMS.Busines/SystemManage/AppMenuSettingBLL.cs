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
   public class AppMenuSettingBLL
    {
        private IAppMenuSettingService service = new AppMenuSettingService();

        /// <summary>
        /// 根据DeptId获取该单位下面的栏目
        /// </summary>
        /// <param name="deptId">单位ID</param>
        /// <param name="themeType">0 第一套工作栏  1 第二套</param>
        /// <param name="platform">2 手机APP 1 安卓终端 0 windwos</param>
        /// <returns></returns>
        public List<AppMenuSettingEntity> GetList(string deptId, int themeType, int platform)
        {
            return service.GetList(deptId, themeType,platform);
        }

        public void SaveForm(string keyValue, AppMenuSettingEntity entity)
        {
            service.SaveForm(keyValue, entity);
        }

        public AppMenuSettingEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public void Remove(string keyValue)
        {
            service.Remove(keyValue);
        }

        public void RemoveByDeptId(string departId)
        {
            service.RemoveByDeptId(departId);
        }

        public List<AppMenuSettingEntity> GetListByDeptId(string departId)
        {
            return service.GetListByDeptId(departId);
        }

        public void InsertList(List<AppMenuSettingEntity> insertMenuSettingEntities)
        {
            service.InsertList(insertMenuSettingEntities);
        }
    }
}
