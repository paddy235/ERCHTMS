using ERCHTMS.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.BaseManage
{
    /// <summary>
    /// �� �������������Ŀ��Ϣ
    /// </summary>
    public class ProjectMap : EntityTypeConfiguration<ProjectEntity>
    {
        public ProjectMap()
        {
            #region ������
            //��
            this.ToTable("BIS_PROJECT");
            //����
            this.HasKey(t => t.ProjectId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
