using ERCHTMS.Entity.SafetyWorkSupervise;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyWorkSupervise
{
    /// <summary>
    /// 描 述：安全重点工作督办反馈信息
    /// </summary>
    public class SuperviseconfirmationMap : EntityTypeConfiguration<SuperviseconfirmationEntity>
    {
        public SuperviseconfirmationMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SUPERVISECONFIRMATION");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
