using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// �� ���������۷���ϸ
    /// </summary>
    public class KScoreDetailMap : EntityTypeConfiguration<KScoreDetailEntity>
    {
        public KScoreDetailMap()
        {
            #region ������
            //��
            this.ToTable("BIS_KSCOREDETAIL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
