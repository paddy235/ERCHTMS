using ERCHTMS.Entity.HseToolMange;
using System.Data.Entity.ModelConfiguration;


namespace ERCHTMS.Mapping.HseToolManage
{
    public class EvaluateCMap : EntityTypeConfiguration<EvaluateCEntity>
    {
        public EvaluateCMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSE_EVALUATEC");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
