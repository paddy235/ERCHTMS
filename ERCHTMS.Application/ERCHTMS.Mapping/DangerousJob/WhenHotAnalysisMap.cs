using ERCHTMS.Entity.DangerousJob;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJob
{
    /// <summary>
    /// �� ����������ҵ������
    /// </summary>
    public class WhenHotAnalysisMap : EntityTypeConfiguration<WhenHotAnalysisEntity>
    {
        public WhenHotAnalysisMap()
        {
            #region ������
            //��
            this.ToTable("BIS_WHENHOTANALYSIS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
