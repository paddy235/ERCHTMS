using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ������
    /// </summary>
    public class DrillplanMap : EntityTypeConfiguration<DrillplanEntity>
    {
        public DrillplanMap()
        {
            #region ������
            //��
            this.ToTable("MAE_DRILLPLAN");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
