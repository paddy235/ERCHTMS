using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� ������ӦԪ�ر�
    /// </summary>
    public class ElementMap : EntityTypeConfiguration<ElementEntity>
    {
        public ElementMap()
        {
            #region ������
            //��
            this.ToTable("HRS_ELEMENT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
