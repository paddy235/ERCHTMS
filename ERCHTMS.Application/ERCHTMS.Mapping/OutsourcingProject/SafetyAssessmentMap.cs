using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public class SafetyAssessmentMap : EntityTypeConfiguration<SafetyAssessmentEntity>
    {
        public SafetyAssessmentMap()
        {
            #region ������
            //��
            this.ToTable("EPG_SAFETYASSESSMENT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
