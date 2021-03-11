using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全考核内容表
    /// </summary>
    public class SafestandarditemMap : EntityTypeConfiguration<SafestandarditemEntity>
    {
        public SafestandarditemMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_SAFESTANDARDITEM");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
