using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SafetyEvaluateMap : EntityTypeConfiguration<SafetyEvaluateEntity>
    {
        public SafetyEvaluateMap()
        {
            #region ������
            //��
            this.ToTable("EPG_SAFETYEVALUATE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
