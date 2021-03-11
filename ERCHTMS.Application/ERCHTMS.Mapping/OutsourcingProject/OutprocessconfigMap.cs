using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包流程配置表
    /// </summary>
    public class OutprocessconfigMap : EntityTypeConfiguration<OutprocessconfigEntity>
    {
        public OutprocessconfigMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_OUTPROCESSCONFIG");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
