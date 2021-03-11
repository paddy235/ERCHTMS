using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;


namespace ERCHTMS.Service.OutsourcingProject
{
    public class HisPeopleReviewService : RepositoryFactory<HisPeopleReviewEntity>, IHisPeopleReviewService
    {
        public DataTable GetHistoryPeopleList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();

            if (!queryParam["hispeoplereviewid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.hispeoplereviewid='{0}' ", queryParam["hispeoplereviewid"].ToString());
            }

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }


        public HisPeopleReviewEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
    }
}
