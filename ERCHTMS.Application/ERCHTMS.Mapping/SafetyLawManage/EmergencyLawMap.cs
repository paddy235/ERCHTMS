using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// 描 述：应急预案范本
    /// </summary>
    public class EmergencyLawMap : EntityTypeConfiguration<EmergencyLawEntity>
    {
        public EmergencyLawMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_EMERGENCYLAW");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
