using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全考核主表
    /// </summary>
    public class SafetyAssessmentMap : EntityTypeConfiguration<SafetyAssessmentEntity>
    {
        public SafetyAssessmentMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_SAFETYASSESSMENT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
