using ERCHTMS.Entity.SafeReward;
using ERCHTMS.IService.SafeReward;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.SafeReward
{
    /// <summary>
    /// 描 述：安全奖励详细
    /// </summary>
    public class SaferewarddetailService : RepositoryFactory<SaferewarddetailEntity>, SaferewarddetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaferewarddetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SaferewarddetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SaferewarddetailEntity entity)
        {
            if (entity.RewardType == "人员")
            {
                UserEntity user = new UserService().GetEntity(entity.RewardNameId);
                entity.BelongDept = user.DepartmentId;
            }
            else if (entity.RewardType == "部门")
            {
                entity.BelongDept = entity.RewardNameId;
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 获取奖励详细列表
        /// </summary>
        /// <param name="rewardId">安全奖励id</param>
        /// <returns></returns>
        public IEnumerable<SaferewarddetailEntity> GetListByRewardId(string rewardId)
        {
            return this.BaseRepository().IQueryable().ToList().Where(t => t.RewardId == rewardId);
        }

        /// <summary>
        /// 根据安全奖励ID删除数据
        /// </summary>
        /// <param name="rewardId">安全奖励ID</param>
        public int Remove(string rewardId)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_SAFEREWARDDETAIL where rewardId='{0}'", rewardId));
        }
        #endregion
    }
}
