using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using BSFramework.Data;

namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：辨识评估计划相关联的机构和人员信息
    /// </summary>
    public class RiskPlanDataService : RepositoryFactory<RiskPlanDataEntity>, RiskPlanDataIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RiskPlanDataEntity> GetList(int dataType, string planId)
        {
            return this.BaseRepository().IQueryable().ToList().Where(t=>t.DataType==dataType && t.PlanId==planId);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskPlanDataEntity GetEntity(string keyValue)
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
        /// 根据计划ID删除数据
        /// </summary>
        /// <param name="planId">计划ID</param>
        public int Remove(string planId )
        {
            //DbParameter[] parameter = 
            //{
            //      DbParameters.CreateDbParameter("@PlanId", planId)
            //};
            return this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_RISKPPLANDATA where PlanId='{0}'", planId));
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RiskPlanDataEntity entity)
        {
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
        /// 批量保存数据
        /// </summary>
        /// <param name="list">集合对象</param>
        /// <returns></returns>
        public void Save(List<RiskPlanDataEntity> list)
        {
            this.BaseRepository().Insert(list);
        }
        #endregion
    }
}
