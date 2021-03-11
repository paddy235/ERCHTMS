using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：起重吊装作业安全措施
    /// </summary>
    public class LifthoistsafetyMap : EntityTypeConfiguration<LifthoistsafetyEntity>
    {
        public LifthoistsafetyMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LIFTHOISTSAFETY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
