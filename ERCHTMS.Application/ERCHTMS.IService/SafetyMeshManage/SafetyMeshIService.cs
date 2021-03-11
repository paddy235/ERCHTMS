using ERCHTMS.Entity.SafetyMeshManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.SafetyMeshManage
{
    /// <summary>
    /// 描 述：安全网络
    /// </summary>
    public interface SafetyMeshIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafetyMeshEntity> GetList();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafetyMeshEntity GetEntity(string keyValue);
        DataTable GetTableList(string queryJson);
        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTable(string queryJson);
        IEnumerable<SafetyMeshEntity> GetListForCon(Expression<Func<SafetyMeshEntity, bool>> condition);
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
        void SaveForm(string keyValue, SafetyMeshEntity entity);
        #endregion
    }
}
