using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：用户组管理
    /// </summary>
    public class UserGroupManageMap : EntityTypeConfiguration<UserGroupManageEntity>
    {
        public UserGroupManageMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_USERGROUPMANAGE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
