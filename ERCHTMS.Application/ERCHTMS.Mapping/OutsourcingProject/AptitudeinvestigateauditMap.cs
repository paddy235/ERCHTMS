using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� �������������˱�
    /// </summary>
    public class AptitudeinvestigateauditMap : EntityTypeConfiguration<AptitudeinvestigateauditEntity>
    {
        public AptitudeinvestigateauditMap()
        {
            #region ������
            //��
            this.ToTable("EPG_APTITUDEINVESTIGATEAUDIT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
