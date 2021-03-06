using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// 描 述：检测、维护记录
    /// </summary>
    public class DetectionRecordService : RepositoryFactory<DetectionRecordEntity>, DetectionRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //查询条件
                if (!queryParam["EquipmentId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and EquipmentId='{0}'", queryParam["EquipmentId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DetectionRecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from HRS_DETECTIONRECORD where 1=1 " + queryJson).ToList();
            //return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DetectionRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, DetectionRecordEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                DetectionRecordEntity de = this.BaseRepository().FindEntity(keyValue);
                if (de == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    SaveFirefightingEntity(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
                SaveFirefightingEntity(entity);
            }
        }
        public void SaveFirefightingEntity(DetectionRecordEntity entity) {

            try
            {
                //更新主表维保时间、下次维保时间
                IRepository db = new RepositoryFactory().BaseRepository();
                FirefightingEntity fe = db.FindEntity<FirefightingEntity>(entity.EquipmentId);
                if (fe != null)
                {
                    fe.DetectionDate = entity.DetectionDate;
                    if (!string.IsNullOrEmpty(fe.DetectionPeriod.Value.ToString()))
                    {
                        fe.NextDetectionDate = entity.DetectionDate.Value.AddDays(fe.DetectionPeriod.Value);
                    }
                    fe.DetectionVerdict = entity.Conclusion;
                    db.Update<FirefightingEntity>(fe);
                }
            }
            catch { }
        }
        #endregion
    }
}
