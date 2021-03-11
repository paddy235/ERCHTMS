using ERCHTMS.Entity.ToolEquipmentManage;
using ERCHTMS.IService.ToolEquipmentManage;
using ERCHTMS.Service.ToolEquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.ToolEquipmentManage
{
    /// <summary>
    /// 描 述：工器具基础信息表
    /// </summary>
    public class ToolequipmentBLL
    {
        private ToolequipmentIService service = new ToolequipmentService();

        #region 获取数据

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ToolequipmentEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ToolequipmentEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取设备编号
        /// </summary>
        /// <param name="EquipmentNo">设备类别</param>
        /// <returns></returns>
        public string GetEquipmentNo(string EquipmentNo, string orgcode)
        {
            return service.GetEquipmentNo(EquipmentNo, orgcode);
        }

        /// <summary>
        /// 获取设备类别统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentTypeStat(string queryJson)
        {
            return service.GetEquipmentTypeStat(queryJson);
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ToolequipmentEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion


        public DataTable GetToolRecordList(string keyValue)
        {
            return service.GetToolRecordList(keyValue);
        }

        public void SaveToolrecord(string keyValue, ToolrecordEntity entity)
        {
            service.SaveToolrecord(keyValue, entity);
        }

        public DataTable GetToolStatisticsList(string queryJson)
        {
            return service.GetToolStatisticsList(queryJson);
        }

        public object GetToolName(string tooltype)
        {
            try
            {
                return service.GetToolName(tooltype);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
