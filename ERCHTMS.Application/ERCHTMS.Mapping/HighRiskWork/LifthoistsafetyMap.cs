using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �������ص�װ��ҵ��ȫ��ʩ
    /// </summary>
    public class LifthoistsafetyMap : EntityTypeConfiguration<LifthoistsafetyEntity>
    {
        public LifthoistsafetyMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LIFTHOISTSAFETY");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
