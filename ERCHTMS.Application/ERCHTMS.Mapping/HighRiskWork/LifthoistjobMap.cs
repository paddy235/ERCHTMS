using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：abc
    /// </summary>
    public class LifthoistjobMap : EntityTypeConfiguration<LifthoistjobEntity>
    {
        public LifthoistjobMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LIFTHOISTJOB");
            //主键
            this.HasKey(t => t.ID);
            this.Ignore(t => t.RiskRecord);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
