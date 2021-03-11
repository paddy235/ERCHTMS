using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包安全活动
    /// </summary>
    public class SafetyActivityMap : EntityTypeConfiguration<SafetyActivityEntity>
    {
        public SafetyActivityMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_SAFETYACTIVITY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

