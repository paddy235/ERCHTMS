using ERCHTMS.Entity.LllegalStandard;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalStandard
{
    /// <summary>
    /// 描 述：违章标准表
    /// </summary>
    public class LllegalstandardMap : EntityTypeConfiguration<LllegalstandardEntity>
    {
        public LllegalstandardMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALSTANDARD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
