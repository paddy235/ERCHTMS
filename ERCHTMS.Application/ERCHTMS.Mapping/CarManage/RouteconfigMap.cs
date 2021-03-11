using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：车辆路线配置树
    /// </summary>
    public class RouteconfigMap : EntityTypeConfiguration<RouteconfigEntity>
    {
        public RouteconfigMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_ROUTECONFIG");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
