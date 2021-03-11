using ERCHTMS.Entity.DangerousJobConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJobConfig
{
    /// <summary>
    /// 描 述：危险作业安全措施配置
    /// </summary>
    public class SafetyMeasureConfigMap : EntityTypeConfiguration<SafetyMeasureConfigEntity>
    {
        public SafetyMeasureConfigMap()
        {
            #region 表、主键
            //表
            this.ToTable("DJ_SAFETYMEASURECONFIG");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
