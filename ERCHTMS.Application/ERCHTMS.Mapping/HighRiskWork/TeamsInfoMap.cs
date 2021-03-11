using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配班组
    /// </summary>
    public class TeamsInfoMap : EntityTypeConfiguration<TeamsInfoEntity>
    {
        public TeamsInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TEAMSINFO");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
