using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ�����ʿ��
    /// </summary>
    public class SuppliesfactoryMap : EntityTypeConfiguration<SuppliesfactoryEntity>
    {
        public SuppliesfactoryMap()
        {
            #region ������
            //��
            this.ToTable("MAE_SUPPLIESFACTORY");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
