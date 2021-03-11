using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：车辆进出记录表
    /// </summary>
    public interface CarinlogIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<CarinlogEntity> GetList(string Cid);

        /// <summary>
        /// 根据车牌号获取最新进场信息
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        CarinlogEntity GetNewCarinLog(string CarNo);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        CarinlogEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DataTable GetTableChar(string year = "");

        /// <summary>
        /// 获取车辆出入场信息
        /// </summary>
        /// <param name="year"></param>
        /// <param name="cartype"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        DataTable GetLogDetail(string year, string cartype, string status);

        /// <summary>
        /// 返回当前场内车辆数量
        /// </summary>
        /// <returns></returns>
        int GetLogNum();
        #endregion

        #region 提交数据
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 添加通过记录
        /// </summary>
        /// <param name="carlog"></param>
        void AddPassLog(CarinlogEntity carlog);
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
        void SaveForm(string keyValue, CarinlogEntity entity);

        /// <summary>
        /// 获取车辆出入统计图
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        string GetLogChart(string year = "");

        /// <summary>
        /// 通过回调添加通过记录
        /// </summary>
        /// <param name="carlog"></param>
        void BackAddPassLog(CarinlogEntity carlog, string DeviceName, string imgUrl);
        int[] GetCarData();
        int Insert(CarinlogEntity carlog);

        #endregion
    }
}
