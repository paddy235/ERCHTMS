using ERCHTMS.Entity.RiskDataBaseConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDataBaseConfig
{
    /// <summary>
    /// �� ������ȫ���չܿ�ȡֵ���������
    /// </summary>
    public class RiskwayconfigdetailMap : EntityTypeConfiguration<RiskwayconfigdetailEntity>
    {
        public RiskwayconfigdetailMap()
        {
            #region ������
            //��
            this.ToTable("BIS_RISKWAYCONFIGDETAIL");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
