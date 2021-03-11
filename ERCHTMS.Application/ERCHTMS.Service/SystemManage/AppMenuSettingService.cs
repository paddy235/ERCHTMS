using BSFramework.Data.Repository;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.SystemManage
{
    public class AppMenuSettingService : RepositoryFactory<AppMenuSettingEntity>, IAppMenuSettingService
    {
        public AppMenuSettingEntity GetEntity(string keyValue)
        {
          return  this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据DeptId获取该单位下面的栏目
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns>List<AppMenuSettingEntity></returns>
        public List<AppMenuSettingEntity> GetList(string deptId, int themeType, int platform)
        {
            var query = this.BaseRepository().IQueryable().Where(x=>x.ThemeCode==themeType && x.PlatformType==platform);
            if (!string.IsNullOrWhiteSpace(deptId))
                query = query.Where(p => p.DeptId.Equals(deptId));
            return query.ToList();
        }

        public List<AppMenuSettingEntity> GetListByDeptId(string departId)
        {
            var query = this.BaseRepository().IQueryable().Where(x => x.DeptId == departId);
            return query.ToList();
        }

        public void InsertList(List<AppMenuSettingEntity> insertMenuSettingEntities)
        {
            this.BaseRepository().Insert(insertMenuSettingEntities);
        }

        public void Remove(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        public void RemoveByDeptId(string departId)
        {
            var expression = LinqExtensions.True< AppMenuSettingEntity> ();
            expression = expression.And(p => p.DeptId == departId);
            this.BaseRepository().Delete(expression);
        }                                                                                             

        public void SaveForm(string keyValue, AppMenuSettingEntity entity)
        {
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
