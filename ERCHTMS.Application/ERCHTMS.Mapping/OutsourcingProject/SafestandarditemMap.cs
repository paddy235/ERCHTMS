using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ�������ݱ�
    /// </summary>
    public class SafestandarditemMap : EntityTypeConfiguration<SafestandarditemEntity>
    {
        public SafestandarditemMap()
        {
            #region ������
            //��
            this.ToTable("EPG_SAFESTANDARDITEM");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
