using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� ������׼������
    /// </summary>
    public class StandardNewsMap : EntityTypeConfiguration<StandardNewsEntity>
    {
        public StandardNewsMap()
        {
            #region ������
            //��
            this.ToTable("BIS_STANDARDNEWS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
