using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包单位黑名单表
    /// </summary>
    public class OutprojectblacklistMap : EntityTypeConfiguration<OutprojectblacklistEntity>
    {
        public OutprojectblacklistMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_OUTPROJECTBLACKLIST");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
