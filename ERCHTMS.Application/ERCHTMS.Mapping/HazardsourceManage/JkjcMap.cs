using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// �� �������
    /// </summary>
    public class JkjcMap : EntityTypeConfiguration<JkjcEntity>
    {
        public JkjcMap()
        {
            #region ������
            //��
            this.ToTable("HSD_JKJC");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
