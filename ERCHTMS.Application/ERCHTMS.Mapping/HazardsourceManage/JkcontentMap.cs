using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// �� �����������
    /// </summary>
    public class JkcontentMap : EntityTypeConfiguration<JkcontentEntity>
    {
        public JkcontentMap()
        {
            #region ������
            //��
            this.ToTable("HSD_JKCONTENT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
