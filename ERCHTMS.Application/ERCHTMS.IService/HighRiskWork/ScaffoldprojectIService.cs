using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架验收项目
    /// </summary>
    public interface ScaffoldprojectIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="scaffoldid">查询参数</param>
        /// <returns>返回列表</returns>
        List<ScaffoldprojectEntity> GetList(string scaffoldid);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ScaffoldprojectEntity GetEntity(string keyValue);


         /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ScaffoldprojectEntity> GetListByCondition(string queryJson);
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
        void SaveForm(string keyValue, ScaffoldprojectEntity entity);

        /// <summary>
        /// 跟据条件表达式删除
        /// </summary>
        /// <param name="condition"></param>
        void RemoveForm(Expression<Func<ScaffoldprojectEntity, bool>> condition);
        #endregion
    }
}
