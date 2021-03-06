using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：班车预约记录
    /// </summary>
    public class CarreservationMap : EntityTypeConfiguration<CarreservationEntity>
    {
        public CarreservationMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CARRESERVATION");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
