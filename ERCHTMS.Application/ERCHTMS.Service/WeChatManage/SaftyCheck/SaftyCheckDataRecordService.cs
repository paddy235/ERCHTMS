using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Code;
using System.Text;

namespace ERCHTMS.Service.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查记录
    /// </summary>
    public class SaftyCheckDataRecordService : RepositoryFactory<SaftyCheckDataRecordEntity>, SaftyCheckDataRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //时间选择
            if (!queryParam["st"].IsEmpty())
            {
                string st = queryParam["st"].ToString();
                string et = queryParam["et"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')", st, et);
            }
            if (!queryParam["stm"].IsEmpty())
            {
                string st = queryParam["stm"].ToString();
                string et = queryParam["etm"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME>=to_date('{0}','yyyy-mm-dd') and CHECKENDTIME<=to_date('{1}','yyyy-mm-dd')", st, et);
            }
            //检查名称
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckDataRecordName like '%{0}%'", queryParam["keyword"].ToString());
            }
            //安全检查类型
            if (!queryParam["ctype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CHECKDATATYPE='{0}'", queryParam["ctype"].ToString());
            }
            //所属部门
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", queryParam["code"].ToString());
            }

            IEnumerable<SaftyCheckDataRecordEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType).Select(r =>
            {
                DataTable dtc = this.BaseRepository().FindTable("select id from bis_htbaseinfo o where o.safetycheckobjectid='" + r.ID + "'");
                r.Count = dtc.Rows.Count;
                return r;
            });

            return list;
        }

        /// <summary>
        ///获取统计表格数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string GetSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            string whereSQL = " 1=1";
            string roleName = OperatorProvider.Provider.Current().RoleName;
            string owndeptcode = OperatorProvider.Provider.Current().DeptCode;
            //单位
            if (!String.IsNullOrEmpty(deptCode))
            {
                whereSQL += " and (CheckDeptCode like '" + deptCode + "%' or CHECKDEPTID like '" + deptCode + "%')";
            }
            else
            {
                if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    whereSQL += string.Format(" and (CheckDeptCode like '{0}%' or CHECKDEPTID like '{0}%')", owndeptcode.Substring(0, 3));
                }
                else
                {
                    whereSQL += string.Format(" and (CheckDeptCode like '{0}%' or CHECKDEPTID like '{0}%')", owndeptcode);
                }
            }
            //年限
            whereSQL += " and to_char(t.checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  t.belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }
            List<SaftyCheckCountEntity> list = new List<SaftyCheckCountEntity>();
            for (int i = 1; i <= 12; i++)
            {
                string whereSQL2 = " and to_char(t.checkbegintime,'mm')=" + i.ToString();
                string forsql = @"select nvl(b.num,0) num,a.itemvalue from (
select t.itemvalue,count(1) as num from BASE_DATAITEMDETAIL t where t.itemid =(select itemid from base_dataitem where itemcode='SaftyCheckType') group by itemvalue
) a
left join(
select to_char(CHECKDATATYPE) as itemvalue, count(1) as num from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + whereSQL2 + " group by CHECKDATATYPE)  b on a.itemvalue = b.itemvalue order by  a.itemvalue";
                DataTable cou = this.BaseRepository().FindTable(forsql);
                int rc = 0, zx = 0, jj = 0, jjr = 0, zh = 0;
                for (int j = 0; j < cou.Rows.Count; j++)
                {
                    var checktype = cou.Rows[j][1].ToString();
                    var checknum = Convert.ToInt32(cou.Rows[j][0].ToString());
                    switch (checktype)
                    {
                        case "1":
                            rc = checknum;
                            break;
                        case "2":
                            zx = checknum;
                            break;
                        case "3":
                            jjr = checknum;
                            break;
                        case "4":
                            jj = checknum;
                            break;
                        case "5":
                            zh = checknum;
                            break;
                        default:
                            break;
                    }
                }
                list.Add(new SaftyCheckCountEntity
                {
                    month = i.ToString() + "月",
                    rc = rc,
                    zx = zx,
                    jj = jj,
                    jjr = jjr,
                    zh = zh,
                    sum = Convert.ToInt32(cou.Rows[0][0].ToString()) + Convert.ToInt32(cou.Rows[1][0].ToString()) + Convert.ToInt32(cou.Rows[2][0].ToString()) + Convert.ToInt32(cou.Rows[3][0].ToString()) + Convert.ToInt32(cou.Rows[4][0].ToString())
                });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = list.Count,
                rows = list
            });
        }


        /// <summary>
        ///获取统计表格数据(对比)
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string GetSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            string sql = "";
            //判断用户觉得是不是厂级或者公司级用户,查看所有数据
            string roleName = OperatorProvider.Provider.Current().RoleName;
            string owndeptcode = OperatorProvider.Provider.Current().DeptCode;
            if (String.IsNullOrEmpty(deptCode))
            {
                if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where encode like '{0}___' ", owndeptcode.Substring(0, 3));
                }
                else
                {
                    sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where (encode='{0}' or  encode like '{0}___' )", owndeptcode);
                }
            }
            else
            {
                sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where (encode='{0}' or  encode like '{0}___' )", deptCode);
            }
            //检查表类型
            ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日前后安全检查,综合安全检查";

            sql += " order by SortCode asc";
            DataTable dtDepts = this.BaseRepository().FindTable(sql);
            string whereSQL = " 1=1";
            //单位

            //年限
            whereSQL += " and to_char(t.checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  t.belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }
            List<SaftyCheckCountEntity> list = new List<SaftyCheckCountEntity>();
            foreach (DataRow item in dtDepts.Rows)
            {
                var where = "";
                if ((deptCode == item[0].ToString() && deptCode.Length == 6 && deptCode == owndeptcode) || (owndeptcode == item[0].ToString() && owndeptcode.Length == 6 && deptCode == ""))
                {
                    where = string.Format(" and ((checkdeptcode = '{0}' or checkdeptcode like '{0},%' or checkdeptcode like '%,{0},%' or checkdeptcode like '%,{0}') or CHECKDEPTID='{0}')", item[0].ToString());
                }
                else
                {
                    where = "  and (CheckDeptCode like '" + item[0].ToString() + "%' or CHECKDEPTID like '" + item[0].ToString() + "%')";
                }
                string forsql = @"select nvl(b.num,0) num,a.itemvalue from (
select t.itemvalue,count(1) as num from BASE_DATAITEMDETAIL t where t.itemid =(select itemid from base_dataitem where itemcode='SaftyCheckType')  group by itemvalue
) a
left join(
select to_char(CHECKDATATYPE) as itemvalue, count(1) as num from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + where + " group by CHECKDATATYPE)  b on a.itemvalue = b.itemvalue order by  a.itemvalue";
                DataTable cou = this.BaseRepository().FindTable(forsql);
                int rc = 0, zx = 0, jj = 0, jjr = 0, zh = 0;
                for (int i = 0; i < cou.Rows.Count; i++)
                {
                    var checktype = cou.Rows[i][1].ToString();
                    var checknum = Convert.ToInt32(cou.Rows[i][0].ToString());
                    switch (checktype)
                    {
                        case "1":
                            rc = checknum;
                            break;
                        case "2":
                            zx = checknum;
                            break;
                        case "3":
                            jjr = checknum;
                            break;
                        case "4":
                            jj = checknum;
                            break;
                        case "5":
                            zh = checknum;
                            break;
                        default:
                            break;
                    }
                }
                list.Add(new SaftyCheckCountEntity
                {
                    month = item[1].ToString(),
                    rc = rc,
                    zx = zx,
                    jjr = jjr,
                    jj = jj,
                    zh = zh,
                    sum = Convert.ToInt32(cou.Rows[0][0].ToString()) + Convert.ToInt32(cou.Rows[1][0].ToString()) + Convert.ToInt32(cou.Rows[2][0].ToString()) + Convert.ToInt32(cou.Rows[3][0].ToString()) + Convert.ToInt32(cou.Rows[4][0].ToString())
                });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = list.Count,
                rows = list
            });
        }
        /// <summary>
        /// 条形图统计数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域code</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string getRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日安全检查,综合性安全检查")
        {
            if (string.IsNullOrEmpty(ctype) || ctype == "请选择")
            {
                ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日前后安全检查,综合安全检查";
            }
            List<string> yValues = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                yValues.Add(i.ToString() + "月");
            }
            List<object> dic = new List<object>();
            string[] type = ctype.Split(',');
            string whereSQL = " 1=1";
            string roleName = OperatorProvider.Provider.Current().RoleName;
            string owndeptcode = OperatorProvider.Provider.Current().DeptCode;
            //单位
            if (!String.IsNullOrEmpty(deptCode))
            {
                whereSQL += " and (CheckDeptCode like '" + deptCode + "%' or CHECKDEPTID like '" + deptCode + "%')";
            }
            else
            {
                if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    whereSQL += string.Format(" and (CheckDeptCode like '{0}%' or CHECKDEPTID like '{0}%')", owndeptcode.Substring(0, 3));
                }
                else
                {
                    whereSQL += string.Format(" and (CheckDeptCode like '{0}%' or CHECKDEPTID like '{0}%')", owndeptcode);
                }
            }
            //年限
            whereSQL += " and to_char(checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }


            foreach (string itemType in type)
            {
                var item = itemType.TrimStart();//传过来的字符中有空格
                string ct = "1";
                if (item == "专项安全检查") ct = "2";
                if (item == "节假日前后安全检查") ct = "3";
                if (item == "季节性安全检查") ct = "4";
                if (item == "综合安全检查") ct = "5";
                List<int> list = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    string whereSQL2 = " and to_char(t.checkbegintime,'mm')=" + i.ToString() + " and CHECKDATATYPE='" + ct + "'";
                    int cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + whereSQL2).ToInt();
                    list.Add(cou);
                }
                dic.Add(new { name = item, data = list });
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }
        /// <summary>
        /// 获取对比数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域</param>
        /// <param name="ctype">检查类型</param>

        /// <returns></returns>
        public string GetAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {

            string sql = "";
            //判断用户觉得是不是厂级或者公司级用户,查看所有数据
            string roleName = OperatorProvider.Provider.Current().RoleName;
            string owndeptcode = OperatorProvider.Provider.Current().DeptCode;
            if (String.IsNullOrEmpty(deptCode))
            {
                if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where encode like '{0}___' ", owndeptcode.Substring(0, 3));
                }
                else
                {
                    sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where (encode='{0}' or  encode like '{0}___' )", owndeptcode);
                }
            }
            else
            {
                sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where (encode='{0}' or  encode like '{0}___' )", deptCode);
            }
            if (string.IsNullOrEmpty(ctype) || ctype == "请选择")
            {
                ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日前后安全检查,综合安全检查";
            }
            sql += " order by SortCode asc";
            DataTable dtDepts = this.BaseRepository().FindTable(sql);

            List<string> yValues = new List<string>();

            List<object> dic = new List<object>();
            string[] type = ctype.Split(',');
            string whereSQL = " 1=1";

            //单位

            //年限
            whereSQL += " and to_char(checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }

            bool isRead = false;
            foreach (string itemType in type)
            {
                var item = itemType.TrimStart();//传值出现空格
                string ct = "1";
                if (item == "专项安全检查") ct = "2";
                if (item == "节假日前后安全检查") ct = "3";
                if (item == "季节性安全检查") ct = "4";
                if (item == "综合安全检查") ct = "5";
                List<int> list = new List<int>();
                foreach (DataRow dept in dtDepts.Rows)
                {
                    if (!isRead)
                    {
                        yValues.Add(dept[1].ToString());
                    }
                    var where = "";
                    if ((deptCode == dept[0].ToString() && deptCode.Length == 6 && deptCode == owndeptcode) || (owndeptcode == dept[0].ToString() && owndeptcode.Length == 6 && deptCode == ""))
                    {
                        where = string.Format("  and CHECKDATATYPE='" + ct + "'  and ((checkdeptcode = '{0}' or checkdeptcode like '{0},%' or checkdeptcode like '%,{0},%' or checkdeptcode like '%,{0}') or CHECKDEPTID='{0}')", dept[0].ToString());
                    }
                    else
                    {
                        where = "  and CHECKDATATYPE='" + ct + "'  and (CheckDeptCode like '" + dept[0].ToString() + "%' or CHECKDEPTID like '" + dept[0].ToString() + "%')";
                    }
                    //string whereSQL2 = " and CHECKDATATYPE='" + ct + "' and (CheckDeptCode like '" + dept[0].ToString() + "%' or CHECKDEPTID like '" + dept[0].ToString() + "%')";

                    int cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + where).ToInt();
                    list.Add(cou);
                }
                isRead = true;
                dic.Add(new { name = item, data = list });
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }
        /// <summary>
        /// 曲线图统计数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域code</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string getMonthCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日安全检查,综合性安全检查")
        {
            if (string.IsNullOrEmpty(ctype) || ctype == "请选择")
            {
                ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日前后安全检查,综合安全检查";
            }
            List<object> dic = new List<object>();
            List<int> list = new List<int>();
            List<string> yValues = new List<string>();
            string[] type = ctype.Split(',');
            string whereSQL = " 1=1";
            string roleName = OperatorProvider.Provider.Current().RoleName;
            string owndeptcode = OperatorProvider.Provider.Current().DeptCode;
            //单位
            if (!String.IsNullOrEmpty(deptCode))
            {
                whereSQL += " and (CheckDeptCode like '" + deptCode + "%' or CHECKDEPTID like '" + deptCode + "%')";
            }
            else
            {
                if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    whereSQL += string.Format(" and (CheckDeptCode like '{0}%' or CHECKDEPTID like '{0}%')", owndeptcode.Substring(0, 3));
                }
                else
                {
                    whereSQL += string.Format(" and (CheckDeptCode like '{0}%' or CHECKDEPTID like '{0}%')", owndeptcode);
                }
            }
            //年限
            whereSQL += " and to_char(checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }
            string cr = " and CHECKDATATYPE in ('";
            if (ctype.Contains("日常安全检查")) cr += "1','";
            if (ctype.Contains("专项安全检查")) cr += "2','";
            if (ctype.Contains("季节性安全检查")) cr += "3','";
            if (ctype.Contains("节假日前后安全检查")) cr += "4','";
            if (ctype.Contains("综合安全检查")) cr += "5','";
            cr += "')";
            whereSQL += cr;
            for (int i = 1; i <= 12; i++)
            {

                string whereSQL2 = " and to_char(t.checkbegintime,'mm')=" + i.ToString();
                int cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + whereSQL2).ToInt();
                list.Add(cou);

                yValues.Add(i.ToString() + "月");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = list, y = yValues });
        }
        /// <summary>
        /// 专项检查和其他导出
        /// </summary>
        public DataTable ExportData(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //时间选择
            if (!queryParam["st"].IsEmpty())
            {
                string st = queryParam["st"].ToString();
                string et = queryParam["et"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')", st, et);
            }
            if (!queryParam["stm"].IsEmpty())
            {
                string st = queryParam["stm"].ToString();
                string et = queryParam["etm"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME>=to_date('{0}','yyyy-mm-dd') and CHECKENDTIME<=to_date('{1}','yyyy-mm-dd')", st, et);
            }
            //检查名称
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckDataRecordName like '%{0}%'", queryParam["keyword"].ToString());
            }
            //安全检查类型
            if (!queryParam["ctype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CHECKDATATYPE='{0}'", queryParam["ctype"].ToString());
            }
            //所属部门
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", queryParam["code"].ToString());
            }
            DataTable dtAll = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            foreach (DataRow item in dtAll.Rows)
            {
                DataTable dtName = this.BaseRepository().FindTable("select itemname from base_dataitemdetail where itemid=(select itemid from base_dataitem where itemcode='SaftyCheckLevel') and itemvalue='" + item["CheckLevel"] + "'");
                item["checklevel"] = dtName.Rows[0]["itemname"];

                DataTable dt = this.BaseRepository().FindTable("select distinct(BELONGDISTRICTID) from bis_saftycheckdatadetailed o where recid='" + item["id"] + "'");
                double total = dt.Rows.Count;
                DataTable dt2 = this.BaseRepository().FindTable("select distinct(BELONGDISTRICTID) from bis_saftycheckdatadetailed o where o.recid='" + item["id"] + "' and  o.CHECKSTATE=2");
                double havdel = dt2.Rows.Count;
                if (total != 0)
                {
                    item["solvecount"] = Math.Round((havdel / total), 2) * 100;
                }
                DataTable dtc = this.BaseRepository().FindTable("select id from bis_htbaseinfo o where o.safetycheckobjectid='" + item["id"] + "'");
                item["count"] = dtc.Rows.Count;
            }
            return dtAll;
        }

        /// <summary>
        /// 专项和其他列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetPageListForType(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //时间选择
            if (!queryParam["st"].IsEmpty())
            {
                string st = queryParam["st"].ToString();
                string et = queryParam["et"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')", st, et);
            }
            if (!queryParam["stm"].IsEmpty())
            {
                string st = queryParam["stm"].ToString();
                string et = queryParam["etm"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME>=to_date('{0}','yyyy-mm-dd') and CHECKENDTIME<=to_date('{1}','yyyy-mm-dd')", st, et);
            }
            //检查名称
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckDataRecordName like '%{0}%'", queryParam["keyword"].ToString());
            }
            //所属部门
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdeptid  in ( (select departmentid from base_department where encode like '{0}%' union select organizeid from base_organize where encode like '{0}%')) ", queryParam["code"].ToString());
            }

            //安全检查类型
            if (!queryParam["ctype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CHECKDATATYPE='{0}'", queryParam["ctype"].ToString());
            }
            IEnumerable<SaftyCheckDataRecordEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType).Select(r =>
            {
                //计算该记录所含区域
                DataTable dtdis = this.BaseRepository().FindTable("select distinct(BELONGDISTRICTID) from bis_saftycheckdatadetailed o where recid='" + r.ID + "'");
                double total = dtdis.Rows.Count;
                //计算已经被登记过隐患的区域
                DataTable dt2 = this.BaseRepository().FindTable("select distinct(BELONGDISTRICTID) from bis_saftycheckdatadetailed o where o.recid='" + r.ID + "' and  o.CHECKSTATE=2");
                double havdel = dt2.Rows.Count;

                if (total != 0)
                {
                    r.SolveCount = Math.Round((havdel / total), 2) * 100;
                }
                //计算所含隐患个数
                DataTable dtc = this.BaseRepository().FindTable("select id from bis_htbaseinfo o where o.safetycheckobjectid='" + r.ID + "'");
                r.Count = dtc.Rows.Count;
                return r;
            });

            return list;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SaftyCheckDataRecordEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 更改登记人
        /// </summary>
        public void RegisterPer(string userAccount, string id)
        {
            this.BaseRepository().ExecuteBySql("update bis_saftycheckdatarecord set SOLVEPERSON=SOLVEPERSON||'" + userAccount + "|',SolvePersonName=SolvePersonName||'" + OperatorProvider.Provider.Current().UserName + ",' where id='" + id + "'");

        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
            this.BaseRepository().ExecuteBySql("delete from bis_saftycheckdatadetailed where recid='" + keyValue + "'");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, SaftyCheckDataRecordEntity entity, ref string recid)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                return this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                recid = entity.ID;
                return this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// 修改已检察人员
        /// </summary>
        public void UpdateCheckMan(string userAccount)
        {
            this.BaseRepository().ExecuteBySql("update BIS_SAFTYCHECKDATARECORD set SOLVEPERSON=SOLVEPERSON+'" + userAccount + "|',SolvePersonName=SolvePersonName||'" + OperatorProvider.Provider.Current().UserName + ",'");
        }
        #endregion

        #region 获取数据手机端
        public IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user)
        {
            //string sqlWhere = "select *from bis_saftycheckdatarecord where 1=1";
            //if (!string.IsNullOrEmpty(safeCheckTypeId))
            //{
            //    sqlWhere += " and CHECKDATATYPE='" + safeCheckTypeId + "'";
            //}
            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    sqlWhere += " and CHECKDATARECORDNAME like '%" + searchString + "%'";
            //}
            ////数据权限判断

            //IEnumerable<SaftyCheckDataRecordEntity> list = this.BaseRepository().FindList(sqlWhere); ;
            //return list;
            List<SaftyCheckDataRecordEntity> listAll = new List<SaftyCheckDataRecordEntity>();
            var arg = "";
            var where1 = "";
            var where2 = "";
            if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司"))
            {
                arg = user.DeptCode.Substring(0, 3);
                where2 = string.Format(" and (recid is not null or createuserdeptcode like '{0}%')", arg);

            }
            else
            {
                arg = user.DeptCode;
                where1 = string.Format(" or senddeptid='{0}'", user.DeptId);
                where2 = string.Format("  and (recid is not null or createuserdeptcode='{0}')", arg);
            }
            string sqlrc = string.Format(@"select * from bis_saftycheckdatarecord t left join(select id as recid from (select id,(','||checkmanid||',') as checkmanid  from  bis_saftycheckdatarecord) a 
left join (
select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode like '{0}%' {1} )
) b on a.checkmanid  like '%'||b.account||'%' where account is not null group by id)t1
on t.ID=t1.recid where  checkdatatype='1'  {2}", arg, where1, where2);
            var list = this.BaseRepository().FindList(sqlrc);//日常检查
            listAll.AddRange(list);

            string sqlOther = string.Format(@"select * from  bis_saftycheckdatarecord t left join  (select recid from (select id,(','||CheckManAccount||',') as CheckManAccount,recid  from  BIS_SAFTYCONTENT) a 
left join (select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode like '{0}%' {1} )
) b on a.CheckManAccount  like '%'||b.account||'%' where account is not null  group by recid)t1
on t.id=t1.recid where  checkdatatype!='1' {2}", arg, where1, where2);
            var listOther = this.BaseRepository().FindList(sqlOther);//其他安全检查
            listAll.AddRange(listOther);
            if (!string.IsNullOrEmpty(safeCheckTypeId))
            {
                listAll = listAll.FindAll(x => x.CheckDataType == int.Parse(safeCheckTypeId));
            }
            return listAll.OrderByDescending(x => x.CreateDate);

        }

        public SaftyCheckDataRecordEntity getSaftyCheckDataRecordEntity(string safeCheckIdItem)
        {
            SaftyCheckDataRecordEntity entity = this.BaseRepository().FindEntity(safeCheckIdItem);
            return entity;
        }

        public DataTable getCheckRecordDetail(string safeCheckIdItem, string riskPointId)
        {
            string sql = string.Format(@"select a.account,a.modifydate,a.createuserdeptcode,a.createuserorgcode,a.createuserid,  a.checktypename,a.hidtypename,a.hidrankname,a.checkdepartname,a.isgetafter,
                                        a.hiddendepartname,a.id as hiddenid ,a.createdate,a.hidcode as problemid ,a.hiddangername,a.hidproject,a.checkdate,a.checkdepart,a.checktype,a.checknumber,
                                        a.isbreakrule,a.breakruleuserids,a.breakruleusernames,a.traintemplateid ,
                                        a.traintemplatename,a.hidtype,a.hidrank,a.hidplace,a.hidpoint,a.workstream ,a.addtype ,b.participant,c.applicationstatus ,
                                        c.postponedept ,c.postponedeptname ,e.useraccount as principal,a.hiddescribe,c.changemeasure,
                                       (case when f.filepath is not null then ('{0}'||substr(f.filepath,2)) else '' end) as filepath   from v_htbaseinfo a
                                        left join v_workflow b on a.id = b.id 
                                        left join v_htchangeinfo c on a.hidcode = c.hidcode
                                        left join v_htacceptinfo d on a.hidcode = d.hidcode
                                        left join v_principal e on c.changedutydepartcode = e.departmentcode
                                        left join v_imageview f on a.hidphoto = f.folderid  
                                        where 1=1 ", Config.GetValue("imgUrl"));
            if (!string.IsNullOrEmpty(safeCheckIdItem))
            {
                sql += string.Format(" and a.safetycheckobjectid = '{0}'", safeCheckIdItem);
            }
            if (!string.IsNullOrEmpty(riskPointId))
            {
                sql += string.Format(" and a.hidpoint = '{0}'", riskPointId);
            }

            DataTable dt = this.BaseRepository().FindTable(sql);
            return dt;
        }

        public int addDailySafeCheck(SaftyCheckDataRecordEntity se, Operator user)
        {
            se.insertInto(se, user);
            se.BelongDept = user.DeptCode;
            se.BelongDeptID = user.DeptId;
            return this.BaseRepository().Insert(se);
        }

        public DataTable selectCheckPerson(Operator user)
        {
            //权限判断

            //
            //
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT u.userid as nameId,u.account as account,
                                    u.realname as name ,
                                    case when u.departmentid is null then o.organizeid else u.departmentid end  parentId,
                                    u.departmentid,
                                    d.fullname AS deptname ,
                                    d.ENCODE as deptcode
                            FROM    Base_User u
                                    LEFT JOIN Base_Organize o ON o.OrganizeId = u.OrganizeId
                                    LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                            WHERE   1=1");
            strSql.Append(" AND u.UserId <> 'System' AND u.EnabledMark = 1 AND u.DeleteMark=0 ");
            return this.BaseRepository().FindTable(strSql.ToString());
        }


        #region 首页待办列表
        public List<SaftyCheckDataRecordEntity> GetSaftDataIndexList(ERCHTMS.Code.Operator user)
        {
            List<SaftyCheckDataRecordEntity> listAll = new List<SaftyCheckDataRecordEntity>();
            string sql = string.Format(@"select * from   bis_saftycheckdatarecord t left join  (select recid from bis_saftycontent where CheckManAccount='{0}' group by  recid)t1
on t.id=t1.recid  where recid is not null and ((instr(solveperson,'{0}'))=0 or solveperson is null)  and belongdept like  '{1}%'", user.Account, user.OrganizeCode);
            var list = this.BaseRepository().FindList(sql).ToList();
            listAll.AddRange(list);
            string sqlRc = string.Format(@"select * from bis_saftycheckdatarecord where checkDataType='1' and  instr(checkmanid,'{0}')>0  and ((instr(solveperson,'{0}'))=0 or solveperson is null)  and belongdept like  '{1}%' ", user.Account, user.OrganizeCode);
            var list1 = this.BaseRepository().FindList(sqlRc).ToList();
            listAll.AddRange(list1);
            return listAll;
        }
        #endregion
        #endregion
    }
}
