using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// 描 述：相应元素表
    /// </summary>
    public class ElementMap : EntityTypeConfiguration<ElementEntity>
    {
        public ElementMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_ELEMENT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
