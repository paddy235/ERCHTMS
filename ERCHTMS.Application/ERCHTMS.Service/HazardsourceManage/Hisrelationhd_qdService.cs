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
    /// �� ����Σ��Դ�嵥
    /// </summary>
    public class Hisrelationhd_qdService : RepositoryFactory<Hisrelationhd_qdEntity>, IHisrelationhd_qdService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<Hisrelationhd_qdEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from HSD_HISRELATIONHD_QD where IsDanger=1 " + queryJson).ToList();
        }

        public IEnumerable<Hisrelationhd_qdEntity> GetListForRecord(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from hsd_hisrelationhd where 1=1 " + queryJson).ToList();
        }

        /// <summary>
        /// ��������ͳ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetReportForDistrictName(string queryJson)
        {
            return this.BaseRepository().FindTable(" select * from V_HSD_HISRELATIONHD_QD_Report2 where 1=1 " + queryJson);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public Hisrelationhd_qdEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ʡ����˾ͳ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string StaQueryList(string queryJson)
        {
            string riskGrade = "�ش����,�ϴ����,һ�����,�ͷ���";//�̶��ȼ�
            string[] grades = riskGrade.TrimStart(',').Split(',');
            var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            List<object> x = new List<object>();//x������
            List<string> y = new List<string>();//y������
            List<object> list = new List<object>();//�б�����

            List<object[]> pie = new List<object[]>();//��״ͼ

            object result = new object();

            var queryParam = queryJson.ToJObject();//��ѯ����

            string sql = string.Empty;
            string sqlWhere = string.Empty;
            string sqlWhere1 = string.Empty;
            string sql1 = string.Empty;

            sql = @"select b.fullname,b.encode,
                            nvl(t.l1,0) l1,nvl(t.l2,0) l2,nvl(t.l3,0) l3,nvl(t.l4,0) l4,
                            nvl((l1+l2+l3+l4),0) total
                          
                            from base_department b
                            left join ({0})t on t.createuserorgcode=b.encode
                             where b.nature='����' and  b.deptcode like'{1}%'";
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
                                                and h.createuserorgcode in (select b.encode from base_department b where b.deptcode like'{1}%' and b.nature='����')  
                                                and h.isdanger='1'  {0}", sqlWhere1,currUser.NewDeptCode);//δ���Σ��Դ����
            string sql3 = string.Format(@"  select count(id) from hsd_hazardsource h where h.id not in(select hdid from hsd_hdjd d where d.ISDJJD='1' {0})
                                                and h.createuserorgcode in (select b.encode from base_department b where b.deptcode like'{1}%' and b.nature='����')  
                                                and h.isdanger='1'
                                                {0}", sqlWhere1,currUser.NewDeptCode);//δ�Ǽ�Σ��Դ����
            string sql4 = string.Format(@"  select count(id) from hsd_jkjc j where j.jkyhzgids>0 
                                                     and j.createuserorgcode in (select b.encode from base_department b where b.deptcode like'{1}%' and b.nature='����')  
                                                    {0}", sqlWhere1, currUser.NewDeptCode);//����������Σ��Դ����
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
                    name = "ȫ��",
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
                    name = "ȫ��",
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
                        object[] arr = { "�ش����", dt.Compute("Sum(l1)", "").ToDouble() };
                        pie.Add(arr);
                        arr = new object[] { "�ϴ����", dt.Compute("Sum(l2)", "").ToDouble() };
                        pie.Add(arr);
                        arr = new object[] { "һ�����", dt.Compute("Sum(l3)", "").ToDouble() };
                        pie.Add(arr);
                        arr = new object[] { "�ͷ���", dt.Compute("Sum(l4)", "").ToDouble() };
                        pie.Add(arr);
                        return Newtonsoft.Json.JsonConvert.SerializeObject(new { pie = pie, list = list, result = result });
                    default:
                        break;
                }
            }
            return null;
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
