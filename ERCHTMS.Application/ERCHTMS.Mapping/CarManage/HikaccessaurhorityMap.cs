using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：门禁点权限表
    /// </summary>
    public class HikaccessaurhorityMap : EntityTypeConfiguration<HikaccessaurhorityEntity>
    {
        public HikaccessaurhorityMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIKACCESSAURHORITY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
