using ERCHTMS.Entity.WorkPlan;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.WorkPlan
{
    /// <summary>
    /// �� ����EHS�ƻ�������ϸ��
    /// </summary>
    public class PlanDetailsEntityMap : EntityTypeConfiguration<PlanDetailsEntity>
    {
        public PlanDetailsEntityMap()
        {
            #region ������
            //��
            this.ToTable("HRS_PLANDETAILS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
