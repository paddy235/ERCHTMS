using ERCHTMS.Entity.WorkPlan;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.WorkPlan
{
    /// <summary>
    /// 描 述：EHS计划申请审批（核）表
    /// </summary>
    public class PlanCheckEntityMap : EntityTypeConfiguration<PlanCheckEntity>
    {
        public PlanCheckEntityMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_PLANCHECK");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
