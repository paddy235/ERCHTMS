using BSFramework.Data;
using BSFramework.Data.Repository;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using System.Data;

namespace ERCHTMS.Service.RiskDatabase
{
    public class RiskEvaluateService : RepositoryFactory<RiskEvaluate>,IRiskEvaluateService
    {
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["WorkId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and workid = '{0}'", queryParam["WorkId"].ToString());
            }  
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        public IEnumerable<RiskEvaluate> GetList() {
            return this.BaseRepository().IQueryable().ToList();
        }
        public void SaveForm(string keyValue, RiskEvaluate entity)
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
    }
}
