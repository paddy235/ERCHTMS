using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描 述：标签上下线日志
    /// </summary>
    public class LableonlinelogMap : EntityTypeConfiguration<LableonlinelogEntity>
    {
        public LableonlinelogMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LABLEONLINELOG");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
