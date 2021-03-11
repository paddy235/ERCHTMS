using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练记录步骤表
    /// </summary>
    public class DrillplanrecordstepMap : EntityTypeConfiguration<DrillplanrecordstepEntity>
    {
        public DrillplanrecordstepMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_DRILLPLANRECORDSTEP");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
