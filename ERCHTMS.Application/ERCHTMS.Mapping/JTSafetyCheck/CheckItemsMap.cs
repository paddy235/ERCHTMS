using ERCHTMS.Entity.JTSafetyCheck;
using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.JTSafetyCheck
{
    /// <summary>
    /// �� ��������λ��
    /// </summary>
    public class CheckItemsMap : EntityTypeConfiguration<CheckItemsEntity>
    {
        public CheckItemsMap()
        {
            #region ������
            //��
            this.ToTable("JT_CHECKITEMS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
