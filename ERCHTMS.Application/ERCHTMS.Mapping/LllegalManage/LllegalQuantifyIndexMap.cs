using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：违章量化指标表
    /// </summary>
    public class LllegalQuantifyIndexMap : EntityTypeConfiguration<LllegalQuantifyIndexEntity>
    {
        public LllegalQuantifyIndexMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALQUANTIFYINDEX");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}