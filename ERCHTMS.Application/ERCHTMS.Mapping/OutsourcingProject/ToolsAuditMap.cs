using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    public class ToolsAuditMap : EntityTypeConfiguration<ToolsAuditEntity>
    {
        public ToolsAuditMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_TOOLSAUDIT");
            //主键
            this.HasKey(t => t.AUDITID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
