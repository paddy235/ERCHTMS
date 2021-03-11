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
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.SystemManage;
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
            //风险类别
            if (!queryParam["riskType"].IsEmpty())
            {
                string riskType = queryParam["riskType"].ToString();
                pagination.conditionJson += string.Format(" and risktype ='{0}'", riskType);
            }
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
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
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
            //查询关键字
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString();
                pagination.conditionJson += string.Format(" and (Description like '%{0}%' or DangerSource like '%{0}%' or riskdesc like '%{0}%' or result like'%{0}%') ", keyWord.Trim());
            }
            if (!queryParam["initAreaId"].IsEmpty())
            {
                string initAreaId = queryParam["initAreaId"].ToString().TrimEnd(',');
                pagination.conditionJson += string.Format(" or areaId in('{0}')", initAreaId.Replace(",", "','"));
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 风险清单导出
        /// </summary>
        /// <param name="queryJson">查询条件</param>
        /// <param name="riskType">风险类型</param>
        /// <param name="authType">授权范围</param>
        /// <param name="IndexState">是否首页跳转</param>
        /// <returns></returns>
        public DataTable GetPageExportList(string queryJson, string riskType,string authType,string IndexState)
        {
            var queryParam = queryJson.ToJObject();
            string sql = string.Empty;
            string sqlWhere =" where 1=1  and b.status = 1 and b.deletemark = 0";
            string groupby = string.Empty;

            //风险类别
            if (!(queryParam["riskType"].IsEmpty()) || !(riskType.IsEmpty()))
            {

                sqlWhere += string.Format(" and b.risktype ='{0}'", riskType);
            }
            //状态
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                sqlWhere += string.Format(" and b.status ={0}", status);
            }

            //国电荥阳版本标示
            var IsGdxy = new DataItemDetailService().GetDataItemListByItemCode("'VManager'").ToList() ;
            if (!string.IsNullOrWhiteSpace(riskType))
            {
                if (IsGdxy.Count>0)
                {
                    switch (riskType)
                    {
                        case "作业":
                            sql += @"select rownum,b.risktype,b.worktask,b.districtname,b.process,b.project,b.dangersource,b.riskdesc,
                                                                        b.result,b.way,b.itema,b.itemb,b.itemc,b.itemr,gcjs,gl,gtfh,pxjy,yj,b.grade,b.levelname,b.deptname,b.postname,cast(b.dutyperson as varchar2(3000)) ";

                            break;
                        case "设备":
                            sql += @"select rownum,b.risktype,b.element,b.equipmentname,b.districtname,b.faultcategory,b.faulttype,b.parts,b.riskdesc,
                                                                          b.result,b.way,b.itema,b.itemb,b.itemc,b.itemr,gcjs,gl,gtfh,pxjy,yj,b.grade,b.levelname,b.deptname,b.postname,cast(b.dutyperson as varchar2(3000)) ";

                            break;
                        case "区域":
                            sql += @"select rownum,b.risktype,b.districtname,b.hjsystem,b.hjequpment,b.dangersource,b.riskdesc,
                                                                         b.result,b.way,b.itema,b.itemb,b.itemc,b.itemr,gcjs,gl,gtfh,pxjy,yj,b.grade,b.levelname,b.deptname,b.postname,cast(b.dutyperson as varchar2(3000))";

                            break;
                        case "管理":
                            sql += @"select rownum,b.risktype,b.districtname,b.majornametype,b.majorname,b.dangersourcetype,b.dangersource,b.riskdesc,
                                                                         b.result,b.way,b.itema,b.itemb,b.itemc,b.itemr,gcjs,gl,gtfh,pxjy,yj,b.grade,b.levelname,b.deptname,b.postname,cast(b.dutyperson as varchar2(3000))  ";

                            break;
                        case "职业病危害":
                            sql += @"select rownum,b.risktype,b.districtname,b.majorname,b.description,b.harmtype,b.harmproperty,
                                                                        b.way,b.itema,b.itemb,b.itemc,b.itemr,gcjs,gl,gtfh,pxjy,yj,b.grade,b.levelname,b.deptname,b.postname ";

                            break;
                        case "岗位":
                            sql += @"select rownum,b.risktype,b.districtname,b.postdept,b.jobname,b.postperson,b.dangersourcetype,b.dangersource,b.riskdesc,b.result,
                                                                        b.way,b.itema,b.itemb,b.itemc,b.itemr,gcjs,gl,gtfh,pxjy,yj,b.grade,b.levelname,b.deptname,b.postname,cast(b.dutyperson as varchar2(3000)) ";

                            break;
                        case "工器具及危化品":
                            sql += @"select rownum,b.risktype,b.districtname,b.toolordanger,b.packuntil,b.packnum,b.storagespace,b.dangersourcetype,b.dangersource,b.riskdesc,b.result,
                                                                        b.way,b.itema,b.itemb,b.itemc,b.itemr,gcjs,gl,gtfh,pxjy,yj,b.grade,b.levelname,b.deptname,b.postname,cast(b.dutyperson as varchar2(3000)) ";
                            break;
                        default:
                            break;
                    }
                }
                else {
                    switch (riskType)
                    {
                        case "作业":
                            sql += @"select rownum,b.risktype,b.districtname,b.worktask,b.process,b.dangersource,b.riskdesc,
                                                                        b.result,b.way,b.itema,b.itemb,b.itemc,b.itemr,b.grade,gcjs,gl,gtfh,pxjy,yj,b.levelname,b.deptname,b.postname ";

                            break;
                        case "设备":
                            sql += @"select rownum,b.risktype,b.districtname,b.equipmentname,b.parts,b.faulttype,b.riskdesc,
                                                                          b.result,b.way,b.itema,b.itemb,b.itemc,b.itemr,b.grade,gcjs,gl,gtfh,pxjy,yj,b.levelname,b.deptname,b.postname ";

                            break;
                        case "区域":
                            sql += @"select rownum,b.risktype,b.districtname,b.hjsystem,b.hjequpment,b.dangersource,b.riskdesc,
                                                                         b.result,b.way,b.itema,b.itemb,b.itemc,b.itemr,b.grade,gcjs,gl,gtfh,pxjy,yj,b.levelname,b.deptname,b.postname ";

                            break;
                        case "管理":
                            sql += @"select rownum,b.risktype,b.districtname,b.dangersource,b.riskdesc,
                                                                         b.result,b.way,b.itema,b.itemb,b.itemc,b.itemr,b.grade,gcjs,gl,gtfh,pxjy,yj,b.levelname,b.deptname,b.postname ";

                            break;
                        case "职业病危害":
                            sql += @"select rownum,b.risktype,b.districtname,b.majorname,b.description,b.harmtype,b.harmproperty,
                                                                        b.way,b.itema,b.itemb,b.itemc,b.itemr,b.grade,gcjs,gl,gtfh,pxjy,yj,b.levelname,b.deptname,b.postname ";

                            break;
                        case "岗位":
                            sql += @"select rownum,b.risktype,b.districtname,b.jobname,b.dangersourcetype,b.dangersource,b.riskdesc,b.result,
                                                                        b.way,b.itema,b.itemb,b.itemc,b.itemr,b.grade,gcjs,gl,gtfh,pxjy,yj,b.levelname,b.deptname,b.postname ";

                            break;
                        case "工器具及危化品":
                            sql += @"select rownum,b.risktype,b.districtname,b.toolordanger,b.dangersourcetype,b.dangersource,b.riskdesc,b.result,
                                                                        b.way,b.itema,b.itemb,b.itemc,b.itemr,b.grade,gcjs,gl,gtfh,pxjy,yj,b.levelname,b.deptname,b.postname ";

                            break;
                        default:
                            break;
                    }
                }
            }
            sql += " from bis_riskassess b left join ({0})m on m.riskid=b.id ";
            groupby = @"V_riskdata";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //首页跳转
            if (!string.IsNullOrWhiteSpace(IndexState))
            {
                if (!string.IsNullOrEmpty(authType))
                {

                    switch (authType)
                    {
                        case "1":
                            sqlWhere += " and createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            sqlWhere += " and deptcode='" + user.DeptCode + "'";
                            break;
                        case "3":
                            sqlWhere += " and deptcode like '" + user.DeptCode + "%'";
                            break;
                        case "4":
                            sqlWhere += " and deptcode like '" + user.OrganizeCode + "%'";
                            break;
                        case "5":
                            sqlWhere += string.Format(" and deptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", user.NewDeptCode);
                            break;
                    }
                }
                else
                {
                    sqlWhere += " and 0=1";
                }
            }
            else {
                if (user.RoleName.Contains("省级"))
                {
                    if (!string.IsNullOrEmpty(authType))
                    {

                        switch (authType)
                        {
                            case "1":
                                sqlWhere += " and createuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                sqlWhere += " and deptcode='" + user.DeptCode + "'";
                                break;
                            case "3":
                                sqlWhere += " and deptcode like '" + user.DeptCode + "%'";
                                break;
                            case "4":
                                sqlWhere += " and deptcode like '" + user.OrganizeCode + "%'";
                                break;
                            case "5":
                                sqlWhere += string.Format(" and deptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", user.NewDeptCode);
                                break;
                        }
                    }
                    else
                    {
                        sqlWhere += " and 0=1";
                    }
                }
                else {
                    sqlWhere += " and deptcode like '" + user.OrganizeCode + "%'";
                }
            }
          
            //区域Code
            if (!queryParam["areaCode"].IsEmpty())
            {
                string areaCode = queryParam["areaCode"].ToString();
                sqlWhere += string.Format(" and b.areaCode like '{0}%'", areaCode);
            }
            //区域ID
            if (!queryParam["areaId"].IsEmpty())
            {
                string areaId = queryParam["areaId"].ToString();
                sqlWhere += string.Format(" and b.areaId = '{0}'", areaId);
            }
            //风险等级
            if (!queryParam["grade"].IsEmpty())
            {
                string grade = queryParam["grade"].ToString();
                sqlWhere += string.Format(" and b.grade = '{0}'", grade);
            }
            //事故类型
            if (!queryParam["accType"].IsEmpty())
            {
                string accType = queryParam["accType"].ToString();
                sqlWhere += string.Format(" and b.AccidentName like '%{0}%'", accType);
            }
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
                            sqlWhere += string.Format(" and deptCode like '{0}%'", deptCode);
                        }
                    }
                }
                else
                {

                    sqlWhere += string.Format(" and deptCode like '{0}%'", deptCode);
                }
            }
            //查询关键字
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString();
                sqlWhere += string.Format(" and (Description like '%{0}%' or riskdesc like '%{0}%' or majorname like '%{0}%' or worktask like '%{0}%' or equipmentname like '%{0}%' or dangersource like '%{0}%') ", keyWord.Trim());
            }
            if (!queryParam["initAreaId"].IsEmpty())
            {
                string initAreaId = queryParam["initAreaId"].ToString().TrimEnd(',');
                sqlWhere += string.Format(" or b.areaId in('{0}')", initAreaId.Replace(",", "','"));
            }
            sql = string.Format(sql + sqlWhere, groupby);
            return this.BaseRepository().FindTable(sql);
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
        /// 首页风险工作指标统计
        /// </summary>
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        public decimal[] GetHomeStat(ERCHTMS.Code.Operator user)
        {
            string roleNames = user.RoleName;
            //string sql = "select grade,count(1) from BIS_RISKASSESS where  status=1 and deletemark=0 and  to_char(createdate,'yyyy')='" + DateTime.Now.Year + "'";
            string sql = "select grade,count(1) from BIS_RISKASSESS where  status=1 and deletemark=0 ";
            if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
            {
                sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
            }
            else
            {
                sql += string.Format(" and deptcode like '" + user.OrganizeCode + "%'");
            }
            sql += "  group by grade";
            DataTable dt = this.BaseRepository().FindTable(sql);

            int count1 = dt.Select("grade='重大风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='重大风险'")[0][1].ToString());
            int count2 = dt.Select("grade='较大风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='较大风险'")[0][1].ToString());
            int count3 = dt.Select("grade='一般风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='一般风险'")[0][1].ToString());
            int count4 = dt.Select("grade='低风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='低风险'")[0][1].ToString());

            //计算本年度风险数量
            sql = "select count(1) from BIS_RISKASSESS where  status=1 and deletemark=0 and to_char(createdate,'yyyy')='" + DateTime.Now.Year + "'";
            if (DbHelper.DbType == DatabaseType.MySql)
            {
                sql = "select count(1) from BIS_RISKASSESS where  status=1 and deletemark=0 and date_format(createdate,'%Y')='" + DateTime.Now.Year + "'";
            }
            if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
            {
                sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
            }
            else
            {
                sql += string.Format(" and deptcode like '" + user.OrganizeCode + "%'");
            }
            int sum = this.BaseRepository().FindObject(sql).ToInt();

            //计算去年风险数量
            sql = "select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and to_char(createdate,'yyyy')='" + (DateTime.Now.Year - 1) + "'";
            if (DbHelper.DbType == DatabaseType.MySql)
            {
                sql = "select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and date_format(createdate,'%Y')='" + DateTime.Now.Year + "'";
            }
            if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
            {
                sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
            }
            else
            {
                sql += string.Format(" and deptcode like '" + user.OrganizeCode + "%'");
            }
            int total = this.BaseRepository().FindObject(sql).ToInt();
            decimal percent = sum == 0 || total == 0 ? 0 : (decimal.Parse(sum.ToString()) / decimal.Parse(total.ToString())) - 1;
            percent = percent * 100;
            decimal[] arr = new decimal[] { (count1 + count2 + count3 + count4), count1, count2, count3, count4, Math.Round(percent, 2) };
            return arr;
            //return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 首页风险排名
        /// </summary>
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        public string GetRiskRank(ERCHTMS.Code.Operator user)
        {
            string roleNames = user.RoleName;
            string sql = string.Format("select * from (select id,t.itemr,t.risktype,t.deptname,t.deptcode,grade,riskdesc from BIS_RISKASSESS t where status=1 and deletemark=0 and deptcode like '{0}%'", user.OrganizeCode);
            sql += "  and t.itemr is not null  order by t.itemr desc,id) t where rownum<=10";
            if (DbHelper.DbType == DatabaseType.SqlServer)
            {
                sql = string.Format("select  top 10 id,t.itemr,t.risktype,t.deptname,t.deptcode,grade,riskdesc from BIS_RISKASSESS t where status=1 and deletemark=0 and deptcode like '{0}%' and t.itemr is not null order by t.itemr desc,id ", user.OrganizeCode);
            }
            if (DbHelper.DbType == DatabaseType.MySql)
            {
                sql = string.Format("select id,t.itemr,t.risktype,t.deptname,t.deptcode,grade,riskdesc from BIS_RISKASSESS t where status=1 and deletemark=0 and deptcode like '{0}%' and t.itemr is not null order by t.itemr desc,id asc limit 0,10", user.OrganizeCode);
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// 首页风险预警
        /// </summary>    
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        public object GetRiskWarn(ERCHTMS.Code.Operator user, string month = "")
        {
            string orgId = user.OrganizeId;//用户所属机构
            string roleNames = user.RoleName;//用户角色
            string sql = string.Format("select count(1) from BIS_CLASSIFICATIONINDEX where affiliatedorganizeid='{0}'", orgId);
            int count = this.BaseRepository().FindObject(sql).ToInt();
            sql = string.Format("select t.indexname name,t.indexscore totalscore,t.indexstandardformat descr,t.indexargsvalue val,0 score,0 finalscore,indexcode from  BIS_CLASSIFICATIONINDEX t where t.classificationcode='03'");
            if (count == 0)
            {
                sql += " and affiliatedorganizeid='0'";
            }
            else
            {
                sql += " and affiliatedorganizeid='" + orgId + "'";
            }
            sql += " order by indexcode asc";
            decimal allScore = 0;//应得总分
            decimal finalScore = 0;//实际总得分
            decimal weight = 0;
            object obj = this.BaseRepository().FindObject("select t.weightcoeffcient from BIS_CLASSIFICATION t where t.affiliatedorganizeid=@orgId", new DbParameter[] { DbParameters.CreateDbParameter("@orgId", orgId) });
            if (obj != null && obj != DBNull.Value)
            {
                weight = obj.ToDecimal();
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                decimal totalScore = decimal.Parse(dr["totalscore"].ToString());
                allScore += totalScore;
                //构造评分标准
                dr["descr"] = string.Format(dr["descr"].ToString(), dr["val"].ToString().Split('|'));
                //计算安全风险辨识、评估计划扣分和得分 
                if (dr["indexcode"].ToString() == "01")
                {
                    if (string.IsNullOrEmpty(month))
                    {
                        sql = string.Format("select count(1) from BIS_RISKPLAN t where to_char(t.startdate,'yyyy')='{0}'", DateTime.Now.Year);
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            string.Format("select count(1) from BIS_RISKPLAN t where date_format(t.startdate,'%Y')='{0}'", DateTime.Now.Year);
                        }
                    }
                    else
                    {
                        sql = string.Format("select count(1) from BIS_RISKPLAN t where to_char(t.startdate,'yyyy-mm-dd hh24:mi:ss')<'{1}' and to_char(t.startdate,'yyyy')='{0}'", DateTime.Parse(month).Year, DateTime.Parse(month).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sql = string.Format("select count(1) from BIS_RISKPLAN t where t.startdate<'{1}' and date_format(t.startdate,'%Y')='{0}'", DateTime.Parse(month).Year, DateTime.Parse(month).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
                        }
                    }
                    //if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
                    //{
                    //    sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
                    //}
                    //else
                    //{
                    sql += string.Format(" and deptcode like '" + user.OrganizeCode + "%'");
                    //}
                    count = this.BaseRepository().FindObject(sql).ToInt();
                    if (count < int.Parse(dr["val"].ToString()))
                    {
                        dr["score"] = dr["totalscore"];
                        dr["finalscore"] = 0;
                    }
                    else
                    {
                        dr["score"] = 0;
                        dr["finalscore"] = dr["totalscore"];
                    }
                }
                else if (dr["indexcode"].ToString() == "02")
                {
                    if (string.IsNullOrEmpty(month))
                    {
                        sql = string.Format("select enddate,modifydate,status from BIS_RISKPLAN t where to_char(t.startdate,'yyyy')='{0}'", DateTime.Now.Year);
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sql = string.Format("select enddate,modifydate,status from BIS_RISKPLAN t where date_format(t.startdate,'%Y')='{0}'", DateTime.Now.Year);
                        }
                    }
                    else
                    {
                        sql = string.Format("select enddate,modifydate,status from BIS_RISKPLAN t where to_char(startdate,'yyyy-mm-dd hh24:mi:ss') between '{0}' and  '{1}'", DateTime.Parse(month).ToString("yyyy-01-01 00:00:00"), DateTime.Parse(month).AddMonths(1).ToString("yyyy-MM-dd 00:00:00"));
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sql = string.Format("select enddate,modifydate,status from BIS_RISKPLAN t where startdate between '{0}' and  '{1}'", DateTime.Parse(month).ToString("yyyy-01-01 00:00:00"), DateTime.Parse(month).AddMonths(1).ToString("yyyy-MM-dd 00:00:00"));
                        }
                    }

                    //if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
                    //{
                    //    sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
                    //}
                    //else
                    //{
                    sql += string.Format(" and deptcode like '" + user.OrganizeCode + "%'");
                    //}
                    int score = 0;
                    DataTable dtPlans = this.BaseRepository().FindTable(sql);
                    foreach (DataRow dr1 in dtPlans.Rows)
                    {
                        DateTime endDate = DateTime.Parse(dr1["enddate"].ToString()).AddDays(1);

                        if (dr1["status"].ToString() == "1")
                        {
                            DateTime time = DateTime.Parse(dr1["modifydate"].ToString());
                            if (endDate < time)
                            {
                                if (endDate < time.AddDays(10))
                                {
                                    score += 5;
                                }
                                else
                                {
                                    score += int.Parse(dr["totalscore"].ToString());
                                }
                            }
                        }
                        else
                        {
                            if (endDate < DateTime.Now)
                            {
                                score += int.Parse(dr["totalscore"].ToString());
                            }

                        }
                    }
                    if (score >= int.Parse(dr["totalscore"].ToString()))
                    {
                        score = int.Parse(dr["totalscore"].ToString());

                    }
                    dr["score"] = score;
                    dr["finalscore"] = int.Parse(dr["totalscore"].ToString()) - score;
                }
                else
                {
                    sql = string.Format("select count(1) from BIS_RISKASSESS t where t.status=1 and t.deletemark=0 and t.enabledmark=0 and grade is not null ");
                    if (!string.IsNullOrEmpty(month))
                    {
                        DateTime dtMonth = DateTime.Parse(month);

                        if (DbHelper.DbType == DatabaseType.Oracle)
                        {
                            sql += " and to_char(createdate,'yyyy-mm-dd hh24:mi:ss')<'" + dtMonth.AddMonths(1).ToString("yyyy-MM-01 00:00:00") + "'";
                        }
                        else
                        {
                            sql += " and  createdate<'" + dtMonth.AddMonths(1).ToString("yyyy-MM-01 00:00:00") + "'";
                        }
                    }
                    //if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
                    //{
                    //    sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
                    //}
                    //else
                    //{
                    sql += string.Format(" and deptcode like '" + user.OrganizeCode + "%'");
                    //}
                    int gradeval = 1;
                    if (dr["indexcode"].ToString() == "03")
                    {
                        gradeval = 1;

                    }
                    else if (dr["indexcode"].ToString() == "04")
                    {
                        gradeval = 2;
                    }
                    else
                    {
                        gradeval = 3;
                    }
                    decimal sum = this.BaseRepository().FindObject(sql).ToDecimal();
                    if (sum > 0)
                    {
                        sql += " and gradeval=" + gradeval;
                        count = this.BaseRepository().FindObject(sql).ToInt();
                        decimal precent = sum == 0 ? 0 : int.Parse(count.ToString()) * 100 / sum;
                        string[] arr = dr["val"].ToString().Split('|');
                        if (precent > decimal.Parse(arr[0]))
                        {
                            decimal s = precent - decimal.Parse(arr[0]);
                            s = s >= 0 && s < decimal.Parse(arr[1]) ? 0 : s;
                            decimal score = arr[1] == "0" ? 0 : Math.Floor(s / decimal.Parse(arr[1])) * decimal.Parse(arr[2]);
                            score = score >= totalScore ? totalScore : score;
                            dr["score"] = score;
                            dr["finalscore"] = decimal.Parse(dr["totalscore"].ToString()) - score;
                        }
                        else
                        {
                            dr["score"] = 0;
                            dr["finalscore"] = dr["totalscore"];
                        }
                    }
                    else
                    {
                        dr["score"] = 0;
                        dr["finalscore"] = dr["totalscore"];
                    }
                }
                finalScore += decimal.Parse(dr["finalscore"].ToString());
            }
            return new { data = dt, allScore = allScore, finalScore = finalScore, weight = weight };
        }
        /// <summary>
        /// 计算最近半年的风险预警值
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetRiskValues(ERCHTMS.Code.Operator user)
        {
            //构造y轴数据
            List<decimal> list = new List<decimal>();
            //x轴时间坐标值
            List<string> xValues = new List<string>();
            //计算最近半年的得分值
            for (int j = -6; j < 0; j++)
            {
                //开始时间
                string startDate = DateTime.Now.AddMonths(j).ToString("yyyy-MM-01 00:00:00");
                //结束时间
                string endDate = DateTime.Parse(startDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
                xValues.Add(DateTime.Parse(startDate).ToString("yyyy.MM"));

                string orgId = user.OrganizeId;
                string roleNames = user.RoleName;
                //查询各项考核指标参数值
                string sql = string.Format("select count(1) from BIS_CLASSIFICATIONINDEX where affiliatedorganizeid='{0}' and classificationcode='03'", orgId);
                int count = this.BaseRepository().FindObject(sql).ToInt();
                sql = string.Format("select t.indexscore totalscore,t.indexstandardformat descr,t.indexargsvalue val,indexcode from  BIS_CLASSIFICATIONINDEX t where t.classificationcode='03'");
                //如果当前电厂没有设置指标则使用内置默认的
                if (count == 0)
                {
                    sql += " and affiliatedorganizeid='0'";
                }
                else
                {
                    sql += " and affiliatedorganizeid='" + orgId + "'";
                }
                DataTable dt = this.BaseRepository().FindTable(sql);

                decimal totalScore = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    decimal sumScore = decimal.Parse(dr["totalscore"].ToString());
                    //计算安全风险辨识、评估计划扣分和得分 
                    if (dr["indexcode"].ToString() == "01")
                    {
                        sql = string.Format("select count(1) from BIS_RISKPLAN t where to_char(startdate,'yyyy')='{0}' and to_char(startdate,'yyyy-mm-dd hh24:mi:ss')<'{1}'", DateTime.Parse(endDate).Year, DateTime.Parse(endDate).ToString("yyyy-MM-dd 23:59:59"));
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sql = string.Format("select count(1) from BIS_RISKPLAN t where date_format(startdate,'%Y')='{0}' and startdate<'{1}'", DateTime.Parse(endDate).Year, DateTime.Parse(endDate).ToString("yyyy-MM-dd 23:59:59"));
                        }
                        //if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
                        //{
                        //    sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
                        //}
                        //else
                        //{
                        sql += string.Format(" and deptcode like '" + user.OrganizeCode + "%'");
                        //}
                        count = this.BaseRepository().FindObject(sql).ToInt();
                        if (count >= int.Parse(dr["val"].ToString()))
                        {
                            totalScore += decimal.Parse(dr["totalscore"].ToString());
                        }
                    }
                    //安全风险辨识、评估计划执行效果
                    else if (dr["indexcode"].ToString() == "02")
                    {
                        sql = string.Format("select enddate,modifydate,status from BIS_RISKPLAN t where to_char(startdate,'yyyy-mm-dd hh24:mi:ss') between '{0}' and  '{1}'", DateTime.Parse(endDate).ToString("yyyy-01-01 00:00:00"), endDate);
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sql = string.Format("select enddate,modifydate,status from BIS_RISKPLAN t where startdate between '{0}' and  '{1}'", DateTime.Parse(endDate).ToString("yyyy-01-01 00:00:00"), endDate);
                        }
                        //if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
                        //{
                        //    sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
                        //}
                        //else
                        //{
                        sql += string.Format(" and deptcode like '" + user.OrganizeCode + "%'");
                        //}
                        int score = 0;
                        DataTable dtPlans = this.BaseRepository().FindTable(sql);
                        foreach (DataRow dr1 in dtPlans.Rows)
                        {
                            //计划结束时间
                            DateTime endDate1 = DateTime.Parse(dr1["enddate"].ToString()).AddDays(1);
                            //如果计划已经设置完成
                            if (dr1["status"].ToString() == "1")
                            {
                                //计划完成时的时间
                                DateTime time = DateTime.Parse(dr1["modifydate"].ToString());
                                //判断是否超期完成
                                if (endDate1 < time)
                                {
                                    if (endDate1 < time.AddDays(10))
                                    {
                                        score += 5;
                                    }
                                    else
                                    {
                                        score += int.Parse(dr["totalscore"].ToString());
                                    }
                                }
                            }
                            //超期还未完成的
                            else
                            {
                                if (endDate1 < DateTime.Parse(endDate))
                                {
                                    score += int.Parse(dr["totalscore"].ToString());
                                }

                            }
                        }
                        if (score >= int.Parse(dr["totalscore"].ToString()))
                        {
                            score = int.Parse(dr["totalscore"].ToString());

                        }
                        totalScore += decimal.Parse(dr["totalscore"].ToString()) - score;
                    }
                    else
                    {
                        //按风险等级计算分值
                        sql = string.Format("select count(1) from BIS_RISKASSESS t where t.status=1 and t.deletemark=0 and t.enabledmark=0 and to_char(createdate,'yyyy-mm-dd hh24:mi:ss')<'" + endDate + "' and grade is not null ");
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sql = string.Format("select count(1) from BIS_RISKASSESS t where t.status=1 and t.deletemark=0 and t.enabledmark=0 and createdate<'" + endDate + "' and grade is not null ");
                        }
                        //if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
                        //{
                        //    sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
                        //}
                        //else
                        //{
                        sql += string.Format(" and deptcode like '" + user.OrganizeCode + "%'");
                        //}
                        int gradeval = 1;
                        //一级风险
                        if (dr["indexcode"].ToString() == "03")
                        {
                            gradeval = 1;

                        }
                        //二级风险
                        else if (dr["indexcode"].ToString() == "04")
                        {
                            gradeval = 2;
                        }
                        else
                        {
                            gradeval = 3;//三级风险
                        }
                        decimal sum = this.BaseRepository().FindObject(sql).ToDecimal();
                        if (sum > 0)
                        {
                            sql += " and gradeval=" + gradeval;
                            count = this.BaseRepository().FindObject(sql).ToInt();
                            decimal precent = sum == 0 ? 0 : decimal.Parse(count.ToString()) * 100 / sum;
                            string[] arr = dr["val"].ToString().Split('|');
                            if (precent > decimal.Parse(arr[0]))
                            {
                                decimal s = precent - decimal.Parse(arr[0]);
                                s = s >= 0 && s < decimal.Parse(arr[1]) ? 0 : s;
                                decimal score = arr[1] == "0" ? 0 : Math.Floor(s / decimal.Parse(arr[1])) * decimal.Parse(arr[2]);
                                score = score >= sumScore ? sumScore : score;
                                totalScore += decimal.Parse(dr["totalscore"].ToString()) - score;
                            }
                            else
                            {
                                totalScore += decimal.Parse(dr["totalscore"].ToString());
                            }
                        }
                        else
                        {
                            totalScore += decimal.Parse(dr["totalscore"].ToString());
                        }
                    }
                }
                dt.Dispose();
                list.Add(Math.Round(totalScore, 1));

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = xValues, y = list });
        }
        /// <summary>
        /// 根据月份计算当月得分
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">月份，如2017-10-01</param>
        /// <returns></returns>
        public decimal GetRiskValueByTime(ERCHTMS.Code.Operator user, string time = "")
        {
            //统计开始时间
            string startDate = string.IsNullOrEmpty(time) ? DateTime.Now.ToString("yyyy-01-01 00:00:00") : DateTime.Parse(time).ToString("yyyy-01-01 00:00:00");
            //统计结束时间
            string endDate = string.IsNullOrEmpty(time) ? DateTime.Now.ToString("yyyy-MM-dd 23:59:59") : DateTime.Parse(time).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string orgId = user.OrganizeId;
            string orgCode = user.OrganizeCode;
            string roleNames = user.RoleName;

            string sql = string.Format("select count(1) from BIS_CLASSIFICATIONINDEX where affiliatedorganizeid=@orgId");
            int count = this.BaseRepository().FindObject(sql, new DbParameter[] { DbParameters.CreateDbParameter("@orgId", orgId) }).ToInt();//查询当前电厂是否有设置考核指标
            sql = string.Format("select t.indexscore totalscore,t.indexstandardformat descr,t.indexargsvalue val,indexcode from  BIS_CLASSIFICATIONINDEX t where t.classificationcode='03'");
            //没有的话就使用内置默认的数据
            if (count == 0)
            {
                sql += " and affiliatedorganizeid='0'";
            }
            else
            {
                sql += " and affiliatedorganizeid=@orgId";
            }
            //获取各项设置的指标值
            DataTable dt = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@orgId", orgId) });
            //总得分
            decimal totalScore = 0;
            foreach (DataRow dr in dt.Rows)
            {
                decimal sumScore = decimal.Parse(dr["totalscore"].ToString());
                //小项编码值，计算安全风险辨识、评估计划扣分和得分 
                if (dr["indexcode"].ToString() == "01")
                {
                    sql = string.Format("select count(1) from BIS_RISKPLAN t where to_char(startdate,'yyyy-mm-dd hh24:mi:ss') between @startDate and @endDate");
                    if (DbHelper.DbType == DatabaseType.MySql)
                    {
                        sql = string.Format("select count(1) from BIS_RISKPLAN t where startdate between @startDate and @endDate");
                    }
                    sql += string.Format(" and deptcode like @orgCode");
                    count = this.BaseRepository().FindObject(sql, new DbParameter[] { DbParameters.CreateDbParameter("@startDate", startDate), DbParameters.CreateDbParameter("@endDate", endDate), DbParameters.CreateDbParameter("@orgCode", orgCode + "%") }).ToInt();
                    if (count >= int.Parse(dr["val"].ToString()))
                    {
                        totalScore += decimal.Parse(dr["totalscore"].ToString());
                    }
                }
                //安全风险辨识、评估计划执行效果
                else if (dr["indexcode"].ToString() == "02")
                {
                    sql = string.Format("select enddate,modifydate,status from BIS_RISKPLAN t where to_char(startdate,'yyyy-mm-dd hh24:mi:ss') between @startDate and @endDate", startDate, endDate);
                    if (DbHelper.DbType == DatabaseType.MySql)
                    {
                        sql = string.Format("select enddate,modifydate,status from BIS_RISKPLAN t where startdate between @startDate and @endDate", startDate, endDate);
                    }
                    sql += string.Format(" and deptcode like  @orgCode");
                    int score = 0;
                    DataTable dtPlans = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@startDate", startDate), DbParameters.CreateDbParameter("@endDate", endDate), DbParameters.CreateDbParameter("@orgCode", orgCode + "%") });
                    foreach (DataRow dr1 in dtPlans.Rows)
                    {
                        DateTime endDate1 = DateTime.Parse(dr1["enddate"].ToString()).AddDays(1);

                        if (dr1["status"].ToString() == "1")
                        {
                            DateTime modifyDate = DateTime.Parse(dr1["modifydate"].ToString());
                            if (endDate1 < modifyDate)
                            {
                                if (endDate1 < modifyDate.AddDays(10))
                                {
                                    score += 5;
                                }
                                else
                                {
                                    score += int.Parse(dr["totalscore"].ToString());
                                }
                            }
                        }
                        else
                        {
                            if (endDate1 < DateTime.Parse(endDate))
                            {
                                score += int.Parse(dr["totalscore"].ToString());
                            }

                        }
                    }
                    if (score >= int.Parse(dr["totalscore"].ToString()))
                    {
                        score = int.Parse(dr["totalscore"].ToString());

                    }
                    totalScore += decimal.Parse(dr["totalscore"].ToString()) - score;
                }
                else
                {
                    sql = string.Format("select count(1) from BIS_RISKASSESS t where t.status=1 and t.deletemark=0 and t.enabledmark=0 and to_char(createdate,'yyyy-mm-dd hh24:mi:ss')<'" + endDate + "' and grade is not null ");
                    if (DbHelper.DbType == DatabaseType.MySql)
                    {
                        sql = string.Format("select count(1) from BIS_RISKASSESS t where t.status=1 and t.deletemark=0 and t.enabledmark=0 and createdate<'" + endDate + "' and grade is not null ");
                    }
                    sql += string.Format(" and deptcode like @orgCode");
                    int gradeval = 1;
                    //风险等级为一级
                    if (dr["indexcode"].ToString() == "03")
                    {
                        gradeval = 1;

                    }
                    //风险等级为二级
                    else if (dr["indexcode"].ToString() == "04")
                    {
                        gradeval = 2;
                    }
                    //风险等级三级
                    else
                    {
                        gradeval = 3;
                    }
                    decimal sum = this.BaseRepository().FindObject(sql, new DbParameter[] { DbParameters.CreateDbParameter("@orgCode", orgCode + "%") }).ToDecimal();
                    if (sum > 0)
                    {
                        sql += " and gradeval=@gradeval";
                        count = this.BaseRepository().FindObject(sql, new DbParameter[] { DbParameters.CreateDbParameter("@orgCode", orgCode + "%"), DbParameters.CreateDbParameter("@gradeval", gradeval) }).ToInt();
                        decimal precent = sum == 0 ? 0 : decimal.Parse(count.ToString()) * 100 / sum;
                        string[] arr = dr["val"].ToString().Split('|');
                        if (precent > decimal.Parse(arr[0]))
                        {
                            decimal score = arr[1] == "0" ? 0 : Math.Floor(((precent - decimal.Parse(arr[0])) / decimal.Parse(arr[1]))) * decimal.Parse(arr[2]);
                            score = score >= sumScore ? sumScore : score;
                            totalScore += decimal.Parse(dr["totalscore"].ToString()) - score;
                        }
                        else
                        {
                            totalScore += decimal.Parse(dr["totalscore"].ToString());
                        }
                    }
                    else
                    {
                        totalScore += decimal.Parse(dr["totalscore"].ToString());
                    }
                }
            }
            dt.Dispose();
            return totalScore;
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
        public DataTable GetAreaList(int type, string areaId, string grade, ERCHTMS.Code.Operator user, string kind, string deptcode, string keyWord = "")
        {
            string sql = @"select identity,identityName,sum(identityNum) as identityNum from( select t.districtid identity,t.districtname identityName,nvl(a.identityNum,0) identityNum  from BIS_DISTRICT t 
                                        right join (select districtname identityName,districtid,count(1) identityNum from BIS_RISKASSESS 
                                        where status=1 and deletemark=0 ";
            if (!string.IsNullOrEmpty(kind))
            {

                sql += " and risktype='" + kind + "'";

            }
            if (!string.IsNullOrEmpty(deptcode))
            {
                sql += " and deptcode like '" + deptcode + "%'";
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                if (kind == "作业")
                {
                    sql += " and WorkTask like '%" + keyWord + "%'";
                }
                else if (kind == "设备")
                {
                    sql += " and equipmentname like '%" + keyWord + "%'";
                }
                else if (kind == "管理" || kind == "环境")
                {
                    sql += " and DangerSource like '%" + keyWord + "%'";
                }
                else if (kind == "职业病危害")
                {
                    sql += " and MajorName like '%" + keyWord + "%'";
                }
                else if (kind == "岗位")
                {
                    sql += " and jobname like '%" + keyWord + "%'";
                }
                else if (kind == "工器具及危化品")
                {
                    sql += " and toolordanger like '%" + keyWord + "%'";
                }
                else if (kind == "作业活动" || kind=="设备设施")
                {
                    sql += " and name like '%" + keyWord + "%'";
                }
                else
                {
                    sql += " and (WorkTask like '%" + keyWord + "%' or equipmentname like '%" + keyWord + "%' or DangerSource like '%" + keyWord + "%' or MajorName like '%" + keyWord + "%'  or jobname like '%" + keyWord + "%'  or toolordanger like '%" + keyWord + "%' or name like '%" + keyWord + "%' )";
                }
            }
            if (type == 1)
            {
                sql += " and createuserid ='" + user.UserId + "'";
            }
            if (type == 2)
            {

                string roleNames = user.RoleName;
                if (roleNames.Contains("省级"))
                {

                }
                else
                {
                    sql += " and deptcode like '" + user.OrganizeCode + "%' ";
                }
                sql += "  and gradeval=1 ";
            }
            if (type == 3)
            {
                string roleNames = user.RoleName;
                if (roleNames.Contains("省级"))
                {

                }
                else 
                {
                    sql += " and deptcode like '" + user.OrganizeCode + "%' ";
                }
            }

            if (!string.IsNullOrEmpty(grade))
            {
                sql += " and grade='" + grade + "'";
            }
            //if (!string.IsNullOrEmpty(areaId))
            //{
            //    sql += " and districtid='" + areaId + "'";
            //}
            if (user.RoleName.Contains("省级"))
            {
                if (!string.IsNullOrEmpty(deptcode))
                {
                    var dept = new DepartmentService().GetList().Where(x => x.EnCode == deptcode && x.Nature == "厂级").FirstOrDefault();
                    sql += " and deptcode like '" + dept.EnCode + "%'";
                    sql += " group by districtname,districtid) a on instr(','||a.districtid,','|| t.districtid)>0  where t.organizeid='" + dept.DepartmentId + "' order by t.districtcode,sortcode ";
                }
                else
                {
                    sql += " group by districtname,districtid) a on instr(','||a.districtid,','|| t.districtid)>0 where t.organizeid='" + user.OrganizeId + "' order by t.districtcode,sortcode ";
                }
            }
            else
            {
                sql += "and deptcode like '" + user.OrganizeCode + "%'";
                sql += " group by districtname,districtid) a on instr(','||a.districtid,','|| t.districtid)>0  where t.organizeid='" + user.OrganizeId + "' order by t.districtcode,sortcode ";
            }
            sql += " )group by identity,identityName";
            if (!string.IsNullOrEmpty(areaId))
            {
                sql += " having identity='" + areaId + "'";
            }
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 11.2 根据区域ID获取风险清单
        /// </summary>
        /// <param name="areaId">区域ID</param>
        /// <param name="user">当前用户</param>
        /// <param name="type">查询类型：1：本人创建的，2：重大风险，3.本部门所有风险</param>
        ///  <param name="grade">风险级别</param>
        /// <returns></returns>
        public DataTable GetRiskList(Pagination pag)
        {
            return this.BaseRepository().FindTableByProcPager(pag, DbHelper.DbType);
        }
        /// <summary>
        /// 11.3 根据风险ID获取风险详细信息
        /// </summary>
        /// <param name="riskId">风险记录ID</param>
        /// <returns></returns>
        public object GetRisk(string riskId)
        {
            string sql = string.Format(@"select t.districtname riskArea,t.deptname managerDutyDept,t.riskdesc riskDescribe,workcontent,
                                                t.HarmType dangerAttribute,Result,faulttype,levelname,
                                                RiskType,postname station,t.way approveMethod,
                                                t.itema happendedPossibility,t.itemb frequentDegree,t.itemc consequenceSerioucness,
                                                t.itemr riskValue,t.grade riskLevel,t.status,t.deptcode,t.postid,EquipmentName,
                                                Parts,WorkTask,Process,resulttype,AccidentType,dangersource,majorname as riskpoint,
                                                description as harmfactor,harmtype as worklevel,harmproperty as illname,
                                                jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
                                                project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                                packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,name,listingid,harmname,hazardtype,case when  HazardType is null then '' when HazardType is not null then (select wm_concat(b.itemname) from  (select * from base_dataitemdetail where itemid in (select itemid from base_dataitem where itemcode='HazardType')) b where instr(','||HazardType||',',','||b.itemvalue||',')>0) end  as hazardtypename,harmdescription,typesofrisk,riskcategory,exposedrisk,existingmeasures,isspecialequ,checkprojectname,checkstandard,consequences,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,measuresresultval,t.remark,isconventional 
                                                from bis_riskassess t left join bis_riskplan a on t.planid=a.id  
                                                where t.id='{0}'", riskId);
            DataTable dt = this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count == 0)
            {
                sql = string.Format(@"select t.districtname riskArea,t.deptname managerDutyDept,
                                                t.riskdesc riskDescribe,workcontent,t.HarmType dangerAttribute,Result,faulttype,levelname,
                                                RiskType,postname station,t.way approveMethod,t.itema happendedPossibility,
                                                t.itemb frequentDegree,t.itemc consequenceSerioucness,t.itemr riskValue,t.grade riskLevel,
                                                t.status,t.deptcode,t.postid,EquipmentName,Parts,WorkTask,Process,resulttype,AccidentType,
                                                dangersource,majorname as riskpoint,description as harmfactor,harmtype as worklevel,
                                                harmproperty as illname,jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
                                                project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                                packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,name,listingid,harmname,hazardtype,case when  HazardType is null then '' when HazardType is not null then (select wm_concat(b.itemname) from  (select * from base_dataitemdetail where itemid in (select itemid from base_dataitem where itemcode='HazardType')) b where instr(','||HazardType||',',','||b.itemvalue||',')>0) end  as hazardtypename,harmdescription,typesofrisk,riskcategory,exposedrisk,existingmeasures,isspecialequ,checkprojectname,checkstandard,consequences,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,measuresresultval,t.remark,isconventional  
                                                from BIS_RISKHISTORY t left join BIS_RISKPLAN a on t.planid=a.id 
                                                where t.id='{0}'", riskId);
                dt = this.BaseRepository().FindTable(sql);
            }

            if (dt.Rows.Count > 0)
            {
                List<object> measures = new List<object>();
                string[] types = { "工程技术", "管理", "个体防护", "培训教育", "应急处置" };
                foreach (string type in types)
                {
                    DataTable dtMeasures = this.BaseRepository().FindTable(string.Format("select content from BIS_MEASURES t where t.riskid='{0}' and typename='{1}'", riskId, type));
                    measures.Add(new { name = type, items = dtMeasures });
                }
                dt.Dispose();
                return new
                {
                    riskid = riskId,
                    project = dt.Rows[0]["project"].ToString(),
                    dutyperson = dt.Rows[0]["dutyperson"].ToString(),
                    dutypersonid = dt.Rows[0]["dutypersonid"].ToString(),
                    element = dt.Rows[0]["element"].ToString(),
                    faultcategory = dt.Rows[0]["faultcategory"].ToString(),
                    majornametype = dt.Rows[0]["majornametype"].ToString(),
                    packuntil = dt.Rows[0]["packuntil"].ToString(),
                    packnum = dt.Rows[0]["packnum"].ToString(),
                    storagespace = dt.Rows[0]["storagespace"].ToString(),
                    postdeptid = dt.Rows[0]["postdeptid"].ToString(),
                    postdept = dt.Rows[0]["postdept"].ToString(),
                    postperson = dt.Rows[0]["postperson"].ToString(),
                    postpersonid = dt.Rows[0]["postpersonid"].ToString(),
                    postdeptcode = dt.Rows[0]["postdeptcode"].ToString(),

                    riskarea = dt.Rows[0]["riskarea"].ToString(),
                    managerdutydept = dt.Rows[0]["managerDutyDept"].ToString(),
                    riskdescribe = dt.Rows[0]["riskDescribe"].ToString(),
                    dangerattribute = dt.Rows[0]["dangerAttribute"].ToString(),
                    dangerresult = dt.Rows[0]["Result"].ToString(),
                    risktype = dt.Rows[0]["RiskType"].ToString(),
                    station = dt.Rows[0]["station"].ToString(),
                    approvemethod = dt.Rows[0]["approveMethod"].ToString(),
                    happendedpossibility = dt.Rows[0]["happendedPossibility"].ToString(),
                    frequentdegree = dt.Rows[0]["frequentDegree"].ToString(),
                    consequenceserioucness = dt.Rows[0]["consequenceSerioucness"].ToString(),
                    riskvalue = dt.Rows[0]["riskValue"].ToString(),
                    risklevel = dt.Rows[0]["riskLevel"].ToString(),

                    deptcode = dt.Rows[0]["deptcode"].ToString(),
                    jobid = dt.Rows[0]["postid"].ToString(),
                    status = dt.Rows[0]["status"].ToString(),
                    resulttype = dt.Rows[0]["resulttype"].ToString(),
                    result = dt.Rows[0]["result"].ToString(),
                    equipmentname = dt.Rows[0]["equipmentname"].ToString(),
                    parts = dt.Rows[0]["parts"].ToString(),
                    workTask = dt.Rows[0]["workTask"].ToString(),
                    process = dt.Rows[0]["process"].ToString(),
                    dangersource = dt.Rows[0]["dangersource"].ToString(),
                    accidenttype = dt.Rows[0]["accidenttype"].ToString(),
                    riskpoint = dt.Rows[0]["riskpoint"].ToString(),
                    harmfactor = dt.Rows[0]["harmfactor"].ToString(),
                    worklevel = dt.Rows[0]["worklevel"].ToString(),
                    illname = dt.Rows[0]["illname"].ToString(),
                    faulttype = dt.Rows[0]["faulttype"].ToString(),
                    levelname = dt.Rows[0]["levelname"].ToString(),
                    measures = measures,
                    jobname = dt.Rows[0]["jobname"].ToString(),
                    toolordanger = dt.Rows[0]["toolordanger"].ToString(),
                    dangersourcetype = dt.Rows[0]["dangersourcetype"].ToString(),
                    hjsystem = dt.Rows[0]["hjsystem"].ToString(),
                    hjequpment = dt.Rows[0]["hjequpment"].ToString(),
                    name = dt.Rows[0]["name"].ToString(),
                    listingid = dt.Rows[0]["listingid"].ToString(),
                    harmname = dt.Rows[0]["harmname"].ToString(),
                    hazardtype = dt.Rows[0]["hazardtype"].ToString(),
                    hazardtypename = dt.Rows[0]["hazardtypename"].ToString(),
                    harmdescription = dt.Rows[0]["harmdescription"].ToString(),
                    typesofrisk = dt.Rows[0]["typesofrisk"].ToString(),
                    riskcategory = dt.Rows[0]["riskcategory"].ToString(),
                    exposedrisk = dt.Rows[0]["exposedrisk"].ToString(),
                    existingmeasures = dt.Rows[0]["existingmeasures"].ToString(),
                    isspecialequ = dt.Rows[0]["isspecialequ"].ToString(),
                    checkprojectname = dt.Rows[0]["checkprojectname"].ToString(),
                    checkstandard = dt.Rows[0]["checkstandard"].ToString(),
                    consequences = dt.Rows[0]["consequences"].ToString(),
                    advicemeasures = dt.Rows[0]["advicemeasures"].ToString(),
                    effectiveness = dt.Rows[0]["effectiveness"].ToString(),
                    costfactor = dt.Rows[0]["costfactor"].ToString(),
                    measuresresult = dt.Rows[0]["measuresresult"].ToString(),
                    isadopt = dt.Rows[0]["isadopt"].ToString(),
                    measuresresultval = dt.Rows[0]["measuresresultval"].ToString(),
                    remark = dt.Rows[0]["remark"].ToString(),
                    isconventional = dt.Rows[0]["isconventional"].ToString(),
                    workcontent = dt.Rows[0]["workcontent"].ToString()
                };

            }
            else
            {
                return null;
            }

        }

        /// <summary>11.4 区域</summary>
        /// <param name="user">当前用户</param>
        /// <param name="planId">计划Id</param>
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
            string areaIds = dt.Rows[0][0].ToString(); string endDate = DateTime.Parse(dt.Rows[0][1].ToString()).ToString("yyyy-MM-dd 23:59:59");
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
                if (DbHelper.DbType == DatabaseType.MySql)
                {
                    sql = string.Format("select t.districtid identifyAreaId,t.districtcode identifyAreaCode,t.districtname identifyAreaName,case when isnull(identityNum) then 0 else identityNum end as identityNum,t.chargedept,t.chargedeptcode from BIS_DISTRICT t ", user.OrganizeId);
                }
                if (status == 0)
                {
                    sql += string.Format("left join (select districtid,count(1) identityNum from BIS_RISKASSESS where status>0 and deletemark=0 and createdate<=to_date('{0}','yyyy-mm-dd hh24:mi:ss')  and districtid in('{1}')", endDate, areaIds.Replace(",", "','"));
                    if (DbHelper.DbType == DatabaseType.MySql)
                    {
                        sql += string.Format("left join (select districtid,count(1) identityNum from BIS_RISKASSESS where status>0 and deletemark=0 and createdate<='{0}' and districtid in('{1}')", endDate, areaIds.Replace(",", "','"));
                    }
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
            DataItemDetailService detailservice = new DataItemDetailService();
            string version = detailservice.GetItemValue("广西华昇版本"); //广西华昇版本查询管控部门是自己部门的数据
            string sql = "";
            if (status == 0)
            {
                DataTable dt = this.BaseRepository().FindTable(string.Format("select areaid,enddate from BIS_RISKPLAN where id='{0}'", planId));
                string areaIds = dt.Rows[0][0].ToString(); string endDate = DateTime.Parse(dt.Rows[0][1].ToString()).ToString("yyyy-MM-dd 23:59:59");
                if (string.IsNullOrWhiteSpace(version))
                {
                    sql = string.Format(@"select t.districtname riskArea,t.deptname controlDutyStation,
                                                t.riskdesc riskDescribe,t.HarmType harmProperty,Result riskResult,
                                                t.id riskid,t.postid jobid,t.deptcode,status,createuserid userid,
                                                risktype,postname job,t.way approveMethod,t.itema happendedPossibility,
                                                t.itemb frequentDegree,t.itemc consequenceSerioucness,t.itemr riskValue,
                                                t.grade riskLevel,majorname as riskpoint,worktask,process,equipmentname,
                                                parts,description,DangerSource,t.jobname,t.toolordanger,t.dangersourcetype,t.hjsystem,t.hjequpment,name,to_char(isconventional) as isconventional,to_char(isspecialequ) as isspecialequ,workcontent,checkprojectname
                                                from bis_riskassess t  
                                                where status>0 and deletemark=0 and createdate<=to_date('{1}','yyyy-mm-dd hh24:mi:ss') 
                                                and areacode like '{0}%' and districtid in('{2}')", areaCode, endDate, areaIds.Replace(",", "','"));
                }
                else
                {
                    sql = string.Format(@"select t.districtname riskArea,t.deptname controlDutyStation,
                                                t.riskdesc riskDescribe,t.HarmType harmProperty,Result riskResult,
                                                t.id riskid,t.postid jobid,t.deptcode,status,createuserid userid,
                                                risktype,postname job,t.way approveMethod,t.itema happendedPossibility,
                                                t.itemb frequentDegree,t.itemc consequenceSerioucness,t.itemr riskValue,
                                                t.grade riskLevel,majorname as riskpoint,worktask,process,equipmentname,
                                                parts,description,DangerSource,t.jobname,t.toolordanger,t.dangersourcetype,t.hjsystem,t.hjequpment,name,to_char(isconventional) as isconventional,to_char(isspecialequ) as isspecialequ,workcontent,checkprojectname
                                                from bis_riskassess t  
                                                where status>0 and deletemark=0 and createdate<=to_date('{0}','yyyy-mm-dd hh24:mi:ss') 
                                                and deptcode = '{1}'", endDate, user.DeptCode);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(version))
                {
                    sql = string.Format(@"select t.districtname riskArea,t.deptname controlDutyStation,
                                                t.riskdesc riskDescribe,t.HarmType harmProperty,Result riskResult,t.id riskid,
                                                t.postid jobid,t.deptcode,status,createuserid userid,risktype,postname job,
                                                t.way approveMethod,t.itema happendedPossibility,t.itemb frequentDegree,t.itemc consequenceSerioucness,
                                                t.itemr riskValue,t.grade riskLevel,majorname as riskpoint,worktask,process,equipmentname,parts,
                                                description,DangerSource,t.jobname,t.toolordanger,t.dangersourcetype,t.hjsystem,t.hjequpment,name,to_char(isconventional) as isconventional,to_char(isspecialequ) as isspecialequ,workcontent,checkprojectname
                                                from bis_riskhistory t 
                                                where status>0 and deletemark=0 
                                                and areacode like '{0}%' and newplanid='{1}'", areaCode, planId);

                }
                else
                {
                    sql = string.Format(@"select t.districtname riskArea,t.deptname controlDutyStation,
                                                t.riskdesc riskDescribe,t.HarmType harmProperty,Result riskResult,t.id riskid,
                                                t.postid jobid,t.deptcode,status,createuserid userid,risktype,postname job,
                                                t.way approveMethod,t.itema happendedPossibility,t.itemb frequentDegree,t.itemc consequenceSerioucness,
                                                t.itemr riskValue,t.grade riskLevel,majorname as riskpoint,worktask,process,equipmentname,parts,
                                                description,DangerSource,t.jobname,t.toolordanger,t.dangersourcetype,t.hjsystem,t.hjequpment,name,to_char(isconventional) as isconventional,to_char(isspecialequ) as isspecialequ,workcontent,checkprojectname
                                                from bis_riskhistory t 
                                                where status>0 and deletemark=0 and newplanid='{0}'",  planId);
                }
               
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
            //IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            string sql = "";
            if (string.IsNullOrEmpty(assess.Id))
            {
                assess.Status = 2;
                assess.DeleteMark = 0;
                assess.State = 1;
                assess.Create();
                DbFactory.Base().Insert<RiskAssessEntity>(assess);
                //db.Insert<RiskAssessEntity>(assess);
//                sql = string.Format(@"insert into BIS_RISKASSESS(id,dangersource,deptcode,
//                                        deptname,postid,postname,createuserid,createdate,createusername,createuserdeptcode,
//                                        createuserorgcode,status,result,harmtype,risktype,DeleteMark,state,districtid,districtname,
//                                        planid,areacode,riskdesc,resulttype,EquipmentName,Parts,WorkTask,Process,
//                                        AccidentType,majorname,description,harmproperty,faulttype,levelname,jobname,
//                                        toolordanger,dangersourcetype,hjsystem,hjequpment,project,dutyperson,dutypersonid,element,faultcategory,majornametype,
//                                        packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,NAME,LISTINGID,HARMNAME,HAZARDTYPE,HARMDESCRIPTION,TYPESOFRISK,RISKCATEGORY,EXPOSEDRISK,EXISTINGMEASURES,ISSPECIALEQU,CHECKPROJECTNAME,CHECKSTANDARD,CONSEQUENCES,ADVICEMEASURES,EFFECTIVENESS,COSTFACTOR,MEASURESRESULT,ISADOPT,MEASURESRESULTVAL,REMARK,ISCONVENTIONAL) values(
//                                             '{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}','{10}',{11},
//                                        '{12}','{13}','{14}',{15},{16},'{17}','{18}','{19}','{20}',
//                                        '{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}',
//                                        '{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}',
//'{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}',
//'{46}','{47}','{48}','{49}','{50}','{51}')", Guid.NewGuid().ToString(), assess.DangerSource,
//                                                              assess.DeptCode, assess.DeptName, assess.PostId, 
//                                                              assess.PostName, user.UserId, "sysdate", user.UserName, 
//                                                              user.DeptCode, user.OrganizeCode, 2, assess.Result, 
//                                                              assess.HarmType, assess.RiskType, 0, 1, assess.DistrictId,
//                                                              assess.DistrictName, assess.PlanId, assess.AreaCode, 
//                                                              assess.RiskDesc, assess.ResultType, assess.EquipmentName,
//                                                              assess.Parts, assess.WorkTask, assess.Process, assess.AccidentType,
//                                                              assess.MajorName, assess.Description, assess.HarmProperty,
//                                                              assess.FaultType, assess.LevelName,assess.JobName,assess.ToolOrDanger,
//                                                              assess.DangerSourceType,assess.HjSystem,assess.HjEqupment,assess.Project,
//                                        assess.DutyPerson,assess.DutyPersonId,assess.Element,assess.FaultCategory,assess.MajorNameType,
//                                        assess.PackUntil,assess.PackNum,assess.StorageSpace,assess.PostDept,assess.PostDeptId,assess.PostPerson,
//                                        assess.PostPersonId,assess.PostDeptCode);
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

                assess.Modify(assess.Id);
                DbFactory.Base().Update<RiskAssessEntity>(assess);
                //db.Update<RiskAssessEntity>(assess);
                //sql = string.Format(@"update BIS_RISKASSESS 
                //                    set dangersource='{1}',deptcode='{2}',deptname='{3}',postid='{4}',
                //                    postname='{5}',result='{6}',risktype='{7}',districtid='{8}',
                //                    districtname='{9}',areacode='{10}',State={11},planid='{12}',
                //                    riskdesc='{13}',resulttype='{14}',EquipmentName='{15}',Parts='{16}',
                //                    WorkTask='{17}',Process='{18}',AccidentType='{19}',majorname='{20}',
                //                    description='{21}',harmtype='{22}',harmproperty='{23}',faulttype='{24}',
                //                    levelname='{25}',jobname='{26}',toolordanger='{27}',dangersourcetype='{28}',
                //                    hjsystem='{29}',hjequpment='{30}',project='{31}',dutyperson='{32}',dutypersonid='{33}',
                //                    element='{34}',faultcategory='{35}',majornametype='{36}',
                //                    packuntil='{37}',packnum='{38}',storagespace='{39}',postdept='{40}',
                //                    postdeptid='{41}',postperson='{42}',postpersonid='{43}',postdeptcode='{44}' where id='{0}'", assess.Id, assess.DangerSource, assess.DeptCode, 
                //                                                    assess.DeptName, assess.PostId, assess.PostName, 
                //                                                    assess.Result, assess.RiskType, assess.DistrictId, 
                //                                                    assess.DistrictName, assess.AreaCode, assess.State, 
                //                                                    assess.PlanId, assess.RiskDesc, assess.ResultType, 
                //                                                    assess.EquipmentName, assess.Parts, assess.WorkTask, 
                //                                                    assess.Process, assess.AccidentType, assess.MajorName, 
                //                                                    assess.Description, assess.HarmType, assess.HarmProperty,
                //                                                    assess.FaultType, assess.LevelName, assess.JobName, assess.ToolOrDanger,
                //                                              assess.DangerSourceType, assess.HjSystem, assess.HjEqupment, assess.Project,
                //                        assess.DutyPerson, assess.DutyPersonId, assess.Element, assess.FaultCategory, assess.MajorNameType,
                //                        assess.PackUntil, assess.PackNum, assess.StorageSpace, assess.PostDept, assess.PostDeptId, assess.PostPerson,
                //                        assess.PostPersonId, assess.PostDeptCode);
            }

            //return this.BaseRepository().ExecuteBySql(sql);
            return 1;
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
        /// <param name="deptCode">部门编码</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DataTable GetPostList(string deptCode, ERCHTMS.Code.Operator user)
        {
            string sql = string.Format("select t.roleid jobId,t.fullname jobStr from base_role t where t.category=2 and t.organizeid='{0}'  and t.deptid  in (select t.departmentid from base_department t where t.encode like '{1}%')", user.OrganizeId, deptCode);
            //if (deptCode.Length == 3)
            //{
            //    sql = string.Format("select t.roleid jobId,t.fullname jobStr from base_role t where t.category=2 and t.organizeid='{0}'  and t.nature='厂级'", user.OrganizeId);
            //}
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
            return this.BaseRepository().FindTable(string.Format("select t.itemname harmPropertyName,t.itemvalue harmPropertyId from BASE_DATAITEMDETAIL t where deletemark=0 and enabledmark=1 and t.itemid=(select itemid from base_dataitem a where a.itemcode='{0}')", itemCode));
        }
        #endregion

        #region 14.2 统计
        /// <summary>
        /// 风险统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object GetStat(ERCHTMS.Code.Operator user,string deptcode="")
        {
            DataTable dt=new DataTable();
            List<object> list = new List<object>();
            if (user.RoleName.Contains("省级用户")) {
                string strWhere = "";
                string strSql = string.Format(@"select t.grade,count(id) num
                                          from BIS_RISKASSESS t
                                          inner join (select b.encode, b.fullname
                                                       from BASE_DEPARTMENT b
                                                      where b.deptcode like '{0}%'
                                                        and b.nature = '厂级') d
                                            on d.encode = t.createuserorgcode
                                         where t.status=1 and t.deletemark=0 and t.enabledmark=0 and t.grade is not null ",user.NewDeptCode);
                                        
                if (!string.IsNullOrWhiteSpace(deptcode)) {
                    strWhere = string.Format("and t.createuserorgcode='{0}'", deptcode);
                    strSql += strWhere;
                }
                string groupby = " group by t.grade";
                strSql += groupby;
                dt = this.BaseRepository().FindTable(strSql);
            
            } else {
                string roleNames = user.RoleName;
                string sql = "select grade,count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and grade is not null and deptcode like '" + user.OrganizeCode + "%'";
                if (!(user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户")))
                {
                    sql += string.Format(" and deptcode like '" + user.DeptCode + "%'");
                }
                sql += " group by grade";
                dt = this.BaseRepository().FindTable(sql);
            }
            int count1 = dt.Select("grade='重大风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='重大风险'")[0][1].ToString());
            int count2 = dt.Select("grade='较大风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='较大风险'")[0][1].ToString());
            int count3 = dt.Select("grade='一般风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='一般风险'")[0][1].ToString());
            int count4 = dt.Select("grade='低风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='低风险'")[0][1].ToString());
            int sum = count1 + count2 + count3 + count4;
            decimal percent = sum == 0 ? 0 : decimal.Parse(count1.ToString()) / decimal.Parse(sum.ToString());
            list.Add(new { risklevel = "重大风险", problemNum = count1, problemrate = Math.Round(percent, 4) });
            percent = sum == 0 ? 0 : decimal.Parse(count2.ToString()) / decimal.Parse(sum.ToString());
            list.Add(new { risklevel = "较大风险", problemNum = count2, problemrate = Math.Round(percent, 4) });
            percent = sum == 0 ? 0 : decimal.Parse(count3.ToString()) / decimal.Parse(sum.ToString());
            list.Add(new { risklevel = "一般风险", problemNum = count3, problemrate = Math.Round(percent, 4) });
            percent = sum == 0 ? 0 : decimal.Parse(count4.ToString()) / decimal.Parse(sum.ToString());
            list.Add(new { risklevel = "低风险", problemNum = count4, problemrate = Math.Round(percent, 4) });

            return new { risktotalnum = sum, riskList = list };
        }
        #endregion
        #endregion
    }
}
