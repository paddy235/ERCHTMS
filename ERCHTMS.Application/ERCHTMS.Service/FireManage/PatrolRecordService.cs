using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// 描 述：巡查记录（重点防火部位子表）
    /// </summary>
    public class PatrolRecordService : RepositoryFactory<PatrolRecordEntity>, PatrolRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //查询条件
                if (!queryParam["MainId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and MainId='{0}'", queryParam["MainId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PatrolRecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from HRS_PATROLRECORD where 1=1 " + queryJson).ToList();
            //return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PatrolRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, PatrolRecordEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                PatrolRecordEntity pe = this.BaseRepository().FindEntity(keyValue);
                if (pe == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    SavePartEntity(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
                SavePartEntity(entity);
            }
        }
        public void SavePartEntity(PatrolRecordEntity entity) {
            try
            {
                //更新主表下次巡查时间、最近巡查时间
                IRepository db = new RepositoryFactory().BaseRepository();
                KeyPartEntity ke = db.FindEntity<KeyPartEntity>(entity.MainId);
                if (ke != null)
                {

                    ke.LatelyPatrolDate = entity.PatrolDate;
                    if (entity.PatrolPeriod != null)
                    {
                        ke.NextPatrolDate = entity.PatrolDate.Value.AddDays(entity.PatrolPeriod.Value);
                    }
                    else
                    {
                        ke.NextPatrolDate = entity.NextPatrolDate;
                    }
                    db.Update<KeyPartEntity>(ke);
                }
            }
            catch { }
        }
        #endregion
    }
}
