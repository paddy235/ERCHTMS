using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �������֤
    /// </summary>
    public class LifthoistcertMap : EntityTypeConfiguration<LifthoistcertEntity>
    {
        public LifthoistcertMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LIFTHOISTCERT");
            //����
            this.HasKey(t => t.ID);
            this.Ignore(t => t.safetys);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
