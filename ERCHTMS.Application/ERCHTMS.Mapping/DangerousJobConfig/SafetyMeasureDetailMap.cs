using ERCHTMS.Entity.DangerousJobConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJobConfig
{
    /// <summary>
    /// �� ������ȫ��ʩ��������
    /// </summary>
    public class SafetyMeasureDetailMap : EntityTypeConfiguration<SafetyMeasureDetailEntity>
    {
        public SafetyMeasureDetailMap()
        {
            #region ������
            //��
            this.ToTable("DJ_SAFETYMEASUREDETAIL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
