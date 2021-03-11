using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using System.Data;
using System.Data.Common;
using BSFramework.Data;
using BSFramework.Util;
using System.Text;
using System;

namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：安全风险库
    /// </summary>
    public class RiskAssessService : RepositoryFactory<RiskAssessEntity>, RiskAssessIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="workId">作业步骤ID</param>
        /// <param name="areaCode">区域编码</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险等级</param>
        ///  <param name="accType">事故类型</param>
        ///  <param name="deptCode">部门编码</param>
        ///  <param name="keyWord">查询关键字</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RiskAssessEntity> GetList(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord)
        {
            var expression = LinqExtensions.True<RiskAssessEntity>();
            if (!string.IsNullOrEmpty(areaCode))
            {
                expression = expression.And(t => t.AreaCode == areaCode);
            }
            if (!string.IsNullOrEmpty(areaId))
            {
                expression = expression.And(t => t.AreaId == areaId);
            }
            if (!string.IsNullOrEmpty(grade))
            {
                expression = expression.And(t => t.Grade == grade);
            }
            if (!string.IsNullOrEmpty(accType))
            {
                expression = expression.And(t => accType.Contains(t.AccidentName));
            }
            if (!string.IsNullOrEmpty(deptCode))
            {
                expression = expression.And(t => t.DeptCode.Contains(deptCode));
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                expression = expression.And(t => t.Description.Contains(keyWord) || t.DangerSource.Contains(keyWord));
            }
            return this.BaseRepository().IQueryable().Where(expression);
        }
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //查询年份
            if (!queryParam["year"].IsEmpty())
            {
                string year = queryParam["year"].ToString();
                pagination.conditionJson += string.Format(" and to_char(createdate,'yyyy')='{0}'", year);
            }
            //风险级别
            if (!queryParam["level"].IsEmpty())
            {
                string level = queryParam["level"].ToString();
                pagination.conditionJson += string.Format(" and gradeval={0}", level);
            }
            //计划ID
            if (!queryParam["planId"].IsEmpty())
            {
                string planId = queryParam["planId"].ToString();
                pagination.conditionJson += string.Format(" and newplanId ='{0}'", planId);
            }
            //岗位
            if (!queryParam["postId"].IsEmpty())
            {
                string postId = queryParam["postId"].ToString();
                pagination.conditionJson += string.Format(" and postId ='{0}'", postId);
            }
            //状态
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                pagination.conditionJson += string.Format(" and status ={0}", status);
            }
            //区域Code
            string areaCode = "";
            if (!queryParam["areaCode"].IsEmpty())
            {
                areaCode = queryParam["areaCode"].ToString();
                pagination.conditionJson += string.Format(" and areaCode like '{0}%'", areaCode);
            }
            //区域ID
            if (!queryParam["areaId"].IsEmpty())
            {
                string areaId = queryParam["areaId"].ToString();
                pagination.conditionJson += string.Format(" and areaId = '{0}'", areaId);
            }
            //风险等级
            if (!queryParam["grade"].IsEmpty())
            {
                string grade = queryParam["grade"].ToString();
                pagination.conditionJson += string.Format(" and grade = '{0}'", grade);
            }
            //事故类型
            if (!queryParam["accType"].IsEmpty())
            {
                string accType = queryParam["accType"].ToString();
                pagination.conditionJson += string.Format(" and AccidentName like '%{0}%'", accType);
            }
            //部门Code
            if (!queryParam["deptCode"].IsEmpty())
            {
                string deptCode = queryParam["deptCode"].ToString();
                pagination.conditionJson += string.Format(" and deptCode like '{0}%'", deptCode);
            }
            if (!queryParam["deptCode1"].IsEmpty())
            {
                string deptCode = queryParam["deptCode1"].ToString();
                pagination.conditionJson += string.Format(" and deptCode='{0}'", deptCode);
            }
            //本人辨识的
            if (!queryParam["selfSpot"].IsEmpty())
            {
                string selfSpot = queryParam["selfSpot"].ToString();
                pagination.conditionJson += string.Format(" and planid in(select planid from BIS_RISKPPLANDATA where userid='{0}' and DATATYPE=0)", selfSpot);
            }
            //本人评估的
            if (!queryParam["selfAssess"].IsEmpty())
            {
                string selfAssess = queryParam["selfAssess"].ToString();
                pagination.conditionJson += string.Format(" and planid in(select planid from BIS_RISKPPLANDATA where userid='{0}' and DATATYPE=1)", selfAssess);
            }
            //查询关键字
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString();
                pagination.conditionJson += string.Format(" and (Description like '%{0}%' or DangerSource like '%{0}%' or result like '%{0}%') ", keyWord.Trim());
            }
            if (!queryParam["keyValue"].IsEmpty())
            {
                string keyValue = queryParam["keyValue"].ToString();
                pagination.conditionJson += string.Format(" and ( deptname like '%{0}%' or postname like '%{0}%') ", keyValue.Trim());
            }
            if (!queryParam["AreaIds"].IsEmpty())
            {
                string AreaIds = queryParam["AreaIds"].ToString();
                if (AreaIds.Length > 0)
                {
                    pagination.conditionJson += string.Format(" and districtid in('{0}')", AreaIds.Replace(",", "','"));
                }
            }
            if (!queryParam["initAreaId"].IsEmpty())
            {
                string initAreaId = GetAreaIdsByCode(areaCode);
                if (initAreaId.Length>0)
                {
                     pagination.conditionJson += string.Format(" or areaId in('{0}')", initAreaId.Replace(",", "','"));
                }
               
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 根据区域Code获取下属所有区域包含的内置区域ID
        /// </summary>
        /// <param name="areaCode">区域code</param>
        /// <returns></returns>
        public string GetAreaIdsByCode(string areaCode)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = this.BaseRepository().FindTable(string.Format("select linktocompanyid from BIS_DISTRICT  where linktocompanyid is not null and districtcode like '{0}%'", areaCode));
            foreach(DataRow dr in dt.Rows)
            {
                if (!sb.ToString().Contains(dr[0].ToString()))
                {
                    sb.Append(dr[0].ToString());
                }
             
            }
            dt.Dispose();
            return sb.ToString();
        }
        /// <summary>
        /// 根据部门编码和查询关键字获取岗位风险卡列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns></returns>
        public int GetPostCardCount(string queryCondition)
        {
            string sql = "select distinct deptcode,deptname,postname,postid from BIS_RISKASSESS where 1=1 ";
            if (!string.IsNullOrEmpty(queryCondition))
            {
                sql += " and " + queryCondition;
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            int count=dt.Rows.Count;
            dt.Dispose();
            return count;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskAssessEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 根据风险点ID获取部门Code（多个用英文逗号分割）
        /// </summary>
        /// <param name="dangerId">风险点ID</param>
        /// <returns></returns>
        public string GetDeptCode(string dangerId)
        {
            DbParameter[] param ={
                        DbParameters.CreateDbParameter("@dangerId",dangerId)
                                };
            DataTable dt = this.BaseRepository().FindTable("select deptcode from BIS_RISKDATABASE  where dangerid=@dangerId", param);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(dr[0].ToString() + ",");
            }
            return sb.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        ///  <param name="workId">作业步骤ID</param>
        /// <param name="dangerId">危险点ID</param>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        public DataTable GetEntity(string workId, string dangerId, string areaId)
        {
            List<DbParameter> param = new List<DbParameter>();
            param.Add(DbParameters.CreateDbParameter("@WorkId", workId));
            param.Add(DbParameters.CreateDbParameter("@DangerId", dangerId));
            param.Add(DbParameters.CreateDbParameter("@AreaId", areaId));
            return this.BaseRepository().FindTable("select deptname ,majorname,teamname,grade from BIS_RISKDATABASE where workid=@WorkId and dangerid=@DangerId and areaid=@AreaId order by gradeval asc", param.ToArray());
        }
        /// <summary>
        /// 根据部门编码获取风险数量统计图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "一级,二级,三级,四级")
        {
            List<object[]> list = new List<object[]>();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string sql = string.Format("select  grade,count(1) from BIS_RISKASSESS where status=1 and deptcode like '{0}%' and  deletemark=0 and enabledmark=0 and districtid is not null", deptCode);
            if(!string.IsNullOrEmpty(year))
            {
                string[] arrDate = year.Split('|');
                sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arrDate[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arrDate[1]).ToString("yyyy-MM-dd 23:59:59"));
            }
            sql += " group by grade";
            DataTable dt = this.BaseRepository().FindTable(sql);
          
            int count = dt.Select("grade='一级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='一级'")[0][1].ToString());
            object[] arr = { "一级风险", count}; list.Add(arr);
            count = dt.Select("grade='二级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='二级'")[0][1].ToString());
            arr = new object[] { "二级风险", count }; list.Add(arr);
            count = dt.Select("grade='三级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='三级'")[0][1].ToString());
            arr = new object[] { "三级风险", count }; list.Add(arr);
            count = dt.Select("grade='四级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='四级'")[0][1].ToString());
            arr = new object[] { "四级风险", count}; list.Add(arr);
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 根据部门编码获取风险数量统计图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetAreaRiskCountByDeptCode(string deptCode, string year = "")
        {
            List<object[]> list = new List<object[]>();
            string dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
            string sql = string.Format("select t.districtcode,t.districtname from BIS_DISTRICT t where length(districtcode)=6 and t.districtcode like '{0}%' order by sortcode ", dCode);
            DataTable dtAreas = this.BaseRepository().FindTable(sql);
            foreach (DataRow area in dtAreas.Rows)
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS where status=1 and deptcode like '{0}%' and  deletemark=0 and enabledmark=0  and areacode like '{1}%'", deptCode,area[0].ToString());
                if (!string.IsNullOrEmpty(year))
                {
                    string[] arr = year.Split('|');
                    sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arr[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arr[1]).ToString("yyyy-MM-dd 23:59:59"));
                }
                int count= this.BaseRepository().FindObject(sql).ToInt();
               // if (count>0)
                //{
                    object[] objs = { area[1].ToString(), count };
                    list.Add(objs);
               // }
                   
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 根据部门编码获取风险数量统计图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>  
        public string GetYearRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "一级,二级,三级,四级",string areaCode="")
        {
            List<int> xValues = new List<int>();
            List<string> yValues = new List<string>();
            string sql = string.Format("select count(1) from BIS_RISKASSESS where deletemark=0 and enabledmark=0 and status=1 and deptcode like '{0}%'", deptCode);
            if (!string.IsNullOrEmpty(riskGrade))
            {
                 sql += string.Format(" and grade in('{0}')",riskGrade.Replace(",","','"));          
            }
            if (!string.IsNullOrEmpty(areaCode))
            {
                sql += string.Format(" and areaCode like '{0}%'", areaCode);
            }
             int startYear=0;int endYear=0;
             if (!string.IsNullOrEmpty(year))
             {
                 string[] arr = year.Split('|');
                 startYear=int.Parse(arr[0]);
                 endYear=int.Parse(arr[1]);
                   
              }
              else
              {
                 startYear= DateTime.Now.Year - 4;endYear=DateTime.Now.Year;
              }
              for (int j =startYear; j <= endYear; j++)
              {
                   string sql1 = sql + string.Format(" and to_char(createdate,'yyyy')='{0}'", j);
                   int count = this.BaseRepository().FindObject(sql1).ToInt();
                   xValues.Add(count);
                   yValues.Add(j + "年");
              }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new {x=xValues,y=yValues });
        }
        /// <summary>
        /// 根据部门编码获取风险对比分析图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetRatherRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "一级,二级,三级,四级", string areaCode = "")
        {
            List<string> yValues = new List<string>();
            string sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where encode like '{0}%' ",deptCode);
            int len = deptCode.Length + 4;
            if(DbHelper.DbType==DatabaseType.Oracle)
            {
                sql += " and length(encode)<=" + len;
            }
            if (DbHelper.DbType == DatabaseType.SqlServer)
            {
                sql += " and len(encode)<=" + len;
            }
            sql += " order by encode,sortcode asc";
            DataTable dtDepts = this.BaseRepository().FindTable(sql);
            List<object> dic = new List<object>();
            string[] grades = riskGrade.TrimStart(',').Split(',');
            bool isRead = false;
            foreach(string grade in grades)
            {
                List<int> list = new List<int>();
                int j = 0;
                foreach(DataRow dept in dtDepts.Rows)
                {
                     if (!isRead)
                     {
                        yValues.Add(dept[1].ToString());
                     }
                     string dCode=dept[0].ToString();
                     sql = string.Format("select count(1) from BIS_RISKASSESS where status=1 and  districtname is not null and deletemark=0 and enabledmark=0 and grade='{0}'", grade);
                     if (!string.IsNullOrEmpty(areaCode))
                     {
                         sql += string.Format(" and areaCode like '{0}%'", areaCode);
                     }
                     if (!string.IsNullOrEmpty(year))
                     {
                         string[] arr = year.Split('|');
                         sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arr[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arr[1]).ToString("yyyy-MM-dd 23:59:59"));
                     }
                     sql += j > 0 ? " and deptcode like '" + dCode + "%'" : " and deptcode='" + dCode + "'";
                     int count = this.BaseRepository().FindObject(sql).ToInt();
                     list.Add(count);
                     j++;
                }
                dic.Add(new { name = grade + "风险", data = list });
                isRead = true;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }
        /// <summary>
        /// 根据部门编码获取风险对比分析图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetAreaRatherRiskStat(string deptCode, string year = "", string riskGrade = "一级,二级,三级,四级")
        {
            List<string> yValues = new List<string>();
            string dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
            string sql = string.Format("select t.districtcode,t.districtname from BIS_DISTRICT t where t.districtcode like '{0}%' ", dCode);
            int len = deptCode.Length + 4;
            if (DbHelper.DbType == DatabaseType.Oracle)
            {
                sql += " and length(districtcode)=6";
            }
            if (DbHelper.DbType == DatabaseType.SqlServer)
            {
                sql += " and len(districtcode)=6";
            }
            sql += " order by sortcode ";
            DataTable dtAreas = this.BaseRepository().FindTable(sql);
            List<object> dic = new List<object>();
            string[] grades = riskGrade.TrimStart(',').Split(',');
            foreach (string grade in grades)
            {
                
                List<int> list = new List<int>();
                bool isRead = false;
                foreach (DataRow area in dtAreas.Rows)
                {
                    sql = string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and grade='{0}'", grade);
                    if (!string.IsNullOrEmpty(year))
                    {
                        string[] arr = year.Split('|');
                        sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arr[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arr[1]).ToString("yyyy-MM-dd 23:59:59"));
                    }
                    sql += " and areacode like '" + area[0].ToString() + "%' and deptcode like '"+deptCode+"%'";
                    int count = this.BaseRepository().FindObject(sql).ToInt();
                    //if (count==0)
                    //{
                        if (!yValues.Contains(area[1].ToString()))
                        {
                            yValues.Add(area[1].ToString());
                        }
                        list.Add(count);
                    //}
                }
                dic.Add(new { name = grade + "风险", data = list });
                isRead = true;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }
        /// <summary>
        ///风险统计图表格
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetAreaRiskList(string deptCode, string year = "", string riskGrade = "一级,二级,三级,四级")
        {
            string dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
            string sql = string.Format("select t.districtcode,t.districtname from BIS_DISTRICT t where length(districtcode)=6 and t.districtcode like '{0}%' order by sortcode ", dCode);
            DataTable dtAreas = this.BaseRepository().FindTable(sql);
            List<RiskStatEntity> list = new List<RiskStatEntity>();
            decimal total = this.BaseRepository().FindObject(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deptcode like '{0}%' and districtname is not null and deletemark=0 and enabledmark=0", deptCode)).ToInt();
            foreach (DataRow area in dtAreas.Rows)
            {
                sql = string.Format("select grade from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and areacode like '{0}%' and deptcode like '{1}%'", area[0].ToString(), deptCode);
                if (!string.IsNullOrEmpty(year))
                {
                    string[] arr = year.Split('|');
                    sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arr[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arr[1]).ToString("yyyy-MM-dd 23:59:59"));
                }
                DataTable dt = this.BaseRepository().FindTable(sql);
                int count1 = riskGrade.Contains("一级") ? dt.Select("grade='一级'").Length : 0;
                int count2 = riskGrade.Contains("二级") ? dt.Select("grade='二级'").Length : 0;
                int count3 = riskGrade.Contains("三级") ? dt.Select("grade='三级'").Length : 0;
                int count4 = riskGrade.Contains("四级") ? dt.Select("grade='四级'").Length : 0;
                int sum = count4 + count3 + count2 + count1;
                //if (sum>0)
                //{
                    decimal percent = total == 0 ? 0 : decimal.Parse(sum.ToString()) / total;
                    percent = percent == 0 ? 0 : Math.Round(percent * 100, 2);
                  list.Add(new RiskStatEntity
                  { 
                   name=area[1].ToString(),
                   lev1=count1,
                   lev2=count2,
                   lev3=count3,
                   lev4=count4,
                   sum=sum,
                   percent = percent
                });
                //}
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new {
                total=1,page=1,records=list.Count,
                rows = list });
        }
        /// <summary>
        ///按部门获取风险对比数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetDeptRiskList(string deptCode, string year = "", string riskGrade = "一级,二级,三级,四级")
        {
            string sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where encode like '{0}%' ", deptCode);
            int len = deptCode.Length + 4;
            if (DbHelper.DbType == DatabaseType.Oracle)
            {
                sql += " and length(encode)<=" + len;
            }
            if (DbHelper.DbType == DatabaseType.SqlServer)
            {
                sql += " and len(encode)<=" + len;
            }
            sql += " order by encode,sortcode asc";
            DataTable dtDepts = this.BaseRepository().FindTable(sql);
            List<RiskStatEntity> list = new List<RiskStatEntity>();
            sql = string.Format("select count(1) from BIS_RISKASSESS where status=1 and deptcode like '{0}%' and districtname is not null and deletemark=0 and enabledmark=0", deptCode);
            if (!string.IsNullOrEmpty(year))
            {
                string[] arr = year.Split('|');
                sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arr[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arr[1]).ToString("yyyy-MM-dd 23:59:59"));
            }
            decimal total = this.BaseRepository().FindObject(sql).ToInt();
            int j = 0;
            riskGrade = riskGrade.TrimStart(',');
            foreach (DataRow dept in dtDepts.Rows)
            {
                string dCode = dept[0].ToString();
                sql = "select grade from BIS_RISKASSESS where status=1 and districtname is not null and deletemark=0 and enabledmark=0";
                sql += j > 0 ? " and deptcode like '" + dCode + "%'" : " and deptcode='" + dCode + "'";
                if (!string.IsNullOrEmpty(year))
                {
                    string[] arr = year.Split('|');
                    sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arr[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arr[1]).ToString("yyyy-MM-dd 23:59:59"));
                }
                DataTable dt = this.BaseRepository().FindTable(sql);
                int count1 = riskGrade.Contains("一级") ? dt.Select("grade='一级'").Length : 0;
                int count2 = riskGrade.Contains("二级") ? dt.Select("grade='二级'").Length : 0;
                int count3 = riskGrade.Contains("三级") ? dt.Select("grade='三级'").Length : 0;
                int count4 = riskGrade.Contains("四级") ? dt.Select("grade='四级'").Length : 0;
 
                int sum = count4 + count3 + count2 + count1;
                decimal percent = total == 0 ? 0 : decimal.Parse(sum.ToString()) / total;
                percent = percent == 0 ? 0 : Math.Round(percent * 100, 2);
                    list.Add(new RiskStatEntity
                    {
                        name = dept[1].ToString(),
                        lev1 = count1,
                        lev2 = count2,
                        lev3 = count3,
                        lev4 = count4,
                        sum = sum,
                        percent = percent
                    });
                    j++;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = list.Count,
                rows = list.OrderByDescending(t=>t.sum).ToList()
            });
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public int RemoveForm(string keyValue,string planId="")
        {
            RiskAssessEntity entity = this.BaseRepository().FindEntity(keyValue);
            if (entity.Status==1)
            {
                return this.BaseRepository().ExecuteBySql(string.Format("update BIS_RISKASSESS  set deletemark=1,state=2,planId='{1}' where id='{0}'",keyValue,planId));
            }
            return this.BaseRepository().ExecuteBySql(string.Format("update BIS_RISKASSESS  set deletemark=1 where id=@keyValue"), new DbParameter[] { DbParameters.CreateDbParameter("@keyValue", keyValue) });
            //return this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 根据区域Id删除风险记录
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public int Remove(string areaId)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_RISKASSESS where districtid=@areaId"), new DbParameter[] { DbParameters.CreateDbParameter("@areaId", areaId) });
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, RiskAssessEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (entity.Status == 1)
                {
                    entity.State = 1;
                }
                entity.Modify(keyValue);
                return this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                return this.BaseRepository().Insert(entity);
            }
        }
        
        #endregion
    }
}
