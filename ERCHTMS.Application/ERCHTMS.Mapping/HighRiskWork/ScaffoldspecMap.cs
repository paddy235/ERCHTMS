using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架规格及形式表
    /// </summary>
    public class ScaffoldspecMap : EntityTypeConfiguration<ScaffoldspecEntity>
    {
        public ScaffoldspecMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SCAFFOLDSPEC");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
