using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� �������湤������swp
    /// </summary>
    public class WrittenWorkMap : EntityTypeConfiguration<WrittenWorkEntity>
    {
        public WrittenWorkMap()
        {
            #region ������
            //��
            this.ToTable("HRS_WRITTENWORK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
