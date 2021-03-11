using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.MatterManage;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// 描 述：拜访车辆表
    /// </summary>
    public class VisitcarBLL
    {
        private VisitcarIService service = new VisitcarService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<VisitcarEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public VisitcarEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public VisitcarEntity GetCar(string CarNo)
        {
            return service.GetCar(CarNo);
        }

        /// <summary>
        /// 根据车牌号获取此车牌今日最新拜访信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public VisitcarEntity NewGetCar(string CarNo)
        {
            return service.NewGetCar(CarNo);
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
        public List<string> GetOutCarNum()
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
        /// 初始化拜访\危化品\物料车辆
        /// </summary>
        /// <returns></returns>
        public List<CarAlgorithmEntity> IniVHOCar()
        {
            return service.IniVHOCar();
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 根据ID改变所选路线
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="LineName"></param>
        /// <param name="LineID"></param>
        public void ChangeLine(string keyValue, string LineName, string LineID)
        {
            service.ChangeLine(keyValue, LineName, LineID);
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
        public void SaveForm(string keyValue, VisitcarEntity entity)
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
        /// 保存表单随行人员及人脸图片
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveFaceUserForm(string keyValue, VisitcarEntity entity, List<CarUserFileImgEntity> userjson)
        {
            try
            {
                service.SaveFaceUserForm(keyValue, entity, userjson);
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
        public void ChangeGps(string keyValue, VisitcarEntity entity, List<PersongpsEntity> pgpslist)
        {
            service.ChangeGps(keyValue, entity, pgpslist);
        }

        /// <summary>
        /// 车辆出厂
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

        #endregion
    }
}
