using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// �� �����û������
    /// </summary>
    public class UserGroupManageMap : EntityTypeConfiguration<UserGroupManageEntity>
    {
        public UserGroupManageMap()
        {
            #region ������
            //��
            this.ToTable("BIS_USERGROUPMANAGE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
