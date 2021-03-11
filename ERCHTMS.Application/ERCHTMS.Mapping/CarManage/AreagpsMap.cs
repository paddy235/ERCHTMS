using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：区域定位管理
    /// </summary>
    public class AreagpsMap : EntityTypeConfiguration<AreagpsEntity>
    {
        public AreagpsMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_AREAGPS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
