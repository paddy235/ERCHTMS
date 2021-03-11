using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Cache.Factory;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ERCHTMS.Cache
{
    /// <summary>
    /// 描 述：岗位信息缓存
    /// </summary>
    public class PostCache
    {
        private PostBLL busines = new PostBLL();
        private DepartmentCache departmentCache = new DepartmentCache();
        /// <summary>
        /// 岗位列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList()
        {
            //var cacheList = CacheFactory.Cache().GetCache<IEnumerable<RoleEntity>>(busines.cacheKey);
            //if (cacheList == null)
            //{
                var data = busines.GetList();
                //CacheFactory.Cache().WriteCache(data, busines.cacheKey);
                return data;
            //}
            //else
            //{
            //    return cacheList;
            //}
        }
        /// <summary>
        /// 岗位列表
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
        public IEnumerable<RoleEntity> GetRealList(string departmentid)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(departmentid))
            {
                data = data.Where(t => t.DeptId == departmentid);
            }
            return data;
        }
        /// <summary>
        /// 岗位列表
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList(string organizeId,string isOrg)
        {
            var allData = this.GetList(organizeId);
            var data = allData;
            if (!string.IsNullOrEmpty(organizeId))
            {
                data = data.Where(t => t.OrganizeId == organizeId || t.IsPublic == 1);
            }
            if (isOrg == "true")//这里是选择的是公司
            {
                data = data.Where(t => t.Nature == "厂级");
                if (data.Count() == 0)
                {
                    data = allData.Where(t => t.OrganizeId == organizeId || t.IsPublic == 1);
                }
            }
            else
            {
                //这里选择的是部门
                //DepartmentEntity de = departmentCache.GetEntity(isOrg);
                DepartmentEntity de = isOrg.Length > 32 && isOrg.Contains("-") ? new DepartmentBLL().GetEntity(isOrg) : departmentCache.GetEntityByDeptCode(isOrg);
                if (de == null)
                {
                    var org = new OrganizeBLL().GetEntity(isOrg);
                    if (org != null)
                    {
                        de = new DepartmentEntity { Nature = "厂级", OrganizeId = isOrg };
                    }
                }
                else
                {
                    data = data.Where(t => (t.Nature == de.Nature && t.DeptId == de.DepartmentId));
                    if (data.Count() == 0)
                    {
                        data = allData.Where(t => t.Nature == de.Nature || (t.IsPublic == 1 && t.Nature == de.Nature));
                    }
                }
            }
            return data;
        }
    }
}
