using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：海康门禁间设备管理
    /// </summary>
    public class HikaccessMap : EntityTypeConfiguration<HikaccessEntity>
    {
        public HikaccessMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIKACCESS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
