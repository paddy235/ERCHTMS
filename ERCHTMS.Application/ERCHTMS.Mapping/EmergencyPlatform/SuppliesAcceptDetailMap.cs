using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ����������������ϸ
    /// </summary>
    public class SuppliesAcceptDetailMap : EntityTypeConfiguration<SuppliesAcceptDetailEntity>
    {
        public SuppliesAcceptDetailMap()
        {
            #region ������
            //��
            this.ToTable("MAE_SUPPLIESACCEPT_DETAIL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
