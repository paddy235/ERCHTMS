using ERCHTMS.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CustomerManage
{
    /// <summary>
    /// �� ������Ʊ��Ϣ
    /// </summary>
    public class InvoiceMap : EntityTypeConfiguration<InvoiceEntity>
    {
        public InvoiceMap()
        {
            #region ������
            //��
            this.ToTable("CLIENT_INVOICE");
            //����
            this.HasKey(t => t.InvoiceId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
