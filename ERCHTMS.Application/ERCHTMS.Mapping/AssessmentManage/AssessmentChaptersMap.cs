using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// �� ����������׼
    /// </summary>
    public class AssessmentChaptersMap : EntityTypeConfiguration<AssessmentChaptersEntity>
    {
        public AssessmentChaptersMap()
        {
            #region ������
            //��
            this.ToTable("BIS_ASSESSMENTCHAPTERS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
