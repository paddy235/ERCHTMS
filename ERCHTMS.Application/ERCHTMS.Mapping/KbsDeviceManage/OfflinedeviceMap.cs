using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� �����豸���߼�¼
    /// </summary>
    public class OfflinedeviceMap : EntityTypeConfiguration<OfflinedeviceEntity>
    {
        public OfflinedeviceMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OFFLINEDEVICE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
