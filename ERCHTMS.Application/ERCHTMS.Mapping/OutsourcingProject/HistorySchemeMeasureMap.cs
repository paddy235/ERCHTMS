using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：历史方案措施管理
    /// </summary>
    public class HistorySchemeMeasureMap : EntityTypeConfiguration<HistorySchemeMeasureEntity>
    {
        public HistorySchemeMeasureMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_HISTORYSCHEMEMEASURE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}