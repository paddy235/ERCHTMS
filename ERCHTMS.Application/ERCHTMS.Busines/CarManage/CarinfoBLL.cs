using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using BSFramework.Util;
using ERCHTMS.Code;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// 描 述：车辆基础信息表
    /// </summary>
    public class CarinfoBLL
    {
        private CarinfoIService service = new CarinfoService();
        private IDataItemDetailService dataItemservice = new DataItemDetailService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarinfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 获取录入车辆的
        /// </summary>
        /// <returns></returns>
        public List<CarinfoEntity> GetGspCar()
        { 
            return service.GetGspCar();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public CarinfoEntity GetUserCar(string userid)
        {
            return service.GetUserCar(userid);
        }

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public CarinfoEntity GetBusCar(string CarNo)
        {
            return service.GetBusCar(CarNo);
        }

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public CarinfoEntity GetCar(string CarNo)
        {
            return service.GetCar(CarNo);
        }

        /// <summary>
        /// 车牌号是否有重复
        /// </summary>
        /// <param name="CarNo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetCarNoIsRepeat(string CarNo, string id)
        {
            return service.GetCarNoIsRepeat(CarNo, id);
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
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarinfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
                var data = dataItemservice.GetDataItemListByItemCode("'SocketUrl'");
                string IP = "";
                int Port = 0;
                foreach (var item in data)
                {
                    if (item.ItemName == "IP")
                    {
                        IP = item.ItemValue;
                    }
                    else if (item.ItemName == "Port")
                    {
                        Port = Convert.ToInt32(item.ItemValue);
                    }
                }
                service.RemoveForm(keyValue, IP, Port);
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
        public void SaveForm(string keyValue, CarinfoEntity entity, string pitem, string url)
        {
            try
            {
                var data = dataItemservice.GetDataItemListByItemCode("'SocketUrl'");
                string IP = "";
                int Port = 0;
                foreach (var item in data)
                {
                    if (item.ItemName == "IP")
                    {
                        IP = item.ItemValue;
                    }
                    else if (item.ItemName == "Port")
                    {
                        Port = Convert.ToInt32(item.ItemValue);
                    }
                }
                service.SaveForm(keyValue, entity, pitem, url, IP,Port);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, CarinfoEntity entity)
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

        public void CartoExamine(string keyValue, CarinfoEntity entity)
        {
            try
            {
                service.CartoExamine(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 修改海康车辆信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="OldCar"></param>
        public void UpdateHiaKangCar(CarinfoEntity entity, string OldCar)
        {
            try
            {
                service.UpdateHiaKangCar(entity, OldCar);
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion
    }
}
