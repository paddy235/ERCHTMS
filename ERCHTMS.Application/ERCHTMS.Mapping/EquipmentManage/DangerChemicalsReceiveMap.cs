using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// �� ����Σ�ջ�ѧƷ����
    /// </summary>
    public class DangerChemicalsReceiveEntityMap : EntityTypeConfiguration<DangerChemicalsReceiveEntity>
    {
        public DangerChemicalsReceiveEntityMap()
        {
            #region ������
            //��
            this.ToTable("XLD_DANGEROUSCHEMICALRECEIVE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
