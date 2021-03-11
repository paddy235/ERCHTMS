using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HseManage.ViewModel;
using ERCHTMS.Entity.HseToolMange;
using ERCHTMS.IService.HseToolMange;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.HseObserveManage
{
    public class HseObserveService : RepositoryFactory<HseObserveEntity>, HseObserveIService
    {


        /// <summary>
        /// 获取台账分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HseObserveEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, HseObserveEntity entity)
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
        /// <summary>
        /// 获取参与率
        /// </summary>
        /// <param name="year">要查询的年份</param>
        /// <param name="deptEncode">部门编码</param>
        /// <returns></returns>
        public List<HseKeyValue> GetCYLData(string year, string deptEncode)
        {
            List<HseKeyValue> data = null;
            string sql = @"  select 
                  case to_char(OBSERVEDATE,'mm') 
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
                                    Count(ID) as SUBMITCOUNT,
                                     Count(DISTINCT CREATEUSERID) as SUBMITUSER,
(select (case cycle when '每周' then 0.25 when '每月' then 1 when '每季度' then 3 when '每年' then 12 else 1 end) as cycle from  hse_observersetting 
where settingname = '安全观察卡') as cycle, (select times from  hse_observersetting 
where settingname = '安全观察卡') as times
                                     from hse_securityobserve
                                     WHERE 1=1 ";

            if (!string.IsNullOrWhiteSpace(year) && year != DateTime.Now.Year.ToString())
            {
                sql += " AND  to_char( OBSERVEDATE,'yyyy' )='" + year + "' ";
                data = new HseKeyValue().InitData(1);
            }
            else
            {
                //为空默认当前年
                year = DateTime.Now.Year.ToString();
                sql += " AND  to_char( OBSERVEDATE,'yyyy' )='" + year + "' ";
                data = new HseKeyValue().InitData(2);    //如果为当前年，只查询到当前月的上月（例：现在是五月，只查到6月，生成的数据接收实体有变化，其他的无变化）
            }
            if (!string.IsNullOrWhiteSpace(deptEncode))
            {
                sql += " AND CREATEUSERDEPTCODE LIKE '" + deptEncode + "%'";
            }
            sql += " GROUP BY  to_char(OBSERVEDATE,'mm')";
            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            string sqlByUserCount = " select Count(*) as Count from BASE_USER where departmentCode like '" + deptEncode + "%'";
            decimal userCount = Convert.ToDecimal(new RepositoryFactory<UserEntity>().BaseRepository().FindObject(string.Format(@"
SELECT
	count( 1 ) 
FROM
	base_user a
	INNER JOIN HSE_OBSERVERSETTINGITEM b ON b.DEPTID = a.departmentid
	INNER JOIN HSE_OBSERVERSETTING c ON c.settingid = b.settingid 
WHERE
	a.ispresence = '1' 
	AND c.settingname = '安全观察卡' 
	AND a.departmentcode LIKE '{0}%'
", deptEncode)));//单位的人员数量（应提交人数）
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var obj = data.FirstOrDefault(p => p.Key == dr["Month"].ToString());
                    if (obj != null)
                    {
                        //参与度=(已提交卡总数/总人数*周数)*(实际提交人数/总人数)*100%

                        decimal submitcount = dr["SUBMITCOUNT"] == null ? 0 : Convert.ToDecimal(dr["SUBMITCOUNT"]);    //已提交总数

                        decimal submituser = dr["SUBMITUSER"] == null ? 0 : Convert.ToDecimal(dr["SUBMITUSER"]);    //已提交人数 (重复提交的人只算一次)
                        decimal cycle = dr["CYCLE"] == null ? 0 : Convert.ToDecimal(dr["CYCLE"]);    //
                        decimal times = dr["TIMES"] == null ? 0 : Convert.ToDecimal(dr["TIMES"]);    //
                        if (cycle == 0) cycle = 1;
                        if (times == 0) times = 1;
                        if (userCount > 0)//分母不为0
                        {
                            obj.Value = Math.Round((submitcount / (userCount * times / cycle)) * (submituser / userCount) * 100, 2);//百分比
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// 获取各部门各月份的参与率
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="organizeCode">机构编码</param>
        /// <returns></returns>
        public DataTable GetDeptCYL(string year, string organizeCode)
        {

            string sql = @"select 
                                        CREATEUSERDEPTCODE,
                                        Count(ID) as SUBMITCOUNT,
                                        Count(DISTINCT CREATEUSERID) as SUBMITUSER,
                                        case to_char(OBSERVEDATE,'mm') 
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
                                        end as Month
                                        from hse_securityobserve

                                        WHERE 1=1  AND CREATEUSERORGCODE like '" + organizeCode + "%' ";
            if (!string.IsNullOrWhiteSpace(year))
                sql += "  AND  to_char( OBSERVEDATE,'yyyy' )='" + year + "'";
            else
                sql += "  AND  to_char( OBSERVEDATE,'yyyy' )='" + DateTime.Now.Year.ToString() + "'";
            sql += "   GROUP BY    CREATEUSERDEPTCODE,to_char(OBSERVEDATE, 'mm') ";
            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            return dt;
        }
        /// <summary>
        /// 危险项统计
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public List<HseKeyValue> GetWXXCount(string year, string month)
        {
            string sql = "SELECT CONTENT,COUNT(*) AS COUNT FROM HSE_SECURITYOBSERVE WHERE 1=1 ";
            if (!string.IsNullOrEmpty(year))
            {
                sql += " AND  TO_CHAR( OBSERVEDATE,'YYYY' )='" + year + "'";
            }
            if (!string.IsNullOrEmpty(month))
            {
                if (int.Parse(month) < 10)
                {
                    month = "0" + month;
                }
                sql += " AND  TO_CHAR( OBSERVEDATE,'MM' )='" + month + "' ";
            }
            sql += " AND ( OBSERVETYPE LIKE '%未遂事件%' OR OBSERVETYPE LIKE '%不安全状况%' OR OBSERVETYPE LIKE '%不安全行为%') GROUP BY CONTENT";

            sql = " SELECT CONTENT,SUM(COUNT) as COUNT FROM  (  SELECT REGEXP_SUBSTR(T.CONTENT,'[^,]+',1,l) as content,COUNT FROM( " + sql;
            sql += "  )  T, (select level l from dual connect by level <= 100) b  where l <= length(T.content) - length(replace(T.content, ',')) + 1  )  T2 GROUP BY CONTENT";
            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            List<HseKeyValue> data = new List<HseKeyValue>();
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                var drList = dt.Rows.GetEnumerator();
                while (drList.MoveNext())
                {
                    var dr = drList.Current as DataRow;
                    if (dr["CONTENT"] != null)
                    {
                        HseKeyValue keyValue = new HseKeyValue() { Key = dr["CONTENT"].ToString(), Value = dr["COUNT"] == null ? 0 : Convert.ToInt32(dr["COUNT"]) };
                        data.Add(keyValue);
                    }
                }
            }
            return data;
        }
        /// <summary>
        /// 获取危险项每月数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetWXX(string year)
        {
            if (string.IsNullOrWhiteSpace(year))
            {
                year = DateTime.Now.Year.ToString();
            }
            string sql = string.Format(@" SELECT CONTENT,SUM(COUNT) as COUNT,Month FROM  (  SELECT REGEXP_SUBSTR(T.CONTENT,'[^,]+',1,l) as content,COUNT,Month
 FROM  (select CONTENT,COUNT(*) as COUNT,
 case to_char(OBSERVEDATE,'mm') 
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
                                    end as Month
  from hse_securityobserve where 1=1 AND  to_char( OBSERVEDATE,'yyyy' )='{0}'  AND ( OBSERVETYPE LIKE '%未遂事件%' OR OBSERVETYPE LIKE '%不安全状况%' OR OBSERVETYPE LIKE '%不安全行为%') GROUP BY CONTENT,to_char(OBSERVEDATE,'mm') )  T,
 (select level l from dual connect by level <=100) b
 where l<=length(T.content)-length(replace(T.content,','))+1 )  T2 GROUP BY CONTENT,Month", year);
            DataTable dt = new RepositoryFactory().BaseRepository().FindTable(sql);
            return dt;
        }
    }
}
