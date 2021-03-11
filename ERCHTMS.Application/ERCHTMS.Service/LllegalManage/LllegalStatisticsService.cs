using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Data;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Code;

namespace ERCHTMS.Service.LllegalManage
{
    /// <summary>
    /// 描 述：违章基本信息
    /// </summary>
    public class LllegalStatisticsService : RepositoryFactory<LllegalRegisterEntity>, LllegalStatisticsIService
    {
        #region 统计图
        /// <summary>
        /// 违章级别统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLllegalLevelTotal(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";
            //年度
            var Year = queryParam["Year"];
            if (Year.IsEmpty() == false)
            {
                where += string.Format(" and to_char(lllegaltime,'yyyy')=='{0}'", Year);
            }
            string deptMark = "lllegalteamcode"; //按何种单位进行统计  lllegalteamcode  createuserdeptcode  reformdeptcode
            if (null != queryParam["deptMark"])
            {
                deptMark = queryParam["deptMark"].ToString();
            }
            var deptCode = queryParam["deptCode"];
            if (deptCode.IsEmpty() == false)
            {
                where += string.Format(" and {0} in (select encode from base_department  start with encode = '{1}' connect by  prior  departmentid = parentid)", deptMark, deptCode.ToString());
            }
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }

            var fields = "";
            var groups = queryParam["levelGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format("(select count(1) from {0} where {1} and lllegallevelname='{2}') as {2},", tableName, where, grpName);
            }
            fields = fields.Substring(0, fields.Length - 1);
            string sql = string.Format("select {0} from dual", fields);

            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        public DataTable GetLllegalLevelTotalGrp(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";
            //年度
            var Year = queryParam["Year"];
            if (Year.IsEmpty() == false)
            {
                where += string.Format(" and to_char(lllegaltime,'yyyy')=='{0}'", Year);
            }

            var deptCode = queryParam["deptCode"];
            if (deptCode.IsEmpty() == false)
            {
                where += string.Format(" and lllegalteamcode in (select encode from base_department  start with encode = '{0}' connect by  prior  departmentid = parentid)", deptCode.ToString());
            }
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            var fields = "";
            var groups = queryParam["levelGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format("(select count(1) from {0} where {1} and lllegallevelname='{2}') as {2},", tableName, where, grpName);
            }
            fields = fields.Substring(0, fields.Length - 1);
            string sql = string.Format("select {0} from dual", fields);

            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        /// <summary>
        /// 违章级别统计列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLllegalLevelList(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            string tableName = "v_lllegalbaseinfo";
            string where = " 1=1 ";
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            var orgId = queryParam["orgId"];
            var parentId = queryParam["deptId"];
            var deptCode = queryParam["deptCode"];
            string deptMark = "lllegalteamcode"; //按何种单位进行统计  lllegalteamcode  createuserdeptcode  reformdeptcode
            if (null != queryParam["deptMark"])
            {
                deptMark = queryParam["deptMark"].ToString();
            }
            var groups = queryParam["levelGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            var fields = "";
            where += string.Format("  and  {0} in (select encode from base_department  where encode like '{1}%')  ", deptMark, deptCode);
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format(@"(select count(1) from {0} where {1} and lllegallevelname='{2}' and {3} like d.encode || '%') as {2},
                                          (select itemdetailid from base_dataitemdetail d where d.itemname='{2}' and d.itemid=(select itemid from base_dataitem i where i.itemcode='LllegalLevel')) as ID{2},", tableName, where, grpName, deptMark);
            }
            fields = fields.Substring(0, fields.Length - 1);
            //部门及子节点
            string sql = string.Format("select departmentid,encode,fullname, {0} from base_department d where  d.parentid='{1}' or d.departmentid='{1}' order by d.encode asc", fields, parentId.ToString());

            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        public DataTable GetLllegalLevelListGrp(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }

            var orgId = queryParam["orgId"];
            var parentId = queryParam["deptId"];
            var deptCode = queryParam["deptCode"];
            var groups = queryParam["levelGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            var fields = "";
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format(@"(select count(1) from {0} where {1} and lllegallevelname='{2}' and lllegalteamcode in(select encode from base_department  start with encode= d.encode connect by  prior  departmentid =parentid)) as {2},
                                          (select itemdetailid from base_dataitemdetail d where d.itemname='{2}' and d.itemid=(select itemid from base_dataitem i where i.itemcode='LllegalLevel')) as ID{2},", tableName, where, grpName);
            }
            fields = fields.Substring(0, fields.Length - 1);
            //集团和电厂
            string sql = string.Format("select departmentid,encode,fullname, {0} from base_department d where (d.nature='集团'  or d.nature='省级'or d.nature='厂级') start with departmentid= '{1}' connect by  prior  departmentid =parentid order by d.encode asc", fields, parentId.ToString());

            dt = this.BaseRepository().FindTable(sql);

            var dept = new DepartmentService().GetEntity(parentId.ToString());
            if (dept.FullName != "各电厂")
            {
                var dr = dt.Select("fullname='各电厂'");
                if (dr.Length > 0)
                    dt.Rows.Remove(dr[0]);
            }

            return dt;
        }
        /// <summary>
        /// 违章类型统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLllegalTypeTotal(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";

            string deptMark = "lllegalteamcode"; //按何种单位进行统计  lllegalteamcode  createuserdeptcode  reformdeptcode
            string timeMark = string.Empty;
            string stDate = string.Empty;
            string etDate = string.Empty;

            //时间类型
            if (!queryParam["seltype"].IsEmpty())
            {
                //统计类型
                switch (queryParam["seltype"].ToString())
                {
                    case "0"://本月
                        stDate = DateTime.Now.ToString("yyyy-MM-01");
                        etDate = DateTime.Now.AddMonths(1).AddDays(-1).ToString("yyyy-MM-01");
                        break;
                    case "1"://本季度
                        stDate = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                        etDate = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day).AddMonths(3).AddDays(-1).ToString("yyyy-MM-dd");
                        break;
                    case "2"://本年
                        stDate = DateTime.Now.ToString("yyyy-01-01");
                        etDate = DateTime.Now.AddYears(1).AddDays(-1).ToString("yyyy-01-01");
                        break;
                }
            }
            if (!queryParam["timeMark"].IsEmpty())
            {
                timeMark = queryParam["timeMark"].ToString();
            }

            if (!stDate.IsEmpty() && !timeMark.IsEmpty())
            {
                where += string.Format(" and {0} >= to_date('{1}','yyyy-mm-dd hh24:mi:ss')", timeMark, stDate.ToString());
            }
            if (!etDate.IsEmpty() && !timeMark.IsEmpty())
            {
                where += string.Format(" and {0} < to_date('{1}','yyyy-mm-dd hh24:mi:ss')", timeMark, DateTime.Parse(etDate.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }

            if (!queryParam["deptMark"].IsEmpty())
            {
                deptMark = queryParam["deptMark"].ToString();
            }
            var deptCode = queryParam["deptCode"];

            if (!deptCode.IsEmpty())
            {
                where += string.Format(" and {0} like '{1}%'", deptMark, deptCode.ToString());
            }
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            var groups = queryParam["typeGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            var fields = "";
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format("(select count(1) from {0} where {1} and lllegaltypename='{2}') as {2},", tableName, where, grpName);
            }
            fields = fields.Substring(0, fields.Length - 1);
            string sql = string.Format("select {0} from dual", fields);

            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        public DataTable GetLllegalTypeTotalGrp(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";
            var deptCode = queryParam["deptCode"];
            if (deptCode.IsEmpty() == false)
            {
                where += string.Format(" and lllegalteamcode in(select encode from base_department  start with encode= '{0}' connect by  prior  departmentid =parentid)", deptCode.ToString());
            }
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            var groups = queryParam["typeGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            var fields = "";
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format("(select count(1) from {0} where {1} and lllegaltypename='{2}') as {2},", tableName, where, grpName);
            }
            fields = fields.Substring(0, fields.Length - 1);
            string sql = string.Format("select {0} from dual", fields);

            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }


        /// <summary>
        /// 违章类型统计列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLllegalTypeList(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            var orgId = queryParam["orgId"];
            string deptMark = "lllegalteamcode"; //按何种单位进行统计  lllegalteamcode  createuserdeptcode  reformdeptcode
            if (null != queryParam["deptMark"])
            {
                deptMark = queryParam["deptMark"].ToString();
            }
            var deptCode = queryParam["deptCode"];
            var parentId = queryParam["deptId"];
            var fields = "";
            where += string.Format("  and  {0} in (select encode from base_department  where encode like '{1}%')  ", deptMark, deptCode);
            var groups = queryParam["typeGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format(@"(select count(1) from {0} where {1} and lllegaltypename='{2}' and {3} like d.encode || '%') as {2},
                                          (select itemdetailid from base_dataitemdetail d where d.itemname='{2}' and d.itemid=(select itemid from base_dataitem i where i.itemcode='LllegalType')) as ID{2},", tableName, where, grpName, deptMark);
            }
            fields = fields.Substring(0, fields.Length - 1);
            //部门及子节点
            string sql = string.Format("select departmentid,encode,fullname, {0} from base_department d where d.parentid='{1}' or d.departmentid='{1}' order by d.encode asc", fields, parentId.ToString());
            //if (deptCode.ToString().Length == 3)
            //{//公司节点
            //    orgId = parentId;
            //    sql = string.Format("select * from (select d.organizeId as departmentid,encode,fullname, {0} from base_organize d where d.organizeid='{1}' union all select departmentid,encode,fullname, {0} from base_department d where (d.organizeid='{1}' and d.parentid='{2}') or d.departmentid='{2}') d order by d.encode asc", fields, orgId.ToString(), "0");
            //}
            //if (orgId.ToString() == "0")//根节点
            //    sql = string.Format("select d.organizeId as departmentid,encode,fullname, {0} from base_organize d where d.parentid='{1}' or d.parentid='-1' order by d.encode asc", fields, orgId);


            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        public DataTable GetLllegalTypeListGrp(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            var orgId = queryParam["orgId"];
            var deptCode = queryParam["deptCode"];
            var parentId = queryParam["deptId"];
            var fields = "";
            var groups = queryParam["typeGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format(@"(select count(1) from {0} where {1} and lllegaltypename='{2}' and lllegalteamcode in(select encode from base_department  start with encode= d.encode connect by  prior  departmentid =parentid)) as {2},
                                          (select itemdetailid from base_dataitemdetail d where d.itemname='{2}' and d.itemid=(select itemid from base_dataitem i where i.itemcode='LllegalType')) as ID{2},", tableName, where, grpName);
            }
            fields = fields.Substring(0, fields.Length - 1);
            //集团和电厂
            string sql = string.Format("select departmentid,encode,fullname, {0} from base_department d where (nature='集团' or nature='省级' or nature='厂级') start with departmentid= '{1}' connect by  prior  departmentid =parentid order by d.encode asc", fields, parentId.ToString());

            dt = this.BaseRepository().FindTable(sql);

            var dept = new DepartmentService().GetEntity(parentId.ToString());
            if (dept.FullName != "各电厂")
            {
                var dr = dt.Select("fullname='各电厂'");
                if (dr.Length > 0)
                    dt.Rows.Remove(dr[0]);
            }

            return dt;
        }
        /// <summary>
        /// 违章整改率
        /// </summary>
        /// <returns></returns>
        public DataTable GetLllegalZgv(string deptcode)
        {
            string sql = string.Format("select Flowstate,lllegallevelname,count(1) as Num from v_lllegalbaseinfo where lllegalteamcode in (select encode from base_department  start with encode= '{0}' connect by  prior  departmentid =parentid) group by Flowstate,lllegallevelname", deptcode);
            var dt = this.BaseRepository().FindTable(sql);
            return dt;
        }
        #endregion

        #region 趋势统计图
        /// <summary>
        /// 违章级别趋势统计数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLllegalTrendData(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";

            string deptMark = "lllegalteamcode"; //按何种单位进行统计  lllegalteamcode  createuserdeptcode  reformdeptcode
            if (null != queryParam["deptMark"]) 
            {
                deptMark = queryParam["deptMark"].ToString();
            }
            var deptCode = queryParam["deptCode"];
            if (deptCode != null)
            {
                where += string.Format(" and {0} like '{1}%'", deptMark, deptCode.ToString());
            }
            where += string.Format("  and  {0} in (select encode from base_department  where encode like '{1}%')  ", deptMark, deptCode);
            var legType = queryParam["legType"];
            if (!legType.IsEmpty())
            {
                where += string.Format(" and lllegaltype = '{0}'", legType.ToString());
            }
            var year = queryParam["year"];
            if (year != null)
            {
                where += string.Format(" and to_char(lllegaltime,'yyyy')={0} and extract(month from lllegaltime)=u.month", year.ToString());
            }
            var fields = "";
            var groups = queryParam["levelGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format(@"(select count(1) from {0} where {1} and lllegallevelname='{2}') as {2},
                                          (select itemdetailid from base_dataitemdetail d where d.itemname='{2}' and d.itemid=(select itemid from base_dataitem i where i.itemcode='LllegalLevel')) as ID{2},", tableName, where, grpName);
            }
            fields = fields.Substring(0, fields.Length - 1);
            string sql = string.Format("select u.month,{0} from (select rownum as month from dual connect by rownum<=12) u", fields);

            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        public DataTable GetLllegalTrendDataGrp(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";
            var deptCode = queryParam["deptCode"];
            if (deptCode != null)
            {
                where += string.Format(" and lllegalteamcode in(select encode from base_department  start with encode='{0}' connect by  prior  departmentid =parentid)", deptCode.ToString());
            }
            var legType = queryParam["legType"];
            if (!legType.IsEmpty())
            {
                where += string.Format(" and lllegaltype = '{0}'", legType.ToString());
            }
            var year = queryParam["year"];
            if (year != null)
            {
                where += string.Format(" and to_char(lllegaltime,'yyyy')={0} and extract(month from lllegaltime)=u.month", year.ToString());
            }
            var fields = "";
            var groups = queryParam["levelGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format(@"(select count(1) from {0} where {1} and lllegallevelname='{2}') as {2},
                                          (select itemdetailid from base_dataitemdetail d where d.itemname='{2}' and d.itemid=(select itemid from base_dataitem i where i.itemcode='LllegalLevel')) as ID{2},", tableName, where, grpName);
            }
            fields = fields.Substring(0, fields.Length - 1);
            string sql = string.Format("select u.month,{0} from (select rownum as month from dual connect by rownum<=12) u", fields);

            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region 对比统计图
        /// <summary>
        /// 违章级别对比数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLllegalCompareData(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";
            var legType = queryParam["legType"];
            if (!legType.IsEmpty())
            {
                where += string.Format(" and lllegaltype = '{0}'", legType.ToString());
            }
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }

            var orgId = queryParam["orgId"];
            var parentId = queryParam["deptId"];
            string deptMark = "lllegalteamcode"; //按何种单位进行统计  lllegalteamcode  createuserdeptcode  reformdeptcode
            if (null != queryParam["deptMark"])
            {
                deptMark = queryParam["deptMark"].ToString();
            }
            var deptCode = queryParam["deptCode"];
            var groups = queryParam["levelGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            var fields = "";
            where += string.Format("  and  {0} in (select encode from base_department  where encode like '{1}%')  ", deptMark, deptCode);
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format(@"(select count(1) from {0} where {1} and lllegallevelname='{2}' and {3} like d.encode || '%') as {2},
                                          (select itemdetailid from base_dataitemdetail d where d.itemname='{2}' and d.itemid=(select itemid from base_dataitem i where i.itemcode='LllegalLevel')) as ID{2},", tableName, where, grpName, deptMark);
            }
            fields = fields.Substring(0, fields.Length - 1);
            //部门及子节点
            string sql = string.Format("select departmentid,encode,fullname, {0} from base_department d where d.parentid='{1}' order by d.encode asc", fields, parentId.ToString());
            //if (deptCode.ToString().Length == 3)
            //{//公司节点
            //    orgId = parentId;
            //    sql = string.Format("select departmentid,encode,fullname, {0} from base_department d where d.organizeid='{1}' and d.parentid='{2}' order by d.encode asc", fields, orgId.ToString(), "0");
            //}
            //if (orgId.ToString() == "0")//根节点
            //    sql = string.Format("select d.organizeId as departmentid,encode,fullname, {0} from base_organize d where d.parentid='{1}' or d.parentid='-1' order by d.encode asc", fields, orgId);


            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        public DataTable GetLllegalCompareDataGrp(string queryJson)
        {
            DataTable dt = null;

            var queryParam = queryJson.ToJObject();
            var tableName = "v_lllegalbaseinfo";
            var where = " 1=1 ";
            var legType = queryParam["legType"];
            if (!legType.IsEmpty())
            {
                where += string.Format(" and lllegaltype = '{0}'", legType.ToString());
            }
            var startTime = queryParam["startTime"];
            if (startTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", startTime.ToString());
            }
            var endTime = queryParam["endTime"];
            if (endTime.IsEmpty() == false)
            {
                where += string.Format(" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Parse(endTime.ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }

            var orgId = queryParam["orgId"];
            var parentId = queryParam["deptId"];
            var deptCode = queryParam["deptCode"];
            var groups = queryParam["levelGroups"].ToString();
            var grpList = groups.Split(new char[] { ',' });
            var fields = "";
            for (var i = 0; i < grpList.Length; i++)
            {
                var grpName = grpList[i];
                fields += string.Format(@"(select count(1) from {0} where {1} and lllegallevelname='{2}' and lllegalteamcode in(select encode from base_department  start with encode= d.encode connect by  prior  departmentid =parentid)) as {2},
                                          (select itemdetailid from base_dataitemdetail d where d.itemname='{2}' and d.itemid=(select itemid from base_dataitem i where i.itemcode='LllegalLevel')) as ID{2},", tableName, where, grpName);
            }
            fields = fields.Substring(0, fields.Length - 1);
            //集团和电厂
            string sql = string.Format("select departmentid,encode,fullname, {0} from base_department d where d.nature='厂级' start with d.departmentid= '{1}' connect by  prior  departmentid =parentid order by d.encode asc", fields, parentId.ToString());

            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        public DataTable GetJFStatis(string queryJson)
        {
            DataTable dt = null;
            string sqlWhere = " where 1=1 ";
            var queryParam = queryJson.ToJObject();
            var year = queryParam["year"];
            if (!year.IsEmpty())
            {
                sqlWhere += string.Format(" and to_char(l.LLLEGALTIME,'yyyy')='{0}'", year);
            }
            var sql1 = string.Format(@"select l.userid,l.lllegaltime,l.lllegalpoint from v_lllegalassesforperson l {0}", sqlWhere);
            var deptCode = queryParam["deptCode"];
            var sql = string.Format(@" select sum(nvl(l.lllegalpoint,0)) point,u.realname,u.userid 
                                            from v_userinfo u left join ({1}) l on l.userid = u.userid
                                             where u.departmentcode like '{0}%'  
                                             group by u.realname,u.userid order by u.realname asc", deptCode, sql1);
            dt = this.BaseRepository().FindTable(sql);
            return dt;
        }


        #region 移动端违章统计
        /// <summary>
        /// 移动端违章统计
        /// </summary>
        /// <param name="code"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetAppLllegalStatistics(string code, string year, int mode)
        {
            string sql = "";

            DataTable dt = new DataTable();

            //当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (mode == 0)  //整改违章数量
            {
                if (!string.IsNullOrEmpty(code))
                {
                    sql = string.Format(@"select nvl(lllegal,0) lllegal ，nvl(yblllegal,0)  yblllegal,nvl(jyzlllegal ,0) jyzlllegal,  nvl(yzlllegal ,0) yzlllegal  from (
                                 select  count(1) as pnum ,lllegallevelname from v_lllegalbaseinfo where reformdeptcode like '{0}%'  and to_char(createdate,'yyyy') ='{1}'  group by lllegallevelname 
                                 union
                                 select  count(1) as pnum ,'全部违章' as lllegallevelname from v_lllegalbaseinfo where reformdeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'
                           ) pivot (sum(pnum) for lllegallevelname in ('一般违章' as yblllegal,'较严重违章' as  jyzlllegal , '严重违章' as yzlllegal,'全部违章' as lllegal))", code, DateTime.Now.Year.ToString());
                }
                else
                {
                    //code = user.NewDeptCode;
                    if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级"))
                    {
                        code = user.OrganizeCode;
                    }
                    else
                    {
                        code = user.DeptCode;
                    }

                    sql = string.Format(@"select nvl(lllegal,0) lllegal ，nvl(yblllegal,0)  yblllegal,nvl(jyzlllegal ,0) jyzlllegal,  nvl(yzlllegal ,0) yzlllegal  from (
                                 select  count(a.id) as pnum ,a.lllegallevelname  from v_lllegalbaseinfo a
                                 where a.reformdeptcode like '{0}%'  and to_char(a.createdate,'yyyy') ='{1}'  group by lllegallevelname 
                                 union
                                 select  count(a.id) as pnum ,'全部违章' as lllegallevelname from v_lllegalbaseinfo a
                                 where a.reformdeptcode like '{0}%' and to_char(a.createdate,'yyyy') ='{1}'
                           ) pivot (sum(pnum) for lllegallevelname in ('一般违章' as yblllegal,'较严重违章' as  jyzlllegal , '严重违章' as yzlllegal,'全部违章' as lllegal))", code, DateTime.Now.Year.ToString());
                }
            }
            //班组终端违章月份趋势统计图(按创建单位)
            else if (mode == 1)
            {
                sql = string.Format(@"select a.month, nvl(b.s1,0) s1 ,nvl(b.s2,0) s2 ,nvl(b.s3,0) s3 from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join (
                                                     select * from (    
                                                       select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,lllegallevelname from v_lllegalbaseinfo where createuserdeptcode  like '{0}%' 
                                                       and to_char(createdate,'yyyy') ='{1}' group by to_char(createdate,'MM') ,lllegallevelname 
                                                   ) pivot (sum(pnum) for lllegallevelname in ( '一般违章'as s1,'较严重违章' as s2,'严重违章' as s3 )) 
                                               ) b on a.month =b.tMonth order by a.month",
                                  code, year);
            }
            //班组终端违章类型统计图(按创建单位)
            else if (mode == 2)
            {
                sql = string.Format(@"select nvl(a.wmtype,0) wmtype,nvl(a.zztype,0) zztype,nvl(a.zytype,0) zytype,nvl(a.gltype,0) gltype,nvl(a.zhtype,0) zhtype,(nvl(a.wmtype,0) + nvl(a.zztype,0)+ nvl(a.zytype,0)+ nvl(a.gltype,0)+ nvl(a.zhtype,0)) as total  from (
                                                select * from ( select  count(1) as pnum , lllegaltypename  from v_lllegalbaseinfo where createuserdeptcode  like '{0}%' 
                                                and to_char(createdate,'yyyy') ='{1}'   group by  lllegaltypename  
                                                ) pivot (sum(pnum) for lllegaltypename in ('文明卫生类' as wmtype,'装置类' as zztype,'作业类'as zytype,'管理类'as gltype,'指挥类'as zhtype ))) a   ",
                                  code, year);
            }

            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region  获取前几个曝光的数据(取当前年的)
        /// <summary>
        /// 获取前几个曝光的数据(取当前年的)
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataTable QueryExposureLllegal(string num)
        {
            string sql = string.Empty;
            DataItemDetailService dataitemdetailservice = new DataItemDetailService();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            sql = string.Format(@"   select a.* from (
                                                  select distinct a.id ,a.id lllegalid,a.lllegalnumber ,a.reformrequire,a.lllegaldescribe,a.createdate,a.flowstate,a.addtype,a.reformdeptcode,
                                                   f.filepath  filepic, (case when f.filepath is not null then ('{3}'||substr(f.filepath,2)) else '' end)  filepath ,a.createuserorgcode from v_lllegalbaseinfo a
                                                  left join v_imageview f on a.lllegalpic = f.recid  where a.isexposure ='1' and 
                                                  a.reformdeptcode like '{1}%' and to_char(a.createdate,'yyyy') ='{2}' ) a where rownum <= {0} order by createdate  ", int.Parse(num), user.OrganizeCode, DateTime.Now.Year.ToString(), dataitemdetailservice.GetItemValue("imgUrl"));

            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion
    }
}