using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配表
    /// </summary>
    public class TaskShareMap : EntityTypeConfiguration<TaskShareEntity>
    {
        public TaskShareMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TASKSHARE");
            //主键
            this.HasKey(t => t.Id);
            this.Ignore(t => t.WorkSpecs);
            this.Ignore(t => t.TeamSpec);
            this.Ignore(t => t.StaffSpec);
            this.Ignore(t => t.DelIds);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
