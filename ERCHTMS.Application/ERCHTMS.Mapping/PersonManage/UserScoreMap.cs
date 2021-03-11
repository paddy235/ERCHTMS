using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// 描 述：人员积分
    /// </summary>
    public class UserScoreMap : EntityTypeConfiguration<UserScoreEntity>
    {
        public UserScoreMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_USERSCORE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
