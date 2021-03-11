using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// 描 述：定位点记录表
    /// </summary>
    public class LocationService : RepositoryFactory<LocationEntity>, LocationIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LocationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LocationEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
            return BaseRepository().IQueryable(it => it.LableID == lableid && it.CreateDate >= st && it.CreateDate <= et).ToList();
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LocationEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 批量新增数据
        /// </summary>
        /// <param name="entityList"></param>
        public bool Insert(List<LocationEntity> entityList)
        {
            int num = this.BaseRepository().Insert(entityList);
            if (num > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
