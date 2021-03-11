using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.IService.QuestionManage;
using ERCHTMS.Service.QuestionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.QuestionManage
{
    /// <summary>
    /// 描 述：发现问题处理记录表
    /// </summary>
    public class FindQuestionHandleBLL
    {
        private FindQuestionHandleIService service = new FindQuestionHandleService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FindQuestionHandleEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FindQuestionHandleEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion


        #region 获取问题处理内容
        /// <summary>
        /// 获取问题处理内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetQuestionHandleTable(string keyValue)
        {
            try
            {
                return service.GetQuestionHandleTable(keyValue);
            }
            catch (Exception)
            {
                
                throw;
            }
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
        public void SaveForm(string keyValue, FindQuestionHandleEntity entity)
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
    }
}