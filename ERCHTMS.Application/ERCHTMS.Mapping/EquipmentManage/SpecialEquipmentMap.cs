using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// �� ���������豸������Ϣ��
    /// </summary>
    public class SpecialEquipmentMap : EntityTypeConfiguration<SpecialEquipmentEntity>
    {
        public SpecialEquipmentMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SPECIALEQUIPMENT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
