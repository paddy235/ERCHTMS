using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查基础信息表
    /// </summary>
    public class AptitudeinvestigateinfoMap : EntityTypeConfiguration<AptitudeinvestigateinfoEntity>
    {
        public AptitudeinvestigateinfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_APTITUDEINVESTIGATEINFO");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
