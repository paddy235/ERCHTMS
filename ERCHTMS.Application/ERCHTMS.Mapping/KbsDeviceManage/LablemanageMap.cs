using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� ������ǩ����
    /// </summary>
    public class LablemanageMap : EntityTypeConfiguration<LablemanageEntity>
    {
        public LablemanageMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LABLEMANAGE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
