using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� ����Ԥ�����͹���
    /// </summary>
    public class EarlywarningmanageMap : EntityTypeConfiguration<EarlywarningmanageEntity>
    {
        public EarlywarningmanageMap()
        {
            #region ������
            //��
            this.ToTable("BIS_EARLYWARNINGMANAGE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
