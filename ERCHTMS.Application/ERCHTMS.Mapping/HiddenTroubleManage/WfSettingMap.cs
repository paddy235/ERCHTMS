using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程转向配置实例表
    /// </summary>
    public class WfSettingMap : EntityTypeConfiguration<WfSettingEntity>
    {
        public WfSettingMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WFSETTING");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}