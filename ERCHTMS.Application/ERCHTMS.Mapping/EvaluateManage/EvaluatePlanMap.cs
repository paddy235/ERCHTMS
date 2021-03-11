using ERCHTMS.Entity.EvaluateManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EvaluateManage
{
    /// <summary>
    /// 描 述：合规性评价计划
    /// </summary>
    public class EvaluatePlanMap : EntityTypeConfiguration<EvaluatePlanEntity>
    {
        public EvaluatePlanMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_EVALUATEPLAN");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
