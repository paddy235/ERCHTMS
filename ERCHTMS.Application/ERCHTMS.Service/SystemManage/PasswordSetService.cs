using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.SystemManage
{
    /// <summary>
    /// 描 述：密码规则设置
    /// </summary>
    public class PasswordSetService : RepositoryFactory<PasswordSetEntity>, IPasswordSetService
    {
        #region 获取数据
        /// <summary>
        /// 区域列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PasswordSetEntity> GetList(string orgCode)
        {
            return this.BaseRepository().IQueryable(t => t.OrgCode.StartsWith(orgCode)).ToList();
        }
        /// <summary>
        /// 区域实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PasswordSetEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool RemoveForm(string keyValue)
        {
            return this.BaseRepository().Delete(keyValue)>0?true:false;
        }
        /// <summary>
        /// 保存区域表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="{">区域实体</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, PasswordSetEntity areaEntity)
        {
            areaEntity.Rule = areaEntity.Rule;
            int count = 0;
            if (!string.IsNullOrEmpty(keyValue))
            {
                areaEntity.Modify(keyValue);
                count = this.BaseRepository().Update(areaEntity);
            }
            else
            {
                areaEntity.Create();
                count = this.BaseRepository().Insert(areaEntity);
            }
            return count > 0 ? true : false;
        }
        #endregion
    }
}
