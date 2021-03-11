using ERCHTMS.Entity.PowerPlantInside;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PowerPlantInside
{
    /// <summary>
    /// 描 述：单位内部快报
    /// </summary>
    public class PowerplantinsideMap : EntityTypeConfiguration<PowerplantinsideEntity>
    {
        public PowerplantinsideMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_POWERPLANTINSIDE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
