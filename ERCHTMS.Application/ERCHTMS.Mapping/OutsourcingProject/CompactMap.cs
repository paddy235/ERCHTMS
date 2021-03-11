using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：合同
    /// </summary>
    public class CompactMap : EntityTypeConfiguration<CompactEntity>
    {
        public CompactMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_COMPACT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
