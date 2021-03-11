using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监管任务
    /// </summary>
    public class TaskUrgeMap : EntityTypeConfiguration<TaskUrgeEntity>
    {
        public TaskUrgeMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TASKURGE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
