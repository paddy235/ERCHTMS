using ERCHTMS.Entity.ComprehensiveManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ComprehensiveManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class ConceptpolicyMap : EntityTypeConfiguration<ConceptpolicyEntity>
    {
        public ConceptpolicyMap()
        {
            #region ������
            //��
            this.ToTable("HRS_CONCEPTPOLICY");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
