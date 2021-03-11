using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.Observerecord
{
    public class ObserverSettingService : IObserverSettingService
    {
        public void Edit(List<ObserverSettingEntity> models)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            try
            {
                foreach (var item in models)
                {
                    var entity = db.FindEntity<ObserverSettingEntity>(item.SettingId);
                    entity.Cycle = item.Cycle;
                    entity.Times = item.Times;
                    var items = item.DeptId.Split(',').Select(x => new ObserverSettingItemEntity() { ItemId = Guid.NewGuid().ToString(), DeptId = x, SettignId = item.SettingId }).ToList();

                    db.Delete<ObserverSettingItemEntity>(x => x.SettignId == item.SettingId);
                    db.Update(entity);
                    db.Insert(items);
                }

                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }

        public List<ObserverSettingEntity> GetData()
        {
            IRepository db = new RepositoryFactory().BaseRepository();

            var query = from q1 in db.IQueryable<ObserverSettingEntity>()
                        join q2 in (from q1 in db.IQueryable<DepartmentEntity>()
                                    join q2 in db.IQueryable<ObserverSettingItemEntity>() on q1.DepartmentId equals q2.DeptId
                                    select new { q1, q2 }) on q1.SettingId equals q2.q2.SettignId into into2
                        select new { q1, q2 = into2 };

            var data = query.ToList();

            return data.Select(x => new ObserverSettingEntity() { SettingId = x.q1.SettingId, SettingName = x.q1.SettingName, DeptId = string.Join(",", x.q2.Select(y => y.q2.DeptId)), DeptName = string.Join(",", x.q2.Select(y => y.q1.FullName)), Cycle = x.q1.Cycle, Times = x.q1.Times }).ToList();
        }
    }
}