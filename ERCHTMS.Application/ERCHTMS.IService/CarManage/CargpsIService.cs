using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：车辆GPS关联表
    /// </summary>
    public interface CargpsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<CargpsEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        CargpsEntity GetEntity(string keyValue);

        /// <summary>
        /// 查询此GPS设备是否被占用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool GetGps(string id, string gpsid);
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
        void SaveForm(string keyValue, CargpsEntity entity);
        #endregion
    }
}
