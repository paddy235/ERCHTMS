using ERCHTMS.Entity.SaftProductTargetManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产责任书
    /// </summary>
    public interface SafeProductDutyBookIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafeProductDutyBookEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafeProductDutyBookEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取安全目标责任书列表
        /// </summary>
        /// <param name="ProductId">安全目标id</param>
        /// <returns></returns>
        IEnumerable<SafeProductDutyBookEntity> GetListByProductId(string productId);

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<SafeProductDutyBookEntity> GetPageList(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, SafeProductDutyBookEntity entity);
        #endregion
    }
}
