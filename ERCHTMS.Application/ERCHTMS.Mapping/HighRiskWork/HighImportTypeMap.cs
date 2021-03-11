using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：显示
    /// </summary>
    public class HighImportTypeMap : EntityTypeConfiguration<HighImportTypeEntity>
    {
        public HighImportTypeMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIGHIMPORTTYPE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
