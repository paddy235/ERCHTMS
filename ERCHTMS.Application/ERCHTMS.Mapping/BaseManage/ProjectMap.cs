using ERCHTMS.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.BaseManage
{
    /// <summary>
    /// 描 述：外包工程项目信息
    /// </summary>
    public class ProjectMap : EntityTypeConfiguration<ProjectEntity>
    {
        public ProjectMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_PROJECT");
            //主键
            this.HasKey(t => t.ProjectId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
