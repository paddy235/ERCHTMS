using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描 述：作业现场安全管控
    /// </summary>
    public class SafeworkcontrolMap : EntityTypeConfiguration<SafeworkcontrolEntity>
    {
        public SafeworkcontrolMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEWORKCONTROL");
            //主键
            this.HasKey(t => t.ID);
            #endregion
             

            #region 配置关系
            #endregion
        }
    }
}
