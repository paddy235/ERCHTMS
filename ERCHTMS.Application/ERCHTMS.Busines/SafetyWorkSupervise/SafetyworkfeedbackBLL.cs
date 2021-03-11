using ERCHTMS.Entity.SafetyWorkSupervise;
using ERCHTMS.IService.SafetyWorkSupervise;
using ERCHTMS.Service.SafetyWorkSupervise;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.JPush;

namespace ERCHTMS.Busines.SafetyWorkSupervise
{
    /// <summary>
    /// 描 述：安全重点工作督办反馈信息
    /// </summary>
    public class SafetyworkfeedbackBLL
    {
        private SafetyworkfeedbackIService service = new SafetyworkfeedbackService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyworkfeedbackEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyworkfeedbackEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyworkfeedbackEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
                SafetyworksuperviseBLL announRes = new SafetyworksuperviseBLL();
                var sl = announRes.GetEntity(entity.SuperviseId);
                UserBLL userbll = new UserBLL();
                UserEntity userEntity = userbll.GetEntity(sl.SupervisePersonId);//获取督办人用户信息
                JPushApi.PushMessage(userEntity.Account, sl.SupervisePerson, "GZDB002", "例行安全工作", sl.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
