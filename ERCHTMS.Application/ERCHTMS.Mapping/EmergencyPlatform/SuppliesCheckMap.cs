using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ�����ʼ��
    /// </summary>
    public class SuppliesCheckMap : EntityTypeConfiguration<SuppliesCheckEntity>
    {
        public SuppliesCheckMap()
        {
            #region ������
            //��
            this.ToTable("MAE_SUPPLIESCHECK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
