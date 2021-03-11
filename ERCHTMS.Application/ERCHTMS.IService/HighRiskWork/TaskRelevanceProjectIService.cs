using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：已检查的检查项目
    /// </summary>
    public interface TaskRelevanceProjectIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TaskRelevanceProjectEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TaskRelevanceProjectEntity GetEntity(string keyValue);

          /// <summary>
        /// 根据监督任务获取已检查项目
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        IEnumerable<TaskRelevanceProjectEntity> GetEndCheckInfo(string superviseid);

        /// <summary>
        /// 根据检查项目id和监督任务id获取信息
        /// </summary>
        /// <returns></returns>
        TaskRelevanceProjectEntity GetCheckResultInfo(string checkprojectid, string superviseid);

         /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageDataTable(Pagination pagination);

        /// <summary>
        /// 根据监督id获取隐患信息
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        DataTable GetTaskHiddenInfo(string superviseid);
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
        void SaveForm(string keyValue, TaskRelevanceProjectEntity entity);
        #endregion
    }
}
