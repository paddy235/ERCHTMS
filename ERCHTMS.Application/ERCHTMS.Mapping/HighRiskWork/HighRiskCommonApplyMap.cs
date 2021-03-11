using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险通用作业申请
    /// </summary>
    public class HighRiskCommonApplyMap : EntityTypeConfiguration<HighRiskCommonApplyEntity>
    {
        public HighRiskCommonApplyMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIGHRISKCOMMONAPPLY");
            //主键
            this.HasKey(t => t.Id);
            this.Ignore(t => t.DeleteFileIds);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
