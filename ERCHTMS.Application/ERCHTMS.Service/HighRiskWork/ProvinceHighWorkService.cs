using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Service.BaseManage;
using BSFramework.Application.Entity;
using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ����
    /// </summary>
    public class ProvinceHighWorkService : RepositoryFactory<HighRiskCommonApplyEntity>, ProvinceHighWorkIService
    {
        #region ͳ��

        #region ����ҵ����ͳ��
        /// <summary>
        /// ����ҵ����ͳ��
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetProvinceHighCount(string starttime, string endtime, string deptid, string deptcode)
        {
            DataTable dt = GetWorkList(starttime, endtime, deptid, deptcode);
            dt.Columns.Remove("itemvalue");
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }

        private DataTable GetWorkList(string starttime, string endtime, string deptid, string deptcode)
        {
            var user = OperatorProvider.Provider.Current();
            var strdeptcode = user.NewDeptCode;
            if (!string.IsNullOrEmpty(deptcode))
            {
                strdeptcode = deptcode;
            }
            string strWhere = string.Format(" where WorkDeptCode  in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", strdeptcode);
            if (!string.IsNullOrEmpty(starttime))
            {
                strWhere += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                var strendtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                strWhere += string.Format(" and WorkStartTime<=to_date('{0}','yyyy-mm-dd')", strendtime);
            }
            string sql = string.Format("select itemvalue,itemname as name,nvl(c,0) y from (select count(1) c,worktype from v_highriskstat {0} group by worktype) a right join(select itemname,itemvalue,sortcode from  base_dataitemdetail  where itemid =(select itemid from base_dataitem where itemcode='StatisticsType')) b  on   b.itemvalue=a.worktype order by b.sortcode", strWhere);
            DataTable dt = this.BaseRepository().FindTable(sql);
            dt.Dispose();
            return dt;
        }


        /// <summary>
        ///��ҵ����ͳ��(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        public string GetProvinceHighList(string starttime, string endtime, string deptid, string deptcode)
        {
            DataTable dt = GetWorkList(starttime, endtime, deptid, deptcode);
            dt.Columns.Add("percent", typeof(string));
            dt.Columns["name"].ColumnName = "worktype";
            dt.Columns["y"].ColumnName = "typenum";

            int allnum = dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Compute("sum(typenum)", "true"));
            foreach (DataRow item in dt.Rows)
            {
                var count = Convert.ToInt32(item["typenum"].ToString());
                decimal percent = allnum == 0 || count == 0 ? 0 : decimal.Parse(count.ToString()) / decimal.Parse(allnum.ToString());
                item["percent"] = Math.Round(percent * 100, 2) + "%";
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }
        #endregion

        #region  ����λ�Ա�
        /// <summary>
        /// ��λ�Ա�(ͳ��ͼ)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public string GetProvinceHighDepartCount(string starttime, string endtime)
        {
            List<string> listdepts;
            List<int> list;
            GetDeptContrast(starttime, endtime, out listdepts, out list);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = listdepts, y = list });
        }

        private DataTable GetDeptContrast(string starttime, string endtime, out List<string> listdepts, out List<int> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("deptname");
            dt.Columns.Add("deptcount");
            dt.Columns.Add("deptcode");
            dt.Columns.Add("deptid");

            var user = OperatorProvider.Provider.Current();
            string strWhere = "";
            if (!string.IsNullOrEmpty(starttime))
            {
                strWhere += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                var strendtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                strWhere += string.Format(" and WorkStartTime<=to_date('{0}','yyyy-mm-dd')", strendtime);
            }
            var dtdepts = this.BaseRepository().FindTable(string.Format("select departmentid,encode,fullname,sortcode from base_department  where nature='����' and deptcode like '{0}%' order by sortcode", user.OrganizeCode));

            listdepts = new List<string>();
            list = new List<int>();

            foreach (DataRow item in dtdepts.Rows)
            {
                listdepts.Add(item["fullname"].ToString());
                var deptcode = item["encode"].ToString();
                string whereSQL2 = string.Format(" and WorkDeptCode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", deptcode);
                int num = this.BaseRepository().FindObject(string.Format(@"select count(1) c from v_highriskstat where 1=1 {0} {1}", strWhere, whereSQL2)).ToInt();
                list.Add(num);
                var row = dt.NewRow();
                row["deptname"] = item["fullname"].ToString();
                row["deptcount"] = num;
                row["deptcode"] = deptcode;
                row["deptid"] = item["departmentid"].ToString(); ;
                dt.Rows.Add(row);
            }
            return dt;
        }

        /// <summary>
        /// ��λ�Ա�(���)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public string GetProvinceHighDepartList(string starttime, string endtime)
        {
            List<string> listdepart;
            List<int> list;
            var dtresult = GetDeptContrast(starttime, endtime, out listdepart, out list);
            try
            {
                dtresult.Columns.Add("percent");
                int allnum = list.Sum();
                foreach (DataRow item in dtresult.Rows)
                {
                    var count = Convert.ToInt32(item["deptcount"].ToString());
                    decimal percent = allnum == 0 || count == 0 ? 0 : decimal.Parse(count.ToString()) / decimal.Parse(allnum.ToString());
                    item["percent"] = Math.Round(percent * 100, 2) + "%";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dtresult.Rows.Count,
                rows = dtresult
            });
        }
        #endregion

        #endregion

        #region ��ȡ�߷�����ҵ�б�
        /// <summary>
        /// ��ȡ�߷�����ҵ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            string role = user.RoleName;
            string deptid = user.DeptId;

            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["worktype"].IsEmpty())//��ҵ����
            {
                pagination.conditionJson += string.Format(" and WorkType='{0}'", queryParam["worktype"].ToString());
            }
            //ʱ��ѡ��
            if (!queryParam["st"].IsEmpty())//��ҵ��ʼʱ��
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//��ҵ����ʱ��
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and WorkStartTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["workdeptcode"].IsEmpty())//�����糧
            {
                pagination.conditionJson += string.Format(" and WorkDeptCode in(select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", queryParam["workdeptcode"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #endregion


        #region �ֻ��˸߷�����ҵͳ��
        public DataTable AppGetHighWork(string starttime, string endtime, string deptid, string deptcode)
        {
            DataTable dt = GetWorkList(starttime, endtime, deptid, deptcode);
            dt.Columns.Add("percent", typeof(string));
            dt.Columns["itemvalue"].ColumnName = "worktypevalue";
            dt.Columns["name"].ColumnName = "worktype";
            dt.Columns["y"].ColumnName = "typenum";

            int allnum = dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Compute("sum(typenum)", "true"));
            foreach (DataRow item in dt.Rows)
            {
                var count = Convert.ToInt32(item["typenum"].ToString());
                decimal percent = allnum == 0 || count == 0 ? 0 : decimal.Parse(count.ToString()) / decimal.Parse(allnum.ToString());
                item["percent"] = Math.Round(percent * 100, 2) + "%";
            }
            return dt;
        }
        #endregion
    }
}
