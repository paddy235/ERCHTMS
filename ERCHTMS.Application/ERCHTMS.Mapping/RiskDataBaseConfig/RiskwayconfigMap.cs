using ERCHTMS.Entity.RiskDataBaseConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDataBaseConfig
{
    /// <summary>
    /// �� ������ȫ���չܿ�ȡֵ���ñ�
    /// </summary>
    public class RiskwayconfigMap : EntityTypeConfiguration<RiskwayconfigEntity>
    {
        public RiskwayconfigMap()
        {
            #region ������
            //��
            this.ToTable("BIS_RISKWAYCONFIG");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
