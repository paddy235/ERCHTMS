using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ������Σ����ҵ���/������
    /// </summary>
    public class HighRiskCheckMap : EntityTypeConfiguration<HighRiskCheckEntity>
    {
        public HighRiskCheckMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HIGHRISKCHECK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
