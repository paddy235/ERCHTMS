using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// �� ���������ܽ�
    /// </summary>
    public class AssessmentSumMap : EntityTypeConfiguration<AssessmentSumEntity>
    {
        public AssessmentSumMap()
        {
            #region ������
            //��
            this.ToTable("BIS_ASSESSMENTSUM");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
