using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：到期时间设置表
    /// </summary>
    public class ExpirationTimeSettingMap : EntityTypeConfiguration<ExpirationTimeSettingEntity>
    {
        public ExpirationTimeSettingMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_EXPIRATIONTIMESETTING");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}