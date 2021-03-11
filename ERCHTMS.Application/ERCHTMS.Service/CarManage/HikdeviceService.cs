using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：门禁设备管理
    /// </summary>
    public class HikdeviceService : RepositoryFactory<HikdeviceEntity>, HikdeviceIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HikdeviceEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //设备名称
            if (!queryParam["DeviceName"].IsEmpty())
            {
                string DeviceName = queryParam["DeviceName"].ToString();

                pagination.conditionJson += string.Format(" and DeviceName like '%{0}%'", DeviceName);

            }
            //进出类型
            if (!queryParam["OutType"].IsEmpty())
            {
                int OutType = Convert.ToInt32(queryParam["OutType"]);

                pagination.conditionJson += string.Format(" and OutType ={0}", OutType);

            }
            //设备所属门岗
            if (!queryParam["AreaName"].IsEmpty())
            {
                string AreaName = queryParam["AreaName"].ToString();
                pagination.conditionJson += string.Format(" and AreaName like '%{0}%'", AreaName);
            }
            //设备IP
            if (!queryParam["DeviceIP"].IsEmpty())
            {
                string DeviceIP = queryParam["DeviceIP"].ToString();
                pagination.conditionJson += string.Format(" and DeviceIP like '%{0}%'", DeviceIP);
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HikdeviceEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HikdeviceEntity GetDeviceEntity(string HikID)
        {
            return this.BaseRepository().FindEntity(it => it.HikID == HikID);
        }


        /// <summary>
        ///  根据设备归属区域 ，获取该区域下所有的设备
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public List<HikdeviceEntity> GetDeviceByArea(string areaName)
        {
            return this.BaseRepository().IQueryable(x => x.AreaName.Equals(areaName) && x.DeviceType.Equals("门禁设备")).ToList();
        }

        /// <summary>
        /// 获取当前电厂所有的门禁设备区域
        /// 配置节在编码管理功能  系统管理->海康门禁设备下面
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataItemEntity> GetDeviceArea()
        {
            var db = new RepositoryFactory().BaseRepository();
            var query = from q1 in db.IQueryable<DataItemEntity>()
                        join q2 in db.IQueryable<DataItemEntity>() on q1.ParentId equals q2.ItemId
                        where q2.ItemName == "海康门禁设备" orderby q1.SortCode ascending
                        select new { q1.ItemId, q1.ItemName };
            var result = query.ToList();
          var data=  result.Select(x => {
                return new DataItemEntity()
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName
                };
            });
            return data;
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
        public void SaveForm(string keyValue, HikdeviceEntity entity)
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

        #endregion
    }
}
