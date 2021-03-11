using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// 描 述：收藏标准
    /// </summary>
    public class StorestandardMap : EntityTypeConfiguration<StorestandardEntity>
    {
        public StorestandardMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_STORESTANDARD");
            //主键
            this.HasKey(t => t.STOREID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
