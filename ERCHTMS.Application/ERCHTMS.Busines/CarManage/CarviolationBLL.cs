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
    /// 描 述：违章信息类
    /// </summary>
    public class CarviolationBLL
    {
        private CarviolationIService service = new CarviolationService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarviolationEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 查询列表
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
        public CarviolationEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, CarviolationEntity entity)
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
        /// 增加一个违章接口
        /// </summary>
        public void AddViolation(string id, int type, int ViolationType, string ViolationMsg)
        {

            service.AddViolation(id, type, ViolationType, ViolationMsg);

        }

        /// <summary>
        /// 插入车辆超速信息
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(CarviolationEntity entity)
        {
            service.Insert(entity);
        }

        /// <summary>
        /// 预警中心数据
        /// </summary>
        /// <returns></returns>
        public List<CarviolationEntity> GetIndexWaring()
        {
            return service.GetIndexWaring();
        }

        public object GetIndexWaringCount()
        {
            return service.GetIndexWaringCount();
        }

        /// <summary>
        /// 获取所有的未处理的预警消息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<CarviolationEntity> GetUntreatedWaringList(Pagination pagination, string queryJson)
        {
            return service.GetUntreatedWaringList(pagination, queryJson);
        }

        #endregion
    }
}
