using ERCHTMS.Entity.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练
    /// </summary>
    public interface IDrillplanService
    {
        #region 获取数据
        IEnumerable<DrillplanEntity> GetListForCon(Expression<Func<DrillplanEntity, bool>> condition);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        IEnumerable<DrillplanEntity> GetList(int year, string deptId, int monthStart, int monthEnd);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<DrillplanEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DrillplanEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, DrillplanEntity entity);
        #endregion
    }
}
