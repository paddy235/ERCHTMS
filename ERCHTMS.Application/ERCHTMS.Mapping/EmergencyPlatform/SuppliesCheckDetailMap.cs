using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ�����ʼ������
    /// </summary>
    public class SuppliesCheckDetailMap : EntityTypeConfiguration<SuppliesCheckDetailEntity>
    {
        public SuppliesCheckDetailMap()
        {
            #region ������
            //��
            this.ToTable("MAE_SUPPLIESCHECKDETAIL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
