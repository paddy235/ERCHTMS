using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：开收工会
    /// </summary>
    public class WorkMeetingEntityMap : EntityTypeConfiguration<WorkMeetingEntity>
    {
        public WorkMeetingEntityMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WORKMEETING");
            //主键
            this.HasKey(t => t.ID);
            this.Ignore(t => t.FILES);
            this.Ignore(t => t.DELETEFILEID);
            this.Ignore(t => t.OUTPROJECTNAME);
            this.Ignore(t => t.OUTPROJECTCODE);
            this.Ignore(t => t.MeasuresList);
            this.Ignore(t => t.ids);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
