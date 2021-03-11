using ERCHTMS.Entity.SaftyCheck;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查表
    /// </summary>
    public class SaftyCheckDataMap : EntityTypeConfiguration<SaftyCheckDataEntity>
    {
        public SaftyCheckDataMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFTYCHECKDATA");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
