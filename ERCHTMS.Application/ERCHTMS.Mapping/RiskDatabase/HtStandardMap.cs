using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：隐患标准库
    /// </summary>
    public class HtStandardMap : EntityTypeConfiguration<HtStandardEntity>
    {
        public HtStandardMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HTSTANDARD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}