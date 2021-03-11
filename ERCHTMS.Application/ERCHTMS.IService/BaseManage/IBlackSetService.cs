using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.BaseManage
{
    /// <summary>
    /// 描 述：黑名单条件设置
    /// </summary>
    public interface IBlackSetService
    {
        #region 获取数据
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<BlackSetEntity> GetList(string deptCode);

        BlackSetEntity GetAgeRange(string deptCode);

        #endregion

        #region 验证数据
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存角色表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        void SaveForm(string keyValue, List<BlackSetEntity> list);
        #endregion
    }
}
