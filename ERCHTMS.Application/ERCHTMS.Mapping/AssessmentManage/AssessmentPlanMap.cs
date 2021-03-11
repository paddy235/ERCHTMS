using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// 描 述：自评计划
    /// </summary>
    public class AssessmentPlanMap : EntityTypeConfiguration<AssessmentPlanEntity>
    {
        public AssessmentPlanMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_ASSESSMENTPLAN");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
