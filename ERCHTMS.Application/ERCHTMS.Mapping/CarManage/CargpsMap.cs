using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：车辆GPS关联表
    /// </summary>
    public class CargpsMap : EntityTypeConfiguration<CargpsEntity>
    {
        public CargpsMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CARGPS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
