using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using System;
using System.Data;
using System.Web;
using ERCHTMS.Cache;
using ERCHTMS.Busines.BaseManage;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.Extension;
using System.Text;
using Aspose.Cells;
namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 描 述：
    /// </summary>
    public class PublicityController : MvcControllerBase
    {
        private PublicityBLL Publicitybll = new PublicityBLL();
        private PublicityDetailsBLL PublicityDetailsbll = new PublicityDetailsBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private UserBLL userbll = new UserBLL();
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.examineTypeFirst = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel examineType = dataitemdetailbll.GetDataItemListByItemCode("'ExamineType'").ToList().FirstOrDefault();
            if (examineType != null)
                ViewBag.examineTypeFirst = examineType.ItemValue;
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            ViewBag.examineTypeFirst = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel examineType = dataitemdetailbll.GetDataItemListByItemCode("'ExamineType'").ToList().FirstOrDefault();
            if (examineType != null)
                ViewBag.examineTypeFirst = examineType.ItemValue;
            return View();
        }
        #endregion

        #region 获取数据
        private DataTable GetDataSource(DataTable data, string queryJson, int mode = 0)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
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
                    where += string.Format(" and ((nature='班组' and rolename like '%负责人%') or (nature='承包商' and (postname='班组长' or postname='班长')) or (nature='分包商' and (postname='班组长'or postname='班长')))");
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
            if(timeType=="1")
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
                if(mode==0)
                {
                    foreach (DataRow dr1 in dtHT.Rows)
                    {
                        DataRow row = data.NewRow();
                        DateTime checkDate = dr1["checkdate"].ToDate();
                        DateTime startDate = dr1["startdate"].ToDate();
                        DateTime endDate = dr1["enddate"].ToDate();

                        row["id"] = Guid.NewGuid().ToString();
                        row["nameid"] = dr["userid"];
                        row["name"] = dr["realname"];
                        row["dept"] = dr["deptname"];
                        row["post"] = dr["postname"];
                        var ds = dtHT.AsEnumerable();
                        //row["hiddennum"] = dtHT.Select("userid='" + dr["userid"].ToString() + "'").Length;
                       
                        if(timeType=="1")
                        {
                            row["hiddennum"] = ds.Where(t => t.Field<string>("userid") == dr["userid"].ToString() && t.Field<DateTime>("checkdate") <= startDate.AddMonths(6)).Count();
                            row["managerhidden"] = ds.Where(t => t.Field<string>("httype") == "1" && t.Field<DateTime>("checkdate") <= startDate.AddMonths(6)).Count();
                            row["entityhidden"] = ds.Where(t => t.Field<string>("httype") == "2" && t.Field<DateTime>("checkdate") <= startDate.AddMonths(6)).Count();
                        }
                        else
                        {
                            row["hiddennum"] = ds.Where(t => t.Field<string>("userid") == dr["userid"].ToString() && t.Field<DateTime>("checkdate") <= startDate.AddDays(70)).Count();
                            row["managerhidden"] = ds.Where(t => t.Field<string>("httype") == "1" && t.Field<DateTime>("checkdate") <= startDate.AddDays(70)).Count();
                            row["entityhidden"] = ds.Where(t => t.Field<string>("httype") == "2" && t.Field<DateTime>("checkdate") <= startDate.AddDays(70)).Count();
                        }
                        //row["managerhidden"] = dtHT.Select("httype='1'").Length;
                        //row["entityhidden"] = dtHT.Select("httype='2'").Length;
                        row["detailsordernum"] = i;
                        row["ordernum"] = k;

                        if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                        {
                            DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}') or encode='{0}' order by deptcode", dr["deptcode"], "部门"));
                            if (dt.Rows.Count > 0)
                            {
                                string name = "";
                                foreach (DataRow dr2 in dt.Rows)
                                {
                                    name += dr2["fullname"].ToString() + "/";
                                }
                                row["dept"] = name.TrimEnd('/');
                            }
                        }
                        if (dr["nature"].ToString() == "承包商")
                        {
                            DataTable dt = departmentBLL.GetDataTable(string.Format(@"select D.FULLNAME,d.nature from BASE_DEPARTMENT O 
LEFT JOIN BASE_DEPARTMENT D ON o.parentid = D.DEPARTMENTID where o.encode='{0}'", dr["deptcode"]));
                            if (dt.Rows.Count > 0)
                            {
                                string name = "";
                                foreach (DataRow dr2 in dt.Rows)
                                {
                                    if (dr1["nature"].ToString() == "承包商")
                                    {
                                        name += dr2["fullname"].ToString() + "/";
                                    }
                                }
                                name += row["dept"].ToString();
                                row["dept"] = name.TrimEnd('/');
                            }
                        }

                      

                        string[] arr = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
                        if (timeType == "1")
                        {
                            DateTime time1 = startDate.ToString("yyyy-MM-1").ToDate().AddMonths(1).AddDays(-1);
                            if (time1 >= endDate)
                            {

                                StringBuilder sb = new StringBuilder();
                                if (checkDate >= startDate && checkDate <= endDate)
                                {
                                    sb.AppendFormat("【{1}】{0}", dr1["hiddescribe"], dr1["workstream"]);
                                }
                                row["one"] = sb.ToString();
                            }
                            else
                            {
                                int months = (endDate.Year - startDate.Year) * 12 + (endDate.Month - startDate.Month);
                                months = months > 6 ? 6 : months + 1;
                                for (int j = 1; j < months; j++)
                                {
                                    DateTime time = startDate.AddMonths(1);
                                    if (j==1)
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
                            int w=startDate.DayOfWeek.GetHashCode();
                            TimeSpan ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
                            double weeks = ts.TotalDays / 7;
                            weeks = weeks > 10 ? 10 : weeks+1;
                            for (int j = 1; j <= weeks; j++)
                            {
                                DateTime time = startDate.AddDays(7);  
                                if(j==1)
                                {
                                    if (w==0)
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
                     int num= dtHT.Select("userid='" + dr["userid"].ToString() + "'").Length;
                    if (num==0)
                    {
                        continue;
                    }
                  
                    DataRow row = data.NewRow();
                    row["nameid"] = dr["userid"];
                    row["name"] = dr["realname"];
                    row["dept"] = dr["deptname"];
                    row["post"] = dr["postname"];
                    row["hiddennum"] = num;
                    row["managerhidden"] = dtHT.Select("httype='1'").Length;
                    row["entityhidden"] = dtHT.Select("httype='2'").Length;
                    row["ordernum"] = k;
                    row["nature"] = dr["nature"];
                    row["organizeid"] =user.OrganizeId;

                    if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                    {
                        DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}') or encode='{0}' order by deptcode", dr["deptcode"], "部门"));
                        if (dt.Rows.Count > 0)
                        {
                            string name = "";
                            foreach (DataRow dr1 in dt.Rows)
                            {
                                name += dr1["fullname"].ToString() + "/";
                            }
                            row["dept"] = name.TrimEnd('/');
                        }
                    }
                    if (dr["nature"].ToString() == "承包商")
                    {
                        DataTable dt = departmentBLL.GetDataTable(string.Format(@"select D.FULLNAME,d.nature from BASE_DEPARTMENT O 
LEFT JOIN BASE_DEPARTMENT D ON o.parentid = D.DEPARTMENTID where o.encode='{0}'", dr["deptcode"]));
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
                            name += row["dept"].ToString();
                            row["dept"] = name.TrimEnd('/');
                        }
                    }

                    data.Rows.Add(row);
                }
            }

           
            return data;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                queryJson = queryJson ?? "";
                pagination.p_kid = "t.Id";
                pagination.p_fields = "t.One,t.Two,t.Three,t.Four,t.Five,t.Six,t.Seven,t.Eight,t.Nine,t.Ten,t.Name,t.NameId,t.Dept,t.DeptCode,t.Post,t.ManagerHidden,t.EntityHidden,t.HiddenNum,t.SortNum,t.OrderNum,t.DetailsOrderNum,d.nature,d.organizeid";
                pagination.p_tablename = " HRS_PUBLICITYDETAILS t left join HRS_PUBLICITY p on t.mainid=p.id left join BASE_DEPARTMENT d on t.DeptCode=d.encode ";
                pagination.conditionJson = string.Format(" p.CreateUserOrgCode='{0}'", user.OrganizeCode);
                var watch = CommonHelper.TimerStart();
                var data = Publicitybll.GetPageList(pagination, queryJson);
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
LEFT JOIN BASE_DEPARTMENT D ON o.parentid = D.DEPARTMENTID where o.encode='{0}' and o.organizeid='{1}'", dr["deptcode"],  dr["organizeid"]));
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
                DataTable dataSource = GetDataSource(data,queryJson);
                var jsonData = new
                {
                    rows = dataSource,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPublicityDefault()
        {
            try
            {
                
                DataTable dtItems = departmentBLL.GetDataTable(string.Format(@"select ExamineType,b.itemname ExamineTypeName,Year,ShowType,StaffType,t.createdate from HRS_PUBLICITY t
left join base_dataitemdetail b on b.itemvalue=t.ExamineType and b.itemid=
(select c.itemid from base_dataitem c where c.itemcode='ExamineType')
 where t.createdate=(select max(a.createdate) from HRS_PUBLICITY a) order by StaffType asc"));

                DataTable dtHt = departmentBLL.GetDataTable(string.Format("select d.checktype,d.timetype,d.year,d.CHECKDATE,d.ROLENAME,d.nature,d.postname from v_checknotice d order by CHECKDATE desc"));
                string examineType = "", year = "", showType = "", staffType="";
                if (dtItems.Rows.Count==0)
                {
                    if (dtHt.Rows.Count>0)
                    {
                        examineType = dtHt.Rows[0]["checktype"].ToString();
                        showType = dtHt.Rows[0]["timetype"].ToString();
                        year = dtHt.Rows[0]["year"].ToString();
                        string postName= dtHt.Rows[0]["postName"].ToString();
                        string nature= dtHt.Rows[0]["nature"].ToString();
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
                        return Content(null);
                    }
                   
                }
                else
                {
                    examineType = dtItems.Rows[0][0].ToString();
                    year = dtItems.Rows[0][2].ToString();
                    showType = dtItems.Rows[0][3].ToString();
                    staffType = dtItems.Rows[0][4].ToString();
                    if (dtHt.Rows.Count > 0)
                    {
                        DateTime time1 = dtItems.Rows[0]["createdate"].ToDate();
                        DateTime time2 = dtHt.Rows[0]["checkdate"].ToDate();
                        if (time1<time2)
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
                var jsonData = new
                {
                    ExamineType = examineType,
                    Year = year,
                    ShowType = showType,
                    StaffType = staffType
                };
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(null);
            }
        }
        /// <summary>
        /// 首页接口
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPublicityList(string queryJson)
        {
            Pagination pagination = new Pagination();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            queryJson = queryJson ?? "";
            pagination.p_kid = "";
            pagination.p_fields = "distinct t.ordernum,t.name,t.nameid,t.dept,t.deptcode,t.post,t.hiddennum,t.managerhidden,t.entityhidden,d.nature,d.organizeid";
            pagination.p_tablename = " HRS_PUBLICITYDETAILS t left join HRS_PUBLICITY p on t.mainid=p.id  left join BASE_DEPARTMENT d on t.DeptCode=d.encode";
            pagination.conditionJson = string.Format(" p.CreateUserOrgCode='{0}'", user.OrganizeCode);
            pagination.sidx = "ordernum";//排序字段
            pagination.sord = "asc";//排序方式 
            pagination.page = 1;//页数
            pagination.rows = 99999999;//行数
            var watch = CommonHelper.TimerStart();
            var data = Publicitybll.GetPageList(pagination, queryJson);
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
            DataTable dataSource = GetDataSource(data, queryJson,1);
            var jsonData = new
            {
                dt = dataSource
            };
            return Content(jsonData.ToJson());
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = Publicitybll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Publicitybll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #region 获取隐患类型树结构
        /// <summary>
        /// 获取检查类型树结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExamineTypeDataJson()
        {
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
            return Content(treeList.TreeToJson());

        }
        #endregion
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            Publicitybll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PublicityEntity entity)
        {
            Publicitybll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 删除表单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult deleteForm(string examineType, string year, string showType, string staffType)
        {
            try
            {
                var currUser = OperatorProvider.Provider.Current();
                string sqldel = string.Format(@"delete from HRS_PUBLICITYDETAILS t where 
mainid=(select id from HRS_PUBLICITY p where p.CreateUserOrgCode='{0}' and p.ExamineType='{1}' and p.Year='{2}' and p.ShowType='{3}' and p.StaffType='{4}')", currUser.OrganizeCode, examineType, year, showType, staffType);
                Publicitybll.ExecuteBySql(sqldel);
                string sqldel1 = string.Format(@"delete from HRS_PUBLICITY p where p.CreateUserOrgCode='{0}' and p.ExamineType='{1}' and p.Year='{2}' and p.ShowType='{3}' and p.StaffType='{4}'", currUser.OrganizeCode, examineType, year, showType, staffType);
                Publicitybll.ExecuteBySql(sqldel1);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                return Error("删除失败。");
            }
        }
        /// <summary>
        /// 导入消防设施
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        // [ValidateAntiForgeryToken]
        public string ImportPublicity()
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            var currUser = OperatorProvider.Provider.Current();
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司          
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            string examineType = HttpContext.Request.Form["ExamineType"];
            string year = HttpContext.Request.Form["Year"];
            string showType = HttpContext.Request.Form["ShowType"];
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }

                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                //部门主任（项目经理）
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);
                //专业管理人员
                Aspose.Cells.Cells cells1 = wb.Worksheets[1].Cells;
                DataTable dt1 = cells1.ExportDataTable(0, 0, cells1.MaxDataRow + 1, cells1.MaxColumn + 1, true);
                //班组长
                Aspose.Cells.Cells cells2 = wb.Worksheets[2].Cells;
                DataTable dt2 = cells2.ExportDataTable(0, 0, cells2.MaxDataRow + 1, cells2.MaxColumn + 1, true);
                //班组
                Aspose.Cells.Cells cells3 = wb.Worksheets[3].Cells;
                DataTable dt3 = cells3.ExportDataTable(0, 0, cells3.MaxDataRow + 1, cells3.MaxColumn + 1, true);

                string orgid = OperatorProvider.Provider.Current().OrganizeId;

                int sortNum = 0;
                int sort = 0;

                #region 部门主任（项目经理）
                string falseMessage1 = getImport(dt,1, examineType, year, showType,out sort, sortNum);
                sortNum = sort;
                if (falseMessage1 != "") { error++; }
                #endregion


                #region 专业管理人员
                string falseMessage2 = getImport(dt1, 2, examineType, year, showType, out sort, sortNum);
                sortNum = sort;
                if (falseMessage2 != "") { error++; }
                #endregion

                #region 班组长
                string falseMessage3 = getImport(dt2, 3, examineType, year, showType, out sort, sortNum);
                sortNum = sort;
                if (falseMessage3 != "") { error++; }
                #endregion

                #region 班组
                string falseMessage4 = getImport1(dt3, 4, examineType, year, showType, out sort, sortNum);
                sortNum = sort;
                if (falseMessage4 != "") { error++; }
                #endregion
                count = sortNum;
                falseMessage = falseMessage1 + falseMessage2 + falseMessage3 + falseMessage4;
                message = "共有4张公示表,成功导入" + (4 - error) + "张，失败" + error + "张";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        public string getImport(DataTable dt,int staffType,string examineType, string year, string showType, out int sort, int sortNum)
        {
            var currUser = OperatorProvider.Provider.Current();
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司          
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";

            string mainid = Guid.NewGuid().ToString();
            //int sortNum = 1;
            int orderNum = 1;
            int detailsOrderNum = 1;

            string Name = "";
            string NameId = "";
            string post = "";
            string Dept = "";
            string DeptCode = "";
            string dname = string.Empty;
            int ManagerHidden = 0;
            int EntityHidden = 0;
            int HiddenNum = 0;

            int order = 2;
            string staffTypeName = "";
            if (staffType == 1) { staffTypeName = "部门主任（项目经理）"; }
            if (staffType == 2) { staffTypeName = "专业管理人员"; }
            if (staffType == 3) { staffTypeName = "班组长"; }
            if (staffType == 4) { staffTypeName = "班组"; }
            List<PublicityDetailsEntity> projects = new List<PublicityDetailsEntity>();
            for (int i = 2; i < dt.Rows.Count; i++)
            {
                order = i + 1;
                PublicityDetailsEntity item = new PublicityDetailsEntity();
                sortNum++;
                
                item.SortNum = sortNum;
                item.MainId = mainid;

                #region 编号
                if (dt.Rows[i][0].ToString() != "")
                {
                    string OrderNum = dt.Rows[i][0].ToString();
                    int tempOrderNum;
                    if (!string.IsNullOrEmpty(OrderNum))
                        if (int.TryParse(OrderNum, out tempOrderNum))
                            orderNum = tempOrderNum;
                        else
                        {
                            falseMessage += string.Format(@"【{1}】第{0}行,编号必须为数字！</br>", order, staffTypeName);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,编号不能为空！</br>", order, staffTypeName);
                        error++;
                        continue;
                    }
                }
                item.OrderNum = orderNum;
                #endregion

               

                #region 部门
                if (dt.Rows[i][2].ToString() != "")
                {
                    string deptlist = dt.Rows[i][2].ToString().Trim();
                    var p1 = string.Empty; var p2 = string.Empty;
                    bool isSkip = false;
                    dname = string.Empty;
                    var array = deptlist.Split('/');
                    for (int j = 0; j < array.Length; j++)
                    {
                        if (j == 0)
                        {
                            var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "厂级" && x.FullName == array[j].ToString()).FirstOrDefault();
                            if (entity == null)
                            {
                                entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "部门" && x.FullName == array[j].ToString()).FirstOrDefault();
                                if (entity == null)
                                {
                                    entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "承包商" && x.FullName == array[j].ToString()).FirstOrDefault();
                                    if (entity == null)
                                    {
                                        falseMessage += string.Format(@"【{1}】第{0}行,部门不存在！</br>", order, staffTypeName);
                                        error++;
                                        isSkip = true;
                                        break;
                                    }
                                    else
                                    {
                                        DeptCode = entity.EnCode;
                                        Dept = entity.FullName;
                                        p1 = entity.DepartmentId;
                                    }
                                }
                                else
                                {
                                    DeptCode = entity.EnCode;
                                    Dept = entity.FullName;
                                    p1 = entity.DepartmentId;
                                }
                            }
                            else
                            {
                                DeptCode = entity.EnCode;
                                Dept = entity.FullName;
                                p1 = entity.DepartmentId;
                            }
                        }
                        else if (j == 1)
                        {
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "专业" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                            if (entity1 == null)
                            {
                                entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "班组" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                if (entity1 == null)
                                {
                                    isSkip = true;
                                    falseMessage += string.Format(@"【{1}】第{0}行,部门不存在！</br>", order, staffTypeName);
                                    error++;
                                    break;
                                }
                                else
                                {
                                    DeptCode = entity1.EnCode;
                                    Dept = entity1.FullName;
                                    p2 = entity1.DepartmentId;
                                }
                            }
                            else
                            {
                                DeptCode = entity1.EnCode;
                                Dept = entity1.FullName;
                                p2 = entity1.DepartmentId;
                                if (entity1.Nature == "承包商")
                                {
                                    if (!string.IsNullOrEmpty(p1))
                                    {
                                        var d = departmentBLL.GetEntity(p1);
                                        if (d != null)
                                        {
                                            dname = d.FullName;
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                            if (entity1 == null)
                            {
                                falseMessage += string.Format(@"【{1}】第{0}行,部门不存在！</br>", order, staffTypeName);
                                error++;
                                isSkip = true;
                                break;
                            }
                            else
                            {
                                DeptCode = entity1.EnCode;
                                Dept = entity1.FullName;
                                if (entity1.Nature == "承包商")
                                {
                                    if (!string.IsNullOrEmpty(p2))
                                    {
                                        var d = departmentBLL.GetEntity(p2);
                                        if (d != null)
                                        {
                                            dname = d.FullName;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (isSkip)
                    {
                        continue;
                    }
                }
                item.Dept = Dept;
                item.DeptCode = DeptCode;
                #endregion

                #region 名称
                if (dt.Rows[i][1].ToString() != "")
                {
                    UserInfoEntity userEntity = userbll.GetUserInfoByName(Dept, dt.Rows[i][1].ToString().Trim());
                    if (!string.IsNullOrEmpty(dname))
                    {
                        userEntity = userbll.GetUserInfoByName(dname, dt.Rows[i][1].ToString().Trim());
                    }
                    if (userEntity == null)
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,姓名在【{2}】不存在！</br>", order, staffTypeName, item.Dept);
                        error++;
                        continue;
                    }
                    else
                    {
                        Name = dt.Rows[i][1].ToString();
                        NameId = userEntity.UserId;
                        post = new PostBLL().GetEntity(userEntity.PostId) == null ? "" : new PostBLL().GetEntity(userEntity.PostId).FullName;
                    }
                }
                item.Name = Name;
                item.NameId = NameId;
                item.Post = post;
                #endregion

                #region 隐患总数
                if (dt.Rows[i][3].ToString() != "")
                {
                    string hiddenNum = dt.Rows[i][3].ToString();
                    int tempHiddenNum;
                    if (!string.IsNullOrEmpty(hiddenNum))
                        if (int.TryParse(hiddenNum, out tempHiddenNum))
                            HiddenNum = tempHiddenNum;
                        else
                        {
                            falseMessage += string.Format(@"【{1}】第{0}行,隐患总数必须为数字！</br>", order, staffTypeName);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,隐患总数不能为空！</br>", order, staffTypeName);
                        error++;
                        continue;
                    }
                }
                item.HiddenNum = HiddenNum;
                #endregion

                #region 管理隐患
                if (dt.Rows[i][4].ToString() != "")
                {
                    string managerHidden = dt.Rows[i][4].ToString();
                    int tempManagerHidden;
                    if (!string.IsNullOrEmpty(managerHidden))
                        if (int.TryParse(managerHidden, out tempManagerHidden))
                            ManagerHidden = tempManagerHidden;
                        else
                        {
                            falseMessage += string.Format(@"【{1}】第{0}行,管理隐患必须为数字！</br>", order, staffTypeName);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,管理隐患不能为空！</br>", order, staffTypeName);
                        error++;
                        continue;
                    }
                }
                item.ManagerHidden = ManagerHidden;
                #endregion

                #region 实体隐患
                if (dt.Rows[i][5].ToString() != "")
                {
                    string entityHidden = dt.Rows[i][5].ToString();
                    int tempEntityHidden;
                    if (!string.IsNullOrEmpty(entityHidden))
                        if (int.TryParse(entityHidden, out tempEntityHidden))
                            EntityHidden = tempEntityHidden;
                        else
                        {
                            falseMessage += string.Format(@"【{1}】第{0}行,实体隐患必须为数字！</br>", order, staffTypeName);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,实体隐患不能为空！</br>", order, staffTypeName);
                        error++;
                        continue;
                    }
                }
                item.EntityHidden = EntityHidden;
                #endregion

                #region 详情序号
                if (dt.Rows[i][6].ToString() != "")
                {
                    string detailsOrderNum1 = dt.Rows[i][6].ToString();
                    int tempDetailsOrderNum;
                    if (!string.IsNullOrEmpty(detailsOrderNum1))
                        if (int.TryParse(detailsOrderNum1, out tempDetailsOrderNum))
                            item.DetailsOrderNum = tempDetailsOrderNum;
                        else
                        {
                            falseMessage += string.Format(@"【{1}】第{0}行,序号必须为数字！</br>", order, staffTypeName);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,序号不能为空！</br>", order, staffTypeName);
                        error++;
                        continue;
                    }
                }
                #endregion


                #region 一
                item.One = dt.Rows[i][7].ToString();
                #endregion

                #region 二
                item.Two = dt.Rows[i][8].ToString();
                #endregion

                #region 三
                item.Three = dt.Rows[i][9].ToString();
                #endregion

                #region 四
                item.Four = dt.Rows[i][10].ToString();
                #endregion

                #region 五
                item.Five = dt.Rows[i][11].ToString();
                #endregion

                #region 六
                item.Six = dt.Rows[i][12].ToString();
                #endregion

                if (showType == "2")
                {
                    #region 七
                    item.Seven = dt.Rows[i][13].ToString();
                    #endregion

                    #region 八
                    item.Eight = dt.Rows[i][14].ToString();
                    #endregion

                    #region 九
                    item.Nine = dt.Rows[i][15].ToString();
                    #endregion

                    #region 十
                    item.Ten = dt.Rows[i][16].ToString();
                    #endregion
                }

                //try
                //{
                //    PublicityDetailsbll.SaveForm("", item);
                //}
                //catch
                //{
                //    error++;
                //}
                projects.Add(item);

            }
            if (projects.Count > 0)
            {
                if (projects.Count < dt.Rows.Count - 2)
                {
                    falseMessage += string.Format(@"【{1}】导入失败！</br>", order, staffTypeName);
                }
                else
                {
                    if (error == 0)
                    {
                        foreach (var item in projects)
                        {
                            PublicityDetailsbll.SaveForm("", item);
                        }
                        PublicityEntity entity = new PublicityEntity();
                        entity.ExamineType = examineType;
                        entity.Year = Convert.ToInt32(year);
                        entity.ShowType = Convert.ToInt32(showType);
                        entity.StaffType = staffType;
                        try
                        {
                            string sqldel = string.Format(@"delete from HRS_PUBLICITYDETAILS t where 
mainid=(select id from HRS_PUBLICITY p where p.CreateUserOrgCode='{0}' and p.ExamineType='{1}' and p.Year='{2}' and p.ShowType='{3}' and p.StaffType='{4}')", currUser.OrganizeCode, examineType, year, showType, staffType);
                            Publicitybll.ExecuteBySql(sqldel);
                            string sqldel1 = string.Format(@"delete from HRS_PUBLICITY p where p.CreateUserOrgCode='{0}' and p.ExamineType='{1}' and p.Year='{2}' and p.ShowType='{3}' and p.StaffType='{4}'", currUser.OrganizeCode, examineType, year, showType, staffType);
                            Publicitybll.ExecuteBySql(sqldel1);
                            Publicitybll.SaveForm(mainid, entity);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            sort = sortNum;
            return falseMessage;
        }
        public string getImport1(DataTable dt, int staffType, string examineType, string year, string showType, out int sort, int sortNum)
        {
            var currUser = OperatorProvider.Provider.Current();
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司          
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";

            string mainid = Guid.NewGuid().ToString();
            //int sortNum = 1;
            int orderNum = 1;
            int detailsOrderNum = 1;

            string Name = "";
            string NameId = "";
            string post = "";
            string Dept = "";
            string DeptCode = "";
            int ManagerHidden = 0;
            int EntityHidden = 0;
            int HiddenNum = 0;

            int order = 2;
            string staffTypeName = "";
            if (staffType == 1) { staffTypeName = "部门主任（项目经理）"; }
            if (staffType == 2) { staffTypeName = "专业管理人员"; }
            if (staffType == 3) { staffTypeName = "班组长"; }
            if (staffType == 4) { staffTypeName = "班组"; }
            List<PublicityDetailsEntity> projects = new List<PublicityDetailsEntity>();
            for (int i = 2; i < dt.Rows.Count; i++)
            {
                order = i + 1;
                PublicityDetailsEntity item = new PublicityDetailsEntity();
                sortNum++;

                item.SortNum = sortNum;
                item.MainId = mainid;

                #region 编号
                if (dt.Rows[i][0].ToString() != "")
                {
                    string OrderNum = dt.Rows[i][0].ToString();
                    int tempOrderNum;
                    if (!string.IsNullOrEmpty(OrderNum))
                        if (int.TryParse(OrderNum, out tempOrderNum))
                            orderNum = tempOrderNum;
                        else
                        {
                            falseMessage += string.Format(@"【{1}】第{0}行,编号必须为数字！</br>", order, staffTypeName);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,编号不能为空！</br>", order, staffTypeName);
                        error++;
                        continue;
                    }
                }
                item.OrderNum = orderNum;
                #endregion



                #region 部门
                if (dt.Rows[i][1].ToString() != "")
                {
                    string deptlist = dt.Rows[i][1].ToString().Trim();
                    var p1 = string.Empty; var p2 = string.Empty;
                    var array = deptlist.Split('/');
                    for (int j = 0; j < array.Length; j++)
                    {
                        if (j == 0)
                        {
                            if (currUser.RoleName.Contains("省级") || currUser.RoleName.Contains("集团"))
                            {
                                var deptEntity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == array[j].ToString()).FirstOrDefault();
                                if (deptEntity1 != null)
                                {
                                    Dept = deptEntity1.FullName;
                                    DeptCode = deptEntity1.EnCode;
                                    p1 = deptEntity1.DepartmentId;
                                }
                                else
                                {
                                    falseMessage += string.Format(@"【{1}】第{0}行,部门不存在！</br>", order, staffTypeName);
                                    error++;
                                    continue; 
                                }
                            }
                            else
                            {
                                var deptEntity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "厂级" && x.FullName == array[j].ToString()).FirstOrDefault();
                                if (deptEntity == null)
                                {
                                    deptEntity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "部门" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString()).FirstOrDefault();
                                    if (deptEntity == null)
                                    {
                                        deptEntity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "承包商" && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (deptEntity != null)
                                        {
                                            Dept = deptEntity.FullName;
                                            DeptCode = deptEntity.EnCode;
                                            p1 = deptEntity.DepartmentId;
                                        }
                                        else
                                        {
                                            falseMessage += string.Format(@"【{1}】第{0}行,部门不存在！</br>", order, staffTypeName);
                                            error++;
                                            continue; 
                                        }
                                    }
                                    else
                                    {
                                        Dept = deptEntity.FullName;
                                        DeptCode = deptEntity.EnCode;
                                        p1 = deptEntity.DepartmentId;
                                    }
                                }
                                else
                                {
                                    Dept = deptEntity.FullName;
                                    DeptCode = deptEntity.EnCode;
                                    p1 = deptEntity.DepartmentId;
                                }
                            }
                        }
                        else if (j == 1)
                        {
                            var deptEntity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "专业" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                            if (deptEntity1 == null)
                            {
                                deptEntity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "班组" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                if (deptEntity1 != null)
                                {
                                    Dept = deptEntity1.FullName;
                                    DeptCode = deptEntity1.EnCode;
                                    p2 = deptEntity1.DepartmentId;
                                }
                                else
                                {
                                    falseMessage += string.Format(@"【{1}】第{0}行,班组不存在！</br>", order, staffTypeName);
                                    error++;
                                    continue; 
                                }
                            }
                            else
                            {
                                Dept = deptEntity1.FullName;
                                DeptCode = deptEntity1.EnCode;
                                p2 = deptEntity1.DepartmentId;
                            }

                        }
                        else
                        {
                            var deptEntity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                            if (deptEntity1 != null)
                            {
                                Dept = deptEntity1.FullName;
                                DeptCode = deptEntity1.EnCode;
                            }
                            else
                            {
                                falseMessage += string.Format(@"【{1}】第{0}行,班组不存在！</br>", order, staffTypeName);
                                error++;
                                continue;
                            }
                        }
                    }
                }
                item.Dept = Dept;
                item.DeptCode = DeptCode;
                #endregion

                #region 名称
                //if (dt.Rows[i][1].ToString() != "")
                //{
                //    var users = new UserBLL().GetUserInfoByDeptCode(currUser.OrganizeCode);
                //    var user = users.Where(t => t.RealName == dt.Rows[i][1].ToString().Trim() && t.DepartmentCode == item.DeptCode).FirstOrDefault();
                //    if (user == null)
                //    {
                //        falseMessage += string.Format(@"【{1}】第{0}行,姓名在【{2}】不存在！</br>", order, staffTypeName, item.Dept);
                //        error++;
                //        continue;
                //    }
                //    else
                //    {
                //        Name = dt.Rows[i][1].ToString();
                //        NameId = user.UserId;
                //        post = new PostBLL().GetEntity(user.PostId).FullName;
                //    }
                //}
                item.Name = Name;
                item.NameId = NameId;
                item.Post = post;
                #endregion

                #region 隐患总数
                if (dt.Rows[i][2].ToString() != "")
                {
                    string hiddenNum = dt.Rows[i][2].ToString();
                    int tempHiddenNum;
                    if (!string.IsNullOrEmpty(hiddenNum))
                        if (int.TryParse(hiddenNum, out tempHiddenNum))
                            HiddenNum = tempHiddenNum;
                        else
                        {
                            falseMessage += string.Format(@"【{1}】第{0}行,隐患总数必须为数字！</br>", order, staffTypeName);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,隐患总数不能为空！</br>", order, staffTypeName);
                        error++;
                        continue;
                    }
                }
                item.HiddenNum = HiddenNum;
                #endregion

                #region 管理隐患
                if (dt.Rows[i][3].ToString() != "")
                {
                    string managerHidden = dt.Rows[i][3].ToString();
                    int tempManagerHidden;
                    if (!string.IsNullOrEmpty(managerHidden))
                        if (int.TryParse(managerHidden, out tempManagerHidden))
                            ManagerHidden = tempManagerHidden;
                        else
                        {
                            falseMessage += string.Format(@"【{1}】第{0}行,管理隐患必须为数字！</br>", order, staffTypeName);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,管理隐患不能为空！</br>", order, staffTypeName);
                        error++;
                        continue;
                    }
                }
                item.ManagerHidden = ManagerHidden;
                #endregion

                #region 实体隐患
                if (dt.Rows[i][4].ToString() != "")
                {
                    string entityHidden = dt.Rows[i][4].ToString();
                    int tempEntityHidden;
                    if (!string.IsNullOrEmpty(entityHidden))
                        if (int.TryParse(entityHidden, out tempEntityHidden))
                            EntityHidden = tempEntityHidden;
                        else
                        {
                            falseMessage += string.Format(@"【{1}】第{0}行,实体隐患必须为数字！</br>", order, staffTypeName);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,实体隐患不能为空！</br>", order, staffTypeName);
                        error++;
                        continue;
                    }
                }
                item.EntityHidden = EntityHidden;
                #endregion

                #region 详情序号
                if (dt.Rows[i][5].ToString() != "")
                {
                    string detailsOrderNum1 = dt.Rows[i][5].ToString();
                    int tempDetailsOrderNum;
                    if (!string.IsNullOrEmpty(detailsOrderNum1))
                        if (int.TryParse(detailsOrderNum1, out tempDetailsOrderNum))
                            item.DetailsOrderNum = tempDetailsOrderNum;
                        else
                        {
                            falseMessage += string.Format(@"【{1}】第{0}行,序号必须为数字！</br>", order, staffTypeName);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"【{1}】第{0}行,序号不能为空！</br>", order, staffTypeName);
                        error++;
                        continue;
                    }
                }
                #endregion


                #region 一
                item.One = dt.Rows[i][6].ToString();
                #endregion

                #region 二
                item.Two = dt.Rows[i][7].ToString();
                #endregion

                #region 三
                item.Three = dt.Rows[i][8].ToString();
                #endregion

                #region 四
                item.Four = dt.Rows[i][9].ToString();
                #endregion

                #region 五
                item.Five = dt.Rows[i][10].ToString();
                #endregion

                #region 六
                item.Six = dt.Rows[i][11].ToString();
                #endregion

                if (showType == "2")
                {
                    #region 七
                    item.Seven = dt.Rows[i][12].ToString();
                    #endregion

                    #region 八
                    item.Eight = dt.Rows[i][13].ToString();
                    #endregion

                    #region 九
                    item.Nine = dt.Rows[i][14].ToString();
                    #endregion

                    #region 十
                    item.Ten = dt.Rows[i][15].ToString();
                    #endregion
                }

                //try
                //{
                //    PublicityDetailsbll.SaveForm("", item);
                //}
                //catch
                //{
                //    error++;
                //}
                projects.Add(item);

            }
            if (projects.Count > 0)
            {
                if (projects.Count < dt.Rows.Count - 2)
                {
                    falseMessage += string.Format(@"【{1}】导入失败！</br>", order, staffTypeName);
                }
                else
                {
                    if (error == 0)
                    {
                        foreach (var item in projects)
                        {
                            PublicityDetailsbll.SaveForm("", item);
                        }
                        PublicityEntity entity = new PublicityEntity();
                        entity.ExamineType = examineType;
                        entity.Year = Convert.ToInt32(year);
                        entity.ShowType = Convert.ToInt32(showType);
                        entity.StaffType = staffType;
                        try
                        {
                            string sqldel = string.Format(@"delete from HRS_PUBLICITYDETAILS t where 
mainid=(select id from HRS_PUBLICITY p where p.CreateUserOrgCode='{0}' and p.ExamineType='{1}' and p.Year='{2}' and p.ShowType='{3}' and p.StaffType='{4}')", currUser.OrganizeCode, examineType, year, showType, staffType);
                            Publicitybll.ExecuteBySql(sqldel);
                            string sqldel1 = string.Format(@"delete from HRS_PUBLICITY p where p.CreateUserOrgCode='{0}' and p.ExamineType='{1}' and p.Year='{2}' and p.ShowType='{3}' and p.StaffType='{4}'", currUser.OrganizeCode, examineType, year, showType, staffType);
                            Publicitybll.ExecuteBySql(sqldel1);
                            Publicitybll.SaveForm(mainid, entity);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            sort = sortNum;
            return falseMessage;
        }
        [HttpGet]
        public ActionResult ExportData(string examineType, string year, string showType, string checkType)
        {
            try
            {
                string[] arr = {"1","2","3","4" };
                string excelName = "安全检查公示表(按月)导出模板.xlsx";
                if (showType=="2")
                {
                    excelName = "安全检查公示表(按周)导出模板.xlsx";
                }
                string title = string.Format("{0}年{1}公示表({2})", year, checkType, showType=="1"?"按月":"按周");
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/safetycheck/" + excelName));
                string queryJson = "";
                int idx = 0;
                Pagination pagination = new Pagination
                {
                    page = 1,
                    rows = 50000,
                    sord = "asc",
                    sidx = "sortnum"
                };
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                queryJson = queryJson ?? "";
                pagination.p_kid = "t.Id";
                pagination.p_fields = "t.ordernum,t.Name,t.NameId,t.Dept,t.DeptCode,t.Post,t.HiddenNum,t.ManagerHidden,t.EntityHidden,t.DetailsOrderNum,t.One,t.Two,t.Three,t.Four,t.Five,t.Six,t.Seven,t.Eight,t.Nine,t.Ten,d.nature,d.organizeid";
                pagination.p_tablename = " HRS_PUBLICITYDETAILS t left join HRS_PUBLICITY p on t.mainid=p.id left join BASE_DEPARTMENT d on t.DeptCode=d.encode ";
                pagination.conditionJson = string.Format(" p.CreateUserOrgCode='{0}'", user.OrganizeCode);
                foreach(string str in arr)
                {
                    queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new {
                        ExamineType = examineType,
                        Year = year,
                        ShowType = showType,
                        StaffType = str
                    });
                    var data = Publicitybll.GetPageList(pagination, queryJson);
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
                    DataTable dataSource = GetDataSource(data, queryJson);

                    dataSource.Columns.Remove("id"); dataSource.Columns.Remove("nameid"); dataSource.Columns.Remove("deptcode"); dataSource.Columns.Remove("nature"); dataSource.Columns.Remove("organizeid"); dataSource.Columns.Remove("r");
                    Cells cells = wb.Worksheets[idx].Cells;
                    cells.ImportDataTable(dataSource, false, "A4");
                    cells["A1"].PutValue(title);
                   // cells = wb.Worksheets[idx].Cells;
                    //wb.Worksheets[0].AutoFitColumns();
                    //StyleFlag sf = new StyleFlag();
                    //sf.HorizontalAlignment = true;
                    //sf.VerticalAlignment = true;
                    ////sf.WrapText = true;
                    //Style style = wb.Styles[wb.Styles.Add()];
                    //style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    //cells.ApplyStyle(style, sf);
                    int r = cells.MaxRow;
                    int rowSpan = 1;
                    for (int i = 1; i < r; i += rowSpan)
                    {
                        rowSpan = 1;
                        for (int j = i + 1; j <= r; j++)
                        {
                            if (cells[i, 0].StringValue == cells[j, 0].StringValue && cells[i, 1].StringValue == cells[j, 1].StringValue && cells[i, 2].StringValue == cells[j, 2].StringValue && cells[i, 3].StringValue == cells[j, 3].StringValue && cells[i, 4].StringValue == cells[j, 4].StringValue && cells[i, 5].StringValue == cells[j, 5].StringValue && cells[i, 6].StringValue == cells[j, 6].StringValue)
                            {
                                rowSpan++;

                            }
                            else
                            {
                                break;
                            }
                        }
                        if (rowSpan > 1)
                        {
                            cells.Merge(i, 0, rowSpan, 1); cells.Merge(i, 1, rowSpan, 1); cells.Merge(i, 2, rowSpan, 1); cells.Merge(i, 3, rowSpan, 1);
                            cells.Merge(i, 4, rowSpan, 1); cells.Merge(i, 5, rowSpan, 1); cells.Merge(i, 6, rowSpan, 1);

                        }
                    }
                    idx++;
                }
                if(wb.Worksheets.Count>4)
                {
                    wb.Worksheets[4].IsVisible = false;
                }
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                wb.Save("安全检查公示表.xls", Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInExcel, resp);
               
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
    }
}
