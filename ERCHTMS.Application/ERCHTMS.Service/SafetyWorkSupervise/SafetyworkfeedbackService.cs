using ERCHTMS.Entity.SafetyWorkSupervise;
using ERCHTMS.IService.SafetyWorkSupervise;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;

namespace ERCHTMS.Service.SafetyWorkSupervise
{
    /// <summary>
    /// 描 述：安全重点工作督办反馈信息
    /// </summary>
    public class SafetyworkfeedbackService : RepositoryFactory<SafetyworkfeedbackEntity>, SafetyworkfeedbackIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyworkfeedbackEntity> GetList(string queryJson)
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
            pagination.conditionJson = string.Format(@" t.superviseid='{0}' and t1.id is not null and superviseresult='1' ", queryJson);
            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.superviseid,t1.id as cid,to_char(t.feedbackdate,'yyyy-MM-dd') as feedbackdate,t.finishinfo,t.signurl,t1.superviseresult,t1.superviseopinion,t1.signurl as signurlt,to_char(t1.confirmationdate,'yyyy-MM-dd') as confirmationdate";
            pagination.p_tablename = @"BIS_SafetyWorkFeedback t left join BIS_SuperviseConfirmation t1 on t.id=t1.feedbackid";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t1.createdate";
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
        public SafetyworkfeedbackEntity GetEntity(string keyValue)
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyworkfeedbackEntity entity)
        {
            bool b = false;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    b = true;
                }
            }
            else
            {
                b = true;
            }
            if (b)
            {
                entity.Id = keyValue;
                entity.Create();
                this.BaseRepository().Insert(entity);
                Repository<SafetyworksuperviseEntity> announRes = new Repository<SafetyworksuperviseEntity>(DbFactory.Base());
                var sl = announRes.FindEntity(entity.SuperviseId);
                if (sl != null)
                {   //更新主表流程状态
                    sl.FlowState = "2";
                    announRes.Update(sl);
                }
            }
        }
        #endregion
    }
}
