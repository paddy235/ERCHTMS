using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ���˱��׼����
    /// </summary>
    public class SafestandardMap : EntityTypeConfiguration<SafestandardEntity>
    {
        public SafestandardMap()
        {
            #region ������
            //��
            this.ToTable("EPG_SAFESTANDARD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
