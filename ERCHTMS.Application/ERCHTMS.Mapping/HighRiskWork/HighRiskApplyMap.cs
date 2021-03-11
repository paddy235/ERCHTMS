using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险作业许可申请
    /// </summary>
    public class HighRiskApplyMap : EntityTypeConfiguration<HighRiskApplyEntity>
    {
        public HighRiskApplyMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIGHRISKAPPLY");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
