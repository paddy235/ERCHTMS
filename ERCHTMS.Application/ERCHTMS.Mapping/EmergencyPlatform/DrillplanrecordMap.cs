using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��������¼
    /// </summary>
    public class DrillplanrecordMap : EntityTypeConfiguration<DrillplanrecordEntity>
    {
        public DrillplanrecordMap()
        {
            #region ������
            //��
            this.ToTable("MAE_DRILLPLANRECORD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
