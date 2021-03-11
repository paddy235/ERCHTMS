using ERCHTMS.Entity.DangerousJob;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJob
{
    /// <summary>
    /// 描 述：动火作业分析表
    /// </summary>
    public class WhenHotAnalysisMap : EntityTypeConfiguration<WhenHotAnalysisEntity>
    {
        public WhenHotAnalysisMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WHENHOTANALYSIS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
