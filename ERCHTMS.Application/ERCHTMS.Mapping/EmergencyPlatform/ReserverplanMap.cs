using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��Ԥ��
    /// </summary>
    public class ReserverplanMap : EntityTypeConfiguration<ReserverplanEntity>
    {
        public ReserverplanMap()
        {
            #region ������
            //��
            this.ToTable("MAE_RESERVERPLAN");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
