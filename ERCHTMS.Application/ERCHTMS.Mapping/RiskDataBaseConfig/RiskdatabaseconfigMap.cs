using ERCHTMS.Entity.RiskDataBaseConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：安全风险管控配置表
    /// </summary>
    public class RiskdatabaseconfigMap : EntityTypeConfiguration<RiskdatabaseconfigEntity>
    {
        public RiskdatabaseconfigMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_RISKDATABASECONFIG");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
