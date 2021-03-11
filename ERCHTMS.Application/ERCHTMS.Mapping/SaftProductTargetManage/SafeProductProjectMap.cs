using ERCHTMS.Entity.SaftProductTargetManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产目标项目
    /// </summary>
    public class SafeProductProjectMap : EntityTypeConfiguration<SafeProductProjectEntity>
    {
        public SafeProductProjectMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEPRODUCTPROJECT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
