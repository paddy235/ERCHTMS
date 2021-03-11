using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：班车上车记录表
    /// </summary>
    public class CarrideMap : EntityTypeConfiguration<CarrideEntity>
    {
        public CarrideMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CARRIDE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
