using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配班组
    /// </summary>
    public interface TeamsInfoIService
    {
        #region 获取数据
        /// <summary>
        /// 根据条件获取班组任务分配信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TeamsInfoEntity> GetList(string queryJson);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<TeamsInfoEntity> GetAllList(string queryJson);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TeamsInfoEntity GetEntity(string keyValue);
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
        string SaveForm(string keyValue, TeamsInfoEntity entity);
        #endregion
    }
}
