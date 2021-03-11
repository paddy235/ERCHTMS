using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：dddd
    /// </summary>
    public class ProjecttoolsMap : EntityTypeConfiguration<ProjecttoolsEntity>
    {
        public ProjecttoolsMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_PROJECTTOOLS");
            //主键
            this.HasKey(t => t.PROJECTTOOLSID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
