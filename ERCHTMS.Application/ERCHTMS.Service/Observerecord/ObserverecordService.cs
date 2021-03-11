using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Service.BaseManage;


namespace ERCHTMS.Service.Observerecord
{
    /// <summary>
    /// 描 述：观察记录表
    /// </summary>
    public class ObserverecordService : RepositoryFactory<ObserverecordEntity>, ObserverecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["starttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and to_char(t.obsstarttime,'yyyy-mm-dd')>=to_char(to_date('{0}','yyyy-mm-dd'),'yyyy-mm-dd') ", queryParam["starttime"].ToString());
            }
            if (!queryParam["endtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and to_char(t.obsendtime,'yyyy-mm-dd')<=to_char(to_date('{0}','yyyy-mm-dd'),'yyyy-mm-dd') ", queryParam["endtime"].ToString());

            }
            if (!queryParam["txt_Keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.workname like'%{0}%' or t.obsperson like'%{0}%' or t.workarea like '%{0}%')", queryParam["txt_Keyword"].ToString());
            }
            if (!queryParam["workname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.workname like'%{0}%'", queryParam["workname"].ToString());
            }
            if (!queryParam["obspeople"].IsEmpty())
            {
                string obsp = queryParam["obspeople"].ToString();
                var array = obsp.Split(',');
                string str = string.Empty;
                for (int i = 0; i < array.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(array[i]))
                        continue;
                    else {
                        str += string.Format(" t.obspersonid like'%{0}%' or", array[i]);
                    }
                }
                if (!string.IsNullOrWhiteSpace(str)) {
                    str = str.Substring(0,str.Length-2);
                }
                pagination.conditionJson += string.Format(" and ({0})",str);
            }
            if (!queryParam["workunitid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.workunitid ='{0}'", queryParam["workunitid"].ToString());
            }
            if (!queryParam["deptcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.createuserdeptcode like'{0}%'", queryParam["deptcode"].ToString());
            }
            
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ObserverecordEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObserverecordEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        public DataTable GetTable(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取安全行为与不安全行为占比统计
        /// </summary>
        /// <param name="deptCode">单位Code</param>
        /// <param name="year">年度</param>
        /// <param name="month">月度</param>
        /// <returns></returns>
        public string GetSafetyStat(string deptCode, string year = "", string quarter = "",string month="")
        {
            List<object[]> list = new List<object[]>();
            var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string strWhere = string.Empty;
            strWhere = string.Format(" and t.createuserorgcode='{0}'", currUser.OrganizeCode);
            if (!string.IsNullOrWhiteSpace(deptCode))
            {
                strWhere += string.Format(" and t.workunitcode like'{0}%'", deptCode);
            }
            if (!string.IsNullOrWhiteSpace(year))
            {
                strWhere += string.Format(" and to_char(t.obsstarttime,'yyyy')='{0}'", year);
            }
            if (!string.IsNullOrWhiteSpace(quarter))
            {
                switch (quarter)
                {
                    case "1":
                        strWhere += string.Format(" and to_char(t.obsstarttime,'MM') in('01','02','03')");
                        break;
                    case "2":
                        strWhere += string.Format(" and to_char(t.obsstarttime,'MM') in('04','05','06')");
                        break;
                    case "3":
                        strWhere += string.Format(" and to_char(t.obsstarttime,'MM') in('07','08','09')");
                        break;
                    case "4":
                        strWhere += string.Format(" and to_char(t.obsstarttime,'MM') in('10','11','12')");
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(month)) {
                strWhere += string.Format(" and to_char(t.obsstarttime,'MM') ='{0}'",month);
            }
            string sql = string.Format(@"select sum(case when s.issafety=1 then 1 else 0 end) safenum,
                                               sum(case when s.issafety=0 then 1 else 0 end) notsafenum,
                                               s.observetypename,s.observetype,'' as safeper,'' as notsafeper,'' as ideaper
                                         from bis_observesafety s
                                          left join (select d.itemcode,d.itemname,d.itemvalue,d.sortcode from base_dataitemdetail d 
                                          where d.itemid in(select d.itemid from base_dataitem d where d.itemcode='ObsType'))d on d.itemvalue=s.observetype
                                          where s.recordid in(select id from bis_observerecord t where t.iscommit=1 {0})
                                          group by s.observetypename,s.observetype,d.sortcode
                                          order by d.sortcode ", strWhere);
            //sql = string.Format(sql, strWhere);
            DataTable dt = this.BaseRepository().FindTable(sql);
            var totalNum = dt.Compute("Sum(safenum)", "").ToDouble();
            var notTotal = dt.Compute("Sum(notsafenum)", "").ToDouble();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (totalNum > 0)
                {
                    dt.Rows[i]["safeper"] = Math.Round(Convert.ToSingle(dt.Rows[i]["safenum"]) / totalNum, 2);
                }
                else
                {
                    dt.Rows[i]["safeper"] = 0;
                }
                if (notTotal > 0)
                {
                    dt.Rows[i]["notsafeper"] = Math.Round(Convert.ToSingle(dt.Rows[i]["notsafenum"]) / notTotal, 2);
                    dt.Rows[i]["ideaper"] = Math.Round(Convert.ToSingle(dt.Rows[i]["notsafenum"]) / notTotal, 2);
                }
                else
                {
                    dt.Rows[i]["notsafeper"] = 0;
                    dt.Rows[i]["ideaper"] = 0;
                }
            }
            list.Add(new object[] { "安全行为", totalNum });
            list.Add(new object[] { "不安全行为", notTotal });
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { pie = list, table = dt });
        }
        /// <summary>
        /// 获取不安全比例趋势图
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetQsStat(string deptCode, string year = "")
        {
            List<object> list = new List<object>();
            List<string> x = new List<string>();
            List<List<object>> y1 = new List<List<object>>();

            List<object> z = new List<object>();
            List<double> data = new List<double>();
            string sql = string.Empty;
            string sqlWhere = string.Empty;
            string sql1 = string.Empty;
            string sql2 = string.Empty;
            string sql3 = string.Empty;
            string sql4 = string.Empty;

            sql = @"select t.observetypename,{0} from 
                            (select {4} d.itemname observetypename,d.itemvalue observetype
from (select d.itemvalue,d.itemname,d.sortcode from base_dataitemdetail d where d.itemid in (select itemid from base_dataitem t where t.itemcode='ObsType')) d
    left join(select s.observetype,s.observetypename,
                             {1}
                             from bis_observesafety s
                             left join bis_observerecord b on  b.id=s.recordid
                              where s.issafety=0 {3}
                             group by s.observetype,s.observetypename)t on t.observetype=d.itemvalue)t,
                            (select {2}
                            from bis_observerecord b 
                            left join bis_observesafety s on s.recordid=b.id where 1=1 {3})t1
                             group by t.observetypename";
            for (int i = 1; i <= 12; i++)
            {
                x.Add((i) + "月");
                sql1 += string.Format(@"nvl(sum(case
                     when cast(to_char({1}, 'MM') as number) = {0} then
                      1
                     else
                      0
                   end),0){2},", i, "b.obsstarttime", "m" + i);
                sql2 += string.Format(@"nvl(sum(case
                     when cast(to_char({1}, 'MM') as number) = {0} then
                      1
                     else
                      0
                   end),0){2},", i, "b.obsstarttime", "t" + i);
                sql3 += string.Format(@"nvl(sum(case
                     when {0}=0 then 0 else round({3}/{1},4)*100 end),0){2},", "t1.t" + i, "t1.t" + i, "p" + i, "t.m" + i);
                sql4 += string.Format(@"nvl(t.m" + i + ",0) m" + i + ",");
            }
            if (!string.IsNullOrWhiteSpace(deptCode))
            {
                sqlWhere += string.Format(" and  b.workunitcode like'{0}%'", deptCode);
            }
            if (!string.IsNullOrWhiteSpace(year))
            {
                sqlWhere += string.Format(" and  to_char(b.obsstarttime, 'yyyy')='{0}'", year);
            }
            sql = string.Format(sql, sql3.Substring(0, sql3.Length - 1), sql1.Substring(0, sql1.Length - 1), sql2.Substring(0, sql2.Length - 1), sqlWhere, sql4);
            DataTable dt = this.BaseRepository().FindTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                List<object> y = new List<object>();
                y.Add(Convert.ToSingle(dt.Rows[i]["p1"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p2"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p3"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p4"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p5"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p6"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p7"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p8"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p9"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p10"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p11"].ToString()));
                y.Add(Convert.ToSingle(dt.Rows[i]["p12"].ToString()));
                z.Add(dt.Rows[i]["observetypename"].ToString());
                y1.Add(y);

            }
            data.Add(dt.Compute("Sum(p1)", "").ToDouble());
            data.Add(dt.Compute("Sum(p2)", "").ToDouble());
            data.Add(dt.Compute("Sum(p3)", "").ToDouble());
            data.Add(dt.Compute("Sum(p4)", "").ToDouble());
            data.Add(dt.Compute("Sum(p5)", "").ToDouble());
            data.Add(dt.Compute("Sum(p6)", "").ToDouble());
            data.Add(dt.Compute("Sum(p7)", "").ToDouble());
            data.Add(dt.Compute("Sum(p8)", "").ToDouble());
            data.Add(dt.Compute("Sum(p9)", "").ToDouble());
            data.Add(dt.Compute("Sum(p10)", "").ToDouble());
            data.Add(dt.Compute("Sum(p11)", "").ToDouble());
            data.Add(dt.Compute("Sum(p12)", "").ToDouble());

            string tableSql = @"select
                                       {0}
                                  from bis_observesafety s
                                  left join bis_observerecord b
                                    on b.id = s.recordid
                                 where s.issafety = 0 {2}
                                 union all
                               select {1}
                                  from bis_observerecord b
                                  left join bis_observesafety s
                                    on s.recordid = b.id where 1=1 {2}";
            tableSql = string.Format(tableSql, sql1.Substring(0, sql1.Length - 1), sql2.Substring(0, sql2.Length - 1), sqlWhere);
            DataTable dt1 = this.BaseRepository().FindTable(tableSql);

            DataRow dr = dt1.NewRow();
            dr["m1"] = dt.Compute("Sum(p1)", "");
            dr["m2"] = dt.Compute("Sum(p2)", "");
            dr["m3"] = dt.Compute("Sum(p3)", "");
            dr["m4"] = dt.Compute("Sum(p4)", "");
            dr["m5"] = dt.Compute("Sum(p5)", "");
            dr["m6"] = dt.Compute("Sum(p6)", "");
            dr["m7"] = dt.Compute("Sum(p7)", "");
            dr["m8"] = dt.Compute("Sum(p8)", "");
            dr["m9"] = dt.Compute("Sum(p9)", "");
            dr["m10"] = dt.Compute("Sum(p10)", "");
            dr["m11"] = dt.Compute("Sum(p11)", "");
            dr["m12"] = dt.Compute("Sum(p12)", "");

            dt1.Rows.Add(dr);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = x, y = y1, z = z, data = data, table = dt1 });
        }
        /// <summary>
        /// 获取观察分析对比图
        /// </summary>
        /// <param name="deptCode">单位Code</param>
        /// <param name="year">年</param>
        /// <param name="quarter">季度</param>
        /// <param name="month">月度</param>
        /// <param name="issafety">issafety 0 不安全行为 1 安全行为</param>
        /// <returns></returns>
        public string GetUntiDbStat(string deptCode, string issafety ,string year = "", string quarter = "", string month = "")
        {
            var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string strWhere = string.Empty;
            string strYear = string.Empty;
            string strLink = string.Empty;
            if (!string.IsNullOrWhiteSpace(deptCode))
            {
                var dept = new DepartmentService().GetEntityByCode(deptCode);
                if (dept.Nature == "部门")
                {
                    strWhere += string.Format(" and d.nature in('班组','专业','部门') and d.encode like'{0}%'", deptCode);
                    strLink += "b.workunitcode=d.encode";

                }
                else if (dept.Nature == "厂级")
                {
                    strWhere += string.Format(" and d.nature in('部门') and d.encode like'{0}%'", deptCode);
                    strLink += "substr(b.workunitcode, 0, length(d.encode)) = d.encode";
                }
                else
                {
                    strWhere += string.Format(" and d.nature in('" + dept.Nature + "') and d.encode like'{0}%'", deptCode);
                    strLink += "b.workunitcode=d.encode";
                }
            }
            else {
                strWhere += string.Format(" and d.nature in('部门') and d.encode like'{0}%' ", currUser.OrganizeCode);
                strLink += "substr(b.workunitcode, 0, length(d.encode)) = d.encode";
            }
            if (!string.IsNullOrWhiteSpace(year)) {
                strYear = string.Format("and to_char(b.obsstarttime, 'yyyy') = '{0}'", year);
            }
            if (!string.IsNullOrWhiteSpace(quarter))
            {
                switch (quarter)
                {
                    case "1":
                        strYear += string.Format(" and to_char(b.obsstarttime,'MM') in('01','02','03')");
                        break;
                    case "2":
                        strYear += string.Format(" and to_char(b.obsstarttime,'MM') in('04','05','06')");
                        break;
                    case "3":
                        strYear += string.Format(" and to_char(b.obsstarttime,'MM') in('07','08','09')");
                        break;
                    case "4":
                        strYear += string.Format(" and to_char(b.obsstarttime,'MM') in('10','11','12')");
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(month))
            {
                strYear += string.Format(" and to_char(b.obsstarttime, 'MM') = '{0}'", month);
            }
            string sql = string.Format(@"select d.fullname name,nvl(t.total,0) total from base_department d
                                    left join (select count(*) total, d.fullname,d.encode
                                                      from bis_observesafety s
                                                      left join bis_observerecord b on b.id = s.recordid
                                                      inner join (select encode, fullname
                                                                   from base_department d
                                                                  where d.description is null    and d.nature != '承包商'
                                                                    {3}
                                                                    and d.organizeid = '{0}') d
                                                        on {4}
                                                     where s.issafety = {2} and b.iscommit=1 {1} group by d.fullname,d.encode) t on t.encode=d.encode
                    where d.organizeid='{0}'  and d.nature!='承包商' and d.description is null {3}", currUser.OrganizeId, strYear, issafety, strWhere,strLink);
            DataTable dt= this.BaseRepository().FindTable(sql);
            var totalSum = dt.Compute("Sum(total)","").ToInt();
            List<object> dic = new List<object>();
            dic.Add(new { name = "数量", data = dt == null ? new List<string>() : from a in dt.Select() select a.ItemArray[1] });
       
            var jsonData = new
            {
                y = dt == null ? new List<string>() : from a in dt.Select() select a.ItemArray[0],
                x = dic,
                table=dt,
                totalSum = totalSum
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(jsonData);
        }
        public DataTable GetObsTypeData(string keyValue)
        {
            string sql = string.Format("select t.id,t.isallsafety,t.existproblemcode,t.observetype from BIS_OBSERVECATEGORY t where t.recordid='{0}'", keyValue);
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 根据观察计划Id与任务分解Id查询是否进行了观察记录
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="planfjid"></param>
        /// <returns></returns>
        public bool GetObsRecordByPlanIdAndFjId(string planid, string planfjid) {
            string sql = string.Format(@"select count(1) from bis_observerecord t where t.obsplanid like'{0}%' and t.obsplanfjid like'{1}%'", planid, planfjid);
            var dt= this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string sql = string.Format("delete bis_observesafety t where t.recordid='{0}'", keyValue);
                res.ExecuteBySql(sql);
                sql = string.Format("delete bis_observecategory t where t.recordid='{0}'", keyValue);
                res.ExecuteBySql(sql);
                ObserverecordEntity obsentity = res.FindEntity<ObserverecordEntity>(keyValue);
                res.Delete(obsentity);

                res.Commit();
            }
            catch
            {
                res.Rollback();
            }
            //this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ObserverecordEntity entity, List<ObservecategoryEntity> listCategory, List<ObservesafetyEntity> listSafety)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                entity.ID = keyValue;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var newEntity = this.BaseRepository().FindEntity(keyValue);
                    if (newEntity != null)
                    {
                        entity.Modify(keyValue);
                        res.Update<ObserverecordEntity>(entity);
                    }
                    else
                    {
                        entity.Create();
                        res.Insert<ObserverecordEntity>(entity);
                    }

                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                res.ExecuteBySql(string.Format("delete from bis_observecategory t where t.recordid='{0}'", entity.ID));
                res.ExecuteBySql(string.Format("delete from bis_observesafety t where t.recordid='{0}'", entity.ID));
                for (int i = 0; i < listCategory.Count; i++)
                {
                    listCategory[i].Create();
                    listCategory[i].Recordid = entity.ID;
                }
                for (int i = 0; i < listSafety.Count; i++)
                {
                    listSafety[i].Create();
                    listSafety[i].Recordid = entity.ID;
                }
                res.Insert<ObservecategoryEntity>(listCategory);
                res.Insert<ObservesafetyEntity>(listSafety);
                res.Commit();
            }
            catch (System.Exception)
            {

                res.Rollback();
            }

        }
        #endregion
    }
}
