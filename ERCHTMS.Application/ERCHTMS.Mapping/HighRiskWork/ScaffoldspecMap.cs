using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּܹ����ʽ��
    /// </summary>
    public class ScaffoldspecMap : EntityTypeConfiguration<ScaffoldspecEntity>
    {
        public ScaffoldspecMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SCAFFOLDSPEC");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
