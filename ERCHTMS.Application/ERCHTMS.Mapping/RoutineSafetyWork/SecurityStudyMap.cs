using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// �� ������ȫ��̬
    /// </summary>
    public class SecurityStudyMap : EntityTypeConfiguration<SecurityStudyEntity>
    {
        public SecurityStudyMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SECURITYSTUDY");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
