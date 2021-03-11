using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查人员表
    /// </summary>
    public class AptitudeinvestigatepeopleMap : EntityTypeConfiguration<AptitudeinvestigatepeopleEntity>
    {
        public AptitudeinvestigatepeopleMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_APTITUDEINVESTIGATEPEOPLE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
