using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.IService.HazardsourceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;

namespace ERCHTMS.Service.HazardsourceManage
{
    /// <summary>
    /// 描 述：危险源清单
    /// </summary>
    public class Hisrelationhd_qdService : RepositoryFactory<Hisrelationhd_qdEntity>, IHisrelationhd_qdService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<Hisrelationhd_qdEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from HSD_HISRELATIONHD_QD where IsDanger=1 " + queryJson).ToList();
        }

        public IEnumerable<Hisrelationhd_qdEntity> GetListForRecord(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from hsd_hisrelationhd where 1=1 " + queryJson).ToList();
        }

        /// <summary>
        /// 根据区域统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetReportForDistrictName(string queryJson)
        {
            return this.BaseRepository().FindTable(" select * from V_HSD_HISRELATIONHD_QD_Report2 where 1=1 " + queryJson);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public Hisrelationhd_qdEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 省级公司统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string StaQueryList(string queryJson)
        {
            string riskGrade = "重大风险,较大风险,一般风险,低风险";//固定等级
            string[] grades = riskGrade.TrimStart(',').Split(',');
            var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            List<object> x = new List<object>();//x轴数据
            List<string> y = new List<string>();//y轴数据
            List<object> list = new List<object>();//列表数据

            List<object[]> pie = new List<object[]>();//饼状图

            object result = new object();

            var queryParam = queryJson.ToJObject();//查询条件

            string sql = string.Empty;
            string sqlWhere = string.Empty;
            string sqlWhere1 = string.Empty;
            string sql1 = string.Empty;

            sql = @"select b.fullname,b.encode,
                            nvl(t.l1,0) l1,nvl(t.l2,0) l2,nvl(t.l3,0) l3,nvl(t.l4,0) l4,
                            nvl((l1+l2+l3+l4),0) total
                          
                            from base_department b
                            left join ({0})t on t.createuserorgcode=b.encode
                             where b.nature='厂级' and  b.deptcode like'{1}%'";
            sql1 = @"select h.createuserorgcode,
                            sum(case when h.gradeval='1' then 1 else 0 end) l1,
                              sum(case when h.gradeval='2' then 1 else 0 end) l2,
                              sum(case when h.gradeval='3' then 1 else 0 end) l3,
                                sum(case when h.gradeval='4' then 1 else 0 end) l4
                             from hsd_hazardsource h where IsDanger=1 {0} group by h.createuserorgcode";
            if (!queryParam["year"].IsEmpty())
            {
                sqlWhere = string.Format(@"  and to_char(h.createdate,'yyyy')='{0}'", queryParam["year"].ToString());
                sqlWhere1 = string.Format(@"  and to_char(createdate,'yyyy')='{0}'", queryParam["year"].ToString());
            }
            sql1 = string.Format(sql1, sqlWhere);
            sql = string.Format(sql, sql1, currUser.NewDeptCode);

            string sql2 = string.Format(@"select count(id) from hsd_hazardsource h 
                                                where h.id not in(select hdid from hsd_jkjc j where j.JkskStatus='1' {0})
                                                and h.createuserorgcode in (select b.encode from base_department b where b.deptcode like'{1}%' and b.nature='厂级')  
                                                and h.isdanger='1'  {0}", sqlWhere1,currUser.NewDeptCode);//未监控危险源数量
            string sql3 = string.Format(@"  select count(id) from hsd_hazardsource h where h.id not in(select hdid from hsd_hdjd d where d.ISDJJD='1' {0})
                                                and h.createuserorgcode in (select b.encode from base_department b where b.deptcode like'{1}%' and b.nature='厂级')  
                                                and h.isdanger='1'
                                                {0}", sqlWhere1,currUser.NewDeptCode);//未登记危险源数量
            string sql4 = string.Format(@"  select count(id) from hsd_jkjc j where j.jkyhzgids>0 
                                                     and j.createuserorgcode in (select b.encode from base_department b where b.deptcode like'{1}%' and b.nature='厂级')  
                                                    {0}", sqlWhere1, currUser.NewDeptCode);//存在隐患的危险源数量
            DataTable dt1 = this.BaseRepository().FindTable(sql2);
            DataTable dt2 = this.BaseRepository().FindTable(sql3);
            DataTable dt3 = this.BaseRepository().FindTable(sql4);
            result = new { wdj = dt2.Rows.Count > 0 ? dt2.Rows[0][0].ToString() : "0", wjk = dt1.Rows.Count > 0 ? dt1.Rows[0][0].ToString() : "0", yyh = dt3.Rows.Count > 0 ? dt3.Rows[0][0].ToString() : "0" };
            DataTable dt = this.BaseRepository().FindTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var totalSum = Convert.ToSingle(dt.Rows[i]["total"].ToString());
                double p1 = 0, p2 = 0, p3 = 0, p4 = 0;
                if (totalSum > 0)
                {
                    p1 = Math.Round((Convert.ToSingle(dt.Rows[i]["l1"].ToString()) / totalSum), 4) * 100;
                    p2 = Math.Round((Convert.ToSingle(dt.Rows[i]["l2"].ToString()) / totalSum), 4) * 100;
                    p3 = Math.Round((Convert.ToSingle(dt.Rows[i]["l3"].ToString()) / totalSum), 4) * 100;
                    p4 = Math.Round((Convert.ToSingle(dt.Rows[i]["l4"].ToString()) / totalSum), 4) * 100;
                }
                else
                {
                    p1 = 0;
                    p2 = 0; p3 = 0; p4 = 0;
                }
                list.Add(new
                {
                    name = dt.Rows[i]["fullname"].ToString(),
                    deptcode = dt.Rows[i]["encode"].ToString(),
                    l1 = Convert.ToInt32(dt.Rows[i]["l1"].ToString()),
                    l2 = Convert.ToInt32(dt.Rows[i]["l2"].ToString()),
                    l3 = Convert.ToInt32(dt.Rows[i]["l3"].ToString()),
                    l4 = Convert.ToInt32(dt.Rows[i]["l4"].ToString()),
                    p1 = p1,
                    p2 = p2,
                    p3 = p3,
                    p4 = p4
                });
            }
            var total = dt.Compute("Sum(total)", "").ToDouble();
            if (total > 0)
            {
                list.Add(new
                {
                    name = "全部",
                    deptcode = currUser.NewDeptCode,
                    l1 = dt.Compute("Sum(l1)", "").ToDouble(),
                    l2 = dt.Compute("Sum(l2)", "").ToDouble(),
                    l3 = dt.Compute("Sum(l3)", "").ToDouble(),
                    l4 = dt.Compute("Sum(l4)", "").ToDouble(),
                    p1 = Math.Round(dt.Compute("Sum(l1)", "").ToDouble() / dt.Compute("Sum(total)", "").ToDouble(), 4) * 100,
                    p2 = Math.Round(dt.Compute("Sum(l2)", "").ToDouble() / dt.Compute("Sum(total)", "").ToDouble(), 4) * 100,
                    p3 = Math.Round(dt.Compute("Sum(l3)", "").ToDouble() / dt.Compute("Sum(total)", "").ToDouble(), 4) * 100,
                    p4 = Math.Round(dt.Compute("Sum(l4)", "").ToDouble() / dt.Compute("Sum(total)", "").ToDouble(), 4) * 100
                });
            }
            else
            {
                list.Add(new
                {
                    name = "全部",
                    deptcode = currUser.NewDeptCode,
                    l1 = dt.Compute("Sum(l1)", "").ToDouble(),
                    l2 = dt.Compute("Sum(l2)", "").ToDouble(),
                    l3 = dt.Compute("Sum(l3)", "").ToDouble(),
                    l4 = dt.Compute("Sum(l4)", "").ToDouble(),
                    p1 = 0,
                    p2 = 0,
                    p3 = 0,
                    p4 = 0
                });
            }
            if (!queryParam["type"].IsEmpty())
            {
                switch (queryParam["type"].ToString())
                {
                    case "Unit":
                        for (int j = 0; j < grades.Length; j++)
                        {
                            List<int> data = new List<int>();

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                y.Add(dt.Rows[i]["fullname"].ToString());
                                var str = "l" + (j + 1);
                                data.Add(Convert.ToInt32(dt.Rows[i][str].ToString()));
                            }
                            x.Add(new { name = grades[j], data = data });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = x, y = y, list = list, result = result });
                    case "Grade":
                        object[] arr = { "重大风险", dt.Compute("Sum(l1)", "").ToDouble() };
                        pie.Add(arr);
                        arr = new object[] { "较大风险", dt.Compute("Sum(l2)", "").ToDouble() };
                        pie.Add(arr);
                        arr = new object[] { "一般风险", dt.Compute("Sum(l3)", "").ToDouble() };
                        pie.Add(arr);
                        arr = new object[] { "低风险", dt.Compute("Sum(l4)", "").ToDouble() };
                        pie.Add(arr);
                        return Newtonsoft.Json.JsonConvert.SerializeObject(new { pie = pie, list = list, result = result });
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
        public void SaveForm(string keyValue, Hisrelationhd_qdEntity entity)
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
