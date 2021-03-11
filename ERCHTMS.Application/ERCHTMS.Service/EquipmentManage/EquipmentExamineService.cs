using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.IService.EquipmentManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;

namespace ERCHTMS.Service.EquipmentManage
{
    /// <summary>
    /// 描 述：特种设备检验记录
    /// </summary>
    public class EquipmentExamineService : RepositoryFactory<EquipmentExamineEntity>, EquipmentExamineIService
    {
        #region 获取数据

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<EquipmentExamineEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public EquipmentExamineEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, EquipmentExamineEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                EquipmentExamineEntity ee = this.BaseRepository().FindEntity(keyValue);
                if (ee == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    SavEquipmentEntity(entity);
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
                SavEquipmentEntity(entity);
            }
        }
        public void SavEquipmentEntity(EquipmentExamineEntity entity)
        {

            try
            {
                //更新主表下次检查时间、检测单位等
                IRepository db = new RepositoryFactory().BaseRepository();
                SpecialEquipmentEntity se = db.FindEntity<SpecialEquipmentEntity>(entity.EquipmentId);
                if (se != null)
                {
                    se.CheckDate = entity.ExamineDate;
                    se.CheckDateCycle = entity.ExaminePeriod.ToString();
                    if (!string.IsNullOrEmpty(entity.ExaminePeriod.Value.ToString()))
                    {
                        se.NextCheckDate = entity.ExamineDate.Value.AddDays(entity.ExaminePeriod.Value);
                    }
                    //检验单位
                    se.ExamineUnit = entity.ExamineUnit;
                    se.ReportNumber = entity.ReportNumber;
                    se.ExamineVerdict = entity.ExamineVerdict;
                    db.Update<SpecialEquipmentEntity>(se);
                }
            }
            catch { }
        }
        #endregion
    }
}
