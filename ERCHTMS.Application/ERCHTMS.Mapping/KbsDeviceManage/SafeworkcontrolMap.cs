using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� ������ҵ�ֳ���ȫ�ܿ�
    /// </summary>
    public class SafeworkcontrolMap : EntityTypeConfiguration<SafeworkcontrolEntity>
    {
        public SafeworkcontrolMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFEWORKCONTROL");
            //����
            this.HasKey(t => t.ID);
            #endregion
             

            #region ���ù�ϵ
            #endregion
        }
    }
}
