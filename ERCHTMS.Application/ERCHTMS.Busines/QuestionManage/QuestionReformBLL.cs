using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.IService.QuestionManage;
using ERCHTMS.Service.QuestionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.QuestionManage
{
    /// <summary>
    /// 描 述：问题整改信息
    /// </summary>
    public class QuestionReformBLL
    {
        private QuestionReformIService service = new QuestionReformService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<QuestionReformEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public QuestionReformEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, QuestionReformEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public QuestionReformEntity GetEntityByBid(string questionId) 
        {
            try
            {
               return   service.GetEntityByBid(questionId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<QuestionReformEntity> GetHistoryList(string questionId) 
        {
            try
            {
                return service.GetHistoryList(questionId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}