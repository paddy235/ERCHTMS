using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：审查记录表
    /// </summary>
    public class InvestigateRecordMap : EntityTypeConfiguration<InvestigateRecordEntity>
    {
        public InvestigateRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_INVESTIGATERECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}