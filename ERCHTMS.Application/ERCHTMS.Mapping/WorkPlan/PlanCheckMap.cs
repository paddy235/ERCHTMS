using ERCHTMS.Entity.WorkPlan;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.WorkPlan
{
    /// <summary>
    /// �� ����EHS�ƻ������������ˣ���
    /// </summary>
    public class PlanCheckEntityMap : EntityTypeConfiguration<PlanCheckEntity>
    {
        public PlanCheckEntityMap()
        {
            #region ������
            //��
            this.ToTable("HRS_PLANCHECK");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
