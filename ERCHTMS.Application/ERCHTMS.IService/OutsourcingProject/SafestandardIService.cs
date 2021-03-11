using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全考核标标准分类
    /// </summary>
    public interface SafestandardIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafestandardEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafestandardEntity GetEntity(string keyValue);

        /// <summary>
        /// 判断节点下有无子节点数据
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
        void SaveForm(string keyValue, SafestandardEntity entity);

        /// <summary>
        /// 导入安全考核标准库
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <param name="three"></param>
        /// <param name="four"></param>
        /// <param name="five"></param>
        /// <param name="content"></param>
        /// <param name="require"></param>
        /// <param name="norm"></param>
        /// <returns></returns>
        string Save(string one, string two, string three, string four, string five, string content, string require, string norm);
        #endregion
    }
}
