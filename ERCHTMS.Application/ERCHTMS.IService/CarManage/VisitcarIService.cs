using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.MatterManage;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：拜访车辆表
    /// </summary>
    public interface VisitcarIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<VisitcarEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        VisitcarEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取门岗查询的物料及拜访信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDoorList();

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        VisitcarEntity GetCar(string CarNo);

        /// <summary>
        /// 根据车牌号获取此车牌今日最新拜访信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        VisitcarEntity NewGetCar(string CarNo);
        /// <summary>
        /// 获得当日外来车辆数量
        /// </summary>
        /// <returns></returns>
        List<string> GetOutCarNum();

        /// <summary>
        /// 查询是否有重复车牌号拜访车辆/危化品车辆
        /// </summary>
        /// <param name="CarNo">车牌号</param>
        /// <param name="type">3位拜访 5为危化品</param>
        /// <returns></returns>
        bool GetVisitCf(string CarNo, int type);

        /// <summary>
        /// 初始化拜访\危化品\物料车辆
        /// </summary>
        /// <returns></returns>
        List<CarAlgorithmEntity> IniVHOCar();
        #endregion

        #region 提交数据

        /// <summary>
        /// 根据ID改变所选路线
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="LineName"></param>
        /// <param name="LineID"></param>
        void ChangeLine(string keyValue, string LineName, string LineID);
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
        void SaveForm(string keyValue, VisitcarEntity entity);
        void SaveFaceUserForm(string keyValue, VisitcarEntity entity, List<CarUserFileImgEntity> userjson);

        /// <summary>
        /// 改变GPS绑定信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        void ChangeGps(string keyValue, VisitcarEntity entity, List<PersongpsEntity> pgpslist);

        /// <summary>
        /// 车辆出厂
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="Note"></param>
        /// <param name="type"></param>
        void CarOut(string keyValue, string Note, int type, List<PersongpsEntity> pergps);

        /// <summary>
        /// 改变GPS绑定信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        void WlChangeGps(string keyValue, OperticketmanagerEntity entity);

        #endregion
    }
}
