using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// �� ����Σ��Դ�嵥
    /// </summary>
    public class Hisrelationhd_qdMap : EntityTypeConfiguration<Hisrelationhd_qdEntity>
    {
        public Hisrelationhd_qdMap()
        {
            #region ������
            //��
            this.ToTable("HSD_HISRELATIONHD_QD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
