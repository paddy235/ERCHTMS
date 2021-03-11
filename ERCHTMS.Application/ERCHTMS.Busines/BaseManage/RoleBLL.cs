using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Cache.Factory;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Busines.BaseManage
{
    /// <summary>
    /// 描 述：角色管理
    /// </summary>
    public class RoleBLL
    {
        private IRoleService service = new RoleService();
        /// <summary>
        /// 缓存key
        /// </summary>
        public string cacheKey = BSFramework.Util.Config.GetValue("SoftName") + "_RoleCache";

        #region 获取数据
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList()
        {
            return service.GetList();
        }
        public IEnumerable<RoleEntity> GetList(int Category)
        {
            return service.GetList(Category);
        }
        /// <summary>
        /// 根据机构Id获取角色信息
        /// </summary>
        /// <param name="organizeId">机构Id</param>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList(string organizeId)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                return GetList().Where(t => t.OrganizeId == organizeId);
            }
            return service.GetList();
        }
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 角色列表all
        /// </summary>
        /// <returns></returns>
        public List<RoleEntity> GetAllList()
        {
            return service.GetAllList().ToList();
        }
        /// <summary>
        /// 角色实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RoleEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 角色编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            return service.ExistEnCode(enCode, keyValue);
        }
        /// <summary>
        /// 角色名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            return service.ExistFullName(fullName, keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool RemoveForm(string keyValue)
        {
            
            return service.RemoveForm(keyValue);
            
        }
        /// <summary>
        /// 保存角色表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, RoleEntity roleEntity)
        {
            return service.SaveForm(keyValue, roleEntity);
        }
        #endregion
    }
}
