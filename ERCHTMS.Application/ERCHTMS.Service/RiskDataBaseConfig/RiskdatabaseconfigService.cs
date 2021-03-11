using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.IService.RiskDataBaseConfig;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：安全风险管控配置表
    /// </summary>
    public class RiskdatabaseconfigService : RepositoryFactory<RiskdatabaseconfigEntity>, RiskdatabaseconfigIService
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
            var queryParam = queryJson.ToJObject();
            if (!queryParam["DataType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and DataType ='{0}'", queryParam["DataType"].ToString());
            }
            if (!queryParam["RiskType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and RiskTypeCode ='{0}'", queryParam["RiskType"].ToString());
            }
            if (!queryParam["ConfigType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ConfigTypeCode ='{0}'", queryParam["ConfigType"].ToString());
            }
            if (!queryParam["ItemType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ItemTypeCode ='{0}'", queryParam["ItemType"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 根据SQL语句获取数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetTable(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }
      
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RiskdatabaseconfigEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskdatabaseconfigEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, RiskdatabaseconfigEntity entity)
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
