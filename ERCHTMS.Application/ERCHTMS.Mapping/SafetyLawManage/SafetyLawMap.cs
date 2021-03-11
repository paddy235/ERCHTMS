using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// 描 述：安全生产法律法规
    /// </summary>
    public class SafetyLawMap : EntityTypeConfiguration<SafetyLawEntity>
    {
        public SafetyLawMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFETYLAW");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
