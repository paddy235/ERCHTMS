using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ��������������
    /// </summary>
    public class DrillassessMap : EntityTypeConfiguration<DrillassessEntity>
    {
        public DrillassessMap()
        {
            #region ������
            //��
            this.ToTable("MAE_DRILLASSESS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
