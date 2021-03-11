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
    public class AppSettingAssociationService : RepositoryFactory<AppSettingAssociationEntity>, IAppSettingAssociationService
    {
        public AppSettingAssociationEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        public AppSettingAssociationEntity GetEntity(string moduleId,string columnId)
        {
            var expression = LinqExtensions.True<AppSettingAssociationEntity>();
            expression = expression.And(p => p.ModuleId.Contains(moduleId) && p.ColumnId.Contains(columnId));
            return this.BaseRepository().FindEntity(expression);
        }
        public List<AppSettingAssociationEntity> GetList(string deptId)
        {
            var query = this.BaseRepository().IQueryable();
            if (!string.IsNullOrWhiteSpace(deptId))
                query = query.Where(p => p.DeptId.Equals(deptId));
            return query.ToList();
        }
        /// <summary>
        /// 取授权过的关系
        /// </summary>
        /// <param name="deptId">单位ID</param>
        /// <param name="list">授权过的菜单的Id</param>
        /// <returns></returns>
        public List<AppSettingAssociationEntity> GetList(string deptId, List<string> list)
        {
            var query = this.BaseRepository().IQueryable();
            if (!string.IsNullOrWhiteSpace(deptId))
                query = query.Where(p => p.DeptId.Equals(deptId));
            if (list != null && list.Count>0)
            {
                query = query.Where(x => list.Contains(x.ModuleId));
            }
            return query.ToList();
        }

        public void Remove(string moduleId, string columnId)
        {
            var expression = LinqExtensions.True<AppSettingAssociationEntity>();
            expression = expression.And(p => p.ModuleId.Contains(moduleId) && p.ColumnId.Contains(columnId));
            this.BaseRepository().Delete(expression);
        }
        public void Remove(List<string> moduleIds, string columnId)
        {
            var expression = LinqExtensions.True<AppSettingAssociationEntity>();
            expression = expression.And(p => moduleIds.Contains(p.ModuleId) && p.ColumnId.Contains(columnId));
            this.BaseRepository().Delete(expression);
        }

        public void RemoveByColumnId(string columnId)
        {
            var expression = LinqExtensions.True<AppSettingAssociationEntity>();
            expression = expression.And(p => p.ColumnId.Contains(columnId));
            this.BaseRepository().Delete(expression);
        }

        public void SaveForm(string keyValue, AppSettingAssociationEntity entity)
        {
            //先判断该数据数据库有没有
            var expression = LinqExtensions.True<AppSettingAssociationEntity>();
            expression = expression.And(p => p.ColumnId == entity.ColumnId && p.DeptId==entity.DeptId && entity.ModuleId==p.ModuleId);
            var oldEntity = this.BaseRepository().FindEntity(expression);


            if (string.IsNullOrWhiteSpace(keyValue) && oldEntity==null)
            {
                entity.Id = Guid.NewGuid().ToString();
                this.BaseRepository().Insert(entity);
            }
            else {
                if (oldEntity != null)
                {
                    oldEntity.Sort = entity.Sort;
                    this.BaseRepository().Update(oldEntity);
                }
                else
                {
                    this.BaseRepository().Update(entity);

                }
            }
        }

        public void SaveList(List<AppSettingAssociationEntity> adds)
        {
            this.BaseRepository().Insert(adds);
        }

        public void Remove(string departId)
        {
            var expression = LinqExtensions.True<AppSettingAssociationEntity>();
            expression = expression.And(p => p.DeptId == departId);
            this.BaseRepository().Delete(expression); ;
        }

        public void InsertList(List<AppSettingAssociationEntity> insertAssociationEntities)
        {
            this.BaseRepository().Insert(insertAssociationEntities);
        }
        /// <summary>
        /// 根据单位ID与菜单ID删除关联关系
        /// </summary>
        /// <param name="departId"></param>
        /// <param name="moduleIds"></param>
        public void Remove(string departId, List<string> moduleIds)
        {
            if (moduleIds != null && moduleIds.Count>0)
            {
                moduleIds.ForEach(x => {
                    var expression = LinqExtensions.True<AppSettingAssociationEntity>();
                    expression = expression.And(p => p.DeptId == departId && p.ModuleId==x);
                    this.BaseRepository().Delete(expression);
                });
            }
        }

        public List<AppSettingAssociationEntity> GetListByColumnId(string columnId,List<string> list)
        {
            var query = this.BaseRepository().IQueryable();
            if (!string.IsNullOrWhiteSpace(columnId))
                query = query.Where(p => p.ColumnId.Equals(columnId));
            if (list != null && list.Count > 0)
                query = query.Where(x => list.Contains(x.ModuleId));
            return query.ToList();
        }

        public List<AppSettingAssociationEntity> GetListByColumnId(string columnId)
        {
            var query = this.BaseRepository().IQueryable();
            if (!string.IsNullOrWhiteSpace(columnId))
                query = query.Where(p => p.ColumnId.Equals(columnId));
            return query.ToList();
        }

        /// <summary>
        /// 验证是否可取消授权，能就返回true
        /// </summary>
        /// <param name="delData"></param>
        /// <returns></returns>
        public bool CheckData(List<DeptMenuAuthEntity> delData)
        {
            var deptId = delData.Select(p => p.DeptId).Distinct().FirstOrDefault();//班组ID
            var menuIds = delData.Select(p => p.ModuleId).ToList();//菜单的ID
           var query = this.BaseRepository().IQueryable(p => p.DeptId == deptId && menuIds.Contains(p.ModuleId));
            int count = query.Count();
            return count < 1;

        }
    }
}
