using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练库
    /// </summary>
    public class RisktrainlibMap : EntityTypeConfiguration<RisktrainlibEntity>
    {
        public RisktrainlibMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_RISKTRAINLIB");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
