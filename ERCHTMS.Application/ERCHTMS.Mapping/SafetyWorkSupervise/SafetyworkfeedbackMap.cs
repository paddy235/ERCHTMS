using ERCHTMS.Entity.SafetyWorkSupervise;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyWorkSupervise
{
    /// <summary>
    /// 描 述：安全重点工作督办反馈信息
    /// </summary>
    public class SafetyworkfeedbackMap : EntityTypeConfiguration<SafetyworkfeedbackEntity>
    {
        public SafetyworkfeedbackMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFETYWORKFEEDBACK");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
