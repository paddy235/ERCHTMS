using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// �� ������ȫ��̬
    /// </summary>
    public class SecurityDynamicsMap : EntityTypeConfiguration<SecurityDynamicsEntity>
    {
        public SecurityDynamicsMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SECURITYDYNAMICS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
