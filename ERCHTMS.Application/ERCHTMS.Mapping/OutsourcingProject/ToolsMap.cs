using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：工器具验收
    /// </summary>
    public class ToolsMap : EntityTypeConfiguration<ToolsEntity>
    {
        public ToolsMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_TOOLS");
            //主键
            this.HasKey(t => t.TOOLSID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

