using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：历史安全考核主表
    /// </summary>
    public class HistorysafetyassessmentMap : EntityTypeConfiguration<HistorysafetyassessmentEntity>
    {
        public HistorysafetyassessmentMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_HISTORYSAFETYASSESSMENT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
