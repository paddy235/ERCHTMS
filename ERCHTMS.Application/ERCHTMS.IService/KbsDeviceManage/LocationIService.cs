using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// 描 述：定位点记录表
    /// </summary>
    public interface LocationIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<LocationEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LocationEntity GetEntity(string keyValue);
        /// <summary>
        /// 根据时间段获取定位记录
        /// </summary>
        /// <param name="lableid"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        List<LocationEntity> GetLocation(string lableid, DateTime st, DateTime et);
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
        void SaveForm(string keyValue, LocationEntity entity);


        /// <summary>
        /// 批量新增数据
        /// </summary>
        /// <param name="entityList"></param>
        bool Insert(List<LocationEntity> entityList);

        #endregion
    }
}
