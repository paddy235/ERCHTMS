using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using System;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包单位基础信息表
    /// </summary>
    public class OutsourcingprojectService : RepositoryFactory<OutsourcingprojectEntity>, OutsourcingprojectIService
    {
        private IUserInfoService userservice = new UserInfoService();
        private UserScoreIService userscoreservice = new UserScoreService();
        IDataItemDetailService dataitem = new DataItemDetailService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutsourcingprojectEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OutsourcingprojectEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 查询外包单位
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["outprojectname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.outsourcingname like'%{0}%'", queryParam["outprojectname"].ToString());
            }
            //省级统计跳转使用--请勿删除
            if (!queryParam["fullName"].IsEmpty())
            {
                if (queryParam["fullName"].ToString() == "全部")
                {

                }
                else
                {
                    if (!queryParam["orgCode"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(" and b.encode like'%{0}%'", queryParam["orgCode"].ToString());
                    }
                }
            }
          
            if (!queryParam["outorin"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.outorin ='{0}'", queryParam["outorin"].ToString());
            }
            if (!queryParam["Time"].IsEmpty())
            {
                var startTime = DateTime.Parse(queryParam["Time"].ToString());
                var endTime = startTime.AddMonths(1).AddDays(-1);
                pagination.conditionJson += string.Format(" and (to_char(t.outintime,'yyyy-MM-dd')<='{0}' and t.outintime is not null) and (to_char(t.leavetime,'yyyy-MM-dd')>= '{1}' or  t.leavetime is null )", endTime.ToString("yyyy-MM-dd"), startTime.ToString("yyyy-MM-dd"));
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            var csjf = dataitem.GetEntity("csjf");
            var jf = csjf == null ? "100" : csjf.ItemValue;
            dt.Columns.Add("score", typeof(string));
            foreach (DataRow item in dt.Rows)
            {
                if (string.IsNullOrEmpty(item["managerid"].ToString()))
                    item["score"] = "";
                else
                    item["score"] = userscoreservice.GetUserScore(userservice.GetUserInfoByAccount(item["managerid"].ToString()).UserId, "2018") + Convert.ToInt32(jf);
            }

            return dt;
        }
        /// <summary>
        /// 根据外包单位Id获取外包单位基础信息
        /// </summary>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        public OutsourcingprojectEntity GetInfo(string outProjectId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from epg_outsourcingproject where outprojectid='{0}'", outProjectId);
            return new RepositoryFactory().BaseRepository().FindList<OutsourcingprojectEntity>(strSql.ToString()).ToList().FirstOrDefault();
        }


        public string StaQueryList(string queryJson)
        {
            var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            List<object> list = new List<object>();
            List<string> x = new List<string>();
            List<object> y = new List<object>();
            List<int> data = new List<int>();
            List<string> month = new List<string>();

            string sql = string.Empty;
            string sqlWhere = string.Empty;

            string sql1 = string.Empty;

            var queryParam = queryJson.ToJObject();
            if (!queryParam["type"].IsEmpty())
            {
                switch (queryParam["type"].ToString())
                {

                    case "Unit"://单位对比
                        sql = @"select b.fullname,sum(nvl(e.pronum,0)) total,b.encode
                           from base_department b 
                            left join ({0})e on e.createuserorgcode = b.encode
                    where b.nature = '厂级' and b.deptcode like'{1}%'
                    group by b.fullname,b.encode ";
                        switch (queryParam["state"].ToString())
                        {
                            case "Project"://在建工程
                                sql1 = @"select count(id) pronum,e.createuserorgcode 
                                        from epg_outsouringengineer e 
                                        where e.engineerstate = '002' and e.isdeptadd=1 {0} group by e.createuserorgcode";

                                break;
                            case "Unit"://在场单位
                                sql1 = @"select count(id) pronum,e.createuserorgcode
                                         from epg_outsourcingproject e where e.outorin='0' {0}
                                         group by e.createuserorgcode";
                                break;
                            case "Person"://在场人员
                                sql1 = @"select count(UserId) pronum,u.createuserorgcode
                                        from base_user u where u.ispresence='1' and u.isepiboly='1' {0}
                                        group by u.createuserorgcode";
                                break;
                            default:
                                break;
                        }
                        sql1 = string.Format(sql1, sqlWhere);
                        sql = string.Format(sql, sql1,currUser.NewDeptCode);
                        DataTable dt = this.BaseRepository().FindTable(sql);
                        double totalNum = 0;
                        totalNum = dt.Compute("Sum(total)", "").ToDouble();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var total = Convert.ToInt32(dt.Rows[i]["total"]);
                            double per = 0;
                            if (totalNum == 0)
                                per = 0;
                            else
                                per = Math.Round(total / totalNum, 2) * 100;
                            x.Add(dt.Rows[i]["fullname"].ToString());
                            data.Add(total);

                            list.Add(new { name = dt.Rows[i]["fullname"].ToString(), num = total, per = per, deptcode = dt.Rows[i]["encode"].ToString() });

                        }
                        list.Add(new { name = "全部", num = totalNum, per = "100",deptcode =currUser.NewDeptCode });
                        dt.Dispose();
                        y.Add(new { name = "各电厂", data = data });
                        return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = x, y = y, list = list });
                    case "Month"://月度对比
                        sql = @"select d.encode,d.fullname,d.deptcode,
                                   nvl(t.m1, 0)m1,nvl(t.m2, 0)m2, nvl(t.m3, 0)m3,
                                   nvl(t.m4, 0)m4,nvl(t.m5, 0)m5,nvl(t.m6, 0)m6,
                                   nvl(t.m7, 0)m7,nvl(t.m8, 0)m8,nvl(t.m9, 0)m9,
                                   nvl(t.m10, 0)m10,nvl(t.m11, 0)m11,nvl(t.m12, 0)m12
                              from base_department d
                              left join ({0}) t
                                on t.createuserorgcode = d.encode
                             where d.nature = '厂级' and d.deptcode like'{1}%'  ";

                        switch (queryParam["state"].ToString())
                        {
                            case "Project":

                                for (int i = 1; i <= 12; i++)
                                {

                                    sqlWhere += string.Format(@" sum(case
                                     when cast(to_char({3}, 'yyyy') as number) < {0} then
                                      1
                                     when cast(to_char({3}, 'yyyy') as number) = {0} and
                                          cast(to_char({3}, 'MM') as number) <= {1} and
                                          (cast(to_char({4}, 'yyyy') as number)>={0} and cast(to_char({4}, 'MM') as number) >= {1}) or
                                           ({4} is null and cast(to_char(sysdate, 'mm') as number) = '{1}' and cast(to_char(sysdate, 'yyyy') as number) = '{0}') then
                                      1
                                     else
                                      0
                                   end) {2},", Convert.ToInt32(queryParam["year"]), i, "m" + i, "e.planenddate", "e.actualenddate");
                                }
                                sql1 = @"select e.createuserorgcode,
                                            {0}
                                       from epg_outsouringengineer e 
                                       where e.planenddate is not null 
                                      group by e.createuserorgcode ";
                                break;
                            case "Unit":
                                for (int i = 1; i <= 12; i++)
                                {

                                    sqlWhere += string.Format(@" sum(case
                                     when cast(to_char({3}, 'yyyy') as number) < {0} then
                                      1
                                     when cast(to_char({3}, 'yyyy') as number) = {0} and
                                          cast(to_char({3}, 'MM') as number) <= {1} and
                                          (cast(to_char({4}, 'yyyy') as number)>={0} and cast(to_char({4}, 'MM') as number) >= {1}) or
                                           ({4} is null and cast(to_char(sysdate, 'mm') as number) = '{1}' and cast(to_char(sysdate, 'yyyy') as number) = '{0}') then
                                      1
                                     else
                                      0
                                   end) {2},", Convert.ToInt32(queryParam["year"]), i, "m" + i, "e.outintime", "e.leavetime");
                                }
                                sql1 = @"select e.createuserorgcode,
                                            {0}
                                       from epg_outsourcingproject e 
                                   where e.outintime is not null
                                      group by e.createuserorgcode";
                                break;
                            case "Person":
                                for (int i = 1; i <= 12; i++)
                                {

                                    sqlWhere += string.Format(@" sum(case
                                     when cast(to_char({3}, 'yyyy') as number) < {0} then
                                      1
                                     when cast(to_char({3}, 'yyyy') as number) = {0} and
                                          cast(to_char({3}, 'MM') as number) <= {1} and
                                          (cast(to_char({4}, 'yyyy') as number)>={0} and cast(to_char({4}, 'MM') as number) >= {1}) or
                                           ({4} is null and cast(to_char(sysdate, 'mm') as number) = '{1}' and cast(to_char(sysdate, 'yyyy') as number) = '{0}') then
                                      1
                                     else
                                      0
                                   end) {2},", Convert.ToInt32(queryParam["year"]), i, "m" + i, "e.entertime", "e.departuretime");
                                }
                                sql1 = @"select e.createuserorgcode,
                                            {0}
                                       from base_user e 
                                     where e.entertime is not null
                                      group by e.createuserorgcode";
                                break;
                            default:
                                break;
                        }
                        sql1 = string.Format(sql1, sqlWhere.Substring(0, sqlWhere.Length - 1));
                        sql = string.Format(sql, sql1, currUser.NewDeptCode);
                  
                        DataTable dt1 = this.BaseRepository().FindTable(sql);

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            list.Add(new
                            {
                                name = dt1.Rows[i]["fullname"].ToString(),
                                deptcode = dt1.Rows[i]["encode"].ToString(),
                                m1 = dt1.Rows[i]["m1"].ToInt(),
                                m2 = dt1.Rows[i]["m2"].ToInt(),
                                m3 = dt1.Rows[i]["m3"].ToInt(),
                                m4 = dt1.Rows[i]["m4"].ToInt(),
                                m5 = dt1.Rows[i]["m5"].ToInt(),
                                m6 = dt1.Rows[i]["m6"].ToInt(),
                                m7 = dt1.Rows[i]["m7"].ToInt(),
                                m8 = dt1.Rows[i]["m8"].ToInt(),
                                m9 = dt1.Rows[i]["m9"].ToInt(),
                                m10 = dt1.Rows[i]["m10"].ToInt(),
                                m11 = dt1.Rows[i]["m11"].ToInt(),
                                m12 = dt1.Rows[i]["m12"].ToInt()
                            });
                        }


                        if (!queryParam["unit"].IsEmpty())
                        {
                            DataRow[] rows = dt1.Select(string.Format("deptcode='{0}'", queryParam["unit"].ToString()));
                            for (int i = 0; i < rows.Length; i++)
                            {
                                data.Add(Convert.ToInt32(rows[i]["m1"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m2"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m3"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m4"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m5"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m6"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m7"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m8"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m9"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m10"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m11"].ToString()));
                                data.Add(Convert.ToInt32(rows[i]["m12"].ToString()));

                            }
                        }
                        else
                        {
                            //for (int i = 0; i < dt1.Rows.Count; i++)
                            //{
                            data.Add(dt1.Compute("Sum(m1)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m2)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m3)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m4)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m5)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m6)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m7)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m8)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m9)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m10)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m11)", "").ToInt());
                            data.Add(dt1.Compute("Sum(m12)", "").ToInt());
                            //}
                        }
                        for (int i = 0; i < 12; i++)
                        {
                            month.Add((i + 1) + "月");
                        }
                        list.Add(new
                        {
                            name = "全部",
                            deptcode=currUser.NewDeptCode,
                            m1 = dt1.Compute("Sum(m1)", "").ToInt(),
                            m2 = dt1.Compute("Sum(m2)", "").ToInt(),
                            m3 = dt1.Compute("Sum(m3)", "").ToInt(),
                            m4 = dt1.Compute("Sum(m4)", "").ToInt(),
                            m5 = dt1.Compute("Sum(m5)", "").ToInt(),
                            m6 = dt1.Compute("Sum(m6)", "").ToInt(),
                            m7 = dt1.Compute("Sum(m7)", "").ToInt(),
                            m8 = dt1.Compute("Sum(m8)", "").ToInt(),
                            m9 = dt1.Compute("Sum(m9)", "").ToInt(),
                            m10 = dt1.Compute("Sum(m10)", "").ToInt(),
                            m11 = dt1.Compute("Sum(m11)", "").ToInt(),
                            m12 = dt1.Compute("Sum(m12)", "").ToInt()
                        });
                        return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = data, y = month, list = list });
                    default:
                        break;
                }
            }
            return null;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, OutsourcingprojectEntity entity)
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
        #endregion
    }
}
