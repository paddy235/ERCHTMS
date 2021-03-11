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
    /// 描 述：
    /// </summary>
    public class ProvinceHighWorkService : RepositoryFactory<HighRiskCommonApplyEntity>, ProvinceHighWorkIService
    {
        #region 统计

        #region 按作业类型统计
        /// <summary>
        /// 按作业类型统计
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
        ///作业类型统计(统计表格)
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

        #region  按单位对比
        /// <summary>
        /// 单位对比(统计图)
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
            var dtdepts = this.BaseRepository().FindTable(string.Format("select departmentid,encode,fullname,sortcode from base_department  where nature='厂级' and deptcode like '{0}%' order by sortcode", user.OrganizeCode));

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
        /// 单位对比(表格)
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

        #region 获取高风险作业列表
        /// <summary>
        /// 获取高风险作业列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            string role = user.RoleName;
            string deptid = user.DeptId;

            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["worktype"].IsEmpty())//作业类型
            {
                pagination.conditionJson += string.Format(" and WorkType='{0}'", queryParam["worktype"].ToString());
            }
            //时间选择
            if (!queryParam["st"].IsEmpty())//作业开始时间
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//作业结束时间
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and WorkStartTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["workdeptcode"].IsEmpty())//所属电厂
            {
                pagination.conditionJson += string.Format(" and WorkDeptCode in(select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", queryParam["workdeptcode"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #endregion


        #region 手机端高风险作业统计
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
