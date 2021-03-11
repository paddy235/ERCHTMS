using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// 描 述：设备记录人员进出日志
    /// </summary>
    public class HikinoutlogBLL
    {
        private HikinoutlogIService service = new HikinoutlogService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HikinoutlogEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取今日全厂人数
        /// </summary>
        /// <returns></returns>
        public DataTable GetNums()
        {
            return service.GetNums();
        }

        /// <summary>
        /// 获取今日最后一条刷卡
        /// </summary>
        /// <returns></returns>
        public HikinoutlogEntity GetLastInoutLog()
        {
            return service.GetLastInoutLog();
        }

        /// <summary>
        /// 园区车辆统计
        /// </summary>
        /// <returns></returns>
        public DataTable GetCarStatistic()
        {
            return service.GetCarStatistic();
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HikinoutlogEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据用户id查询该用户进场未出厂记录
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public HikinoutlogEntity GetInUser(string UserId)
        {
            return service.GetInUser(UserId);
        }

        /// <summary>
        /// 根据设备ID
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        public HikinoutlogEntity DeviceGetLog(string HikId)
        {
            return service.DeviceGetLog(HikId);
        }

        /// <summary>
        /// 获取各个一级区域人数
        /// </summary>
        /// <returns></returns>
        public List<AreaModel> GetAccPersonNum()
        {
            return service.GetAccPersonNum();
        }

        /// <summary>
        /// 获取父节点Code获取其下所有子节点
        /// </summary>
        /// <returns></returns>
        public List<DistrictEntity> GetAreaSon(string code)
        {
            return service.GetAreaSon(code);
        }

        /// <summary>
        /// 获取各部门的人员统计数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetAllDepartment(string queryJson)
        {
            return service.GetAllDepartment(queryJson);
        }

        /// <summary>
        /// 根据部门名称加载人员数据
        /// </summary>
        /// <param name="deptName"></param>
        /// <returns></returns>
        public DataTable GetTableByDeptname(Pagination pagination, string deptName, string personName)
        {
            return service.GetTableByDeptname(pagination, deptName, personName);
        }


        /// <summary>
        /// 获取人员设置表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        public DataTable GetPersonSet(Pagination pagination, string ModuleType)
        {
            return service.GetPersonSet(pagination, ModuleType);
        }

        /// <summary>
        /// 获取车辆测速数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable Get_BIS_CARVIOLATION(Pagination pagination, string queryJson)
        {
            return service.Get_BIS_CARVIOLATION(pagination, queryJson);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 人员通道提交
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="update"></param>
        public void UserAisleSave(HikinoutlogEntity insert, HikinoutlogEntity update)
        {
            service.UserAisleSave(insert, update);
        }

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
        public void SaveForm(string keyValue, HikinoutlogEntity entity)
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

        public List<int[]> GetPersonData()
        {
            return service.GetPersonData();
        }

        public int[] GetAreaData()
        {
            return service.GetAreaData();
        }
        /// <summary>
        /// 获取设备间监控第一条数据
        /// </summary>
        /// <returns></returns>
        public HikinoutlogEntity GetFirsetData()
        {
            return service.GetFirsetData();
        }

        /// <summary>
        /// 根据hikid设备的ID获取人员进出的前五条数据
        /// </summary>
        /// <param name="hikId">设备的Id</param>
        /// <returns></returns>
        public System.Collections.IList GetTopFiveById(string hikId)
        {
            return service.GetTopFiveById(hikId);
        }

        /// <summary>
        /// 获取当天的人员进出数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetTodayCarPeopleCount()
        {
            return service.GetTodayCarPeopleCount();
        }

        /// <summary>
        /// 获取最新的车辆人员进出数据，取前五条
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetCarPeopleTopData()
        {
            return service.GetCarPeopleTopData();
        }

        /// <summary>
        /// 根据门禁点设备编号获取监控编号
        /// </summary>
        /// <param name="DoorIndexCode">门禁点设备编号</param>
        /// <returns></returns>
        public string GetCameraIndexCodeByDoorIndexCode(string DoorIndexCode)
        {
            return service.GetCameraIndexCodeByDoorIndexCode(DoorIndexCode);
        }

        /// <summary>
        /// 考勤告警
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetAttendanceWarningPageList(Pagination pagination, string queryJson)
        {
            return service.GetAttendanceWarningPageList(pagination, queryJson);
        }

        /// <summary>
        /// 连续缺勤统计
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetAbsenteeismPageList(Pagination pagination, string queryJson)
        {
            return service.GetAbsenteeismPageList(pagination, queryJson);
        }

        /// <summary>
        /// 连续缺勤统计
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetAbsenteeismPersonSet(Pagination pagination, string queryJson)
        {
            return service.GetAbsenteeismPersonSet(pagination, queryJson);
        }

        /// <summary>
        /// 批量设置新增人员不可查询门禁
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ModuleType"></param>
        public void SaveAbsenteeismPersonSet(string json, string ModuleType)
        {
            try
            {
                service.SaveAbsenteeismPersonSet(json, ModuleType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据人员设置ID删除数据
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteAbsenteeismPersonSet(string keyValue)
        {
            try
            {
                service.DeleteAbsenteeismPersonSet(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取考勤告警人员设置表数据
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetAttendanceWarningPersonSet(Pagination pagination, string queryJson)
        {
            return service.GetAttendanceWarningPersonSet(pagination, queryJson);
        }

        /// <summary>
        /// 批量设置新增人员不可查询门禁
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ModuleType"></param>
        public void SaveAttendanceWarningPersonSet(string json, string ModuleType)
        {
            try
            {
                service.SaveAttendanceWarningPersonSet(json, ModuleType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据人员设置ID删除数据
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteAttendanceWarningPersonSet(string keyValue)
        {
            try
            {
                service.DeleteAttendanceWarningPersonSet(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据用户ID修改离场状态
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateByID(string keyValue)
        {
            service.UpdateByID(keyValue);
        }

        /// <summary>
        /// 获取所有人员门禁进出统计
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTableUserRole(Pagination pagination, string queryJson)
        {
            return service.GetTableUserRole(pagination, queryJson);
        }

        /// <summary>
        /// 获取人员门禁进出详细
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTableByUserID(Pagination pagination, string queryJson)
        {
            return service.GetTableByUserID(pagination, queryJson);
        }

        /// <summary>
        /// 批量设置新增人员不可查询门禁
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ModuleType"></param>
        public void SavePersonSet(string json, string ModuleType)
        {
            try
            {
                service.SavePersonSet(json, ModuleType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据人员设置ID删除数据
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeletePersonSet(string keyValue)
        {
            try
            {
                service.DeletePersonSet(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
