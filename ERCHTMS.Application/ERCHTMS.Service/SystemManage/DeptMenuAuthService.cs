using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.SystemManage
{
    public class DeptMenuAuthService : RepositoryFactory<DeptMenuAuthEntity>, IDeptMenuAuthService
    {
        public void Add(List<DeptMenuAuthEntity> newdata)
        {
            this.BaseRepository().Insert(newdata);
        }

        public DeptMenuAuthEntity GetEntityByModuleId(string moduleId)
        {
            var expression = LinqExtensions.True<DeptMenuAuthEntity>();
            expression = expression.And(p => p.ModuleId == moduleId);
            return this.BaseRepository().FindEntity(expression);
        }

        public List<DeptMenuAuthEntity> GetList(string departId)
        {
            var query = this.BaseRepository().IQueryable();
            if (!string.IsNullOrWhiteSpace(departId))
                query = query.Where(p => p.DeptId.Equals(departId));
            return query.ToList();
        }
        /// <summary>
        /// 判断菜单是否被授权
        /// </summary>
        /// <param name="moduleId">菜单ID</param>
        /// <returns></returns>
        public bool HasAuth(string moduleId)
        {
            return this.BaseRepository().IQueryable(p => p.ModuleId == moduleId).Count() > 0;
         }

        public void InsertList(List<DeptMenuAuthEntity> insetDeptMenuAuthList)
        {
            this.BaseRepository().Insert(insetDeptMenuAuthList);
        }

        public void Remove(string departId)
        {
            var expression = LinqExtensions.True<DeptMenuAuthEntity>();
            expression = expression.And(p =>p.DeptId==departId);
            this.BaseRepository().Delete(expression);
        }

        public void Remove(List<DeptMenuAuthEntity> delData)
        {
            this.BaseRepository().Delete(delData);
        }
    }
}
