using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ����������ʩ����
    /// </summary>
    public class ProjectFilesMap : EntityTypeConfiguration<ProjectFilesEntity>
    {
        public ProjectFilesMap()
        {
            #region ������
            //��
            this.ToTable("");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
