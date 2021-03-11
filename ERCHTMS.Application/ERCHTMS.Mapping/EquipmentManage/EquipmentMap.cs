using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// �� ������ͨ�豸������Ϣ��
    /// </summary>
    public class EquipmentMap : EntityTypeConfiguration<EquipmentEntity>
    {
        public EquipmentMap()
        {
            #region ������
            //��
            this.ToTable("BIS_EQUIPMENT");
            //����
            this.HasKey(t => t.Id);
            this.Ignore(t => t.AutoId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
