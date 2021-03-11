using ERCHTMS.Entity.RiskDataBaseConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：工作任务清单说明表
    /// </summary>
    public class WorkfileMap : EntityTypeConfiguration<WorkfileEntity>
    {
        public WorkfileMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WORKFILE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
