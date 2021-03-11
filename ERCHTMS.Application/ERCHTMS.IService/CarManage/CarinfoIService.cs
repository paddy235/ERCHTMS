using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：车辆基础信息表
    /// </summary>
    public interface CarinfoIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<CarinfoEntity> GetList(string queryJson);

        /// <summary>
        /// 获取录入车辆的
        /// </summary>
        /// <returns></returns>
        List<CarinfoEntity> GetGspCar();
        /// <summary>
        /// 车牌号是否有重复
        /// </summary>
        /// <param name="CarNo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool GetCarNoIsRepeat(string CarNo, string id);
        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        CarinfoEntity GetBusCar(string CarNo);

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        CarinfoEntity GetCar(string CarNo);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        CarinfoEntity GetUserCar(string userid);
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
        CarinfoEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue, string IP, int Port);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, CarinfoEntity entity, string pitem, string url, string IP, int Port);
        //void SaveForm(string keyValue, CarinfoEntity entity, string pitem, string url);
        void SaveForm(string keyValue, CarinfoEntity entity);

        void CartoExamine(string keyValue, CarinfoEntity entity);
        /// <summary>
        /// 修改海康车辆信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="NewCar"></param>
        void UpdateHiaKangCar(CarinfoEntity entity, string OldCar);

        #endregion
    }
}
