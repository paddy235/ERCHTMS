using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：审查记录明细表
    /// </summary>
    public class InvestigateDtRecordService : RepositoryFactory<InvestigateDtRecordEntity>, InvestigateDtRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary> 
        /// <param name="InvestigateRecordid">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<InvestigateDtRecordEntity> GetList(string InvestigateRecordId)
        {
            return this.BaseRepository().IQueryable(p => p.INVESTIGATERECORDID == InvestigateRecordId).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public InvestigateDtRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, InvestigateDtRecordEntity entity)
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
        #endregion
    }
}