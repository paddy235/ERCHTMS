using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：编号规则
    /// </summary>
    public class CodeRuleMap : EntityTypeConfiguration<CodeRuleEntity>
    {
        public CodeRuleMap()
        {
            #region 表、主键
            //表
            this.ToTable("BASE_CODERULE");
            //主键
            this.HasKey(t => t.RuleId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
