using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练记录
    /// </summary>
    public class DrillplanrecordMap : EntityTypeConfiguration<DrillplanrecordEntity>
    {
        public DrillplanrecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_DRILLPLANRECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
