using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全评价
    /// </summary>
    public class SafetyEvaluateMap : EntityTypeConfiguration<SafetyEvaluateEntity>
    {
        public SafetyEvaluateMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_SAFETYEVALUATE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
