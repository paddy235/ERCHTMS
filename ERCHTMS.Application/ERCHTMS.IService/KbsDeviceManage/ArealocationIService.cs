using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.CarManage;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// 描 述：区域定位表
    /// </summary>
    public interface ArealocationIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ArealocationEntity> GetList(string queryJson);
        /// <summary>
        /// 获取所有区域表及关联坐标
        /// </summary>
        /// <returns></returns>
        List<KbsAreaLocation> GetTable();

        /// <summary>
        /// 获取所有区域表及关联坐标
        /// </summary>
        /// <returns></returns>
        List<KbsAreaColor> GetRiskTable();

        /// <summary>
        /// 获取隐患数量
        /// </summary>
        /// <returns></returns>
        List<AreaHiddenCount> GetHiddenCount();

        /// <summary>
        /// 获取所有区域表及关联坐标(一级区域)
        /// </summary>
        /// <returns></returns>
        List<KbsAreaLocation> GetOneLevelTable();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ArealocationEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, ArealocationEntity entity);
        #endregion
    }
}
