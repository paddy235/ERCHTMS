using ERCHTMS.Entity.RiskDataBaseConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：安全风险管控取值配置表
    /// </summary>
    public class RiskwayconfigMap : EntityTypeConfiguration<RiskwayconfigEntity>
    {
        public RiskwayconfigMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_RISKWAYCONFIG");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
