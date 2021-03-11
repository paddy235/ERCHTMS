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
    /// 描 述：黑名单条件设置
    /// </summary>
    public class BlackSetBLL
    {
        private IBlackSetService service = new BlackSetService();
        /// <summary>
        /// 缓存key
        /// </summary>
        public string cacheKey = BSFramework.Util.Config.GetValue("SoftName") + "_BlackSetCache";

        #region 获取数据
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BlackSetEntity> GetList(string deptCode)
        {
            return service.GetList(deptCode);
        }

        public BlackSetEntity GetAgeRange(string deptCode)
        {
            return service.GetAgeRange(deptCode);
        }
        #endregion

        #region 验证数据

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除角色
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
        /// 保存角色表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, List<BlackSetEntity> list)
        {
            try
            {
                service.SaveForm(keyValue, list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
