using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// �� ���������豸��ȫ��������
    /// </summary>
    public class EquipmentTechnologyMap : EntityTypeConfiguration<EquipmentTechnologyEntity>
    {
        public EquipmentTechnologyMap()
        {
            #region ������
            //��
            this.ToTable("HRS_EQUIPMENTTECHNOLOGY");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
