using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ա��
    /// </summary>
    public class AptitudeinvestigatepeopleMap : EntityTypeConfiguration<AptitudeinvestigatepeopleEntity>
    {
        public AptitudeinvestigatepeopleMap()
        {
            #region ������
            //��
            this.ToTable("EPG_APTITUDEINVESTIGATEPEOPLE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
