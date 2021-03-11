using ERCHTMS.Entity.SafeReward;
using ERCHTMS.IService.SafeReward;
using ERCHTMS.Service.SafeReward;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.SafeReward
{
    /// <summary>
    /// 描 述：安全奖励详细
    /// </summary>
    public class SaferewarddetailBLL
    {
        private SaferewarddetailIService service = new SaferewarddetailService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaferewarddetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SaferewarddetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取奖励详细列表
        /// </summary>
        /// <param name="rewardId">安全奖励id</param>
        /// <returns></returns>
        public IEnumerable<SaferewarddetailEntity> GetListByRewardId(string rewardId)
        {
            return service.GetListByRewardId(rewardId);
        }

        /// <summary>
        /// 根据安全奖励id删除数据
        /// </summary>
        /// <param name="rewardId">安全奖励id</param>
        public int Remove(string rewardId)
        {
            try
            {
                service.Remove(rewardId);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SaferewarddetailEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
