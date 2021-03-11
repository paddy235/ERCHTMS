using ERCHTMS.Entity.HseToolMange;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HseToolManage
{
    public class EvaluateEMap : EntityTypeConfiguration<EvaluateEEntity>
    {
        public EvaluateEMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSE_EVALUATEE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
