using ERCHTMS.Entity.HseToolMange;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HseToolManage
{
    public class SelfEvaluateMap : EntityTypeConfiguration<SelfEvaluateEntity>
    {
        public SelfEvaluateMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSE_SELFEVALUATE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
