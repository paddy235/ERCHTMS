using ERCHTMS.Entity.EnvironmentalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EnvironmentalManage
{
    /// <summary>
    /// �� �����������
    /// </summary>
    public class NoisecheckMap : EntityTypeConfiguration<NoisecheckEntity>
    {
        public NoisecheckMap()
        {
            #region ������
            //��
            this.ToTable("BIS_NOISECHECK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
