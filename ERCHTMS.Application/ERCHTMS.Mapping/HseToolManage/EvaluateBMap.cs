using ERCHTMS.Entity.HseToolMange;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HseToolManage
{
    public class EvaluateBMap : EntityTypeConfiguration<EvaluateBEntity>
    {
        public EvaluateBMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSE_EVALUATEB");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
