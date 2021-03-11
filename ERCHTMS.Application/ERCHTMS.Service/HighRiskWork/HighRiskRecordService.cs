using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data.Repository;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 高风险作业安全分析
    /// </summary>
    public class HighRiskRecordService : RepositoryFactory<HighRiskRecordEntity>, HighRiskRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="workId">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HighRiskRecordEntity> GetList(string workId)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_highriskrecord where workid='{0}' order by createdate  desc", workId)).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HighRiskRecordEntity GetEntity(string keyValue)
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
        /// 根据关联id删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveFormByWorkId(string workId)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<HighRiskRecordEntity>(t => t.WorkId.Equals(workId));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HighRiskRecordEntity entity)
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
