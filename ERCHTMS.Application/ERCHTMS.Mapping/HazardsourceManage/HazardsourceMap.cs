using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// �� ����Σ��Դ��ʶ����
    /// </summary>
    public class HazardsourceMap : EntityTypeConfiguration<HazardsourceEntity>
    {
        public HazardsourceMap()
        {
            #region ������
            //��
            this.ToTable("HSD_HAZARDSOURCE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
