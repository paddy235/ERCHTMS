using ERCHTMS.Entity.WorkPlan;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.WorkPlan
{
    /// <summary>
    /// 描 述：EHS计划申请明细表
    /// </summary>
    public class PlanDetailsEntityMap : EntityTypeConfiguration<PlanDetailsEntity>
    {
        public PlanDetailsEntityMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_PLANDETAILS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
