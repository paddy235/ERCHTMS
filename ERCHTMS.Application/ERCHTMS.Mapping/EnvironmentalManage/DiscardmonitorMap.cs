using ERCHTMS.Entity.EnvironmentalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EnvironmentalManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class DiscardmonitorMap : EntityTypeConfiguration<DiscardmonitorEntity>
    {
        public DiscardmonitorMap()
        {
            #region ������
            //��
            this.ToTable("BIS_DISCARDMONITOR");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
