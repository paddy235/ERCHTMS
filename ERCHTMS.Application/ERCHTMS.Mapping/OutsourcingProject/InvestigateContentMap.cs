using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ���������������
    /// </summary>
    public class InvestigateContentMap : EntityTypeConfiguration<InvestigateContentEntity>
    {
        public InvestigateContentMap()
        {
            #region ������
            //��
            this.ToTable("EPG_INVESTIGATECONTENT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
