using ERCHTMS.Entity.DangerousJobConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJobConfig
{
    /// <summary>
    /// �� ����Σ����ҵ�ּ���׼����
    /// </summary>
    public class ClassStandardConfigMap : EntityTypeConfiguration<ClassStandardConfigEntity>
    {
        public ClassStandardConfigMap()
        {
            #region ������
            //��
            this.ToTable("DJ_CLASSSTANDARDCONFIG");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
