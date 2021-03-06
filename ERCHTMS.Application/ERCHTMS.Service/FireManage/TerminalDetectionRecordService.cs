using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// 描 述：消防设施-定期检测记录
    /// </summary>
    public class TerminalDetectionRecordService : RepositoryFactory<TerminalDetectionRecordEntity>, TerminalDetectionRecordIService
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
                    pagination.conditionJson += string.Format(" and t.EquipmentId='{0}'", queryParam["EquipmentId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TerminalDetectionRecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from HRS_TERMINALDETECTIONRECORD where 1=1 " + queryJson).ToList();
            //return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TerminalDetectionRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, TerminalDetectionRecordEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                TerminalDetectionRecordEntity te = this.BaseRepository().FindEntity(keyValue);
                if (te == null)
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
        public void SaveFirefightingEntity(TerminalDetectionRecordEntity entity)
        {

            try
            {
                //更新主表定期检测时间、下次定期检测时间
                IRepository db = new RepositoryFactory().BaseRepository();
                FirefightingEntity fe = db.FindEntity<FirefightingEntity>(entity.EquipmentId);
                if (fe != null)
                {
                    fe.TerminalDetectionDate = entity.DetectionDate;
                    if (!string.IsNullOrEmpty(fe.TerminalDetectionPeriod.Value.ToString()))
                    {
                        fe.NextTerminalDetectionDate = entity.DetectionDate.Value.AddDays(fe.TerminalDetectionPeriod.Value);
                    }
                    fe.TerminalDetectionUnit = entity.DetectionUnit;
                    fe.TerminalDetectionVerdict = entity.DetectionResult;
                    db.Update<FirefightingEntity>(fe);
                }
            }
            catch { }
        }
        #endregion
    }
}
