using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：预知训练作业风险措施
    /// </summary>
    public class TrainmeasuresMap : EntityTypeConfiguration<TrainmeasuresEntity>
    {
        public TrainmeasuresMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TRAINMEASURES");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}