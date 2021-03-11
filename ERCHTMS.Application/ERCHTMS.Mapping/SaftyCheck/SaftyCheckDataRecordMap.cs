using ERCHTMS.Entity.SaftyCheck;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查记录
    /// </summary>
    public class SaftyCheckDataRecordMap : EntityTypeConfiguration<SaftyCheckDataRecordEntity>
    {
        public SaftyCheckDataRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFTYCHECKDATARECORD");
            //主键
            this.HasKey(t => t.ID);
            this.Ignore(t => t.WzCount);
            this.Ignore(t => t.WtCount);
            this.Ignore(t => t.Count1);
            this.Ignore(t => t.WzCount1);
            this.Ignore(t => t.WtCount1);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
