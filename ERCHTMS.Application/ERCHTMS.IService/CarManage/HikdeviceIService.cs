using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：门禁设备管理
    /// </summary>
    public interface HikdeviceIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HikdeviceEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HikdeviceEntity GetEntity(string keyValue);

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HikdeviceEntity GetDeviceEntity(string HikID);

        /// <summary>
        /// 获取当前电厂所有的门禁设备区域
        /// 配置节在编码管理功能  系统管理->海康门禁设备下面
        /// </summary>
        /// <returns></returns>
         IEnumerable<DataItemEntity> GetDeviceArea();
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
        void SaveForm(string keyValue, HikdeviceEntity entity);
        List<HikdeviceEntity> GetDeviceByArea(string areaName);
        #endregion
    }
}
