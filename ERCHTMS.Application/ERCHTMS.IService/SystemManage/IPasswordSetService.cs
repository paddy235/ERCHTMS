using ERCHTMS.Entity.SystemManage;
using System.Collections.Generic;

namespace ERCHTMS.IService.SystemManage
{
    /// <summary>
    /// 描 述：区域管理
    /// </summary>
    public interface IPasswordSetService
    {
        #region 获取数据
        /// <summary>
        /// 区域列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<PasswordSetEntity> GetList(string orgCode);
       
        /// <summary>
        /// 区域实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        PasswordSetEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="keyValue">主键</param>
        bool RemoveForm(string keyValue);
        /// <summary>
        /// 保存区域表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="{">区域实体</param>
        /// <returns></returns>
        bool SaveForm(string keyValue, PasswordSetEntity areaEntity);
        #endregion
    }
}
