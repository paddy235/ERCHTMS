using ERCHTMS.Entity.DangerousJobConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJobConfig
{
    /// <summary>
    /// �� ����Σ����ҵ��ȫ��ʩ����
    /// </summary>
    public class SafetyMeasureConfigMap : EntityTypeConfiguration<SafetyMeasureConfigEntity>
    {
        public SafetyMeasureConfigMap()
        {
            #region ������
            //��
            this.ToTable("DJ_SAFETYMEASURECONFIG");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
