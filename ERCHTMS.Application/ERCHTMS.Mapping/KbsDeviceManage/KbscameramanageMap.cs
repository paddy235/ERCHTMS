using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� ��������ʲ����ͷ����
    /// </summary>
    public class KbscameramanageMap : EntityTypeConfiguration<KbscameramanageEntity>
    {
        public KbscameramanageMap()
        {
            #region ������
            //��
            this.ToTable("BIS_KBSCAMERAMANAGE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
