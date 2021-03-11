using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：监督任务签到
    /// </summary>
    public class TaskSignMap : EntityTypeConfiguration<TaskSignEntity>
    {
        public TaskSignMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TASKSIGN");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
