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
    public class SpecialEquipmentBLL
    {
        private SpecialEquipmentIService service = new SpecialEquipmentService();

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
        public IEnumerable<SpecialEquipmentEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SpecialEquipmentEntity GetEntity(string keyValue)
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
        /// 获取设备类别统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentTypeStat(string queryJson, IEnumerable<SpecialEquipmentEntity> se)
        {
            return service.GetEquipmentTypeStat(queryJson, se);
        }

        /// <summary>
        /// 获取设备类别统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public object GetOperationFailureStat(string queryJson, IEnumerable<SpecialEquipmentEntity> se)
        {

            return service.GetOperationFailureStat(queryJson, se);
        }
        /// <summary>
        /// 根据Id获取特种设备或普通设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetEquimentList(string id)
        {
            return service.GetEquimentList(id);
        }

        /// <summary> 
        /// 通过设备id获取特种设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSpecialEquipmentTable(string[] ids)
        {
            return service.GetSpecialEquipmentTable(ids);
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
        public void SaveForm(string keyValue, SpecialEquipmentEntity entity)
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
        /// 特种设备离场
        /// </summary>
        /// <param name="equipmentId">用户Id</param>
        /// <param name="leaveTime">离场时间</param>
        /// <returns></returns>
        public int SetLeave(string specialequipmentId, string leaveTime, string DepartureReason)
        {
            return service.SetLeave(specialequipmentId, leaveTime, DepartureReason);
        }
        /// <summary>
        /// 特种修改检验日期
        /// </summary>
        /// <param name="equipmentId">用户Id</param>
        /// <param name="CheckDate">检验日期</param>
        /// <returns></returns>
        public int SetCheck(string specialequipmentId, string CheckDate)
        {
            return service.SetCheck(specialequipmentId, CheckDate);
        }
        #endregion

        #region 获取省级统计数据
        /// <summary>
        /// 获取省级设备类别统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentTypeStatGridForSJ(string queryJson)
        {
            return service.GetEquipmentTypeStatGridForSJ(queryJson);
        }

        /// <summary>
        /// 获取省级设备类别图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentTypeStatDataForSJ(string queryJson)
        {
            return service.GetEquipmentTypeStatDataForSJ(queryJson);
        }


        /// <summary>
        /// 获取省级隐患数量图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentHidDataForSJ(string queryJson)
        {
            return service.GetEquipmentHidDataForSJ(queryJson);
        }

        /// <summary>
        /// 获取省级隐患数量表格
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentHidGridForSJ(string queryJson)
        {
            return service.GetEquipmentHidGridForSJ(queryJson);
        }

        /// <summary>
        /// 获取省级检查次数图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentCheckDataForSJ(string queryJson)
        {
            return service.GetEquipmentCheckDataForSJ(queryJson);
        }

        /// <summary>
        /// 获取省级安全检查列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentCheckGridForSJ(string queryJson)
        {
            return service.GetEquipmentCheckGridForSJ(queryJson);
        }

        /// <summary>
        /// 获取省级运行故障图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentFailureDataForSJ(string queryJson)
        {
            return service.GetEquipmentFailureDataForSJ(queryJson);
        }

        /// <summary>
        /// 获取省级运行故障列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentFailureGridForSJ(string queryJson)
        {
            return service.GetEquipmentFailureGridForSJ(queryJson);
        }

        public DataTable GetSafetyCheckRecordForSJ(string queryJson)
        {
            return service.GetSafetyCheckRecordForSJ(queryJson);
        }
        #endregion

        #region app接口
        public DataTable SelectData(string sql)
        {
            return service.SelectData(sql);
        }
        public int UpdateData(string sql)
        {
            return service.UpdateData(sql);
        }
        #endregion
    }
}
