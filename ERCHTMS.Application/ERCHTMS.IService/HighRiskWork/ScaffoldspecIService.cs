using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架规格及形式表
    /// </summary>
    public interface ScaffoldspecIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="scaffoldid">查询参数</param>
        /// <returns>返回列表</returns>
        List<ScaffoldspecEntity> GetList(string scaffoldid);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ScaffoldspecEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, ScaffoldspecEntity entity);
        #endregion
    }
}
