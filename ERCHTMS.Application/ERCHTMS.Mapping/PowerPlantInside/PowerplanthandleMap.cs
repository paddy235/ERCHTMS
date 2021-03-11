using ERCHTMS.Entity.PowerPlantInside;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件处理
    /// </summary>
    public class PowerplanthandleMap : EntityTypeConfiguration<PowerplanthandleEntity>
    {
        public PowerplanthandleMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_POWERPLANTHANDLE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
