using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ������
    /// </summary>
    public class SuppliesMap : EntityTypeConfiguration<SuppliesEntity>
    {
        public SuppliesMap()
        {
            #region ������
            //��
            this.ToTable("MAE_SUPPLIES");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
