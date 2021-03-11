using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：仓库范围管理类
    /// </summary>
    public class WarehousegpsMap : EntityTypeConfiguration<WarehousegpsEntity>
    {
        public WarehousegpsMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WAREHOUSEGPS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
