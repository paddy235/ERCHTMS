using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��������¼�����
    /// </summary>
    public class DrillplanrecordstepMap : EntityTypeConfiguration<DrillplanrecordstepEntity>
    {
        public DrillplanrecordstepMap()
        {
            #region ������
            //��
            this.ToTable("MAE_DRILLPLANRECORDSTEP");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
