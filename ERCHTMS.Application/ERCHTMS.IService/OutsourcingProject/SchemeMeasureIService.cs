using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：方案措施管理
    /// </summary>
    public interface SchemeMeasureIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SchemeMeasureEntity GetEntity(string keyValue);
        IEnumerable<SchemeMeasureEntity> GetList();

                /// <summary>
        /// 获取外包及三措两案相关信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetObjectByKeyValue(string keyValue);

        /// <summary>
        /// 获取最近一次审核通过的三措两岸的信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        SchemeMeasureEntity GetSchemeMeasureListByOutengineerId(string keyValue);

        /// <summary>
        /// 获取历史外包及三措两案相关信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetHistoryObjectByKeyValue(string keyValue); 
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
        void SaveForm(string keyValue, SchemeMeasureEntity entity);
        #endregion
    }
}
