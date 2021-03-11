using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.CarManage;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// 描 述：区域定位表
    /// </summary>
    public class ArealocationService : RepositoryFactory<ArealocationEntity>, ArealocationIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ArealocationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取所有区域(风险用)
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaColor> GetRiskTable()
        {
            string sql = string.Format(
                "select districtid,parentid,districtcode,districtname,NVL(id,'') id,pointlist,modelids,0 \"Level\",0 HtNum from bis_District dis left join bis_arealocation  area on dis.districtid=area.areaid order by SortCode");
            Repository<KbsAreaColor> inlogdb = new Repository<KbsAreaColor>(DbFactory.Base());
            List<KbsAreaColor> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }

        /// <summary>
        /// 获取隐患数量
        /// </summary>
        /// <returns></returns>
        public List<AreaHiddenCount> GetHiddenCount()
        {
            string sql = string.Format(
                "select HIDPOINT areacode,HIDPOINTNAME areaname,count(1) htcount from V_BASEHIDDENINFO t  where HIDPOINT is not null  group by HIDPOINT,HIDPOINTNAME");
            Repository<AreaHiddenCount> inlogdb = new Repository<AreaHiddenCount>(DbFactory.Base());
            List<AreaHiddenCount> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }

        /// <summary>
        /// 获取所有区域表及关联坐标
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaLocation> GetTable()
        {
            string sql = string.Format(
                @"select districtid,parentid,districtcode,districtname,NVL(id,'') id,pointlist,modelids from bis_District dis
            left join bis_arealocation  area on dis.districtid=area.areaid order by SortCode");
            Repository<KbsAreaLocation> inlogdb = new Repository<KbsAreaLocation>(DbFactory.Base());
            List<KbsAreaLocation> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }
        /// <summary>
        /// 获取所有区域表及关联坐标(一级区域)
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaLocation> GetOneLevelTable()
        {
            string sql = string.Format(
                @"select districtid,parentid,districtcode,districtname,NVL(id,'') id,pointlist,modelids from bis_District dis
            left join bis_arealocation  area on dis.districtid=area.areaid where dis.parentid='0' order by SortCode");
            Repository<KbsAreaLocation> inlogdb = new Repository<KbsAreaLocation>(DbFactory.Base());
            List<KbsAreaLocation> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ArealocationEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ArealocationEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().Update(entity);
            }
            else
            {
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
