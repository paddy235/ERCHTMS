using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Cache.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERCHTMS.Cache
{
    /// <summary>
    /// 描 述：部门信息缓存
    /// </summary>
    public class DepartmentCache
    {
        private DepartmentBLL busines = new DepartmentBLL();

        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList()
        {
            var cacheList = CacheFactory.Cache().GetCache<IEnumerable<DepartmentEntity>>(busines.cacheKey);
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
        /// 获取指定机构下所有的部门
        /// </summary>
        /// <param name="organizeId">机构Id</param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList(string organizeId)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(organizeId))
            {
                data = data.Where(t => t.OrganizeId == organizeId);
            }
            return data;
        }
        /// <summary>
        /// 获取指定机构下所有的部门
        /// </summary>
        /// <param name="organizeCode">机构Code/param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetDeptListByCode(string organizeCode)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(organizeCode))
            {
                data = data.Where(t => t.EnCode.StartsWith(organizeCode));
            }
            return data;
        }
        /// <summary>
        ///根据部门ID获取下属部门列表
        /// </summary>
        /// <param name="parentId">上级部门Code</param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetDeptList(string parentId)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(parentId))
            {
                data = data.Where(t => t.ParentId == parentId);
            }
            return data;
        }
        /// <summary>
        ///根据部门编码获取部门列表
        /// </summary>
        /// <param name="deptIds">由多个部门Id拼接的字符串,如1,2,3</param>
        /// <param name="mode">查询模式。2:获取部门的parentId在deptCodes中的部门，3:获取部门Id在deptCodes中的部门</param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetDeptList(string deptIds, int mode)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(deptIds))
            {
                if (mode == 2)
                {
                    List<string> list = new List<string>();
                    foreach (string str in deptIds.Split(','))
                    {
                        list.Add(str);
                    }
                    data = data.Where(t => list.Contains(t.DepartmentId) || list.Contains(t.ParentId) || list.Contains(t.OrganizeId));
                }
                else if (mode == 8)
                {
                    List<string> list = new List<string>();
                    foreach (string str in deptIds.Split(','))
                    {
                        list.Add(str);
                    }
                    data = data.Where(t => list.Contains(t.DepartmentId) || list.Contains(t.ParentId) || list.Contains(t.OrganizeId) || list.Contains(t.SendDeptID));
                }
                else if (mode == 5)
                {
                    List<string> list = new List<string>();
                    foreach (string str in deptIds.Split(','))
                    {
                        list.Add(str);
                    }
                    var dept = busines.GetEntity(deptIds);
                    if (dept != null)
                        data = data.Where(t => t.EnCode.Contains(dept.EnCode) || list.Contains(t.ParentId) || list.Contains(t.OrganizeId) || list.Contains(t.SendDeptID));
                    else
                        data = data.Where(t => list.Contains(t.DepartmentId) || list.Contains(t.ParentId) || list.Contains(t.OrganizeId) || list.Contains(t.SendDeptID));
                }
                else
                {
                    data = data.Where(t => deptIds.Contains(t.DepartmentId));
                }

            }
            return data;
        }
        /// <summary>
        /// 根据部门Id获取部门
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <returns></returns>

        public DepartmentEntity GetEntity(string departmentId)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(departmentId))
            {
                var d = data.Where(t => t.DepartmentId == departmentId).ToList<DepartmentEntity>();
                if (d.Count > 0)
                {
                    return d[0];
                }
            }
            return new DepartmentEntity();
        }
        /// <summary>
        /// 根据部门Code获取部门
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        public DepartmentEntity GetEntityByDeptCode(string deptCode)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(deptCode))
            {
                var d = data.Where(t => t.EnCode == deptCode).ToList<DepartmentEntity>();
                if (d.Count > 0)
                {
                    return d[0];
                }
            }
            return new DepartmentEntity();
        }
    }
}
