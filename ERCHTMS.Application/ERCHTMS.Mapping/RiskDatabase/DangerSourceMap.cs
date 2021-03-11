using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：风险点信息
    /// </summary>
    public class DangerSourceMap : EntityTypeConfiguration<DangerSourceEntity>
    {
        public DangerSourceMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_DANGERSOURCE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}