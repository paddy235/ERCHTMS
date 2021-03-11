using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：安全风险库
    /// </summary>
    public class RiskMap : EntityTypeConfiguration<RiskEntity>
    {
        public RiskMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_RISKDATABASE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}