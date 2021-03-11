using ERCHTMS.Entity.TwoTickets;
using ERCHTMS.Entity.WorkPlan;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.WorkPlan
{
    /// <summary>
    /// 描 述：EHS计划申请表
    /// </summary>
    public class TwoTicketsMap : EntityTypeConfiguration<TwoTicketsEntity>
    {
        public TwoTicketsMap()
        {
            #region 表、主键
            //表
            this.ToTable("XSS_TWOTICKETS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
