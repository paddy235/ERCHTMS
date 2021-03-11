using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配人员
    /// </summary>
    public class StaffInfoMap : EntityTypeConfiguration<StaffInfoEntity>
    {
        public StaffInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_STAFFINFO");
            //主键
            this.HasKey(t => t.Id);
            this.Ignore(t => t.SpecialtyTypeName);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
