using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// �� �����λ���Ա����
    /// </summary>
    public class UserListManageMap : EntityTypeConfiguration<UserListManageEntity>
    {
        public UserListManageMap()
        {
            #region ������
            //��
            this.ToTable("BIS_USERLISTMANAGE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
