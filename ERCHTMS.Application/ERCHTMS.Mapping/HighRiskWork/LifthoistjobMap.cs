using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ����abc
    /// </summary>
    public class LifthoistjobMap : EntityTypeConfiguration<LifthoistjobEntity>
    {
        public LifthoistjobMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LIFTHOISTJOB");
            //����
            this.HasKey(t => t.ID);
            this.Ignore(t => t.RiskRecord);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
