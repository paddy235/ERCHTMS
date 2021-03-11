using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：危害因素车辆表
    /// </summary>
    public class HazardouscarMap : EntityTypeConfiguration<HazardouscarEntity>
    {
        public HazardouscarMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HAZARDOUSCAR");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
