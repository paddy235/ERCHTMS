using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// �� ���������豸�����¼
    /// </summary>
    public class EquipmentExamineMap : EntityTypeConfiguration<EquipmentExamineEntity>
    {
        public EquipmentExamineMap()
        {
            #region ������
            //��
            this.ToTable("HRS_EQUIPMENTEXAMINE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
