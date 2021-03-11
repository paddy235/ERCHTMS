using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：隐患标准库
    /// </summary>
    public class HtStandardItemMap : EntityTypeConfiguration<HtStandardItemEntity>
    {
        public HtStandardItemMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HTSTANDARDITEM");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}