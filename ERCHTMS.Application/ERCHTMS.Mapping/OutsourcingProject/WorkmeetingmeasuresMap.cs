using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：开收工会预防措施表
    /// </summary>
    public class WorkmeetingmeasuresMap : EntityTypeConfiguration<WorkmeetingmeasuresEntity>
    {
        public WorkmeetingmeasuresMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_WORKMEETINGMEASURES");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}