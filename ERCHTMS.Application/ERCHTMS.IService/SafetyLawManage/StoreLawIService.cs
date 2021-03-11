using ERCHTMS.Entity.SafetyLawManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.SafetyLawManage
{
    /// <summary>
    /// 描 述：收藏法律法规
    /// </summary>
    public interface StoreLawIService
    {
        #region 获取数据
        /// <summary>
        /// 获取安全生产法律法规我的收藏列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageDataTable(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取安全管理制度我的收藏列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageJsonInstitution(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取安全操作规程我的收藏列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageJsonStandards(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<StoreLawEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        StoreLawEntity GetEntity(string keyValue);

        /// <summary>
        /// 根据法律id确定是否已收藏
        /// </summary>
        /// <returns></returns>
        int GetStoreBylawId(string lawid);
       
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
        void SaveForm(string keyValue, StoreLawEntity entity);
        #endregion
    }
}
