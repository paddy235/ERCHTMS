using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Cache.Factory;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ERCHTMS.Cache
{
    /// <summary>
    /// 描 述：职位信息缓存
    /// </summary>
    public class JobCache
    {
        private JobBLL busines = new JobBLL();
        private DepartmentCache departmentCache = new DepartmentCache();
        /// <summary>
        /// 职位列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList()
        {
            var cacheList = CacheFactory.Cache().GetCache<IEnumerable<RoleEntity>>(busines.cacheKey);
            if (cacheList == null)
            {
                var data = busines.GetList();
                CacheFactory.Cache().WriteCache(data, busines.cacheKey);
                return data;
            }
            else
            {
                return cacheList;
            }
        }
        /// <summary>
        /// 职位列表
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList(string organizeId)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(organizeId))
            {
                data = data.Where(t => t.OrganizeId == organizeId);
            }
            return data;
        }

        /// <summary>
        /// 岗位列表
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList(string organizeId, string Nature)
        {
            var data = this.GetList();

            data = data.Where(t => t.OrganizeId == organizeId && t.Nature == Nature);

            return data;
        }
    }
}
