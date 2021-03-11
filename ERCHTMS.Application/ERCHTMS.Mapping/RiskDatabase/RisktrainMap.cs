using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练
    /// </summary>
    public class RisktrainMap : EntityTypeConfiguration<RisktrainEntity>
    {
        public RisktrainMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_RISKTRAIN");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
