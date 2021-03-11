using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.IService.DangerousJob;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.DangerousJob
{
    /// <summary>
    /// 描 述：危险作业流程表
    /// </summary>
    public class DangerousJobFlowService : RepositoryFactory<DangerousJobFlowEntity>, DangerousJobFlowIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DangerousJobFlowEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.CreateUserId,t.jobstate,t.applyno,t.jobtype,t.jobdeptname,
                        t.jobplace,t.jobstarttime,t.realityjobstarttime,t.applyusername,t.applytime";
            pagination.p_tablename = @"BIS_JobSafetyCardApply t";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return data;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DangerousJobFlowEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取实体/链表查
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fid">反馈历史数据id</param>
        /// <returns></returns>
        public DataTable GetEntityByT(string keyValue, string fid)
        {
            string sql = string.Format(@"select t.*,t1.id as fid,t1.FinishInfo,t1.FeedbackDate,t1.SignUrl,t1.CreateDate as FCreateDate,t2.id as cid,t2.SuperviseResult,t2.SuperviseOpinion,t2.ConfirmationDate,t2.SignUrl as SignUrlT,t2.CreateDate as CCreateDate
 from BIS_SafetyWorkSupervise t left join (select * from  BIS_SafetyWorkFeedback where flag='0') t1 
on t.id=t1.superviseid left join (select * from BIS_SuperviseConfirmation where flag='0') t2 on t1.id=t2.feedbackid
where t.id='{0}'", keyValue);
            if (!string.IsNullOrEmpty(fid))
            {
                sql = string.Format(@"select t.*,t1.id as fid,t1.FinishInfo,t1.FeedbackDate,t1.SignUrl,t1.CreateDate as FCreateDate,t2.id as cid,t2.SuperviseResult,t2.SuperviseOpinion,t2.ConfirmationDate,t2.SignUrl as SignUrlT,t2.CreateDate as CCreateDate
 from BIS_SafetyWorkSupervise t left join (select * from BIS_SafetyWorkFeedback where id='{1}') t1 
on t.id=t1.superviseid left join (select * from BIS_SuperviseConfirmation  where feedbackid='{1}') t2 on t1.id=t2.feedbackid
where t.id='{0}'", keyValue, fid);
            }
            return this.BaseRepository().FindTable(sql);
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DangerousJobFlowEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
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
        #endregion
    }
}
