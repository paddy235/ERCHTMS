using ERCHTMS.Entity.RiskDataBaseConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDataBaseConfig
{
    /// <summary>
    /// �� ������ȫ���չܿ����ñ�
    /// </summary>
    public class RiskdatabaseconfigMap : EntityTypeConfiguration<RiskdatabaseconfigEntity>
    {
        public RiskdatabaseconfigMap()
        {
            #region ������
            //��
            this.ToTable("BIS_RISKDATABASECONFIG");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
