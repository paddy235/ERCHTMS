using ERCHTMS.Entity.SafeReward;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.SafeReward
{
    /// <summary>
    /// 描 述：安全奖励详细
    /// </summary>
    public interface SaferewarddetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SaferewarddetailEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SaferewarddetailEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取奖励详细列表
        /// </summary>
        /// <param name="rewardId">安全奖励id</param>
        /// <returns></returns>
        IEnumerable<SaferewarddetailEntity> GetListByRewardId(string rewardId);

        /// <summary>
        /// 根据安全奖励ID删除数据
        /// </summary>
        /// <param name="rewardId">安全奖励ID</param>
        int Remove(string rewardId);
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
        void SaveForm(string keyValue, SaferewarddetailEntity entity);
        #endregion
    }
}
