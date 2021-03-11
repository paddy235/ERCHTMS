using ERCHTMS.Entity.SafePunish;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.SafePunish
{
    /// <summary>
    /// 描 述：安全考核详细
    /// </summary>
    public interface SafepunishdetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafepunishdetailEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafepunishdetailEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, SafepunishdetailEntity entity);

        /// <summary>
        /// 获取奖励详细列表
        /// </summary>
        /// <param name="punishId">安全考核id</param>
        /// <param name="type">类型 0:主考核对象 1:连带考核对象</param>
        /// <returns></returns>
        IEnumerable<SafepunishdetailEntity> GetListByPunishId(string punishId, string type);

        /// <summary>
        /// 根据安全考核ID删除数据
        /// </summary>
        /// <param name="punishId">安全考核ID</param>
        /// <param name="type">类型</param>
        int Remove(string punishId, string type);
        #endregion
    }
}
