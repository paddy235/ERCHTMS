using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：开工申请表
    /// </summary>
    public class StartapplyforMap : EntityTypeConfiguration<StartapplyforEntity>
    {
        public StartapplyforMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_STARTAPPLYFOR");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
