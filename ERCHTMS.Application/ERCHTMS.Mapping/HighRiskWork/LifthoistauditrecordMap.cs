using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ���������ҵ
    /// </summary>
    public class LifthoistauditrecordMap : EntityTypeConfiguration<LifthoistauditrecordEntity>
    {
        public LifthoistauditrecordMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LIFTHOISTAUDITRECORD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
