using ERCHTMS.Entity.ToolEquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.ToolEquipmentManage
{
    /// <summary>
    /// 描 述：工器具基础信息表
    /// </summary>
    public interface ToolequipmentIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ToolequipmentEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ToolequipmentEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, ToolequipmentEntity entity);
        #endregion

        DataTable GetPageList(Pagination pagination, string queryJson);
        string GetEquipmentNo(string equipmentNo, string orgcode);
        string GetEquipmentTypeStat(string queryJson);

        DataTable GetToolRecordList(string keyValue);


        void SaveToolrecord(string keyValue, ToolrecordEntity entity);

        DataTable GetToolStatisticsList(string queryJson);

        object GetToolName(string tooltype);
    }
}
