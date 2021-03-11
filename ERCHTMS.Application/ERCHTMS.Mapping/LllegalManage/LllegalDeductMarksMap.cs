using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：违章档案扣分表
    /// </summary>
    public class LllegalDeductMarksMap : EntityTypeConfiguration<LllegalDeductMarksEntity>
    {
        public LllegalDeductMarksMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALDEDUCTMARKS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}