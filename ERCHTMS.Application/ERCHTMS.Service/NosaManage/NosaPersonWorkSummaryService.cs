using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Data;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.NosaManage
{
    /// <summary>
    /// 元素负责人工作总结
    /// </summary>
    public class NosaPersonWorkSummaryService : RepositoryFactory<NosaPersonWorkSummaryEntity>, NosaPersonWorkSummaryIService
    {
        /// <summary>
        /// 执行同步Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>true 执行成功 false 执行失败</returns>
        public bool SyncPersonWorkSummary(string sql)
        {
            var num = this.BaseRepository().ExecuteBySql(sql);
            if (num > 0) return true;
            else return false;
        }
        /// <summary>
        /// 工作总结提交
        /// </summary>
        /// <param name="keyValue"></param>
        public void CommitPeopleSummary(string keyValue) {
            var entity = this.BaseRepository().FindEntity(keyValue);
            entity.IsCommit = 1;
           this.BaseRepository().Update(entity);
        }
        public DataTable GetTable(string sql) {
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取元素负责人工作总结分页列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetElementPageJson(Pagination pagination, string queryJson) {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();

            if (!queryParam["KeyWord"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and (elementname like '%{0}%' or elementsuper like '%{0}%' or dutydepart like '%{0}%' )", queryParam["KeyWord"].ToString());
            }
            if (!queryParam["DataRange"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and iscommit = {0}", Convert.ToInt32(queryParam["DataRange"].ToString()));
            }
            if (!queryParam["Month"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and to_char(month,'yyyy-MM') = '{0}'", queryParam["Month"].ToString());
            }
            if (!queryParam["ElementId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and elementid = '{0}'", queryParam["ElementId"].ToString());
            }
            
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public NosaPersonWorkSummaryEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
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
        public void SaveForm(string keyValue, NosaPersonWorkSummaryEntity entity)
        {
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                NosaPersonWorkSummaryEntity ne = this.BaseRepository().FindEntity(keyValue);
                if (ne == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
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
            }
        }
        #endregion
    }
}
