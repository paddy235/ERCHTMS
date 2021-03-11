using ERCHTMS.Entity.SaftProductTargetManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产责任书
    /// </summary>
    public class SafeProductDutyBookMap : EntityTypeConfiguration<SafeProductDutyBookEntity>
    {
        public SafeProductDutyBookMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEPRODUCTDUTYBOOK");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
