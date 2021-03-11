using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// 描 述：标准分类
    /// </summary>
    public class StcategoryMap : EntityTypeConfiguration<StcategoryEntity>
    {
        public StcategoryMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_STCATEGORY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
