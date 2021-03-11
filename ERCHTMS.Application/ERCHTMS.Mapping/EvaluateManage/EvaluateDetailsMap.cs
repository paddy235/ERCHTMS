using ERCHTMS.Entity.EvaluateManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EvaluateManage
{
    /// <summary>
    /// 描 述：合规性评价明细
    /// </summary>
    public class EvaluateDetailsMap : EntityTypeConfiguration<EvaluateDetailsEntity>
    {
        public EvaluateDetailsMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_EVALUATEDETAILS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
