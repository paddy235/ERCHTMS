using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� �����Ž��豸����
    /// </summary>
    public class HikdeviceMap : EntityTypeConfiguration<HikdeviceEntity>
    {
        public HikdeviceMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HIKDEVICE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
