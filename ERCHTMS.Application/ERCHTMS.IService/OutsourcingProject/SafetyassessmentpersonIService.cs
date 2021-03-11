using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全考核 考核信息表
    /// </summary>
    public interface SafetyassessmentpersonIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafetyassessmentpersonEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafetyassessmentpersonEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetList(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, SafetyassessmentpersonEntity entity);
        /// <summary>
        /// 根据案件主键ID删除考核信息表
        /// </summary>
        /// <param name="keyValue"></param>
        void DelByKeyId(string keyValue);
        #endregion
    }
}
