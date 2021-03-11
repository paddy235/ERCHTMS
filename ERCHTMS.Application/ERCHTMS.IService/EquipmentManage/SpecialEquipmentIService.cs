using ERCHTMS.Entity.EquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.EquipmentManage
{
    /// <summary>
    /// 描 述：特种设备基本信息表
    /// </summary>
    public interface SpecialEquipmentIService
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
        IEnumerable<SpecialEquipmentEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SpecialEquipmentEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取设备编号
        /// </summary>
        /// <param name="EquipmentNo">设备类别</param>
        /// <returns></returns>
        string GetEquipmentNo(string EquipmentNo, string orgcode);

        /// <summary>
        /// 获取设备类别统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetEquipmentTypeStat(string queryJson, IEnumerable<SpecialEquipmentEntity> se);

        /// <summary>
        /// 获取设备运行故障统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        object GetOperationFailureStat(string queryJson, IEnumerable<SpecialEquipmentEntity> se);
        /// <summary>
        /// 根据Id获取特种设备或普通设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetEquimentList(string id);
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
        void SaveForm(string keyValue, SpecialEquipmentEntity entity);

        /// <summary>
        /// 特种设备离场
        /// </summary>
        /// <param name="equipmentId">用户Id</param>
        /// <param name="leaveTime">离场时间</param>
        /// <returns></returns>
        int SetLeave(string specialequipmentId, string leaveTime, string DepartureReason);

        /// <summary>
        /// 特种设备批量修改检验日期
        /// </summary>
        /// <param name="specialequipmentId"></param>
        /// <param name="CheckDate"></param>
        /// <returns></returns>
        int SetCheck(string specialequipmentId, string CheckDate);
        #endregion


        #region 获取省级统计数据
        /// <summary>
        /// 获取省级设备类别统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetEquipmentTypeStatGridForSJ(string queryJson);

        /// <summary>
        /// 获取省级设备类别图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        string GetEquipmentTypeStatDataForSJ(string queryJson);

        /// <summary>
        /// 获取省级隐患数量图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        string GetEquipmentHidDataForSJ(string queryJson);

        /// <summary>
        /// 获取省级隐患数量表格
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetEquipmentHidGridForSJ(string queryJson);

        /// <summary>
        /// 获取省级安全检查列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetEquipmentCheckGridForSJ(string queryJson);


        /// <summary>
        /// 获取省级检查次数图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        string GetEquipmentCheckDataForSJ(string queryJson);


        /// <summary>
        /// 获取省级运行故障列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetEquipmentFailureGridForSJ(string queryJson);

        /// <summary>
        /// 获取省级运行故障图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        string GetEquipmentFailureDataForSJ(string queryJson);

        /// <summary>
        /// 获取省级检查次数记录
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetSafetyCheckRecordForSJ(string queryJson);

        /// <summary> 
        /// 通过设备id获取特种设备
        /// </summary>
        /// <returns></returns>
        DataTable GetSpecialEquipmentTable(string[] ids);
        #endregion
        #region app接口
        DataTable SelectData(string sql);
        int UpdateData(string sql);
        #endregion
    }
}
