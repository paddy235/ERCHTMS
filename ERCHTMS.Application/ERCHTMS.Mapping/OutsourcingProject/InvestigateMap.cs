using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� �����������
    /// </summary>
    public class InvestigateMap : EntityTypeConfiguration<InvestigateEntity>
    {
        public InvestigateMap()
        {
            #region ������
            //��
            this.ToTable("EPG_INVESTIGATE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
