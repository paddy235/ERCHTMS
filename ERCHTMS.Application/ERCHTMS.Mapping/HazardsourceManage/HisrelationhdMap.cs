using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// �� ������ʷ��¼
    /// </summary>
    public class HisrelationhdMap : EntityTypeConfiguration<HisrelationhdEntity>
    {
        public HisrelationhdMap()
        {
            #region ������
            //��
            this.ToTable("HSD_HISRELATIONHD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
