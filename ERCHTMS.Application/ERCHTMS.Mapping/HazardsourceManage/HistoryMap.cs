using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// �� ������ʷ��¼
    /// </summary>
    public class HistoryMap : EntityTypeConfiguration<HistoryEntity>
    {
        public HistoryMap()
        {
            #region ������
            //��
            this.ToTable("HSD_HISTORY");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
