using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// �� �����Ǽǽ���
    /// </summary>
    public class HdjdMap : EntityTypeConfiguration<HdjdEntity>
    {
        public HdjdMap()
        {
            #region ������
            //��
            this.ToTable("HSD_HDJD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
