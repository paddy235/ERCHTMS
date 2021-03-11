using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    public class SpecificToolsMap : EntityTypeConfiguration<SpecificToolsEntity>
    {
        public SpecificToolsMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_SPECIFICTOOLS");
            //主键
            this.HasKey(t => t.SPECIFICTOOLSID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
