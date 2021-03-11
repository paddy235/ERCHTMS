using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：天气预警
    /// </summary>
    public class WeatherMap : EntityTypeConfiguration<WeatherEntity>
    {
        public WeatherMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WEATHER");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
