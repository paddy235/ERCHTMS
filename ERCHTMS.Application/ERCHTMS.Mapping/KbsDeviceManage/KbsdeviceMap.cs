using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� ��������ʲ�Ž�����
    /// </summary>
    public class KbsdeviceMap : EntityTypeConfiguration<KbsdeviceEntity>
    {
        public KbsdeviceMap()
        {
            #region ������
            //��
            this.ToTable("BIS_KBSDEVICE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
