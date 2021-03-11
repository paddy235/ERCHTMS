using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// 描 述：员工风采
    /// </summary>
    public class StaffMienMap : EntityTypeConfiguration<StaffMienEntity>
    {
        public StaffMienMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_STAFFMIEN");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
