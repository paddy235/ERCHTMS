using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� �����Ž���Ȩ�ޱ�
    /// </summary>
    public class HikaccessaurhorityMap : EntityTypeConfiguration<HikaccessaurhorityEntity>
    {
        public HikaccessaurhorityMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HIKACCESSAURHORITY");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
