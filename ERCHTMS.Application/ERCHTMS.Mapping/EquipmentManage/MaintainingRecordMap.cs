using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// �� ����ά��������¼��
    /// </summary>
    public class MaintainingRecordMap : EntityTypeConfiguration<MaintainingRecordEntity>
    {
        public MaintainingRecordMap()
        {
            #region ������
            //��
            this.ToTable("BIS_MAINTAININGRECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
