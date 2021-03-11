using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：通知公告
    /// </summary>
    public class AnnouncementMap : EntityTypeConfiguration<AnnouncementEntity>
    {
        public AnnouncementMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_ANNOUNCEMENT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
