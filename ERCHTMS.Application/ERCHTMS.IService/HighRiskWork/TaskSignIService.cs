using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：监督任务签到
    /// </summary>
    public interface TaskSignIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TaskSignEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TaskSignEntity GetEntity(string keyValue);


        /// <summary>
        /// 根据监督任务id获取监督签到信息
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        IEnumerable<TaskSignEntity> GetTaskSignInfo(string superviseId);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageDataTable(Pagination pagination);
       
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
        void SaveForm(string keyValue, TaskSignEntity entity);
        #endregion
    }
}
