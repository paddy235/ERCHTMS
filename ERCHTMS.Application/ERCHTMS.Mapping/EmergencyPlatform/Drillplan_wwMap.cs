using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ������ί��λӦ��Ԥ��
    /// </summary>
    public class Drillplan_wwMap : EntityTypeConfiguration<Drillplan_wwEntity>
    {
        public Drillplan_wwMap()
        {
            #region ������
            //��
            this.ToTable("MAE_DRILLPLAN_WW");
            //����
            this.HasKey(t => t.ID);
            #endregion

              #region ���ù�ϵ
            #endregion
        }
    }
}
