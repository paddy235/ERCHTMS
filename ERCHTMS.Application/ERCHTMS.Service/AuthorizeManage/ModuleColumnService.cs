using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.IService.AuthorizeManage;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// 描 述：系统视图
    /// </summary>
    public class ModuleColumnService : RepositoryFactory<ModuleColumnEntity>, IModuleColumnService
    {
        #region 获取数据
        /// <summary>
        /// 视图列表
        /// </summary>
        /// <returns></returns>
        public List<ModuleColumnEntity> GetList()
        {
            return this.BaseRepository().IQueryable().OrderBy(t => t.SortCode).ToList();
        }
        /// <summary>
        /// 视图列表
        /// </summary>
        /// <param name="moduleId">功能Id</param>
        /// <returns></returns>
        public List<ModuleColumnEntity> GetList(string moduleId)
        {
            var expression = LinqExtensions.True<ModuleColumnEntity>();
            expression = expression.And(t => t.ModuleId.Equals(moduleId));
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.SortCode).ToList();
        }
        /// <summary>
        /// 视图实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ModuleColumnEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 添加视图
        /// </summary>
        /// <param name="moduleButtonEntity">视图实体</param>
        public void AddEntity(ModuleColumnEntity moduleColumnEntity)
        {
            moduleColumnEntity.Create();
            this.BaseRepository().Insert(moduleColumnEntity);
        }
        #endregion
    }
}
