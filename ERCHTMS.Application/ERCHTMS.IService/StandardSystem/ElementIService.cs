using ERCHTMS.Entity.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.StandardSystem
{
    /// <summary>
    /// 描 述：相应元素表
    /// </summary>
    public interface ElementIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ElementEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ElementEntity GetEntity(string keyValue);
        /// <summary>
        /// 判断此节点下是否有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        bool IsHasChild(string parentId);
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
        void SaveForm(string keyValue, ElementEntity entity);
        #endregion
    }
}
