using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包-安全验收
    /// </summary>
    public class SafetyCollectMap : EntityTypeConfiguration<SafetyCollectEntity>
    {
        public SafetyCollectMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_SAFETYCOLLECT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

