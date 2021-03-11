using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：审查配置
    /// </summary>
    public class InvestigateMap : EntityTypeConfiguration<InvestigateEntity>
    {
        public InvestigateMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_INVESTIGATE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
