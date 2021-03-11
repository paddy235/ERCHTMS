using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：辨识评估计划相关联的机构和人员信息
    /// </summary>
    public class RiskPlanDataMap : EntityTypeConfiguration<RiskPlanDataEntity>
    {
        public RiskPlanDataMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_RISKPPLANDATA");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}