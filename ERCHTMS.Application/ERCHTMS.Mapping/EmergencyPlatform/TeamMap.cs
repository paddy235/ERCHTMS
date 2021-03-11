using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急任务
    /// </summary>
    public class TeamMap : EntityTypeConfiguration<TeamEntity>
    {
        public TeamMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_TEAM");
            //主键
            this.HasKey(t => t.TEAMID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
