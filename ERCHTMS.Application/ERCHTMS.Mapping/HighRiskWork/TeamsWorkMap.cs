using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：班组任务分配作业信息
    /// </summary>
    public class TeamsWorkMap : EntityTypeConfiguration<TeamsWorkEntity>
    {
        public TeamsWorkMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TEAMSWORK");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
