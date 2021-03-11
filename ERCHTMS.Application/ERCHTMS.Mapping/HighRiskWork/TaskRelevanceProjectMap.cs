using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：已检查的检查项目
    /// </summary>
    public class TaskRelevanceProjectMap : EntityTypeConfiguration<TaskRelevanceProjectEntity>
    {
        public TaskRelevanceProjectMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TASKRELEVANCEPROJECT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
