using ERCHTMS.Entity.SaftProductTargetManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产目标项目
    /// </summary>
    public interface SafeProductProjectIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafeProductProjectEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafeProductProjectEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取安全目标项目列表
        /// </summary>
        /// <param name="ProductId">安全目标id</param>
        /// <returns></returns>
        IEnumerable<SafeProductProjectEntity> GetListByProductId(string productId);

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<SafeProductProjectEntity> GetPageList(Pagination pagination, string queryJson);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);

        /// <summary>
        /// 根据安全生产目标id删除安全目标项目
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        int Remove(string productId);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, SafeProductProjectEntity entity);

        #endregion
    }
}
