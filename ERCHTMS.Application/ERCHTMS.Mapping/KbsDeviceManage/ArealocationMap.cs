using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� ��������λ��
    /// </summary>
    public class ArealocationMap : EntityTypeConfiguration<ArealocationEntity>
    {
        public ArealocationMap()
        {
            #region ������
            //��
            this.ToTable("BIS_AREALOCATION");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
