using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// �� ���������ƻ�
    /// </summary>
    public class AssessmentPlanMap : EntityTypeConfiguration<AssessmentPlanEntity>
    {
        public AssessmentPlanMap()
        {
            #region ������
            //��
            this.ToTable("BIS_ASSESSMENTPLAN");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
