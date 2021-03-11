using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// 描 述：定位点记录表
    /// </summary>
    public class LocationBLL
    {
        private LocationIService service = new LocationService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LocationEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LocationEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据时间段获取定位记录
        /// </summary>
        /// <param name="lableid"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public List<LocationEntity> GetLocation(string lableid, DateTime st, DateTime et)
        {
            return service.GetLocation(lableid, st, et);
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
        public void SaveForm(string keyValue, LocationEntity entity)
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
        /// 批量新增数据
        /// </summary>
        /// <param name="entityList"></param>
        public bool Insert(List<LocationEntity> entityList)
        {
            try
            {
                return service.Insert(entityList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
