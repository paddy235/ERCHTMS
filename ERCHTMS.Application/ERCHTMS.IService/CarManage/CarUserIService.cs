using BSFramework.Util.WebControl;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.MatterManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.CarManage
{
    public interface CarUserIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<CarUserEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        CarUserEntity GetEntity(string keyValue);

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
        CarUserEntity GetCar(string CarNo);


        /// <summary>
        /// 获得当日外来车辆数量
        /// </summary>
        /// <returns></returns>
        int GetOutCarNum();

        int GetStayApprovalRecordCount(string userid);

        /// <summary>
        /// 查询是否有重复车牌号拜访车辆/危化品车辆
        /// </summary>
        /// <param name="CarNo">车牌号</param>
        /// <param name="type">3位拜访 5为危化品</param>
        /// <returns></returns>
        bool GetVisitCf(string CarNo, int type);

        /// <summary>
        /// 获取人员与Gps关联表
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        List<PersongpsEntity> GetPersongpslist(string keyValue);

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
        void SaveForm(string keyValue, CarUserEntity entity, List<CarUserFileImgEntity> userjson);

        /// <summary>
        /// 改变GPS绑定信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        void ChangeGps(string keyValue, CarUserEntity entity, List<PersongpsEntity> pgpslist);

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

        /// <summary>
        /// 保存门岗人脸录入照片
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        void SaveFileImgForm(CarUserFileImgEntity entity);

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        List<AddJurisdictionEntity> addUserJurisdiction(string keyValue, int state, string baseurl, string Key, string Signature);

        #endregion
    }
}
