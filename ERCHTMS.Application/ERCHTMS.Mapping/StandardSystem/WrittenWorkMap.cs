using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// 描 述：书面工作程序swp
    /// </summary>
    public class WrittenWorkMap : EntityTypeConfiguration<WrittenWorkEntity>
    {
        public WrittenWorkMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_WRITTENWORK");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
