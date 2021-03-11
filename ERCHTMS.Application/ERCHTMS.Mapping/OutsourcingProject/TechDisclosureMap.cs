using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全技术交底
    /// </summary>
    public class TechDisclosureMap : EntityTypeConfiguration<TechDisclosureEntity>
    {
        public TechDisclosureMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_TECHDISCLOSURE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
