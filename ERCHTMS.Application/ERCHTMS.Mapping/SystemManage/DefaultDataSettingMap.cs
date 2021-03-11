using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：默认数据设置表
    /// </summary>
    public class DefaultDataSettingMap : EntityTypeConfiguration<DefaultDataSettingEntity>
    {
        public DefaultDataSettingMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_DEFAULTDATASETTING");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}