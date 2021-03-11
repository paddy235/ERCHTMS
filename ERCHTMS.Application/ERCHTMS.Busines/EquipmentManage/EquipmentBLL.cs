using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.IService.EquipmentManage;
using ERCHTMS.Service.EquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.EquipmentManage
{
    /// <summary>
    /// 描 述：特种设备基本信息表
    /// </summary>
    public class EquipmentBLL
    {
        private EquipmentIService service = new EquipmentService();

        #region 获取数据
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<EquipmentEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public EquipmentEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        /// 通过设备id获取设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetEquipmentTable(string[] ids)
        {
            return service.GetEquipmentTable(ids);
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
        public void SaveForm(string keyValue, EquipmentEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 普通设备离场
        /// </summary>
        /// <param name="equipmentId">用户Id</param>
        /// <param name="leaveTime">离场时间</param>
        /// <returns></returns>
        public int SetLeave(string equipmentId, string leaveTime, string DepartureReason)
        {
            return service.SetLeave(equipmentId, leaveTime, DepartureReason);
        }
        #endregion
    }
}
