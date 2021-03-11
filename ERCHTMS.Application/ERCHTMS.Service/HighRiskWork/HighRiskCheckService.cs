using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：高危险作业审核/审批表
    /// </summary>
    public class HighRiskCheckService : RepositoryFactory<HighRiskCheckEntity>, HighRiskCheckIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HighRiskCheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HighRiskCheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据申请id获取审核信息[不包括没审的]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<HighRiskCheckEntity> GetCheckListInfo(string approveid)
        {
            var expression = LinqExtensions.True<HighRiskCheckEntity>();
            if (!string.IsNullOrEmpty(approveid))
            {
                expression = expression.And(t => t.ApproveId == approveid).And(t => t.ApproveState != "0").And(t => t.ApproveStep == "1");
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.ModifyDate).ToList();
        }

        /// <summary>
        /// 根据申请id获取审批信息[不包括没审的]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public HighRiskCheckEntity GetApproveInfo(string approveid)
        {
            var expression = LinqExtensions.True<HighRiskCheckEntity>();
            if (!string.IsNullOrEmpty(approveid))
            {
                expression = expression.And(t => t.ApproveId == approveid).And(t => t.ApproveState != "0").And(t => t.ApproveStep == "2");
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.ModifyDate).ToList().FirstOrDefault();
        }

        /// <summary>
        /// 根据申请id获取没审核的条数
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public int GetNoCheckNum(string approveid)
        {
            int num = 0;
            var expression = LinqExtensions.True<HighRiskCheckEntity>();
            if (!string.IsNullOrEmpty(approveid))
            {
                expression = expression.And(t => t.ApproveId == approveid).And(t => t.ApproveState == "0").And(t => t.ApproveStep == "1");
                num = this.BaseRepository().IQueryable(expression).ToList().Count();
            }
            return num;
        }

        /// <summary>
        /// 根据申请id和当前登录人获取审核(批)记录
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public HighRiskCheckEntity GetNeedCheck(string approveid)
        {
            var user = OperatorProvider.Provider.Current();
            var expression = LinqExtensions.True<HighRiskCheckEntity>();
            if (!string.IsNullOrEmpty(approveid))
            {
                expression = expression.And(t => t.ApproveId == approveid).And(t => t.ApprovePerson == user.UserId);
            }
            return this.BaseRepository().IQueryable(expression).ToList().FirstOrDefault();
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
        ///根据申请表id删除数据
        /// </summary>
        public int Remove(string workid)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from bis_highriskcheck  where  ApproveId='{0}'", workid));
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HighRiskCheckEntity entity)
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
