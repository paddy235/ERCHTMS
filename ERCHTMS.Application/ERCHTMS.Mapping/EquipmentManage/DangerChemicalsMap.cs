using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// �� ����Σ�ջ�ѧƷ���
    /// </summary>
    public class DangerChemicalsEntityMap : EntityTypeConfiguration<DangerChemicalsEntity>
    {
        public DangerChemicalsEntityMap()
        {
            #region ������
            //��
            this.ToTable("XLD_DANGEROUSCHEMICAL");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
