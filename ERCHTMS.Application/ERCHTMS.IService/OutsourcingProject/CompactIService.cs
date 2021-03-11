using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：合同
    /// </summary>
    public interface CompactIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetList(Pagination pagination, string queryJson);
        IEnumerable<CompactEntity> GetList();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DataTable GetEntity(string keyValue);
        /// <summary>
        /// 根基工程Id获取合同期限
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        DataTable GetComoactTimeByProjectId(string projectId);

        #region 获取工程下的合同信息
        /// <summary>
        /// 获取工程下的合同信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        List<CompactEntity> GetListByProjectId(string projectId);
        #endregion

        object GetCompactProtocol(string keyValue);

        object GetLastCompactProtocol(string keyValue);
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
        void SaveForm(string keyValue, CompactEntity entity);
        #endregion
    }
}
