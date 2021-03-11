using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：安全动态
    /// </summary>
    public class SecurityStudyMap : EntityTypeConfiguration<SecurityStudyEntity>
    {
        public SecurityStudyMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SECURITYSTUDY");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
