using ERCHTMS.Entity.ToolEquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ToolEquipmentManage
{
    /// <summary>
    /// �� ���������߻�����Ϣ��
    /// </summary>
    public class ToolequipmentMap : EntityTypeConfiguration<ToolequipmentEntity>
    {
        public ToolequipmentMap()
        {
            #region ������
            //��
            this.ToTable("BIS_TOOLEQUIPMENT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
