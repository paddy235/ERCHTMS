using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：车辆基础信息表
    /// </summary>
    public class CarinfoMap : EntityTypeConfiguration<CarinfoEntity>
    {
        public CarinfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CARINFO");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
