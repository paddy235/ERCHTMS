using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：高危险作业审核/审批表
    /// </summary>
    public class HighRiskCheckMap : EntityTypeConfiguration<HighRiskCheckEntity>
    {
        public HighRiskCheckMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIGHRISKCHECK");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
