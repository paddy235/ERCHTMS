using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// 描 述：车辆进出记录表
    /// </summary>
    public class CarinlogBLL
    {
        private CarinlogIService service = new CarinlogService();

        #region 获取数据
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 根据车牌号获取最新进场信息
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        public CarinlogEntity GetNewCarinLog(string CarNo)
        {
            return service.GetNewCarinLog(CarNo);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarinlogEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarinlogEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取车辆出入统计图
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetLogChart(string year = "")
        {
            return service.GetLogChart(year);
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetTableChar(string year = "")
        {
            return service.GetTableChar(year);
        }

        /// <summary>
        /// 获取车辆出入场信息
        /// </summary>
        /// <param name="year"></param>
        /// <param name="cartype"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetLogDetail(string year, string cartype, string status)
        {
            return service.GetLogDetail(year, cartype, status);
        }

        /// <summary>
        /// 返回当前场内车辆数量
        /// </summary>
        /// <returns></returns>
        public int GetLogNum()
        {
            return service.GetLogNum();
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 添加通过记录
        /// </summary>
        /// <param name="carlog"></param>
        public void AddPassLog(CarinlogEntity carlog)
        {
            service.AddPassLog(carlog);
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
        public void SaveForm(string keyValue, CarinlogEntity entity)
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
        /// 通过回调添加通过记录
        /// </summary>
        /// <param name="carlog"></param>
        public void BackAddPassLog(CarinlogEntity carlog, string DeviceName, string imgUrl)
        {
            service.BackAddPassLog(carlog,DeviceName,imgUrl);
        }

        public int[] GetCarData()
        {
            return service.GetCarData();
        }

        public int Insert(CarinlogEntity carlog)
        {
            return service.Insert(carlog);
        }

        #endregion
    }
}
