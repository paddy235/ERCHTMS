using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ���� ������Ϣ��
    /// </summary>
    public class SafetyassessmentpersonMap : EntityTypeConfiguration<SafetyassessmentpersonEntity>
    {
        public SafetyassessmentpersonMap()
        {
            #region ������
            //��
            this.ToTable("EPG_SAFETYASSESSMENTPERSON");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
