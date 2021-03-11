using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// 描 述：标准体系
    /// </summary>
    public class StandardsystemMap : EntityTypeConfiguration<StandardsystemEntity>
    {
        public StandardsystemMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_STANDARDSYSTEM");
            //主键
            this.HasKey(t => t.ID);
            this.Ignore(t=>t.CATEGORYNAME);
            this.Ignore(t => t.CREATEUSERDEPTNAME);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
