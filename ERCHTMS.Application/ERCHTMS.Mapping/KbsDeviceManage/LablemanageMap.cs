using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描 述：标签管理
    /// </summary>
    public class LablemanageMap : EntityTypeConfiguration<LablemanageEntity>
    {
        public LablemanageMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LABLEMANAGE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
