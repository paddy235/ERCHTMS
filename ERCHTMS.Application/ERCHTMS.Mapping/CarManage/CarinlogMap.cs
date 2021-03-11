using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：车辆进出记录表
    /// </summary>
    public class CarinlogMap : EntityTypeConfiguration<CarinlogEntity>
    {
        public CarinlogMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CARINLOG");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
