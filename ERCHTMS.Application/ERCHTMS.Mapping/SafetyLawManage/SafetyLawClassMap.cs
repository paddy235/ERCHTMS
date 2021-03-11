using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// 描 述：安全法律法规分类
    /// </summary>
    public class SafetyLawClassMap : EntityTypeConfiguration<SafetyLawClassEntity>
    {
        public SafetyLawClassMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFETYLAWCLASS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}