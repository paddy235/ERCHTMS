using ERCHTMS.Entity.SafetyWorkSupervise;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyWorkSupervise
{
    /// <summary>
    /// �� ������ȫ�ص㹤������
    /// </summary>
    public class SafetyworksuperviseMap : EntityTypeConfiguration<SafetyworksuperviseEntity>
    {
        public SafetyworksuperviseMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFETYWORKSUPERVISE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
