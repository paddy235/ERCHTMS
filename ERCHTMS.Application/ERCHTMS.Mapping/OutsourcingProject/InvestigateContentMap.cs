using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：审查配置内容
    /// </summary>
    public class InvestigateContentMap : EntityTypeConfiguration<InvestigateContentEntity>
    {
        public InvestigateContentMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_INVESTIGATECONTENT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
