using ERCHTMS.Entity.EvaluateManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.StandardSystem;

namespace ERCHTMS.IService.EvaluateManage
{
    /// <summary>
    /// 描 述：合规性评价明细
    /// </summary>
    public interface EvaluateDetailsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<EvaluateDetailsEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        EvaluateDetailsEntity GetEntity(string keyValue);
        
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
        void SaveForm(string keyValue, EvaluateDetailsEntity entity);
        #endregion
        /// <summary>
        /// 根据文件名称获取分类名称（取大分类）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string getStCategory(string str);

    }
}
