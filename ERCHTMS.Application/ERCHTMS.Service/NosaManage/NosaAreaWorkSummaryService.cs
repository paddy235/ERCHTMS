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
    /// 区域代表工作总结
    /// </summary>
    public class NosaAreaWorkSummaryService : RepositoryFactory<NosaAreaWorkSummaryEntity>, NosaAreaWorkSummaryIService
    {
        /// <summary>
        /// 执行同步Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>true 执行成功 false 执行失败</returns>
        public bool SyncAreaWorkSummary(string sql) {
            var num = this.BaseRepository().ExecuteBySql(sql);
            if (num > 0) return true;
            else return false;
        }
        /// <summary>
        /// 工作总结提交
        /// </summary>
        /// <param name="keyValue"></param>
        public void CommitAreaSummary(string keyValue)
        {
            var entity = this.BaseRepository().FindEntity(keyValue);
            entity.IsCommit = 1;
            this.BaseRepository().Update(entity);
        }

        /// <summary>
        /// 获取区域代表工作总结分页列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetAreaWorkPageJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();

            if (!queryParam["KeyWord"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and (areaname like '%{0}%' or areasuper like '%{0}%' or dutydepart like '%{0}%' )", queryParam["KeyWord"].ToString());
            }
            if (!queryParam["DataRange"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and IsCommit = {0}", Convert.ToInt32(queryParam["DataRange"].ToString()));
            }
            if (!queryParam["Month"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and to_char(Month,'yyyy-MM') = '{0}'", queryParam["Month"].ToString());
            }
            if (!queryParam["AreaId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and areaid = '{0}'", queryParam["AreaId"].ToString());
            }
            
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
    }
}
