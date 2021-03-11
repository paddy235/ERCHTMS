using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// 描 述：设备离线记录
    /// </summary>
    public interface OfflinedeviceIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<OfflinedeviceEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        OfflinedeviceEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取柱状图统计数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetTable(int type);

        /// <summary>
        /// 查询离线设备前几条
        /// </summary>
        /// <param name="type">设备类型 0标签 1基站 2门禁 3摄像头</param>
        /// <param name="Time">1本年 2本周</param>
        /// <param name="topNum">前几条</param>
        /// <returns></returns>
        DataTable GetOffTop(int type, int Time, int topNum);
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
        void SaveForm(string keyValue, OfflinedeviceEntity entity);
        #endregion
    }
}
