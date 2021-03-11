using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描 述：设备离线记录
    /// </summary>
    public class OfflinedeviceMap : EntityTypeConfiguration<OfflinedeviceEntity>
    {
        public OfflinedeviceMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OFFLINEDEVICE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
