using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查审核表
    /// </summary>
    public class AptitudeinvestigateauditMap : EntityTypeConfiguration<AptitudeinvestigateauditEntity>
    {
        public AptitudeinvestigateauditMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_APTITUDEINVESTIGATEAUDIT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
