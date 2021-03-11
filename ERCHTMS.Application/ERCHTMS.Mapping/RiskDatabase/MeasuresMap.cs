using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：风险管控措施表
    /// </summary>
    public class MeasuresMap : EntityTypeConfiguration<MeasuresEntity>
    {
        public MeasuresMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_MEASURES");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}