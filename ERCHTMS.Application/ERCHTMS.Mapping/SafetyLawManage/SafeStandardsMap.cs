using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// 描 述：安全操作规程
    /// </summary>
    public class SafeStandardsMap : EntityTypeConfiguration<SafeStandardsEntity>
    {
        public SafeStandardsMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFESTANDARDS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
