using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� ������λ���¼��
    /// </summary>
    public class LocationMap : EntityTypeConfiguration<LocationEntity>
    {
        public LocationMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LOCATION");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
