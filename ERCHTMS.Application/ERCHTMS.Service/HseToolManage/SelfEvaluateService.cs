using ERCHTMS.Entity.HseToolMange;
using ERCHTMS.IService.HseToolMange;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using System;
using BSFramework.Data;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.HseToolManage
{
    public class SelfEvaluateService : RepositoryFactory<SelfEvaluateEntity>, ISelfEvaluateService
    {

        public IEnumerable<SelfEvaluateEntity> GetList(string userid, string deptCode, string keyword, string year, string month)
        {
            var db = this.BaseRepository();
            var query = db.IQueryable();
            if (!string.IsNullOrEmpty(userid))
            {
                query = query.Where(x => x.CreateUserId == userid);
            }
            if (!string.IsNullOrEmpty(deptCode))
            {
                query = query.Where(x => x.DeptCode.StartsWith(deptCode));
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.CreateUser.Contains(keyword));
            }
            if (!string.IsNullOrWhiteSpace(year))
            {
                query = query.Where(p => p.Year == year);
            }
            if (!string.IsNullOrWhiteSpace(month))
            {
                query = query.Where(p => p.Month == month);
            }
            return query.ToList();
        }
        public SelfEvaluateEntity GetEntity(string id)
        {
            var entity = this.BaseRepository().FindEntity(id);
            if (entity != null)
            {
                entity.A = this.GetA(id);
                entity.B = this.GetB(id);
                entity.C = this.GetC(id);
                entity.D = this.GetD(id);
                entity.E = this.GetE(id);
            }
            return entity;
        }
        public EvaluateAEntity GetA(string evaid)
        {
            var db = new RepositoryFactory<EvaluateAEntity>().BaseRepository();
            var query = db.IQueryable();
            query = query.Where(x => x.EvaId == evaid);
            return query.ToList().FirstOrDefault();
        }
        public EvaluateBEntity GetB(string evaid)
        {
            var db = new RepositoryFactory<EvaluateBEntity>().BaseRepository();
            var query = db.IQueryable();
            query = query.Where(x => x.EvaId == evaid);
            return query.ToList().FirstOrDefault();
        }
        public EvaluateCEntity GetC(string evaid)
        {
            var db = new RepositoryFactory<EvaluateCEntity>().BaseRepository();
            var query = db.IQueryable();
            query = query.Where(x => x.EvaId == evaid);
            return query.ToList().FirstOrDefault();
        }
        public EvaluateDEntity GetD(string evaid)
        {
            var db = new RepositoryFactory<EvaluateDEntity>().BaseRepository();
            var query = db.IQueryable();
            query = query.Where(x => x.EvaId == evaid);
            return query.ToList().FirstOrDefault();
        }
        public EvaluateEEntity GetE(string evaid)
        {
            var db = new RepositoryFactory<EvaluateEEntity>().BaseRepository();
            var query = db.IQueryable();
            query = query.Where(x => x.EvaId == evaid);
            return query.ToList().FirstOrDefault();
        }
        public void SaveForm(SelfEvaluateEntity entity)
        {
            var db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {


                var entity1 = db.IQueryable<SelfEvaluateEntity>(p => p.Id == entity.Id).Count() < 1;
                if (entity1)
                {
                    entity.CreateDate = DateTime.Now;
                    entity.Year = DateTime.Now.Year.ToString();
                    entity.Month = DateTime.Now.Month.ToString();
                    db.Insert(entity);
                    this.SaveA(entity.A, db);
                    this.SaveB(entity.B, db);
                    this.SaveC(entity.C, db);
                    this.SaveD(entity.D, db);
                    this.SaveE(entity.E, db);
                }
                else
                {
                    this.SaveA(entity.A, db);
                    this.SaveB(entity.B, db);
                    this.SaveC(entity.C, db);
                    this.SaveD(entity.D, db);
                    this.SaveE(entity.E, db);
                    entity.A = null;
                    entity.B = null;
                    entity.C = null;
                    entity.D = null;
                    entity.E = null;
                    var entityMode = db.FindEntity<SelfEvaluateEntity>(p => p.Id == entity.Id);
                    entityMode.Year = entityMode.CreateDate.Value.Year.ToString();
                    entityMode.Month = entityMode.CreateDate.Value.Month.ToString();
                    entityMode.IsSubmit = entity.IsSubmit;
                    entityMode.IsFill = entity.IsFill;
                    entityMode.Summary = entity.Summary;
                    if (entity.IsSubmit == "1")
                    {
                        entityMode.SubmitDate = DateTime.Now;
                    }
                    db.Update(entityMode);
                    // db.Update(entity);
                }

                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        public void SaveSummary(EvaluateGroupSummaryEntity entity)
        {
            var db = new RepositoryFactory<EvaluateGroupSummaryEntity>().BaseRepository();
            var entity1 = db.FindEntity(entity.Id);
            if (entity1 == null)
            {
                db.Insert(entity);
            }
            else
            {
                entity1.Id = entity.Id;
                entity1.DeptId = entity.DeptId;
                entity1.Content = entity.Content;
                entity1.Year = entity.Year;
                entity1.Month = entity.Month;
                db.Update(entity1);
            }
        }
        public EvaluateGroupSummaryEntity GetSummary(string year, string month)
        {
            var db = new RepositoryFactory<EvaluateGroupSummaryEntity>().BaseRepository();
            var query = db.IQueryable();
            query = query.Where(x => x.Year == year && x.Month == month);
            return query.ToList().FirstOrDefault();
        }
        /// <summary>
        /// 根据主键查找小结
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public EvaluateGroupSummaryEntity GetSummaryById(string id)
        {
            var db = new RepositoryFactory<EvaluateGroupSummaryEntity>().BaseRepository();
            return db.FindEntity(id);
        }
        public void SaveA(EvaluateAEntity entity, IRepository db)
        {

            var entity1 = db.IQueryable<EvaluateAEntity>(p => p.Id == entity.Id).Count() < 1;
            if (entity1)
            {
                db.Insert(entity);
            }
            else
            {
                db.Update(entity);
            }

        }
        public void SaveB(EvaluateBEntity entity, IRepository db)
        {

            var entity1 = db.IQueryable<EvaluateBEntity>(p => p.Id == entity.Id).Count() < 1;
            if (entity1)
            {
                db.Insert(entity);
            }
            else
            {
                db.Update(entity);
            }

        }
        public void SaveC(EvaluateCEntity entity, IRepository db)
        {

            var entity1 = db.IQueryable<EvaluateCEntity>(p => p.Id == entity.Id).Count() < 1;
            if (entity1)
            {
                db.Insert(entity);
            }
            else
            {
                db.Update(entity);
            }

        }
        public void SaveD(EvaluateDEntity entity, IRepository db)
        {

            var entity1 = db.IQueryable<EvaluateDEntity>(p => p.Id == entity.Id).Count() < 1;
            if (entity1)
            {
                db.Insert(entity);
            }
            else
            {
                db.Update(entity);
            }

        }
        public void SaveE(EvaluateEEntity entity, IRepository db)
        {

            var entity1 = db.IQueryable<EvaluateEEntity>(p => p.Id == entity.Id).Count() < 1;
            if (entity1)
            {
                db.Insert(entity);
            }
            else
            {
                db.Update(entity);
            }

        }
        public void RemoveForm(string id)
        {
            var db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                var entity = db.FindEntity<SelfEvaluateEntity>(id);
                var Aentity = db.FindEntity<EvaluateAEntity>(x => x.EvaId == id);
                var Bentity = db.FindEntity<EvaluateBEntity>(x => x.EvaId == id);
                var Centity = db.FindEntity<EvaluateCEntity>(x => x.EvaId == id);
                var Dentity = db.FindEntity<EvaluateDEntity>(x => x.EvaId == id);
                var Eentity = db.FindEntity<EvaluateEEntity>(x => x.EvaId == id);
                db.Delete(Aentity);
                db.Delete(Bentity);
                db.Delete(Centity);
                db.Delete(Dentity);
                db.Delete(Eentity);
                db.Delete(entity);
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }

        #region 统计
        /// <summary>
        /// 获取各部门的自我评价参与度
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="organizeCode">组织编码</param>
        /// <returns></returns>
        public DataTable GetChartsData(string year, string month, string organizeCode)
        {
            string sql = @"select  DEPTCODE,COUNT(ID) AS COUNT, COUNT(DISTINCT CREATEUSERID) AS USERCOUNT  from HSE_SELFEVALUATE 
                            WHERE 1=1  AND ISFILL='1' ";
            if (!string.IsNullOrEmpty(year))
            {
                sql += " AND TO_CHAR(CREATEDATE,'YYYY')='" + year + "' ";
            }
            if (!string.IsNullOrWhiteSpace(month))
            {
                int intMonth = 0;
                if (int.TryParse(month, out intMonth))
                {
                    if (intMonth < 10)
                    {
                        sql += " AND TO_CHAR(CREATEDATE,'MM')= '0" + month.ToString() + "' ";//月份小于十，前面补0
                    }
                    else
                    {
                        sql += " AND TO_CHAR(CREATEDATE,'MM')= '" + month.ToString() + "' ";
                    }
                }
            }
            sql += " GROUP BY DEPTCODE";
            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            return dt;
        }
        /// <summary>
        /// 查询已提交的用户Id
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<string> GetSubmitByDeptCode(string deptCode, string year, string month)
        {
            string sql = string.Format(@"select DISTINCT CREATEUSERID from HSE_SELFEVALUATE where 1=1 AND deptcode like '{0}%' ", deptCode, year, month);
            if (!string.IsNullOrWhiteSpace(year))
            {
                sql += "and to_char(createdate,'yyyy')='" + year + "'";
            }
            if (!string.IsNullOrWhiteSpace(month))
            {
                int intMonth = 0;
                if (int.TryParse(month, out intMonth))
                {
                    if (intMonth < 10)
                    {
                        sql += " AND TO_CHAR(CREATEDATE,'MM')= '0" + month.ToString() + "' ";//月份小于十，前面补0
                    }
                    else
                    {
                        sql += " AND TO_CHAR(CREATEDATE,'MM')= '" + month.ToString() + "' ";
                    }
                }
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            List<string> data = new List<string>();
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                var drEnumerator = dt.Rows.GetEnumerator();
                while (drEnumerator.MoveNext())
                {
                    DataRow dr = drEnumerator.Current as DataRow;
                    if (dr["CREATEUSERID"] != null)
                    {
                        data.Add(dr["CREATEUSERID"].ToString());
                    }
                }
            }
            return data;
        }
        /// <summary>
        /// 获取安全危害的种类与人次
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable GetDangerCount(string deptCode, string year, string month)
        {
            int intMonth = 0;
            if (int.TryParse(month, out intMonth))
            {
                if (intMonth < 10)
                {
                    month = "0" + month;
                }
            }

            string sql = string.Format(@"SELECT DANGER,
                                                                    SUM(COUNT) as COUNT
                                                                     FROM  (  SELECT REGEXP_SUBSTR(T.DANGER,'[^,]+',1,l) as DANGER,COUNT FROM  
                                                                                   (select DANGER,COUNT(*) AS COUNT  from HSE_EVALUATEA A LEFT JOIN HSE_SELFEVALUATE B ON A.EVAID=B.ID WHERE B.DEPTCODE LIKE '{0}%' AND TO_CHAR(CREATEDATE,'YYYY-MM')='{1}-{2}' GROUP BY DANGER )  T,
                                                                                   (select level l from dual connect by level <=100) b
                                                                     where l<=length(T.DANGER)-length(replace(T.DANGER,','))+1 )  T2 
                                                                     GROUP BY DANGER", deptCode, year, month);

            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            return dt;
        }
        /// <summary>
        /// 获取PPE的种类与人次
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable GetPPECount(string deptCode, string year, string month)
        {
            int intMonth = 0;
            if (int.TryParse(month, out intMonth))
            {
                if (intMonth < 10)
                {
                    month = "0" + month;
                }
            }
            string sql = string.Format(@" SELECT USEPPE,
            SUM(COUNT) as COUNT
             FROM  (  SELECT REGEXP_SUBSTR(T.USEPPE,'[^,]+',1,l) as USEPPE,COUNT FROM  
                           (select USEPPE,COUNT(*) AS COUNT  from HSE_EVALUATEA A LEFT JOIN HSE_SELFEVALUATE B ON A.EVAID=B.ID WHERE B.DEPTCODE LIKE '{0}%' AND TO_CHAR(CREATEDATE,'YYYY-MM')='{1}-{2}' GROUP BY USEPPE )  T,
                           (select level l from dual connect by level <=100) b
             where l<=length(T.USEPPE)-length(replace(T.USEPPE,','))+1 )  T2 
             GROUP BY USEPPE", deptCode, year, month);

            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            return dt;
        }
        /// <summary>
        /// 	HSE培训与授权
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable GetHseCount(string deptCode, string year, string month)
        {
            int intMonth = 0;
            if (int.TryParse(month, out intMonth))
            {
                if (intMonth < 10)
                {
                    month = "0" + month;
                }
            }
            string sql = string.Format(@"select
                                                    DGPX,QZDZPX, CNJDCPX,YLRQ, GLSPX,GLZYPX,DHZYPX,JSJZYPX,GKZYPX,JJPX,NONEPX
                                                    from HSE_EVALUATEB A LEFT JOIN HSE_SELFEVALUATE B ON A.EVAID=B.ID WHERE B.DEPTCODE LIKE '{0}%' AND TO_CHAR(CREATEDATE,'YYYY-MM')='{1}-{2}'", deptCode, year, month);
            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            return dt;
        }
        /// <summary>
        /// 安全参与
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable GetSafeCount(string deptCode, string year, string month)
        {
            int intMonth = 0;
            if (int.TryParse(month, out intMonth))
            {
                if (intMonth < 10)
                {
                    month = "0" + month;
                }
            }
            string sql = string.Format(@"select AQGCK,LXZBK,AQHY,ZYAQJD,AQJC,AQPX
                                                        from HSE_EVALUATEC A LEFT JOIN HSE_SELFEVALUATE B ON A.EVAID=B.ID  WHERE B.DEPTCODE LIKE '{0}%' AND TO_CHAR(CREATEDATE,'YYYY-MM')='{1}-{2}'", deptCode, year, month);
            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            return dt;
        }
        /// <summary>
        /// 工余安健环
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="type">1 交通，2用电，3防火，4体力操作，5其他</param>
        /// <returns></returns>
        public DataTable GetFiveData(string deptCode, string year, string month, int type)
        {
            string columnName = string.Empty;
            int intMonth = 0;
            if (int.TryParse(month, out intMonth))
            {
                if (intMonth < 10)
                {
                    month = "0" + month;
                }
            }
            switch (type)
            {
                case 1:
                    columnName = "TRAFFIC";
                    break;
                case 2:
                    columnName = "ELECTRICITY";
                    break;
                case 3:
                    columnName = "FIRE";
                    break;
                case 4:
                    columnName = "POWER";
                    break;
                case 5:
                    columnName = "OTHER";
                    break;
                default:
                    return new DataTable();
            }
            string sql = string.Format(@" SELECT {0} KEY,
            SUM(COUNT) as COUNT
             FROM  (  SELECT REGEXP_SUBSTR(T.{0},'[^,]+',1,l) as {0},COUNT FROM  
                           (select {0},COUNT(*) AS COUNT  from HSE_EVALUATEE A LEFT JOIN HSE_SELFEVALUATE B ON A.EVAID=B.ID WHERE  B.DEPTCODE LIKE '{1}%' AND TO_CHAR(CREATEDATE,'YYYY-MM')='{2}-{3}'  GROUP BY {0} )  T,
                           (select level l from dual connect by level <=100) b
             where l<=length(T.{0})-length(replace(T.{0},','))+1 )  T2 
             GROUP BY {0}", columnName, deptCode, year, month);
            return new RepositoryFactory().BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取班组的评估小结
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public EvaluateGroupSummaryEntity GetSummary(string year, string month, string deptId)
        {
            var db = new RepositoryFactory<EvaluateGroupSummaryEntity>().BaseRepository();
            var data = db.FindEntity(x => x.Year == year && x.Month == month && x.DeptId == deptId);
            return data;
        }

        public decimal GetPeopleCount(string departmentId)
        {
            var db = new RepositoryFactory<EvaluateGroupSummaryEntity>().BaseRepository();

            var sql = @"SELECT
	count( 1 ) 
FROM
	base_user a
	INNER JOIN HSE_OBSERVERSETTINGITEM b ON b.DEPTID = a.departmentid
	INNER JOIN HSE_OBSERVERSETTING c ON c.settingid = b.settingid 
WHERE
	a.ispresence = '1' 
	AND c.settingname = 'HSE自我评估' 
	AND a.departmentcode LIKE '' || ( SELECT encode FROM base_department WHERE departmentid = '{0}' ) || '%'";

            return Convert.ToDecimal(db.FindObject(string.Format(sql, departmentId)));
        }

        public decimal GetCycle(string category)
        {
            var db = new RepositoryFactory<EvaluateGroupSummaryEntity>().BaseRepository();

            var sql = @"select (case cycle when '每周' then 0.25 when '每月' then 1 when '每季度' then 3 when '每年' then 12 else 1 end) as cycle from  hse_observersetting 
where settingname = '{0}'";

            return Convert.ToDecimal(db.FindObject(string.Format(sql, category)));
        }

        public decimal Times(string category)
        {
            var db = new RepositoryFactory<EvaluateGroupSummaryEntity>().BaseRepository();

            var sql = @"select times from  hse_observersetting 
where settingname = '{0}'";

            return Convert.ToDecimal(db.FindObject(string.Format(sql, category)));
        }
        #endregion
    }
}
