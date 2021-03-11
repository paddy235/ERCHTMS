using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：辨识评估计划表
    /// </summary>
    public class RiskPlanMap : EntityTypeConfiguration<RiskPlanEntity>
    {
        public RiskPlanMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_RISKPLAN");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}