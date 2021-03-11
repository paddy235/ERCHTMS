using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using ERCHTMS.Entity.HseManage;
using System;
using ERCHTMS.IService.HseManage;
using ERCHTMS.Entity.PublicInfoManage;

namespace ERCHTMS.Service.HseManage
{
    /// <summary>
    /// 描 述：劳动防护预警表
    /// </summary>
    public class CheckRecordService : ICheckRecordService
    {
        public List<CheckRecordEntity> GetData(string userid, int pagesize, int pageindex, out int total)
        {
            IRepository db = new RepositoryFactory().BaseRepository();

            var query = from q in db.IQueryable<CheckRecordEntity>()
                        where q.CreateUserId == userid
                        orderby q.CheckTime descending
                        select q;

            total = query.Count();
            return query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
        }

        public CheckRecordEntity GetDetail(string id)
        {
            IRepository db = new RepositoryFactory().BaseRepository();

            var data = (from q1 in db.IQueryable<CheckRecordEntity>()
                        join q2 in
                            (
                                from q1 in db.IQueryable<CheckItemEntity>()
                                join q2 in db.IQueryable<FileInfoEntity>() on q1.CheckItemId equals q2.RecId into t2
                                select new { q1, q2 = t2 }
                                ) on q1.CheckRecordId equals q2.q1.CheckRecordId into t2
                        where q1.CheckRecordId == id
                        select new { q1, q2 = t2 }).FirstOrDefault();
            foreach (var item in data.q2)
            {
                item.q1.Files = item.q2.ToList();
            }
            data.q1.CheckItems = data.q2.Select(x => x.q1).ToList();
            return data.q1;
        }

        public List<CheckRecordEntity> GetList(string[] deptId, string key, DateTime? from, DateTime? to, int pageSize, int pageIndex, out int total)
        {
            IRepository db = new RepositoryFactory().BaseRepository();

            var query = from q in db.IQueryable<CheckRecordEntity>()
                        select q;

            if (deptId != null && deptId.Length > 0) query = query.Where(x => deptId.Contains(x.DeptId));
            if (!string.IsNullOrEmpty(key)) query = query.Where(x => x.CardName.Contains(key));
            if (from != null) query = query.Where(x => x.CheckTime >= from.Value);
            if (to != null) query = query.Where(x => x.CheckTime <= to);

            total = query.Count();
            var data = query.OrderByDescending(x => x.CheckTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return data;
        }

        public List<CheckRecordEntity> GetList(string[] deptid, string checkuser, string key, DateTime? from, DateTime? to, int pageSize, int pageIndex, out int total)
        {
            IRepository db = new RepositoryFactory().BaseRepository();

            var query = from q1 in db.IQueryable<CheckRecordEntity>()
                        join q2 in db.IQueryable<CheckItemEntity>() on q1.CheckRecordId equals q2.CheckRecordId into into2
                        select new { q1, q2 = into2 };

            if (deptid != null && deptid.Length > 0) query = query.Where(x => deptid.Contains(x.q1.DeptId));
            if (!string.IsNullOrEmpty(checkuser)) query = query.Where(x => x.q1.CheckUser.Contains(checkuser));
            if (!string.IsNullOrEmpty(key)) query = query.Where(x => x.q1.CardName.Contains(key));
            if (from != null) query = query.Where(x => x.q1.CheckTime >= from.Value);
            if (to != null) query = query.Where(x => x.q1.CheckTime <= to);

            total = query.Count();
            var ss = query.OrderByDescending(x => x.q1.CheckTime).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var data = query.OrderByDescending(x => x.q1.CheckTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            foreach (var item in data)
            {
                item.q1.Num1 = item.q2.Sum(x => x.Num1);
                item.q1.Num2 = item.q2.Sum(x => x.Num2 + x.Num3);
            }

            return data.Select(x => x.q1).ToList();
        }

        public List<CheckRecordEntity> GetMine(string userId, int pageSize, int pageIndex, out int total)
        {
            IRepository db = new RepositoryFactory().BaseRepository();

            var query = from q in db.IQueryable<CheckRecordEntity>()
                        where q.CreateUserId == userId
                        orderby q.CreateTime descending
                        select q;

            total = query.Count();
            var data = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return data;
        }

        public void Remove(string id)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            db.Delete<CheckRecordEntity>(id);
            db.Delete<CheckItemEntity>(x => x.CheckRecordId == id);

            db.Commit();

        }

        public void Save(CheckRecordEntity model)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            try
            {
                var entity = (from q in db.IQueryable<CheckRecordEntity>()
                              where q.CheckRecordId == model.CheckRecordId
                              select q).FirstOrDefault();

                foreach (var item in model.CheckItems)
                {
                    var fileentities = (from q in db.IQueryable<FileInfoEntity>()
                                        where q.RecId == item.clientid
                                        select q).ToList();
                    fileentities.ForEach(x => x.RecId = item.CheckItemId);
                    db.Update(fileentities);
                }

                if (entity == null)
                {
                    db.Insert(model);
                    db.Insert(model.CheckItems);
                    if (model.Files != null)
                        db.Insert(model.Files);
                }
                else
                {
                    entity.CardId = model.CardId;
                    entity.CardName = model.CardName;
                    entity.CheckPlace = model.CheckPlace;
                    entity.ModifyTime = model.ModifyTime;
                    entity.ModifyUserId = model.ModifyUserId;
                    entity.CheckUser = model.CheckUser;
                    entity.CheckTime = model.CheckTime;
                    entity.Category = model.Category;
                    entity.CheckPlace = model.CheckPlace;

                    db.Update(entity);

                    var contents = (from q in db.IQueryable<CheckItemEntity>()
                                    where q.CheckRecordId == model.CheckRecordId
                                    select q).ToList();
                    var deleteitems = contents.Where(x => !model.CheckItems.Any(y => y.CheckItemId == x.CheckItemId)).ToList();
                    db.Delete(deleteitems);

                    foreach (var item in model.CheckItems)
                    {
                        var ss = contents.Find(x => x.CheckItemId == item.CheckItemId);
                        if (ss == null) db.Insert(item);
                        else
                        {
                            ss.CheckContentId = item.CheckContentId;
                            ss.CheckContent = item.CheckContent;
                            ss.Num1 = item.Num1;
                            ss.Num2 = item.Num2;
                            ss.Num3 = item.Num3;
                            ss.Num4 = item.Num4;
                            ss.Dangerous = item.Dangerous;
                            ss.Measures = item.Measures;
                            ss.Dangerous2 = item.Dangerous2;
                            ss.Measures2 = item.Measures2;
                            ss.ModifyTime = item.ModifyTime;
                            ss.ModifyUserId = item.ModifyUserId;
                            db.Update(ss);
                        }
                    }
                }

                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
    }
}
