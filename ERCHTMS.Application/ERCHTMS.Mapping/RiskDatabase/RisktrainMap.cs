using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// �� ��������Ԥ֪ѵ��
    /// </summary>
    public class RisktrainMap : EntityTypeConfiguration<RisktrainEntity>
    {
        public RisktrainMap()
        {
            #region ������
            //��
            this.ToTable("BIS_RISKTRAIN");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
