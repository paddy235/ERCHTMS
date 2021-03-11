using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：参会人员管理
    /// </summary>
    public class UserListManageMap : EntityTypeConfiguration<UserListManageEntity>
    {
        public UserListManageMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_USERLISTMANAGE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
