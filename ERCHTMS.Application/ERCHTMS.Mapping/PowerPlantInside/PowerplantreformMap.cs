using ERCHTMS.Entity.PowerPlantInside;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件处理整改
    /// </summary>
    public class PowerplantreformMap : EntityTypeConfiguration<PowerplantreformEntity>
    {
        public PowerplantreformMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_POWERPLANTREFORM");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
