using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：审查记录明细表
    /// </summary>
    public class InvestigateDtRecordMap : EntityTypeConfiguration<InvestigateDtRecordEntity>
    {
        public InvestigateDtRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_INVESTIGATEDTRECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}