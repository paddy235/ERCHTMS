using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：监督任务检查项目
    /// </summary>
    public class SideCheckProjectMap : EntityTypeConfiguration<SideCheckProjectEntity>
    {
        public SideCheckProjectMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SIDECHECKPROJECT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
