using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ��֤��
    /// </summary>
    public class SafetyEamestMoneyMap : EntityTypeConfiguration<SafetyEamestMoneyEntity>
    {
        public SafetyEamestMoneyMap()
        {
            #region ������
            //��
            this.ToTable("EPG_SAFETYEAMESTMONEY");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
