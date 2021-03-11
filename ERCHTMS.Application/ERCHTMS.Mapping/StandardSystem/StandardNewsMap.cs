using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// 描 述：标准化新闻
    /// </summary>
    public class StandardNewsMap : EntityTypeConfiguration<StandardNewsEntity>
    {
        public StandardNewsMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_STANDARDNEWS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
