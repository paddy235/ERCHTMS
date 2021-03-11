using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// 描 述：自评评分标准
    /// </summary>
    public class AssessmentStandardMap : EntityTypeConfiguration<AssessmentStandardEntity>
    {
        public AssessmentStandardMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_ASSESSMENTSTANDARD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
