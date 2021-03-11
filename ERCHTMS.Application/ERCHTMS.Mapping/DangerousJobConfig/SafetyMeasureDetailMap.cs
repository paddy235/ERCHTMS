using ERCHTMS.Entity.DangerousJobConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJobConfig
{
    /// <summary>
    /// 描 述：安全措施配置详情
    /// </summary>
    public class SafetyMeasureDetailMap : EntityTypeConfiguration<SafetyMeasureDetailEntity>
    {
        public SafetyMeasureDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("DJ_SAFETYMEASUREDETAIL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
