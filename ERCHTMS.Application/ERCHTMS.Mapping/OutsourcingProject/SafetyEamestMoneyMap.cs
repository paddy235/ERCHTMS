using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全保证金
    /// </summary>
    public class SafetyEamestMoneyMap : EntityTypeConfiguration<SafetyEamestMoneyEntity>
    {
        public SafetyEamestMoneyMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_SAFETYEAMESTMONEY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
