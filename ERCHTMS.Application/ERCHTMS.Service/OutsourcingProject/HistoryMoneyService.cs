using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全保证金历史信息
    /// </summary>
    public class HistoryMoneyService : RepositoryFactory<HistoryMoneyEntity>, HistoryMoneyIService
    {
        /// <summary>
        /// 获取历史记录分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetHisPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();

            if (!queryParam["moneyid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" t.moneyid='{0}' ", queryParam["moneyid"].ToString());
            }

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键ID</param>
        /// <returns></returns>
        public HistoryMoneyEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
    }
}
