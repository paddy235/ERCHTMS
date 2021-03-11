using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：安全风险库
    /// </summary>
    public class RiskAssessService : RepositoryFactory<RiskAssessEntity>, RiskAssessIService
    {
        private DataItemDetailService detailservice = new DataItemDetailService();
        #region 获取数据
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<RiskAssessEntity> GetListFor(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from BIS_RISKASSESS where 1=1 " + queryJson).ToList();
        }

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
        public IEnumerable<RiskAssessEntity> GetPageListRisk(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            IEnumerable<RiskAssessEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType).Select(r =>
            {
                //得到检查内容和检查人员
                string content = "";
                DataTable dtContent = this.BaseRepository().FindTable(string.Format("select content from BIS_MEASURES riskid='{0}'", r.Id));
                foreach (DataRow item2 in dtContent.Rows)
                {
                    content += item2["content"].ToString() + "|";
                }
                content = content.TrimEnd('|');
                r.PostName = content;
                return r;
            });
            return list;
        }
        /// <summary>
        /// 加载所有数据
        /// </summary>
        /// <param name="sql">查询参数</param>
        /// <returns></returns>
        public IEnumerable<RiskAssessEntity> GetPageListRiskAll(string sql)
        {
            DatabaseType dataType = DbHelper.DbType;
            DataTable dtContent = this.BaseRepository().FindTable("select content,riskid from BIS_MEASURES ");
            IEnumerable<RiskAssessEntity> list = this.BaseRepository().FindList(sql).Select(r =>
            {
                //得到检查内容和检查人员
                string content = "";
                DataRow[] datar = dtContent.Select("riskid='" + r.Id + "'");
                foreach (DataRow item2 in datar)
                {
                    content += item2["content"].ToString() + "|";
                }
                content = content.TrimEnd('|');
                r.PostName = content;
                return r;
            });
            return list;
        }

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                //查询年份
                if (!queryParam["year"].IsEmpty())
                {
                    string year = queryParam["year"].ToString();
                    pagination.conditionJson += string.Format(" and to_char(createdate,'yyyy')='{0}'", year);
                }
                //风险类别
                if (!queryParam["riskType"].IsEmpty())
                {
                    string riskType = queryParam["riskType"].ToString();
                    pagination.conditionJson += string.Format(" and risktype ='{0}'", riskType);
                }
                //风险级别
                if (!queryParam["level"].IsEmpty())
                {
                    string level = queryParam["level"].ToString();
                    pagination.conditionJson += string.Format(" and gradeval={0}", level);
                }
                //风险点
                if (!queryParam["riskname"].IsEmpty())
                {
                    string name = queryParam["riskname"].ToString().Trim();
                    pagination.conditionJson += string.Format(" and riskname like '%{0}%'", name);
                }
                //区域名称
                if (!queryParam["areaname"].IsEmpty())
                {
                    string name = queryParam["areaname"].ToString().Trim();
                    pagination.conditionJson += string.Format(" and districtname='{0}'", name);
                }
                if (!queryParam["createCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and createuserdeptcode like '{0}%'", queryParam["createCode"].ToString());
                }
                if (!queryParam["fxname"].IsEmpty() && !queryParam["riskType"].IsEmpty())
                {
                    string name = queryParam["fxname"].ToString().Trim();
                    string type = queryParam["riskType"].ToString().Trim();
                    if (type == "作业")
                    {
                        pagination.conditionJson += string.Format(" and worktask='{0}'", name);
                    }
                    if (type == "设备")
                    {
                        pagination.conditionJson += string.Format(" and equipmentname='{0}'", name);
                    }
                    if (type == "区域")
                    {
                        pagination.conditionJson += string.Format(" and HjSystem='{0}'", name);
                    }
                    if (type == "岗位")
                    {
                        pagination.conditionJson += string.Format(" and JobName='{0}'", name);
                    }
                    if (type == "管理")
                    {
                        pagination.conditionJson += string.Format(" and dangersource='{0}'", name);
                    }
                    if (type == "工器具及危化品")
                    {
                        pagination.conditionJson += string.Format(" and ToolOrDanger='{0}'", name);
                    }

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
                    if (user.RoleName.Contains("省级"))
                    {
                        var dept = new ERCHTMS.Service.BaseManage.DepartmentService().GetEntityByCode(deptCode);
                        if (dept != null)
                        {
                            if (dept.Nature == "厂级")
                            {
                                pagination.conditionJson += string.Format(" and deptCode like '{0}%'", deptCode);
                            }
                        }
                    }
                    else
                    {

                        pagination.conditionJson += string.Format(" and deptCode like '{0}%'", deptCode);
                    }
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
                    string selfAssess = queryParam["selfAssess"].ToString().Trim();
                    pagination.conditionJson += string.Format(" and planid in(select planid from BIS_RISKPPLANDATA where userid='{0}' and DATATYPE=1)", selfAssess);
                }
                //查询关键字
                if (!queryParam["keyWord"].IsEmpty())
                {
                    string keyWord = queryParam["keyWord"].ToString().Trim();
                    pagination.conditionJson += string.Format(" and (Description like '%{0}%' or riskdesc like '%{0}%' or majorname like '%{0}%' or worktask like '%{0}%' or equipmentname like '%{0}%' or dangersource like '%{0}%' or ToolOrDanger like '%{0}%' or jobname like '%{0}%' or HjSystem like '%{0}%') ", keyWord.Trim());
                }
                if (!queryParam["pguser"].IsEmpty())
                {
                    if (("," + queryParam["pguser"] + ",").Contains("," + user.Account + ","))
                    {
                        pagination.conditionJson += string.Format(" and ( deptcode in (select DEPTCODE from bis_riskpplandata t where t.planid ='{0}' and DATATYPE=0) or deptcode ='{1}')", queryParam["plantid"].ToString(), user.DeptCode);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and deptcode ='{0}'", user.DeptCode);
                    }
                }

                //查询关键字
                if (!queryParam["keyWordNew"].IsEmpty())
                {
                    string keyWord = queryParam["keyWordNew"].ToString().Trim();
                    pagination.conditionJson += string.Format(" and  t.worktask like '%{0}%' ", keyWord.Trim());
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
                    if (initAreaId.Length > 0)
                    {
                        pagination.conditionJson += string.Format(" or areaId in('{0}')", initAreaId.Replace(",", "','"));
                    }

                }
                if (!queryParam["Districtid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and instr(','|| districtid || ',',',{0},')>0", queryParam["Districtid"].ToString());
                }
                if (!queryParam["orgcode"].IsEmpty())
                {
                    string orgcode = queryParam["orgcode"].ToString();
                    pagination.conditionJson += string.Format(" and createuserorgcode='{0}'", orgcode);
                }
                if (!queryParam["equipmentname"].IsEmpty())
                {
                    string equipmentname = queryParam["equipmentname"].ToString();
                    pagination.conditionJson += string.Format(" and equipmentname='{0}'", equipmentname);
                }
                if (!queryParam["name"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and name like '%{0}%'", queryParam["name"].ToString());
                }

                if (!queryParam["ListingId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and ListingId = '{0}'", queryParam["ListingId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        public DataTable GetPageControlListJson(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //查询年份
            if (!queryParam["year"].IsEmpty())
            {
                string year = queryParam["year"].ToString();
                pagination.p_tablename += string.Format(" and to_char(createdate,'yyyy')='{0}'", year);
            }
            //风险类别
            if (!queryParam["riskType"].IsEmpty())
            {
                string riskType = queryParam["riskType"].ToString();
                pagination.p_tablename += string.Format(" and risktype ='{0}'", riskType);
            }
            //风险级别
            if (!queryParam["level"].IsEmpty())
            {
                string level = queryParam["level"].ToString();
                pagination.p_tablename += string.Format(" and gradeval={0}", level);
            }
            //风险点
            if (!queryParam["riskname"].IsEmpty())
            {
                string name = queryParam["riskname"].ToString().Trim();
                pagination.p_tablename += string.Format(" and riskname like '%{0}%'", name);
            }
            //区域名称
            if (!queryParam["areaname"].IsEmpty())
            {
                string name = queryParam["areaname"].ToString().Trim();
                pagination.p_tablename += string.Format(" and districtname='{0}'", name);
            }
            if (!queryParam["createCode"].IsEmpty())
            {
                pagination.p_tablename += string.Format(" and createuserdeptcode like '{0}%'", queryParam["createCode"].ToString());
            }
            //计划ID
            if (!queryParam["planId"].IsEmpty())
            {
                string planId = queryParam["planId"].ToString();
                pagination.p_tablename += string.Format(" and newplanId ='{0}'", planId);
            }
            //岗位
            if (!queryParam["postId"].IsEmpty())
            {
                string postId = queryParam["postId"].ToString();
                pagination.p_tablename += string.Format(" and postId ='{0}'", postId);
            }
            //状态
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                pagination.p_tablename += string.Format(" and status ={0}", status);
            }
            //区域Code
            string areaCode = "";
            if (!queryParam["areaCode"].IsEmpty())
            {
                areaCode = queryParam["areaCode"].ToString();
                pagination.p_tablename += string.Format(" and instr( ',' || areaCode || ',',',{0},')>0", areaCode);
            }
            //区域ID
            if (!queryParam["areaId"].IsEmpty())
            {
                string areaId = queryParam["areaId"].ToString();
                pagination.p_tablename += string.Format(" and areaId = '{0}'", areaId);
            }
            //事故类型
            if (!queryParam["accType"].IsEmpty())
            {
                string accType = queryParam["accType"].ToString();
                pagination.p_tablename += string.Format(" and AccidentName like '%{0}%'", accType);
            }
            //部门Code
            if (!queryParam["deptCode"].IsEmpty())
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string deptCode = queryParam["deptCode"].ToString();
                if (user.RoleName.Contains("省级"))
                {
                    var dept = new ERCHTMS.Service.BaseManage.DepartmentService().GetEntityByCode(deptCode);
                    if (dept != null)
                    {
                        if (dept.Nature == "厂级")
                        {
                            pagination.p_tablename += string.Format(" and deptCode like '{0}%'", deptCode);
                        }
                    }
                }
                else
                {

                    pagination.p_tablename += string.Format(" and deptCode like '{0}%'", deptCode);
                }
            }
            if (!queryParam["deptCode1"].IsEmpty())
            {
                string deptCode = queryParam["deptCode1"].ToString();
                pagination.p_tablename += string.Format(" and deptCode='{0}'", deptCode);
            }
            //本人辨识的
            if (!queryParam["selfSpot"].IsEmpty())
            {
                string selfSpot = queryParam["selfSpot"].ToString();
                pagination.p_tablename += string.Format(" and planid in(select planid from BIS_RISKPPLANDATA where userid='{0}' and DATATYPE=0)", selfSpot);
            }
            //本人评估的
            if (!queryParam["selfAssess"].IsEmpty())
            {
                string selfAssess = queryParam["selfAssess"].ToString().Trim();
                pagination.p_tablename += string.Format(" and planid in(select planid from BIS_RISKPPLANDATA where userid='{0}' and DATATYPE=1)", selfAssess);
            }
            //查询关键字
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString().Trim();
                pagination.p_tablename += string.Format(" and (Description like '%{0}%' or riskdesc like '%{0}%' or majorname like '%{0}%' or worktask like '%{0}%' or equipmentname like '%{0}%' or dangersource like '%{0}%' or ToolOrDanger like '%{0}%' or jobname like '%{0}%' or HjSystem like '%{0}%') ", keyWord.Trim());
            }

            //查询关键字
            if (!queryParam["keyWordNew"].IsEmpty())
            {
                string keyWord = queryParam["keyWordNew"].ToString().Trim();
                pagination.p_tablename += string.Format(" and  t.worktask like '%{0}%' ", keyWord.Trim());
            }
            if (!queryParam["keyValue"].IsEmpty())
            {
                string keyValue = queryParam["keyValue"].ToString();
                pagination.p_tablename += string.Format(" and ( deptname like '%{0}%' or postname like '%{0}%') ", keyValue.Trim());
            }
            if (!queryParam["AreaIds"].IsEmpty())
            {
                string AreaIds = queryParam["AreaIds"].ToString();
                if (AreaIds.Length > 0)
                {
                    pagination.p_tablename += string.Format(" and districtid in('{0}')", AreaIds.Replace(",", "','"));
                }
            }
            if (!queryParam["initAreaId"].IsEmpty())
            {
                string initAreaId = GetAreaIdsByCode(areaCode);
                if (initAreaId.Length > 0)
                {
                    pagination.p_tablename += string.Format(" or areaId in('{0}')", initAreaId.Replace(",", "','"));
                }

            }
            if (!queryParam["orgcode"].IsEmpty())
            {
                string orgcode = queryParam["orgcode"].ToString();
                pagination.p_tablename += string.Format(" and createuserorgcode='{0}'", orgcode);
            }
            if (!queryParam["equipmentname"].IsEmpty())
            {
                string equipmentname = queryParam["equipmentname"].ToString();
                pagination.p_tablename += string.Format(" and equipmentname='{0}'", equipmentname);
            }
            if (!queryParam["name"].IsEmpty())
            {
                pagination.p_tablename += string.Format(" and name like '%{0}%'", queryParam["name"].ToString());
            }

            if (!queryParam["ListingId"].IsEmpty())
            {
                pagination.p_tablename += string.Format(" and ListingId = '{0}'", queryParam["ListingId"].ToString());
            }

            pagination.p_tablename += " group by deptcode,deptname,name,risktype";
            //风险等级
            if (!queryParam["grade"].IsEmpty())
            {
                string grade = queryParam["grade"].ToString();
                pagination.p_tablename += string.Format(" having min(gradeval) = '{0}'", grade);
            }
            pagination.p_tablename += ")";
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
            foreach (DataRow dr in dt.Rows)
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
            int count = dt.Rows.Count;
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
        public string GetRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "低风险,一般风险,较大风险,重大风险")
        {
            string gxhs = detailservice.GetItemValue("广西华昇版本");
            List<object[]> list = new List<object[]>();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string sql = string.Format("select  grade,count(1) from BIS_RISKASSESS where status=1 and deptcode like '{0}%' and  deletemark=0 and enabledmark=0 ", deptCode);
            if (!string.IsNullOrEmpty(year))
            {
                string[] arrDate = year.Split('|');
                sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arrDate[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arrDate[1]).ToString("yyyy-MM-dd 23:59:59"));
            }
            sql += " group by grade";
            DataTable dt = this.BaseRepository().FindTable(sql);

            int count = dt.Select("grade='低风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='低风险'")[0][1].ToString());
            object[] arr = { string.IsNullOrWhiteSpace(gxhs) ? "低风险" : "四级风险", count }; list.Add(arr);
            count = dt.Select("grade='一般风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='一般风险'")[0][1].ToString());
            arr = new object[] { string.IsNullOrWhiteSpace(gxhs) ? "一般风险" : "三级风险", count }; list.Add(arr);
            count = dt.Select("grade='较大风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='较大风险'")[0][1].ToString());
            arr = new object[] { string.IsNullOrWhiteSpace(gxhs) ? "较大风险" : "二级风险", count }; list.Add(arr);
            count = dt.Select("grade='重大风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='重大风险'")[0][1].ToString());
            arr = new object[] { string.IsNullOrWhiteSpace(gxhs) ? "重大风险" : "一级风险", count }; list.Add(arr);
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
            string dCode = string.Empty;
            List<object[]> list = new List<object[]>();
            var d = new DepartmentService().GetEntityByCode(deptCode);
            if (d != null)
            {
                var p = new DepartmentService().GetEntity(d.OrganizeId);
                if (p != null)
                {
                    dCode = p.EnCode;
                }
                else
                {
                    dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
                }
            }
            else
            {
                dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
            }
            string sql = string.Format("select t.districtcode,t.districtname from BIS_DISTRICT t where parentid='0' and t.districtcode like '{0}%' order by sortcode ", dCode);
            DataTable dtAreas = this.BaseRepository().FindTable(sql);
            foreach (DataRow area in dtAreas.Rows)
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS where status=1 and deptcode like '{0}%' and  deletemark=0 and enabledmark=0  and instr( ',' || areacode, ',{1}')>0", deptCode, area[0].ToString());
                if (!string.IsNullOrEmpty(year))
                {
                    string[] arr = year.Split('|');
                    sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arr[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arr[1]).ToString("yyyy-MM-dd 23:59:59"));
                }
                int count = this.BaseRepository().FindObject(sql).ToInt();
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
        public string GetYearRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "低风险,一般风险,较大风险,重大风险", string areaCode = "")
        {
            List<int> xValues = new List<int>();
            List<string> yValues = new List<string>();
            string sql = string.Format("select count(1) from BIS_RISKASSESS where deletemark=0 and enabledmark=0 and status=1 and deptcode like '{0}%'", deptCode);
            if (!string.IsNullOrEmpty(riskGrade))
            {
                sql += string.Format(" and grade in('{0}')", riskGrade.Replace(",", "','"));
            }
            if (!string.IsNullOrEmpty(areaCode))
            {
                sql += string.Format(" and areaCode like '{0}%'", areaCode);
            }
            int startYear = 0; int endYear = 0;
            if (!string.IsNullOrEmpty(year))
            {
                string[] arr = year.Split('|');
                startYear = int.Parse(arr[0]);
                endYear = int.Parse(arr[1]);

            }
            else
            {
                startYear = DateTime.Now.Year - 4; endYear = DateTime.Now.Year;
            }
            for (int j = startYear; j <= endYear; j++)
            {
                string sql1 = sql + string.Format(" and to_char(createdate,'yyyy')='{0}'", j);
                int count = this.BaseRepository().FindObject(sql1).ToInt();
                xValues.Add(count);
                yValues.Add(j + "年");
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = xValues, y = yValues });
        }
        /// <summary>
        /// 根据部门编码获取风险对比分析图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetRatherRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "低风险,一般风险,较大风险,重大风险", string areaCode = "")
        {
            string gxhs = detailservice.GetItemValue("广西华昇版本");
            List<string> yValues = new List<string>();
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
            List<object> dic = new List<object>();
            string[] grades = riskGrade.TrimStart(',').Split(',');
            bool isRead = false;
            foreach (string grade in grades)
            {
                List<int> list = new List<int>();
                int j = 0;
                foreach (DataRow dept in dtDepts.Rows)
                {
                    if (!isRead)
                    {
                        yValues.Add(dept[1].ToString());
                    }
                    string dCode = dept[0].ToString();
                    sql = string.Format("select count(1) from BIS_RISKASSESS where status=1  and deletemark=0 and enabledmark=0 and grade='{0}'", grade);
                    if (!string.IsNullOrEmpty(areaCode))
                    {
                        sql += string.Format(" and instr( ',' || areaCode,',{0}')>0", areaCode);
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
                dic.Add(new { name = !string.IsNullOrWhiteSpace(gxhs) ? grade.Replace("重大风险", "一级风险").Replace("较大风险", "二级风险").Replace("一般风险", "三级风险").Replace("低风险", "四级风险") : grade, data = list });
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
        public string GetAreaRatherRiskStat(string deptCode, string year = "", string riskGrade = "低风险,一般风险,较大风险,重大风险")
        {
            string gxhs = detailservice.GetItemValue("广西华昇版本");
            List<string> yValues = new List<string>();
            string dCode = string.Empty;
            var d = new DepartmentService().GetEntityByCode(deptCode);
            if (d != null)
            {
                var p = new DepartmentService().GetEntity(d.OrganizeId);
                if (p != null)
                {
                    dCode = p.EnCode;
                }
                else
                {
                    dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
                }
            }
            else
            {
                dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
            }
            //string dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
            string sql = string.Format("select t.districtcode,t.districtname from BIS_DISTRICT t where parentid='0' and t.districtcode like '{0}%' ", dCode);
            sql += " order by sortcode ";
            DataTable dtAreas = this.BaseRepository().FindTable(sql);
            List<object> dic = new List<object>();
            string[] grades = riskGrade.TrimStart(',').Split(',');
            foreach (string grade in grades)
            {

                List<int> list = new List<int>();
                foreach (DataRow area in dtAreas.Rows)
                {
                    sql = string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and grade='{0}'", grade);
                    if (!string.IsNullOrEmpty(year))
                    {
                        string[] arr = year.Split('|');
                        sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arr[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arr[1]).ToString("yyyy-MM-dd 23:59:59"));
                    }
                    sql += " and instr(','|| areacode,'," + area[0].ToString() + "')>0 and deptcode like '" + deptCode + "%'";
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
                dic.Add(new { name = !string.IsNullOrWhiteSpace(gxhs) ? grade.Replace("重大风险", "一级风险").Replace("较大风险", "二级风险").Replace("一般风险", "三级风险").Replace("低风险", "四级风险") : grade, data = list });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }
        /// <summary>
        ///风险统计图表格
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetAreaRiskList(string deptCode, string year = "", string riskGrade = "低风险,一般风险,较大风险,重大风险")
        {
            string dCode = string.Empty;
            var d = new DepartmentService().GetEntityByCode(deptCode);
            if (d != null)
            {
                var p = new DepartmentService().GetEntity(d.OrganizeId);
                if (p != null)
                {
                    dCode = p.EnCode;
                }
                else
                {
                    dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
                }
            }
            else
            {
                dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
            }
            //string dCode = deptCode.Length >= 3 ? deptCode.Substring(0, 3) : deptCode;
            string sql = string.Format("select t.districtcode,t.districtname from BIS_DISTRICT t where parentid='0' and t.districtcode like '{0}%' order by sortcode ", dCode);
            DataTable dtAreas = this.BaseRepository().FindTable(sql);
            List<RiskStatEntity> list = new List<RiskStatEntity>();
            decimal total = this.BaseRepository().FindObject(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deptcode like '{0}%' and districtname is not null and deletemark=0 and enabledmark=0", deptCode)).ToInt();
            foreach (DataRow area in dtAreas.Rows)
            {
                sql = string.Format("select grade from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and instr(','|| areacode,',{0}')>0 and instr(','|| deptcode, ',{1}')>0", area[0].ToString(), deptCode);
                if (!string.IsNullOrEmpty(year))
                {
                    string[] arr = year.Split('|');
                    sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arr[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arr[1]).ToString("yyyy-MM-dd 23:59:59"));
                }
                DataTable dt = this.BaseRepository().FindTable(sql);
                int count1 = riskGrade.Contains("重大风险") ? dt.Select("grade='重大风险'").Length : 0;
                int count2 = riskGrade.Contains("较大风险") ? dt.Select("grade='较大风险'").Length : 0;
                int count3 = riskGrade.Contains("一般风险") ? dt.Select("grade='一般风险'").Length : 0;
                int count4 = riskGrade.Contains("低风险") ? dt.Select("grade='低风险'").Length : 0;
                int sum = count4 + count3 + count2 + count1;
                //if (sum>0)
                //{
                decimal percent = total == 0 ? 0 : decimal.Parse(sum.ToString()) / total;
                percent = percent == 0 ? 0 : Math.Round(percent * 100, 2);
                list.Add(new RiskStatEntity
                {
                    name = area[1].ToString(),
                    lev1 = count1,
                    lev2 = count2,
                    lev3 = count3,
                    lev4 = count4,
                    sum = sum,
                    percent = percent
                });
                //}
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
        ///按部门获取风险对比数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetDeptRiskList(string deptCode, string year = "", string riskGrade = "低风险,一般风险,较大风险,重大风险")
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
                int count1 = riskGrade.Contains("重大风险") ? dt.Select("grade='重大风险'").Length : 0;
                int count2 = riskGrade.Contains("较大风险") ? dt.Select("grade='较大风险'").Length : 0;
                int count3 = riskGrade.Contains("一般风险") ? dt.Select("grade='一般风险'").Length : 0;
                int count4 = riskGrade.Contains("低风险") ? dt.Select("grade='低风险'").Length : 0;

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
                rows = list.OrderByDescending(t => t.sum).ToList()
            });
        }
        /// <summary>
        /// 根据区域获取设备信息
        /// </summary>
        /// <param name="areaId">电厂区域Id</param>
        /// <returns></returns>
        public DataTable GetEuqByAreaId(string areaId)
        {
            DataTable dt = this.BaseRepository().FindTable(string.Format("select distinct t.equipmentname,replace(t.equipmentname,' ','') as nameValue from BIS_RISKASSESS t where risktype='设备' and t.districtid='{0}'", areaId));
            return dt;
        }

        /// <summary>
        /// 获取首页风险指标
        /// 省级使用
        /// </summary>
        /// <param name="riskGrade">风险等级</param>
        /// <param name="type">1:重大风险 2 较大风险 3 重大风险超过3个</param>
        /// <returns></returns>
        public DataTable GetIndexRiskTarget(string riskGrade, Operator currUser, int type)
        {
            string strWhere = " having count(1)>=3";
            string gradeWhere = string.Empty;
            string sql = string.Format(@" select count(id) num,d.fullname deptname,d.encode deptcode 
                                          from BIS_RISKASSESS t
                                          inner join (select b.encode, b.fullname
                                                       from BASE_DEPARTMENT b
                                                      where b.deptcode like '{0}%'
                                                        and b.nature = '厂级') d
                                            on d.encode = t.createuserorgcode
                                         where t.status=1 and t.deletemark=0 and t.enabledmark=0 ", currUser.NewDeptCode);
            if (!string.IsNullOrWhiteSpace(riskGrade))
            {
                gradeWhere = string.Format(" and t.grade = '{0}'", riskGrade);
                sql += gradeWhere;
            }
            string groupby = "group by d.fullname,d.encode";
            sql += groupby;
            if (type == 3)
            {
                sql += strWhere;
            }
            return this.BaseRepository().FindTable(sql);
        }

        public DataTable FindTableBySql(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public int RemoveForm(string keyValue, string planId = "")
        {
            RiskAssessEntity entity = this.BaseRepository().FindEntity(keyValue);
            if (entity.Status == 1)
            {
                return this.BaseRepository().ExecuteBySql(string.Format("update BIS_RISKASSESS  set deletemark=1,state=2,planId='{1}' where id='{0}'", keyValue, planId));
            }
            return this.BaseRepository().ExecuteBySql(string.Format("update BIS_RISKASSESS  set deletemark=1 where id='{0}'", keyValue));
            //return this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int Delete(string ids)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("update BIS_RISKASSESS  set deletemark=1 where id in ('{0}')", ids.Replace(",", "','")));
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




        public List<RiskAssessEntity> RiskCount(bool includeDynamic)
        {
            var db = DbFactory.Base();

            var recursiveQuery = from q in db.IQueryable<DistrictEntity>()
                                 where q.ParentID == "0"
                                 select new { RootId = q.DistrictID, q.DistrictID, q.ParentID };

            var subQuery = from q1 in db.IQueryable<DistrictEntity>()
                           join q2 in recursiveQuery on q1.ParentID equals q2.DistrictID
                           select new { q2.RootId, q1.DistrictID, q1.ParentID };

            if (subQuery.Count() > 0)
            {
                recursiveQuery = recursiveQuery.Concat(subQuery);
                subQuery = from q1 in db.IQueryable<DistrictEntity>()
                           join q2 in subQuery on q1.ParentID equals q2.DistrictID
                           select new { q2.RootId, q1.DistrictID, q1.ParentID };
            }



            var query1 = from q1 in recursiveQuery
                         join q2 in db.IQueryable<RiskAssessEntity>() on q1.DistrictID equals q2.DistrictId
                         select new { DistrictID = q1.RootId, GradeVal = q2.GradeVal };

            var query2 = from q1 in recursiveQuery
                         join q2 in db.IQueryable<SafeworkcontrolEntity>() on q1.DistrictID equals q2.Taskregionid
                         where q2.State == 1
                         select new { DistrictID = q1.RootId, GradeVal = (int?)(q2.DangerLevel == "一级风险" ? 1 : q2.DangerLevel == "二级风险" ? 2 : q2.DangerLevel == "三级风险" ? 3 : 4) };

            var query = query1;
            if (includeDynamic) query = query1.Concat(query2);

            var dataquery = query.GroupBy(x => x.DistrictID, y => y.GradeVal, (x, y) => new { DistrictID = x, GradeVal = y.Min() });

            return dataquery.ToList().Select(x => new RiskAssessEntity { DistrictId = x.DistrictID, GradeVal = x.GradeVal }).ToList();
        }

        public List<RiskAssessEntity> RiskCount2(string id)
        {
            var db = DbFactory.Base();

            var riskquery = from q in db.IQueryable<DistrictEntity>()
                            where q.DistrictID == id
                            select q;
            var subquery = from q1 in db.IQueryable<DistrictEntity>()
                           join q2 in riskquery on q1.ParentID equals q2.DistrictID
                           select q1;

            while (subquery.Count() > 0)
            {
                riskquery = riskquery.Concat(subquery);
                subquery = from q1 in db.IQueryable<DistrictEntity>()
                           join q2 in subquery on q1.ParentID equals q2.DistrictID
                           select q1;
            }

            var query = from q1 in db.IQueryable<RiskAssessEntity>()
                        join q2 in riskquery on q1.DistrictId equals q2.DistrictID
                        select new { q1.Id, q1.Grade };

            return query.ToList().Select(x => new RiskAssessEntity { Id = x.Id, Grade = x.Grade }).ToList();
        }

        public List<RiskAssessEntity> RiskCount3(string id)
        {
            var db = DbFactory.Base();

            var riskquery = from q in db.IQueryable<DistrictEntity>()
                            where q.DistrictID == id
                            select q;
            var subquery = from q1 in db.IQueryable<DistrictEntity>()
                           join q2 in riskquery on q1.ParentID equals q2.DistrictID
                           select q1;

            while (subquery.Count() > 0)
            {
                riskquery = riskquery.Concat(subquery);
                subquery = from q1 in db.IQueryable<DistrictEntity>()
                           join q2 in subquery on q1.ParentID equals q2.DistrictID
                           select q1;
            }

            var query = from q1 in riskquery
                        join q2 in db.IQueryable<HTBaseInfoEntity>() on q1.DistrictCode equals q2.HIDPOINT
                        join q3 in db.IQueryable<DataItemDetailEntity>() on q2.HIDRANK equals q3.ItemDetailId
                        where q2.WORKSTREAM == "隐患整改"
                        select new { q2.ID, q3.ItemName };

            return query.ToList().Select(x => new RiskAssessEntity { Id = x.ID, Grade = x.ItemName }).ToList();
        }

        public List<RiskAssessEntity> RiskCount4(string id)
        {
            //var db = DbFactory.Base();

            //var riskquery = from q in db.IQueryable<DistrictEntity>()
            //                where q.DistrictID == id
            //                select q;
            //var subquery = from q1 in db.IQueryable<DistrictEntity>()
            //               join q2 in riskquery on q1.ParentID equals q2.DistrictID
            //               select q1;

            //while (subquery.Count() > 0)
            //{
            //    riskquery = riskquery.Concat(subquery);
            //    subquery = from q1 in db.IQueryable<DistrictEntity>()
            //               join q2 in subquery on q1.ParentID equals q2.DistrictID
            //               select q1;
            //}

            //var query = from q1 in riskquery
            //            join q2 in db.IQueryable<SafeworkcontrolEntity>() on q1.DistrictID equals q2.Taskregionid
            //            where q2.State == 1
            //            select new { q2.ID, q2.DangerLevel };

            //return query.ToList().Select(x => new RiskAssessEntity { Id = x.ID, Grade = x.DangerLevel }).ToList();


            var db = DbFactory.Base();
            var areaCode = db.IQueryable<DistrictEntity>().FirstOrDefault(x => x.DistrictID == id)?.DistrictCode;
            var query = from s in db.IQueryable<SafeworkcontrolEntity>()
                        where s.State == 1 && s.Taskregioncode.StartsWith(areaCode)
                        select new { s.ID, s.DangerLevel };
            return query.ToList().Select(x => new RiskAssessEntity { Id = x.ID, Grade = x.DangerLevel }).ToList();

        }
    }
}
