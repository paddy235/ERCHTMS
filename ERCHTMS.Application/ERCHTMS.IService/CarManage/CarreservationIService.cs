using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：班车预约记录
    /// </summary>
    public interface CarreservationIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<CarreservationEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        CarreservationEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取当前车辆预约记录列表
        /// </summary>
        /// <returns></returns>
        DataTable GetCarReser(string userid);

        /// <summary>
        /// 预约列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        #endregion

        #region 提交数据

        /// <summary>
        /// 预约/取消预约
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cid"></param>
        /// <param name="time"></param>
        /// <param name="CarNo"></param>
        /// <param name="IsReser"></param>
        void AddReser(string userid, string cid, int time, string CarNo, int IsReser,string baseid);
        void AddDriverCarInfo(string userid, CarreservationEntity entity);
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
        void SaveForm(string keyValue, CarreservationEntity entity);
        #endregion
    }
}
