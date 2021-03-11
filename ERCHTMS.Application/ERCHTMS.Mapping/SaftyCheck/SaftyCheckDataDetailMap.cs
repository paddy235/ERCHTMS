using ERCHTMS.Entity.SaftyCheck;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查表详情
    /// </summary>
    public class SaftyCheckDataDetailMap : EntityTypeConfiguration<SaftyCheckDataDetailEntity>
    {
        public SaftyCheckDataDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFTYCHECKDATADETAILED");
            //主键
            this.HasKey(t => t.ID);
            this.Ignore(t => t.Content);
            //this.Ignore(t => t.IsSure);
            //this.Ignore(t => t.Remark);
            this.Ignore(t => t.WzCount);
            this.Ignore(t => t.WtCount);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
