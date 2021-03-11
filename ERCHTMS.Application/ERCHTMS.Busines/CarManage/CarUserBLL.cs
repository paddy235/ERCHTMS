using BSFramework.Util.WebControl;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.MatterManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// 描述:拜访人员
    /// </summary>
    public class CarUserBLL
    {
        private CarUserIService service = new CarUserService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarUserEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarUserEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public CarUserEntity GetCar(string CarNo)
        {
            return service.GetCar(CarNo);
        }

        /// <summary>
        /// 获取门岗查询的物料及拜访信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDoorList()
        {
            return service.GetDoorList();
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获得当日外来车辆数量
        /// </summary>
        /// <returns></returns>
        public int GetOutCarNum()
        {
            return service.GetOutCarNum();
        }

        /// <summary>
        /// 查询是否有重复车牌号拜访车辆/危化品车辆
        /// </summary>
        /// <param name="CarNo">车牌号</param>
        /// <param name="type">3位拜访 5为危化品</param>
        /// <returns></returns>
        public bool GetVisitCf(string CarNo, int type)
        {
            return service.GetVisitCf(CarNo, type);
        }
        /// <summary>
        /// 手机端待审批记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetStayApprovalRecordCount(string userid)
        {
            return service.GetStayApprovalRecordCount(userid);
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
        public void SaveForm(string keyValue, CarUserEntity entity, List<CarUserFileImgEntity> userjson)
        {
            try
            {
                service.SaveForm(keyValue, entity, userjson);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 添加用户出入权限
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<AddJurisdictionEntity> addUserJurisdiction(string keyValue, int state, string baseurl, string Key, string Signature)
        {
            try
            {
                return service.addUserJurisdiction(keyValue, state, baseurl, Key, Signature);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 改变GPS绑定信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void ChangeGps(string keyValue, CarUserEntity entity, List<PersongpsEntity> pgpslist)
        {
            service.ChangeGps(keyValue, entity, pgpslist);
        }

        /// <summary>
        /// 拜访人员出厂
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="Note"></param>
        /// <param name="type"></param>
        public void CarOut(string keyValue, string Note, int type, List<PersongpsEntity> pergps)
        {
            service.CarOut(keyValue, Note, type, pergps);
        }

        /// <summary>
        /// 改变GPS绑定信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void WlChangeGps(string keyValue, OperticketmanagerEntity entity)
        {
            service.WlChangeGps(keyValue, entity);
        }

        /// <summary>
        /// 获取人员与Gps关联表
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<PersongpsEntity> GetPersongpslist(string keyValue)
        {
            return service.GetPersongpslist(keyValue);
        }

        /// <summary>
        /// 保存门岗人脸录入照片
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public void SaveFileImgForm(CarUserFileImgEntity entity)
        {
            service.SaveFileImgForm(entity);
        }


        #endregion
    }
}
