using ERCHTMS.Entity.DangerousJobConfig;
using ERCHTMS.IService.DangerousJobConfig;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using BSFramework.Data;

namespace ERCHTMS.Service.DangerousJobConfig
{
    /// <summary>
    /// 描 述：危险作业分级标准配置
    /// </summary>
    public class ClassStandardConfigService : RepositoryFactory<ClassStandardConfigEntity>, ClassStandardConfigIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            DataTable dt = new DataTable();
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!string.IsNullOrWhiteSpace("WorkType"))
                {
                    pagination.conditionJson += string.Format(" and worktype ='{0}'", queryParam["WorkType"].ToString());
                }
            }
            dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ClassStandardConfigEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ClassStandardConfigEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ClassStandardConfigEntity entity)
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
