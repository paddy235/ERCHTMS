using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：区域定位管理
    /// </summary>
    public class AreagpsService : RepositoryFactory<AreagpsEntity>, AreagpsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AreagpsEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取所有区域表及关联坐标
        /// </summary>
        /// <returns></returns>
        public List<DistrictGps> GetTable()
        {
            string sql = string.Format(
                @"select districtid,parentid,districtcode,districtname,NVL(id,'') id,pointlist from bis_District dis
            left join BIS_AREAGPS  area on dis.districtid=area.areaid order by SortCode");
            Repository<DistrictGps> inlogdb = new Repository<DistrictGps>(DbFactory.Base());
            List<DistrictGps> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //车辆类型
            if (!queryParam["Type"].IsEmpty())
            {
                string Type = queryParam["Type"].ToString();

                pagination.conditionJson += string.Format(" and info.Type = {0}", Type);

            }

            if (!queryParam["WzStatus"].IsEmpty())
            {
                int WzStatus = Convert.ToInt32(queryParam["WzStatus"]);
                if (WzStatus == 0)
                {
                    pagination.conditionJson += string.Format(" and vi.num >0");
                }
                else
                {
                    pagination.conditionJson += string.Format(" and vi.num is null");
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AreagpsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(string keyValue, string DistrictId, string PointList)
        {

            if (!string.IsNullOrEmpty(keyValue))
            {
                Repository<AreagpsEntity> inlogdb = new Repository<AreagpsEntity>(DbFactory.Base());
                AreagpsEntity entity = inlogdb.FindEntity(keyValue);
                entity.PointList = PointList;
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                Repository<DistrictEntity> inlogdb = new Repository<DistrictEntity>(DbFactory.Base());
                DistrictEntity dis = inlogdb.FindEntity(DistrictId);

                AreagpsEntity entity = new AreagpsEntity();
                entity.AreaId = dis.DistrictID;
                entity.AreaName = dis.DistrictName;
                entity.PointList = PointList;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
