using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：危害因素车辆表
    /// </summary>
    public interface HazardouscarIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HazardouscarEntity> GetList(string queryJson);

        /// <summary>
        /// 分页查询列表
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
        HazardouscarEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取此危害因素是否配置了检查表
        /// </summary>
        /// <param name="HazardousId"></param>
        /// <returns></returns>
        bool GetHazardous(string HazardousId);

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        HazardouscarEntity GetCar(string CarNo);

        /// <summary>
        /// 获取当日危化品车辆数量
        /// </summary>
        /// <returns></returns>
        List<HazardouscarEntity> GetHazardousList(string day);
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
        void SaveForm(string keyValue, HazardouscarEntity entity);
        void SaveFaceUserForm(string keyValue, HazardouscarEntity entity, List<CarUserFileImgEntity> userjson);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void Update(string keyValue, HazardouscarEntity entity);

        /// <summary>
        /// 改变GPS绑定信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        void ChangeGps(string keyValue, HazardouscarEntity entity, List<PersongpsEntity> pgpslist);

        /// <summary>
        /// 改变危化品车辆数据状态位交接完成
        /// </summary>
        /// <param name="id"></param>
        void ChangeProcess(string id);

        #endregion
    }
}
