using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// �� ��������Ԥ֪ѵ����
    /// </summary>
    public class RisktrainlibMap : EntityTypeConfiguration<RisktrainlibEntity>
    {
        public RisktrainlibMap()
        {
            #region ������
            //��
            this.ToTable("BIS_RISKTRAINLIB");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
