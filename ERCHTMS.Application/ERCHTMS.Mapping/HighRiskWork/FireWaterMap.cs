using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ����ʹ������ˮ
    /// </summary>
    public class FireWaterMap : EntityTypeConfiguration<FireWaterEntity>
    {
        public FireWaterMap()
        {
            #region ������
            //��
            this.ToTable("BIS_FIREWATER");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
