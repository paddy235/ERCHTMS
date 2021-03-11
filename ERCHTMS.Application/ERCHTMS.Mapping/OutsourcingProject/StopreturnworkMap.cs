using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：停复工管理表
    /// </summary>
    public class StopreturnworkMap : EntityTypeConfiguration<StopreturnworkEntity>
    {
        public StopreturnworkMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_STOPRETURNWORK");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
