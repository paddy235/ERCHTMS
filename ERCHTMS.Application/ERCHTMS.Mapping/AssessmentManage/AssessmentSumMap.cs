using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// 描 述：自评总结
    /// </summary>
    public class AssessmentSumMap : EntityTypeConfiguration<AssessmentSumEntity>
    {
        public AssessmentSumMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_ASSESSMENTSUM");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
