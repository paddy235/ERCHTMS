using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：安全会议参会人员表
    /// </summary>
    public class ConferenceUserMap : EntityTypeConfiguration<ConferenceUserEntity>
    {
        public ConferenceUserMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CONFERENCEUSER");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
