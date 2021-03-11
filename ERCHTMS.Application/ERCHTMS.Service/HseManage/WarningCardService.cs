using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using ERCHTMS.Entity.HseManage;
using System;
using ERCHTMS.IService.HseManage;
using System.Data;
using ERCHTMS.Entity.HseManage.ViewModel;

namespace ERCHTMS.Service.HseManage
{
    /// <summary>
    /// 描 述：劳动防护预警表
    /// </summary>
    public class WarningCardService : IWarningCardService
    {
        #region 统计分析
        /// <summary>
        /// 获取安全比趋势图
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public List<HseKeyValue> GetAQBData(string year, string deptId)
        {
            var db = new RepositoryFactory().BaseRepository();
            string yearWhere = string.Empty;
            if (!string.IsNullOrEmpty(year))
            {
                yearWhere = "  to_char( b.CREATETIME,'yyyy' )='" + year + "' ";
            }
            string deptWhere = string.Empty;
            if (!string.IsNullOrEmpty(deptId))
            {
                //如果部门ID不是空的，查询本部门以及下属部门
                string deptSql = string.Format("select DepartmentId from base_department where  departmentid='{0}' or encode like ''|| (select encode from base_department where  departmentid='{0}')  ||'%'", deptId);

                var dtIds = db.FindTable(deptSql);
                var deptIds = new List<string>();
                if (dtIds.Rows != null && dtIds.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIds.Rows)
                    {
                        deptIds.Add("'" + dr[0].ToString() + "'");
                    }

                }
                deptWhere = " B.deptid in (" + string.Join(",", deptIds) + ")";
            }
            #region 废弃
            //            select
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where  to_char(B.checktime, 'mm') = '01') as 一月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '02') as 二月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '03') as 三月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '04') as 四月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '05') as 五月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '06') as 六月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '07') as 七月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '08') as 八月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '09') as 九月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '10') as 十月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '11') as 十一月份,
            //(select ROUND((SUM(A.num1) + SUM(A.num4)) / (SUM(A.num1) + SUM(A.num2) + SUM(A.num3) + SUM(A.num4)), 2)  from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid = B.checkrecordid where to_char(B.checktime,'mm')= '12') as 十二月份
            //from dual
            #endregion
            string sql = @"select   case to_char(B.CREATETIME,'mm') 
                                    when '01' then '一月份'
                                    when '02' then '二月份'
                                    when '03' then '三月份'
                                    when '04' then '四月份'
                                    when '05' then '五月份'
                                    when '06' then '六月份'
                                    when '07' then '七月份'
                                    when '08' then '八月份'
                                    when '09' then '九月份'
                                    when '10' then '十月份'
                                    when '11' then '十一月份'                                                                                                                                        
                                    when '12' then '十二月份' 
                                      else '其他' 
                                    end as Month,
                                    ROUND((SUM(A.num1)+SUM(A.num4))/(SUM(A.num1)+SUM(A.num2)+SUM(A.num3)+SUM(A.num4))*100,2) as AQB
                         from hse_checkitem A LEFT JOIN hse_checkrecord B ON A.checkrecordid=B.checkrecordid  ";
            if (!string.IsNullOrEmpty(yearWhere))
            {
                sql += " where " + yearWhere;
            }
            if (!string.IsNullOrEmpty(deptWhere))
            {
                if (!string.IsNullOrWhiteSpace(yearWhere))
                {
                    sql += " AND " + deptWhere;
                }
                else
                {
                    sql += " where " + deptWhere;
                }
            }

            sql += " group by  to_char(B.CREATETIME,'mm')";
            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            List<HseKeyValue> listObj = new HseKeyValue().InitData(1);
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var obj = listObj.FirstOrDefault(p => p.Key == dr["Month"].ToString());
                    if (obj != null)
                    {
                        obj.Value = dr["AQB"] == null ? 0 : Convert.ToDecimal(dr["AQB"]);
                    }
                    //switch (dr["Month"].ToString())
                    //{
                    //    case "一月份":

                    //        break;
                    //    case "二月份":
                    //        break;
                    //    case "三月份":
                    //        break;
                    //    case "四月份":
                    //        break;
                    //    case "五月份":
                    //        break;
                    //    case "六月份":
                    //        break;
                    //    case "七月份":
                    //        break;
                    //    case "八月份":
                    //        break;
                    //    case "九月份":
                    //        break;
                    //    case "十月份":
                    //        break;
                    //    case "十一月份":
                    //        break;
                    //    case "十二月份":
                    //        break;

                    //}
                }

            }
            return listObj;
        }
        /// <summary>
        /// 预警指标卡各部门提交数据统计
        /// </summary>
        /// <param name="deptIdList">要统计的部门的ID</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public List<HseKeyValue> GetWarningCardCount(List<string> deptIdList, string start, string end)
        {
            List<HseKeyValue> data = new List<HseKeyValue>();
            var db = new RepositoryFactory().BaseRepository();
            string sql = @"   SELECT B.DEPTID,
                                        COUNT(DISTINCT B.CHECKRECORDID) AS ALLCOUNT,
                                        SUM(C.NUM1)+SUM(C.NUM4) AS SAFETYCOUNT,
                                        SUM(C.NUM2)+SUM(C.NUM3) RISKCOUNT 
                                        FROM  HSE_CHECKRECORD B  LEFT JOIN  HSE_CHECKITEM  C ON B.CHECKRECORDID=C.CHECKRECORDID WHERE 1=1 ";
            if (!string.IsNullOrWhiteSpace(start))
            {
                sql += " AND B.CREATETIME>=to_date('" + start + "','yyyy-MM-dd') ";
            }
            if (!string.IsNullOrWhiteSpace(end))
            {
                sql += " AND B.CREATETIME<=to_date('" + end + "','yyyy-MM-dd') ";
            }
            if (deptIdList != null && deptIdList.Count > 0)
            {
                List<string> deptsStr = new List<string>();
                deptIdList.ForEach(x =>
                {
                    deptsStr.Add("'" + x + "'");
                });
                sql += " AND B.DEPTID in (" + string.Join(",", deptsStr) + ")";
            }
            sql += " group by B.deptid";

            DataTable dt = db.FindTable(sql);
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    HseKeyValue keyValue = new HseKeyValue()
                    {
                        DeptId = dr["DEPTID"] == DBNull.Value ? "" : dr["DEPTID"].ToString(),
                        Num1 = dr["ALLCOUNT"] == DBNull.Value ? 0 : Convert.ToInt64(dr["ALLCOUNT"]),
                        Num3 = dr["SAFETYCOUNT"] == DBNull.Value ? 0 : Convert.ToInt64(dr["SAFETYCOUNT"]),
                        Num4 = dr["RISKCOUNT"] == DBNull.Value ? 0 : Convert.ToInt64(dr["RISKCOUNT"]),
                    };
                    data.Add(keyValue);
                }
            }
            return data;
        }
        /// <summary>
        /// 参与度趋势统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public List<HseKeyValue> GetCYDData(string year, string deptId)
        {
            var db = new RepositoryFactory().BaseRepository();
            string yearWhere = string.Empty;

            string deptWhere = string.Empty;

            string sql = string.Format(@" select 
                  case to_char(createtime,'mm') 
                                    when '01' then '一月份'
                                    when '02' then '二月份'
                                    when '03' then '三月份'
                                    when '04' then '四月份'
                                    when '05' then '五月份'
                                    when '06' then '六月份'
                                    when '07' then '七月份'
                                    when '08' then '八月份'
                                    when '09' then '九月份'
                                    when '10' then '十月份'
                                    when '11' then '十一月份'                                                                                                                                        
                                    when '12' then '十二月份' 
                                      else '其他' 
                                    end as Month,
                                    count(1) as COUNT,
(SELECT
	count( 1 ) 
FROM
	base_user a
	INNER JOIN HSE_OBSERVERSETTINGITEM b ON b.DEPTID = a.departmentid
	INNER JOIN HSE_OBSERVERSETTING c ON c.settingid = b.settingid 
WHERE
	a.ispresence = '1' 
	AND c.settingname = '预警指标卡' 
	AND a.departmentcode LIKE '' || ( SELECT encode FROM base_department WHERE departmentid = '{0}' ) || '%') as USERCOUNT,
                                     count(distinct createuserid) as SUBMITCOUNT, 
(select (case cycle when '每周' then 0.25 when '每月' then 1 when '每季度' then 3 when '每年' then 12 else 1 end) as cycle from  hse_observersetting 
where settingname = '预警指标卡') as cycle, (select times from  hse_observersetting 
where settingname = '预警指标卡') as times
                   from HSE_CHECKRECORD  WHERE 1=1 ", deptId);
            // COUNT 总提交数 USERCOUNT应提交总人数 SUBMITCOUNT已提交的人数Month月份
            if (!string.IsNullOrEmpty(year))
            {
                sql += " AND  to_char( createtime,'yyyy' )='" + year + "' ";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                //如果部门ID不是空的，查询本部门以及下属部门
                string deptSql = string.Format("select DepartmentId from base_department where  departmentid='{0}' or encode like ''|| (select encode from base_department where  departmentid='{0}')  ||'%'", deptId);

                var dtIds = db.FindTable(deptSql);
                var deptIds = new List<string>();
                if (dtIds.Rows != null && dtIds.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIds.Rows)
                    {
                        deptIds.Add("'" + dr[0].ToString() + "'");
                    }

                }
                sql += " and deptid in (" + string.Join(",", deptIds) + ")";
            }
            sql += " group by  to_char(createtime,'mm')";
            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            List<HseKeyValue> listObj = new HseKeyValue().InitData(1);
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var obj = listObj.FirstOrDefault(p => p.Key == dr["Month"].ToString());
                    if (obj != null)
                    {
                        //参与度=(已提交卡总数/总人数*周数)*(实际提交人数/总人数)*100%

                        decimal count = dr["COUNT"] == null ? 0 : Convert.ToDecimal(dr["COUNT"]);    //已提交总数
                        decimal userCount = dr["USERCOUNT"] == null ? 0 : Convert.ToDecimal(dr["USERCOUNT"]);    //总人数
                        decimal submitcount = dr["SUBMITCOUNT"] == null ? 0 : Convert.ToDecimal(dr["SUBMITCOUNT"]);    //已提交人数 (重复提交的人只算一次)
                        decimal cycle = dr["CYCLE"] == null ? 0 : Convert.ToDecimal(dr["CYCLE"]);
                        decimal times = dr["TIMES"] == null ? 0 : Convert.ToDecimal(dr["TIMES"]);
                        if (cycle == 0) cycle = 1;
                        if (times == 0) times = 1;
                        if (userCount > 0)//分母不为0
                        {
                            //obj.Value = Math.Round((count / (userCount * 4)) * (submitcount / userCount) * 100, 2);//百分比
                            obj.Value = Math.Round((count / (userCount * times / cycle)) * (submitcount / userCount) * 100, 2);//百分比
                        }
                    }
                }

            }
            return listObj;
        }
        #endregion
        public List<WarningCardEntity> GetData(string key, int pagesize, int pageindex, out int total)
        {
            IRepository db = new RepositoryFactory().BaseRepository();

            var query = from q in db.IQueryable<WarningCardEntity>()
                        select q;

            if (!string.IsNullOrEmpty(key)) query = query.Where(x => x.CardName.Contains(key));

            total = query.Count();
            return query.OrderByDescending(x => x.SubmitTime).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
        }

        public WarningCardEntity GetDetail(string id)
        {
            IRepository db = new RepositoryFactory().BaseRepository();

            var data = (from q1 in db.IQueryable<WarningCardEntity>()
                        join q2 in db.IQueryable<CheckContentEntity>() on q1.CardId equals q2.CardId into t2
                        where q1.CardId == id
                        select new { q1, q2 = t2 }).FirstOrDefault();
            data.q1.CheckContents = data.q2.ToList();
            return data.q1;
        }

        public List<WarningCardEntity> GetList(string[] deptId, string key, DateTime? from, DateTime? to, int pageSize, int pageIndex, out int total)
        {
            IRepository db = new RepositoryFactory().BaseRepository();

            var query = from q in db.IQueryable<WarningCardEntity>()
                        select q;

            if (deptId != null && deptId.Length > 0) query = query.Where(x => deptId.Contains(x.DeptId));
            if (!string.IsNullOrEmpty(key)) query = query.Where(x => x.CardName.Contains(key));
            if (from != null) query = query.Where(x => x.SubmitTime >= from.Value);
            if (to != null) query = query.Where(x => x.SubmitTime <= to);

            total = query.Count();
            var data = query.OrderByDescending(x => x.SubmitTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return data;
        }

        public List<WarningCardEntity> GetMine(string userId, int pageSize, int pageIndex, out int total)
        {
            IRepository db = new RepositoryFactory().BaseRepository();

            var query = from q in db.IQueryable<WarningCardEntity>()
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

            db.Delete<WarningCardEntity>(id);
            db.Delete<CheckContentEntity>(x => x.CardId == id);

            db.Commit();

        }

        public void Save(WarningCardEntity model)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            try
            {
                var entity = (from q in db.IQueryable<WarningCardEntity>()
                              where q.CardId == model.CardId
                              select q).FirstOrDefault();

                if (entity == null)
                {
                    db.Insert(model);
                    db.Insert(model.CheckContents);
                }
                else
                {
                    entity.CardName = model.CardName;
                    entity.Category = model.Category;
                    entity.ModifyTime = model.ModifyTime;
                    entity.ModifyUserId = model.ModifyUserId;

                    db.Update(entity);

                    var contents = (from q in db.IQueryable<CheckContentEntity>()
                                    where q.CardId == model.CardId
                                    select q).ToList();
                    var deleteitems = contents.Where(x => !model.CheckContents.Any(y => y.CheckContentId == x.CheckContentId)).ToList();
                    db.Delete(deleteitems);

                    foreach (var item in model.CheckContents)
                    {
                        var ss = contents.Find(x => x.CheckContentId == item.CheckContentId);
                        if (ss == null) db.Insert(item);
                        else
                        {
                            ss.Content = item.Content;
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
