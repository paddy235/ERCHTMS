using ERCHTMS.Entity.DangerousJob;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJob
{
    public class JobSafetyCardApplyMap : EntityTypeConfiguration<JobSafetyCardApplyEntity>
    {
        public JobSafetyCardApplyMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_JOBSAFETYCARDAPPLY");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
