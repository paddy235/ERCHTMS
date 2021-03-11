using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BSFramework.Util.Extension;
using BSFramework.Util;
using System.Text;
using System.Linq;
namespace ERCHTMS.AppSerivce.Controllers
{
    public class PublicityController : BaseApiController
    {
        public HttpContext ctx { get { return HttpContext.Current; } }
        private PublicityBLL Publicitybll = new PublicityBLL();
        private PublicityDetailsBLL PublicityDetailsbll = new PublicityDetailsBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPublicityDefault([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                DataTable dtItems = departmentBLL.GetDataTable(string.Format(@"select ExamineType,b.itemname ExamineTypeName,Year,ShowType,StaffType,t.createdate from HRS_PUBLICITY t
left join base_dataitemdetail b on b.itemvalue=t.ExamineType and b.itemid=
(select c.itemid from base_dataitem c where c.itemcode='ExamineType')
 where t.createdate=(select max(a.createdate) from HRS_PUBLICITY a) order by StaffType asc"));

                DataTable dtHt = new DataTable();
                string examineType = "", year = "", showType = "", staffType = "";
                if (dtItems.Rows.Count == 0)
                {
                    dtHt = departmentBLL.GetDataTable(string.Format("select d.checktype,d.timetype,d.year,d.CHECKDATE,d.ROLENAME,d.nature,d.postname from v_checknotice d order by CHECKDATE desc"));
                    if (dtHt.Rows.Count > 0)
                    {
                        examineType = dtHt.Rows[0]["checktype"].ToString();
                        showType = dtHt.Rows[0]["timetype"].ToString();
                        year = dtHt.Rows[0]["year"].ToString();
                        string postName = dtHt.Rows[0]["postName"].ToString();
                        string nature = dtHt.Rows[0]["nature"].ToString();
                        string roleNames = dtHt.Rows[0]["ROLENAME"].ToString();
                        if ((nature == "部门" && roleNames.Contains("负责人")) || (nature == "承包商" && roleNames.Contains("负责人")) || (nature == "分包商" && roleNames.Contains("负责人")))
                        {
                            staffType = "1";
                        }
                        if (roleNames.Contains("专工"))
                        {
                            staffType = "2";
                        }
                        if ((nature == "='班组" && roleNames.Contains("负责人")) || (nature == "承包商" && (postName == "班组长" || postName == "='班长")) || (nature == "承包商" && (postName == "班组长" || postName == "='班长")))
                        {
                            staffType = "3";
                        }
                        if ((nature == "班组" && roleNames.Contains("普通用户") && !roleNames.Contains("负责人")) || (nature == "承包商" && roleNames.Contains("普通用户") && !roleNames.Contains("负责人")) || (nature == "分包商" && roleNames.Contains("普通用户") && !roleNames.Contains("负责人")))
                        {
                            staffType = "4";
                        }
                    }
                    else
                    {
                        return new { code = 0, info = "获取数据成功", count = 0, data = new { } };
                    }

                }
                else
                {
                    examineType = dtItems.Rows[0][0].ToString();
                    year = dtItems.Rows[0][2].ToString();
                    showType = dtItems.Rows[0][3].ToString();
                    staffType = dtItems.Rows[0][4].ToString();
                    dtHt = departmentBLL.GetDataTable(string.Format("select d.checktype,d.timetype,d.year,d.CHECKDATE,d.ROLENAME,d.nature,d.postname from v_checknotice d order by CHECKDATE desc"));
                    if (dtHt.Rows.Count > 0)
                    {
                        DateTime time1 = dtItems.Rows[0]["createdate"].ToDate();
                        DateTime time2 = dtHt.Rows[0]["checkdate"].ToDate();
                        if (time1 < time2)
                        {
                            examineType = dtHt.Rows[0]["checktype"].ToString();
                            showType = dtHt.Rows[0]["timetype"].ToString();
                            year = dtHt.Rows[0]["year"].ToString();
                            string postName = dtHt.Rows[0]["postName"].ToString();
                            string nature = dtHt.Rows[0]["nature"].ToString();
                            string roleNames = dtHt.Rows[0]["ROLENAME"].ToString();
                            if ((nature == "部门" && roleNames.Contains("负责人")) || (nature == "承包商" && roleNames.Contains("负责人")) || (nature == "分包商" && roleNames.Contains("负责人")))
                            {
                                staffType = "1";
                            }
                            if (roleNames.Contains("专工"))
                            {
                                staffType = "2";
                            }
                            if ((nature == "='班组" && roleNames.Contains("负责人")) || (nature == "承包商" && (postName == "班组长" || postName == "='班长")) || (nature == "分包商" && (postName == "班组长" || postName == "='班长")))
                            {
                                staffType = "3";
                            }
                            if ((nature == "班组" && roleNames.Contains("普通用户") && !roleNames.Contains("负责人")) || (nature == "承包商" && roleNames.Contains("普通用户") && !roleNames.Contains("负责人")) || (nature == "分包商" && roleNames.Contains("普通用户") && !roleNames.Contains("负责人")))
                            {
                                staffType = "4";
                            }
                        }
                    }

                }
                string examineTypeName = "";
                DataTable dtRow = departmentBLL.GetDataTable(string.Format(@"select c.itemname from base_dataitemdetail c where c.itemvalue='{0}' and c.itemid=(select c.itemid from base_dataitem c where c.itemcode='ExamineType')", examineType));
                if (dtRow.Rows.Count > 0)
                {
                    examineTypeName = dtRow.Rows[0][0].ToString();
                }
                var jsonData = new
                {
                    ExamineType = examineType,
                    ExamineTypeName = examineTypeName,
                    Year = year,
                    ShowType = showType,
                    StaffType = staffType
                };
                return new { code = 0, info = "获取数据成功", count = 0, data = jsonData };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        private DataTable GetDataSource(DataTable data, string queryJson, int mode = 0)
        {
            string sql = "select distinct userid,realname,deptname,postname,nature,deptcode from V_CHECKNOTICE where 1=1 ";
            string where = "";
            string order = "";
            var queryParam = queryJson.ToJObject();
            if (!queryParam["ExamineType"].IsEmpty())
            {
                where += string.Format(" and checktype='{0}'", queryParam["ExamineType"].ToString());
            }
            if (!queryParam["Year"].IsEmpty())
            {
                where += string.Format(" and year='{0}'", queryParam["Year"].ToString());
            }

            if (!queryParam["NameId"].IsEmpty())
            {
                where += string.Format(" and userid='{0}'", queryParam["NameId"].ToString());
            }
            string timeType = "1";
            if (!queryParam["ShowType"].IsEmpty())
            {
                timeType = queryParam["ShowType"].ToString();
                where += string.Format(" and timetype='{0}'", queryParam["ShowType"].ToString());
            }
            if (!queryParam["StaffType"].IsEmpty())
            {
                string type = queryParam["StaffType"].ToString();
                if (type == "1")
                {
                    where += string.Format(" and ((nature='部门' and rolename like '%负责人%') or (nature='承包商' and rolename like '%负责人%') or (nature='分包商' and rolename like '%负责人%'))");
                }
                if (type == "2")
                {
                    where += string.Format(" and rolename like '%专工%'");
                }
                if (type == "3")
                {
                    where += string.Format(" and ((nature='班组' and rolename like '%负责人%') or (nature='承包商' and (postname='班组长' or postname='班长')) or (nature='分包商' and (postname='班组长' or postname='班长')))");
                }
                if (type == "4")
                {
                    where += string.Format(" and ((nature='班组' and rolename like '%普通用户%' and rolename not like '%负责人%') or (nature='承包商' and rolename='普通用户') or (nature='分包商' and rolename='普通用户'))");

                    order = " order by deptname";
                }

            }
            sql += where;
            int k = 1;
            DataTable dtItems = departmentBLL.GetDataTable(sql + order);
            sql = string.Format("select userid,httype,hiddescribe,workstream,CHECKDATE,startdate,enddate,nature,deptcode from V_CHECKNOTICE where 1=1 {0}", where);
            if (timeType == "1")
            {
                sql += string.Format(" and CHECKDATE<add_months(startdate,6)");
            }
            if (timeType == "2")
            {
                sql += string.Format(" and CHECKDATE<add_months(startdate,3)");
            }
            if (mode == 0)
            {

                int count = data.Rows.Count;
                if (count > 0 && dtItems.Rows.Count > 0)
                {
                    int sort = data.Rows[count - 1]["ordernum"].ToInt();
                    k = sort;
                    if (data.Rows[count - 1]["nameid"].ToString() == dtItems.Rows[0]["userid"].ToString())
                    {
                        k = sort;
                    }
                    else
                    {
                        k++;
                    }
                }
            }
            foreach (DataRow dr in dtItems.Rows)
            {
                string sql1 = sql + string.Format(" and userid='{0}'", dr["userid"].ToString());
                DataTable dtHT = departmentBLL.GetDataTable(sql1);
                int i = 1;
                if (mode == 0)
                {
                    foreach (DataRow dr1 in dtHT.Rows)
                    {
                        DataRow row = data.NewRow();
                        //row["id"] = Guid.NewGuid().ToString();
                        row["nameid"] = dr["userid"];
                        //row["name"] = dr["realname"];
                        //row["dept"] = dr["deptname"];
                        //row["post"] = dr["postname"];
                        //row["hiddennum"] = dtHT.Select("userid='" + dr["userid"].ToString() + "'").Length;
                        //row["managerhidden"] = dtHT.Select("httype='1'").Length;
                        //row["entityhidden"] = dtHT.Select("httype='2'").Length;
                        row["detailsordernum"] = i;
                        row["ordernum"] = k;

                        //                        if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                        //                        {
                        //                            DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}') or encode='{0}' order by deptcode", dr["deptcode"], "部门"));
                        //                            if (dt.Rows.Count > 0)
                        //                            {
                        //                                string name = "";
                        //                                foreach (DataRow dr2 in dt.Rows)
                        //                                {
                        //                                    name += dr2["fullname"].ToString() + "/";
                        //                                }
                        //                                row["dept"] = name.TrimEnd('/');
                        //                            }
                        //                        }
                        //                        if (dr["nature"].ToString() == "承包商")
                        //                        {
                        //                            DataTable dt = departmentBLL.GetDataTable(string.Format(@"select D.FULLNAME,d.nature from BASE_DEPARTMENT O 
                        //LEFT JOIN BASE_DEPARTMENT D ON o.parentid = D.DEPARTMENTID where o.encode='{0}'", dr["deptcode"]));
                        //                            if (dt.Rows.Count > 0)
                        //                            {
                        //                                string name = "";
                        //                                foreach (DataRow dr2 in dt.Rows)
                        //                                {
                        //                                    if (dr1["nature"].ToString() == "承包商")
                        //                                    {
                        //                                        name += dr2["fullname"].ToString() + "/";
                        //                                    }
                        //                                }
                        //                                name += row["dept"].ToString();
                        //                                row["dept"] = name.TrimEnd('/');
                        //                            }
                        //                        }

                        DateTime checkDate = dr1["checkdate"].ToDate();
                        DateTime startDate = dr1["startdate"].ToDate();
                        DateTime endDate = dr1["enddate"].ToDate();

                        string[] arr = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
                        if (timeType == "1")
                        {
                            DateTime time1 = startDate.ToString("yyyy-MM-1").ToDate().AddMonths(1).AddDays(-1);
                            if (time1 >= endDate)
                            {

                                StringBuilder sb = new StringBuilder();
                                if (checkDate >= startDate && checkDate <= endDate)
                                {
                                    sb.AppendFormat("{0}[!^&]{1}", dr1["hiddescribe"], dr1["workstream"]);
                                }
                                row["one"] = sb.ToString();
                            }
                            else
                            {
                                int months = (endDate.Year - startDate.Year) * 12 + (endDate.Month - startDate.Month);
                                months = months > 10 ? 10 : months + 1;
                                for (int j = 1; j < months; j++)
                                {
                                    DateTime time = startDate.AddMonths(1);
                                    if (j == 1)
                                    {
                                        time = startDate.ToString("yyyy-MM-1").ToDate().AddMonths(1).AddDays(-1);
                                    }
                                    StringBuilder sb = new StringBuilder();
                                    if (checkDate >= startDate && checkDate <= time)
                                    {
                                        sb.AppendFormat("【{1}】{0}", dr1["hiddescribe"], dr1["workstream"]);
                                    }
                                    startDate = time;
                                    row[arr[j - 1]] = sb.ToString();
                                }
                            }
                        }
                        else
                        {
                            int w = startDate.DayOfWeek.GetHashCode();
                            TimeSpan ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
                            double weeks = ts.TotalDays / 7;
                            weeks = weeks > 10 ? 10 : weeks + 1;
                            for (int j = 1; j <= weeks; j++)
                            {
                                DateTime time = startDate.AddDays(7);
                                if (j == 1)
                                {
                                    if (w == 0)
                                    {
                                        time = startDate.ToString("yyyy-MM-dd 23:59:59").ToDate();
                                    }
                                    else
                                    {
                                        time = startDate.AddDays(7 - w);
                                    }
                                }
                                StringBuilder sb = new StringBuilder();
                                if (checkDate > startDate && checkDate <= time)
                                {
                                    sb.AppendFormat("【{1}】{0}", dr1["hiddescribe"], dr1["workstream"]);
                                }
                                startDate = time;
                                if (j - 1 < arr.Length)
                                {
                                    row[arr[j - 1]] = sb.ToString();
                                }

                            }
                        }
                        data.Rows.Add(row);
                        i++;
                    }
                    k++;
                }
                else
                {
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    DataRow row = data.NewRow();
                    row["nameid"] = dr["userid"];
                    //row["name"] = dr["realname"];
                    //row["dept"] = dr["deptname"];
                    //row["post"] = dr["postname"];
                    //row["hiddennum"] = dtHT.Select("userid='" + dr["userid"].ToString() + "'").Length;
                    //row["managerhidden"] = dtHT.Select("httype='1'").Length;
                    //row["entityhidden"] = dtHT.Select("httype='2'").Length;
                    row["ordernum"] = k;
                    //row["nature"] = dr["nature"];
                    //row["organizeid"] = user.OrganizeId;

                    //                    if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                    //                    {
                    //                        DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}') or encode='{0}' order by deptcode", dr["deptcode"], "部门"));
                    //                        if (dt.Rows.Count > 0)
                    //                        {
                    //                            string name = "";
                    //                            foreach (DataRow dr1 in dt.Rows)
                    //                            {
                    //                                name += dr1["fullname"].ToString() + "/";
                    //                            }
                    //                            row["dept"] = name.TrimEnd('/');
                    //                        }
                    //                    }
                    //                    if (dr["nature"].ToString() == "承包商")
                    //                    {
                    //                        DataTable dt = departmentBLL.GetDataTable(string.Format(@"select D.FULLNAME,d.nature from BASE_DEPARTMENT O 
                    //LEFT JOIN BASE_DEPARTMENT D ON o.parentid = D.DEPARTMENTID where o.encode='{0}'", dr["deptcode"]));
                    //                        if (dt.Rows.Count > 0)
                    //                        {
                    //                            string name = "";
                    //                            foreach (DataRow dr1 in dt.Rows)
                    //                            {
                    //                                if (dr1["nature"].ToString() == "承包商")
                    //                                {
                    //                                    name += dr1["fullname"].ToString() + "/";
                    //                                }
                    //                            }
                    //                            name += row["dept"].ToString();
                    //                            row["dept"] = name.TrimEnd('/');
                    //                        }
                    //                    }

                    data.Rows.Add(row);
                }
            }


            return data;
        }


        private DataTable GetDataSource1(DataTable data, string queryJson, int mode = 0)
        {
            string sql = "select distinct userid,realname,deptname,postname,nature,deptcode from V_CHECKNOTICE where 1=1 ";
            string where = "";
            var queryParam = queryJson.ToJObject();
            if (!queryParam["ExamineType"].IsEmpty())
            {
                where += string.Format(" and checktype='{0}'", queryParam["ExamineType"].ToString());
            }
            if (!queryParam["Year"].IsEmpty())
            {
                where += string.Format(" and year='{0}'", queryParam["Year"].ToString());
            }
            string timeType = "1";
            if (!queryParam["ShowType"].IsEmpty())
            {
                timeType = queryParam["ShowType"].ToString();
                where += string.Format(" and timetype='{0}'", queryParam["ShowType"].ToString());
            }
            if (!queryParam["StaffType"].IsEmpty())
            {
                string type = queryParam["StaffType"].ToString();
                if (type == "1")
                {
                    where += string.Format(" and ((nature='部门' and rolename like '%负责人%') or (nature='承包商' and rolename like '%负责人%') or (nature='分包商' and rolename like '%负责人%'))");
                }
                if (type == "2")
                {
                    where += string.Format(" and rolename like '%专工%'");
                }
                if (type == "3")
                {
                    where += string.Format(" and ((nature='班组' and rolename like '%负责人%') or (nature='承包商' and (postname='班组长' or postname='班长')) or (nature='分包商' and (postname='班组长' or postname='班长')))");
                }
                if (type == "4")
                {
                    where += string.Format(" and ((nature='班组' and rolename like '%普通用户%' and rolename not like '%负责人%') or (nature='承包商' and rolename='普通用户') or (nature='分包商' and rolename='普通用户'))");


                }

            }
            sql += where;
            int k = 1;
            DataTable dtItems = departmentBLL.GetDataTable(sql);

            sql = string.Format("select userid,httype,checkdate,startdate from V_CHECKNOTICE where 1=1 {0}", where);
            if (timeType == "1")
            {
                sql += string.Format(" and CHECKDATE<add_months(startdate,6)");
            }
            if (timeType == "2")
            {
                sql += string.Format(" and CHECKDATE<add_months(startdate,3)");
            }
            if (mode == 0)
            {

                int count = data.Rows.Count;
                if (count > 0 && dtItems.Rows.Count > 0)
                {
                    int sort = data.Rows[count - 1]["ordernum"].ToInt();
                    k = sort;
                    if (data.Rows[count - 1]["nameid"].ToString() == dtItems.Rows[0]["userid"].ToString())
                    {
                        k = sort;
                    }
                    else
                    {
                        k++;
                    }
                }
            }
            //nameid,t.ordernum,t.name,t.dept,t.deptcode,t.post,t.hiddennum,t.managerhidden,t.entityhidden,d.nature,d.organizeid
            foreach (DataRow dr in dtItems.Rows)
            {
                string sql1 = sql + string.Format(" and userid='{0}'", dr["userid"].ToString());
                DataTable dtHT = departmentBLL.GetDataTable(sql1);
                DataRow newRow = data.NewRow();
                newRow["nameid"] = dr["userid"].ToString();
                newRow["name"] = dr["realname"].ToString();
                newRow["dept"] = dr["deptname"].ToString();
                newRow["deptcode"] = dr["deptcode"].ToString();
                newRow["post"] = dr["postname"].ToString();

                newRow["nature"] = dr["nature"].ToString();
                newRow["organizeid"] = curUser.OrganizeId;


                var ds = dtHT.AsEnumerable();
                if (timeType == "1")
                {
                    newRow["hiddennum"] = ds.Where(t => t.Field<string>("userid") == dr["userid"].ToString() && t.Field<DateTime>("checkdate") <= t.Field<DateTime>("startdate").AddMonths(6)).Count();
                    newRow["managerhidden"] = ds.Where(t => t.Field<string>("httype") == "1" && t.Field<DateTime>("checkdate") <= t.Field<DateTime>("startdate").AddMonths(6)).Count();
                    newRow["entityhidden"] = ds.Where(t => t.Field<string>("httype") == "2" && t.Field<DateTime>("checkdate") <= t.Field<DateTime>("startdate").AddMonths(6)).Count();
                }
                else
                {
                    newRow["hiddennum"] = ds.Where(t => t.Field<string>("userid") == dr["userid"].ToString() && t.Field<DateTime>("checkdate") <= t.Field<DateTime>("startdate").AddDays(70)).Count();
                    newRow["managerhidden"] = ds.Where(t => t.Field<string>("httype") == "1" && t.Field<DateTime>("checkdate") <= t.Field<DateTime>("startdate").AddDays(70)).Count();
                    newRow["entityhidden"] = ds.Where(t => t.Field<string>("httype") == "2" && t.Field<DateTime>("checkdate") <= t.Field<DateTime>("startdate").AddDays(70)).Count();
                }

                data.Rows.Add(newRow);
            }
            return data;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPublicityList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "distinct nameid";
                pagination.p_fields = "t.ordernum,t.name,t.dept,t.deptcode,t.post,t.hiddennum,t.managerhidden,t.entityhidden,d.nature,d.organizeid";
                pagination.p_tablename = " HRS_PUBLICITYDETAILS t left join HRS_PUBLICITY p on t.mainid=p.id  left join BASE_DEPARTMENT d on t.DeptCode=d.encode";
                pagination.conditionJson = string.Format(" p.CreateUserOrgCode='{0}'", user.OrganizeCode);
                pagination.sidx = "ordernum";//排序字段
                pagination.sord = "asc";//排序方式 
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                //查询条件 安全检查类型
                string ExamineType = dy.data.ExamineType;
                if (!string.IsNullOrEmpty(ExamineType))
                {
                    pagination.conditionJson += string.Format(" and p.ExamineType='{0}'", ExamineType);
                }

                //查询条件 年度
                string Year = dy.data.Year;
                if (!string.IsNullOrEmpty(Year))
                {
                    pagination.conditionJson += string.Format(" and p.Year='{0}'", Year);
                }
                //查询条件 展示类别  1 按周  2 按月
                string ShowType = dy.data.ShowType;
                if (!string.IsNullOrEmpty(ShowType))
                {
                    pagination.conditionJson += string.Format(" and p.ShowType='{0}'", ShowType);
                }
                //查询条件 人员类别
                string StaffType = dy.data.StaffType;
                if (!string.IsNullOrEmpty(StaffType))
                {
                    pagination.conditionJson += string.Format(" and p.StaffType='{0}'", StaffType);
                }

                var data = Publicitybll.GetPageList(pagination, null);
                foreach (DataRow dr in data.Rows)
                {
                    if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                    {
                        DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode", dr["deptcode"], "部门", dr["organizeid"]));
                        if (dt.Rows.Count > 0)
                        {
                            string name = "";
                            foreach (DataRow dr1 in dt.Rows)
                            {
                                name += dr1["fullname"].ToString() + "/";
                            }
                            dr["dept"] = name.TrimEnd('/');
                        }
                    }
                    if (dr["nature"].ToString() == "承包商")
                    {
                        DataTable dt = departmentBLL.GetDataTable(string.Format(@"select D.FULLNAME,d.nature from BASE_DEPARTMENT O 
LEFT JOIN BASE_DEPARTMENT D ON o.parentid = D.DEPARTMENTID where o.encode='{0}' and o.organizeid='{1}'", dr["deptcode"], dr["organizeid"]));
                        if (dt.Rows.Count > 0)
                        {
                            string name = "";
                            foreach (DataRow dr1 in dt.Rows)
                            {
                                if (dr1["nature"].ToString() == "承包商")
                                {
                                    name += dr1["fullname"].ToString() + "/";
                                }
                            }
                            name += dr["dept"].ToString();
                            dr["dept"] = name.TrimEnd('/');
                        }
                    }
                }
                ExpandoObject obj = dy.data;
                DataTable dataSource = GetDataSource1(data, Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                return new { code = 0, info = "获取数据成功", count = dataSource.Rows.Count, data = dataSource };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPublicityDetails([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }


                Pagination pagination = new Pagination();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "t.Id";
                pagination.p_fields = "One,Two,Three,Four,Five,Six,Seven,Eight,Nine,Ten,DetailsOrderNum,nameid,ordernum";
                pagination.p_tablename = " HRS_PUBLICITYDETAILS t left join HRS_PUBLICITY p on t.mainid=p.id ";
                pagination.conditionJson = string.Format(" p.CreateUserOrgCode='{0}'", user.OrganizeCode);
                pagination.sidx = "ordernum";//排序字段
                pagination.sord = "asc";//排序方式 
                pagination.page = 1;//页数
                pagination.rows = 99999999;//行数
                //查询条件 安全检查类型
                string ExamineType = dy.data.ExamineType;
                if (!string.IsNullOrEmpty(ExamineType))
                {
                    pagination.conditionJson += string.Format(" and p.ExamineType='{0}'", ExamineType);
                }

                //查询条件 年度
                string Year = dy.data.Year;
                if (!string.IsNullOrEmpty(Year))
                {
                    pagination.conditionJson += string.Format(" and p.Year='{0}'", Year);
                }
                //查询条件 展示类别  1 按周  2 按月
                string ShowType = dy.data.ShowType;
                if (!string.IsNullOrEmpty(ShowType))
                {
                    pagination.conditionJson += string.Format(" and p.ShowType='{0}'", ShowType);
                }
                //查询条件 人员类别
                string StaffType = dy.data.StaffType;
                if (!string.IsNullOrEmpty(StaffType))
                {
                    pagination.conditionJson += string.Format(" and p.StaffType='{0}'", StaffType);
                }
                //查询条件 人员
                string NameId = dy.data.NameId;
                if (!string.IsNullOrEmpty(NameId))
                {
                    pagination.conditionJson += string.Format(" and t.NameId='{0}'", NameId);
                }
                //查询条件 部门
                string DeptCode = dy.data.DeptCode;
                if (!string.IsNullOrEmpty(DeptCode))
                {
                    pagination.conditionJson += string.Format(" and t.DeptCode='{0}'", DeptCode);
                }
                var data = Publicitybll.GetPageList(pagination, null);
                ExpandoObject obj = dy.data;
                DataTable dataSource = GetDataSource(data, Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                return new { code = 0, info = "获取数据成功", count = dataSource.Rows.Count, data = dataSource };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetExamineTypeDataJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                //集合
                var data = dataitemdetailbll.GetDataItemListByItemCode("'ExamineType'");


                var treeList = new List<TreeEntity>();
                foreach (DataItemModel item in data)
                {
                    TreeEntity tree = new TreeEntity();
                    bool hasChildren = data.Where(p => p.ItemCode == item.ItemValue).ToList().Count() == 0 ? false : true;
                    tree.id = item.ItemValue;
                    tree.text = item.ItemName;
                    tree.value = item.ItemValue;
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.parentId = item.ItemCode;
                    tree.Attribute = "Code";
                    tree.AttributeValue = item.ItemValue;
                    treeList.Add(tree);
                }
                if (treeList.Count > 0)
                {
                    treeList[0].isexpand = true;
                }

                return new { code = 0, info = "获取数据成功", data = treeList };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
    }
}