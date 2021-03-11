using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using BSFramework.Util.Extension;
using BSFramework.Util;

namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：预知训练作业风险措施
    /// </summary>
    public class TrainmeasuresService : RepositoryFactory<TrainmeasuresEntity>, TrainmeasuresIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TrainmeasuresEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 根据作业Id获取列表
        /// </summary>
        /// <param name="workId">作业Id</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TrainmeasuresEntity> GetListByWorkId(string workId)
        {
            return this.BaseRepository().IQueryable(t=>t.WorkId==workId).OrderByDescending(x=>x.CreateDate).ToList();
        }
        public DataTable GetPageListByWorkId(Pagination pagination, string queryJson)
        {
            //return this.BaseRepository().IQueryable(t => t.WorkId == workId).OrderByDescending(x => x.CreateDate).ToList();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["WorkId"].IsEmpty()) {
                pagination.conditionJson += string.Format(" and workid='{0}' ", queryParam["WorkId"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TrainmeasuresEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, TrainmeasuresEntity entity)
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