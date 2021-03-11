using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：外委单位应急预案
    /// </summary>
    public class Drillplan_wwMap : EntityTypeConfiguration<Drillplan_wwEntity>
    {
        public Drillplan_wwMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_DRILLPLAN_WW");
            //主键
            this.HasKey(t => t.ID);
            #endregion

              #region 配置关系
            #endregion
        }
    }
}
