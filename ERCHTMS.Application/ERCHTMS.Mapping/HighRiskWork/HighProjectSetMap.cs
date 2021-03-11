using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险中项目配置
    /// </summary>
    public class HighProjectSetMap : EntityTypeConfiguration<HighProjectSetEntity>
    {
        public HighProjectSetMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIGHPROJECTSET");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
