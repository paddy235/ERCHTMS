using ERCHTMS.Entity.SafetyWorkSupervise;
using ERCHTMS.IService.SafetyWorkSupervise;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.SafetyWorkSupervise
{
    /// <summary>
    /// 描 述：安全重点工作督办反馈信息
    /// </summary>
    public class SuperviseconfirmationService : RepositoryFactory<SuperviseconfirmationEntity>, SuperviseconfirmationIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SuperviseconfirmationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SuperviseconfirmationEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SuperviseconfirmationEntity entity)
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
                bool flag = false;
                entity.Id = keyValue;
                entity.Create();
                if (entity.SuperviseResult == "0")
                {
                    flag = false;
                }
                else if (entity.SuperviseResult == "1")
                {
                    flag = true;
                    entity.Flag = "1";
                }
                this.BaseRepository().Insert(entity);
                Repository<SafetyworksuperviseEntity> announRes = new Repository<SafetyworksuperviseEntity>(DbFactory.Base());
                var sl = announRes.FindEntity(entity.SuperviseId);
                if (sl != null)
                {   //更新主表流程状态
                    if (flag)
                    {
                        sl.FlowState = "1";
                    }
                    else{
                        sl.FlowState = "3";
                    }
                    announRes.Update(sl);
                }
                Repository<SafetyworkfeedbackEntity> feedback = new Repository<SafetyworkfeedbackEntity>(DbFactory.Base());
                var s2 = feedback.FindEntity(entity.FeedbackId);
                if (s2 != null)
                {   //更新反馈表数据状态(当前和历史数据)
                    if (flag)
                    {
                        s2.Flag = "1";
                    }
                    feedback.Update(s2);
                }
            }
        }
        #endregion
    }
}
