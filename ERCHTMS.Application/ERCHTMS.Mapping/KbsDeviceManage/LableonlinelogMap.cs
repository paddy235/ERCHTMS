using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� ������ǩ��������־
    /// </summary>
    public class LableonlinelogMap : EntityTypeConfiguration<LableonlinelogEntity>
    {
        public LableonlinelogMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LABLEONLINELOG");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
