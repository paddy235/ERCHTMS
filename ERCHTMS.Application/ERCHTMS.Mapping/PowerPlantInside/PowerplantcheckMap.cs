using ERCHTMS.Entity.PowerPlantInside;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件验收
    /// </summary>
    public class PowerplantcheckMap : EntityTypeConfiguration<PowerplantcheckEntity>
    {
        public PowerplantcheckMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_POWERPLANTCHECK");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
