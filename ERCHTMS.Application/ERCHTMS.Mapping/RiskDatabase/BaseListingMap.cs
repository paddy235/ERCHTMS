using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：作业活动及设备设施清单
    /// </summary>
    public class BaseListingMap : EntityTypeConfiguration<BaseListingEntity>
    {
        public BaseListingMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_BASELISTING");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
