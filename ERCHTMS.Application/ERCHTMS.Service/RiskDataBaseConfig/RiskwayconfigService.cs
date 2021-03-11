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
    /// 描 述：安全风险管控取值配置表
    /// </summary>
    public class RiskwayconfigService : RepositoryFactory<RiskwayconfigEntity>, RiskwayconfigIService
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
            if (!queryParam["WayType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and WayTypeCode ='{0}'", queryParam["WayType"].ToString());
            }

            if (!queryParam["RiskType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and RiskTypeCode ='{0}'", queryParam["RiskType"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RiskwayconfigEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskwayconfigEntity GetEntity(string keyValue)
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
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string sql = string.Format("delete from BIS_RISKWAYCONFIGDETAIL t where t.WAYCONFIGID='{0}'", keyValue);
                res.ExecuteBySql(sql);
                res.Delete<RiskwayconfigEntity>(keyValue);
                res.Commit();
            }
            catch (System.Exception)
            {

                res.Rollback();
            }
            

        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RiskwayconfigEntity entity, List<RiskwayconfigdetailEntity> list)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<RiskwayconfigEntity>(entity);
                }
                else
                {
                    entity.Create();
                    res.Insert<RiskwayconfigEntity>(entity);
                }
                string sql = string.Format("delete from BIS_RISKWAYCONFIGDETAIL t where t.WAYCONFIGID='{0}'", entity.ID);
                res.ExecuteBySql(sql);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Create();
                    list[i].WayConfigId = entity.ID;
                }
                res.Insert<RiskwayconfigdetailEntity>(list);

                res.Commit();
            }
            catch (System.Exception)
            {

                res.Rollback();
            }
           
          
        }
        #endregion
    }
}
