using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：门禁设备管理
    /// </summary>
    public class HikdeviceMap : EntityTypeConfiguration<HikdeviceEntity>
    {
        public HikdeviceMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIKDEVICE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
