using ERCHTMS.Entity.FlowManage;
using BSFramework.Util.WebControl;
using System.Data;

namespace ERCHTMS.IService.FlowManage
{
    /// <summary>
    /// 描 述：表单管理
    /// </summary>
    public interface WFFrmMainIService
    {
        #region 获取数据
        /// <summary>
        /// 获取表单列表分页数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取表单数据ALL(用于下拉框)
        /// </summary>
        /// <returns></returns>
        DataTable GetAllList();
        /// <summary>
        /// 设置表单
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        WFFrmMainEntity GetForm(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除表单
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="entity">表单模板实体类</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        int SaveForm(string keyValue,WFFrmMainEntity entity);
        /// <summary>
        /// 更新表单模板状态（启用，停用）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="status">状态 1:启用;0.停用</param>
        void UpdateState(string keyValue, int state);
        #endregion
    }
}
