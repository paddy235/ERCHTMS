using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：历史
    /// </summary>
    public class HistorydailyexamineMap : EntityTypeConfiguration<HistorydailyexamineEntity>
    {
        public HistorydailyexamineMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_HISTORYDAILYEXAMINE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
