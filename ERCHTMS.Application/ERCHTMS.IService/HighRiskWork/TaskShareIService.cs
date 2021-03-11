using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配表
    /// </summary>
    public interface TaskShareIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TaskShareEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TaskShareEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取分配任务列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetDataTable(Pagination page, string queryJson, string authType);

        /// <summary>
        /// 旁站监督统计
        /// </summary>
        /// <param name="sentity"></param>
        /// <returns></returns>
        DataTable QueryStatisticsByAction(StatisticsEntity sentity);
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
        List<PushMessageData> SaveForm(string keyValue, TaskShareEntity entity);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveOnlyShare(string keyValue, TaskShareEntity entity);
        #endregion
    }
}
