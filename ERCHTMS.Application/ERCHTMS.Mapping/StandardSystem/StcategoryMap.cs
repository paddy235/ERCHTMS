using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� ������׼����
    /// </summary>
    public class StcategoryMap : EntityTypeConfiguration<StcategoryEntity>
    {
        public StcategoryMap()
        {
            #region ������
            //��
            this.ToTable("HRS_STCATEGORY");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
