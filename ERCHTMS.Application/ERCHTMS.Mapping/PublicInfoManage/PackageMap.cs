using ERCHTMS.Entity.PublicInfoManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PublicInfoManage
{
    /// <summary>
    /// �� ����app�汾
    /// </summary>
    public class PackageMap : EntityTypeConfiguration<PackageEntity>
    {
        public PackageMap()
        {
            #region ������
            //��
            this.ToTable("BIS_PACKAGE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
