using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������ͬ
    /// </summary>
    public class CompactMap : EntityTypeConfiguration<CompactEntity>
    {
        public CompactMap()
        {
            #region ������
            //��
            this.ToTable("EPG_COMPACT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
