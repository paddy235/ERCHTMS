using ERCHTMS.Entity.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// 描 述：辨识评估计划相关联的机构和人员信息
    /// </summary>
    public interface RiskPlanDataIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<RiskPlanDataEntity> GetList(int dataType, string planId);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        RiskPlanDataEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        int Remove(string planId);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, RiskPlanDataEntity entity);
        void Save(List<RiskPlanDataEntity> list);
        #endregion
    }
}