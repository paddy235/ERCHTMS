using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.IService.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.RiskDatabase
{
    public class RiskAssessHistoryService : RepositoryFactory<RiskAssessHistoryEntity>, RiskAssessHistoryIService
    {

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson.Length > 0) {
                var queryParam = queryJson.ToJObject();
                 //查询条件
                if (!queryParam["TimeEnd"].IsEmpty())
                {
                    string TimeEnd = queryParam["TimeEnd"].ToString();
                    TimeEnd = DateTime.Parse(TimeEnd).AddDays(1).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and to_char(CreateDate,'yyyy-MM-dd') < to_char(to_date('{0}', 'yyyy-MM-dd'),'yyyy-MM-dd')", TimeEnd);

                }
                //查询条件
                if (!queryParam["TimeStart"].IsEmpty())
                {
                    string TimeStart = queryParam["TimeStart"].ToString();
                    pagination.conditionJson += string.Format(" and CreateDate >= (select  to_date('{0}', 'yyyy-MM-dd') from dual)", TimeStart);
                }
                //查询条件
                if (!queryParam["DangerSourceName"].IsEmpty())
                {
                    string DangerSourceName = queryParam["DangerSourceName"].ToString();
                    pagination.conditionJson += string.Format(" and HisName like '%{0}%'", DangerSourceName);
                }
            }
           
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        public IEnumerable<RiskAssessHistoryEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public RiskAssessHistoryEntity GetEntity(string keyValue)
        {
            throw new NotImplementedException();
        }

        public void SaveForm(string keyValue, RiskAssessHistoryEntity entity)
        {
            this.BaseRepository().Insert(entity);
        }

        public int Remove(string historyid)
        {
            int count = this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_RISKHISTORY t where t.historyid='{0}'", historyid));
            if (count > 0)
            {
                return this.BaseRepository().Delete(historyid);
            }
            else {
                return -1;
            }
        }
        public int ExecuteBySql(string sql) {
            return this.BaseRepository().ExecuteBySql(sql);
        }
    }
}
