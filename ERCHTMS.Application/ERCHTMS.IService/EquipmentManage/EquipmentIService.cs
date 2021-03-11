using ERCHTMS.Entity.EquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.EquipmentManage
{
    /// <summary>
    /// 描 述：普通设备基本信息表
    /// </summary>
    public interface EquipmentIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<EquipmentEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        EquipmentEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取设备编号
        /// </summary>
        /// <param name="EquipmentNo">设备类别</param>
        /// <returns></returns>
        string GetEquipmentNo(string EquipmentNo, string orgcode);

        /// <summary> 
        /// 通过设备id获取用户列表
        /// </summary>
        /// <returns></returns>
        DataTable GetEquipmentTable(string[] ids);
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
        void SaveForm(string keyValue, EquipmentEntity entity);

        /// <summary>
        /// 普通设备离场
        /// </summary>
        /// <param name="equipmentId">用户Id</param>
        /// <param name="leaveTime">离场时间</param>
        /// <returns></returns>
        int SetLeave(string equipmentId, string leaveTime, string DepartureReason);
        #endregion
    }
}
