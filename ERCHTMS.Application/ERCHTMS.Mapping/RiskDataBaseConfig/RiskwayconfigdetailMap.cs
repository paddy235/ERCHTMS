using ERCHTMS.Entity.RiskDataBaseConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：安全风险管控取值配置详情表
    /// </summary>
    public class RiskwayconfigdetailMap : EntityTypeConfiguration<RiskwayconfigdetailEntity>
    {
        public RiskwayconfigdetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_RISKWAYCONFIGDETAIL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
