using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：设备记录人员进出日志
    /// </summary>
    public interface HikinoutlogIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HikinoutlogEntity> GetList(string queryJson);
        /// <summary>
        /// 获取今日全厂人数
        /// </summary>
        /// <returns></returns>
        DataTable GetNums();
        /// <summary>
        /// 获取当天的人员进出数据
        /// </summary>
        /// <returns></returns>
        Dictionary<string, int> GetTodayCarPeopleCount();
        /// <summary>
        /// 获取最新的车辆人员进出数据，取前五条
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetCarPeopleTopData();

        /// <summary>
        /// 获取当天最后一条刷卡记录
        /// </summary>
        /// <returns></returns>
        HikinoutlogEntity GetLastInoutLog();

        /// <summary>
        /// 一号大屏车辆统计
        /// </summary>
        /// <returns></returns>
        DataTable GetCarStatistic();


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
        HikinoutlogEntity GetEntity(string keyValue);

        /// <summary>
        /// 根据用户id查询该用户进场未出厂记录
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        HikinoutlogEntity GetInUser(string UserId);

        /// <summary>
        /// 根据设备ID
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        HikinoutlogEntity DeviceGetLog(string HikId);
        /// <summary>
        /// 获取各个一级区域人数
        /// </summary>
        /// <returns></returns>
        List<AreaModel> GetAccPersonNum();

        /// <summary>
        /// 获取父节点Code获取其下所有子节点
        /// </summary>
        /// <returns></returns>
        List<DistrictEntity> GetAreaSon(string code);

        /// <summary>
        /// 获取各部门的人员统计数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetAllDepartment(string queryJson);

        /// <summary>
        /// 根据部门名称加载人员数据
        /// </summary>
        /// <param name="deptName"></param>
        /// <returns></returns>
        DataTable GetTableByDeptname(Pagination pagination, string deptName,string personName);


        /// <summary>
        /// 获取人员设置表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        DataTable GetPersonSet(Pagination pagination, string ModuleType);

        /// <summary>
        /// 获取车辆测速数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable Get_BIS_CARVIOLATION(Pagination pagination, string queryJson);
        #endregion

        #region 提交数据

        /// <summary>
        /// 人员通道提交
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="update"></param>
        void UserAisleSave(HikinoutlogEntity insert, HikinoutlogEntity update);
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
        void SaveForm(string keyValue, HikinoutlogEntity entity);
        List<int[]> GetPersonData();
        int[] GetAreaData();
        System.Collections.IList GetTopFiveById(string hikId);
        HikinoutlogEntity GetFirsetData();

        /// <summary>
        /// 根据门禁点设备编号获取监控编号
        /// </summary>
        /// <param name="DoorIndexCode">门禁点设备编号</param>
        /// <returns></returns>
        string GetCameraIndexCodeByDoorIndexCode(string DoorIndexCode);

        /// <summary>
        /// 考勤告警
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetAttendanceWarningPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 连续缺勤统计
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetAbsenteeismPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取连续缺勤统计人员设置表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        DataTable GetAbsenteeismPersonSet(Pagination pagination, string ModuleType);


        /// <summary>
        /// 批量设置新增人员不可查询门禁
        /// </summary>
        void SaveAbsenteeismPersonSet(string json, string ModuleType);


        /// <summary>
        /// 根据人员设置ID删除数据
        /// </summary>
        void DeleteAbsenteeismPersonSet(string keyValue);

        /// <summary>
        /// 获取考勤告警人员设置表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        DataTable GetAttendanceWarningPersonSet(Pagination pagination, string ModuleType);


        /// <summary>
        /// 批量设置新增人员不可查询门禁
        /// </summary>
        void SaveAttendanceWarningPersonSet(string json, string ModuleType);


        /// <summary>
        /// 根据人员设置ID删除数据
        /// </summary>
        void DeleteAttendanceWarningPersonSet(string keyValue);

        /// <summary>
        /// 根据用户ID修改离场状态
        /// </summary>
        /// <param name="keyValue"></param>
        void UpdateByID(string keyValue);

        /// <summary>
        /// 获取所有人员门禁进出统计
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTableUserRole(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取人员门禁进出详细
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTableByUserID(Pagination pagination, string queryJson);

        /// <summary>
        /// 批量设置新增人员不可查询门禁
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ModuleType"></param>
        void SavePersonSet(string json, string ModuleType);

        /// <summary>
        /// 根据人员设置ID删除数据
        /// </summary>
        /// <param name="keyValue"></param>
        void DeletePersonSet(string keyValue);
        #endregion
    }
}
