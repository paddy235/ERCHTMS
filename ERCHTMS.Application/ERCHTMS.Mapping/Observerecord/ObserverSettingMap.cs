using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// 描 述：参与度统计
    /// </summary>
    public class ObserverSettingMap : EntityTypeConfiguration<ObserverSettingEntity>
    {
        public ObserverSettingMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSE_OBSERVERSETTING");
            //主键
            this.HasKey(t => t.SettingId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
    /// <summary>
    /// 描 述：参与度统计部门
    /// </summary>
    public class ObserverSettingItemMap : EntityTypeConfiguration<ObserverSettingItemEntity>
    {
        public ObserverSettingItemMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSE_OBSERVERSETTINGITEM");
            //主键
            this.HasKey(t => t.ItemId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}