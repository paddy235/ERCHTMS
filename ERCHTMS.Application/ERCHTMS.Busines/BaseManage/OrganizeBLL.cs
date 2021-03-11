using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Cache.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util;
using System.Data;

namespace ERCHTMS.Busines.BaseManage
{
    /// <summary>
    /// 描 述：机构管理
    /// </summary>
    public class OrganizeBLL
    {
        private IOrganizeService service = new OrganizeService();
        /// <summary>
        /// 缓存key
        /// </summary>
        public string cacheKey = BSFramework.Util.Config.GetValue("SoftName")+"_OrganizeCache";

        #region 获取数据
        /// <summary>
        /// 机构列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrganizeEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 机构实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OrganizeEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据机构编码获取机构
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public OrganizeEntity GetEntityByCode(string orgCode)
        {
            return GetList().Where(t => t.EnCode == orgCode).FirstOrDefault();
        }
        public DataTable GetDTList()
        {
            return service.GetDTList();
        }
         /// <summary>
        /// 根据部门编码获取机构编码
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public string GetOrgCode(string deptCode)
        {
            return service.GetOrgCode(deptCode);
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 公司名称不能重复
        /// </summary>
        /// <param name="organizeName">公司名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            return service.ExistFullName(fullName, keyValue);
        }
        /// <summary>
        /// 外文名称不能重复
        /// </summary>
        /// <param name="enCode">外文名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            return service.ExistEnCode(enCode, keyValue);
        }
        /// <summary>
        /// 中文名称不能重复
        /// </summary>
        /// <param name="shortName">中文名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistShortName(string shortName, string keyValue)
        {
            return service.ExistShortName(shortName, keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
                CacheFactory.Cache().RemoveCache(cacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存机构表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="organizeEntity">机构实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, OrganizeEntity organizeEntity)
        {
            try
            {
                service.SaveForm(keyValue, organizeEntity);
                CacheFactory.Cache().RemoveCache(cacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
