using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.TrainPlan;
using ERCHTMS.IService.TrainPlan;
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
    /// 安全培训计划
    /// </summary>
    public class SafeTrainPlanService : RepositoryFactory<SafeTrainPlanEntity>, ISafeTrainPlanService
    {
        #region [获取数据]
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

            pagination.p_kid = "t.ID";
            pagination.p_fields = @"PROCESSSTATE,PROJECTNAME,TRAINCONTENT,PARTICIPANTS,TRAINDATE,to_char(last_day(TRAINDATE),'yyyy-MM-dd') LastDay,DEPARTMENTID,DEPARTMENTNAME,DEPARTMENTCODE,DUTYUSERID,DUTYUSERNAME,EXECUTEUSERID,EXECUTEUSERNAME,CREATEUSERID,CREATEUSERNAME";
            pagination.p_tablename = "BIS_SAFETRAINPLAN t";

            if ( queryJson.Contains("code") && !queryParam["code"].IsEmpty())
            {
               pagination.conditionJson += string.Format(" and t.DepartmentCode like '{0}%'", queryParam["code"].ToString());
            }
            
            if (!queryParam["type"].IsEmpty())
            {
                if (queryParam["type"].ToString() == "search")
                {
                    //时间选择
                    if (!queryParam["st"].IsEmpty())
                    {
                        string st = queryParam["st"].ToString();
                        pagination.conditionJson += string.Format(" and to_char(t.traindate,'yyyy-mm')>='{0}' ", st);
                    }
                    if (!queryParam["et"].IsEmpty())
                    {
                        string et = queryParam["et"].ToString();
                        pagination.conditionJson += string.Format(" and to_char(t.traindate,'yyyy-mm')<= '{0}' ", et);
                    }
                }
                else
                {
                    //默认显示最新一年的数据
                    pagination.conditionJson += " and extract(year from t.CreateDate)=(select extract(year from max(CreateDate)) from  BIS_SAFETRAINPLAN)";
                }
            }


            if (!queryParam["flowstate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.ProcessState='{0}'", queryParam["flowstate"].ToString());
            }

            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.PROJECTNAME like '%{0}%'", queryParam["keyword"].ToString());
            }
            if (!queryParam["showrange"].IsEmpty() && queryParam["showrange"].ToString() == "1")
            {
                pagination.conditionJson += string.Format(" and (t.CREATEUSERID='{0}' or t.FEEDBACKUSERID='{0}') ", user.UserId);
            }
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.TRAINDATE";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "asc";
            }
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return data;
        }

        /// <summary>
        /// 数据查重 根据培训项目、培训内容、培训对象、培训时间以及责任部门
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CheckDataExists(SafeTrainPlanEntity entity)
        {
            bool flag = false;
            SafeTrainPlanEntity sentity = this.BaseRepository().IQueryable()
                .Where(t => t.ProjectName.Equals(entity.ProjectName)
                    && t.TrainContent.Equals(entity.TrainContent)
                    && t.Participants.Equals(entity.Participants)
                    && t.TrainDate.Value.Year == entity.TrainDate.Value.Year
                    && t.TrainDate.Value.Month == entity.TrainDate.Value.Month).FirstOrDefault();
            if (sentity != null)
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 判断是否有待下发数据
        /// </summary>
        /// <returns></returns>
        public bool CheckUnpublishPlan(string userId)
        {
            return this.BaseRepository().IQueryable().Where(t => t.ProcessState == 0 && t.CreateUserId.Equals(userId)).Count() > 0 ? true : false;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public SafeTrainPlanEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        #endregion

        #region [提交数据]

        public void SaveForm(string keyValue,SafeTrainPlanEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (GetEntity(keyValue) != null)
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
        /// 批量保存
        /// </summary>
        /// <param name="list"></param>
        public void InsertSafeTrainPlan(List<SafeTrainPlanEntity> list)
        {
            this.BaseRepository().Insert(list);
        }

        /// <summary>
        /// 下发安全培训计划
        /// </summary>
        public void IssueData(string userId)
        {
            string sql = "update bis_safetrainplan set processstate=1 where processstate=0 and CreateUserId=:CreateUserId";
            this.BaseRepository().ExecuteBySql(sql, new DbParameter[] { DbParameters.CreateDbParameter(":CreateUserId", userId) });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        public void Remove(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        #endregion
    }
}
