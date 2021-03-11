using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架验收项目
    /// </summary>
    public class ScaffoldprojectMap : EntityTypeConfiguration<ScaffoldprojectEntity>
    {
        public ScaffoldprojectMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SCAFFOLDPROJECT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
