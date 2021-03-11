using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using ERCHTMS.Code;
using System.Text;
using System;

namespace ERCHTMS.Service.Observerecord
{
    /// <summary>
    /// 描 述：观察计划
    /// </summary>
    public class ObsplanService : RepositoryFactory<ObsplanEntity>, ObsplanIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["ItemCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.plandeptcode like '{0}%' ", queryParam["ItemCode"].ToString());
            }
            if (!queryParam["PlanYear"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.planyear>='{0}' ", queryParam["PlanYear"].ToString());
            }
            if (!queryParam["PlanLevel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.planlevel='{0}'", queryParam["PlanLevel"].ToString());
            }
            if (!queryParam["txt_Keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.workname like'%{0}%' or p.workname like'%{0}%' or p.obsperson like'%{0}%' ) ", queryParam["txt_Keyword"].ToString());
            }
            if (!queryParam["DeptCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.acceptdeptcode like '{0}%' ", queryParam["DeptCode"].ToString());
            }
            if (!queryParam["PlanDeptCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.plandeptcode = '{0}' or t.planspeciatycode='{0}')", queryParam["PlanDeptCode"].ToString());
            }
            if (!queryParam["PlanArea"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.planareacode = '{0}' ", queryParam["PlanArea"].ToString());
            }
            if (!queryParam["PlanWorkName"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.workname like '%{0}%' ", queryParam["PlanWorkName"].ToString());
            }
            if (!queryParam["PlanRiskLevel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and p.risklevel like '{0}%' ", queryParam["PlanRiskLevel"].ToString());
            }
            if (!queryParam["PlanMonth"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ','||p.obsmonth||',' like '%,{0},%' ",Convert.ToInt32(queryParam["PlanMonth"].ToString()));
            }
            
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 根据观察计划Id与任务分解Id获取相应信息--此处查询的是发布后的数据--BIS_OBSPLAN_FB
        /// </summary>
        /// <param name="PlanId">计划id </param>
        /// <param name="PlanFjId">任务分解Id</param>
        /// <returns></returns>
        public DataTable GetPlanById(string PlanId, string PlanFjId,string month) {
            string sql = string.Format(@"select t.workname||'_'||p.workname WorkName,t.planarea WorkArea,t.planyear,
                                           t.planareacode WorkAreaId,t.plandept WorkUnit,t.plandeptcode WorkUnitCode,
                                           t.plandeptid WorkUnitId,p.obsperson ObsPerson,p.obspersonid ObsPersonId,
                                           null ObsStartTime,null ObsEndTime
                                      from bis_obsplan_fb t left join bis_obsplanwork p on p.planid = t.id
                                        where t.id like'{0}%' and p.id like'{1}%'",PlanId,PlanFjId);
           var dt= this.BaseRepository().FindTable(sql);
           if (dt.Rows.Count > 0) {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   if(!string.IsNullOrWhiteSpace(dt.Rows[i]["planyear"].ToString())){
                       dt.Rows[i]["planyear"] = DateTime.Now.Year.ToString();
                   }
                   dt.Rows[i]["ObsStartTime"] = new DateTime(Convert.ToInt32(dt.Rows[i]["planyear"]), Convert.ToInt32(month), 1);
                   dt.Rows[i]["ObsEndTime"] = Convert.ToDateTime(dt.Rows[i]["ObsStartTime"]).AddMonths(1).AddDays(-1);
               }
           }
           return dt;
        
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ObsplanEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObsplanEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            var res = this.BaseRepository().BeginTrans();
            try
            {
                string subSql = string.Format(@"delete from BIS_OBSPLANWORK t where  t.planid='{0}'", keyValue);
                res.ExecuteBySql(subSql);
                res.Delete(keyValue);
                res.Commit();
            }
            catch (Exception)
            {

                res.Rollback();
            }
            //this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ObsplanEntity entity)
        {
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {

                ObsplanEntity old = this.BaseRepository().FindEntity(keyValue);
                if (old == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }

            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// 部门级提交到Ehs--
        /// Ehs提交到分管领导
        /// </summary>
        /// <param name="currUser"></param>
        /// <returns></returns>
        public bool CommitEhsData(Operator currUser)
        {
            var res = this.BaseRepository().BeginTrans();
            try
            {
                var oldTable = "BIS_OBSPLAN";
                var tableName = "BIS_OBSPLAN_COMMITEHS";
                var oldFeedBackTable = "BIS_OBSFEEDBACK";
                var newFeddBackTable = "BIS_OBSFEEDBACK_EHS";
                if (currUser.RoleName.Contains("公司级") && currUser.RoleName.Contains("安全管理员"))
                {
                    oldTable = "BIS_OBSPLAN_FB";
                    tableName = "BIS_OBSPLAN_TZ";

                    //第一次提交插入数据，后面提交的数据先删除原先数据在进行插入
                    string sql = string.Format(@"SELECT ID FROM {0} T ", tableName);
                    DataTable dt = this.BaseRepository().FindTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        sql = string.Format(@"DELETE FROM {0} T ", tableName);
                        res.ExecuteBySql(sql);
                    }
                    //向关系表中添加数据
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append(string.Format(" UPDATE BIS_OBSPLAN_FB SET ISCOMMIT='1',ISPUBLIC='1'"));
                    res.ExecuteBySql(sbSql.ToString());
                    //将发布状态同步到部门表中
                    string updateSql1 = string.Format(@"UPDATE BIS_OBSPLAN_COMMITEHS SET ISPUBLIC='1' WHERE OLDPLANID IN(SELECT OLDPLANID FROM BIS_OBSPLAN_FB) OR ID IN(SELECT OLDPLANID FROM BIS_OBSPLAN_FB)");
                    res.ExecuteBySql(updateSql1);
                    string updateSql = string.Format(@"UPDATE BIS_OBSPLAN SET ISPUBLIC='1' WHERE ID IN(SELECT OLDPLANID FROM BIS_OBSPLAN_COMMITEHS)");
                    res.ExecuteBySql(updateSql);
                    //res.Commit();
                }
                else
                {
                    if (currUser.RoleName.Contains("负责人") && currUser.RoleName.Contains("厂级部门"))
                    {
                        oldTable = "BIS_OBSPLAN_COMMITEHS";
                        tableName = "BIS_OBSPLAN_FB";
                        oldFeedBackTable = "BIS_OBSFEEDBACK_EHS";
                        newFeddBackTable = "BIS_OBSFEEDBACK_FB";
                        //第一次提交插入数据，后面提交的数据先删除原先数据在进行插入
                        string sql = string.Format(@"SELECT ID FROM {0} T ", tableName);
                        DataTable dt = this.BaseRepository().FindTable(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                           
                            sql = string.Format(@"DELETE FROM {0} T ", tableName);
                            res.ExecuteBySql(sql);
                        }
                        //更新数据为提交状态
                        string updateSql = string.Format(@"UPDATE {0} T SET T.ISCOMMIT='1'  ", oldTable);
                        res.ExecuteBySql(updateSql);

                        //先将已经提交的数据状态更新到部门表中
                        string update = string.Format(@"UPDATE BIS_OBSPLAN SET ISEMSCOMMIT='1' WHERE ID IN(SELECT OLDPLANID FROM BIS_OBSPLAN_COMMITEHS)");
                        res.ExecuteBySql(update);

                        //EHS部意见同步到分管领导
                        string sqlFeed = string.Format(@"SELECT ID FROM {0} T ", newFeddBackTable);
                        DataTable dtFeed = this.BaseRepository().FindTable(sqlFeed);
                        if (dtFeed != null && dtFeed.Rows.Count > 0)
                        {
                            sqlFeed = string.Format(@"DELETE FROM {0} T ", newFeddBackTable);
                            res.ExecuteBySql(sqlFeed);
                        }
                    }
                    else {
                        //第一次提交插入数据，后面提交的数据先删除原先数据在进行插入
                        string sql = string.Format(@"SELECT ID FROM {1} T WHERE T.PLANDEPTCODE='{0}'", currUser.DeptCode, tableName);
                        DataTable dt = this.BaseRepository().FindTable(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            sql = string.Format(@"DELETE FROM {1} T WHERE T.PLANDEPTCODE='{0}'", currUser.DeptCode, tableName);
                            res.ExecuteBySql(sql);
                        }
                        //更新数据为提交状态
                        string updateSql = string.Format(@"UPDATE {1} T SET T.ISCOMMIT='1'  WHERE t.PLANDEPTCODE='{0}'", currUser.DeptCode, oldTable);
                        res.ExecuteBySql(updateSql);

                        //部门负责人提交时--部门提交的意见也一并同步到EHS部
                        string sqlFeed = string.Format(@"SELECT ID FROM {1} T WHERE T.ACCEPTDEPTCODE='{0}'", currUser.DeptCode, newFeddBackTable);
                        DataTable dtFeed = this.BaseRepository().FindTable(sqlFeed);
                        if (dtFeed != null && dtFeed.Rows.Count > 0)
                        {
                            sqlFeed = string.Format(@"DELETE FROM {1} T WHERE T.ACCEPTDEPTCODE='{0}'", currUser.DeptCode, newFeddBackTable);
                            res.ExecuteBySql(sqlFeed);
                        }
                    }
                  
                }
                string newId = Guid.NewGuid().ToString();
                #region 向关系表中添加数据--同步计划内容
                var strCase = string.Empty;
                var strOld = string.Empty;
                if ((currUser.RoleName.Contains("负责人") && currUser.RoleName.Contains("厂级部门")) || (currUser.RoleName.Contains("安全管理员") && currUser.RoleName.Contains("公司级")))
                {
                    strCase = "CASE WHEN ISCOMMIT='1' THEN '1' ELSE '0' END ";
                    strOld = "CASE WHEN OLDPLANID IS NOT NULL THEN OLDPLANID ELSE ID END ";
                }
                else
                {
                    strOld = "ID";
                    strCase = "CASE WHEN ISEMSCOMMIT='1' THEN '1' ELSE '0' END ";
                }
                StringBuilder sbSql1 = new StringBuilder();
                sbSql1.Append(string.Format(" INSERT INTO {0} (ID,CREATEUSERID,", tableName));
                sbSql1.Append(" CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,");
                sbSql1.Append(" PLANYEAR,PLANDEPT,PLANDEPTCODE,PLANSPECIATY,PLANSPECIATYCODE,PLANAREA,PLANAREACODE,");
                sbSql1.Append(" WORKNAME,REMARK,PLANLEVEL,ISCOMMIT,PLANDEPTID,OLDPLANID,ISPUBLIC)");
                sbSql1.Append(" SELECT (ID||'-'||'" + newId + "'),CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,");
                sbSql1.Append(" PLANYEAR,PLANDEPT,PLANDEPTCODE,PLANSPECIATY,PLANSPECIATYCODE,PLANAREA,PLANAREACODE,");
                sbSql1.Append(string.Format(" WORKNAME,REMARK,PLANLEVEL,{1},PLANDEPTID,{2},ISPUBLIC FROM {0} WHERE 1=1 ", oldTable, strCase, strOld));
                if (currUser.RoleName.Contains("负责人") && currUser.RoleName.Contains("厂级部门") || (currUser.RoleName.Contains("安全管理员") && currUser.RoleName.Contains("公司级")))
                {

                }
                else
                {

                    sbSql1.Append(string.Format(" AND PLANDEPTCODE='{0}'", currUser.DeptCode));
                }
                res.ExecuteBySql(sbSql1.ToString());
                #endregion
                #region 关联字表--同步字表
                StringBuilder subsbSql = new StringBuilder();
                string where = string.Empty;
                if ((currUser.RoleName.Contains("负责人") && currUser.RoleName.Contains("厂级部门")) || (currUser.RoleName.Contains("安全管理员") && currUser.RoleName.Contains("公司级")))
                {
                    strCase = "CASE WHEN OLDWORKID IS NOT NULL THEN OLDWORKID ELSE ID END ";
                }
                else
                {
                    strCase = "ID";
                    where = string.Format(" AND T.PLANDEPTCODE='{0}'", currUser.DeptCode);
                }
                subsbSql.Append("INSERT INTO BIS_OBSPLANWORK(ID,CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,OBSPERSON,");
                subsbSql.Append("OBSPERSONID,RISKLEVEL,OBSNUM,OBSMONTH,PLANID,WORKNAME,REMARK,OBSNUMTEXT,OLDWORKID)");
                subsbSql.Append("SELECT (ID||'" + newId + "'),CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,OBSPERSON,");
                subsbSql.Append(string.Format("OBSPERSONID,RISKLEVEL,OBSNUM,OBSMONTH,(PLANID||'-'||'" + newId + "'),WORKNAME,REMARK,OBSNUMTEXT,{2} FROM BIS_OBSPLANWORK WHERE PLANID IN(SELECT ID FROM {1} T WHERE 1=1 {0})", where, oldTable, strCase));
                res.ExecuteBySql(subsbSql.ToString());
                #endregion
                #region 同步意见
                if (currUser.RoleName.Contains("安全管理员") && currUser.RoleName.Contains("公司级"))
                {
                    //分管领导提交后不需要同步意见到台账
                }
                else
                {
                    StringBuilder feedSql = new StringBuilder();
                    string where1 = string.Empty;
                    if ((currUser.RoleName.Contains("负责人") && currUser.RoleName.Contains("厂级部门")))
                    {

                    }
                    else
                    {
                        where1 = string.Format(" AND ACCEPTDEPTCODE='{0}'", currUser.DeptCode);
                    }
                    feedSql.Append(string.Format("INSERT INTO {0}(ID,CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,", newFeddBackTable));
                    feedSql.Append("MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,ACCEPTDEPT,ACCEPTDEPTCODE,SUGGEST,ACCEPTDEPTID)");
                    feedSql.Append("SELECT (ID||'" + newId + "'),CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,");
                    feedSql.Append(string.Format("ACCEPTDEPT,ACCEPTDEPTCODE,SUGGEST,ACCEPTDEPTID FROM {0} WHERE 1=1 {1}", oldFeedBackTable, where1));
                    res.ExecuteBySql(feedSql.ToString());
                }
                #endregion
                res.Commit();
                return true;
            }
            catch (Exception)
            {
                res.Rollback();
                return false;
            }

        }

        /// <summary>
        /// 查询本年度该观察计划那些月份进行了观察记录
        /// </summary>
        /// <param name="planId">计划Id</param>
        /// <param name="planfjid">计划任务分解Id</param>
        /// <param name="year">年度</param>
        /// <returns></returns>
        public DataTable GetObsRecordIsExist(string planId, string planfjid, string year) {

            string sql = string.Format(@"select nvl((case when (to_char(t.obsstarttime,'MM')='01'or to_char(t.obsendtime,'MM')='01') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m1,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='02' or to_char(t.obsendtime,'MM')='02') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m2,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='03' or to_char(t.obsendtime,'MM')='03') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m3,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='04' or to_char(t.obsendtime,'MM')='04') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m4,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='05' or to_char(t.obsendtime,'MM')='05') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m5,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='06' or to_char(t.obsendtime,'MM')='06') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m6,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='07' or to_char(t.obsendtime,'MM')='07') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m7,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='08' or to_char(t.obsendtime,'MM')='08') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m8,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='09' or to_char(t.obsendtime,'MM')='09') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m9,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='10' or to_char(t.obsendtime,'MM')='10') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m10,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='11' or to_char(t.obsendtime,'MM')='11') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m11,
                                            nvl((case when (to_char(t.obsstarttime,'MM')='12' or to_char(t.obsendtime,'MM')='12') and to_char(t.obsstarttime,'yyyy')='{0}' then 1 else 0 end),0) m12
                                            from bis_observerecord t
                                         where t.obsplanid like'{1}%' and t.obsplanfjid like'{2}%' and t.iscommit='1' ", year, planId, planfjid);


            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 导入模板数据
        /// </summary>
        /// <param name="obsplan"></param>
        /// <param name="obsplanwork"></param>
        public void InsertImportData(List<ObsplanEntity> obsplan, List<ObsplanworkEntity> obsplanwork) {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                res.Insert<ObsplanEntity>(obsplan);
                res.Insert<ObsplanworkEntity>(obsplanwork);
                res.Commit();
            }
            catch (System.Exception)
            {

                res.Rollback();
            }
        }
        /// <summary>
        /// 导入模板数据
        /// </summary>
        /// <param name="obsplan"></param>
        /// <param name="obsplanwork"></param>
        public void InsertImportData(List<ObsplanEHSEntity> obsplan, List<ObsplanworkEntity> obsplanwork)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                res.Insert<ObsplanEHSEntity>(obsplan);
                res.Insert<ObsplanworkEntity>(obsplanwork);
                res.Commit();
            }
            catch (System.Exception)
            {

                res.Rollback();
            }
        }
        /// <summary>
        /// 复制年度计划
        /// </summary>
        /// <param name="currUser">当前用户</param>
        /// <param name="oldYear">复制的年度</param>
        /// <param name="newYear">复制到的年度</param>
        /// <returns></returns>
        public bool CopyHistoryData(Operator currUser, string oldYear, string newYear) {
            var res = this.BaseRepository().BeginTrans();
            try
            {
                //部门用户复制本部门数据--EHS部复制EHS部数据（EHS数据在BIS_OBSPLAN_COMMITEHS）
                var tableName = "BIS_OBSPLAN";
                var strCase = "ID";

                if (currUser.RoleName.Contains("厂级部门"))
                {
                    tableName = "BIS_OBSPLAN_COMMITEHS";
                    strCase = "CASE WHEN OLDWORKID IS NOT NULL THEN OLDWORKID ELSE ID END ";
                }
                string newId = Guid.NewGuid().ToString();
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append(string.Format(" INSERT INTO {0} (ID,CREATEUSERID,", tableName));
                sbSql.Append(" CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,");
                sbSql.Append(" PLANYEAR,PLANDEPT,PLANDEPTCODE,PLANSPECIATY,PLANSPECIATYCODE,PLANAREA,PLANAREACODE,");
                sbSql.Append(" WORKNAME,REMARK,PLANLEVEL,ISCOMMIT,PLANDEPTID)");
                sbSql.Append(" SELECT (ID||'-'||'" + newId + "'),CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,");
                sbSql.Append(" " + newYear + ",PLANDEPT,PLANDEPTCODE,PLANSPECIATY,PLANSPECIATYCODE,PLANAREA,PLANAREACODE,");
                sbSql.Append(string.Format(" WORKNAME,REMARK,PLANLEVEL,0,PLANDEPTID FROM {0} T WHERE T.PLANYEAR='{1}' AND T.PLANDEPTCODE='{2}' ", tableName, oldYear, currUser.DeptCode));
                res.ExecuteBySql(sbSql.ToString());

                StringBuilder subsbSql = new StringBuilder();
                subsbSql.Append("INSERT INTO BIS_OBSPLANWORK(ID,CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,OBSPERSON,");
                subsbSql.Append("OBSPERSONID,RISKLEVEL,OBSNUM,OBSMONTH,PLANID,WORKNAME,REMARK,OBSNUMTEXT,OLDWORKID)");
                subsbSql.Append("SELECT (ID||'-'||'" + newId + "'),CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,OBSPERSON,");
                subsbSql.Append(string.Format("OBSPERSONID,RISKLEVEL,OBSNUM,OBSMONTH,(PLANID||'-'||'" + newId + "'),WORKNAME,REMARK,OBSNUMTEXT,{0} FROM BIS_OBSPLANWORK WHERE PLANID ",strCase));
                subsbSql.Append(string.Format(@" IN(SELECT ID FROM {0} T WHERE T.PLANYEAR='{1}' AND T.PLANDEPTCODE='{2}')", tableName, oldYear, currUser.DeptCode));
                res.ExecuteBySql(subsbSql.ToString());
                res.Commit();
                return true;
            }
            catch (Exception)
            {
                res.Rollback();
                return false;
            }
        }

        /// <summary>
        /// 只修改计划月份直接同步到EHS与发布的数据
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="planfjid"></param>
        /// <returns></returns>
        public void SynchData(string planid, string planfjid) {
            ObsplanworkIService work = new ObsplanworkService();
            var workentity = work.GetEntity(planfjid);
            
            string sql = string.Format(@"SELECT * FROM BIS_OBSPLANWORK T WHERE T.PLANID LIKE'{0}%' AND T.ID LIKE '{1}%'", planid,planfjid);
            var dt = this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string update = string.Format(@"UPDATE BIS_OBSPLANWORK T  SET T.OBSMONTH='{0}' WHERE T.ID='{1}'", workentity.ObsMonth, dt.Rows[i]["id"].ToString());
                    this.BaseRepository().ExecuteBySql(update);
                }
            }
        
        }
        #endregion
    }
}
