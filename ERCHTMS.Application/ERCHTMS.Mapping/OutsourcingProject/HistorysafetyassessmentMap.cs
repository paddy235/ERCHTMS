using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������ʷ��ȫ��������
    /// </summary>
    public class HistorysafetyassessmentMap : EntityTypeConfiguration<HistorysafetyassessmentEntity>
    {
        public HistorysafetyassessmentMap()
        {
            #region ������
            //��
            this.ToTable("EPG_HISTORYSAFETYASSESSMENT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
