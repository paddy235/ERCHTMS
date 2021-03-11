using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：安全措施变动申请表
    /// </summary>
    public class SafetychangeMap : EntityTypeConfiguration<SafetychangeEntity>
    {
        public SafetychangeMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFETYCHANGE");
            //主键
            this.HasKey(t => t.ID);
            this.Ignore(t => t.ACCSPECIALTYTYPENAME);
            this.Ignore(t => t.SPECIALTYTYPENAME);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
