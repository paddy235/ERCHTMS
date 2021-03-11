using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.TrainPlan;
using ERCHTMS.IService.TrainPlan;
using ERCHTMS.Service.BaseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.TrainPlan
{
    /// <summary>
    /// 描 述：安措计划
    /// </summary>
    public class SafeMeasureService : RepositoryFactory<SafeMeasureEntity>, ISafeMeasureService
    {
        #region 获取数据
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public SafeMeasureEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            string mode = "";
            if (queryJson.Contains("mode") && !string.IsNullOrEmpty(queryParam["mode"].ToString()))
            {
                mode = queryParam["mode"].ToString();
                if (mode.Equals("dbsx"))
                {
                    //首页代办事项
                    pagination.conditionJson += string.Format(" and t.iscommit='1' and t.isover='0' and t.FLOWDEPT='{0}'", user.DeptId);
                }
            }

            //pagination.conditionJson = " 1=1 ";
            pagination.p_kid = "t.ID";
            pagination.p_fields = @"PLANTYPE,PROJECTNAME,DEPARTMENTID,DEPARTMENTNAME,COST,to_char(PLANFINISHDATE,'yyyy-MM-dd') as PLANFINISHDATE,CHECKUSERNAME,PUBLISHSTATE,CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERNAME,ISCOMMIT,ISOVER,FLOWID,FLOWROLENAME,FLOWDEPT,PROCESSSTATE,STAUTS,'' as approveuserids,'' as approveusernames,STATE,DEPTCODE";
            pagination.p_tablename = "BIS_SAFEMEASURE t";
            if (!queryParam["type"].IsEmpty())
            {
                if (queryParam["type"].ToString() == "search")
                {
                    //时间选择
                    if (!queryParam["st"].IsEmpty())
                    {
                        string st = queryParam["st"].ToString();
                        pagination.conditionJson += string.Format(" and t.PLANFINISHDATE>=to_date('{0}','yyyy-mm-dd') ", st);
                    }
                    if (!queryParam["et"].IsEmpty())
                    {
                        string et = queryParam["et"].ToString();
                        pagination.conditionJson += string.Format(" and t.PLANFINISHDATE<= to_date('{0}','yyyy-mm-dd')", et);
                    }
                }
                else
                {
                    if (!mode.Equals("dbsx"))
                    {
                        //默认显示最新一年的数据
                        pagination.conditionJson += " and extract(year from t.CreateDate)=(select extract(year from max(CreateDate)) from  BIS_SAFEMEASURE)";
                    }
                }
            }
            if (queryJson.Contains("code") && !queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.DEPARTMENTID in (select departmentid from base_department start with encode='{0}' connect by  prior departmentid = parentid)", queryParam["code"].ToString());
            }


            if (!queryParam["flowstate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.PUBLISHSTATE='{0}'", queryParam["flowstate"].ToString());
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.PROJECTNAME like '%{0}%'", queryParam["keyword"].ToString());
            }
            if (!queryParam["showrange"].IsEmpty() && queryParam["showrange"].ToString() == "1")
            {
                pagination.conditionJson += string.Format(" and (t.CREATEUSERID='{0}' OR EXISTS (SELECT 1  FROM BIS_SAFEMEASURE_Adjustment where SafeMeasureId=t.Id and CreateUserId='{0}' union all SELECT  1  FROM EPG_APTITUDEINVESTIGATEAUDIT where to_char(APTITUDEID)=t.Id and AUDITPEOPLEID='{0}'))", user.UserId);
            }
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.PLANFINISHDATE";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "asc";
            }
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return data;
        }

        /// <summary>
        /// 根据类别、项目、责任部门、计划完成时间查重
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExistSafeMeasure(SafeMeasureEntity entity)
        {
            var expression = LinqExtensions.True<SafeMeasureEntity>();
            expression = expression.And(t => t.PlanType == entity.PlanType && t.ProjectName == entity.ProjectName && t.DepartmentName == entity.DepartmentName && t.PlanFinishDate == entity.PlanFinishDate);
            if (!string.IsNullOrEmpty(entity.Id))
            {
                expression = expression.And(t => t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }

        /// <summary>
        /// 获取流程节点
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="measureId"></param>
        /// <returns></returns>
        public DataTable GetCheckInfo(string moduleName, string measureId, string adjustId)
        {
            string sql = string.Format(@"select c.APTITUDEID,a.id,a.flowname,a.serialnum,a.autoid,a.checkdeptname,a.checkroleid,a.checkrolename,a.checkdeptid,c.AUDITPEOPLE,c.AUDITOPINION,c.AUDITDEPT, c.AUDITTIME,c.AUDITRESULT from bis_manypowercheck a 
left join EPG_APTITUDEINVESTIGATEAUDIT c on a.id=c.flowid and c.APTITUDEID='{0}' AND C.APPLYID='{1}'  where a.modulename='{2}' order by SERIALNUM", measureId, adjustId, moduleName);
            DataTable dt = this.BaseRepository().FindTable(sql);

            return dt;

        }

        public string GetExecuteDept(string flowId)
        {
            string executeDeptId = "";
            string sql = string.Format(@"SELECT CREATEUSERID FROM (SELECT CREATEUSERID FROM BIS_SAFEMEASURE_ADJUSTMENT WHERE SAFEMEASUREID='{0}' ORDER BY CREATEDATE DESC) WHERE rownum<2", flowId);
            string userid = this.BaseRepository().FindObject(sql).ToString();
            if (!string.IsNullOrEmpty(userid))
            {
                var entity = new UserService().GetEntity(userid);
                if (entity != null)
                {
                    executeDeptId = entity.DepartmentId;
                }
            }
            return executeDeptId;
        }

        /// <summary>
        /// 获取审核人账号
        /// </summary>
        /// <param name="flowRoleName"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public string GetNextStepUser(string flowRoleName, string deptId)
        {
            return new UserService().GetUserAccount(deptId, flowRoleName);
        }

        /// <summary>
        /// 获取安措计划季度数据
        /// </summary>
        /// <param name="belongYear">年</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        public DataTable GetSafeMeasureData(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string deptCode = user.DeptCode;
            var queryParam = queryJson.ToJObject();
            var belongYear = Convert.ToInt32(queryParam["belongYear"]);
            var quarter = Convert.ToInt32(queryParam["quarter"]);
            if (queryJson.Contains("deptCode") && !string.IsNullOrEmpty(queryParam["deptCode"].ToString()))
            {
                deptCode = queryParam["deptCode"].ToString();
            }
            string actionType = string.Empty;
            if (queryJson.Contains("actiontype") && !string.IsNullOrEmpty(queryParam["actiontype"].ToString()))
            {
                actionType = queryParam["actiontype"].ToString();
            }
            string sql = string.Format(@"select id,ProjectName,PublishState,stauts,to_char(PlanFinishDate,'yyyy-MM-dd') as PlanFinishDate,Cost,DepartmentName,to_char(FinishDate,'yyyy-MM-dd') FinishDate,flowrolename,flowdept,'' as approveusernames
,Fee,TempState,processstate,(select ID from (select ID from bis_safemeasure_adjustment  where safemeasureid=t.id order by CreateDate desc) where rownum<2) as AdjustId from BIS_SAFEMEASURE t where extract(year from planfinishdate)={0} and to_char(PlanFinishDate,'q')='{1}' and deptcode like '{2}%'", belongYear, quarter, deptCode);
            string keyValue = queryParam["keyValue"].ToString();
            if (actionType.Equals("detail") && !string.IsNullOrEmpty(keyValue))
            {
                //查看详情根据总结报告id带出本季度安措计划
                sql += string.Format(" and reportid='{0}'", keyValue);
            }
            else
            {
                //新增编辑 获取的都是已下发的数据
                sql += " and publishstate in ('1','2')";
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            return dt;
        }

        /// <summary>
        /// 获取反馈信息
        /// </summary>
        /// <param name="safeMeasureId"></param>
        /// <returns></returns>
        public DataTable GetFeedbackInfo(string safeMeasureId)
        {
            string sql = @"select a.FinishDate,a.Fee,b.OperateUserName,b.DepartmentName from BIS_SAFEMEASURE a inner join BIS_SAFEMEASURE_Summary b on a.ReportId=B.ID WHERE a.ID=:SafeMeasureId and b.State=1";
            DbParameter[] dbParameters = {
                 DbParameters.CreateDbParameter(":SafeMeasureId", safeMeasureId)
            };
            return this.BaseRepository().FindTable(sql, dbParameters);
        }
        #endregion

        #region 提交数据
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="list"></param>
        public void SaveForm(List<SafeMeasureEntity> list)
        {
            this.BaseRepository().Insert(list);
        }

        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void UpdateForm(string keyValue, SafeMeasureEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (this.BaseRepository().FindEntity(keyValue) != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 修改安措计划
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateSafeMeasure(SafeMeasureEntity entity)
        {
            string sql = "update BIS_SAFEMEASURE set PlanType=:PlanType,ProjectName=:ProjectName,Cost=:Cost,PlanFinishDate=:PlanFinishDate,DepartmentName=:DepartmentName,DEPARTMENTID=:DepartmentId,DeptCode=:DeptCode,CheckUserName=:CheckUserName where ID=:ID";
            DbParameter[] dbParameters = {
                DbParameters.CreateDbParameter(":PlanType", entity.PlanType),
                DbParameters.CreateDbParameter(":ProjectName", entity.ProjectName),
                DbParameters.CreateDbParameter(":Cost", entity.Cost),
                DbParameters.CreateDbParameter(":PlanFinishDate", entity.PlanFinishDate),
                DbParameters.CreateDbParameter(":DepartmentName", entity.DepartmentName),
                DbParameters.CreateDbParameter(":DepartmentId", entity.DepartmentId),
                DbParameters.CreateDbParameter(":DeptCode", entity.DeptCode),
                DbParameters.CreateDbParameter(":CheckUserName", entity.CheckUserName),
                DbParameters.CreateDbParameter(":ID", entity.Id)
            };
            this.BaseRepository().ExecuteBySql(sql, dbParameters);
        }

        /// <summary>
        /// 安措计划台账提交后回写数据
        /// </summary>
        /// <param name="postState"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ChangeFinishData(string postState, SafeMeasureEntity entity)
        {
            string sql = string.Empty;
            DbParameter[] dbParameters;
            if (postState.Equals("0"))
            {
                //暂存
                sql = "update BIS_SAFEMEASURE set TempState=:TempState,Fee=:Fee,FinishDate=:FinishDate,ReportID=:ReportID where ID=:ID";
                dbParameters = new DbParameter[]
                {
                    DbParameters.CreateDbParameter(":TempState", entity.TempState),
                    DbParameters.CreateDbParameter(":Fee", entity.Fee),
                    DbParameters.CreateDbParameter(":FinishDate", entity.FinishDate, DbType.DateTime),
                    DbParameters.CreateDbParameter(":ReportID", entity.ReportID),
                    DbParameters.CreateDbParameter(":ID", entity.Id)
                };
            }
            else if (postState.Equals("1"))
            {
                //提交
                sql = "update BIS_SAFEMEASURE set PublishState=:PublishState,TempState=:TempState,Fee=:Fee,FinishDate=:FinishDate,ReportID=:ReportID,STATE=1 where ID=:ID";
                dbParameters = new DbParameter[]
               {
                   DbParameters.CreateDbParameter(":PublishState", entity.TempState),
                    DbParameters.CreateDbParameter(":TempState", entity.TempState),
                    DbParameters.CreateDbParameter(":Fee", entity.Fee),
                    DbParameters.CreateDbParameter(":FinishDate", entity.FinishDate, DbType.DateTime),
                    DbParameters.CreateDbParameter(":ReportID", entity.ReportID),
                    DbParameters.CreateDbParameter(":ID", entity.Id)
               };
            }
            else
            {
                //删除安措报告时，清空实际费用和实际完成时间
                sql = "update BIS_SAFEMEASURE set TempState=NULL,Fee=NULL,FinishDate=NULL,PublishState=1,ReportID=NULL,State=0 where ReportID=:ReportID";
                dbParameters = new DbParameter[]
               {
                    DbParameters.CreateDbParameter(":ReportID", entity.ReportID)
               };
            }
            return this.BaseRepository().ExecuteBySql(sql, dbParameters) > 0 ? true : false;
        }

        /// <summary>
        /// 是否存在未下发的数据
        /// </summary>
        /// <returns></returns>
        public bool CheckUnPublish(string userId)
        {
            object obj = this.BaseRepository().FindObject("select count(1) from BIS_SAFEMEASURE where PublishState=0 and CreateUserId=:CreateUserId"
                , new DbParameter[] { DbParameters.CreateDbParameter(":CreateUserId", userId) });
            return Convert.ToInt32(obj) > 0 ? true : false;
        }

        /// <summary>
        /// 下发
        /// </summary>
        public void IssueData(string userId)
        {
            string sql = "update BIS_SAFEMEASURE set PublishState=1 where PublishState=0 and CreateUserId=:CreateUserId";
            this.BaseRepository().ExecuteBySql(sql, new DbParameter[] { DbParameters.CreateDbParameter(":CreateUserId", userId) });
        }

        #endregion
    }
}
