using ERCHTMS.Entity.SafetyWorkSupervise;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyWorkSupervise
{
    /// <summary>
    /// �� ������ȫ�ص㹤�����췴����Ϣ
    /// </summary>
    public class SafetyworkfeedbackMap : EntityTypeConfiguration<SafetyworkfeedbackEntity>
    {
        public SafetyworkfeedbackMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFETYWORKFEEDBACK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
