using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;

namespace ERCHTMS.Service.RoutineSafetyWork
{
    public class AnnounDetailService : RepositoryFactory<AnnounDetailEntity>, AnnounDetailIService
    {
        public DataTable GetPageList(Pagination pagination, string queryJson) {
            var queryParam = queryJson.ToJObject();
            DatabaseType dataType = DbHelper.DbType;
            if (!queryParam["Auuounid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and t.auuounid ='{0}'", queryParam["Auuounid"].ToString());
            }
            if (!queryParam["State"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and t.status ={0}", queryParam["State"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        public IEnumerable<AnnounDetailEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        public AnnounDetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        public void SaveForm(string keyValue, AnnounDetailEntity entity)
        {
            if (string.IsNullOrWhiteSpace(keyValue))
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            else {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl == null)
                {
                    entity.Id = keyValue;
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            
        }


        public AnnounDetailEntity GetEntity(string UserId, string keyValue)
        {
            return this.BaseRepository().IQueryable().Where(x => x.AuuounId == keyValue && x.UserId == UserId).ToList().FirstOrDefault();
        }
    }
}
