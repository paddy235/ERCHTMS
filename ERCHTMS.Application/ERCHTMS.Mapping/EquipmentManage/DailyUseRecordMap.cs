using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// 描 述：日常使用状况记录表
    /// </summary>
    public class DailyUseRecordMap : EntityTypeConfiguration<DailyUseRecordEntity>
    {
        public DailyUseRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_DAILYUSERECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
