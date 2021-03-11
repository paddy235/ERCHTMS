using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描 述：定位点记录表
    /// </summary>
    public class LocationMap : EntityTypeConfiguration<LocationEntity>
    {
        public LocationMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LOCATION");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
