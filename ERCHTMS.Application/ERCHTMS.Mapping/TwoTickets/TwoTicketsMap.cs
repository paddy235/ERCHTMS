using ERCHTMS.Entity.TwoTickets;
using ERCHTMS.Entity.WorkPlan;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.WorkPlan
{
    /// <summary>
    /// �� ����EHS�ƻ������
    /// </summary>
    public class TwoTicketsMap : EntityTypeConfiguration<TwoTicketsEntity>
    {
        public TwoTicketsMap()
        {
            #region ������
            //��
            this.ToTable("XSS_TWOTICKETS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
