using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� �����ղر�׼
    /// </summary>
    public class StorestandardMap : EntityTypeConfiguration<StorestandardEntity>
    {
        public StorestandardMap()
        {
            #region ������
            //��
            this.ToTable("HRS_STORESTANDARD");
            //����
            this.HasKey(t => t.STOREID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
