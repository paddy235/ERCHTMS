using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// 描 述：自评标准
    /// </summary>
    public class AssessmentChaptersMap : EntityTypeConfiguration<AssessmentChaptersEntity>
    {
        public AssessmentChaptersMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_ASSESSMENTCHAPTERS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
