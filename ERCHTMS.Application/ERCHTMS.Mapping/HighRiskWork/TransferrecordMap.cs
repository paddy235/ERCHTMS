using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ����ת����¼��
    /// </summary>
    public class TransferrecordMap : EntityTypeConfiguration<TransferrecordEntity>
    {
        public TransferrecordMap()
        {
            #region ������
            //��
            this.ToTable("BIS_TRANSFERRECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
