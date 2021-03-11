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
    public class RiskService : RepositoryFactory<RiskEntity>, RiskIService
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
        public IEnumerable<RiskEntity> GetList(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord)
        {
            var expression = LinqExtensions.True<RiskEntity>();
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
                expression = expression.And(t =>accType.Contains(t.AccidentName));
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
            //状态
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                pagination.conditionJson += string.Format(" and status ={0}", status);
            }
            //区域Code
            if (!queryParam["areaCode"].IsEmpty())
            {
                string areaCode = queryParam["areaCode"].ToString();
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
            if (!queryParam["deptCode"].IsEmpty())
            {
                string deptCode = queryParam["deptCode"].ToString();
                pagination.conditionJson += string.Format(" and deptCode like '{0}%'", deptCode);
            }
            //查询关键字
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString();
                pagination.conditionJson += string.Format(" and (Description like '%{0}%' or DangerSource like '%{0}%' or result like '%{0}%') ", keyWord.Trim());
            }
            if (!queryParam["initAreaId"].IsEmpty())
            {
                string initAreaId = queryParam["initAreaId"].ToString().TrimEnd(',');
                pagination.conditionJson += string.Format(" or areaId in('{0}')", initAreaId.Replace(",","','"));
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskEntity GetEntity(string keyValue)
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
            DbParameter []param={
                        DbParameters.CreateDbParameter("@dangerId",dangerId)
                                };
            DataTable dt = this.BaseRepository().FindTable("select deptcode from BIS_RISKDATABASE  where dangerid=@dangerId", param);
            StringBuilder sb=new StringBuilder();
            foreach(DataRow dr in dt.Rows)
            {
                sb.Append(dr[0].ToString()+",");
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
        /// 首页风险工作指标统计
        /// </summary>
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        public string GetHomeStat(ERCHTMS.Code.Operator user)
        {
            string roleNames = user.RoleName;
            string sql = "select grade,count(1) from BIS_RISKASSESS where  status=1 and deletemark=0 and  to_char(createdate,'yyyy')='" + DateTime.Now.Year + "'";
            if (!(user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户")))
            {
                sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
            }
            else
            {
                sql += string.Format(" and deptcode like '" + user.OrganizeCode + "%'");
            }
            sql += " group by grade";
            
            DataTable dt = this.BaseRepository().FindTable(sql);

            int count1 = dt.Select("grade='一级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='一级'")[0][1].ToString());
            int count2 = dt.Select("grade='二级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='二级'")[0][1].ToString());
            int count3 = dt.Select("grade='三级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='三级'")[0][1].ToString());
            int count4 = dt.Select("grade='四级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='四级'")[0][1].ToString());
            int sum = count1 + count2 + count3 + count4;

            sql = "select count(1) from BIS_RISKASSESS where to_char(createdate,'yyyy')='" + (DateTime.Now.Year - 1) + "'";
            if (!(user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户")))
            {
                sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
            }
            int total = this.BaseRepository().FindObject(sql).ToInt();
            decimal percent = sum==0?0:(decimal.Parse(total.ToString()) / decimal.Parse(sum.ToString())) - 1;
            percent= Math.Abs(percent)*100;
            List<object> list = new List<object>() { sum, count1, count2, count3, count4, Math.Round(percent,2)};
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 首页风险排名
        /// </summary>
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        public string GetRiskRank(ERCHTMS.Code.Operator user)
        {
            string roleNames = user.RoleName;
            string sql = string.Format("select * from (select t.dangersource,t.itemr,t.risktype,t.result,t.deptname,t.postname,grade from BIS_RISKASSESS t where status=1 and deletemark=0 and deptcode like '{0}%'", user.OrganizeCode);
            sql+="  and t.itemr is not null  order by gradeval asc,t.itemr desc) t where rownum<=10";
            DataTable dt = this.BaseRepository().FindTable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public int RemoveForm(string keyValue)
        {
            return this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, RiskEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
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

        #region 手机app端使用
        #region  11.1-12.3 风险清单及辨识
        /// <summary>
        /// 11.1 风险清单列表
        /// </summary>
        /// <param name="type">查询方式，1：我的，2：重大风险，3：全部</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险级别</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetAreaList(int type, string areaId, string grade, ERCHTMS.Code.Operator user)
        {
            string sql = "select t.districtid identity,t.districtname identityName,nvl(a.identityNum,0) identityNum  from BIS_DISTRICT t right join (select districtname identityName,districtid,count(1) identityNum from BIS_RISKASSESS where status=1 and deletemark=0 and deptcode like '" + user.OrganizeCode + "%'";
            if (type == 1)
            {
                sql += " and grade is not null and createuserid ='" + user.UserId + "'";
            }
            if (type == 2)
            {
                sql += " and grade is not null and gradeval=1";
            }
            if (type == 3)
            {
                string roleNames = user.RoleName;
                if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
                {
                    sql += " and deptcode like '" + user.DeptCode + "%'";
                }
            }
            if (!string.IsNullOrEmpty(areaId))
            {
                sql += " and grade is not null and districtid='" + areaId + "'";
            }
            if (!string.IsNullOrEmpty(grade))
            {
                sql += " and grade='" + grade + "'";
            }
            sql += " group by districtname,districtid) a on t.districtid=a.districtid  where t.organizeid='" + user.OrganizeId + "' order by t.districtcode,sortcode ";
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 11.2 根据区域ID获取风险清单
        /// </summary>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        public DataTable GetRiskList(string areaId, ERCHTMS.Code.Operator user, long type, string grade)
        {
            string sql = string.Format("select id riskId,dangersource riskDescribe,grade riskLevel from BIS_RISKASSESS where  districtid='{0}' and deletemark=0 and enabledmark=0 ", areaId);
            if (type == 1)
            {
                sql += " and grade is not null and createuserid ='" + user.UserId + "'";
            }
            if (type == 2)
            {
                sql += " and grade is not null and gradeval=1";
            }
            if (type == 3)
            {
                string roleNames = user.RoleName;
                if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
                {
                    sql += " and deptcode like '" + user.DeptCode + "%'";
                }
            }
            if (!string.IsNullOrEmpty(areaId))
            {
                sql += " and grade is not null and districtid='" + areaId + "'";
            }
            if (!string.IsNullOrEmpty(grade))
            {
                sql += " and grade='" + grade + "'";
            }
            sql += "order by createdate desc";
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 11.3 根据风险ID获取风险详细信息
        /// </summary>
        /// <param name="riskId">风险记录ID</param>
        /// <returns></returns>
        public object GetRisk(string riskId)
        {
            string sql = string.Format(@"select t.districtname riskArea,t.deptname managerDutyDept,t.dangersource riskDescribe,t.HarmType dangerAttribute,Result,
            RiskType dangerCatogery,postname station,t.way approveMethod,t.itema happendedPossibility,t.itemb frequentDegree,t.itemc consequenceSerioucness,t.itemr riskValue,t.grade riskLevel,a.status,t.deptcode,t.postid from BIS_RISKASSESS t left join BIS_RISKPLAN a on t.planid=a.id  where t.id='{0}'", riskId);
            DataTable dt = this.BaseRepository().FindTable(sql);


            List<string> measures = new List<string>();
            DataTable dtMeasures = this.BaseRepository().FindTable(string.Format("select content from BIS_MEASURES t where t.riskid='{0}'", riskId));
            foreach (DataRow dr in dtMeasures.Rows)
            {
                measures.Add(dr[0].ToString());
            }
            object obj = new
            {
                riskarea = dt.Rows[0][0].ToString(),
                managerdutydept = dt.Rows[0][1].ToString(),
                riskdescribe = dt.Rows[0][2].ToString(),
                dangerattribute = dt.Rows[0][3].ToString(),
                dangerresult = dt.Rows[0][4].ToString(),
                dangercatogery = dt.Rows[0][5].ToString(),
                station = dt.Rows[0][6].ToString(),
                approvemethod = dt.Rows[0][7].ToString(),
                happendedpossibility = dt.Rows[0][8].ToString(),
                frequentdegree = dt.Rows[0][9].ToString(),
                consequenceserioucness = dt.Rows[0][10].ToString(),
                riskvalue = dt.Rows[0][11].ToString(),
                risklevel = dt.Rows[0][12].ToString(),
                measures = measures,
                deptcode = dt.Rows[0]["deptcode"].ToString(),
                jobid = dt.Rows[0]["postid"].ToString(),
                status = dt.Rows[0]["status"].ToString()
            };
            dt.Dispose(); dtMeasures.Dispose();
            return obj;
        }

        /// <summary>11.4 区域</summary>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetAreas(ERCHTMS.Code.Operator user, string planId)
        {
            string sql = string.Format("select t.districtid identifyAreaId,t.districtcode identifyAreaCode,t.districtname identifyAreaName,chargedeptcode deptcode,chargedept deptname from BIS_DISTRICT t where t.districtid!='0' and organizeid='{0}'", user.OrganizeId);
            if (!user.RoleName.Contains("公司级用户") && !user.RoleName.Contains("厂级部门用户"))
            {
                sql += " and chargedeptcode like '" + user.DeptCode + "%'";
            }
            if (!string.IsNullOrEmpty(planId))
            {
                StringBuilder sb = new StringBuilder(",");
                DataTable dt = this.BaseRepository().FindTable(string.Format("select areaid from BIS_RISKPPLANDATA where datatype=0 and userid='{0}' and planid='{1}'", user.Account, planId));
                foreach (DataRow dr in dt.Rows)
                {
                    if (!sb.ToString().Contains("," + dr[0].ToString() + ","))
                    {
                        sb.AppendFormat("{0},", dr[0].ToString());
                    }
                }

                sql += string.Format(" and districtid in('{0}')", sb.ToString().Trim(',').Replace(",", "','"));
            }
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>12.1	获取辨识计划列表</summary>
        /// <returns></returns>
        public DataTable GetPlanList(ERCHTMS.Code.Operator user, string condition)
        {
            string sql = string.Format("select id riskIdentifyId,t.planname riskIdentifyName,t.startdate riskIdentifyTime,t.enddate,t.deptname,status riskIdentifyDepart,status from BIS_RISKPLAN t where ");
            sql += condition;
            sql += " order by createdate desc";
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>12.2	根据计划ID获取辨识区域</summary>
        /// <param name="planId">计划ID</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetAreaByPlanId(string planId, int status, ERCHTMS.Code.Operator user)
        {
            DataTable dt = this.BaseRepository().FindTable(string.Format("select areaid,enddate,status from BIS_RISKPLAN where id='{0}'", planId));
            string areaIds = dt.Rows[0][0].ToString(); string endDate = dt.Rows[0][1].ToString();
            StringBuilder sb = new StringBuilder(",");
            dt = this.BaseRepository().FindTable(string.Format("select areaid from BIS_RISKPPLANDATA where datatype=0 and userid='{0}' and planid='{1}'", user.Account, planId));
            foreach (DataRow dr in dt.Rows)
            {
                if (!sb.ToString().Contains("," + dr[0].ToString() + ","))
                {
                    sb.AppendFormat("{0},", dr[0].ToString());
                }
            }
            if (dt.Rows.Count > 0)
            {
                areaIds = sb.ToString().TrimEnd(',');
            }
            string sql = string.Format("select t.districtid identifyAreaId,t.districtcode identifyAreaCode,t.districtname identifyAreaName from BIS_DISTRICT t ");
            if (string.IsNullOrEmpty(planId))
            {
                sql += string.Format(" where t.districtid!='0' and t.organizeid='{0}' order by districtcode,sortcode", user.OrganizeId);
            }
            else
            {
                sql = string.Format("select t.districtid identifyAreaId,t.districtcode identifyAreaCode,t.districtname identifyAreaName,nvl(identityNum,0) identityNum,t.chargedept,t.chargedeptcode from BIS_DISTRICT t ", user.OrganizeId);
                if (status == 0)
                {
                    sql += string.Format("left join (select districtid,count(1) identityNum from BIS_RISKASSESS where status>0 and deletemark=0 and createdate<=to_date('{0}','yyyy-mm-dd hh24:mi:ss')  and districtid in('{1}')", endDate, areaIds.Replace(",", "','"));
                }
                else
                {
                    sql += string.Format("left join (select districtid,count(1) identityNum from BIS_RISKHISTORY where status>0 and deletemark=0 and newplanid='{0}'", planId);
                }
                sql += " group by districtid) b on t.districtid=b.districtid  where organizeid='" + user.OrganizeId + "' and t.districtid in('" + areaIds.Replace(",", "','") + "') and t.districtid!='0' order by districtcode,sortcode";
            }
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>12.3	根据计划ID和区域Code获取风险清单</summary>
        /// <param name="planId">计划ID</param>
        /// <param name="areaCode">区域Code</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetRiskListByPlanId(string planId, string areaCode, int status, ERCHTMS.Code.Operator user)
        {
            string sql = "";
            if (status == 0)
            {
                DataTable dt = this.BaseRepository().FindTable(string.Format("select areaid,enddate from BIS_RISKPLAN where id='{0}'", planId));
                string areaIds = dt.Rows[0][0].ToString(); string endDate = dt.Rows[0][1].ToString();
                sql = string.Format(@"select t.districtname riskArea,t.deptname controlDutyStation,t.dangersource riskDescribe,t.HarmType harmProperty,Result riskResult,t.id riskid,t.postid jobid,t.deptcode,status,createuserid userid,
            RiskType riskCategory,postname job,t.way approveMethod,t.itema happendedPossibility,t.itemb frequentDegree,t.itemc consequenceSerioucness,t.itemr riskValue,t.grade riskLevel from BIS_RISKASSESS t  where status>0 and deletemark=0 and createdate<=to_date('{1}','yyyy-mm-dd hh24:mi:ss') and areacode like '{0}%' and districtid in('{2}')", areaCode, endDate, areaIds.Replace(",", "','"));
            }
            else
            {
                sql = string.Format(@"select t.districtname riskArea,t.deptname controlDutyStation,t.dangersource riskDescribe,t.HarmType harmProperty,Result riskResult,t.id riskid,t.postid jobid,t.deptcode,status,createuserid userid,
               RiskType riskCategory,postname job,t.way approveMethod,t.itema happendedPossibility,t.itemb frequentDegree,t.itemc consequenceSerioucness,t.itemr riskValue,t.grade riskLevel from BIS_RISKHISTORY t  where status>0 and deletemark=0 and areacode like '{0}%' and newplanid='{1}'", areaCode, planId);
            }
            sql += " order by createdate desc";
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 12.4 新增风险辨识信息
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public int SaveRisk(RiskAssessEntity assess, ERCHTMS.Code.Operator user)
        {
            string sql = "";
            if (string.IsNullOrEmpty(assess.Id))
            {
                sql = string.Format(@"insert into BIS_RISKASSESS(id,dangersource,deptcode,deptname,postid,postname,createuserid,createdate,createusername,createuserdeptcode,createuserorgcode,status,result,harmtype,risktype,DeleteMark,state,districtid,districtname,planid,areacode) values(
     '{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}','{10}',{11},'{12}','{13}','{14}',{15},{16},'{17}','{18}','{19}','{20}')", Guid.NewGuid().ToString(), assess.DangerSource, assess.DeptCode, assess.DeptName, assess.PostId, assess.PostName, user.UserId, "sysdate", user.UserName, user.DeptCode, user.OrganizeCode, 2, assess.Result, assess.HarmType, assess.RiskType, 0, 1, assess.DistrictId, assess.DistrictName, assess.PlanId, assess.AreaCode);
            }
            else
            {
                if (assess.Status == 1)
                {
                    assess.State = 1;
                }
                else
                {
                    assess.State = 0;
                }
                sql = string.Format(@"update BIS_RISKASSESS set dangersource='{1}',deptcode='{2}',deptname='{3}',postid='{4}',postname='{5}',result='{6}',harmtype='{7}',risktype='{8}',districtid='{9}',districtname='{10}',areacode='{11}',State={12},planid='{13}' where id='{0}'", assess.Id, assess.DangerSource, assess.DeptCode, assess.DeptName, assess.PostId, assess.PostName, assess.Result, assess.HarmType, assess.RiskType, assess.DistrictId, assess.DistrictName, assess.AreaCode, assess.State, assess.PlanId);
            }

            return this.BaseRepository().ExecuteBySql(sql);
        }
        /// <summary>
        /// 12.5 获取管控责任单位列表
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetDeptList(ERCHTMS.Code.Operator user)
        {
            string sql = string.Format("select t.departmentid dutyStationId,t.encode DutyStationCode,t.fullname dutyStationName from BASE_DEPARTMENT t");
            string roleNames = user.RoleName;
            if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
            {
                sql += " and deptcode like '" + user.OrganizeCode + "%'";
            }
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 12.9 获取风险辨识岗位列表
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetPostList(string deptCode, ERCHTMS.Code.Operator user)
        {
            string sql = string.Format("select t.roleid jobId,t.fullname jobStr from base_role t where t.category=2 and t.organizeid='{0}'  and t.nature=(select nature from BASE_DEPARTMENT a where a.encode='{1}')", user.OrganizeId, deptCode);
            if (deptCode.Length == 3)
            {
                sql = string.Format("select t.roleid jobId,t.fullname jobStr from base_role t where t.category=2 and t.organizeid='{0}'  and t.nature='厂级'", user.OrganizeId);
            }
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 13.1-13.2 岗位风险卡
        /// <summary>
        /// 13.1-13.2获取本人或全部岗位风险卡列表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode">查询方式，0：本人，1：全部</param>
        /// <returns></returns>
        public List<object> GetPostCardList(ERCHTMS.Code.Operator user, int mode = 0)
        {
            List<object> list = new List<object>();
            string sql = "select distinct deptcode,deptname from BIS_RISKASSESS where status=1 and grade is not null";
            if (mode == 0)
            {
                sql = string.Format("select roleid jobId,fullname jobStr from base_role where roleid in(select t.objectid from BASE_USERRELATION t where userid='{0}' and t.category=3)", user.UserId);
                DataTable dt = this.BaseRepository().FindTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new
                    {
                        jobid = dr[0].ToString(),
                        jobstr = dr[1].ToString(),
                    });
                }
                return list;
            }
            else
            {

                if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
                {
                    sql += " and deptcode like '" + user.OrganizeCode + "%'";
                }
                else
                {
                    sql += " and deptcode like '" + user.DeptCode + "%'";
                }
                DataTable dt = this.BaseRepository().FindTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    DataTable dt1 = this.BaseRepository().FindTable(string.Format("select distinct postid,postname from BIS_RISKASSESS where deptcode='{0}' and status=1 and grade is not null", dr[0].ToString()));
                    StringBuilder sbIds = new StringBuilder();
                    StringBuilder sbNames = new StringBuilder();
                    foreach (DataRow post in dt1.Rows)
                    {
                        list.Add(new
                        {
                            departstr = dr[1].ToString(),
                            departcode = dr[0].ToString(),
                            jobstr = post[1].ToString(),
                            jobid = post[0].ToString()
                        });
                    }


                }
                return list;
            }

        }
        /// <summary>
        /// 13.3 根据岗位Id获取风险详情
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="postId">岗位Id</param>
        /// <returns></returns>
        public List<object> GetPostRiskList(ERCHTMS.Code.Operator user, string postId, string deptCode = "")
        {
            List<object> list = new List<object>();

            DataTable dt = this.BaseRepository().FindTable(string.Format("select distinct postname,deptname from BIS_RISKASSESS where postid='{0}' and deptcode='{1}'", postId, deptCode));
            string deptname = dt.Rows[0][1].ToString(); string postname = dt.Rows[0][0].ToString();
            string sql = string.Format("select id,dangersource,postname,deptname,result from BIS_RISKASSESS where status=1 and postid='{0}'", postId);
            if (!string.IsNullOrEmpty(deptCode))
            {
                sql += " and deptcode='" + deptCode + "'";
            }
            dt = this.BaseRepository().FindTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                StringBuilder sb = new StringBuilder();
                DataTable dtMeasures = this.BaseRepository().FindTable(string.Format("select content from BIS_MEASURES where  riskid='{0}'", dr[0].ToString()));
                int j = 0;
                foreach (DataRow measure in dtMeasures.Rows)
                {
                    sb.AppendFormat("{0}.{1}\r\n", j + 1, measure[0].ToString());
                    j++;
                }
                list.Add(new
                {
                    jobstr = postname,
                    departstr = deptname,
                    riskDetailsList = new
                    {
                        riskdescribe = dr[1].ToString(),
                        riskresult = dr[4].ToString(),
                        riskcontrolmeasure = sb.ToString()
                    }
                });
            }
            return list;
        }
        #endregion

        #region 12.6-12.8 获取编码相关数据
        /// <summary>
        /// 获取危害属性，危害分类等编码
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public DataTable GetCodeList(string itemCode)
        {
            return this.BaseRepository().FindTable(string.Format("select t.itemname harmPropertyName,t.itemvalue harmPropertyId from BASE_DATAITEMDETAIL t where t.itemid=(select itemid from base_dataitem a where a.itemcode='{0}')", itemCode));
        }
        #endregion

        #region 14.2 统计
        /// <summary>
        /// 风险统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object GetStat(ERCHTMS.Code.Operator user)
        {
            string roleNames = user.RoleName;
            string sql = "select grade,count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and grade is not null and deptcode like '" + user.OrganizeCode + "%'";
            if (!(user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户")))
            {
                sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
            }
            sql += " group by grade";
            List<object> list = new List<object>();
            DataTable dt = this.BaseRepository().FindTable(sql);

            int count1 = dt.Select("grade='一级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='一级'")[0][1].ToString());
            int count2 = dt.Select("grade='二级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='二级'")[0][1].ToString());
            int count3 = dt.Select("grade='三级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='三级'")[0][1].ToString());
            int count4 = dt.Select("grade='四级'").Length == 0 ? 0 : int.Parse(dt.Select("grade='四级'")[0][1].ToString());
            int sum = count1 + count2 + count3 + count4;
            decimal percent = sum == 0 ? 0 : decimal.Parse(count1.ToString()) / decimal.Parse(sum.ToString());
            list.Add(new { risklevel = "一级", problemNum = count1, problemrate = Math.Round(percent, 4) });
            percent = sum == 0 ? 0 : decimal.Parse(count2.ToString()) / decimal.Parse(sum.ToString());
            list.Add(new { risklevel = "二级", problemNum = count2, problemrate = Math.Round(percent, 4) });
            percent = sum == 0 ? 0 : decimal.Parse(count3.ToString()) / decimal.Parse(sum.ToString());
            list.Add(new { risklevel = "三级", problemNum = count3, problemrate = Math.Round(percent, 4) });
            percent = sum == 0 ? 0 : decimal.Parse(count4.ToString()) / decimal.Parse(sum.ToString());
            list.Add(new { risklevel = "四级", problemNum = count4, problemrate = Math.Round(percent, 4) });

            return new { risktotalnum = sum, riskList = list };
        }
        #endregion
        #endregion
    }
}
