using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：演练评估表
    /// </summary>
    public class DrillassessMap : EntityTypeConfiguration<DrillassessEntity>
    {
        public DrillassessMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_DRILLASSESS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
