using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// �� �����������ֱ�׼
    /// </summary>
    public class AssessmentStandardMap : EntityTypeConfiguration<AssessmentStandardEntity>
    {
        public AssessmentStandardMap()
        {
            #region ������
            //��
            this.ToTable("BIS_ASSESSMENTSTANDARD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
