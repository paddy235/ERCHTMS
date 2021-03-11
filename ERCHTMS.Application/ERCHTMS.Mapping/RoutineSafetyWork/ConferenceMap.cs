using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：安全会议
    /// </summary>
    public class ConferenceMap : EntityTypeConfiguration<ConferenceEntity>
    {
        public ConferenceMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CONFERENCE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
