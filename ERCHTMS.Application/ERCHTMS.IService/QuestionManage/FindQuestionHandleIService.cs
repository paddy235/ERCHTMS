using ERCHTMS.Entity.QuestionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.QuestionManage
{
    /// <summary>
    /// 描 述：发现问题处理记录表
    /// </summary>
    public interface FindQuestionHandleIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<FindQuestionHandleEntity> GetList(string queryJson); 
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        FindQuestionHandleEntity GetEntity(string keyValue);
        #endregion

        #region 获取问题处理内容
        /// <summary>
        /// 获取问题处理内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetQuestionHandleTable(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, FindQuestionHandleEntity entity);
        #endregion
    }
}