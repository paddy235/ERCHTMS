using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描 述：区域定位表
    /// </summary>
    public class ArealocationMap : EntityTypeConfiguration<ArealocationEntity>
    {
        public ArealocationMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_AREALOCATION");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
