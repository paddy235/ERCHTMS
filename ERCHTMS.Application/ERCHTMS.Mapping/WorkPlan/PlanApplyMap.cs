using ERCHTMS.Entity.WorkPlan;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.WorkPlan
{
    /// <summary>
    /// �� ����EHS�ƻ������
    /// </summary>
    public class PlanApplyEntityMap : EntityTypeConfiguration<PlanApplyEntity>
    {
        public PlanApplyEntityMap()
        {
            #region ������
            //��
            this.ToTable("HRS_PLANAPPLY");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
