using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��������������
    /// </summary>
    public class SuppliesacceptMap : EntityTypeConfiguration<SuppliesacceptEntity>
    {
        public SuppliesacceptMap()
        {
            #region ������
            //��
            this.ToTable("MAE_SUPPLIESACCEPT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
