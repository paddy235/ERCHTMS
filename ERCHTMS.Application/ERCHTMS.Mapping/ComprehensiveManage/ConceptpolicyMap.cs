using ERCHTMS.Entity.ComprehensiveManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ComprehensiveManage
{
    /// <summary>
    /// 描 述：理念政策
    /// </summary>
    public class ConceptpolicyMap : EntityTypeConfiguration<ConceptpolicyEntity>
    {
        public ConceptpolicyMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_CONCEPTPOLICY");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
