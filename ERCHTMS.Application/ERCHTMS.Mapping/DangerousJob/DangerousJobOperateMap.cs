using ERCHTMS.Entity.DangerousJob;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJob
{
    /// <summary>
    /// �� ������Σ��ҵ������
    /// </summary>
    public class DangerousJobOperateMap : EntityTypeConfiguration<DangerousJobOperateEntity>
    {
        public DangerousJobOperateMap()
        {
            #region ������
            //��
            this.ToTable("BIS_DANGEROUSJOBOPERATE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
